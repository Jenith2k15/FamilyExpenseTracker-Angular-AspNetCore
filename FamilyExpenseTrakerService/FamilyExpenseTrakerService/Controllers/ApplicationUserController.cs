using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using FamilyExpenseTrakerService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace FamilyExpenseTrakerService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly UserManager<FamilyMember> _userManager;
        private readonly SignInManager<FamilyMember> _singInManager;
        private readonly FamilyExpenseTrackerContext _context;
        private readonly IFamilyExpenseTrackerRepo _familyExpenseTrackerRepo;
        private readonly ApplicationSettings _appSettings;

        public ApplicationUserController(UserManager<FamilyMember> userManager, 
            SignInManager<FamilyMember> signInManager,
            FamilyExpenseTrackerContext context, 
            IOptions<ApplicationSettings> appSettings,
            IFamilyExpenseTrackerRepo familyExpenseTrackerRepo)
        {
            _userManager = userManager;
            _singInManager = signInManager;
            _context = context;
            _familyExpenseTrackerRepo = familyExpenseTrackerRepo;
            _appSettings = appSettings.Value;
        }

        [HttpPost]
        [Route("Register")]
        //POST : /api/ApplicationUser/Register
        public async Task<Object> PostApplicationUser(ApplicationNewUserModel newUser)
        {
            try
            {
                var validationResult = await _familyExpenseTrackerRepo.ValidateNewUser(newUser.FamilyName, newUser.Email);

                if(!validationResult.Succeeded)
                {
                    return Ok(validationResult);
                }

                //Insert family name into FamilyMasters table
                var familyMasterModel = new FamilyMaster()
                {
                    FamilyName = newUser.FamilyName
                };
                _context.FamilyMasters.Add(familyMasterModel);
                _context.SaveChanges();

                var familyMember = new FamilyMember()
                {
                    UserName = newUser.UserName,
                    Email = newUser.Email,
                    FamilyId = _context.FamilyMasters.FirstOrDefault(f => f.FamilyName == newUser.FamilyName).FamilyId
                };

                //Insert new user into AspNetUsers table
                var createResult = await _userManager.CreateAsync(familyMember, newUser.Password);

                //Make first user as Admin in AspNetRoles table if user is 1st user of FamilyName is Unique
                familyMember = await _userManager.FindByIdAsync(familyMember.Id);
                int firstUserForFamily = _context.FamilyMembers.Where(fm => fm.FamilyId == familyMember.FamilyId ).Count();
                if (firstUserForFamily == 1)
                {
                    await _userManager.AddToRoleAsync(familyMember, "Admin");
                }
                else
                {
                    await _userManager.AddToRoleAsync(familyMember, "User");
                }
                return Ok(createResult);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [HttpPost]
        [Route("Login")]
        //POST : /api/ApplicationUser/Login
        public async Task<IActionResult> Login(LoginModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            IdentityOptions options = new IdentityOptions();
            //Get role assigned to the user
            var role = await _userManager.GetRolesAsync(user);

            try
            { 
            if (user != null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new Claim[]
                    {
                        new Claim("UserID",user.Id.ToString()),
                        new Claim(options.ClaimsIdentity.RoleClaimType,role.FirstOrDefault() == null?"User":role.FirstOrDefault())
                    }),
                    Expires = DateTime.UtcNow.AddDays(1),
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_appSettings.JWT_Secret)), SecurityAlgorithms.HmacSha256Signature)
                };
                var tokenHandler = new JwtSecurityTokenHandler();
                var securityToken = tokenHandler.CreateToken(tokenDescriptor);
                var token = tokenHandler.WriteToken(securityToken);
                return Ok(new { token });
            }
            else
                return BadRequest(new { message = "Username or password is incorrect." });
            }

            catch(Exception ex)
            {

            }

            return null;
        }
    }
}
