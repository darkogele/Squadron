import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { catchError, Observable, throwError } from 'rxjs';
import { ToastrService } from 'ngx-toastr';
import { Router } from '@angular/router';

@Injectable()
export class ErrorInterceptor implements HttpInterceptor {

  constructor(private router: Router, private toastr: ToastrService) { }

  intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
    return next.handle(request).pipe(
      catchError(error => {
        if (error) {
          switch (error.status) {
            case 400:
              // TODO add more types of 400 responces such as model state errors
              debugger;
              this.toastr.error(error.error, error.status);
              break;
            case 401:
              if (error?.error?.title)
                this.toastr.error(error.error.title, error.status);
              else if (error?.error)
                this.toastr.error(error.error, error.status);
              else
                this.toastr.error('Unauthorized', error.status);
              break
            case 404:
              this.router.navigateByUrl('/not-found');
              break;
            default:
              this.toastr.error('Something unexpected went wrong', 'ERROR');
              console.log(error);
              break;
          }
        }
        return throwError(() => error)
      })
    )
  }
}
