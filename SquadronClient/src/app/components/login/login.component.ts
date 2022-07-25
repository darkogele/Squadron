import { ToastrService } from 'ngx-toastr';
import { UserService } from './../../services/user.service';
import { Router } from '@angular/router';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { User } from 'src/app/models/user';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  loginForm: FormGroup;
  minPw: number = 5;


  constructor(private fb: FormBuilder, private router: Router, private userService: UserService, private toastrService: ToastrService) {

    this.loginForm = this.fb.group({
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(this.minPw)]],
    });
  }

  get password() { return this.loginForm.get('password') as FormControl; }
  get email() { return this.loginForm.get('email') as FormControl; }

  ngOnInit(): void {
    this.userService.currentUser$.subscribe(user => {
      if (user) {
        this.router.navigateByUrl('/');
      }
    });
  }

  login(): void {
    this.userService.login(this.loginForm.value).subscribe(() => {
      this.router.navigateByUrl('/');
    })
  }

  forgotPassword() {

  }
}
