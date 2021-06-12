import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
// import { AccountService } from './account.service'; // Commenting because, in future we will add services here
import { HomeService } from './home.service';
// import { LoginService } from './login.service';
// import { ProductsService } from './products.service';
// import { SignUpService } from './sign-up.service';

// Facade design pattern for complex systems
import { FacadeService } from './facade.service'; 

@NgModule({
  imports: [
    CommonModule
  ],
  declarations: [],
  providers:[
    // AccountService,
    HomeService,
    // LoginService,
    // ProductsService,
    // SignUpService,
    FacadeService
  ]
})
export class ServicesModule { }