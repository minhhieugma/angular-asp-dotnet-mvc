import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClientModule } from '@angular/common/http';
import { FormBuilder, Validators } from '@angular/forms';
import { ReactiveFormsModule } from '@angular/forms';
import { By } from '@angular/platform-browser';

import { AddUserComponent } from 'src/app/user';
import { User, UserService } from 'src/app/shared';

describe('AddUserComponent', () => {
  let httpMock: HttpTestingController;

  let component: AddUserComponent;
  let fixture: ComponentFixture<AddUserComponent>;

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule, ReactiveFormsModule],

      // https://stackoverflow.com/questions/55261912/unit-test-fails-with-nullinjectorerror-in-angular?rq=1
      providers: [UserService, { provide: 'BASE_URL', useValue: 'http://localhost/' }, FormBuilder, Validators],

      declarations: [AddUserComponent],
    });

    httpMock = TestBed.get(HttpTestingController);

    fixture = TestBed.createComponent(AddUserComponent);
    component = fixture.componentInstance;
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should display First Name, Last Name inputs', async () => {
    const firstName = fixture.nativeElement.querySelector('input[formcontrolname="firstName"]');
    const lastName = fixture.nativeElement.querySelector('input[formcontrolname="lastName"]');

    await expect(firstName).not.toBeNull()
    await expect(lastName).not.toBeNull()

  });


  it('should be submitable', async () => {
    spyOn(component, 'send');

    await fixture.detectChanges();

    const firstName = fixture.nativeElement.querySelector('input[formcontrolname="firstName"]');
    const lastName = fixture.nativeElement.querySelector('input[formcontrolname="lastName"]');

    firstName.value = 'Hieu'; firstName.dispatchEvent(new Event('input'));
    lastName.value = 'Le'; lastName.dispatchEvent(new Event('input'));

    await fixture.detectChanges();

    const sendBtn = fixture.nativeElement.querySelector('button[type="button"]');

    await expect(sendBtn).not.toBeNull()

    await expect(sendBtn.innerText).toEqual('Create new user')

    sendBtn.click()

    await fixture.whenStable().then(async () => {
      await expect(component.send).toHaveBeenCalled();

    });

  });


  it('should not be submitable', async () => {
    spyOn(component, 'send');

    await fixture.detectChanges();

    const firstName = fixture.nativeElement.querySelector('input[formcontrolname="firstName"]');
    const lastName = fixture.nativeElement.querySelector('input[formcontrolname="lastName"]');

    firstName.value = 'Hieu'; firstName.dispatchEvent(new Event('input'));
    firstName.value = ''; firstName.dispatchEvent(new Event('input'));

    await fixture.detectChanges();

    const sendBtn = fixture.nativeElement.querySelector('button[type="button"]');

    await expect(sendBtn.innerText).toEqual('Create new user')

    sendBtn.click()

    await fixture.whenStable().then(async () => {
      await expect(component.send).not.toHaveBeenCalled();

    });

  });


});
