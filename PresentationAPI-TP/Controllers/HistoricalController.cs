using Application.Interfaces;

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class HistoricalController : ControllerBase
    {
        private readonly IHistoricalService _historicalService;

        public HistoricalController(IHistoricalService historicalService)
        {
            _historicalService = historicalService;
        }

        [HttpGet("user/{userId}")]
        public IActionResult GetUserHistory(int userId)
        {
            var history = _historicalService.GetUserHistory(userId);
            return Ok(history);
        }
    }
}
