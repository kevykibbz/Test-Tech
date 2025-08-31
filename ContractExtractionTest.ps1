# Contract Extraction Testing Script
# PowerShell script to test Contract Extraction API endpoints

$baseUrl = "http://localhost:9091"

Write-Host "Testing Contract Extraction Endpoints..." -ForegroundColor Green
Write-Host "Base URL: $baseUrl" -ForegroundColor Yellow
Write-Host ("=" * 60) -ForegroundColor Gray

# Test 1: /extract-text endpoint with RAW STRING (correct format)
Write-Host "`nTest 1: Contract Extract-Text (RAW STRING - CORRECT FORMAT)" -ForegroundColor Cyan
Write-Host "Endpoint: POST /ContractExtraction/extract-text" -ForegroundColor White

# The endpoint expects [FromBody] string - so we send a JSON-encoded string
$contractText = "This Service Agreement is entered into on January 15, 2024, between ABC Corporation, a Delaware corporation ('Client') and XYZ Services LLC, a California LLC ('Provider'). The contract value is `$75,000 annually and expires on December 31, 2025. The governing law shall be the State of California."

# Convert to JSON string (just the string value, not an object)
$rawStringBody = $contractText | ConvertTo-Json

Write-Host "Request Body (Raw String):" -ForegroundColor White
Write-Host $rawStringBody -ForegroundColor Gray

try {
    $response1 = Invoke-RestMethod -Uri "$baseUrl/ContractExtraction/extract-text" -Method POST -Body $rawStringBody -ContentType "application/json"
    Write-Host "Success!" -ForegroundColor Green
    Write-Host "Response:" -ForegroundColor White
    Write-Host ($response1 | ConvertTo-Json -Depth 5) -ForegroundColor Green
} catch {
    Write-Host "Failed: $($_.Exception.Message)" -ForegroundColor Red
    
    # Try to get detailed error response
    if ($_.Exception.Response) {
        try {
            $responseStream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($responseStream)
            $responseBody = $reader.ReadToEnd()
            Write-Host "Error Response Body:" -ForegroundColor Yellow
            Write-Host $responseBody -ForegroundColor Red
        } catch {
            Write-Host "Could not read error response body" -ForegroundColor Yellow
        }
    }
}

# Test 2: Simple contract test
Write-Host "`nTest 2: Simple Contract Test" -ForegroundColor Cyan
Write-Host "Endpoint: POST /ContractExtraction/extract-text" -ForegroundColor White

$simpleContract = "Agreement between Company A and Company B dated 2024-01-01. Value: `$5000."
$simpleStringBody = $simpleContract | ConvertTo-Json

Write-Host "Request Body (Simple):" -ForegroundColor White
Write-Host $simpleStringBody -ForegroundColor Gray

try {
    $response2 = Invoke-RestMethod -Uri "$baseUrl/ContractExtraction/extract-text" -Method POST -Body $simpleStringBody -ContentType "application/json"
    Write-Host "Simple contract extraction successful!" -ForegroundColor Green
    Write-Host "Response:" -ForegroundColor White
    Write-Host ($response2 | ConvertTo-Json -Depth 5) -ForegroundColor Green
} catch {
    Write-Host "Simple contract test failed: $($_.Exception.Message)" -ForegroundColor Red
    
    if ($_.Exception.Response) {
        try {
            $responseStream = $_.Exception.Response.GetResponseStream()
            $reader = New-Object System.IO.StreamReader($responseStream)
            $responseBody = $reader.ReadToEnd()
            Write-Host "Error Response Body:" -ForegroundColor Yellow
            Write-Host $responseBody -ForegroundColor Red
        } catch {
            Write-Host "Could not read error response body" -ForegroundColor Yellow
        }
    }
}

# Test 3: Check if /extract endpoint exists (with JSON object)
Write-Host "`nTest 3: Testing /extract endpoint (JSON Object)" -ForegroundColor Cyan
Write-Host "Endpoint: POST /ContractExtraction/extract" -ForegroundColor White

$contractData = @{
    contractText = $contractText
} | ConvertTo-Json

Write-Host "Request Body (JSON Object):" -ForegroundColor White
Write-Host $contractData -ForegroundColor Gray

try {
    $response3 = Invoke-RestMethod -Uri "$baseUrl/ContractExtraction/extract" -Method POST -Body $contractData -ContentType "application/json"
    Write-Host "JSON Object format also works!" -ForegroundColor Green
    Write-Host "Response:" -ForegroundColor White
    Write-Host ($response3 | ConvertTo-Json -Depth 5) -ForegroundColor Green
} catch {
    Write-Host "JSON Object format failed: $($_.Exception.Message)" -ForegroundColor Red
    Write-Host "This is expected if only /extract-text endpoint exists" -ForegroundColor Yellow
}

# Test 4: Check API health
Write-Host "`nTest 4: API Health Check" -ForegroundColor Cyan

try {
    $healthCheck = Invoke-RestMethod -Uri "$baseUrl/swagger/v1/swagger.json" -Method GET
    Write-Host "API is responding (Swagger accessible)" -ForegroundColor Green
} catch {
    Write-Host "API might not be running or accessible" -ForegroundColor Red
    Write-Host "Make sure the application is running on $baseUrl" -ForegroundColor Yellow
}

Write-Host "`n$('=' * 60)" -ForegroundColor Gray
Write-Host "Contract extraction testing completed!" -ForegroundColor Green
Write-Host "`nKey Findings:" -ForegroundColor Yellow
Write-Host "- The /extract-text endpoint expects a RAW STRING (not JSON object)" -ForegroundColor White
Write-Host "- Send: `"Your contract text here`" (with quotes)" -ForegroundColor White
Write-Host "- NOT: {`"contractText`": `"Your text`"}" -ForegroundColor White
