import { User } from './../../models/user';
import { UserService } from './../../services/user.service';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-nav',
  templateUrl: './nav.component.html',
  styleUrls: ['./nav.component.scss']
})
export class NavComponent implements OnInit {
  user: User | null = null;

  constructor(private noticitaitons: ToastrService, private router: Router, private userService: UserService) { }

  ngOnInit(): void {
    this.userService.currentUser$.subscribe(user => {
      this.user = user;
      if (user) {
        this.noticitaitons.info(`Welcome ${user.displayName}`);
      } else if (user === null)
        this.router.navigateByUrl('/login');
    });
  }

  logout(): void {
    this.noticitaitons.info(`Log out ${this.user?.displayName}`)
    this.userService.logout();
    this.router.navigateByUrl('/login');
  }

  editAccount() {
    this.router.navigateByUrl('/account/edit');
  }
}
