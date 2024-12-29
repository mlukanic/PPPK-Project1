using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using MedicalSystemClassLibrary.Utilities;
using MedicalSystemWebAPI.Dtos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MedicalSystemWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly MedicalSystemDbContext _context;

        public UserController(IConfiguration configuration, MedicalSystemDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        [HttpGet("[action]")]
        public ActionResult GetToken()
        {
            try
            {
                // The same secure key must be used here to create JWT,
                // as the one that is used by middleware to verify JWT
                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtProvider.CreateToken(secureKey, 10);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<UserDto> Register(UserDto userDto)
        {
            try
            {
                // Check if there is such a username in the database already
                var trimmedUsername = userDto.Username.Trim();
                if (_context.Users.Any(x => x.Username.Equals(trimmedUsername)))
                    return BadRequest($"Username {trimmedUsername} already exists");

                var userRole = _context.Roles.FirstOrDefault(x => x.RoleName == "User");

                // Hash the password
                var b64salt = PasswordHashProvider.GetSalt();
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, b64salt);

                // Create user from DTO and hashed password
                var user = new User
                {
                    UserId = userDto.Id,
                    Username = userDto.Username,
                    PwdHash = b64hash,
                    PwdSalt = b64salt,
                    FirstName = userDto.FirstName,
                    LastName = userDto.LastName,
                    Email = userDto.Email,
                    Phone = userDto.Phone,
                    RoleId = userRole.RoleId
                };

                // Add user and save changes to database
                _context.Add(user);
                _context.SaveChanges();

                // Update DTO Id to return it to the client
                userDto.Id = user.UserId;
                return Ok(userDto);

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult Login(UserLoginDto userDto)
        {
            try
            {
                var genericLoginFail = "Incorrect username or password";

                // Try to get a user from database
                var existingUser = _context.Users.FirstOrDefault(x => x.Username == userDto.Username);
                if (existingUser == null)
                    return BadRequest(genericLoginFail);

                // Check is password hash matches
                var b64hash = PasswordHashProvider.GetHash(userDto.Password, existingUser.PwdSalt);
                if (b64hash != existingUser.PwdHash)
                    return BadRequest(genericLoginFail);

                // Create and return JWT token
                var secureKey = _configuration["JWT:SecureKey"];
                var serializedToken = JwtProvider.CreateToken(secureKey, 120, userDto.Username);

                return Ok(serializedToken);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<UserChangePasswordDto> ChangePassword(UserChangePasswordDto userChangePasswordDto)
        {
            try
            {
                var trimmedUsername = userChangePasswordDto.Username.Trim();
                var existingUser = _context.Users.FirstOrDefault(x => x.Username == trimmedUsername);
                if (existingUser == null)
                {
                    return BadRequest($"User {trimmedUsername} not found");
                }

                existingUser.PwdSalt = PasswordHashProvider.GetSalt();
                existingUser.PwdHash = PasswordHashProvider.GetHash(userChangePasswordDto.Password, existingUser.PwdSalt);

                _context.Update(existingUser);
                _context.SaveChanges();

                return Ok();
            }

            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost("[action]")]
        public ActionResult<UserDto> PromoteUser(UserPromoteDto userPromoteDto)
        {
            try
            {
                var trimmedUsername = userPromoteDto.Username.Trim();
                var existingUser = _context.Users.FirstOrDefault(x => x.Username == trimmedUsername);
                if (existingUser == null)
                {
                    return BadRequest($"User with username: {trimmedUsername} not found");
                }

                var adminRole = _context.Roles.FirstOrDefault(x => x.RoleName == "Admin");

                existingUser.RoleId = adminRole.RoleId;
                _context.Update(existingUser);
                _context.SaveChanges();

                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
