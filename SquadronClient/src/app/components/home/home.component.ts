import { Component, OnInit } from '@angular/core';
import { UserService } from 'src/app/services/user.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})
export class HomeComponent implements OnInit {
  showLogin: boolean = false;

  constructor(private userService: UserService) { }

  ngOnInit(): void {
    this.userService.currentUser$.subscribe(user => {
      if (user) {
        this.showLogin = false;
      }
      else { this.showLogin = true; }
    })
  }

}
