# PowerShell Script to Assign Matters to Lawyer
# Run this from the project root directory

$baseUrl = "http://localhost:9091"
$lawyerId = "02f59f9f-b4f2-4455-8714-c69c6490e489"

# Matter IDs to assign
$matterIds = @(
    "2b3c510d-ea71-436e-a162-09d8059b76de",
    "a1ce582c-66a4-4317-947e-bd39a2f57daa", 
    "317f0ccc-9001-4df3-96ec-be6d11fbc125",
    "a97003ee-c08c-4d48-a312-e21d3769ef2d"
)

Write-Host "🔗 Testing Lawyer-Matter Assignment..." -ForegroundColor Green
Write-Host "Lawyer ID: $lawyerId" -ForegroundColor Yellow

# Test 1: Try bulk assignment endpoint
Write-Host "`n📋 Test 1: Bulk Assignment" -ForegroundColor Cyan
$assignUrl = "$baseUrl/Lawyer/$lawyerId/assign-matters"
$jsonBody = $matterIds | ConvertTo-Json

try {
    $response = Invoke-RestMethod -Uri $assignUrl -Method POST -Body $jsonBody -ContentType "application/json"
    Write-Host "✅ Bulk assignment successful!" -ForegroundColor Green
    Write-Host $response
} catch {
    Write-Host "❌ Bulk assignment failed: $($_.Exception.Message)" -ForegroundColor Red
    
    # Test 2: Try individual matter updates
    Write-Host "`n📝 Test 2: Individual Matter Updates" -ForegroundColor Cyan
    
    foreach ($matterId in $matterIds) {
        Write-Host "Updating matter: $matterId" -ForegroundColor Yellow
        
        # First get the current matter
        try {
            $currentMatter = Invoke-RestMethod -Uri "$baseUrl/LegalMatter/$matterId" -Method GET
            
            # Update with lawyer ID
            $currentMatter.lawyerId = $lawyerId
            $updateJson = $currentMatter | ConvertTo-Json -Depth 10
            
            # Try to update (PUT)
            try {
                $updateResponse = Invoke-RestMethod -Uri "$baseUrl/LegalMatter/$matterId" -Method PUT -Body $updateJson -ContentType "application/json"
                Write-Host "  ✅ Updated matter $matterId" -ForegroundColor Green
            } catch {
                Write-Host "  ❌ PUT failed for $matterId : $($_.Exception.Message)" -ForegroundColor Red
            }
            
        } catch {
            Write-Host "  ❌ Could not get matter $matterId : $($_.Exception.Message)" -ForegroundColor Red
        }
    }
}

# Test 3: Verify assignment by getting lawyer's matters
Write-Host "`n🔍 Test 3: Verify Assignment" -ForegroundColor Cyan
try {
    $lawyerMatters = Invoke-RestMethod -Uri "$baseUrl/Lawyer/$lawyerId/matters" -Method GET
    Write-Host "✅ Lawyer now has $($lawyerMatters.Count) assigned matters:" -ForegroundColor Green
    
    foreach ($matter in $lawyerMatters) {
        Write-Host "  - $($matter.matterName) (ID: $($matter.id))" -ForegroundColor White
    }
} catch {
    Write-Host "❌ Could not get lawyer's matters: $($_.Exception.Message)" -ForegroundColor Red
}

Write-Host "`n🎉 Assignment test completed!" -ForegroundColor Green
