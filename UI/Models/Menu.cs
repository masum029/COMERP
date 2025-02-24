﻿namespace UI.Models
{
    public class Menu : BaseModel
    {
        public string? ParentMenuId { get; set; }
        public Menu ParentMenu { get; set; }
        public string Title { get; set; }
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public string? CompanyId { get; set; }
        public Company Company { get; set; }
    }
}
