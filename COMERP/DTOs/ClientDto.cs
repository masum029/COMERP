﻿namespace COMERP.DTOs
{
    public class ClientDto
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string? ContactPerson { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        public string? Icon { get; set; }
        public bool isActive { get; set; }
        public string CompanyId { get; set; }
    }
}
