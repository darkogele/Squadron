import { Component, OnInit } from '@angular/core';
import { ChartConfiguration } from 'chart.js';
import { ChartItemDto } from 'src/app/models/chartItemDto';
import { UploadService } from 'src/app/services/upload.service';

@Component({
  selector: 'app-graphic',
  templateUrl: './graphic.component.html',
  styleUrls: ['./graphic.component.scss']
})
export class GraficComponent implements OnInit {
  uploadedFilalData: ChartItemDto[] = [];
  listOfFiles: string[] = [];

  constructor(private UploadService: UploadService) { }

  ngOnInit(): void {
    this.UploadService.getAllFiles().subscribe(res => {
      this.listOfFiles = res;
    })

    this.UploadService.getLatestFile().subscribe(data => {
      this.uploadedFilalData = data;
      this.FillChart();
    });
  }

  public barChartOptions: ChartConfiguration<'bar'>['options'] = {
    responsive: true
  };
  public barChartPlugins = [];
  public barChartLegend = false;


  public chartData: ChartConfiguration<'bar'>['data'] = {
    labels: [],
    datasets: [{ data: [], }]
  }

  FillChart() {
    this.chartData = {
      labels: this.uploadedFilalData.map(x => x.label),
      datasets: [{
        data: this.uploadedFilalData.map(x => x.number),
        backgroundColor: this.uploadedFilalData.map(x => x.color),
      }]
    }
  }

  public tiles = [
    { text: 'One', cols: 1, rows: 1, color: 'lightblue' },
    { text: 'Two', cols: 1, rows: 1, color: 'lightgreen' },
    { text: 'Three', cols: 1, rows: 1, color: 'lightpink' },
  ];
}
