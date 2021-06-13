import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { OlympicWinnerModel } from '../models/olympic-winner.model';

@Injectable()
export class HomeService {
    // Define Base API
    apiURL = "https://localhost:44341";

    // Http Options
    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    }

    constructor(private http: HttpClient) { }

    public getOlympicWinners(postData): Observable<OlympicWinnerModel> {
        return this.http.post<OlympicWinnerModel>(this.apiURL + '/api/OlympicWinner/GetOlympicWinnerList', JSON.stringify(postData), this.httpOptions)
            .pipe(
                retry(1),
                catchError(this.handleError)
            )
    }

    // Error handling 
    handleError(error) {
        let errorMessage = '';
        if (error.error instanceof ErrorEvent) {
            // Get client-side error
            errorMessage = error.error.message;
        } else {
            // Get server-side error
            errorMessage = `Error Code: ${error.status}\nMessage: ${error.message}`;
        }
        window.alert(errorMessage);
        return throwError(errorMessage);
    }
}