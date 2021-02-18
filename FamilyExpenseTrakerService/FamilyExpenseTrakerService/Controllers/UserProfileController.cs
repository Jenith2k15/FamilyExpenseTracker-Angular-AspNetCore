using System;
using System.Collections.Generic;
using System.Linq;
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
    public class UserProfileController : ControllerBase
    {
        private readonly UserManager<FamilyMember> _userManager;

        public UserProfileController(UserManager<FamilyMember> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        //GET : /api/FamilyMember
        public async Task<Object> GetUserProfile()
        {
            string userId = User.Claims.First(c => c.Type == "UserID").Value;
            var userData = await _userManager.FindByIdAsync(userId);
            var userRole = await _userManager.GetRolesAsync(userData);
            bool? isAdmin = userRole[0]=="Admin"?true:false;
            return new { userData.UserName,userData.Email,isAdmin };
        }
        
        //[HttpGet]
        //[Authorize(Roles = "Customer")]
        //[Route("ForCustomer")]
        //public string GetCustomer()
        //{
        //    return "Web method for Customer";
        //}

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles ="Admin")]
        [Route("ForAdmin")]
        public string GetForAdmin()
        {
            return "Web method for Admin";
        }

        [HttpGet]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [Route("ForUser")]
        public string GetForUser()
        {
            return "Web method for User";
        }
    }
}