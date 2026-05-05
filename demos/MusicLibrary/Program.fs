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

/// Main menu choices
let menuChoices = [
    "Show me what songs are in a directory"
    "Create a catalogue of songs in a directory"
    "Convert FLAC files to MP3"
    "Exit"
]

/// Display main menu and get user selection
let showMenu () =
    AnsiConsole.Prompt(
        SelectionPrompt<string>()
            .Title("[bold]What would you like to do?[/]")
            .AddChoices(menuChoices)
    )

/// Handle user selection
let handleSelection selection = task {
    try
        match selection with
        | "Show me what songs are in a directory" ->
            let path = AnsiConsole.Prompt(
                TextPrompt<string>("[bold]Enter directory path:[/]")
                    .DefaultValue("d:/music")
            )
            do! listSongs path

        | "Create a catalogue of songs in a directory" ->
            AnsiConsole.MarkupLine("[yellow]Catalogue (not yet implemented)[/]")

        | "Convert FLAC files to MP3" ->
            AnsiConsole.MarkupLine("[yellow]Convert FLAC to MP3 (not yet implemented)[/]")

        | "Exit" ->
            AnsiConsole.MarkupLine("[green]Goodbye![/]")

        | _ ->
            AnsiConsole.MarkupLine("[red]Unknown option[/]")
    with ex ->
       AnsiConsole.MarkupLine $"[red]Failed to call Agent.[/]"
       AnsiConsole.WriteException(ex)

    AnsiConsole.MarkupLine ""
}

// ============================================================================
// Main Entry Point
// ============================================================================

[<EntryPoint>]
let main argv =
    // Display header
    AnsiConsole.MarkupLine("[bold cyan]MusicLibrary Demo[/]")
    AnsiConsole.MarkupLine("[dim]====================[/]\n")

    // Main loop - keep showing menu until user exits
    let rec mainLoop () = task {
        let selection = showMenu ()
        do! handleSelection selection

        if selection <> "Exit" then
            return! mainLoop ()
    }

    mainLoop () |> Async.AwaitTask |> Async.RunSynchronously

    0  // Exit code
