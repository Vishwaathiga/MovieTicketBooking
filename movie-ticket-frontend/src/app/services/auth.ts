
import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  private apiUrl = 'https://localhost:7162/api/auth';

  constructor(private http: HttpClient) { }

  register(user: any): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/register`,
      user
    );
  }

  login(user: any): Observable<any> {
    return this.http.post<any>(
      `${this.apiUrl}/login`,
      user
    );
  }
}
