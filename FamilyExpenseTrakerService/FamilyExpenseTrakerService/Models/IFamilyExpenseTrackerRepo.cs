using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyExpenseTrakerService.Models
{
    public interface IFamilyExpenseTrackerRepo
    {
        Task<IdentityResult> ValidateNewUser(string familyName,string email);
       //Task<IdentityResult> AddNewUser(ApplicationNewUserModel newUser);
    }
}
