$(function () {

    $('#chatBody').hide();
    $('#chatboys').hide();
    $('#chatgirls').hide();
    $('#chatRoomName').hide();
    $('#loginBlock').hide();
    $('#allChats').hide();
    $('#allChatsLabel').hide();

    // Ссылка на автоматически-сгенерированный прокси хаба
    var chat = $.connection.chatHub;

    var selectedChat;
    

    chat.client.displayChatsList = function (chatRooms) {
        debugger

        //$('#chatList').empty();
        for (i = 0; i < chatRooms.length; i++) {
            if ($('#select' + htmlEncode(chatRooms[i].Name) + 'Chat').length) { }
            else {
                $('#chatList').append('<input type="button" id="select' + htmlEncode(chatRooms[i].Name) + 'Chat" value="' + htmlEncode(chatRooms[i].Name) + '">');
            }
            if ($('#chatArea' + chatRooms[i].Name).length) { }
            else {
                $('#chatArea').append('<div id="chatArea' + chatRooms[i].Name + '"> </div>');
            }
            if ($('#usersOf' + chatRooms[i].Name + 'Chat').length) { }
            else {
                $('#chatUsers').append('<div id="usersOf' + chatRooms[i].Name + 'Chat"> </div>');
            }

            $('#select' + htmlEncode(chatRooms[i].Name) + 'Chat').click(function () {
                selectedChat = this.value; console.log(selectedChat);
                $('#chatArea').children().hide();
                $('#chatArea' + this.value).show();
                $('#chatUsers').children().hide();
                $('#usersOf' + this.value + 'Chat').show();
                $('#chatRoomName').show();
                $('#chatRoomName').empty();
                $('#chatRoomName').append(this.value);
                chat.server.changeRoom($('#hdId').val(), selectedChat);
            });
        }

        if ($('#createChat').length) { }
        else {
            $('#chatList').prepend('<input type="button" id="createChat" value="Create">');
        }

        $('#createChat').click(function () {
            chat.server.getUserChatsNumberLeft();
        });
        
    };

    chat.client.createChat = function () {
        $('#chatList').append('<input type="text" id="newChatName"> <input type="button" id="applyNewChat" value="apply">');
        $('#applyNewChat').click(function () {
            var name = $("#newChatName").val();
            if (name.length > 0) {
                chat.server.createChat(name);
                $('#chatArea').append('<div id="chatArea' + name + '"> </div>');

            }
        });
    }

    chat.client.deleteChatInputs = function () {
        $('#applyNewChat').remove();
        $('#newChatName').remove();
    }

    chat.client.displayUsers = function (allUsers) {
        displayUsers(allUsers);
    };

    // Объявление функции, которая хаб вызывает при получении сообщений
    chat.client.addMessage = function (name, message, chatName) {

        $('#chatArea' + chatName).prepend('<p><b>' + htmlEncode(name)
            + '</b>: ' + htmlEncode(message) + '</p>');
    };

    // Функция, вызываемая при подключении нового пользователя
    chat.client.onConnected = function (id, userName) {

        $('#loginBlock').hide();
        $('#chatBody').show();
        $('#allChatsLabel').show();
        $('#allChats').show();

        chat.server.initialize();
        // установка в скрытых полях имени и id текущего пользователя
        $('#hdId').val(id);
        $('#username').val(userName);
        $('#header').html('<h3>Welcome, ' + userName + '</h3>');

    }

    // Добавляем нового пользователя
    chat.client.onNewUserConnected = function (chatName, id, name) {

        AddUser(chatName, id, name);
    }

    // Удаляем пользователя
    chat.client.onUserDisconnected = function (id, userName) {

        $('#' + id).remove();
    }

    // Открываем соединение
    $.connection.hub.start().done(function () {


        $('#loginBlock').show();

        $('#sendmessage').click(function () {
            // Вызываем у хаба метод Send
            chat.server.send($('#username').val(), $('#message').val(), selectedChat);
            $('#message').val('');
        });

        // обработка логина
        $("#btnLogin").click(function () {
            var name = $("#txtUserName").val();
            if (name.length > 0) {
                chat.server.connect(name, selectedChat);
            }
            else {
                alert("Введите имя");
            }
        });
    });
});
// Кодирование тегов
function htmlEncode(value) {
    var encodedValue = $('<div />').text(value).html();
    return encodedValue;
}
//Добавление нового пользователя
function AddUser(chat, id, name) 
{

    $('#usersOf' + chat + 'Chat').append('<p id="' + id + '"><b>' + name + '</b></p>');
}

function displayUsers(allUsers) {
    $('#chatUsers').children().empty();
    $('#chatUsers').children().append('<p><b>Users in that chat</b></p>');
    for (i = 0; i < allUsers.length; i++) {

        AddUser(allUsers[i].ChatName, allUsers[i].ConnectionId, allUsers[i].Name);
    }
}
