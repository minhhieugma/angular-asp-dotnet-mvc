import { Component, Inject } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { UserService } from '../shared/user.service';
import { User } from '../shared/user.model';

@Component({
  selector: 'user',
  templateUrl: './user.component.html'
})
export class UserComponent {

  public firstName: string = 'Hieu';
  public lastName: string = 'Le';


  constructor(private userService: UserService) {
  }

  public async send() {

    const user = new User()
    user.firstName = this.firstName
    user.lastName = this.lastName

    try {
      const result = await this.userService.addUser(user).toPromise()
    } catch (e) {
      alert(e)
    }


  }


}

