open System
open Spectre.Console

// ============================================================================
// MusicLibrary Demo
// ============================================================================
// This demo shows how to use the library to:
// 1. Scan a music directory for MP3/FLAC files
// 2. Generate a structured catalogue (Artist/Album/Song)
// 3. Write output to file or console
// ============================================================================

[<EntryPoint>]
let main argv =
    // Display header
    AnsiConsole.MarkupLine("[bold cyan]MusicLibrary Demo[/]")
    AnsiConsole.MarkupLine("[dim]====================[/]\n")

    // Get user request from command line or prompt
    let request =
        if argv.Length > 0 then
            String.Join(" ", argv)
        else
            AnsiConsole.Prompt(
                TextPrompt<string>("[bold]What would you like to do?[/]")
                    .DefaultValue("What MP3 files are in d:/music")
            )

    AnsiConsole.MarkupLine(sprintf "\n[bold]Request:[/] %s\n" request)

    // Display a sample table (placeholder for future implementation)
    let table = Table()
    table.AddColumn("[bold]Artist[/]") |> ignore
    table.AddColumn("[bold]Album[/]") |> ignore
    table.AddColumn("[bold]Year[/]") |> ignore
    table.AddColumn("[bold]Format[/]") |> ignore
    table.AddColumn("[bold]Files[/]") |> ignore

    table.AddRow("Pink Floyd", "The Dark Side of the Moon", "1973", "MP3", "9") |> ignore
    table.AddRow("Pink Floyd", "The Wall", "1979", "MP3", "26") |> ignore
    table.AddRow("Led Zeppelin", "IV", "1971", "MP3", "8") |> ignore
    table.AddRow("Ange de Poitrine", "Métamorphose", "2024", "FLAC", "12") |> ignore

    AnsiConsole.Write(table)

    AnsiConsole.MarkupLine("\n[yellow]⚠️  Library not yet implemented.[/]")
    AnsiConsole.MarkupLine("[dim]See TODO.md for implementation status.[/]")

    0  // Exit code
