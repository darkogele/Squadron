import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/services/user.service';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Router } from '@angular/router';

@Component({
  selector: 'app-create-account',
  templateUrl: './create-account.component.html',
  styleUrls: ['./create-account.component.scss']
})
export class CreateAccountComponent implements OnInit {
  createAccountForm: FormGroup;
  minPw: number = 5;

  constructor(
    private fb: FormBuilder,
    private router: Router,
    private noticitaitons: ToastrService,
    private accountService: UserService) {
    this.createAccountForm = this.fb.group({
      displayName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
      password: ['', [Validators.required, Validators.minLength(this.minPw), Validators.maxLength(100)]],
      confirmPassword: ['', [Validators.required, this.accountService.matchValues('password')]]
    });

    this.createAccountForm.controls['password'].valueChanges.subscribe(() => {
      this.createAccountForm.controls['confirmPassword'].updateValueAndValidity();
    });
  }

  get displayName() { return this.createAccountForm.get('displayName') as FormControl; }
  get email() { return this.createAccountForm.get('email') as FormControl; }
  get phoneNumber() { return this.createAccountForm.get('phoneNumber') as FormControl; }
  get password() { return this.createAccountForm.get('password') as FormControl; }
  get confirmPassword() { return this.createAccountForm.get('confirmPassword') as FormControl; }

  ngOnInit(): void { }

  createAccount(): void {
    if (this.createAccountForm.valid) {
      this.accountService.createUser(this.createAccountForm.value).subscribe((res) => {
        if (res) {
          this.noticitaitons.success('Account was successfully created');
        }
      });
    }
  }
}
