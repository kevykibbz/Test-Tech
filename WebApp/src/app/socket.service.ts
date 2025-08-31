import { Injectable } from '@angular/core';
import { combineLatest, ReplaySubject, Subject } from 'rxjs';
import { distinctUntilChanged, filter, map, tap } from 'rxjs/operators';

import { SocketState } from './socket-state';
import { environment } from '../environments/environment';

@Injectable({
  providedIn: 'root',
})
export class SocketService {
  message$ = new Subject<any>();
  socketState!: SocketState;
  socketState$ = new ReplaySubject<SocketState>(1);

  private connection!: WebSocket;
  private outgoingMessage$ = new Subject<any>();
  private retryCount = 0;

  constructor() {
    this.setSocketState(SocketState.Closed);
    this.initMessageHandler();
    this.initConnection();
  }

  sendMessage(data: any): void {
    this.outgoingMessage$.next(data);
  }

  private attachConnectionListeners(connection: WebSocket): void {
    connection.addEventListener('open', this.onSocketOpen);
    connection.addEventListener('close', this.onSocketClose);
    connection.addEventListener('message', this.onSocketMessage);
  }

  private initConnection(): void {
    this.setSocketState(SocketState.Opening);

    this.connection = new WebSocket(environment.websocketServerBase);
    this.attachConnectionListeners(this.connection);
  }

  private initMessageHandler(): void {
    const messageQueue: any[] = [];

    combineLatest([
      this.outgoingMessage$
        .pipe(
          tap(message => messageQueue.push(message)),
        ),
      this.socketState$
        .pipe(
          map(s => s === SocketState.Opened),
          distinctUntilChanged(),
        ),
    ])
      .pipe(
        filter(([, isReady]) => isReady),
        map(() => {
          const messages = messageQueue.slice();
          messageQueue.length = 0;
          return messages;
        }),
      )
      .subscribe(messages => {
        for (const m of messages) {
          this.connection.send(JSON.stringify(m));
        }
      });
  }

  private onSocketClose = (): void => {
    this.removeAllSocketListeners(this.connection);
    this.setSocketState(SocketState.Closed);

    setTimeout(() => this.initConnection(), 1e3 * this.retryCount);
  }

  private onSocketMessage = (e: MessageEvent): void => {
    try {
      this.message$.next(JSON.parse(e.data));
    } catch (e) {
      console.error('Error processing socket message', e);
    }
  }

  private onSocketOpen = (): void => {
    this.setSocketState(SocketState.Opened);
  }

  private removeAllSocketListeners(connection: WebSocket): void {
    if (!connection) {
      return;
    }

    connection.removeEventListener('close', this.onSocketClose);
    connection.removeEventListener('message', this.onSocketMessage);
    connection.removeEventListener('open', this.onSocketOpen);
  }

  private setSocketState(state: SocketState): void {
    if (state === SocketState.Opened) {
      this.retryCount = 0;
    } else if (state === SocketState.Closed) {
      this.retryCount++;
    }

    this.socketState = state;
    this.socketState$.next(this.socketState);
  }
}
