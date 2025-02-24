﻿

namespace UI.Dtos
{
    public class Register 
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Password { get; set; }
        public string ConfirmationPassword { get; set; }
        public List<string> ? Roles { get; set; }
        public string RoleName { get; set; }

    }
}
