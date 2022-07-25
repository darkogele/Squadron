import { UserService } from 'src/app/services/user.service';
import { Injectable } from '@angular/core';
import { CanActivate, Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { Observable, map } from 'rxjs';

@Injectable({ providedIn: 'root' })
export class AuthGuard implements CanActivate {
  constructor(private accountService: UserService, private toastr: ToastrService, private router: Router) { }

  canActivate(): Observable<boolean> {
    return this.accountService.currentUser$.pipe(
      map(user => {
        if (user) { return true; }
        this.toastr.error('You must login!');
        this.router.navigateByUrl('/login');
        return false;
      })
    );
  }
}

