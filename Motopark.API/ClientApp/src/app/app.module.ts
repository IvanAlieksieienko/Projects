import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { NgxSpinnerModule } from "ngx-spinner";

import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { AboutComponent } from './components/about/about.component';
import { BasketComponent } from './components/basket/basket.component';
import { ContactComponent } from './components/contact/contact.component';
import { OrderComponent } from './components/order/order.component';
import { SideBarComponent } from './components/side-bar/side-bar.component';
import { SharedService } from './services/shared.service';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import {NgbModule} from '@ng-bootstrap/ng-bootstrap';
import {NgxPaginationModule} from 'ngx-pagination';



@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    AboutComponent,
    BasketComponent,
    ContactComponent,
    OrderComponent,
    SideBarComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    AppRoutingModule,
    FontAwesomeModule,
    NgxSpinnerModule,
    BrowserAnimationsModule,
    NgbModule,
    NgxPaginationModule
  ],
  providers: [SharedService],
  bootstrap: [AppComponent]
})
export class AppModule { }
