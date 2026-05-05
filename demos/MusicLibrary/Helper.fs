module Helper

open System
open Agents

let CreateAgent() = task {

    let serviceUrl = Configuration.LOCAL_OLLAMA_URL
    let model = Configuration.MODEL

    let llmConfig:LlmConfig = { ServiceUrl=serviceUrl; Model=model}

    let agentBuilder = Agents.AgentBuilder(llmConfig)

    let orchestratorDefinition = {
        Name = "Music Library Agent"
        Description = "Orchestraor agent manage other sub-agents to answer user question and commands"
        Instructions = """
        You are a cherfull agent.
        """
    }

    return! agentBuilder.CreateOrchestrator(orchestratorDefinition)
}