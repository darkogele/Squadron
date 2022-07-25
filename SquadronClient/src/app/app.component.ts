import { Component } from '@angular/core';
import { ChartConfiguration } from 'chart.js';
import { ChartItemDto } from './models/chartItemDto';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  title = 'SquadronClient';

  public barChartLegend = false;
  public barChartPlugins = [];

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true,
  };

  public data: ChartItemDto[] = [
    { label: 'Skopje', number: 40, color: 'red' },
    { label: 'Ohrid', number: 28, color: 'green' },
    { label: 'Tetovo', number: 13, color: 'blue' },
    { label: 'Gevgelija', number: 41, color: 'red' },
  ];

  public chartData: ChartConfiguration<'bar'>['data'] = {
    labels: this.data.map(x => x.label),
    datasets: [{
      data: this.data.map(x => x.number),
      backgroundColor: this.data.map(x => x.color),
    }]
  }

}
