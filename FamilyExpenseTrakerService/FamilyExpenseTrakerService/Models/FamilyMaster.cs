using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyExpenseTrakerService.Models
{
    public class FamilyMaster
    {
        public int FamilyId { get; set; }
        public string FamilyName { get; set; }

        public virtual ICollection<FamilyMember> FamilyMembers { get; set; }
    }
}
