import { TestBed } from '@angular/core/testing';
import { HttpClientTestingModule, HttpTestingController } from '@angular/common/http/testing';
import { HttpClientModule } from '@angular/common/http';

import { UserService } from './user.service';
import { User } from './user.model';

describe('UserService', () => {
  let httpMock: HttpTestingController;
  let service: UserService;

  beforeEach(() => {

    TestBed.configureTestingModule({
      imports: [HttpClientTestingModule],

      // https://stackoverflow.com/questions/55261912/unit-test-fails-with-nullinjectorerror-in-angular?rq=1
      providers: [UserService, { provide: 'BASE_URL', useValue: 'http://localhost/' }]
    });

    httpMock = TestBed.get(HttpTestingController);
    service = TestBed.inject(UserService);
  });

  afterEach(() => {
    httpMock.verify();
  });

  //it('should be created', () => {

  //  expect(service).toBeTruthy();

  //});

  //it('should have addUser function', () => {

  //  expect(service.addUser).toBeTruthy();

  //});


  it('should run addUser function successful', async () => {

    const user = new User()
    user.firstName = 'Hieu'
    user.lastName = 'Le'

    await service.addUser(user).subscribe(async (u) => {

      await expect(u).toEqual(user);

    });

    const request = httpMock.expectOne(`http://localhost/user`);

    await expect(request.request.method).toBe('POST');

    await request.flush(user)

  });

});
