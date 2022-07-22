import { NgModule } from '@angular/core';
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatIconModule } from '@angular/material/icon';
import { MatButtonModule } from '@angular/material/button';
import { MatGridListModule } from '@angular/material/grid-list';
import { MatCardModule } from '@angular/material/card';
import { MatInputModule } from '@angular/material/input';
import { MatMenuModule } from '@angular/material/menu';

const materialModules = [
  MatToolbarModule,
  MatIconModule,
  MatButtonModule,
  MatGridListModule,
  MatCardModule,
  MatInputModule,
  MatMenuModule
]

@NgModule({
  declarations: [],
  imports: [
    ...materialModules
  ],
  exports: [
    ...materialModules
  ],
})
export class MaterialModule { }
