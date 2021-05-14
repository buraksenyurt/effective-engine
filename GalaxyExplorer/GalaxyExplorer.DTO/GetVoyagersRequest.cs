using System.ComponentModel.DataAnnotations;

namespace GalaxyExplorer.DTO
{
    public class GetVoyagersRequest
    {
        [Required]
        public int PageNumber { get; set; }
        [Required]
        [Range(5,20)] // Sayfa başına minimum 5 maksimum 20 satır kabul edelim
        public int PageSize { get; set; }
        public bool OnMission { get; set; }
    }
}
