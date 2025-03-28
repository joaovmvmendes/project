import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { LoginRequest } from '../models/login-request.model';

@Injectable({
  providedIn: 'root',
})
export class AuthService {
  private apiUrl = `${environment.apiBaseUrl}/auth`;

  constructor(private http: HttpClient) {}

  login(loginRequest: LoginRequest) {
    return this.http.post<{ token: string }>(
      `${this.apiUrl}/login`,
      loginRequest
    );
  }

  // Adicionando o método storeToken para armazenar o token no localStorage
  storeToken(token: string): void {
    localStorage.setItem('token', token);
  }
}
