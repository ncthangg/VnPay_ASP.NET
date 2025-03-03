using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VNPAYSystem.Common.DTOs.Request
{
    public class OrderReq
    {
        public int UserId { get; set; }

        public decimal Amount { get; set; }
    }
}
