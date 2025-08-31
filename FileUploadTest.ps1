# File Upload Contract Extraction Test Script
# PowerShell script to test /ContractExtraction/extract-file endpoint

$baseUrl = "http://localhost:9091"
$filePath = ".\SampleContract.txt"

Write-Host "Testing Contract Extraction File Upload Endpoint..." -ForegroundColor Green
Write-Host "Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host "File: $filePath" -ForegroundColor Yellow
Write-Host ("=" * 60) -ForegroundColor Gray

# Check if sample file exists
if (-not (Test-Path $filePath)) {
    Write-Host "Error: Sample contract file not found at $filePath" -ForegroundColor Red
    Write-Host "Please make sure SampleContract.txt exists in the current directory" -ForegroundColor Yellow
    exit 1
}

Write-Host "`nFile Upload Test: Contract Extract from File" -ForegroundColor Cyan
Write-Host "Endpoint: POST /ContractExtraction/extract-file" -ForegroundColor White
Write-Host "File Size: $((Get-Item $filePath).Length) bytes" -ForegroundColor Gray

# Create multipart form data for file upload
$boundary = [System.Guid]::NewGuid().ToString()
$fileBytes = [System.IO.File]::ReadAllBytes((Resolve-Path $filePath))
$fileName = (Get-Item $filePath).Name

# Build multipart form body
$bodyLines = @(
    "--$boundary",
    "Content-Disposition: form-data; name=`"file`"; filename=`"$fileName`"",
    "Content-Type: text/plain",
    "",
    [System.Text.Encoding]::UTF8.GetString($fileBytes),
    "--$boundary--"
)

$body = $bodyLines -join "`r`n"

Write-Host "`nUploading file and processing..." -ForegroundColor White

try {
    # Start timing the request
    $stopwatch = [System.Diagnostics.Stopwatch]::StartNew()
    
    $response = Invoke-RestMethod -Uri "$baseUrl/ContractExtraction/extract-file" `
                                  -Method POST `
                                  -Body $body `
                                  -ContentType "multipart/form-data; boundary=$boundary" `
                                  -TimeoutSec 120
    
    $stopwatch.Stop()
    
    Write-Host "Success! File processed in $($stopwatch.Elapsed.TotalSeconds) seconds" -ForegroundColor Green
    Write-Host "`nExtracted Information:" -ForegroundColor White
    Write-Host ("=" * 40) -ForegroundColor Gray
    
    # Display the response in a formatted way
    if ($response) {
        # Check if response has common contract fields
        if ($response.parties) {
            Write-Host "Parties:" -ForegroundColor Yellow
            $response.parties | ForEach-Object { Write-Host "  - $_" -ForegroundColor White }
        }
        
        if ($response.contractType) {
            Write-Host "Contract Type: $($response.contractType)" -ForegroundColor Yellow
        }
        
        if ($response.effectiveDate) {
            Write-Host "Effective Date: $($response.effectiveDate)" -ForegroundColor Yellow
        }
        
        if ($response.expirationDate) {
            Write-Host "Expiration Date: $($response.expirationDate)" -ForegroundColor Yellow
        }
        
        if ($response.contractValue) {
            Write-Host "Contract Value: $($response.contractValue)" -ForegroundColor Yellow
        }
        
        if ($response.governingLaw) {
            Write-Host "Governing Law: $($response.governingLaw)" -ForegroundColor Yellow
        }
        
        Write-Host "`nFull Response (JSON):" -ForegroundColor Cyan
        Write-Host ($response | ConvertTo-Json -Depth 5) -ForegroundColor Green
    }
    
} catch {
    Write-Host "Failed: $($_.Exception.Message)" -ForegroundColor Red
    
    # Try to get detailed error response
    if ($_.Exception.Response) {
        try {
            $responseStream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($responseStream)
            $responseBody = $reader.ReadToEnd()
            Write-Host "`nError Response Body:" -ForegroundColor Yellow
            Write-Host $responseBody -ForegroundColor Red
        } catch {
            Write-Host "Could not read error response body" -ForegroundColor Yellow
        }
    }
}

# Alternative method using Invoke-WebRequest (if the above fails)
Write-Host "`n" + ("=" * 60) -ForegroundColor Gray
Write-Host "Alternative Test: Using Invoke-WebRequest" -ForegroundColor Cyan

try {
    $fileContent = Get-Content $filePath -Raw
    $form = @{
        file = Get-Item $filePath
    }
    
    Write-Host "Trying alternative file upload method..." -ForegroundColor White
    
    $response2 = Invoke-WebRequest -Uri "$baseUrl/ContractExtraction/extract-file" `
                                  -Method POST `
                                  -Form $form `
                                  -TimeoutSec 120
    
    Write-Host "Alternative method success!" -ForegroundColor Green
    Write-Host "Response:" -ForegroundColor White
    Write-Host $response2.Content -ForegroundColor Green
    
} catch {
    Write-Host "Alternative method also failed: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n" + ("=" * 60) -ForegroundColor Gray
Write-Host "File upload test completed!" -ForegroundColor Green
Write-Host "`nTips:" -ForegroundColor Yellow
Write-Host "- File upload endpoints may take longer due to LLM processing" -ForegroundColor White
Write-Host "- Check Aspire dashboard for detailed logs" -ForegroundColor White
Write-Host "- The sample file contains a comprehensive software license agreement" -ForegroundColor White
