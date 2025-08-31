import { Component, Input, OnChanges, OnDestroy, OnInit, SimpleChanges } from '@angular/core';
import { AsyncSubject, Subscription, finalize, takeUntil, map, distinctUntilChanged, filter } from 'rxjs';
import { SocketState } from '../socket-state';
import { SocketService } from '../socket.service';
import { DEFAULT_LOG_PAGE_SIZE, LogService } from '../log.service';
import { LogEntry } from '../log-entry';
import { LogQueryParameters } from '../log-query-parameters';

@Component({
  selector: 'app-log-explorer',
  imports: [],
  templateUrl: './log-explorer.component.html',
  styleUrl: './log-explorer.component.scss'
})
export class LogExplorerComponent implements OnChanges, OnInit, OnDestroy {
  @Input() entityId?: string;

  hasMoreEntries = false;
  loading = false;
  log: LogEntry[] = [];

  private destroy$ = new AsyncSubject<any>();
  private firstLogEntryId?: number;
  private lastLogEntryId?: number;
  private logChangeListener?: Subscription;

  constructor(
    private logService: LogService,
    private socketService: SocketService,
  ) {
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes['entityId'].isFirstChange()) {
      if (this.entityId) {
        this.stopListeningForLogChanges(changes['entityId'].previousValue);
        this.startListeningForLogChanges();
        this.log = [];
        this.hasMoreEntries = false;
        this.firstLogEntryId = undefined;
        this.lastLogEntryId = undefined;
        this.loadNewLogItems();
      }
    }
  }

  ngOnInit(): void {
    this.startListeningForLogChanges();
    this.loadNewLogItems();
  }

  ngOnDestroy(): void {
    this.stopListeningForLogChanges();
    this.destroy$.next(true);
    this.destroy$.complete();
  }

  loadMore(): void {
    const params: LogQueryParameters = {
      before: this.firstLogEntryId,
      take: DEFAULT_LOG_PAGE_SIZE,
    };

    if (this.entityId) {
      params.entity_id = this.entityId;
    }
    this.loading = true;
    this.logService.getLog(params)
      .pipe(
        finalize(() => this.loading = false),
        takeUntil(this.destroy$),
      )
      .subscribe(logEntries => {
        this.hasMoreEntries = !!params.take && !(logEntries.length < params.take);
        this.log = [...this.log, ...logEntries];

        this.firstLogEntryId = (this.log[this.log.length - 1] || { id: null }).id;
        this.lastLogEntryId = (this.log[0] || { id: null }).id;
      });
  }

  private loadNewLogItems(): void {
    let params: LogQueryParameters = {
      take: DEFAULT_LOG_PAGE_SIZE,
    };
    if (this.lastLogEntryId) {
      params = {
        after: this.lastLogEntryId,
        take: 1000,
      };
    }

    if (this.entityId) {
      params.entity_id = this.entityId;
    }

    this.loading = true;
    this.logService.getLog(params)
      .pipe(
        finalize(() => this.loading = false),
        takeUntil(this.destroy$),
      )
      .subscribe(logEntries => {
        if (params.after) {
          this.log = [...logEntries, ...this.log];
        } else {
          this.hasMoreEntries = !!params.take && !(logEntries.length < params.take);
          this.log = [...this.log, ...logEntries];
        }

        this.firstLogEntryId = (this.log[this.log.length - 1] || { id: null }).id;
        this.lastLogEntryId = (this.log[0] || { id: null }).id;
      });
  }

  private startListeningForLogChanges(): void {
    this.socketService.socketState$
      .pipe(
        map(s => s === SocketState.Opened),
        distinctUntilChanged(),
        filter(opened => opened),
        takeUntil(this.destroy$),
      )
      .subscribe(() => {
        const message = this.entityId ?
          {
            type: 'action',
            parameters: {
              action: 'join-channel',
              parameters: {
                channel_type: 'log.entity.updates',
                entity_id: this.entityId,
              },
            },
          }
          : {
            type: 'action',
            parameters: {
              action: 'join-channel',
              parameters: {
                channel_type: 'log.global.updates',
              },
            },
          };
        this.socketService.sendMessage(message);
      });

    this.logChangeListener = this.socketService.message$
      .pipe(
        filter((m: any) => m && (m.context === 'log.global' || m.context === 'log.entity')),
      )
      .subscribe(() => this.loadNewLogItems());
  }

  // tslint:disable-next-line:variable-name
  private stopListeningForLogChanges(_entityId?: string): void {
    if (this.logChangeListener) {
      this.logChangeListener.unsubscribe();
      this.logChangeListener = undefined;
    }

    const entityId = _entityId || this.entityId;
    const message = entityId ?
      {
        type: 'action',
        parameters: {
          action: 'leave-channel',
          parameters: {
            channel_type: 'log.entity.updates',
            entity_id: entityId,
          },
        },
      }
      : {
        type: 'action',
        parameters: {
          action: 'leave-channel',
          parameters: {
            channel_type: 'log.global.updates',
          },
        },
      };

    this.socketService.sendMessage(message);
  }
}
