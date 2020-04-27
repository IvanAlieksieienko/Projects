using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookStore.API.Layer.Controllers
{
    public static class Roles
    {
        public enum Role
        {
            user,
            admin,
            unauthorized
        }
    }
}
