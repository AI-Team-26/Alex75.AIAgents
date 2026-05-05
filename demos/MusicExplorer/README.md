# MusicExplorer Demo

Demonstrates Alex75.AIAgents.Core library for music library analysis and FLAC→MP3 conversion.

## Prerequisites

- .NET 10 SDK
- LLM API key (OpenAI, Azure, etc.)

## Usage

```bash
dotnet run -- "Find FLAC files in d:/music/Ange de Poitrine and convert to MP3"
```

## Examples

### Analyze music library
```bash
dotnet run -- "Show me my music collection in d:/music"
```

### Convert FLAC to MP3
```bash
dotnet run -- "Convert all FLAC files in d:/music to MP3 in d:/mp3"
```

### Count songs by artist
```bash
dotnet run -- "How many songs by Pink Floyd in d:/music?"
```

## Architecture

This demo uses:
- **AgentOrchestrator** - Central supervisor managing workflow
- **ContextProvider** - Shared memory for agent communication
- **fileExplorerAgent** - Read-only file system access
- **fileEditorAgent** - Read-write file operations
- **Custom conversion tool** - FLAC to MP3 conversion

See `Program.fs` for implementation details.
