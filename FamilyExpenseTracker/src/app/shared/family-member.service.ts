import { Injectable } from '@angular/core';
import { FamilyMember } from './family-member.model';

@Injectable({
  providedIn: 'root'
})
export class FamilyMemberService {

  formData:FamilyMember = new FamilyMember();

  constructor() { }
}
