import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { catchError, Observable, of, tap } from 'rxjs';
import { Observation } from './observation';

@Injectable({
  providedIn: 'root',
})
export class ObservationService {
  private observationsUrl = 'https://localhost:7223/api/observations'; // URL to web api

  private httpOptions = {
    headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  };

  constructor(private http: HttpClient) {}

  /** GET observations from the server */
  getObservations(): Observable<Observation[]> {
    return this.http.get<Observation[]>(this.observationsUrl).pipe(
      tap((_) => console.debug('Fetched observations')),
      catchError(this.handleError<Observation[]>('getObservations', []))
    );
  }

  /** GET observation by id. Will 404 if id not found */
  getObservation(id: number): Observable<Observation> {
    const url = `${this.observationsUrl}/${id}`;
    return this.http.get<Observation>(url).pipe(
      tap((_) => console.debug(`Fetched observation id=${id}`)),
      catchError(this.handleError<Observation>(`getObservation id=${id}`))
    );
  }

  //////// Save methods //////////

  /** POST: add a new observation to the server */
  addObservation(observation: Observation): Observable<Observation> {
    const observationRequest = {
      birdId: observation.bird.id,
      observationDate: observation.observationDate,
      location: observation.location,
      remark: observation.remark,
      //photo: observation.photo
    }

    return this.http.post<Observation>(this.observationsUrl, observationRequest, this.httpOptions).pipe(
      tap((newObservation: Observation) => console.debug(`added observation w/ id=${newObservation.id}`)),
      catchError(this.handleError<Observation>('addObservation'))
    );
  }

  /** DELETE: delete the observation from the server */
  deleteObservation(id: number): Observable<any> {
    const url = `${this.observationsUrl}/${id}`;

    return this.http.delete(url, this.httpOptions).pipe(
      tap((_) => console.debug(`deleted observation id=${id}`)),
      catchError(this.handleError<Observation>('deleteObservation'))
    );
  }

  /** PUT: update the observation on the server */
  updateObservation(observation: Observation): Observable<any> {
    const url = `${this.observationsUrl}/${observation.id}`;

    const observationRequest = {
      birdId: observation.bird.id,
      observationDate: observation.observationDate,
      location: observation.location,
      remark: observation.remark,
      // photo: observation.photo
    }

    return this.http.put<Observation>(url, observationRequest, this.httpOptions).pipe(
      tap((_) => console.debug(`updated observation id=${observation.id}`)),
      catchError(this.handleError<any>('updateObservation'))
    );
  }


  /** DELETE: delete the observation from the server */
  deleteObservationPhoto(id: number): Observable<any> {
    const url = `${this.observationsUrl}/${id}/photo`;

    return this.http.delete(url, this.httpOptions).pipe(
      tap((_) => console.debug(`deleted observation photo id=${id}`)),
      catchError(this.handleError<Observation>('deleteObservationPhoto'))
    );
  }

  /** PUT: update the observation on the server */
  updateObservationPhoto(id: number, photo: File): Observable<any> {
    const url = `${this.observationsUrl}/${id}/photo`;

    const formData = new FormData();
    formData.append("file", photo);

    return this.http.put(url, formData, { headers: new HttpHeaders('multipart/form-data')}).pipe(
      tap((_) => console.debug(`updated observation photo id=${id}`)),
      catchError(this.handleError<any>('updateObservationPhoto'))
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
