using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Personnel.Api.Statics;
using Personnel.Application.Interfaces;
using Personnel.Domain.Dtos.Users;
using Personnel.Domain.Entities.Identity;

namespace Personnel.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AccountController : ControllerBase
    {

        //private IAddressService AddressService { get; }
        private readonly ILogger<AccountController> _logger;
        private readonly UserManager<User> _userManger;
        private readonly SignInManager<User> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IMapper _mapper;
        public readonly ITokenService _tokenService;

        public AccountController(ILogger<AccountController> logger, UserManager<User> userManger,
            SignInManager<User> signInManager,
            RoleManager<IdentityRole> roleManager,
            ITokenService tokenService,
            IMapper mapper)
        {
            //AddressService = addressService;
            _logger = logger;
            _userManger = userManger;
            _signInManager = signInManager;
            _mapper = mapper;
            _roleManager = roleManager;
            _tokenService = tokenService;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration Attempt for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest($"Not Found {ModelState}");

            }
            try
            {
                var user = _mapper.Map<User>(userDto);

                user.UserName = userDto.UserName;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.PhoneNumber = userDto.PhoneNumber;
                var userFinded = await _userManger.FindByEmailAsync(user.Email);
                if (userFinded == null)
                {

                    var result = await _userManger.CreateAsync(user, userDto.Password);

                    if (!result.Succeeded)
                    {
                        // Role creation successful
                        foreach (var error in result.Errors)
                        {
                            _logger.LogError($"User Creation Went Wrong {error}");
                        }
                        return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
                    }

                }
                else if (userFinded.Email == userDto.Email)
                {
                    return BadRequest($"User Already Exists {userDto.Email}");
                }
                return Ok(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the Regiter Method{nameof(Register)}");

                //throw new EntityNotFoundException("Entity Not found", ex.InnerException);
                //return StatusCode(500,$"Internal Server Error Please Try Again ");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserDto userDto)
        {
            _logger.LogInformation($"Registration Attempt for {userDto.Email}");
            if (!ModelState.IsValid)
            {
                return BadRequest($"Not Found {ModelState}");

            }
            try
            {
                var result = await _signInManager.PasswordSignInAsync(userDto.UserName, userDto.Password, false, false);


                if (!result.Succeeded)
                {
                    return Unauthorized($"Unauthorized {userDto} UserName Or Password Is Wrong");
                }
                else
                {
                    var tempUser = await _userManger.FindByEmailAsync(userDto.Email);
                    var tempUserClaims = _userManger.GetClaimsAsync(tempUser).Result.ToList();
                    userDto.Claims = new List<UserClaimsDto>();
                    foreach (var claim in tempUserClaims)
                    {
                        userDto.Claims.Add(new UserClaimsDto()
                        {
                            ClaimType = claim.Type,
                            ClaimValue = claim.Value
                        });
                    }
                    string role = _userManger.GetRolesAsync(tempUser).Result.FirstOrDefault() ?? "";
                    if (role == UserRoles.Admin)
                    {
                        userDto.Claims.Add(new UserClaimsDto() { ClaimType = UserRoles.Admin, ClaimValue = role });
                    }
                    if (role == UserRoles.User)
                    {
                        userDto.Claims.Add(new UserClaimsDto() { ClaimType = UserRoles.User, ClaimValue = role });
                    }
                    userDto.FirstName = tempUser.FirstName;
                    userDto.LastName = tempUser.LastName;
                    userDto.Role = role;
                    userDto.Email = tempUser.Email;
                    userDto.Token = _tokenService.CreateToken(tempUser);

                }

                return Accepted(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the Regiter Method{nameof(Register)}");

                //throw new EntityNotFoundException("Entity Not found", ex.InnerException);
                //return StatusCode(500,$"Internal Server Error Please Try Again ");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }

        }



        [HttpGet]
        [Route("UserCount")]

        public async Task<IActionResult> UserCount()
        {
            try
            {
                var userCount = await _userManger.Users.CountAsync();
                return Ok(userCount);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the Regiter Method{nameof(Register)}");
                //throw new EntityNotFoundException("Entity Not found", ex.InnerException);
                //return StatusCode(500,$"Internal Server Error Please Try Again ");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }

        }


        [HttpPost]
        [Route("DeleteUser")]
        public async Task<IActionResult> DeleteUser(string UserId)
        {

            try
            {
                var user = await _userManger.FindByIdAsync(UserId);
                if (user == null)
                {
                    return NotFound("User Not Found");
                }

                var result = await _userManger.DeleteAsync(user);
                if (result.Succeeded)
                {
                    return Ok("User Deleted Successfully");
                }
                else
                {
                    return BadRequest("Failed To Delete User");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the Regiter Method{nameof(Register)}");
                //throw new EntityNotFoundException("Entity Not found", ex.InnerException);
                //return StatusCode(500,$"Internal Server Error Please Try Again ");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }

        }



        [HttpPost]
        [Route("UpdateUser")]
        public async Task<IActionResult> UpdateUser(string UserId, [FromBody] UserUpdateDto userUpdateModel)
        {

            try
            {
                var user = await _userManger.FindByIdAsync(UserId);
                if (user == null)
                {
                    return NotFound("User Not Found");
                }
                user.FirstName = userUpdateModel.FirstName;
                user.LastName = userUpdateModel.LastName;
                user.PhoneNumber = userUpdateModel.PhoneNumber;
                user.Email = userUpdateModel.Email;

                var result = await _userManger.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return Ok("User Deleted Successfully");
                }
                else
                {
                    return BadRequest("Failed To Delete User");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the Regiter Method{nameof(Register)}");
                //throw new EntityNotFoundException("Entity Not found", ex.InnerException);
                //return StatusCode(500,$"Internal Server Error Please Try Again ");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }

        }


        [HttpPost]
        [Route("CreateUserRole")]
        public async Task<IActionResult> CreateUserRole(string UserId, [FromBody] UserDto userDto)
        {

            try
            {
                var user = _mapper.Map<User>(userDto);

                if (!await _roleManager.RoleExistsAsync(userDto.Role))
                {
                    var newrole = new IdentityRole(userDto.Role);
                    var RoleResult = await _roleManager.CreateAsync(newrole);
                    await _userManger.AddToRoleAsync(user, userDto.Role);

                    if (!RoleResult.Succeeded)
                    {
                        // Role creation successful
                        foreach (var error in RoleResult.Errors)
                        {
                            _logger.LogError($"Role Creation Went Wrong {error}");
                        }
                        return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
                    }
                }
                else
                {
                    _logger.LogError($"Role already Exists...{nameof(Register)}");
                    return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 409);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the Regiter Method{nameof(Register)}");
                //throw new EntityNotFoundException("Entity Not found", ex.InnerException);
                //return StatusCode(500,$"Internal Server Error Please Try Again ");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
            return Ok($"Role Created {userDto.Role}");
        }



        [HttpPost]
        [Route("UpdateUserRole")]
        [Authorize(Roles = UserRoles.Admin)]
        public async Task<IActionResult> UpdateUserRole(string UserId, [FromBody] UserDto userDto)
        {

            try
            {
                var user = _mapper.Map<User>(userDto);

                if (await _roleManager.RoleExistsAsync(userDto.Role))
                {
                    var newrole = new IdentityRole(userDto.Role);
                    var RoleResult = await _roleManager.UpdateAsync(newrole);
                    if (!RoleResult.Succeeded)
                    {
                        // Role creation successful
                        foreach (var error in RoleResult.Errors)
                        {
                            _logger.LogError($"Role Creation Went Wrong {error}");
                        }
                        return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
                    }
                }
                else
                {
                    _logger.LogError($"Role already Exists...{nameof(Register)}");
                    return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 409);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went Wrong in the Regiter Method{nameof(Register)}");
                //throw new EntityNotFoundException("Entity Not found", ex.InnerException);
                //return StatusCode(500,$"Internal Server Error Please Try Again ");
                return Problem($"Something Went Wrong in the {nameof(Register)}", statusCode: 500);
            }
            return Ok($"Role Created {userDto.Role}");
        }

        //////////-----------------------------------------------------------------------------------------------
        //[HttpPost]
        //public async Task<ActionResult<UserDto>> Login([FromBody] LoginBindingModel model) 
        //{
        //    try
        //    {
        //        var result = await _signInManager.PasswordSignInAsync(model.Email,model.Password,false,false);
        //        var userDto = new UserDto();
        //        if (result != null && result.Succeeded)
        //        {
        //            var tempUser = await _userManger.FindByEmailAsync(model.Email);
        //            var tempUserClaims = _userManger.GetClaimsAsync(tempUser).Result.ToList();
        //            userDto.Claims = new List<UserClaimsDto>();
        //            foreach (var claim in tempUserClaims)
        //            {
        //                userDto.Claims.Add(new UserClaimsDto()
        //                {
        //                    ClaimType = claim.Type,
        //                    ClaimValue = claim.Value
        //                });
        //            }
        //            string role = _userManger.GetRolesAsync(tempUser).Result.FirstOrDefault() ?? "";
        //            if (role != UserRoles.Admin)
        //            {
        //                userDto.Claims.Add(new UserClaimsDto() { ClaimType = UserRoles.Admin, ClaimValue = role });
        //            }
        //            if (role != UserRoles.User)
        //            {
        //                userDto.Claims.Add(new UserClaimsDto() { ClaimType = UserRoles.User, ClaimValue = role });
        //            }
        //            userDto.FirstName = tempUser.FirstName;
        //            userDto.LastName = tempUser.LastName;
        //            userDto.Role = role;
        //            userDto.Email = tempUser.Email;
        //            return userDto;
        //        }
        //        else
        //        {
        //            _logger.LogError("Email or PassWord Is Incorrect...");
        //            return BadRequest("Email or PassWord Is Incorrect");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogError("Bad Request Login...");
        //        throw new EntityNotFoundException("Entity Not found", ex.InnerException);
        //        return BadRequest(ex.Message);
        //    }
        //}

    }

}
