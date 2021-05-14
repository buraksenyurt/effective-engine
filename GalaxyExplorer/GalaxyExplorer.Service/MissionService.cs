using GalaxyExplorer.DTO;
using GalaxyExplorer.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GalaxyExplorer.Service
{
    public class MissionService
        : IMissionService
    {
        private readonly GalaxyExplorerDbContext _dbContext;
        // Servisi kullanan uygulamanın DI Container Service Registery'si üzerinden gelecektir.
        // O anki opsiyonları ile birlikte gelir. SQL olur, Postgresql olur, Mongo olur bilemiyorum.
        // Entity modelin uygun düşen bir DbContext gelecektir.
        public MissionService(GalaxyExplorerDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public async Task<GetVoyagersResponse> GetVoyagers(GetVoyagersRequest request)
        {
            var currentStartRow = (request.PageNumber - 1) * request.PageSize;
            var response = new GetVoyagersResponse
            {
                // Kolaylık olsun diye sonraki sayfa için de bir link bıraktım
                // Lakin başka kayıt yoksa birinci sayfaya da döndürebiliriz
                NextPage = $"api/voyager?PageNumber={request.PageNumber + 1}&PageSize={request.PageSize}&OnMission={request.OnMission}", 
                TotalVoyagers = await _dbContext.Voyagers.CountAsync(),
                TotalActiveVoyagers = await _dbContext.Voyagers.CountAsync(v => v.OnMission == true)
            };

            var voyagers = await _dbContext.Voyagers
                .Where(v => v.OnMission == request.OnMission)
                .Skip(currentStartRow)
                .Take(request.PageSize)
                .Select(v => new VoyagerResponse
                {
                    Name = v.Name,
                    Grade = v.Grade,
                    Detail = $"api/voyager/{v.VoyagerId}" // Bu Voyager'ın detaylarını görmek için bir sayfaya gitmek isterse diye
                })
                .ToListAsync();
            response.Voyagers = voyagers;

            return response;
        }
        public async Task<MissionStartResponse> StartMissionAsync(MissionStartRequest request)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync(); // Transaction başlatalım
            try
            {
                // Mürettebat sayısı uygun olup aktif görevde olmayan bir gemi bulmalıyız. Aday havuzunu çekelim.
                var crewCount = request.Voyagers.Count;
                var candidates = _dbContext.Spaceships.Where(s => s.MaxCrewCount >= crewCount && s.OnMission == false).ToList();
                if (candidates.Count > 0)
                {
                    Random rnd = new();
                    var candidateId = rnd.Next(0, candidates.Count);
                    var ship = candidates[candidateId]; // Index değerine göre rastgele bir tanesini alalım

                    ship.OnMission = true;
                    await _dbContext.SaveChangesAsync(); // Gemiyi görevde durumuna alalım

                    // Görev nesnesini oluşturalım
                    Mission mission = new()
                    {
                        Name = request.Name,
                        PlannedDuration = request.PlannedDuration,
                        SpaceshipId = ship.SpaceshipId, // Gemi ile ilişkilendirdik
                        StartDate = DateTime.Now
                    };
                    await _dbContext.Missions.AddAsync(mission);
                    await _dbContext.SaveChangesAsync(); // Görev nesnesini db'ye yollayalım

                    // Gelen gezginlerin listesini dolaşıp
                    var voyagers = new List<Voyager>();
                    foreach (var v in request.Voyagers)
                    {
                        Voyager voyager = new()
                        {
                            Name = v.Name,
                            Grade = v.Grade,
                            OnMission = true,
                            MissionId = mission.MissionId // Görevle ilişkilendirdik
                        };
                        voyagers.Add(voyager);
                    }
                    await _dbContext.Voyagers.AddRangeAsync(voyagers); // Bunları topluca Voyagers listesine ekleyelim
                    await _dbContext.SaveChangesAsync(); // Değişiklikleri kaydedelim.
                    await transaction.CommitAsync(); // Transaction'ı commit edelim

                    return new MissionStartResponse
                    {
                        Success = true,
                        Message = "Görev başlatıldı."
                    };
                }
                else // Müsait veya uygun gemi yoksa burda durmamızın anlamı yok
                {
                    await transaction.RollbackAsync();

                    return new MissionStartResponse
                    {
                        Success = false,
                        Message = "Şu anda görev için müsait gemi yok"
                    };
                }
            }
            catch (Exception exp)
            {
                await transaction.RollbackAsync();
                return new MissionStartResponse
                {
                    Success = false,
                    Message = $"Sistem Hatası:{exp.Message}"
                };
            }
        }
    }
}
