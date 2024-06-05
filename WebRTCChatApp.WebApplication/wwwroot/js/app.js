$(document).ready(function () {
    // Connect to the SignalR hub
    // Create a global variable for the connection
    window.connection = new signalR.HubConnectionBuilder()
        .withUrl("https://localhost:7057/ChatHub")
        .build();
    var user = document.getElementById("username").value;

    // Start the connection
    window.connection.start().then(function (user) {
        console.log("SignalR Connected");
    }).catch(function (err) {
        console.log("Error while connecting to SignalR: " + err);
    });

    // Receive message
    window.connection.on("ReceiveMessage", function (user, message) {
        // Display the received message
        var li = document.createElement("li");
        li.textContent = user + ": " + message;
        document.getElementById("messagesList").appendChild(li);
    });

    //window.connection.on("ReceiveMessage", function (user, message) {
    //    // Create the list item element
    //    var li = document.createElement("li");
    //    li.className = "clearfix";

    //    // Create the message-data div
    //    var messageDataDiv = document.createElement("div");
    //    messageDataDiv.className = "message-data text-right";

    //    // Create the time span
    //    var timeSpan = document.createElement("span");
    //    timeSpan.className = "message-data-time";
    //    var now = new Date();
    //    var hours = now.getHours();
    //    var minutes = now.getMinutes();
    //    var ampm = hours >= 12 ? 'PM' : 'AM';
    //    hours = hours % 12;
    //    hours = hours ? hours : 12; // the hour '0' should be '12'
    //    minutes = minutes < 10 ? '0' + minutes : minutes;
    //    var strTime = hours + ':' + minutes + ' ' + ampm + ', Today';
    //    timeSpan.textContent = strTime;

    //    // Create the avatar image
    //    var img = document.createElement("img");
    //    img.src = "https://bootdey.com/img/Content/avatar/avatar7.png";
    //    img.alt = "avatar";

    //    // Append the time span and image to the message-data div
    //    messageDataDiv.appendChild(timeSpan);
    //    messageDataDiv.appendChild(img);

    //    // Create the message div
    //    var messageDiv = document.createElement("div");
    //    messageDiv.className = "message other-message float-right";
    //    messageDiv.textContent = user+ " : "+message;

    //    // Append the message-data div and message div to the list item
    //    li.appendChild(messageDataDiv);
    //    li.appendChild(messageDiv);

    //    // Append the list item to the messages list
    //    document.getElementById("messagesList").appendChild(li);
    //});

});
function sendMessage() {
    var user = document.getElementById("username").value;
    var message = document.getElementById("chatMessage").value;
    // Connect to the SignalR hub
    //var connection=connectToSignalR();
    // Send message to the SignalR hub
    console.log(window.connection.state);
    // Check if the connection is established
    if (window.connection.state === signalR.HubConnectionState.Connected) {
        // Send message to the SignalR hub
        window.connection.invoke("SendMessage", user, message).catch(function (err) {
            console.log(user + " " + message);
            console.error("Error while sending message: " + err);
        });

        // Clear the message input
        document.getElementById("chatMessage").value = "";
    } else {
        console.error("SignalR connection is not established.");
    }
}
