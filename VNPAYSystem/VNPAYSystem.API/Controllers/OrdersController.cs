using Microsoft.AspNetCore.Mvc;
using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Common.DTOs.Request;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Service;

namespace VNPAYSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // GET: api/Orders
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            return await _orderService.GetAll();
        }

        // GET: api/Orders/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Order>> GetOrder(int id)
        {
            var order = await _orderService.GetById(id);

            if (order == null)
            {
                return NotFound();
            }

            return order;
        }

        // PUT: api/Orders/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Order>> PutOrder(int id, OrderDto order)
        {
            if (id != order.Id)
            {
                return BadRequest();
            }

            var result = await _orderService.Update(id, order);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // POST: api/Orders
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Order>> PostOrder(OrderReq order)
        {

            var result = await _orderService.Create(order);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // DELETE: api/Orders/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteOrder(int id)
        {
            var result = await _orderService.Delete(id);

            if (result < 0)
            {
                return 0;
            }

            return 1;
        }

    }
}
