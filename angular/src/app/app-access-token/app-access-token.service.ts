import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { AppAccessToken } from './app-access-token';

@Injectable()
export class AppAccessTokenService {
  constructor(private httpClient: HttpClient) {}

  public GetAccessToken(username: string): Observable<AppAccessToken> {
    var request = { Username: username };

    return this.httpClient.post<AppAccessToken>(
      'http://localhost:5000/tokens/',
      request
    );
  }
}
