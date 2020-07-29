import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { SignalrService } from 'src/app/services/signalr.service';

@Component({
  selector: 'app-signalr',
  templateUrl: './signalr.component.html',
  styleUrls: ['./signalr.component.scss']
})
export class SignalrComponent implements OnInit {
  public chartOptions: any = {
    scaleShowVerticalLines: true,
    responsive: true,
    scales: {
      yAxes: [{
        ticks: {
          beginAtZero: true
        }
      }]
    }
  };
  public chartLabels: string[] = ['Real time data for the chart'];
  public chartType = 'bar';
  public chartLegend = true;
  public colors: any[] = [{ backgroundColor: '#5491DA' }, { backgroundColor: '#E74C3C' }, { backgroundColor: '#82E0AA' }, { backgroundColor: '#E5E7E9' }];
  constructor(
    public signalRService: SignalrService,
    private http: HttpClient
  ) { }
  ngOnInit() {
    this.signalRService.startConnection();
    this.signalRService.addTransferChartDataListener();
    this.signalRService.addBroadcastChartDataListener();
    this.startHttpRequest();
  }
  private startHttpRequest = () => {
    this.http.get('http://localhost:3000/api/chart')
      .subscribe(res => {
        console.log(res);
      });
  }
  public chartClicked = (event) => {
    // console.log(event);
    this.signalRService.broadcastChartData();
  }

}
