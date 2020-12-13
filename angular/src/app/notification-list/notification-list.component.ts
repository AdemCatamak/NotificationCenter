import { Component, Input, OnInit } from '@angular/core';
import { AppNotification } from './app-notification';

@Component({
  selector: 'app-notification-list',
  templateUrl: './notification-list.component.html',
  styleUrls: ['./notification-list.component.css'],
})
export class NotificationListComponent implements OnInit {

  @Input() notifications: AppNotification[] = [];

  ngOnInit(): void {
    if (this.notifications == null || this.notifications == undefined) {
      this.notifications = new Array<AppNotification>();
    }
  }
}
