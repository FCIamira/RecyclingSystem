using RecyclingSystem.Domain.Interfaces;
using RecyclingSystem.Infrastructure.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RecyclingSystem.Domain.Models;
using System.Security.Cryptography;
namespace RecyclingSystem.Infrastructure.Repository
{
    public class FactoryOrdersRepo:GenericRepo<FactoryOrders,int>, IFactoryOrder
    {
        private readonly RecyclingContext context;

        public FactoryOrdersRepo (RecyclingContext context):base (context)
        {
            this.context = context;
        }
    }
}
