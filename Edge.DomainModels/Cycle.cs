using System.ComponentModel.DataAnnotations;

namespace Edge.DomainModels
{
    public class Cycle
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Artwork> Artworks { get; set; }
    }
}
