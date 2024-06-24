namespace Edge.Dtos
{
    public class CycleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] ImageData { get; set; }
        public List<ArtworkDto> Artworks { get; set; }
    }
}
