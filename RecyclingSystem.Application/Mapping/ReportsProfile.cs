using AutoMapper;
using RecyclingSystem.Application.DTOs.ReportsDTOs;
using RecyclingSystem.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Application.Mapping
{
    public class ReportsProfile:Profile
    {
        public ReportsProfile() {
            CreateMap<CreateReportDTO, Report>();
        }
    }
}
