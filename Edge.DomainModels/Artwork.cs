﻿using Edge.Shared.DataContracts.Enums;
using System.ComponentModel.DataAnnotations;

namespace Edge.DomainModels
{
    public class Artwork
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Technique { get; set; }
        public int Year { get; set; }
        public float Price { get; set; }
        public EArtworkType Type { get; set; }
        public byte[] ImageData { get; set; }
        public int? CycleId { get; set; }
        public Cycle Cycle { get; set; }
    }
}
