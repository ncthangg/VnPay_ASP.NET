using Microsoft.AspNetCore.Mvc;
using VNPAYSystem.Common.DTOs.Response;
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
        public async Task<ActionResult<PaymentRes>> CreatePayment(string orderCode)
        {

            var result = await _paymentService.Create(orderCode);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }


    }
}
