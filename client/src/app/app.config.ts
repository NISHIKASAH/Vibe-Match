import { ApplicationConfig, inject, provideAppInitializer, provideBrowserGlobalErrorListeners, provideZonelessChangeDetection } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { InitService } from '../core/services/init-service';
import { lastValueFrom } from 'rxjs';
import { errorInterceptor } from '../core/interceptors/error-interceptor';

export const appConfig: ApplicationConfig = {
  providers: [
    provideBrowserGlobalErrorListeners(),
    provideZonelessChangeDetection(),
    provideRouter(routes),
    provideHttpClient(withInterceptors([errorInterceptor])),
    provideAppInitializer(   //implementing, so when refering  , guard initilising for user is done before anything
      async () => {
        const initService = inject(InitService);

        return new Promise<void>((resolve) => {

          setTimeout(async () => {
            try {
              return lastValueFrom(initService.init());

            } finally {
              const splash = document.getElementById('initial-splash');
              if (splash) {
                splash.remove()
              }
              resolve(); //resolving the promise 
            }
          } ,500)
        })

      }
    )
  ]
};
