param (
    [string]$directory = "./paths",
    [int]$port = 5000
)

if (!(Test-Path -Path $directory)) {
    Write-Host "Directory $directory does not exist."
    exit 1
}

$files = Get-ChildItem -Path $directory -Filter *.json

# Send one POST request per path file
foreach ($file in $files) {
    $jsonContent = Get-Content -Path $file.FullName -Raw
    $url = "http://localhost:$port/tibber-developer-test/enter-path"

    try {
        $response = Invoke-RestMethod -Uri $url -Method Post -Body $jsonContent -ContentType "application/json"
        Write-Host "Sent $($file.Name) successfully. Server response:"
        Write-Host $response
    }
    catch {
        Write-Host "Failed to send $($file.Name): $_"
    }
}
