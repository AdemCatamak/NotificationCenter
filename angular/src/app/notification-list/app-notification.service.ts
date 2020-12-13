import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppNotification } from './app-notification';

@Injectable()
export class AppNotificationService {
  constructor(private httpClient: HttpClient) {}

  public GetNotifications(userId: string): Observable<AppNotification[]> {
    return this.httpClient.get<AppNotification[]>(
      'http://localhost:5000/users/' + userId + '/notifications?take=5'
    );
  }
}
