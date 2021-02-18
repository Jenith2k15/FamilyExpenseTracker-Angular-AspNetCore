import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { FamilyMember } from './family-member.model';

@Injectable({
  providedIn: 'root'
})
export class FamilyMemberService {

  readonly baseUrl='http://localhost:54068/api/FamilyMembers';

  formData:FamilyMember = new FamilyMember();
  
  constructor(private http:HttpClient) { }

  postFamilyMember(){
    console.log(this.formData);
    return this.http.post(this.baseUrl,this.formData);
  }

  putFamilyMember(){
    console.log(this.formData);
    return this.http.put(`${this.baseUrl}/${this.formData.id}`,this.formData);
  }

  refreshList():Observable<FamilyMember[]>{
    return this.http.get<FamilyMember[]>(this.baseUrl)
  }


}
