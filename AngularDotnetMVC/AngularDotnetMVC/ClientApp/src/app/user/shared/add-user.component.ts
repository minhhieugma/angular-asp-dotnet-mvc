import { Component } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';

import { User, UserService } from 'src/app/shared';

@Component({
  selector: 'add-user',
  templateUrl: './add-user.component.html'
})

export class AddUserComponent {
  processing: boolean = false;

  userForm = this.fb.group({
    firstName: ['Hieu', Validators.required],
    lastName: ['Le', Validators.required],
  });


  constructor(private fb: FormBuilder, private userService: UserService) {
  }

  public async send() {
    if (this.processing) return
    this.processing = true

    const user = new User()
    user.firstName = this.userForm.value.firstName
    user.lastName = this.userForm.value.lastName

    try {
      const result = await this.userService.addUser(user).toPromise()
      //alert(JSON.stringify(result))

      this.userForm.patchValue({
        firstName: '',
        lastName: ''
      });

      alert(`User ${result.id} created!`)
    } catch (e) {
      debugger
      alert(JSON.stringify(e.error))
    }
    finally {
      this.processing = false
    }

  }


}

