using IronGridC2Api.Data;
using IronGridC2Api.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace IronGridC2Api.Repositories
{
    public class AssetRepository:IAssetRepository
    {
        private readonly IronGridDbContext _db;
        public AssetRepository (IronGridDbContext db)
        {
            _db = db;
        }
        public async Task<AssetDto?> GatByIdAsync(int id)
        {

            return await _db.Assets

            .AsNoTracking()
            .AsQueryable()
            .Where(asset => asset.Id == id)
            .Select(asset => new AssetDto
            {
                Id = asset.Id,
                UnitId = asset.UnitId,
                AssetSerial = asset.AssetSerial,
                AssetType = asset.AssetType,
                UnitName = asset.Unit.UnitName,
                Sector = asset.Unit.Sector
            }).FirstOrDefaultAsync();
        }

        public async Task<bool> CreateUnitAsync(UnitsDto request)
        {
            bool unitExists = await _db.Units.AnyAsync(unit => unit.Id == request.Id);
            if (unitExists)
                return false;

            var unit = new Unit
            {
                Id = request.Id,
                UnitName = request.UnitName.Trim(),
                Sector = request.Sector.Trim()
            };

            _db.Units.Add(unit);
            await _db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> UpdateAsync(int id, UpdateAssetRequest request)
        {
            var asset = await _db.Assets.FirstOrDefaultAsync(asset => asset.Id == id);
            if (asset is null)
                return false;

            bool unitExists = await _db.Units.AnyAsync(unit => unit.Id == request.UnitId);
            if (!unitExists)
                return false;

            asset.UnitId = request.UnitId;
            asset.AssetSerial = request.AssetSerial.Trim();
            asset.AssetType = request.AssetType.Trim();

            await _db.SaveChangesAsync();

            var updatedAsset = await GatByIdAsync(id);
            return true;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            var asset = await _db.Assets.FirstOrDefaultAsync(asset => asset.Id == id);
            if (asset is null)
                return false;

            _db.Assets.Remove(asset);
            await _db.SaveChangesAsync();
            return true;
        }
    }

}

