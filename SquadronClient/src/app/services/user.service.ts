import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject, map, Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { ChangePassword, Login, User } from '../models/user';
import { JwtHelperService } from '@auth0/angular-jwt';
import { ValidatorFn, AbstractControl, FormGroup } from '@angular/forms';

@Injectable({ providedIn: 'root' })
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

  matchValues(matchTo: string): ValidatorFn {
    return (control: AbstractControl) => {
      if (control.parent) {
        return control?.value === (control?.parent as FormGroup).controls[matchTo].value
          ? null : { isMatching: true };
      }
      return null;
    };
  }

  changePassword(changePassword: ChangePassword): Observable<boolean> {
    return this.http.post<boolean>(this.baseUrl + 'users/change-password', changePassword);
  }

  editUserData(user: User): Observable<void> {
    return this.http.put<User>(this.baseUrl + 'users/edit-user', user).pipe(
      map(response => {
        if (response) this.setCurrentUser(response);
      })
    );
  }

  createUser(user: User): Observable<boolean> {
    return this.http.post<User>(this.baseUrl + 'users/register', user).pipe(
      map((response) => {
        if (response) {
          this.setCurrentUser(response);
          return true;
        }
        return false;
      })
    )
  }

}
