import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';
import { AuthService } from '../service/auth.service';
import { UserLogin } from '../models/user-login';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;
  errorMessageShow:boolean = false;
  
  constructor(private authService: AuthService, private toastrService: ToastrService) {
    this.loginForm = new FormGroup({
      username: new FormControl("", [Validators.required]),
      password: new FormControl("", [Validators.required])
    });
  }

  ngOnInit(): void {
  }


  submitForm(){
    if(this.loginForm.invalid){
      this.loginForm.markAllAsTouched();
      this.toastrService.error("Failed");
      this.errorMessageShow = true;
    }
    else {
      var cred = new UserLogin();
      cred.UserName = this.Username.value;
      cred.Password = this.Password.value;
      this.authService.login(cred).subscribe(data => {
        this.errorMessageShow = false;
        this.toastrService.success("Welcome");
        // To be implemented
        // view how to use resolver....
      }, (error)=>{
        this.errorMessageShow = true;
      })
    }
    
  }

  //#region Getter methods
  get Username() {
    return this.loginForm.get('username') as FormControl
  }

  get Password() {
    return this.loginForm.get('password') as FormControl
  }
  //#endregion
  
}
