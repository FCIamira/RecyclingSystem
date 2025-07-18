﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Common
{
    public class BaseModel<T>
    {
        [Key]
        public T Id { get; set; }
        public bool IsDeleted { get; set; } = false;
        public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    }
}
