using IronGridC2Api.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace IronGridC2Api.Controllers
{
    [ApiController]
    [Route("api/reports")]
    public class OperationsReportsController : ControllerBase
    {
        private readonly IAssetStatusRepository _repo;

        public OperationsReportsController(IAssetStatusRepository repo)
        {
            _repo = repo;
        }

        [HttpGet("critical-assets")]
        public async Task<ActionResult> GetCriticalAssets()
        {
            var assets = await _repo.GetCriticalAssetsAsync();
            return Ok(assets);
        }
        [HttpGet("unit/{unitId}/assets")]
        public async Task<ActionResult> GetAssetsByUnit(int unitId)
        {
            if (unitId <= 0)
                return BadRequest();

            var result = await _repo.GetAssetsByUnitAsync(unitId);
            if (result == null)
                return NotFound();
            return Ok(result);
        }
        [HttpGet("summary-by-unit")]
        public async Task<IActionResult> GetSummaryByUnit()
        {
            var summary = await _repo.GetSummaryByUnitAsync();
            return Ok(summary);
        }
    }
}
