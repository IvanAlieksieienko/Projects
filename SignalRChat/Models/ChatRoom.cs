﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SignalRChat.Models
{
    public class ChatRoom
    {
        public string ConnectionId { get; set; }
        public string Name { get; set; }
    }
}