﻿namespace UI.Models
{
    public class Company : BaseModel
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public DateTime? EstablishedDate { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Website { get; set; }
    }
}