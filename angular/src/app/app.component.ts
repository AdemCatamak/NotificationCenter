import { Component } from '@angular/core';
import { AppNotification } from './notification-list/app-notification';
import { AppNotificationService } from './notification-list/app-notification.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css'],
})
export class AppComponent {
  title = 'angular';

  userId = '';
  notificationCollection: AppNotification[] = [];

  constructor(private notificationService: AppNotificationService) {
    this.notificationCollection = new Array<AppNotification>();
  }

  ChangeUserId() {
    console.log('userId :' + this.userId);
    if (this.userId == null || this.userId == undefined) {
      this.userId = '';
    }

    if (this.userId != '') {
      this.notificationService.GetNotifications(this.userId).subscribe(
        (res) => {
          this.notificationCollection = res;
        },
        (er) => {
          if (er.status == 404) {
            this.notificationCollection = [];
          } else {
            console.error(er);
          }
        }
      );
    }
  }
}
