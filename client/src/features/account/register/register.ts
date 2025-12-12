import { Component, inject, Inject, output, Output } from '@angular/core';
import { RegisterCreds } from '../../../types/user';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../../../core/services/account-service';

@Component({
  selector: 'app-register',
  imports: [FormsModule],
  templateUrl: './register.html',
  styleUrl: './register.css',
})
export class Register {
  private accountService  = inject(AccountService);
  protected creds = {} as RegisterCreds;
  cancelRegister = output<boolean>();
  register(){
    this.accountService.register(this.creds).subscribe({
      next : response => {
        console.log(response), 
        this.cancel();
      },
      error : err=> console.log(err.message)
    })
  }
  cancel(){
    console.log("cancelled!");
    this.cancelRegister.emit(false);
  }


}
