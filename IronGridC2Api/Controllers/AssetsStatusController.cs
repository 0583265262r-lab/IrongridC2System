using IronGridC2Api.Models;
using IronGridC2Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IronGridC2Api.Controllers
{

    [ApiController]
    [Route("api/assets-status")]
    public class AssetsStatusController : ControllerBase
    {
        private readonly IAssetStatusRepository _repo;
        public AssetsStatusController(IAssetStatusRepository repo)
        {
            _repo = repo;
        }


        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var assets = await _repo.GetAllAsync();
            return Ok(assets);
        }
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var assetStatus = await _repo.GetByIdCachedAsync(id);
            if (assetStatus == null)
                return NotFound();
            return Ok(assetStatus);
        }
        [HttpGet("status/")]
        public async Task<IActionResult> GetByStatusAsync([FromQuery] string? status)
        {
            if (!string.IsNullOrWhiteSpace(status) && status is not "Stable" and not "Warning")
                return BadRequest();

            var assets = await _repo.GetByStatusAsync(status);
            return Ok(assets);
        }
    }


}
