import { ComponentFixture, TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClientModule } from '@angular/common/http';

import { AddUserComponent } from 'src/app/user';
import { User, UserService } from 'src/app/shared';

describe('AddUserComponent', () => {
  let httpMock: HttpTestingController;
  let service: UserService;

  let component: AddUserComponent;
  let fixture: ComponentFixture<AddUserComponent>;

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],

      // https://stackoverflow.com/questions/55261912/unit-test-fails-with-nullinjectorerror-in-angular?rq=1
      providers: [UserService, { provide: 'BASE_URL', useValue: 'http://localhost/' }],

      declarations: [AddUserComponent],
    });

    httpMock = TestBed.get(HttpTestingController);
    service = TestBed.inject(UserService);

    fixture = TestBed.createComponent(AddUserComponent);
    component = fixture.componentInstance;
  });

  afterEach(() => {
    httpMock.verify();
  });

  it('should display First Name, Last Name inputs', async () => {
    const firstName = fixture.nativeElement.querySelector('input[name="firstName"]');
    const lastName = fixture.nativeElement.querySelector('input[name="lastName"]');

    await expect(firstName).not.toBeNull()
    await expect(lastName).not.toBeNull()

  });


  it('should Send button clickable', async () => {
    spyOn(component, 'send');
    spyOn(service, 'addUser');

    const sendBtn = fixture.nativeElement.querySelector('button[type="button"]');

    await expect(sendBtn).not.toBeNull()
    await expect(sendBtn.textContent).toEqual('Send')

    sendBtn.click()

    await fixture.whenStable().then(async () => {
      await expect(component.send).toHaveBeenCalled();

      await expect(service.addUser).not.toHaveBeenCalled();
    });

  });


});
