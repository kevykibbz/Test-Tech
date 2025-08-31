import { Component, input, OnChanges, OnDestroy, OnInit, SimpleChanges, signal, computed, effect } from '@angular/core';
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
  // Input signals
  entityId = input<string>();

  // State signals
  hasMoreEntries = signal(false);
  loading = signal(false);
  log = signal<LogEntry[]>([]);

  // Computed signals
  hasLogs = computed(() => this.log().length > 0);
  isLoadingOrHasLogs = computed(() => this.loading() || this.hasLogs());

  private destroy$ = new AsyncSubject<any>();
  private firstLogEntryId?: number;
  private lastLogEntryId?: number;
  private logChangeListener?: Subscription;

  constructor(
    private logService: LogService,
    private socketService: SocketService,
  ) {
    // Effect to handle entityId changes
    effect(() => {
      const currentEntityId = this.entityId();
      if (currentEntityId) {
        this.handleEntityIdChange(currentEntityId);
      }
    });
  }

  private handleEntityIdChange(newEntityId: string): void {
    this.log.set([]);
    this.hasMoreEntries.set(false);
    this.firstLogEntryId = undefined;
    this.lastLogEntryId = undefined;
    this.loadNewLogItems();
  }

  ngOnChanges(changes: SimpleChanges): void {
    if (!changes['entityId'].isFirstChange()) {
      const currentEntityId = this.entityId();
      if (currentEntityId) {
        this.stopListeningForLogChanges(changes['entityId'].previousValue);
        this.startListeningForLogChanges();
        this.log.set([]);
        this.hasMoreEntries.set(false);
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

    const entityId = this.entityId();
    if (entityId) {
      params.entity_id = entityId;
    }
    this.loading.set(true);
    this.logService.getLog(params)
      .pipe(
        finalize(() => this.loading.set(false)),
        takeUntil(this.destroy$),
      )
      .subscribe(logEntries => {
        this.hasMoreEntries.set(!!params.take && !(logEntries.length < params.take));
        const currentLog = this.log();
        this.log.set([...currentLog, ...logEntries]);

        const newLog = this.log();
        this.firstLogEntryId = (newLog[newLog.length - 1] || { id: null }).id;
        this.lastLogEntryId = (newLog[0] || { id: null }).id;
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

    const entityId = this.entityId();
    if (entityId) {
      params.entity_id = entityId;
    }

    this.loading.set(true);
    this.logService.getLog(params)
      .pipe(
        finalize(() => this.loading.set(false)),
        takeUntil(this.destroy$),
      )
      .subscribe(logEntries => {
        const currentLog = this.log();
        if (params.after) {
          this.log.set([...logEntries, ...currentLog]);
        } else {
          this.hasMoreEntries.set(!!params.take && !(logEntries.length < params.take));
          this.log.set([...currentLog, ...logEntries]);
        }

        const newLog = this.log();
        this.firstLogEntryId = (newLog[newLog.length - 1] || { id: null }).id;
        this.lastLogEntryId = (newLog[0] || { id: null }).id;
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
