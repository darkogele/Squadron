import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { ToastrService } from 'ngx-toastr';
import { UploadService } from 'src/app/services/upload.service';
import { environment } from 'src/environments/environment';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {
  // progress!: number;
  // message!: string;

  uploader: FileUploader;
  baseUrl = environment.baseUrl;
  hasBaseDropzoneOver = false;

  constructor(private uploadService: UploadService, private toastr: ToastrService, private router: Router) {
    this.uploader = new FileUploader({
      url: this.baseUrl + 'files/add-file',
      isHTML5: true,
      removeAfterUpload: true,
      autoUpload: false,
      maxFileSize: 10 * 1024 * 1024
    });

    this.uploader.onAfterAddingFile = (file) => {
      file.withCredentials = false;
    }

    this.uploader.onSuccessItem = (item, response, status, headers) => {
      if (response) {
        this.toastr.success('File uploaded successfully');
        this.router.navigate(['/grafic']);
      }
    }

    this.uploader.onErrorItem = (item, response, status, headers) => {
      if (response) {
        this.toastr.error('File upload error ' + response);
      }
    }
  }

  ngOnInit(): void { }

  // uploadFile(file: any) {
  //   if (file.length === 0) {
  //     return;
  //   }

  //   const fileToUpload = <File>file[0];
  //   const formData = new FormData();
  //   formData.append('file', fileToUpload, fileToUpload.name);

  //   this.uploadService.upload(formData).subscribe({
  //     next: (res) => {
  //       debugger;
  //       this.toastr.success('File uploaded successfully');

  //     },
  //     error: (err) => { debugger; this.toastr.error(err) }
  //   })
  // }

  fileOverBase(e: any) {
    this.hasBaseDropzoneOver = e;
  }
}