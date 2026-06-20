const sendButton = document.getElementById("sendButton");
const input = document.getElementById("messageInput");
const messages = document.getElementById("messages");

// Sidebar workflow shortcuts

const btnKnowledge = document.getElementById("btnKnowledge");
const btnIssue = document.getElementById("btnIssue");
const btnPoc = document.getElementById("btnPoc");
const btnWeekend = document.getElementById("btnWeekend");
const btnAzureVm = document.getElementById("btnAzureVm");
const btnAriba = document.getElementById("btnAriba");

// Business Logic:
// Allow users to submit chat messages by either
// clicking Send or pressing Enter.

sendButton.addEventListener("click", sendMessage);

// Business Logic:
// Quick launch workflow actions from sidebar.

btnKnowledge?.addEventListener("click", () => {
    input.value = "knowledge search";
    sendMessage();
});

btnIssue?.addEventListener("click", () => {
    input.value = "issue";
    sendMessage();
});

btnPoc?.addEventListener("click", () => {
    input.value = "poc";
    sendMessage();
});

btnWeekend?.addEventListener("click", () => {
    input.value = "weekend";
    sendMessage();
});

btnAzureVm?.addEventListener("click", () => {
    input.value = "azure vm";
    sendMessage();
});

btnAriba?.addEventListener("click", () => {
    input.value = "ariba";
    sendMessage();
});

input.addEventListener("keypress", function (event) {

    if (event.key === "Enter") {

        event.preventDefault();
        sendMessage();
    }
});
async function sendMessage() {

    const text = input.value.trim();

    if (!text)
        return;

    messages.innerHTML += `
        <div class="user-msg">
            ${text}
        </div>
    `;

    input.value = "";

    try {

        const response = await fetch("/api/chat", {
            method: "POST",
            headers: {
                "Content-Type": "application/json"
            },
            body: JSON.stringify({
                message: text
            })
        });

        const result = await response.json();

        messages.innerHTML += `
            <div class="assistant-msg">
                ${result.response}
            </div>
        `;

    }
    catch (error) {

        // Business Logic:
        // Display a meaningful message when the API
        // cannot be reached or returns an unexpected error.

        messages.innerHTML += `
        <div class="assistant-msg">
            Unable to reach assistant: ${error.message}
        </div>
    `;

        console.error(error);
    }

    messages.scrollTop = messages.scrollHeight;
}