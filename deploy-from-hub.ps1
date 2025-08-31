# Quick Deploy from Docker Hub
# This script pulls and runs the complete LawVu Tech Test from Docker Hub

Write-Host "Deploying LawVu Tech Test from Docker Hub..." -ForegroundColor Green
Write-Host "Pulling images: kevykibbz/lawvu-webapi:latest, kevykibbz/lawvu-webapp:latest" -ForegroundColor Cyan

# Pull and run from Docker Hub
Write-Host "`nStarting services..." -ForegroundColor Yellow
docker-compose -f docker-compose.prod.yml up -d

if ($LASTEXITCODE -eq 0) {
    Write-Host "`nDeployment successful!" -ForegroundColor Green
    Write-Host "`nService URLs:" -ForegroundColor Cyan
    Write-Host "Angular App:     http://localhost:4200" -ForegroundColor White
    Write-Host "WebAPI:          http://localhost:9091" -ForegroundColor White
    Write-Host "API Docs:        http://localhost:9091/swagger" -ForegroundColor White
    Write-Host "Ollama AI:       http://localhost:11434" -ForegroundColor White
    Write-Host "Dashboard:       http://localhost:15026" -ForegroundColor White
    Write-Host "`nTo check status: docker-compose -f docker-compose.prod.yml ps" -ForegroundColor Yellow
}
else {
    Write-Host "`nDeployment failed!" -ForegroundColor Red
    Write-Host "Check logs with: docker-compose -f docker-compose.prod.yml logs" -ForegroundColor Yellow
}
