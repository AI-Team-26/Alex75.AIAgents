namespace Agents

open System
open System.Threading.Tasks
open Microsoft.Agents.AI
open Microsoft.Extensions.AI

type LlmConfig = { ServiceUrl:string; Model:string }

type AgentDefinition = {
    Name: string
    Description: string
    Instructions: string
}

type AgentBuilder (config:LlmConfig) =

    let getChatClient () : IChatClient =
        failwith "not implemented"

    member __.CreateOrchestrator(definition:AgentDefinition) : Task<AIAgent> = task {

        let chatClient:IChatClient = getChatClient()

        let agent = chatClient.AsAIAgent(definition.Instructions, definition.Name, definition.Description)

        let! session = agent.CreateSessionAsync()

        return agent :> AIAgent
    }