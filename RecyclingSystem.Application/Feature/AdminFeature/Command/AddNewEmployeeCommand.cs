using MediatR;
using Microsoft.AspNetCore.Identity;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.AccountDTOs;
using RecyclingSystem.Application.Feature.Account.Commands;
using RecyclingSystem.Domain.Enums;
using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Feature.AdminFeature.Command
{
    public class AddNewEmployeeCommand:IRequest<Result<string>>
    {
        public RegisterEmployeeDTO Dto { get; set; }
    }

    public class AddNewEmployeeCommandHandler : IRequestHandler<AddNewEmployeeCommand, Result<string>>
    {

        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;
        private readonly IUnitOfWork _unitOfWork;

        public AddNewEmployeeCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<int>> roleManager, IUnitOfWork unitOfWork)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result<string>> Handle(AddNewEmployeeCommand request, CancellationToken cancellationToken)
        {
            var dto = request.Dto;

            // Check if user already exists
            var existingUser = await _userManager.FindByEmailAsync(dto.EmailAddress);
            if (existingUser != null)
                return Result<string>.Failure("This email is already registered.");

            // Create new user
            var employee = new ApplicationUser
            {
                UserName = $"{dto.FirstName}{dto.LastName}",
                Email = dto.EmailAddress,
                FullName = $"{dto.FirstName} {dto.LastName}",
                Address = dto.Address,
                PhoneNumber = dto.PhoneNumber,
                Role = "Employee",
                CreatedAt = DateTime.UtcNow
            };

            IdentityResult result = await _userManager.CreateAsync(employee, dto.Password);
            if (!result.Succeeded)
                return Result<string>.Failure(result.Errors.First().Description);
        
            // Ensure role exists
            if (!await _roleManager.RoleExistsAsync("Employee"))
            {
                await _roleManager.CreateAsync(new IdentityRole<int>("Employee"));
            }

            await _userManager.AddToRoleAsync(employee, "Employee");

            var warehouse = await _unitOfWork.warehouse.GetSpecificWithFilter(w => w.Name == dto.WarehouseName);
            if (warehouse != null)
            {
                return Result<string>.Failure(ErrorCode.NotFound, "Warehouse not found.");
            }

            var employeeWarehouseHistory = new EmployeeWarehouseHistory
            {
                EmployeeId = employee.Id,
                WarehouseId = warehouse.Id, 
                AssignedDate = DateTime.UtcNow
            };

            await _unitOfWork.employeeWarehouseHistory.Add(employeeWarehouseHistory);
            await _unitOfWork.SaveChangesAsync();

            return Result<string>.Success("Employee registered successfully.");
        }
    }
}
