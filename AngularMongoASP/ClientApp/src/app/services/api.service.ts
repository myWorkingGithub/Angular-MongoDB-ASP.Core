import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { IBook } from '../fetch-data/fetch-data.component';

@Injectable({
  providedIn: 'root'
})
export class ApiService {

  private url = 'http://localhost:3000/';
  private myUrl = 'http://localhost:3000/my';
  protected httpHeaders: HttpHeaders = new HttpHeaders();
  protected httpHeadersForm: HttpHeaders = new HttpHeaders();
  protected httpHeadersFormData: HttpHeaders = new HttpHeaders();
  constructor(
    private http: HttpClient
  ) {
    this.httpHeaders.append('Content-Type', 'application/json, charset=utf-8');
    this.httpHeadersForm.append('Content-Type', 'application/x-www-form-urlencoded');
    this.httpHeadersFormData.append('Content-Type', 'multipart/form-data');
  }

  getAllBooks(): Observable<Array<IBook>> {
    return this.http.get<Array<IBook>>(this.url + 'books', {headers: this.httpHeaders});
  }

  deleteBook(bookId: string): Observable<any> {
    return this.http.delete<any>(this.url + `books/${bookId}`, {headers: this.httpHeaders});
  }

  /*addOneBook(newBook: IBook, file: FormData): Observable<any> {
    return this.http.post<any>(this.url + `books`, {newBook, file}, {headers: this.httpHeaders});
  }*/


  addOneBook(newBook: IBook): Observable<any> {
   /* const body = new FormData();
    body.append('Author', newBook.author);
    body.append('BookName', newBook.bookName);
    body.append('Category', newBook.category);
    body.append('file', newBook.icon);
    console.log(body);*/

    const formData: FormData = new FormData();
    formData.append('icon', newBook.icon);
    delete newBook.id;
    delete newBook.icon;
    formData.append('body', JSON.stringify(newBook));
    console.log(formData);
    /*const body = this._toFormData(newBook);
    console.log(body);*/
    return this.http.post<any>(this.url + `books`, formData, {headers: this.httpHeadersFormData});
  }

  private _toFormData(data): FormData {
    const fd = new FormData();
    const keys = Object.keys(data);
    keys.forEach((key) => {
      fd.append(key, data[key]);
    });
    return fd;
  }


  updateOneBook(bookId, newBook: IBook): Observable<any> {
    return this.http.put<any>(this.url + `books/${bookId}`, newBook, {headers: this.httpHeaders});
  }
  // for a collection MyBooks
  getAllMyBooks(): Observable<Array<IBook>> {
    return this.http.get<Array<IBook>>(this.myUrl + 'books', {headers: this.httpHeaders});
  }

  deleteMyBook(bookId: string): Observable<any> {
    return this.http.delete<any>(this.myUrl + `books/${bookId}`, {headers: this.httpHeaders});
  }

  addOneMyBook(newBook: IBook): Observable<any> {
    return this.http.post<any>(this.myUrl + `books`, newBook, {headers: this.httpHeaders});
  }

  updateOneMyBook(bookId, newBook: IBook): Observable<any> {
    return this.http.put<any>(this.myUrl + `books/${bookId}`, newBook, {headers: this.httpHeaders});
  }
 /* uploadFile(file: FormData): Observable<any> {
    return this.http.post<any>(this.url + `api/upload`, file, {
      reportProgress: true,
    });
  }*/
  uploadFile(file: FormData): Observable<any> {
    return this.http.post<any>(this.url + `file`, file, {
      reportProgress: true,
    });
  }
}
