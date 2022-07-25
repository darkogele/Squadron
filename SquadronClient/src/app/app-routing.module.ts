import { CreateAccountComponent } from './components/accounts/create-account/create-account.component';
import { EditAccountComponent } from './components/accounts/edit-account/edit-account.component';
import { GraphicComponent } from './components/graphic/graphic.component';
import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { HomeComponent } from './components/home/home.component';
import { UploadComponent } from './components/upload/upload.component';
import { LoginComponent } from './components/login/login.component';
import { NotFoundComponent } from './errors/not-found/not-found.component';
import { AuthGuard } from './guards/auth.guard';

const routes: Routes = [
  { path: '', component: HomeComponent },
  { path: 'login', component: LoginComponent },
  { path: 'account/create', component: CreateAccountComponent },
  {
    path: '',
    runGuardsAndResolvers: 'always',
    canActivate: [AuthGuard],
    children: [
      { path: 'file-upload', component: UploadComponent },
      { path: 'graphic', component: GraphicComponent },
      { path: 'account/edit', component: EditAccountComponent },
    ]
  },
  { path: '**', component: NotFoundComponent, pathMatch: 'full' }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
