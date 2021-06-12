import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable, throwError } from 'rxjs';
import { retry, catchError } from 'rxjs/operators';
import { BranchModel } from '../models/branch.model';

@Injectable()
export class HomeService {
    // Define Base API
    apiURL = "https://localhost:5001";

    // Http Options
    httpOptions = {
        headers: new HttpHeaders({
            'Content-Type': 'application/json'
        })
    }

    constructor(private http: HttpClient) { }

    public getBranches(startRow, endRow): Observable<BranchModel> {
        let postData = {
            "startIndex": startRow + 1,
            "pageSize": endRow,
            "totalRecords": 1569,
            "filterFormId": "string",
            "gridContainerId": "string",
            "gridPageIndex": 0,
            "searchKeyword": "string",
            "branchId": null,
            "branchGridFilterListItem": [{
                "id": 0,
                "branch": "string",
                "address": "string"
            }]
        };
        return this.http.post<BranchModel>(this.apiURL + '/api/Branch/GetBranchList', JSON.stringify(postData), this.httpOptions)
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