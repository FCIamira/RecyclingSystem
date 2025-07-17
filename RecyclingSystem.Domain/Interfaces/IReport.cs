using Microsoft.EntityFrameworkCore;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Interfaces
{
    public interface IReport:IGenericRepo<Report,int>
    {
        Task<List<Report>> GetAllReportWithDetailsAsync();
        Task<Report> GetReportById(int id);
    }
}
