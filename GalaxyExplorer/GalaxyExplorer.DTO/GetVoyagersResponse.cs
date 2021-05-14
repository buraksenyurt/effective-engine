using System.Collections.Generic;

namespace GalaxyExplorer.DTO
{
    public class GetVoyagersResponse
    {
        public int TotalVoyagers { get; set; }
        public int TotalActiveVoyagers { get; set; }
        public List<VoyagerResponse> Voyagers { get; set; }
        public string NextPage { get; set; }
    }
}
