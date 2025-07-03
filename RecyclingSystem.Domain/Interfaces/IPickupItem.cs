using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace RecyclingSystem.Domain.Interfaces
{
    public interface IPickupItem:IGenericRepo<PickupItem,int>
    {
    }
}
