using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using FamilyExpenseTrakerService.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FamilyExpenseTrakerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FamilyMembersController : ControllerBase
    {
        private readonly FamilyExpenseTrackerContext _context;
        private readonly UserManager<FamilyMember> _userManager;

        public FamilyMembersController(FamilyExpenseTrackerContext context,UserManager<FamilyMember> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: api/FamilyMembers
        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<object> GetFamilyMembers()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            int? familyId = _context.FamilyMembers.FirstOrDefault(fm => fm.Id == userId).FamilyId;
            var data = _context.FamilyMembers.Where(fm=>fm.FamilyId == familyId).Select(fm=>new { fm.Id,fm.UserName,fm.MobileNo,fm.Work,fm.Income });
            return data;
        }

        // POST: api/FamilyMembers
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme,Roles = "Admin")]
        public async Task<ActionResult<FamilyExpense>> PostFamilyMember(NewFamilyMemberModel newfamilyMember)
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            int? familyId = _context.FamilyMembers.FirstOrDefault(fm => fm.Id == userId).FamilyId;
            var familyMember = new FamilyMember
            {
                UserName = newfamilyMember.UserName,
                MobileNo = newfamilyMember.MobileNo,
                Work = newfamilyMember.Work,
                Income = newfamilyMember.Income,
                FamilyId = familyId
            };
            //Insert new user into AspNetUsers table
            var createResult = await _userManager.CreateAsync(familyMember, newfamilyMember.Password);

            //Make the FamilyMember role as User
            await _userManager.AddToRoleAsync(familyMember, "User");

            return Ok(createResult);
        }

        // PUT: api/FamilyMembers/5
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<ActionResult<FamilyExpense>> PutFamilyMember(string id,NewFamilyMemberModel editedFamilyMember)
        {
            var familyMember = new FamilyMember
            {
                Id = id,
                UserName = editedFamilyMember.UserName,
                MobileNo = editedFamilyMember.MobileNo,
                Work = editedFamilyMember.Work,
                Income = editedFamilyMember.Income,
            };
            try
            {
                await _userManager.UpdateSecurityStampAsync(familyMember);
                //Update user into AspNetUsers table
                var updateResult = await _userManager.UpdateAsync(familyMember);
                return Ok(updateResult);
            }

            catch(Exception ex)
            {
                throw ex;
            }
            
        }
    }
}