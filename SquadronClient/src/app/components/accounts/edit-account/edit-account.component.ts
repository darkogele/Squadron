import { AbstractControl, FormBuilder, FormControl, FormGroup, ValidatorFn, Validators } from '@angular/forms';
import { UserService } from './../../../services/user.service';
import { ToastrService } from 'ngx-toastr';
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';

@Component({
  selector: 'app-edit-account',
  templateUrl: './edit-account.component.html',
  styleUrls: ['./edit-account.component.scss']
})
export class EditAccountComponent implements OnInit {
  changePassword: FormGroup;
  minPw: number = 5;
  changePassText: string = 'Change Password'
  changePasswordMode: boolean = false;
  editAccount: FormGroup;

  constructor(private fb: FormBuilder,
    private accountService: UserService,
    private router: Router,
    private noticitaitons: ToastrService) {
    this.changePassword = this.fb.group({
      currentPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      newPassword: ['', [Validators.required, Validators.minLength(6), Validators.maxLength(100)]],
      confirmNewPassword: ['', [Validators.required, this.accountService.matchValues('newPassword')]]
    });

    this.editAccount = this.fb.group({
      displayName: ['', Validators.required],
      email: ['', [Validators.required, Validators.email]],
    });
  }

  get currentPassword() { return this.changePassword.get('currentPassword') as FormControl; }
  get newPassword() { return this.changePassword.get('newPassword') as FormControl; }
  get confirmNewPassword() { return this.changePassword.get('confirmNewPassword') as FormControl; }
  get displayName() { return this.editAccount.get('displayName') as FormControl; }
  get email() { return this.editAccount.get('email') as FormControl; }

  ngOnInit(): void {
    this.changePassword.controls['newPassword'].valueChanges.subscribe(() => {
      this.changePassword.controls['confirmNewPassword'].updateValueAndValidity();
    });
    this.loadAccount();
  }

  changePasswordBtn() {
    this.changePasswordMode = !this.changePasswordMode;
    if (this.changePasswordMode) this.changePassText = 'Hide Change Password';
    else this.changePassText = 'Change Password';
  }

  updatePassword(): void {
    if (this.changePassword.valid) {
      this.accountService.changePassword(this.changePassword.value).subscribe((res) => {
        if (res) {
          this.noticitaitons.success('Password was successfully changed');
          this.router.navigateByUrl('/');
        }
      });
    }
  }

  loadAccount(): void {
    this.accountService.currentUser$.subscribe(user => {
      if (user) {
        this.editAccount.patchValue({
          displayName: user.displayName,
          email: user.email,
        })
      }
    })
  }

  updateAccount(): void {
    if (this.editAccount.valid) {
      this.accountService.editUserData(this.editAccount.value).subscribe(() => {
        this.noticitaitons.success('Account was successfully updated');
        this.router.navigateByUrl('/');
      })
    }
  }
}
