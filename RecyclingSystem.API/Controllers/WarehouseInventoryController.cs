using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.Warehouses.Commonds;
using RecyclingSystem.Application.Feature.WarehousingInventory.Commands;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseInventoryController : ControllerBase
    {
       private readonly IMediator mediator;
        public WarehouseInventoryController( IMediator mediator) { 
         this.mediator = mediator;
        }

       [ HttpPost]
 public async Task<IActionResult> addWarehouseInventory( int materialId,int quantity)
        {

            var query=new AddWharehosingInventoryCommand { MaterialId = materialId,Quantity = quantity };
            var result=await mediator.Send(query);
            if(result.IsSuccess) 
            return Ok(result);

            return BadRequest(result);

        }
    }
}
