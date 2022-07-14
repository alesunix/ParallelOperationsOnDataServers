using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParallelOperationsOnDataServers.Extensions
{
    public static class DateTimeExtensions
    {
        public static bool Between(this DateTime serverDate, DateTime startDate, DateTime endDate)
        {
            return serverDate >= startDate && serverDate <= endDate;
        }
    }
}
