import { Component } from '@angular/core';

import { User, UserService } from 'src/app/shared';

@Component({
  selector: 'add-user',
  templateUrl: './add-user.component.html'
})

export class AddUserComponent {

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

