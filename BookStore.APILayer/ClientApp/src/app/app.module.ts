/* System Components */
import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule } from '@angular/common/http';

/* My components */
import { AppComponent } from './app.component';
import { AppRoutingModule } from './app-routing.module';
import { HeadBarComponent } from './modules/headBar/headBar.component';
import { SideBarComponent } from './modules/sideBar/sideBar.component';
import { SharedService } from './services/shared.service';

@NgModule({

  declarations: [
    AppComponent,
    HeadBarComponent,
    SideBarComponent
  ],

  imports: [
    BrowserModule,
    HttpClientModule,
    FormsModule,
    AppRoutingModule
  ],

  providers: [SharedService],

  bootstrap: [AppComponent]

})
export class AppModule { }
