import { Component, EventEmitter, OnInit, Output } from '@angular/core';

@Component({
  selector: 'app-upload',
  templateUrl: './upload.component.html',
  styleUrls: ['./upload.component.scss']
})
export class UploadComponent implements OnInit {

  @Output() fileImage: EventEmitter<File> = new EventEmitter();
//  @Output() fileImage: EventEmitter<any> = new EventEmitter();
  public progress: number;
  public message: string;
  constructor(
  ) { }

  ngOnInit(): void {}

  public upload(event) {
    console.log(event.target.files[0])
    this.fileImage.emit(event.target.files[0]);
    if (event.target.files && event.target.files[0]) {
      const reader = new FileReader();
      reader.onload = (e: any) => {
      //  this.asset.imageUrl = e.target.result;
      };
    //  this.fileImage.emit(reader.readAsDataURL(event.target.files[0]));
    }
  }

 /* public uploadFormData(files) {
    if (files.length === 0)
      return;
    const formData = new FormData();
    for (const file of files) {
    //  formData.append(file.name, file);
      formData.append('file', file);
    }
    this.fileImage.emit(formData);
  }
*/
}
