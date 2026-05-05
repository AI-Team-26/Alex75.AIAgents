# MusicLibrary Demo

Demonstrates Alex75.AIAgents.Core library for music library analysis and organization.

## Overview

MusicLibrary is a **mock-only** demo application that showcases the agent orchestration pattern. It simulates analyzing a music collection without requiring real LLM API keys.

## Quick Start

```bash
cd demos/MusicLibrary
dotnet run -- "What MP3 files are in /music"
```

## Demo Scenarios

### Demo 1: Simple File Listing

List all MP3 files in a directory:

```bash
dotnet run -- "What MP3 files are in d:/music"
```

**Output:**
```
Found 47 MP3 files in d:/music:
- d:/music/Pink Floyd/The Dark Side of the Moon/01 Speak to Me.mp3
- d:/music/Pink Floyd/The Dark Side of the Moon/02 Breathe.mp3
- d:/music/Led Zeppelin/IV/01 Black Mountain Side.mp3
...
```

### Demo 2: Catalogue to File

Generate a structured catalogue and write to a file:

```bash
dotnet run -- "Catalogue the music in d:/music to d:/output.txt"
```

**Output (d:/output.txt):**
```
Music Library Catalogue
=======================

Pink Floyd
  The Dark Side of the Moon (1973)
    01 Speak to Me
    02 Breathe
    03 On the Run
    ...

Led Zeppelin
  IV (1971)
    01 Black Mountain Side
    02 Immigrant Song
    ...
```

### Demo 3: Custom Format (Future)

Request specific output format:

```bash
dotnet run -- "Catalogue music in d:/music as JSON to d:/catalogue.json"
```

## Architecture

This demo uses the **Orchestrator + Context Provider** pattern:

```
┌─────────────────┐
│     User        │
│  "Catalogue     │
│   my music"     │
└────────┬────────┘
         │
         ▼
┌─────────────────┐
│  Orchestrator   │
│  (supervisor)   │
└────────┬────────┘
         │
    ┌────┴────┐
    │         │
    ▼         ▼
┌─────────┐ ┌─────────┐
│ file    │ │ file    │
│ Explorer│ │ Editor  │
│ Agent   │ │ Agent   │
└────┬────┘ └────┬────┘
     │           │
     └─────┬─────┘
           │
           ▼
    ┌─────────────┐
    │   Context   │
    │  (shared    │
    │   memory)   │
    └─────────────┘
```

### Components

| Component | Role | Tools |
|-----------|------|-------|
| **Orchestrator** | Central supervisor | Manages workflow, routes requests |
| **fileExplorerAgent** | Read-only file operations | `scanFiles`, `readFile`, `listDirectories`, `getFileMetadata` |
| **fileEditorAgent** | Read-write file operations | `writeFile`, `createDirectory`, `fileOperation` |
| **ContextProvider** | Shared memory | `Get`, `Set`, `TryGetValue` |

### Workflow Example

**Request:** "Catalogue the music in d:/music to d:/output.txt"

1. **Orchestrator** parses request → identifies need for scanning + writing
2. **fileExplorerAgent** scans directory → writes results to context (`"fileExplorer.output"`)
3. **Orchestrator** formats results → Artist/Album/Song structure
4. **fileEditorAgent** writes formatted output → reads from context, writes to file
5. **Orchestrator** returns confirmation to user

## Project Structure

```
demos/MusicLibrary/
├── MusicLibrary.fsproj    # Project file
├── Program.fs             # Demo entry point
└── README.md              # This file
```

## Why "MusicLibrary"?

The name reflects the dual capabilities:
- **Library** = Collection of music (cataloguing)
- **Library** = Code library (the tool itself)

Future extensions may include:
- Format conversion (FLAC → MP3)
- Metadata editing
- Duplicate detection
- Playlist generation

## Mock vs. Real Implementation

**Current (Mock):**
- Pre-programmed responses
- Simulated file operations
- No LLM API required
- Deterministic output

**Future (Real LLM):**
- Natural language understanding
- Dynamic tool selection
- Requires API key (OpenAI, Azure, etc.)
- Flexible request handling

## See Also

- [Core Library README](../../src/Core/README.md)
- [Project TODO](../../TODO.md)
