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
  
  modules: Module[] = [ServerSideRowModelModule];
  rowModelType;
  serverSideStoreType;
  cacheBlockSize;

  defaultColDef;
  autoGroupColumnDef;
  columnDefs;

  constructor(private facadeService: FacadeService) { 
    this.rowModelType = 'serverSide';
    this.serverSideStoreType = 'partial';
    this.cacheBlockSize = 10;

    this.columnDefs = [
      // { field: 'athlete' },
      // { field: 'age' },
      { 
        field: 'country',
        valueGetter: 'data.country',
        rowGroup: true,
        hide: true,
      },
      { 
        field: 'sport',
        hide: true,
      },
      { field: 'year' },
      // { field: 'date' },
      { 
        field: 'gold',
        aggFunc: 'sum', 
      },
      { 
        field: 'silver',
        aggFunc: 'sum',  
      },
      { 
        field: 'bronze',
        aggFunc: 'sum', 
      },
      // { 
      //   field: 'total',
      //   aggFunc: 'sum',  
      // }
    ];

    this.defaultColDef = {
      flex: 1,
      minWidth: 120,
      resizable: true,
      sortable: true,
      filter: true
    };

    this.autoGroupColumnDef  = {
      flex: 1,
      minWidth: 280,
      field: 'athlete',
    };
  }

  ngOnInit() { }

  onGridReady(params) {
    this.gridApi = params.api;
    this.gridColumnApi = params.columnApi;
    var datasource = this.createServerSideDatasource(this.facadeService);
    params.api.setServerSideDatasource(datasource);
  }

  createServerSideDatasource(facadeService: FacadeService) {
    return {
      getRows: function (params) {
        setTimeout(function () {
          let postData = {
            "startIndex": params.request.startRow,
            "pageSize": params.request.endRow,
            "totalRecords": 0,
            "filterFormId": "string",
            "gridContainerId": "string",
            "gridPageIndex": 0,
            "searchKeyword": "string",
            "rowGroupCols": params.request.rowGroupCols,
            "valueCols": params.request.valueCols,
            "groupKeys": params.request.groupKeys,
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
