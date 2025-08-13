using Edge.Shared.DataContracts.Enums;

namespace Edge.Dtos
{
    public class ArtworkFilterDto
    {
        public EArtworkType? Type { get; set; }
        public int CycleId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public decimal? Width { get; set; }
        public decimal? Height { get; set; }
        public string SortBy { get; set; } // e.g. "price", "name"
        public string SortDirection { get; set; } = "asc"; // or "desc"
    }
}
