open System
open Spectre.Console

// ============================================================================
// MusicLibrary Demo
// ============================================================================
// This demo shows how to use the library to:
// 1. Scan a music directory for MP3/FLAC files
// 2. Generate a structured catalogue (Artist/Album/Song)
// 3. Convert FLAC files to MP3
// ============================================================================

/// List songs in a directory (placeholder)
let listSongs (directory: string) = task {
    // TODO: Implement with fileExplorerAgent

    let question = $"List songs in {directory}"

    AnsiConsole.MarkupLine $"[yellow]{question}[/]"

    let! agent = Helper.CreateAgent()
    // Add FileSystemAgent(drectory)

    let! response = agent.RunAsync(question)

    let answer = response.Text

    AnsiConsole.MarkupLine $"[cyan]{answer}[/]"
}


// Display header
AnsiConsole.MarkupLine("[bold cyan]MusicLibrary Demo[/]")
AnsiConsole.MarkupLine("[dim]====================[/]\n")

// Main menu selector
let choices = [
    "Show me what songs are in a directory"
    "Create a catalogue of songs in a directory"
    "Convert FLAC files to MP3"
]

let selection = AnsiConsole.Prompt(
    SelectionPrompt<string>()
        .Title("[bold]What would you like to do?[/]")
        .AddChoices(choices)
)
    
(task {

    try
        match selection with
        | "Show me what songs are in a directory" ->
            let path = AnsiConsole.Prompt(
                TextPrompt<string>("[bold]Enter directory path:[/]")
                    .DefaultValue("d:/music")
            )
            listSongs path |> ignore

        | "Create a catalogue of songs in a directory" ->
            AnsiConsole.MarkupLine("[yellow]Catalogue (not yet implemented)[/]")

        | "Convert FLAC files to MP3" ->
            AnsiConsole.MarkupLine("[yellow]Convert FLAC to MP3 (not yet implemented)[/]")

        | _ ->
            AnsiConsole.MarkupLine("[red]Unknown option[/]")
    with ex ->
       AnsiConsole.MarkupLine $"[red]Failed to call Agent.[/]"
       AnsiConsole.WriteException(ex)

})//.Spinner(Spinner.Known.Aesthetic)
|> Async.AwaitTask |> Async.RunSynchronously
