namespace Alex75.AIAgents.Core

open System
open System.Threading.Tasks
open Microsoft.Agents.AI
open Microsoft.Extensions.AI

// ============================================================================
// LLM Service Configuration
// ============================================================================

/// Configuration for local Ollama instance
type LocalOllamaConfig = { URL: string }

/// Configuration for OpenAI-compatible custom endpoints (URL + API key)
type OpenAICompatibleConfig = { URL: string; ApiKey: string }

/// Union type representing all supported LLM service types
type LLMService =
    | LocalOllama of LocalOllamaConfig
    | OpenAICompatible of OpenAICompatibleConfig
    | KnownProvider of KnownProviderConfig

// ============================================================================
// Agent Definition
// ============================================================================

/// Definition for creating an AI agent
type AgentDefinition = {
    Name: string
    Description: string
    Instructions: string
}

// ============================================================================
// Agent Factory
// ============================================================================

/// Factory for creating AI agents with different LLM backends
type AgentFactory(llmService: LLMService, model: string) =

    // TODO: Refactor to reduce duplication between OpenAICompatible and KnownProvider branches
    // Both branches create chat clients the same way, only URL resolution differs.
    // Consider: extract helper function or use polymorphism.
    // Tracking: This is intentional for now as we're still moving pieces around.

    let create (settings: AgentDefinition, chatClient: IChatClient) : AIAgent =
        let chatClientBuilder = chatClient.AsBuilder()
        let chatClient = chatClientBuilder.Build()

        let agentBuilder =
            chatClient.AsAIAgent(settings.Instructions, settings.Name, settings.Description)
                .AsBuilder()

        agentBuilder.Build()

    let createOpenAiCompatibleClient (url: string) (apiKey: string) =
        let credentials = ClientModel.ApiKeyCredential apiKey
        let options = OpenAI.OpenAIClientOptions()
        options.Endpoint <- Uri url
        OpenAI.Chat.ChatClient(model, credentials, options).AsIChatClient()

    let createAgent (definition: AgentDefinition) =
        match llmService with
        | LocalOllama ollama ->
            // Ollama provides OpenAI-compatible API at /v1 endpoint
            let chatClient = createOpenAiCompatibleClient ollama.URL "ollama"
            create(definition, chatClient)

        | OpenAICompatible openai ->
            let chatClient = createOpenAiCompatibleClient openai.URL openai.ApiKey
            create(definition, chatClient)

        | KnownProvider knownProvider ->
            let url = LlmProviders.getEndpoint knownProvider.Provider
            let chatClient = createOpenAiCompatibleClient url knownProvider.ApiKey
            create(definition, chatClient)

    member this.CreateOrchestrator(definition: AgentDefinition) : Task<AIAgent> =
        task {
            let agent = createAgent definition
            return agent
        }

    member this.CreateOrchestrator(name: string, description: string, instructions: string) =
        let definition = { Name = name; Description = description; Instructions = instructions }
        this.CreateOrchestrator(definition)
