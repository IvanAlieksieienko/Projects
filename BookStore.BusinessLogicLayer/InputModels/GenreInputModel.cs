using System;
using System.Collections.Generic;
using System.Text;

namespace BookStore.BusinessLogicLayer.InputModels
{
    public struct GenreInputModel
    {
        public string Name { get; set; }

        public GenreInputModel(string name)
        {
            Name = name;
        }
    }
}
