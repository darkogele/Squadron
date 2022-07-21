import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { environment } from 'src/environments/environment';
import { ChartItemDto } from '../models/chartItemDto';

@Injectable({
  providedIn: 'root'
})
export class UploadService {
  baseUrl = environment.baseUrl;

  constructor(private http: HttpClient) { }


  public upload(formData: FormData) {
    return this.http.post(`${this.baseUrl}upload`, formData, {
      reportProgress: true,
      observe: 'events',
    });
  }

  public getLatestFile() {
    return this.http.get<ChartItemDto[]>(`${this.baseUrl}files/latest-File`);
  }

  public getAllFiles() {
    return this.http.get<string[]>(`${this.baseUrl}files/Grid-View`);
  }
}
