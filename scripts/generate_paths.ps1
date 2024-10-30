param (
    [int]$numPaths = 10,
    [string]$outputDirectory = "./paths"
)

# Funktion zum Generieren von Pfaden
function Generate-Paths {
    param (
        [int]$numPaths,
        [string]$outputDirectory
    )

    # Überprüfe, ob das Ausgabeverzeichnis existiert, und erstelle es, falls nicht
    if (-not (Test-Path -Path $outputDirectory)) {
        New-Item -ItemType Directory -Path $outputDirectory | Out-Null
    }

    for ($i = 0; $i -lt $numPaths; $i++) {
        # Generiere Startkoordinaten
        $startX = Get-Random -Minimum -100000 -Maximum 100000
        $startY = Get-Random -Minimum -100000 -Maximum 100000

        # HashTable für einzigartige Plätze
        $uniquePlaces = @{}

        # Logik zur Generierung von Befehlen
        $commands = @()
        $numCommands = Get-Random -Minimum 1 -Maximum 100

        for ($j = 0; $j -lt $numCommands; $j++) {
            $direction = Get-Random -InputObject @('north', 'east', 'south', 'west')
            $steps = Get-Random -Minimum 1 -Maximum 100000

            $commands += @{
                direction = $direction
                steps = $steps
            }

            # Berechne besuchte Plätze für den aktuellen Befehl
            $currentX = $startX
            $currentY = $startY

            for ($k = 0; $k -lt $steps; $k++) {
                switch ($direction) {
                    'north' { $currentY++ }
                    'east'  { $currentX++ }
                    'south' { $currentY-- }
                    'west'  { $currentX-- }
                }

                # Füge den neuen Platz hinzu
                $uniquePlaces["$currentX,$currentY"] = $true
            }
        }

        # Erstelle das JSON mit dem Start und der Anzahl der einzigartigen Plätze
        $json = @{
            start = @{
                x = $startX
                y = $startY
            }
            commands = $commands
            uniquePlacesCount = $uniquePlaces.Count  # Anzahl der einzigartigen Plätze
        }

        # Speichere das JSON in einer Datei im angegebenen Ausgabeverzeichnis
        $filePath = Join-Path -Path $outputDirectory -ChildPath "path_$i.json"
        $json | ConvertTo-Json -Depth 4 | Set-Content -Path $filePath
    }
}

# Aufruf der Funktion
Generate-Paths -numPaths $numPaths -outputDirectory $outputDirectory
