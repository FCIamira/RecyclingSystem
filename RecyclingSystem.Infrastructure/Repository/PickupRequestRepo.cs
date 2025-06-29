﻿using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Domain.Models;
using RecyclingSystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Infrastructure.Repository
{
    public class PickupRequestRepo : GenericRepo<PickupRequest, int>, IPickupRequest
    {
        private readonly RecyclingContext context;

        public PickupRequestRepo(RecyclingContext _context) : base(_context)
        {
            context = _context;
        }
    }
}
