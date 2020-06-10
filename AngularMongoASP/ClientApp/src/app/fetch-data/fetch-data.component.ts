import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Observable } from 'rxjs';

export interface IBook {
  author: string;
  bookName: string;
  category: string;
  id: string;
  price?: number;
  iconId: string;
 // icon: FormData;
  icon: File;
}

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {

  public isEdit: boolean;
  public isMyEdit: boolean;
  public myFile: FormData;
  public books$: Observable<Array<IBook>>;
  public myBooks$: Observable<Array<IBook>>;
  public newBook: IBook = {id: '', bookName: 'bookName', author: 'author', category: 'category', price: 123, icon: null, iconId: null};
  public newMyBook: IBook = {id: null, bookName: null, author: null, category: null, price: null, icon: null, iconId: null};
  constructor(
    private apiService: ApiService
  ) {
  }
  ngOnInit() {
    this.getAllBooks();
    this.getAllMyBooks();
  }

  public deleteBook(bookId: string): void {
    this.apiService.deleteBook(bookId)
      .subscribe(
        res => {
          console.log(res);
          this.getAllBooks();
        },
        error => console.log(error)
      );
  }

  public addOneBook(): void {
    this.apiService.addOneBook(this.newBook)
      .subscribe(
        res => {
          console.log(res);
          this.newBook = {id: null, bookName: null, author: null, category: null, price: null, icon: null, iconId: null};
          this.getAllBooks();
        },
        error => console.log(error)
      );
  }
  public updateOneBook(bookId: string): void {
    this.apiService.updateOneBook(bookId, this.newBook)
      .subscribe(
        res => {
          console.log(res);
          this.newBook = {id: null, bookName: null, author: null, category: null, price: null, iconId: null, icon: null};
          this.getAllBooks();
        },
        error => console.log(error)
      );
  }

  public deleteMyBook(bookId: string): void {
    this.apiService.deleteMyBook(bookId)
      .subscribe(
        res => {
          console.log(res);
          this.getAllMyBooks();
        },
        error => console.log(error)
      );
  }
  public addOneMyBook(): void {
    this.apiService.addOneMyBook(this.newMyBook)
      .subscribe(
        res => {
          console.log(res);
          this.newMyBook = {id: null, bookName: null, author: null, category: null, price: null, icon: null, iconId: null};
          this.getAllMyBooks();
        },
        error => console.log(error)
      );
  }
  public updateOneMyBook(bookId: string): void {
    this.apiService.updateOneMyBook(bookId, this.newMyBook)
      .subscribe(
        res => {
          console.log(res);
          this.newMyBook = {id: null, bookName: null, author: null, category: null, price: null, iconId: null, icon: null};
          this.getAllMyBooks();
        },
        error => console.log(error)
      );
  }
  private getAllBooks(): void {
    this.books$ = this.apiService.getAllBooks();
  }
  private getAllMyBooks(): void {
    this.myBooks$ = this.apiService.getAllMyBooks();
  }
  public uploadFile(): void {
    this.apiService.uploadFile(this.myFile).subscribe(
      res => {
        console.log(res);
      }, error => console.error(error)
    );
  }

}
