param (
    [int]$port = 5000,
    [string]$pathDir = "paths"
)

# Lade alle JSON-Dateien aus dem Verzeichnis
$files = Get-ChildItem -Path $pathDir -Filter *.json

foreach ($file in $files) {
    # Lade den JSON-Inhalt der Datei
    $jsonContent = Get-Content -Path $file.FullName -Raw | ConvertFrom-Json

    # Hole die erwartete Anzahl an uniquePlaces aus dem JSON
    $expectedUniquePlaces = $jsonContent.uniquePlaces

    # Schicke die POST-Anfrage
    $response = Invoke-RestMethod -Uri "http://localhost:$port/tibber-developer-test/enter-path" -Method Post -Body ($jsonContent | ConvertTo-Json) -ContentType "application/json"
        Write-Host "Sent $($file.Name) successfully. Server response:"
        Write-Host $response
    # Pr�fen, ob die Antwort den erwarteten Wert zur�ckgibt
    $resultUniquePlaces = $response.result
    if ($resultUniquePlaces -eq $expectedUniquePlaces) {
        Write-Output "Test erfolgreich f�r $($file.Name): Erwartet = $expectedUniquePlaces, Ergebnis = $resultUniquePlaces"
    }
    else {
        Write-Output "Test fehlgeschlagen f�r $($file.Name): Erwartet = $expectedUniquePlaces, Ergebnis = $resultUniquePlaces"
    }
}
