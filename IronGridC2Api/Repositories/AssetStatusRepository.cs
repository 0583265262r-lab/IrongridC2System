using IronGridC2Api.Data;
using IronGridC2Api.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace IronGridC2Api.Repositories
{
    public class AssetStatusRepository : IAssetStatusRepository
    {
        private readonly IronGridDbContext _db;
        public AssetStatusRepository(IronGridDbContext db)
        {
            _db = db;
        }
        public async Task<List<AssetStatusDto>> GetAllAsync()
        {
            var query = _db.Assets.AsNoTracking().AsQueryable();
            return await query
                .OrderBy(asset => asset.Id)
                .Select(asset => new AssetStatusDto
                {
                    AssetId = asset.Id,
                    AssetSerial = asset.AssetSerial,
                    AssetType = asset.AssetType,
                    UnitId = asset.UnitId,
                    UnitName = asset.Unit.UnitName,
                    Sector = asset.Unit.Sector,
                    RawValue = asset.LiveStatus.RawValue,
                    ProcessedStatus = asset.LiveStatus.ProcessedStatus,
                    IsVerified = asset.LiveStatus.IsVerified,
                    LastUpdate = asset.LiveStatus.LastUpdate
                })
                .ToListAsync();
        }
        public async Task<List<AssetStatusDto>> GetByStatusAsync(string status)
        {
            var query = _db.Assets.AsNoTracking().AsQueryable();

            if (!string.IsNullOrWhiteSpace(status))
            {
                query = query.Where(asset =>
                     asset.LiveStatus.ProcessedStatus == status);
            }

            return await query
                .OrderBy(asset => asset.Id)
                .Select(asset => new AssetStatusDto
                {
                    AssetId = asset.Id,
                    AssetSerial = asset.AssetSerial,
                    AssetType = asset.AssetType,
                    UnitId = asset.UnitId,
                    UnitName = asset.Unit.UnitName,
                    Sector = asset.Unit.Sector,
                    RawValue = asset.LiveStatus.RawValue,
                    ProcessedStatus = asset.LiveStatus.ProcessedStatus, 
                    IsVerified = asset.LiveStatus.IsVerified,
                    LastUpdate = asset.LiveStatus.LastUpdate

                })
                .ToListAsync();
        }
        public async Task<AssetStatusDto?> GetByIdCachedAsync(int id)
        {
            var assetStatus = await _db.Assets
                .AsNoTracking()
                .Where(asset => asset.Id == id)
                .Select(asset => new AssetStatusDto
                {
                    AssetId = asset.Id,
                    AssetSerial = asset.AssetSerial,
                    AssetType = asset.AssetType,
                    UnitId = asset.UnitId,
                    UnitName = asset.Unit.UnitName,
                    Sector = asset.Unit.Sector,
                    RawValue = asset.LiveStatus.RawValue,
                    ProcessedStatus = asset.LiveStatus.ProcessedStatus,
                    IsVerified = asset.LiveStatus.IsVerified,
                    LastUpdate = asset.LiveStatus.LastUpdate

                }).FirstOrDefaultAsync();

            return assetStatus;
        }
        public async Task<List<CriticalAssetDto>> GetCriticalAssetsAsync()
        {
            return await _db.Assets
                .AsNoTracking()
                .Where(asset => asset.LiveStatus.ProcessedStatus == "Warning" || !asset.LiveStatus.IsVerified)
                .OrderBy(asset => asset.Id)
                .Select(asset => new CriticalAssetDto
                {
                    AssetId = asset.Id,
                    AssetSerial = asset.AssetSerial,
                    AssetType = asset.AssetType,
                    UnitName = asset.Unit.UnitName,
                    Sector = asset.Unit.Sector,
                    ProcessedStatus = asset.LiveStatus!.ProcessedStatus,
                    IsVerified = asset.LiveStatus.IsVerified,
                    LastUpdate = asset.LiveStatus.LastUpdate

                }).ToListAsync();

        }

        public async Task<List<UnitAssetReportDto>>GetAssetsByUnitAsync(int unitId)
        {
            bool unitExists = await _db.Units
                .AsNoTracking()
                .AnyAsync(unit => unit.Id == unitId);

            if (!unitExists)
                return null;

            var assets = await _db.Assets
                .AsNoTracking()
                .Where(asset => asset.UnitId == unitId)
                .OrderBy(asset => asset.Id)
                .Select(asset => new UnitAssetReportDto
                {
                    AssetId = asset.Id,
                    AssetSerial = asset.AssetSerial,
                    AssetType = asset.AssetType,
                    ProcessedStatus = asset.LiveStatus != null ? asset.LiveStatus.ProcessedStatus : null,
                    IsVerified = asset.LiveStatus != null ? asset.LiveStatus.IsVerified : null,
                    LastUpdate = asset.LiveStatus != null ? asset.LiveStatus.LastUpdate : null

                }).ToListAsync();

            return new List<UnitAssetReportDto>(assets);
        }
        public async Task<List<UnitSummaryDto>> GetSummaryByUnitAsync()
        {
            return await _db.Units
                .AsNoTracking()
                .OrderBy(unit => unit.Id)
                .Select(unit => new UnitSummaryDto
                {
                    UnitId = unit.Id,
                    UnitName = unit.UnitName,
                    Sector = unit.Sector,
                    TotalAssets = unit.Assets.Count(),
                    StableAssets = unit.Assets.Count(asset => asset.LiveStatus != null && asset.LiveStatus.ProcessedStatus == "Stable"),
                    WarningAssets = unit.Assets.Count(asset => asset.LiveStatus != null && asset.LiveStatus.ProcessedStatus == "Warning"),
                    UnverifiedAssets = unit.Assets.Count(asset => asset.LiveStatus != null && !asset.LiveStatus.IsVerified)

                }).ToListAsync();
    }
    }
}
