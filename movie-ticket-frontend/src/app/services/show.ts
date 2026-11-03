
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class ShowService {

  private apiUrl = 'https://localhost:7162/api/shows';

  constructor(private http: HttpClient) { }

  getShows(): Observable<any> {
    return this.http.get<any>(this.apiUrl);
  }
}
