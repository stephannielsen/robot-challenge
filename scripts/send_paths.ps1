param (
    [int]$port = 5000,
    [string]$pathDir = "paths"
)

$files = Get-ChildItem -Path $pathDir -Filter *.json

foreach ($file in $files) {
    $jsonContent = Get-Content -Path $file.FullName -Raw | ConvertFrom-Json

    $expectedUniquePlaces = $jsonContent.uniquePlaces

    $response = Invoke-RestMethod -Uri "http://localhost:$port/tibber-developer-test/enter-path" -Method Post -Body ($jsonContent | ConvertTo-Json) -ContentType "application/json"
        Write-Host "Sent $($file.Name) successfully. Server response:"
        Write-Host $response
    $resultUniquePlaces = $response.result
    if ($resultUniquePlaces -eq $expectedUniquePlaces) {
        Write-Output "Test successful for $($file.Name): Expected = $expectedUniquePlaces, Actual = $resultUniquePlaces"
    }
    else {
        Write-Output "Test failed for $($file.Name): Expected = $expectedUniquePlaces, Actual = $resultUniquePlaces"
    }
}
