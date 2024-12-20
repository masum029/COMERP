namespace COMERP.DTOs
{
    public class MenuDto
    {
        public string? Id { get; set; }
        public string? ParentMenuId { get; set; }
        public string Title { get; set; }
        public string? LinkUrl { get; set; }
        public int DisplayOrder { get; set; }
        public bool IsVisible { get; set; } = true;
        public string? CompanyId { get; set; }
    }
}
