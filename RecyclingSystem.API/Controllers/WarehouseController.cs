﻿using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RecyclingSystem.Application.Feature.Warehouses.Commonds;
using RecyclingSystem.Application.Feature.Warehouses.Query;

namespace RecyclingSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WarehouseController : ControllerBase
    {
        private readonly IMediator _mediator;
        public WarehouseController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddWarehouse([FromBody] AddWarehouseCommond commond)
        {
            var message = await _mediator.Send(commond);
            return Ok(new {  message });
        }

        
        [HttpGet]
        [Authorize(Roles = "Admin, Employee")]
        public async Task<IActionResult> GetAllWarehouses()
        {
            var warehouses = await _mediator.Send(new GetAllWarehouseQuery());
            return Ok(warehouses);
        }
    }
}
