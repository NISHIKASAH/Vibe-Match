import { Component, inject, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../core/services/account-service';
import { Router, RouterLink, RouterLinkActive } from "@angular/router";
import { ToastService } from '../../core/services/toast-service';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, RouterLink , RouterLinkActive],
  templateUrl: './nav.html',
  styleUrl: './nav.css',
})
export class Nav {
  protected accountService = inject(AccountService);
  protected  toast = inject(ToastService);
  private router = inject(Router);
  protected creds : any = {};
  


  login(){
    this.accountService.login(this.creds).subscribe({
      next : response => {
        this.router.navigateByUrl('/members');
      this.toast.success(" login sucessfully");
        console.log(response);
        this.creds = {}
      },
      error : err => {
        alert(err.message);
        this.toast.error("invalid login credential");
      }
    })
  }
  logout(){
    this.router.navigateByUrl('/');
    this.toast.error("logout successfully")
    this.accountService.logout();
  }
}
