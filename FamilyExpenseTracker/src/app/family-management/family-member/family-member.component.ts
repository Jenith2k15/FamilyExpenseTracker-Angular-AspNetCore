import { Component, OnInit } from '@angular/core';
import { FormControl, NgForm } from '@angular/forms';
import { FamilyMemberService } from 'src/app/shared/family-member.service';

export interface PeriodicElement {
  id:number;
  userName: string;
  mobileNo: number;
  work: string;
  income: number;
}

const ELEMENT_DATA: PeriodicElement[] = [
  {id: 1, userName: 'Manju', mobileNo: 98986523225, work: 'Engineer',income:50000},
];

@Component({
  selector: 'app-family-member',
  templateUrl: './family-member.component.html',
  styleUrls: ['./family-member.component.css']
})
export class FamilyMemberComponent implements OnInit {

  constructor(public service:FamilyMemberService) { }

  ngOnInit(): void {
  }

  displayedColumns: string[] = ['id', 'userName', 'mobileNo', 'work', 'income'];
  dataSource = ELEMENT_DATA;

  onSubmit(formData:NgForm){
    console.log(formData.value);
  }

}
