﻿using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class Service : BaseEntity
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        [Precision(18, 2)]
        public decimal Price { get; set; }
        public int? DurationHours { get; set; }
        public string? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}