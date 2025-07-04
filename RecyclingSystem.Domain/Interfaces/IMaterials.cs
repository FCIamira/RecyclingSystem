using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Interfaces
{
    public interface IMaterials:IGenericRepo<Material,int>
    {
        Task<IQueryable<Material>> GetAllMaterialsAsync();
    }
}
