﻿using COMERP.Entities.Base;

namespace COMERP.Entities
{
    public class SiteSettings : BaseEntity
    {
        public string CompanyId { get; set; }
        public Company Company { get; set; }
        public string? LogoUrl { get; set; }
        public string? FaviconUrl { get; set; }
        public string ContactEmail { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
    }
}