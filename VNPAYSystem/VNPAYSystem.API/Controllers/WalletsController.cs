using Microsoft.AspNetCore.Mvc;
using VNPAYSystem.Common.DTOs;
using VNPAYSystem.Data.Models;
using VNPAYSystem.Service;

namespace VNPAYSystem.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletsController : ControllerBase
    {
        private readonly IWalletService _walletService;

        public WalletsController(IWalletService walletService)
        {
            _walletService = walletService;
        }

        // GET: api/Wallets
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Wallet>>> GetWallets()
        {
            return await _walletService.GetAll();
        }

        // GET: api/Wallets/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Wallet>> GetWallet(int id)
        {
            var Wallet = await _walletService.GetById(id);

            if (Wallet == null)
            {
                return NotFound();
            }

            return Wallet;
        }

        // PUT: api/Wallets/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Wallet>> PutWallet(int userId, WalletDto Wallet)
        {
            if (userId != Wallet.UserId)
            {
                return BadRequest();
            }

            var result = await _walletService.Update(userId, Wallet);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // POST: api/Wallets
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Wallet>> PostWallet(WalletDto Wallet)
        {

            var result = await _walletService.Create(Wallet);

            if (result == null)
            {
                return NotFound();
            }

            return result;
        }

        // DELETE: api/Wallets/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<int>> DeleteWallet(int id)
        {
            var result = await _walletService.Delete(id);

            if (result < 0)
            {
                return 0;
            }

            return 1;
        }
    }
}
