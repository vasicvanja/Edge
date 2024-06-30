namespace Edge.Dtos
{
    public class CreateCycleDto
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public byte[] ImageData { get; set; }
        public List<int> ArtworkIds { get; set; }
    }
}