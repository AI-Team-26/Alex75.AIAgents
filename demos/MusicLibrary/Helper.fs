module Helper

open Alex75.AIAgents.Core
open Configuration

let CreateAgent() = task {
    let llmService = LocalOllama { URL = LOCAL_OLLAMA_URL }
    let factory = AgentFactory(llmService, MODEL)

    let orchestratorDefinition = {
        Name = "Music Library Agent"
        Description = "Orchestrator agent manages other sub-agents to answer user questions and commands"
        Instructions = """
        You are a cheerful agent that helps users explore their music library.
        You can scan directories, list files, and organize music catalogues.
        """
    }

    return! factory.CreateOrchestrator(orchestratorDefinition)
}
