using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VNPAYSystem.Data.Models;

namespace VNPAYSystem.Common.DTOs.Response
{
    public class PaymentRes
    {
        public Payment Payment { get; set; }
        public string Link { get; set; }

    }
}
