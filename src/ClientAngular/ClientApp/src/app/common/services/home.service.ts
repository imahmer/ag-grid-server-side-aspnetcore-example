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

    public getOlympicWinners(startRow, endRow): Observable<OlympicWinnerModel> {
        let postData = {
            "startIndex": startRow + 1,
            "pageSize": endRow,
            "totalRecords": 1569,
            "filterFormId": "string",
            "gridContainerId": "string",
            "gridPageIndex": 0,
            "searchKeyword": "string",
            "olympicWinnerId": null,
            "olympicWinnerGridFilterListItem": [{
                "id": 0,
                "athlete": "string",
                "age": 0,
                "country": "string",
                "year": 0,
                "date": "string",
                "sport": "string",
                "gold": 0,
                "silver": 0,
                "bronze": 0,
                "total": 0,
            }]
        };
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