import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { IUserLogin } from '../interfaces/iuser-login';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(private http: HttpClient) { }

  login = (credentials: IUserLogin) => {
    return this.http.post("https://localhost:44322/api/Account/Login", credentials)
  }
  
}
