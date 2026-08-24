using Consumer.Data;
using Consumer.Models;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

namespace Consumer.Services
{
    public class UavProcessingService
    {
        private readonly IronGridDbContext _dbContext;
        public UavProcessingService(IronGridDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> ProcessUavAsync(string jsonMessage)
        {
            var reading = JsonSerializer.Deserialize<FieldReport>(jsonMessage);
            if (reading.AssetType != "UAV")
            {
                return false;
            }
            if (reading == null)
            {
                Console.WriteLine(" Failed to deserialize Uav");
                return false;
            }

            string status;
            bool verified;

            if (!int.TryParse(reading.RawValue, out int intRow))
            {
                status = "Warning";
                verified = false;
            }
            else if (intRow >= 20 && intRow <= 100)
            {
                status = "Stable";
                verified = true;
            }
            else if (intRow >= 0 && intRow <= 19)
            {
                status = "Warning";
                verified = true;
            }
            else
            {
                status = "Warning";
                verified = false;
            }

            Console.WriteLine(status);
            Console.WriteLine(verified);

            var result = new AssetLiveStatus
            {
                AssetId = reading.AssetId,
                AssetType = reading.AssetType,
                RawValue = reading.RawValue,
                ProcessedStatus = status,
                IsVerified = verified,
                LastUpdate = reading.Timestamp
            };
            var existing = await _dbContext.AssetLiveStatus
                                   .FirstOrDefaultAsync(a => a.AssetId == reading.AssetId);
            if (existing != null)
            {
                _dbContext.Entry(existing).CurrentValues.SetValues(result);
            }
            else
            {
                await _dbContext.AssetLiveStatus.AddAsync(result);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public async Task<bool> ProcessPerimeterSensorAsync(string jsonMessage)
        {
            var reading = JsonSerializer.Deserialize<FieldReport>(jsonMessage);
            if (reading.AssetType != "PerimeterSensor")
            {
                return false;
            }
            if (reading == null)
            {
                Console.WriteLine(" Failed to deserialize PerimeterSensor");
                return false;
            }

            string status;
            bool Verified;
            string row = reading.RawValue;
            string[] good = { "Good", "GOOD", "good", "gud" };
            string[] bad = { "Bad", "BAD", "bad", "bed" };
            if (good.Contains(row))
            {
                status = "Stable";
                Verified = true;
            }
            if (bad.Contains(row))
            {
                status = "Warning";
                Verified = true;
            }
            else
            {
                status = "Warning";
                Verified = false;
            }
            var result = new AssetLiveStatus
            {
                AssetId = reading.AssetId,
                AssetType = reading.AssetType,
                RawValue = reading.RawValue,
                ProcessedStatus = status,
                IsVerified = Verified,
                LastUpdate = reading.Timestamp
            };
            var existing = await _dbContext.AssetLiveStatus
                                   .FirstOrDefaultAsync(a => a.AssetId == reading.AssetId);
            if (existing != null)
            {
                _dbContext.Entry(existing).CurrentValues.SetValues(result);
            }
            else
            {
                await _dbContext.AssetLiveStatus.AddAsync(result);
            }
            await _dbContext.SaveChangesAsync();
            return true;
        }
            }
        }