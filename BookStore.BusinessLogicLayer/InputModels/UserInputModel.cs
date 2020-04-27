using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.BusinessLogicLayer.InputModels
{
    public struct UserInputModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string[] UserBook { get; set; }
        public string[] UserBasket { get; set; }
    }
}
