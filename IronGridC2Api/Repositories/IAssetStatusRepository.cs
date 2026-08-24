using IronGridC2Api.Models;

namespace IronGridC2Api.Repositories
{
    public interface IAssetStatusRepository
    {
        Task<List<AssetStatusDto>> GetAllAsync();
        Task<AssetStatusDto?> GetByIdCachedAsync(int id);
        Task<List<AssetStatusDto>> GetByStatusAsync(string? status = null);
        Task<List<CriticalAssetDto>> GetCriticalAssetsAsync();
        Task<List<UnitAssetReportDto>> GetAssetsByUnitAsync(int unitId);
        Task<List<UnitSummaryDto>> GetSummaryByUnitAsync();
    }
}
