using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.BusinessLogicLayer.InputModels
{
    public struct AdminInputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }

        public AdminInputModel(string email, string password)
        {
            Email = email;
            Password = password;
        }
    }
}
