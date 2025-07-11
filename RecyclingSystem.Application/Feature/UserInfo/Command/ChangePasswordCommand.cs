using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
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
    public class ChangePasswordCommand : IRequest<string>
    {
        public string OldPassword { get; set; }
        public string NewPassword { get; set; }
        [Compare("NewPassword")]
        public string ConfirmPassword { get; set; }
    }

    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, string>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ILogger<ChangePasswordCommandHandler> _logger;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        public ChangePasswordCommandHandler(UserManager<ApplicationUser> userManager,
            ILogger<ChangePasswordCommandHandler> logger, IMediator mediator,
            IHttpContextAccessor contextAccessor)
        {
            _userManager = userManager;
            _logger = logger;
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Change user password");
            try
            {
                var httpUser = _contextAccessor.HttpContext?.User;
                if (!httpUser.Identity.IsAuthenticated)
                {
                    _logger.LogWarning("User is not authenticated.");
                    return "User is not authenticated.";
                }

                var user = await _userManager.GetUserAsync(httpUser);

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
                _logger.LogError(ex, "An error occurred while change password.");
                return $"Error: {ex.Message}";
            }
        }
    }
}