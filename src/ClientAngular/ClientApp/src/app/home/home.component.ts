import { Component, OnInit } from '@angular/core';
import { OlympicWinnerModel } from '../common/models/olympic-winner.model';
import { FacadeService } from '../common/services/facade.service';
import { Module } from '@ag-grid-community/core';
import { ServerSideRowModelModule } from '@ag-grid-enterprise/server-side-row-model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  olympicWinnerList: Array<OlympicWinnerModel>;

  private gridApi;
  private gridColumnApi;

  defaultColDef = {
    sortable: true,
    filter: true
  };
  
  modules: Module[] = [ServerSideRowModelModule];
  rowModelType;
  serverSideStoreType;

  columnDefs = [
    { field: 'id', sortable: false, filter: false },
    { field: 'athlete' },
    { field: 'age' },
    { field: 'country' },
    { field: 'year' },
    { field: 'date' },
    { field: 'sport' },
    { field: 'gold' },
    { field: 'silver' },
    { field: 'bronze' },
    { field: 'total' }
  ];



  constructor(private facadeService: FacadeService) { 
    this.rowModelType = 'serverSide';
    this.serverSideStoreType = 'partial';
  }

  ngOnInit() {
    // this.facadeService.getOlympicWinners(1, 20).subscribe((data: any) => {
    //   this.olympicWinnerList = data.olympicWinnerGridFilterListItem;
    //   console.log(this.olympicWinnerList)
    // });
  }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
    var datasource = this.createServerSideDatasource(this.facadeService);
    params.api.setServerSideDatasource(datasource);
    // this.http
    //   .get('https://www.ag-grid.com/example-assets/olympic-winners.json')
    //   .subscribe((data) => {
    //     var fakeServer = createFakeServer(data);
    //     var datasource = createServerSideDatasource(fakeServer);
    //     params.api.setServerSideDatasource(datasource);
    //   });
  }

  createServerSideDatasource(facadeService: FacadeService) {
    return {
      getRows: function (params) {
        setTimeout(function () {
          console.log(params.request)
          let postData = {
            "startIndex": params.request.startRow + 1,
            "pageSize": params.request.endRow,
            "totalRecords": 0,
            "filterFormId": "string",
            "gridContainerId": "string",
            "gridPageIndex": 0,
            "searchKeyword": "string",
            "olympicWinnerId": null,
            "sortModel": params.request.sortModel,
            "filterModel": params.request.filterModel,
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
          facadeService.getOlympicWinners(postData).subscribe((data: any) => {
            this.olympicWinnerList = data.olympicWinnerGridFilterListItem;
            params.successCallback(this.olympicWinnerList, data.totalRecords);
          });
        }, 500);
      }
    }
  }
}
