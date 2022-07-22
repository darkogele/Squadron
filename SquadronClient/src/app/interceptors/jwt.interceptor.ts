import { UserService } from './../services/user.service';
import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, take } from 'rxjs';

@Injectable()
export class JwtInterceptor implements HttpInterceptor {

  constructor(private userService: UserService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    this.userService.currentUser$.pipe(take(1)).subscribe(user => {
      if (user) {
        this.userService.decodedJwtToken(user.token);
        request = request.clone({
          setHeaders: {
            Authorization: `Bearer ${user.token}`
          }
        })
      }
    });

    return next.handle(request);
  }
}
