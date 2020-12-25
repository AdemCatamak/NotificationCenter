import { HttpClient, HttpHeaders } from '@angular/common/http';
import { EventEmitter, Injectable, Output } from '@angular/core';
import { Observable } from 'rxjs';
import { AppNotification } from './app-notification';

@Injectable()
export class AppNotificationService {
  @Output()
  NewNotificationArrived: EventEmitter<AppNotification> = new EventEmitter<AppNotification>();

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
}
