import { Injectable } from '@angular/core';

import * as signalR from '@aspnet/signalr';

export interface ChartModel {
  data: [];
  label: string;
}

@Injectable({
  providedIn: 'root'
})
export class SignalrService {
  public data: ChartModel[];
  private hubConnection: signalR.HubConnection;
  public bradcastedData: ChartModel[];
  constructor() { }
  public startConnection = () => {
    this.hubConnection = new signalR.HubConnectionBuilder()
      .withUrl('http://localhost:3000/chart')
      .build();
    this.hubConnection
      .start()
      .then(() => console.log('Connection started'))
      .catch(err => console.log('Error while starting connection: ' + err));
  }
  public addTransferChartDataListener = () => {
    this.hubConnection.on('transferchartdata', (data) => {
      this.data = data;
      console.log(data);
    });
  }
  public broadcastChartData(): void {
    const data = this.data.map(m => {
      const temp = {
        data: m.data,
        label: m.label
      };
      return temp;
    });
    this.hubConnection.invoke('broadcastchartdata', data)
      //  .then(resp => console.log(resp))
      .catch(err => console.error(err));
  }
  public addBroadcastChartDataListener = () => {
    this.hubConnection.on('broadcastchartdata', (data) => {
      this.bradcastedData = data;
      console.log(data);
    });
  }
}
