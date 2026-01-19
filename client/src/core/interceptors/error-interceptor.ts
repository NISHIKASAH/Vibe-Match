import { HttpInterceptorFn } from '@angular/common/http';
import { ToastService } from '../services/toast-service';
import { inject } from '@angular/core';
import { NavigationExtras, Router } from '@angular/router';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toast = inject(ToastService);
  const router = inject(Router);
  return next(req).pipe(
    catchError(error => {
      switch (error.status){
        case 400 : 
        if(error.error.errors){
          const modelstateErrors = [];
          for(const key in error.error.errors){
            if(error.error.errors[key]){
              modelstateErrors.push(error.error.errors[key]);
            }
          }
          throw modelstateErrors.flat();
        }
        else{
          toast.error(error.error, error.status);
          
        }
        break;
         case 401 : 
        toast.error('Unauthorized');
        break;
         case 404 : 
         router.navigateByUrl('/not-found');
        break;
         case 500 : 
         const navigationExtras  : NavigationExtras = {state : { error : error.error}}
         router.navigateByUrl('/server-error' , navigationExtras);
        break;
        default :
        toast.error('something went wrong');
        break;
      }
      throw error;
    })
  )
};
