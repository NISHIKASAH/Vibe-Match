import { inject } from '@angular/core';
import { CanActivateFn } from '@angular/router';
import { AccountService } from '../services/account-service';
import { ToastService } from '../services/toast-service';

export const authGuard: CanActivateFn = () => {
  
   var accountservices = inject(AccountService);
   var toast = inject(ToastService);

   if(accountservices.currentUser()){
    return true;
   }
   else{
    toast.error(' you shall not pass');
     return false;
   }
   

};
