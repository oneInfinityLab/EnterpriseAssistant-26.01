const pluginActivity =
    document.getElementById("pluginActivity");
const sendButton = document.getElementById("sendButton");
const input = document.getElementById("messageInput");
const messages = document.getElementById("messages");
// Business Logic:
// Tracks workflow execution counts
// during the current application session.

let issueCount = 0;
let pocCount = 0;
let weekendCount = 0;

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
    logPluginActivity(
        "Knowledge Search Plugin Executed"
    );
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
                ${result.response.replace(/\n/g, "<br>")}
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
// Business Logic:
// Return assistant to normal chat mode.

function resetToChatMode() {

    hideWorkflowForms();

    input.value = "";
    input.focus();
    document
        .querySelectorAll("input, textarea")
        .forEach(control => {

            if (control.id !== "messageInput")
                control.value = "";
        });

    logPluginActivity(
        "Returned to Chat Assistant"
    );
}
function showWorkflow(formId) {

    hideWorkflowForms();

    document.getElementById("workflowContainer").classList.remove("d-none");
    document.getElementById(formId).classList.remove("d-none");
}
// Business Logic:
// Submits an Issue request to the Workflow API.

async function submitIssue() {

    const title =
        document.getElementById("issueTitle").value.trim();

    const description =
        document.getElementById("issueDescription").value.trim();
    const priority =
        document.getElementById(
            "issuePriority"
        ).value;

    if (!title) {

        alert("Issue title is required.");
        return;
    }

    try {

        const response =
            await fetch(
                "/api/workflow/issue",
                {
                    method: "POST",
                    headers: {
                        "Content-Type":
                            "application/json"
                    },
                    body: JSON.stringify({
                        title,
                        description,
                        priority
                    })
                });
        if (!response.ok) {

            throw new Error(
                `HTTP ${response.status}`
            );
        }
        const result =
            await response.json();

        messages.innerHTML += `
    <div class="assistant-msg">
        <strong>Issue Created</strong><br>
        Id: ${result.id}<br>
        Title: ${result.title}<br>
        Priority: ${result.priority}<br>
        Status: ${result.status}<br>
        Created By: ${result.createdBy}
    </div>
`;
        logPluginActivity(
            "Issue Plugin Executed"
        );
        addRecentRequest(
            "Issue",
            result.id,
            result.title
        );
        
        await loadDashboardMetrics();

        document.getElementById(
            "issueTitle"
        ).value = "";

        document.getElementById(
            "issueDescription"
        ).value = "";

        hideWorkflowForms();

        messages.scrollTop =
            messages.scrollHeight;
    }
    catch (error) {

        console.error(error);

        alert(
            "Unable to submit issue."
        );
    }
}
// Business Logic:
// Submits a POC request to the Workflow API
// and displays the resulting request details.

async function submitPoc() {

    const title =
        document.getElementById("pocName").value.trim();

    const businessJustification =
        document.getElementById("pocBusinessNeed").value.trim();
    const customer =
        document.getElementById(
            "pocCustomer"
        ).value.trim();
    if (!title) {

        alert("POC Name is required.");
        return;
    }

    try {

        const response =
            await fetch(
                "/api/workflow/poc",
                {
                    method: "POST",
                    headers: {
                        "Content-Type":
                            "application/json"
                    },
                    body: JSON.stringify({
                        title,
                        customer,
                        businessJustification
                    })
                });

        const result =
            await response.json();

        messages.innerHTML += `
            <div class="assistant-msg">
                <strong>POC Request Created</strong><br>
                Request Id: ${result.id}<br>
                Title: ${result.title}<br>
                Status: ${result.status}<br>
                Customer: ${result.customer}<br>
                Requested By: ${result.requestedBy}
            </div>
        `;

        logPluginActivity(
            "POC Plugin Executed"
        );
        addRecentRequest(
            "POC",
            result.id,
            result.title
        );
        await loadDashboardMetrics();

        document.getElementById(
            "pocName"
        ).value = "";

        document.getElementById(
            "pocCustomer"
        ).value = "";

        document.getElementById(
            "pocBusinessNeed"
        ).value = "";

        hideWorkflowForms();

        messages.scrollTop =
            messages.scrollHeight;
    }
    catch (error) {

        console.error(error);

        alert(
            "Unable to submit POC request."
        );
    }
}
// Business Logic:
// Submits a Weekend Exclusion request to the
// Workflow API and displays the resulting details.

async function submitWeekendExclusion() {

    const applicationName =
        document.getElementById(
            "applicationName"
        ).value.trim();
    const changeRequest =
        document.getElementById(
            "changeRequest"
        ).value.trim();

    const weekendDate =
        document.getElementById(
            "weekendDate"
        ).value;

    const justification =
        document.getElementById(
            "weekendReason"
        ).value.trim();
    if (!applicationName) {

        alert(
            "Application Name is required."
        );

        return;
    }
    if (!changeRequest) {

        alert(
            "Change Request Number is required."
        );

        return;
    }

    try {

        const response =
            await fetch(
                "/api/workflow/weekend",
                {
                    method: "POST",
                    headers: {
                        "Content-Type":
                            "application/json"
                    },
                    body: JSON.stringify({
                        applicationName,
                        changeRequest,
                        weekendDate,
                        justification
                    })
                });

        const result =
            await response.json();

        messages.innerHTML += `
            <div class="assistant-msg">
                <strong>Weekend Exclusion Submitted</strong><br>
                Request Id: ${result.id}<br>
                Application: ${result.applicationName}<br>
                Change Request:
    ${result.changeRequest}<br>

Weekend Date:
    ${result.weekendDate}<br>

Status:
    ${result.status}<br>

Requested By:
    ${result.requestedBy}
            </div>
        `;

        logPluginActivity(
            "Weekend Exclusion Plugin Executed"
        );

        addRecentRequest(
            "Weekend",
            result.id,
            result.changeRequest
        );
        await loadDashboardMetrics();

        document.getElementById(
            "changeRequest"
        ).value = "";

        document.getElementById(
            "weekendDate"
        ).value = "";

        document.getElementById(
            "weekendReason"
        ).value = "";

        hideWorkflowForms();

        messages.scrollTop =
            messages.scrollHeight;
    }
    catch (error) {

        console.error(error);

        alert(
            "Unable to submit Weekend Exclusion request."
        );
    }
}

// Business Logic:
// Records plugin execution activity.

function logPluginActivity(message) {

    const activityLog =
        document.getElementById("activityLog");

    if (!activityLog)
        return;

    const item =
        document.createElement("li");

    item.innerHTML =
        `🟢 ${new Date().toLocaleTimeString()} - ${message}`;

    activityLog.prepend(item);
}
// Business Logic:
// Displays recently executed workflow requests
// in the operations dashboard.

function addRecentRequest(
    requestType,
    requestId,
    requestTitle) {

    const recentRequests =
        document.getElementById(
            "recentRequests"
        );

    if (!recentRequests)
        return;

    const item =
        document.createElement("li");

    item.className =
        "recent-request";

    item.innerHTML = `
        <div class="request-type">
            ${requestType}
        </div>

        <div>
            ${requestTitle}
        </div>

        <small>
            ${requestId}
        </small>
    `;

    recentRequests.prepend(item);

    while (
        recentRequests.children.length > 10
    ) {
        recentRequests.removeChild(
            recentRequests.lastChild
        );
    }
}

async function loadDashboardMetrics() {

    const response =
        await fetch("/api/dashboard/metrics");

    if (!response.ok) {
        return;
    }

    const metrics =
        await response.json();

    document.getElementById("issueCount").textContent =
        metrics.issueCount;

    document.getElementById("pocCount").textContent =
        metrics.pocCount;

    document.getElementById("weekendCount").textContent =
        metrics.weekendExclusionCount;
}

btnChat?.addEventListener("click", () => {

    resetToChatMode();
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
// Business Logic:
// Load dashboard metrics when the application
// starts so the dashboard reflects current
// repository state.

loadDashboardMetrics();