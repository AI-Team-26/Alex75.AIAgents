# TODO - Alex75.AIAgents.Core

A .NET F# library for creating AI agents with tools - simple functions over framework complexity.

**Architecture:** Orchestrator + Context Provider pattern
- **Orchestrator:** Central supervisor that manages all agent communication
- **Context Provider:** Shared memory (Redis or in-memory) for passing data between agents
- **Agents:** Stateless workers that read/write to context, don't know about each other

---

## Backlog

### Core Types & Interfaces
- [ ] Define `IContextProvider` interface (Get, Set, Delete, Keys)
- [ ] Implement `InMemoryContextProvider` (for simple apps)
- [ ] Implement `RedisContextProvider` (for distributed/scalable apps)
- [ ] Define `IAgent` interface (Ask, Tools, Context)
- [ ] Define `ITool` interface (Name, Description, Execute)

### Core API (Tier 1 - Simple Helpers)
- [ ] Define `LlmConfig` type (provider, model, API key, endpoint)
- [ ] Implement `Agents.createAgent` - basic agent from LLM config + context
- [ ] Implement `Agents.createAgentWithTools` - agent with custom tools + context
- [ ] Implement `Tools.create` - create tool from function
- [ ] Implement `Tools.createFromFunctions` - batch create tools from module

### Ready-to-Use Agents (Tier 2)
- [ ] Implement `Agents.fileExplorerAgent` - **READ-ONLY**: browse directories, list files, read contents
  - Writes to context: `"{agentName}.output"` = file list
- [ ] Implement `Agents.fileEditorAgent` - **READ-WRITE**: create, delete, move, copy, modify files
  - Reads from context: `"{agentName}.input"` = files to process
- [ ] Implement `Agents.webSearchAgent` - search the web (requires API key)

### Orchestrator
- [ ] Implement `AgentOrchestrator` class
  - `AddAgent(agent)` - register agent with orchestrator
  - `Ask(question)` - LLM-based routing and workflow execution
  - Internal: Manages context keys for agent communication
- [ ] Implement workflow planning (LLM breaks request into agent steps)
- [ ] Implement context key conventions (`"{agent}.input"`, `"{agent}.output"`)

### Example Tools
- [ ] FLAC → MP3 conversion tool (example custom tool)
- [ ] Audio metadata extractor (ID3 tags, FLAC metadata)
- [ ] Directory scanner for media files (with pagination/summary)

### Music Library Analyzer (Example App)
- [ ] Create demo console app: `demos/MusicExplorer`
- [ ] Implement orchestrator workflow for music analysis
- [ ] Demo: "Analyze /home/music and show my collection"
- [ ] Demo: "Convert all FLAC to MP3 in /home/music"

### Infrastructure
- [ ] Add XML documentation to public API
- [ ] Add usage examples to `/src/Core/README.md`
- [ ] Set up unit test project
- [ ] Add integration tests (requires LLM API key)

---

## In Progress

- [ ] (nothing currently)

---

## Done

*(nothing yet)*

---

## Design Principles

**Library, not Framework:**
- Simple functions: `Agents.createAgent config "name" "instructions" context`
- No inheritance required
- No interface implementation (except internal interfaces)
- Opt-in complexity (Builder pattern for advanced users)

**Tiers:**
- Tier 1: Simple helpers (80% of users)
- Tier 2: Ready-to-use agents (50% of users)
- Tier 3: Extensibility (20% of users)

**Context Provider Pattern:**
- Agents don't communicate directly
- All data passes through shared context
- Orchestrator manages workflow and context keys
- Large data (15,000 files) stays in context, not LLM prompts

---

## Architecture Notes

### Orchestrator + Context Provider Flow

```
User Request → Orchestrator
    ↓
Orchestrator LLM plans: "Need fileExplorerAgent, then fileEditorAgent"
    ↓
Step 1: fileExplorerAgent.Ask("Find FLAC files")
    → Writes to context: "fileExplorer.output" = [files]
    ↓
Step 2: fileEditorAgent.Ask("Convert files from context")
    → Reads from context: "fileExplorer.output"
    → Writes to context: "fileEditor.output" = [converted]
    ↓
Orchestrator returns final result to user
```

### Context Key Conventions

| Key Pattern | Purpose | Who Writes | Who Reads |
|-------------|---------|------------|-----------|
| `"{agent}.input"` | Input for agent | Orchestrator | Agent |
| `"{agent}.output"` | Agent results | Agent | Orchestrator / Other agents |
| `"{agent}.status"` | Progress/errors | Agent | Orchestrator (for monitoring) |
| `"orchestrator.summary"` | Final summary | Orchestrator | User |

### Why Context Provider?

1. **Avoids context overflow:** Large data (15,000 files) stored in context, not passed via LLM prompts
2. **Decouples agents:** Agents don't know about each other, only about context
3. **Centralizes control:** Orchestrator manages all communication and workflow
4. **Flexible storage:** In-memory for simple apps, Redis for distributed/scalable apps
