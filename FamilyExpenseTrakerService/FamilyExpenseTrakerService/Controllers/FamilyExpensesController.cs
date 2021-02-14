using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FamilyExpenseTrakerService.Models;

namespace FamilyExpenseTrakerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyExpensesController : ControllerBase
    {
        private readonly FamilyExpenseTrackerContext _context;

        public FamilyExpensesController(FamilyExpenseTrackerContext context)
        {
            _context = context;
        }

        // GET: api/FamilyExpenses
        [HttpGet]
        public async Task<object> GetFamilyExpenses()

        {
            var data = await _context.FamilyExpenses.Include(fe=>fe.FamilyMember)
                .Select(fe=>new { fe.FamilyMember.UserName,fe.Purpose,fe.Amount }).ToListAsync();
            return data;
        }

        // GET: api/FamilyExpenses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<FamilyExpense>> GetFamilyExpense(int? id)
        {
            var familyExpense = await _context.FamilyExpenses.FindAsync(id);

            if (familyExpense == null)
            {
                return NotFound();
            }

            return familyExpense;
        }

        // PUT: api/FamilyExpenses/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFamilyExpense(int? id, FamilyExpense familyExpense)
        {
            if (id != familyExpense.ExpenseId)
            {
                return BadRequest();
            }

            _context.Entry(familyExpense).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FamilyExpenseExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/FamilyExpenses
        [HttpPost]
        public async Task<ActionResult<FamilyExpense>> PostFamilyExpense(FamilyExpense familyExpense)
        {
            _context.FamilyExpenses.Add(familyExpense);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFamilyExpense", new { id = familyExpense.ExpenseId }, familyExpense);
        }

        // DELETE: api/FamilyExpenses/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<FamilyExpense>> DeleteFamilyExpense(int? id)
        {
            var familyExpense = await _context.FamilyExpenses.FindAsync(id);
            if (familyExpense == null)
            {
                return NotFound();
            }

            _context.FamilyExpenses.Remove(familyExpense);
            await _context.SaveChangesAsync();

            return familyExpense;
        }

        private bool FamilyExpenseExists(int? id)
        {
            return _context.FamilyExpenses.Any(e => e.ExpenseId == id);
        }
    }
}
