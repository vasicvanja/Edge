using Edge.Shared.DataContracts.Enums;

namespace Edge.Dtos
{
    public class ArtworkDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technique { get; set; }
        public int Year { get; set; }
        public float Price { get; set; }
        public int Quantity { get; set; }
        public EArtworkType Type { get; set; }
        public byte[] ImageData { get; set; }
        public int? CycleId { get; set; }
        public CycleDto CycleDto { get; set; }
    }
}
