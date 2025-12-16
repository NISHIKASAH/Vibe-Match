import { inject, Injectable } from '@angular/core';
import { AccountService } from './account-service';
import { of as observableof } from 'rxjs';

@Injectable({
  providedIn: 'root',
})
export class InitService {

  private accountService = inject(AccountService);

  init() {
    const userString = localStorage.getItem('user');
    if (!userString) return observableof(null);
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);

    return observableof(null);
  }


}
