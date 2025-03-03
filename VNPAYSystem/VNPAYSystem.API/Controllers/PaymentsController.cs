using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using VNPAYSystem.Common.DTOs.Response;
using VNPAYSystem.Data;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Service;

namespace VNPAYSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentsController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentsController(IPaymentService paymentService)
        {
            _paymentService = paymentService;
        }
        // GET: api/payments
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Payment>>> GetPayments()
        {
            return await _paymentService.GetAll();
        }

        // GET: api/payments/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Payment>> Getpayment(int id)
        {
            var payment = await _paymentService.GetById(id);

            if (payment == null)
            {
                return NotFound();
            }

            return payment;
        }

        [HttpGet("ipn")]
        public async Task<IActionResult> VNPayIPN()
        {
            var result = await _paymentService.ProcessVNPayIPN(Request.Query);
            if (result)
            {
                return Ok("Payment success");
            }
            return BadRequest("Payment failed");
        }

        [HttpPost]
        public async Task<ActionResult<PaymentRes>> CreatePayment(int orderId)
        {

            var result = await _paymentService.Create(orderId);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }





        //// PUT: api/payments/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<ActionResult<Payment>> PutPayment(int id, PaymentDto payment)
        //{
        //    if (id != payment.Id)
        //    {
        //        return BadRequest();
        //    }

        //    var result = await _paymentService.Update(id, payment);

        //    if (result == null)
        //    {
        //        return NotFound();
        //    }

        //    return result;
        //}

        //// DELETE: api/payments/5
        //[HttpDelete("{id}")]
        //public async Task<ActionResult<int>> DeletePayment(int id)
        //{
        //    var result = await _paymentService.Delete(id);

        //    if (result < 0)
        //    {
        //        return 0;
        //    }

        //    return 1;
        //}
    }
}
