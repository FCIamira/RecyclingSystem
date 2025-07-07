using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Application.Feature.Reports.Command;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReportController : ControllerBase
    {
        private readonly IMediator mediator;

        public ReportController(IMediator mediator) { 
        this.mediator = mediator;   
        
        }

        [Authorize(Roles = "Employee")]
        [HttpPost]
        public async Task<IActionResult> CreateReport(CreateReportDTO reportDTO)
        {
            if (ModelState.IsValid)
            {
                var command = new AddNewReportCommand { Report = reportDTO };
                var result = await mediator.Send(command);

                if (result.IsSuccess)
                    return Ok(result);

                return BadRequest(result);
            }

           
            return BadRequest(ModelState);
        }

    }
}
