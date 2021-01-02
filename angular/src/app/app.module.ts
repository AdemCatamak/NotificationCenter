import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { HttpClientModule } from '@angular/common/http';
import { FormsModule } from '@angular/forms';

import { NotificationListComponent } from './notification-list/notification-list.component';
import { AppNotificationService } from './notification-list/app-notification.service';
import { AppAccessTokenService } from './app-access-token/app-access-token.service';
import { GlobalVariables } from './global-variables';

@NgModule({
  declarations: [AppComponent, NotificationListComponent],
  imports: [BrowserModule, HttpClientModule, FormsModule, AppRoutingModule],
  providers: [AppNotificationService, AppAccessTokenService, GlobalVariables],
  bootstrap: [AppComponent],
})
export class AppModule {}
