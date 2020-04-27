using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.BusinessLogicLayer.InputModels
{
    public struct AuthorInputModel
    {
        public string Name { get; set; }

        public AuthorInputModel(string name)
        {
            Name = name;
        }
    }
}
