using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecyclingSystem.Domain.Enums
{
    public enum ErrorCode
    {
        None = 0,

        ValidationError = 1,

        UnExcepectedError = 2,

        Unauthorized = 3,

        ServerError = 4,

        Conflict = 5,

        BadRequest = 6,

        NotFound = 7
    }
}
