import { AfterViewInit, ViewChild } from '@angular/core';
import { Component, OnInit } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { ToastrService } from 'ngx-toastr';
import { FamilyMemberService } from 'src/app/shared/family-member.service';

@Component({
  selector: 'app-family-member',
  templateUrl: './family-member.component.html',
  styleUrls: ['./family-member.component.css']
})
export class FamilyMemberComponent implements OnInit,AfterViewInit {
  displayedColumns: string[]; 
  dataSource; 
  
  @ViewChild(MatPaginator) paginator: MatPaginator;

  ngAfterViewInit() {
  }

  constructor(public service: FamilyMemberService,
    private toastr: ToastrService) {
  }

  ngOnInit(): void {
    this.service.refreshList()
                .subscribe((val)=>
                {
                  let ELEMENT_DATA: FamilyMember[];
                  ELEMENT_DATA = val;
                  this.displayedColumns = ['userName', 'mobileNo', 'work', 'income','id','action'];
                  this.dataSource = new MatTableDataSource<FamilyMember>(ELEMENT_DATA);
                  this.dataSource.paginator = this.paginator;
                });
  }

  onSubmit(form: NgForm) {
    if(this.service.formData.id === ''){
      this.insertRecord(form);
    }
    else{
      this.updateRecord(form);
    }
  }

  clearForm(form:NgForm){
    alert('Hai');
    console.log(form);
    this.resetForm(form);
    this.service.formData.id = '';
  }

  insertRecord(form:NgForm){
    // console.log(formData.value);
    this.service.postFamilyMember().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.toastr.success('New user created!', 'Registration successful.');
          this.resetForm(form);
          console.log(res);
          this.ngOnInit();
        } else {
          res.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                console.log(element.code)
                this.toastr.error('Username is already taken', 'Registration failed.');
                break;

              default:
                this.toastr.error(element.description, 'Registration failed.');
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

  updateRecord(form:NgForm){
    
    this.service.putFamilyMember().subscribe(
      (res: any) => {
        if (res.succeeded) {
          this.toastr.success('User updated!', 'Updation successful.');
          this.resetForm(form);
          console.log(res);
          this.ngOnInit();
        } else {
          res.errors.forEach(element => {
            switch (element.code) {
              case 'DuplicateUserName':
                console.log(element.code)
                this.toastr.error('Username is already taken', 'Registration failed.');
                break;

              default:
                this.toastr.error(element.description, 'Registration failed.');
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

  populateForm(id,userName,mobileNo,work,income){
    console.log(id);
    this.service.formData.id = id;
    this.service.formData.userName = userName;
    this.service.formData.mobileNo = mobileNo;
    this.service.formData.work = work;
    this.service.formData.income = income;
  }

  resetForm(form: NgForm) {
    form.resetForm()
  }

}

export interface FamilyMember{
  id:string;
  userName:string;
  mobileNo:string;
  work:string;
  income:string;
}

// let ELEMENT_DATA: FamilyMember[] = [
//   {id:'jkasjndaw',userName:'Jimmy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},  
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
//   {id:'jkasjndaw',userName:'Lucy',mobileNo:'9994402229',work:'Doctor',income:'250000'},
// ];
