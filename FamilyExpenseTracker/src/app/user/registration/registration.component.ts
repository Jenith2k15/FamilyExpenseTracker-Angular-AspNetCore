import { Component, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { UserService } from 'src/app/shared/user.service';

@Component({
  selector: 'app-registration',
  templateUrl: './registration.component.html',
  styleUrls: ['./registration.component.css']
})
export class RegistrationComponent implements OnInit {
  showLoadingIndiacator = false;

  constructor(public service: UserService, private toastr: ToastrService) { }

  ngOnInit() {
    this.service.formModel.reset();
  }

  onSubmit() {
    this.showLoadingIndiacator = true;

    this.service.register().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.showLoadingIndiacator = false;
          this.service.formModel.reset();
          this.toastr.success('New user created!', 'Registration successful.');
        } else {
          res.errors.forEach(element => {
            console.log('Hai');
            switch (element.code) {
              case 'DuplicateUserName':
                this.showLoadingIndiacator = false;
                console.log(element.code)
                this.toastr.error('Username is already taken','Registration failed.');
                break;

              default:
                this.showLoadingIndiacator = false;
              this.toastr.error(element.description,'Registration failed.');
                break;
            }
          });
        }
      },
      err => {
        console.log(err);
      }
    );
  }


}
