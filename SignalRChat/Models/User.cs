﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Models
{
    public class User
    {
        public string ConnectionId { get; set; }
        public string ChatName { get; set; }
        public string Name { get; set; }
        public int AmountOfPossibilityChatsMayCreate { get; set; }
    }
}