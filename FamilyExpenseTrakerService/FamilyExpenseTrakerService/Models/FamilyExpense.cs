using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FamilyExpenseTrakerService.Models
{
    public class FamilyExpense
    {
        public int? ExpenseId { get; set; }
        public string Purpose { get; set; }
        public int? Amount { get; set; }
        public DateTime Date { get; set; }

        public virtual FamilyMember FamilyMember { get; set; }
    }
}
