using MedicalSystemClassLibrary.Data;
using MedicalSystemClassLibrary.Models;
using MedicalSystemClassLibrary.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MedicalSystemClassLibrary.Services
{
    public class UserService : IUserService
    {
        private readonly MedicalSystemDbContext _context;

        public UserService(MedicalSystemDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.Include(u => u.Role).ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            return await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
        }

        public async Task CreateUserAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.UserId);
            if (existingUser != null)
            {
                existingUser.Username = user.Username;
                existingUser.PwdHash = user.PwdHash;
                existingUser.PwdSalt = user.PwdSalt;
                existingUser.FirstName = user.FirstName;
                existingUser.LastName = user.LastName;
                existingUser.Email = user.Email;
                existingUser.Phone = user.Phone;
                existingUser.RoleId = user.RoleId;
                existingUser.Role = user.Role;

                _context.Users.Update(existingUser);
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteUserAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task ChangePasswordAsync(int id, string newPassword)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                user.PwdHash = newPassword;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task PromoteUserAsync(int id)
        {
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.UserId == id);
            if (user != null && user.Role.RoleName != "Admin")
            {
                var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == "Admin");
                if (adminRole != null)
                {
                    user.RoleId = adminRole.RoleId;
                    user.Role = adminRole;
                    _context.Users.Update(user);
                    await _context.SaveChangesAsync();
                }
            }
        }
    }
}

