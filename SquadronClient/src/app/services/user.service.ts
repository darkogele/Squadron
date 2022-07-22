import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { Login, User } from '../models/user';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  baseUrl = environment.baseUrl;

  private currentUserSource = new BehaviorSubject<User | null>(null);
  currentUser$ = this.currentUserSource.asObservable();

  constructor(private http: HttpClient) {
    const user: User = JSON.parse(localStorage.getItem('user')!);
    if (user) this.setCurrentUser(user);
  }

  login(model: Login): Observable<void> {
    return this.http.post<User>(this.baseUrl + 'users/login', model).pipe(
      map((response) => {
        if (response) this.setCurrentUser(response);
      })
    )
  }

  // Log In and set user in local storage
  setCurrentUser(user: User): void {
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUserSource.next(user);
  }

  // Log out and remove user from local storage
  logout(): void {
    localStorage.removeItem('user');
    this.currentUserSource.next(null);
  }

  decodedJwtToken(token: string): void {
    //const manualToken = JSON.parse(atob(token.split('.')[1]));
    const helper = new JwtHelperService();
    const decodedToken = helper.decodeToken(token);
    const isExpired = helper.isTokenExpired(token);
    if (isExpired) this.logout();
  }
}
