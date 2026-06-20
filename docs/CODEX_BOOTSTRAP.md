# Enterprise Assistant - Codex Bootstrap

Project: EnterpriseAssistant

Current Branch:
develop

Goal:
Build an internal Enterprise Assistant using ASP.NET Core, Semantic Kernel, Azure OpenAI and plugin architecture.

Current State:

Completed:

* Foundation architecture
* Semantic Kernel bootstrap
* Assistant Orchestrator
* Knowledge Search
* Plugin Discovery
* Conversation Memory
* Entra ID foundation
* Business Workflows

  * Issue Plugin
  * POC Plugin
  * Weekend Exclusion Plugin

Known Issues:

* Entra ID is mandatory and blocks local startup.
* Azure OpenAI is not fully wired.
* Swagger is not fully configured.
* No usable UI exists.
* ChatController exists.
* AssistantOrchestrator currently contains mock responses.

Immediate Objective:
Create a demoable application.

Requirements:

1. Local startup must work without Azure AD.
2. Local startup must work without Azure OpenAI.
3. Enable Swagger.
4. Create a simple UI.
5. UI should:

   * Show chat box.
   * Call /api/chat.
   * Display response.
6. Keep existing architecture.
7. Add comments only to business logic.
8. Do not redesign the solution.
9. Make the application demo-ready.

Priority:
Working demo > perfect architecture.
