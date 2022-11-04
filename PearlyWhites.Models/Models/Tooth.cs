﻿namespace PearlyWhites.Models.Models
{
    public record Tooth
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public List<Treatment> Treatments { get; set; } = new List<Treatment>();
        public int PatientId { get; set; }
    }
}
