import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of, shareReplay, tap } from 'rxjs';
import { Bird } from './bird';

@Injectable({
  providedIn: 'root',
})
export class BirdService {
  private birdsUrl = 'https://localhost:7223/api/birds'; // URL to web api

  constructor(private http: HttpClient) {}

  /** GET birds from the server */
  getBirds(): Observable<Bird[]> {
    return this.http.get<Bird[]>(this.birdsUrl).pipe(
      tap((_) => console.debug('Fetched birds')),
      catchError(this.handleError<Bird[]>('getBirds', []))
    );
  }

  /**
   * Handle Http operation that failed.
   * Let the app continue.
   *
   * @param operation - name of the operation that failed
   * @param result - optional value to return as the observable result
   */
  private handleError<T>(operation = 'operation', result?: T) {
    return (error: any): Observable<T> => {
      // TODO: send the error to remote logging infrastructure
      console.error(error); // log to console instead

      // Let the app keep running by returning an empty result.
      return of(result as T);
    };
  }
}
