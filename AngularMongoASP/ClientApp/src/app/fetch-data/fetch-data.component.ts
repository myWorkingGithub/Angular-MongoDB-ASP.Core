import { Component, OnInit } from '@angular/core';
import { ApiService } from '../services/api.service';
import { Observable } from 'rxjs';

export interface IBook {
  author: string;
  bookName: string;
  category: string;
  id: string;
  price: number;
}

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent implements OnInit {

  public isEdit: boolean;
  public isMyEdit: boolean;
  public books$: Observable<Array<IBook>>;
  public myBooks$: Observable<Array<IBook>>;
  public newBook: IBook = {id: null, bookName: null, author: null, category: null, price: null};
  public newMyBook: IBook = {id: null, bookName: null, author: null, category: null, price: null};
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
          this.newBook = {id: null, bookName: null, author: null, category: null, price: null};
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
          this.newBook = {id: null, bookName: null, author: null, category: null, price: null};
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
          this.newMyBook = {id: null, bookName: null, author: null, category: null, price: null};
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
          this.newMyBook = {id: null, bookName: null, author: null, category: null, price: null};
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

}

interface WeatherForecast {
  date: string;
  temperatureC: number;
  temperatureF: number;
  summary: string;
}
