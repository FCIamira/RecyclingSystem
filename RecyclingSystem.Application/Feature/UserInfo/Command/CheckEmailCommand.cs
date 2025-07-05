using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Command
{

    public class CheckEmailCommand : IRequest<bool>
    {
        public string Email { get; set; }
    }

    public class CheckEmailCommandHandler : IRequestHandler<CheckEmailCommand, bool>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<CheckEmailCommandHandler> _logger;
        public CheckEmailCommandHandler(UserManager<ApplicationUser> userManager, ILogger<CheckEmailCommandHandler> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<bool> Handle(CheckEmailCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Checking if email exists.");
            try
            {
                var user = await _userManager.FindByEmailAsync(request.Email);
                if (user == null)
                {
                    _logger.LogWarning("Email not found.");
                    return false;
                }
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking the email.");
                return false;
            }
        }
    }
}
