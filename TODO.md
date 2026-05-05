# TODO - Alex75.AIAgents.Core

A .NET F# library for creating AI agents with tools - simple functions over framework complexity.

**Architecture:** Orchestrator + Context Provider pattern
- **Orchestrator:** Central supervisor that manages all agent communication
- **Context Provider:** Shared memory (in-memory or Redis) for passing data between agents
- **Agents:** Stateless workers that read/write to context, don't know about each other

---

## In Progress

- [ ] (nothing currently)

---

## Backlog

### Phase 1: Core Foundation (Mock-Only Demo)

#### Core Types & Interfaces
- [ ] Define `ITool` interface (Name, Description, Execute)
- [ ] Define `IAgent` interface (Name, Tools, Ask)
- [ ] Define `IContextProvider` interface (Get, Set, TryGetValue, Remove, Keys)
- [ ] Define `IAgentOrchestrator` interface (Agents, Context, Execute)

#### Core Implementations (Mock)
- [ ] Implement `InMemoryContextProvider` (thread-safe, ConcurrentDictionary)
- [ ] Implement `ToolBuilder` module (create, createAsync)
- [ ] Implement `MockAgent` (pre-programmed responses for testing)
- [ ] Implement `AgentOrchestrator` (basic coordination)

#### Built-in Agents (Mock)
- [ ] Create `fileExplorerAgent` tools:
  - `scanFiles` - scan directory for files matching pattern
  - `readFile` - read file content
  - `listDirectories` - list subdirectories
  - `getFileMetadata` - get file size, dates, extension
- [ ] Create `fileEditorAgent` tools:
  - `writeFile` - write content to file
  - `createDirectory` - create new directory
  - `fileOperation` - copy, move, delete files

#### MusicLibrary Demo (Mock-Only)
- [ ] Rename `demos/MusicExplorer` → `demos/MusicLibrary`
- [ ] Update demo to use agents and orchestrator
- [ ] Implement **Demo 1**: "What MP3 files are in `<path>`"
  - Simple text output listing files
- [ ] Implement **Demo 2**: "Catalogue music in `<path>` to `<output.txt>`"
  - Artist/Album/Song format
  - Plain text output file

---

### Phase 2: Real LLM Integration (Future)

#### LLM Configuration
- [ ] Define `LlmConfig` type (provider, model, API key, endpoint)
- [ ] Implement `LlmAgent` (real LLM-backed agent)
- [ ] Integrate with Microsoft.Agents.AI or similar

#### Enhanced Orchestrator
- [ ] LLM-based request routing
- [ ] Workflow planning (break request into agent steps)
- [ ] Context key conventions (`"{agent}.input"`, `"{agent}.output"`)

#### Additional Agents (Future)
- [ ] `webSearchAgent` - search the web (requires API key)
- [ ] `codeExplorerAgent` - analyze codebases
- [ ] `dataAnalyzerAgent` - process structured data

---

### Phase 3: Advanced Features (Future)

#### Tools & Extensions
- [ ] FLAC → MP3 conversion tool (requires FFmpeg)
- [ ] Audio metadata extractor (ID3 tags, FLAC metadata)
- [ ] Directory scanner with pagination/summary

#### Context Providers
- [ ] `RedisContextProvider` (for distributed/scalable apps)

#### Infrastructure
- [ ] Add XML documentation to public API
- [ ] Add usage examples to `/src/Core/README.md`
- [ ] Set up unit test project
- [ ] Add integration tests (requires LLM API key)

---

## Done

- [x] Project structure created (F# library)
- [x] Solution file (.slnx format)
- [x] CI/CD workflow (GitHub Actions)
- [x] NuGet package configuration (icon, README)

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
