import { Component, OnInit } from '@angular/core';
import { OlympicWinnerModel } from '../common/models/olympic-winner.model';
import { FacadeService } from '../common/services/facade.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  olympicWinnerList: Array<OlympicWinnerModel>;
  constructor(private facadeService: FacadeService) { }

  ngOnInit() {
    this.facadeService.getOlympicWinners(1, 20).subscribe((data:any) => {
      this.olympicWinnerList = data.olympicWinnerGridFilterListItem;
      console.log(this.olympicWinnerList)
    });
  }
}
