namespace Alex75.AIAgents.Core

/// Represents a known LLM provider with OpenAI-compatible API
type LlmProvider =
    | AliBaba
    | AliBabaPlan
    | DeepSeek
    | GitHub
    | Mistral
    | Openrouter
    | Xiaomi
    | Google

/// Configuration for a known LLM provider (API key only, URL is predefined)
type KnownProviderConfig = { Provider: LlmProvider; ApiKey: string }

/// Module with LLM provider endpoint URLs and helper functions
module LlmProviders =
    /// Get the API endpoint URL for a known provider
    let getEndpoint (provider: LlmProvider) =
        match provider with
        | AliBaba -> "https://dashscope-intl.aliyuncs.com/compatible-mode/v1"
        | AliBabaPlan -> "https://coding-intl.dashscope.aliyuncs.com/v1"
        | DeepSeek -> "https://api.deepseek.com"
        | GitHub -> "https://models.github.ai/inference"
        | Mistral -> "https://api.mistral.ai/v1"
        | Openrouter -> "https://openrouter.ai/api/v1"
        | Xiaomi -> "https://api.xiaomimimo.com/v1"
        | Google -> "https://generativelanguage.googleapis.com/v1beta/openai"

    /// List of all supported providers
    let all =
        [ AliBaba; AliBabaPlan; DeepSeek; GitHub; Mistral; Openrouter; Xiaomi; Google ]

    /// Check if a provider is supported
    let isSupported (provider: LlmProvider) =
        all |> List.contains provider
