using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyExpenseTrakerService.Models
{
    public class NewFamilyMemberModel
    {
        public string UserName { get; set; }
        public long? MobileNo { get; set; }
        public string Work { get; set; }
        public int? Income { get; set; }
        public string Password { get; set; }
    }
}
