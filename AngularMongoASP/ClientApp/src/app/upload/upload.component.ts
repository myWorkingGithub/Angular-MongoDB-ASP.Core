import { Component, EventEmitter, OnInit, Output } from '@angular/core';
import { HttpClient, HttpEventType, HttpRequest } from '@angular/common/http';
import { ApiService } from '../services/api.service';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {

  @Output() fileImage: EventEmitter<FormData> = new EventEmitter();
  public progress: number;
  public message: string;
  constructor(
    private http: HttpClient,
    private apiService: ApiService
  ) { }

  ngOnInit(): void {
  /*  this.apiService.getOneImage('Intelico One – Chromium_027')
      .subscribe(resp => {
        console.log(resp);
      }, error =>  console.log(error));*/
  }

  upload(event) {

    console.log(event.target.files);
   // this.asset.imageFile = event.target.files;
    this.fileImage.emit(event.target.files);
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
      //  this.asset.imageUrl = e.target.result;
      };
      reader.readAsDataURL(event.target.files[0]);
    }
  }

  upload2(files) {
    if (files.length === 0)
      return;
    const formData = new FormData();
    for (let file of files)
    //  formData.append('file', file, file.name);
      formData.append(file.name,  file);


    this.fileImage.emit(formData);


    /*const uploadReq = new HttpRequest('POST', `api/upload`, formData, {
      reportProgress: true,
    });
    this.http.request(uploadReq).subscribe(event => {
      if (event.type === HttpEventType.UploadProgress)
        this.progress = Math.round(100 * event.loaded / event.total);
      else if (event.type === HttpEventType.Response)
        this.message = event.body.toString();
    });*/
  }

}
