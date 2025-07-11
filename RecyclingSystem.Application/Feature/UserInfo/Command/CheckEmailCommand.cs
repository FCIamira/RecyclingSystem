using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Command
{

    public class CheckEmailCommand : IRequest<string>
    {
        public string Email { get; set; }
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class CheckEmailCommandHandler : IRequestHandler<CheckEmailCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CheckEmailCommandHandler> _logger;
        public CheckEmailCommandHandler(UserManager<ApplicationUser> userManager, ILogger<CheckEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<string> Handle(CheckEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if email exists.");
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("Email not found.");
                    return "Email not found.";
                }
                var checkPass = await _userManager.CheckPasswordAsync(user, request.OldPassword);
                if (!checkPass)
                {
                    _logger.LogWarning("Old password is incorrect.");
                    return "Old password is incorrect.";
                }

                var result = await _userManager.ChangePasswordAsync(user, request.OldPassword, request.NewPassword);
                if (!result.Succeeded)
                {
                    _logger.LogWarning("Password change failed: {Errors}", string.Join(", ", result.Errors.Select(e => e.Description)));
                    return "Password change failed.";
                }

                return "Password changed successfully";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking the email.");
                return "An unexpected error occurred while processing your request.";
            }
        }
    }
}
