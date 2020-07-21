import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';

import { ChartsModule } from 'ng2-charts';

import { AppComponent } from './app.component';
import { FetchDataComponent } from './fetch-data/fetch-data.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { CounterComponent } from './components/counter/counter.component';
import { UploadComponent } from './fetch-data/upload/upload.component';
import { HomeComponent } from './components/home/home.component';
import { SignalrComponent } from './components/signalr/signalr.component';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    UploadComponent,
    SignalrComponent,
    CounterComponent,
    FetchDataComponent
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    ChartsModule,
    FormsModule,
    RouterModule.forRoot([
      { path: '', component: HomeComponent, pathMatch: 'full' },
      { path: 'counter', component: CounterComponent },
      { path: 'fetch-data', component: FetchDataComponent },
      { path: 'signalr', component: SignalrComponent },
    ])
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
