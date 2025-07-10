using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Behaviors;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Application.Feature.Reports.Command;
using RecyclingSystem.Application.Feature.Reports.Query;

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

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet]
        public async Task<IActionResult> GetAllReports([FromQuery] string? status)
        {
            var query = new GetAllReportsQuery { Status = status };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return BadRequest(result);
        }

        [Authorize(Roles = "Admin,Employee")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetReportById(int id)
        {
            var query = new GetReportByIdQuery { Id = id };
            var result = await mediator.Send(query);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("respond")]
        public async Task<IActionResult> RespondToReport(AdminReportRespondCommand request)
        {
            if (ModelState.IsValid)
            {
                var command = new AdminReportRespondCommand { ReportId = request.ReportId, Status = request.Status, ResponseMessage = request.ResponseMessage };
                var result = await mediator.Send(command);
                if (result.IsSuccess)
                    return Ok(result);
                return BadRequest(result);
            }
            return BadRequest(ModelState);
        }
        
        
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReport(int id)
        {
            var command = new DeleteReportCommand { ReportId = id };
            var result = await mediator.Send(command);
            if (result.IsSuccess)
            {
                return Ok(result);
            }
            return NotFound(result);
        }

    }
}
