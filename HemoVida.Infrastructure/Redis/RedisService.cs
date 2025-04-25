using AutoMapper;
using HemoVida.Core.Entities;
using HemoVida.Core.Interfaces.Service;
using StackExchange.Redis;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HemoVida.Infrastructure.Redis;

public class RedisService : IRedisService
{
    private readonly IDatabase _db;

    public RedisService(IConnectionMultiplexer redis)
    {
        _db = redis.GetDatabase();
    }

    public async Task AddAvailableDonorAsync(Donor donor)
    {
        string key = $"donor:{donor.Id}";

        var options = new JsonSerializerOptions
        {
            ReferenceHandler = ReferenceHandler.Preserve
        };

        string json = JsonSerializer.Serialize(donor, options);
        await _db.StringSetAsync(key, json, TimeSpan.FromMinutes(30));
    }

    public async Task<List<Donor>> GetAvailableDonorsAsync()
    {
        var server = _db.Multiplexer.GetServer(_db.Multiplexer.GetEndPoints()[0]);
        var keys = server.Keys(pattern: "donor:*").ToArray();

        var donors = new List<Donor>();
        foreach (var key in keys)
        {
            var json = await _db.StringGetAsync(key);
            if (!json.IsNullOrEmpty)
            {
                var donor = JsonSerializer.Deserialize<Donor>(json);
                if (donor != null)
                    donors.Add(donor);
            }
        }
        return donors;
    }

    public async Task RemoveDonorAsync(int donorId)
    {
        await _db.KeyDeleteAsync($"donor:{donorId}");
    }
}
