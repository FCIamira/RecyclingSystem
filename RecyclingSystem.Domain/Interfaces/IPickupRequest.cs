using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Interfaces
{
    public interface IPickupRequest:IGenericRepo<PickupRequest,int>
    {
        Task<List<PickupRequest>> GetAllDetails();

        // get a pickup request by id with all details
        Task<PickupRequest> GetByIdWithDetails(int id);
        Task<EmployeeWarehouseHistory> GetLatestWarehouseByEmployeeIdAsync(int employeeId);

    }
}
