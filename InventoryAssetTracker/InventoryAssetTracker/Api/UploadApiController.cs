using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAssetTracker.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadApiController : ControllerBase
    {
        [HttpPost]
        public IActionResult Upload()
        {
            return Ok();
        }
    }
}
