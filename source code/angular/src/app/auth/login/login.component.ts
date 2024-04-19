import { Component, OnInit } from '@angular/core';
import { FormGroup, FormControl, Validators } from '@angular/forms';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrl: './login.component.css'
})
export class LoginComponent implements OnInit {

  loginForm: FormGroup;

  constructor() {
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
    }
    else {
      console.log(this.loginForm)
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
