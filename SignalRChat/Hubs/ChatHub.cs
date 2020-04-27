using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNet.SignalR;
using SignalRChat.Models;

namespace SignalRChat.Hubs
{
    public class ChatHub : Hub
    {
        static List<User> Users = new List<User>();
        static List<ChatRoom> ChatRooms = new List<ChatRoom>();


        // Отправка сообщений
        public void Send(string name, string message, string chatName)
        {
            var id = Context.ConnectionId;
            
            Clients.All.addMessage(Users.FirstOrDefault(u => u.ConnectionId == id).Name, message, chatName);
        }

        // Подключение нового пользователя
        public void Connect(string userName, string chatName)
        {
            var id = Context.ConnectionId;


            if (!Users.Any(x => x.ConnectionId == id))
            {
                Users.Add(new User { ConnectionId = id, Name = userName, ChatName = chatName, AmountOfPossibilityChatsMayCreate = 1 });

                // Посылаем сообщение текущему пользователю
                Clients.Caller.onConnected(id, userName);

                // Посылаем сообщение всем пользователям, кроме текущего
                Clients.AllExcept(id).onNewUserConnected(Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId).ChatName, id, userName);
            }
        }

        // Отключение пользователя
        public override Task OnDisconnected(bool stopCalled)
        {
            var item = Users.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            if (item != null)
            {
                Users.Remove(item);
                var id = Context.ConnectionId;
                Clients.All.onUserDisconnected(id, item.Name);
            }

            return base.OnDisconnected(stopCalled);
        }

        public void Initialize()
        {
            var id = Context.ConnectionId;
            if (!ChatRooms.Any(x => x.ConnectionId == id))
            {
                if (ChatRooms.Count == 0)
                {
                    ChatRooms.Add(new ChatRoom { ConnectionId = "1", Name = "Boys" });
                    ChatRooms.Add(new ChatRoom { ConnectionId = "2", Name = "Girls" });
                }
            }
            Clients.All.displayChatsList(ChatRooms);
        }

        public void ChangeRoom(string userId, string chatName)
        {
            Users.FirstOrDefault(u => u.ConnectionId == userId).ChatName = chatName;
            Clients.All.displayUsers(Users);
        }

        public void CreateChat(string chatName)
        {
            var id = Context.ConnectionId;
            Users.FirstOrDefault(u => u.ConnectionId == id).AmountOfPossibilityChatsMayCreate = 1;
            ChatRooms.Add(new ChatRoom { ConnectionId = (ChatRooms.Count + 1).ToString(), Name = chatName });
            Clients.Caller.deleteChatInputs();
            Clients.All.displayChatsList(ChatRooms);
        }

        public void GetUserChatsNumberLeft()
        {
            var id = Context.ConnectionId;
            var number = Users.FirstOrDefault(u => u.ConnectionId == id).AmountOfPossibilityChatsMayCreate;
            if (number > 0)
            {
                Users.FirstOrDefault(u => u.ConnectionId == id).AmountOfPossibilityChatsMayCreate--;
                Clients.Caller.createChat();
            }
            
        }
    }
}