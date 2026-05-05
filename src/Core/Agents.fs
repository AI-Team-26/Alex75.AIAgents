namespace Alex75.AIAgents.Core

open System
open System.Threading.Tasks
open Microsoft.Agents.AI
open Microsoft.Agents.AI.OpenAI
open OllamaSharp

// ============================================================================
// LLM Service Configuration
// ============================================================================

type LLMProvider =
    | AliBaba
    | AliBabaPlan
    | DeepSeek
    | GitHub
    | Mistral
    | Openrouter
    | Xiaomi
    | Google

type LocalOllama = { URL: string }
type OpenAICompatibleData = { URL: string; ApiKey: string }
type OpenAICompatibleKnownProvider = { KnownProvider: LLMProvider; ApiKey: string }

type LLMService =
    | LocalOllama of LocalOllama
    | OpenAICompatible of OpenAICompatibleData
    | KnownProvider of OpenAICompatibleKnownProvider

// ============================================================================
// Constants - Known Provider URLs
// ============================================================================

module internal Constants =
    module LLMProviders =
        let ALIBABA_URL = "https://dashscope-intl.aliyuncs.com/compatible-mode/v1"
        let ALIBABA_PLAN_URL = "https://coding-intl.dashscope.aliyuncs.com/v1"
        let DEEPSEEK_URL = "https://api.deepseek.com"
        let GITHUB_URL = "https://models.github.ai/inference"
        let MISTRAL_URL = "https://api.mistral.ai/v1"
        let OPENROUTER_URL = "https://openrouter.ai/api/v1"
        let XIAOMI_URL = "https://api.xiaomimimo.com/v1"
        let GOOGLE_URL = "https://generativelanguage.googleapis.com/v1beta/openai"

// ============================================================================
// Agent Definition
// ============================================================================

type AgentDefinition = {
    Name: string
    Description: string
    Instructions: string
}

// ============================================================================
// Agent Factory
// ============================================================================

type AgentFactory(llmService: LLMService, model: string) =

    let createChatClient () =
        match llmService with 
        | LocalOllama ollama -> 
            let config = OllamaApiClient.Configuration()
            config.Uri <- Uri ollama.URL
            config.Model <- model
            new OllamaApiClient(config) :> _

        | OpenAICompatible openai ->
            let credentials = ClientModel.ApiKeyCredential openai.ApiKey
            let options = OpenAIClientOptions()
            options.Endpoint <- Uri openai.URL
            OpenAIChatClient(model, credentials, options) :> _
            
        | KnownProvider knownProvider ->
            let url = 
                match knownProvider.KnownProvider with
                | LLMProvider.AliBaba -> Constants.LLMProviders.ALIBABA_URL
                | LLMProvider.AliBabaPlan -> Constants.LLMProviders.ALIBABA_PLAN_URL
                | LLMProvider.DeepSeek -> Constants.LLMProviders.DEEPSEEK_URL
                | LLMProvider.GitHub -> Constants.LLMProviders.GITHUB_URL
                | LLMProvider.Mistral -> Constants.LLMProviders.MISTRAL_URL
                | LLMProvider.Openrouter -> Constants.LLMProviders.OPENROUTER_URL
                | LLMProvider.Xiaomi -> Constants.LLMProviders.XIAOMI_URL
                | LLMProvider.Google -> Constants.LLMProviders.GOOGLE_URL

            let credentials = ClientModel.ApiKeyCredential knownProvider.ApiKey
            let options = OpenAIClientOptions()
            options.Endpoint <- Uri url
            OpenAIChatClient(model, credentials, options) :> _

    member this.CreateOrchestrator(definition: AgentDefinition) : Task<AIAgent> = task {
        let chatClient = createChatClient()
        let agent = chatClient.AsAIAgent(definition.Instructions, definition.Name, definition.Description)
        let! session = agent.CreateSessionAsync()
        return session.Agent
    }

    member this.CreateOrchestrator(name: string, description: string, instructions: string) =
        let definition = { Name = name; Description = description; Instructions = instructions }
        this.CreateOrchestrator(definition)
