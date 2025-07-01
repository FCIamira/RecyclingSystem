using RecyclingSystem.Application.DTOs.AccountDTOs;
using RecyclingSystem.Application.Validators;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;


namespace RecyclingSystem.Application.Feature.Account.Commands
{
    public class RegisterCommandResponse
    {
        public ApplicationUser ApplicationUser { get; set; }
        public DateTime Expired { get; set; }
    }

    public class RegisterCommand : IRequest<Result<RegisterCommandResponse>>
    {
        public RegisterRequest registerRequest { get; set; }
    }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterCommandResponse>>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RegisterCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            if (await _userManager.Users.AnyAsync(u => u.Email == request.registerRequest.EmailAddress))
            {
                return Result<RegisterCommandResponse>.Failure(ErrorCode.Unauthorized, "Email is already registered.");
            }

            if (await _userManager.Users.AnyAsync(u => u.UserName == request.registerRequest.UserName))
            {
                return Result<RegisterCommandResponse>.Failure(ErrorCode.Unauthorized, "UserName is already registered.");
            }

            ApplicationUser user = new ApplicationUser()
            {
               
                UserName = request.registerRequest.UserName,
                Email = request.registerRequest.EmailAddress,
                FullName = $"{request.registerRequest.FirstName} {request.registerRequest.LastName}",
                //Address = request.registerRequest.Address,
            };

            string[] roles = new[] { "Admin", "Manager", "Employee", "Customer" };
            foreach (var role in roles)
            {
                if (!await _roleManager.RoleExistsAsync(role))
                {
                    await _roleManager.CreateAsync(new IdentityRole<int>(role));
                }
            }

            string assignedRole = await _userManager.Users.CountAsync() > 3 ? "Customer" : "Admin";
            user.Role = assignedRole; // Assign role based on user count


            IdentityResult result = await _userManager.CreateAsync(user, request.registerRequest.Password);
            if (!result.Succeeded)
            {
                string errorMessages = string.Join(", ", result.Errors.Select(e => e.Description));
                return Result<RegisterCommandResponse>.Failure(ErrorCode.Unauthorized, errorMessages);
            }

            
            await _userManager.AddToRoleAsync(user, assignedRole);

            DateTime expired = DateTime.Now.AddHours(3);
            return Result<RegisterCommandResponse>.Success(new RegisterCommandResponse()
            {
                ApplicationUser = user,
                Expired = expired
            });
        }
    }
}
