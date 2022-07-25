export interface Login {
  email: string;
  password: string;
}


export interface User {
  displayName: string;
  email: string;
  token: string;
}

export interface ChangePassword {
  currentPassword: string;
  newPassword: string;
  confirmNewPassword: string;
}
