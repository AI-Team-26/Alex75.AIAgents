open System
open Spectre.Console

// ============================================================================
// MusicExplorer Demo - Alex75.AIAgents.Core
// ============================================================================
// This demo shows how to use the library to:
// 1. Scan a music directory for FLAC/MP3 files
// 2. Extract metadata (artist, album, etc.)
// 3. Convert FLAC files to MP3
// 4. Organize results by artist/album
// ============================================================================

[<EntryPoint>]
let main argv =
    // Display header
    AnsiConsole.MarkupLine("[bold cyan]MusicExplorer Demo - Alex75.AIAgents.Core[/]")
    AnsiConsole.MarkupLine("[dim]==========================================[/]\n")
    
    // Get user request from command line or prompt
    let request = 
        if argv.Length > 0 then 
            String.Join(" ", argv)
        else
            AnsiConsole.Prompt(
                TextPrompt<string>("[bold]What would you like to do?[/]")
                    .DefaultValue("Show me my music collection")
            )
    
    AnsiConsole.MarkupLine(sprintf "\n[bold]Request:[/] %s\n" request)
    
    // Display a sample table (placeholder for future implementation)
    let table = Table()
    table.AddColumn("[bold]Artist[/]") |> ignore
    table.AddColumn("[bold]Album[/]") |> ignore
    table.AddColumn("[bold]Year[/]") |> ignore
    table.AddColumn("[bold]Format[/]") |> ignore
    table.AddColumn("[bold]Files[/]") |> ignore
    
    table.AddRow("Pink Floyd", "The Dark Side of the Moon", "1973", "FLAC", "9") |> ignore
    table.AddRow("Pink Floyd", "The Wall", "1979", "FLAC", "26") |> ignore
    table.AddRow("Led Zeppelin", "IV", "1971", "MP3", "8") |> ignore
    table.AddRow("Angine de Poitrine", "Métamorphose", "2024", "FLAC", "12") |> ignore
    
    AnsiConsole.Write(table)
    
    AnsiConsole.MarkupLine("\n[yellow]⚠️  Library not yet implemented.[/]")
    AnsiConsole.MarkupLine("[dim]See TODO.md for implementation status.[/]")
    
    0  // Exit code
