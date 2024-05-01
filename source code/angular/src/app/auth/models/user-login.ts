import { IUserLogin } from "../interfaces/iuser-login";

export class UserLogin implements IUserLogin {
    UserName: string = "";
    Password: string = "";
}
