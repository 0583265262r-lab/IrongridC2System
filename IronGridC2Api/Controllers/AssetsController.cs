using Microsoft.AspNetCore.Mvc;
using IronGridC2Api.Data;
using IronGridC2Api.Repositories;
using IronGridC2Api.Models;
using Microsoft.EntityFrameworkCore;


namespace IronGridC2Api.Controllers
{

    [ApiController]
    [Route("api/Assets")]
    public class AssetsController : ControllerBase
    {
        private readonly IronGridDbContext _db;
        private readonly IAssetRepository _repo;

        public AssetsController(IAssetRepository repo, IronGridDbContext db)
        {
            _db = db;
            _repo = repo;
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id <= 0)
                return BadRequest();

            var asset = await _repo.GatByIdAsync(id);
            if (asset == null)
                return NotFound();
            return Ok(asset);
        }
        [HttpPost("units")]
        public async Task<ActionResult<bool>> Create(UnitsDto request)
        {
            var created = await _repo.CreateUnitAsync(request);

            if (!created)
                return BadRequest();

            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateAssetRequest request)
        {
            var asset = await _db.Assets.FirstOrDefaultAsync(asset => asset.Id == id);
            if (asset is null)
                return NotFound();
            if (id <= 0)
                return BadRequest();

            var result = await _repo.UpdateAsync(id, request);
            
            return Ok(result);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var asset = await _db.Assets.FirstOrDefaultAsync(asset => asset.Id == id);
            if (asset is null)
                return NotFound();

            bool deleted = await _repo.DeleteAsync(id);
            return NoContent();
        }

    }
}
