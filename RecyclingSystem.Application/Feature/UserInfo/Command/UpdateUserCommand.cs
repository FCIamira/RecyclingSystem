using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.NotificationsDTOs;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.UserInfo.Command
{
    public class UpdateUserCommand : IRequest<string>
    {
        [MinLength(3, ErrorMessage = "Name must be at least 3 characters.")]
        [MaxLength(20, ErrorMessage = "Name cannot exceed 20 characters.")]
        public string? FullName { get; set; }

        [EmailAddress]
        public string? Email { get; set; }
        public string? Address { get; set; }

        [MaxLength(11, ErrorMessage = "Phone Number cannot exceed 11 number.")]
        public string? PhoneNumber { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }

    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, string>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _contextAccessor;
        public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IMapper mapper, 
            ILogger<UpdateUserCommandHandler> logger, IMediator mediator, IHttpContextAccessor contextAccessor)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _logger = logger;
            _mediator = mediator;
            _contextAccessor = contextAccessor;
        }

        public async Task<string> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Update user information");
            try
            {
                var User = _contextAccessor.HttpContext.User;
                if (!User.Identity.IsAuthenticated)
                {
                    _logger.LogError("User is not authenticated.");
                    return "User is not authenticated.";
                }
                int userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value);

                var user = await _unitOfWork.applicationUser.GetById(userId);
                if (user == null)
                {
                    _logger.LogWarning("User not found.");
                    return "User not found.";
                }

                user.FullName = request.FullName ?? user.FullName;
                user.Email = request.Email ?? user.Email;
                user.Address = request.Address ?? user.Address;
                user.PhoneNumber = request.PhoneNumber ?? user.PhoneNumber;
                
                await _unitOfWork.applicationUser.Update(userId, user);
                await _unitOfWork.SaveChangesAsync();
                return "Update user success";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while update user information.");
                return $"Error: {ex.Message}";
            }
        }
    } 
}
