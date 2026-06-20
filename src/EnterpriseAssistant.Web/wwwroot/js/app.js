const sendButton = document.getElementById("sendButton");
const input = document.getElementById("messageInput");
const messages = document.getElementById("messages");
const btnChat =
    document.getElementById("btnChat");
const submitIssueBtn =
    document.getElementById("submitIssueBtn");
// POC workflow controls

const submitPocBtn =
    document.getElementById("submitPoc");

// Weekend workflow controls

const submitWeekendBtn =
    document.getElementById("submitWeekend");
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
function hideWorkflowForms() {

    document.getElementById("workflowContainer").classList.add("d-none");

    document.getElementById("issueForm").classList.add("d-none");
    document.getElementById("pocForm").classList.add("d-none");
    document.getElementById("weekendForm").classList.add("d-none");
}

function showWorkflow(formId) {

    hideWorkflowForms();

    document.getElementById("workflowContainer").classList.remove("d-none");
    document.getElementById(formId).classList.remove("d-none");
}
function submitIssue() {

    const title =
        document.getElementById("issueTitle").value;

    const description =
        document.getElementById("issueDescription").value;

    const priority =
        document.getElementById("issuePriority").value;

    if (!title) {

        alert("Issue title is required.");
        return;
    }

    messages.innerHTML += `
        <div class="assistant-msg">
            <strong>Issue Created</strong><br>
            Id: INC-${Date.now()}<br>
            Title: ${title}<br>
            Priority: ${priority}<br>
            Status: Submitted
        </div>
    `;

    hideWorkflowForms();

    messages.scrollTop =
        messages.scrollHeight;
}
// Business Logic:
// Creates a demo POC request and displays
// confirmation in the conversation panel.

function submitPoc() {

    const pocName =
        document.getElementById("pocName").value.trim();

    const customer =
        document.getElementById("pocCustomer").value.trim();

    const businessNeed =
        document.getElementById("pocBusinessNeed").value.trim();

    if (!pocName) {

        alert("POC Name is required.");
        return;
    }

    const pocId =
        "POC-" + Date.now();

    messages.innerHTML += `
        <div class="assistant-msg">
            <strong>POC Request Created</strong><br>
            Request Id: ${pocId}<br>
            POC: ${pocName}<br>
            Customer: ${customer}<br>
            Status: Submitted
        </div>
    `;

    document.getElementById("pocName").value = "";
    document.getElementById("pocCustomer").value = "";
    document.getElementById("pocBusinessNeed").value = "";

    hideWorkflowForms();

    messages.scrollTop =
        messages.scrollHeight;
}
// Business Logic:
// Creates a demo weekend exclusion request.

function submitWeekendExclusion() {

    const changeRequest =
        document.getElementById("changeRequest").value.trim();

    const weekendDate =
        document.getElementById("weekendDate").value;

    const justification =
        document.getElementById("weekendReason").value.trim();

    if (!changeRequest) {

        alert("Change Request Number is required.");
        return;
    }

    const exclusionId =
        "WE-" + Date.now();

    messages.innerHTML += `
        <div class="assistant-msg">
            <strong>Weekend Exclusion Submitted</strong><br>
            Request Id: ${exclusionId}<br>
            Change Request: ${changeRequest}<br>
            Weekend Date: ${weekendDate}<br>
            Status: Pending Approval
        </div>
    `;

    document.getElementById("changeRequest").value = "";
    document.getElementById("weekendDate").value = "";
    document.getElementById("weekendReason").value = "";

    hideWorkflowForms();

    messages.scrollTop =
        messages.scrollHeight;
}

btnChat?.addEventListener("click", () => {

    hideWorkflowForms();
});
btnIssue?.addEventListener("click", () => {
    showWorkflow("issueForm");
});

btnPoc?.addEventListener("click", () => {
    showWorkflow("pocForm");
});

btnWeekend?.addEventListener("click", () => {
    showWorkflow("weekendForm");
});
submitIssueBtn?.addEventListener("click", submitIssue);
submitPocBtn?.addEventListener(
    "click",
    submitPoc
);
submitWeekendBtn?.addEventListener(
    "click",
    submitWeekendExclusion
);
