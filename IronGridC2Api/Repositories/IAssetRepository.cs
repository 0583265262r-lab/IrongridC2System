using IronGridC2Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace IronGridC2Api.Repositories
{
    public interface IAssetRepository
    {
        Task<AssetDto?> GatByIdAsync(int id);
        Task<bool> CreateUnitAsync(UnitsDto request);
        Task<bool> UpdateAsync(int id, UpdateAssetRequest request);
        Task<bool> DeleteAsync(int id);
    }
}
