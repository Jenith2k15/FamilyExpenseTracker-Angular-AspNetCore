using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyExpenseTrakerService.Models
{
    public class SQLFamilyExpenseTrackerRepo : IdentityResult, IFamilyExpenseTrackerRepo
    {
        private readonly FamilyExpenseTrackerContext _context;
        private readonly UserManager<FamilyMember> _userManager;

        public SQLFamilyExpenseTrackerRepo(FamilyExpenseTrackerContext context, 
            UserManager<FamilyMember> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        //public async Task<IdentityResult> AddNewUser(ApplicationNewUserModel newUser)
        //{
            
        //}

        public async Task<IdentityResult> ValidateNewUser(string familyName,string email,string userName)
        {
            
            var familyMember = await _userManager.FindByEmailAsync(email);
            if (familyMember != null)
            {
                base.Succeeded = false;
                return Failed(new IdentityError[]
                { new IdentityError
                    {
                      Code="EmailAreadyAvailable",
                      Description=$"Email name {email} is already taken."
                    }
                });
            }

            var familyNameCount = _context.FamilyMasters.Count(fm => fm.FamilyName == familyName);
            if (familyNameCount > 0)
            {
                base.Succeeded = false;
                return Failed(new IdentityError[] 
                { new IdentityError
                    {
                      Code="FamilyNameAlreadyTaken",
                      Description=$"Family name {familyName} is already taken."
                    }
                });
            }

            var user = _context.FamilyMembers.FirstOrDefault(fm => fm.UserName == userName);

            if (user != null)
            {
                base.Succeeded = false;
                return Failed(new IdentityError[]
                { new IdentityError
                    {
                      Code="UserNameAlreadyTaken",
                      Description=$"User name {user.UserName} is already taken."
                    }
                });
            }

            return Success;
        }
    }
}
