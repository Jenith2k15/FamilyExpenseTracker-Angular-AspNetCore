using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyExpenseTrakerService.Models
{
    public class FamilyMember : IdentityUser
    {
        public long? MobileNo { get; set; }
        public string Work { get; set; }
        public int? Income { get; set; }

        public int? FamilyId { get; set; }
        public virtual FamilyMaster FamilyMaster { get; set; }
        public virtual ICollection<FamilyExpense> FamilyExpenses { get; set; }
    }
}
