import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { AppNotification } from './app-notification';
import * as signalR from '@aspnet/signalr';
import { AppNotificationSeenStatusChangedInfo } from './app-notification-seen-status-changed-response';

@Injectable()
export class AppNotificationService {
  @Output()
  NotificationReceived: EventEmitter<AppNotification> = new EventEmitter<AppNotification>();
  NotificationSeenStatusChanged: EventEmitter<AppNotificationSeenStatusChangedInfo> = new EventEmitter<AppNotificationSeenStatusChangedInfo>();

  private hubConnection: signalR.HubConnection | null = null;

  constructor(private httpClient: HttpClient) {}

  public GetNotifications(
    username: string,
    accessToken: string
  ): Observable<AppNotification[]> {
    var reqHeader = new HttpHeaders({
      'Content-Type': 'application/json',
      Authorization: 'Bearer ' + accessToken,
    });

    return this.httpClient.get<AppNotification[]>(
      'http://localhost:5000/users/' + username + '/notifications?take=5',
      { headers: reqHeader }
    );
  }

  public ObserveNotifications(accessToken: string) {
    console.log('ObserveNotifications' + accessToken);

    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:5000/auth-hubs/notification-hub', {
        accessTokenFactory: () => accessToken,
      })
      .build();

    this.hubConnection
      .start()
      .then(() => {
        console.log('Hub connection started');
        this.registerOnServerEvents();
      })
      .catch((err) => {
        console.log(err);
      });
  }

  private registerOnServerEvents(): void {
    if (this.hubConnection == null) {
      return;
    }

    this.hubConnection.on('NotificationReceived', (data: AppNotification) => {
      this.NotificationReceived.emit(data);
    });

    this.hubConnection.on(
      'NotificationSeenStatusChanged',
      (data: AppNotificationSeenStatusChangedInfo) => {
        this.NotificationSeenStatusChanged.emit(data);
      }
    );
  }
}
