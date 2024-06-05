const connection = new signalR.HubConnectionBuilder()
    .withUrl("/chatHub")
    .build();
var user = document.getElementById("username").value;
var message = "Hello";
connection.on("ReceiveMessage", (user, message) => {
    console.log(`${user} says ${message}`)
})

connection
    .start()
    .then(() => {
        console.log("SignalR connection established");
    }).catch(err => {
        console.error(err.toString())
    })

connection
    .invoke("SendMessage", user, message)
    .catch(err => {
        console.error(err)
    });

// Handle incoming messages from the hub
connection.on("ReceiveMessage", (user, message) => {
    const messagesList = document.getElementById("messagesList");
    const newMessage = document.createElement("div");
    newMessage.textContent = `${user}: ${message}`;
    console.log(`User: ${user}, Message: ${message}`);
    messagesList.appendChild(newMessage);
});
connection.start().catch(err => console.error(err.toString()));
// Function to send messages to the SignalR hub
function sendMessage() {
    const messageInput = document.getElementById("messageInput");
    const message = messageInput.value;

    connection.invoke("SendMessage", "User", message) // Replace "User" with actual user info
        .then(() => {
            messageInput.value = ""; // Clear input after sending
        })
        .catch(err => console.error(err));
}

// Define the base URL of the SignalService
const clientServiceUrl = window.location.origin + "/api/Chat/SendSignal";
console.log(clientServiceUrl);
// Function to send signaling messages via HTTP POST
async function sendSignal(signal) {
    try {
        const response = await fetch(clientServiceUrl, {
            method: "POST",
            headers: {
                "Content-Type": "application/json",
                "Accept": "application/json"
            },
            body: JSON.stringify(signal)
        });
        if (!response.ok) {
            throw new Error(`HTTP error! status: ${response.status}`);
        }
    } catch (error) {
        console.error("Error sending signal:", error);
    }
}

// Function to send an offer
async function sendOffer(user, offer) {
    const signal = {
        type: "offer",
        user: user,
        data: offer
    };
    await sendSignal(signal);
}

// Function to send an answer
async function sendAnswer(user, answer) {
    const signal = {
        type: "answer",
        user: user,
        data: answer
    };
    await sendSignal(signal);
}

// Function to send an ICE candidate
async function sendIceCandidate(user, candidate) {
    const signal = {
        type: "candidate",
        user: user,
        data: candidate
    };
    await sendSignal(signal);
}

// Handling received signaling messages
connection.on("ReceiveSignal", async (signal) => {
    if (signal.type === "offer") {
        await handleReceivedOffer(signal.data);
    } else if (signal.type === "answer") {
        await handleReceivedAnswer(signal.data);
    } else if (signal.type === "candidate") {
        await handleReceivedCandidate(signal.data);
    }
});

// Function to handle received offer
async function handleReceivedOffer(offer) {
    await peerConnection.setRemoteDescription(new RTCSessionDescription(offer));
    const answer = await peerConnection.createAnswer();
    await peerConnection.setLocalDescription(answer);
    await sendAnswer(offer.user, peerConnection.localDescription);
}

// Function to handle received answer
async function handleReceivedAnswer(answer) {
    await peerConnection.setRemoteDescription(new RTCSessionDescription(answer));
}

// Function to handle received ICE candidate
async function handleReceivedCandidate(candidate) {
    await peerConnection.addIceCandidate(new RTCIceCandidate(candidate));
}
