import { Component, OnInit } from '@angular/core';
import { BranchModel } from '../common/models/branch.model';
import { FacadeService } from '../common/services/facade.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
})
export class HomeComponent implements OnInit {
  branchList: Array<BranchModel>;
  constructor(private facadeService: FacadeService) { }

  ngOnInit() {
    this.facadeService.getBranches(1, 20).subscribe((data:any) => {
      this.branchList = data.branchGridFilterListItem;
      console.log(this.branchList)
    });
  }
}
