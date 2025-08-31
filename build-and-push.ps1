# Docker Build and Push Script for LawVu Tech Test
# This script builds and pushes all custom images to Docker Hub

Write-Host "Building and Pushing LawVu Tech Test to Docker Hub..." -ForegroundColor Green
Write-Host "Username: kevykibbz" -ForegroundColor Cyan

# Build and tag images
Write-Host "`nBuilding images..." -ForegroundColor Yellow

# Build WebAPI
Write-Host "`nBuilding LawVu WebAPI..." -ForegroundColor Blue
docker build -f WebApi/Dockerfile -t kevykibbz/lawvu-webapi:latest .
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build WebAPI" -ForegroundColor Red
    exit 1
}

# Build WebApp
Write-Host "`nBuilding LawVu WebApp..." -ForegroundColor Blue
docker build -f WebApp/Dockerfile -t kevykibbz/lawvu-webapp:latest ./WebApp
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to build WebApp" -ForegroundColor Red
    exit 1
}

# Push images to Docker Hub
Write-Host "`nPushing images to Docker Hub..." -ForegroundColor Yellow

Write-Host "`nPushing WebAPI..." -ForegroundColor Blue
docker push kevykibbz/lawvu-webapi:latest
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to push WebAPI" -ForegroundColor Red
    exit 1
}

Write-Host "`nPushing WebApp..." -ForegroundColor Blue
docker push kevykibbz/lawvu-webapp:latest
if ($LASTEXITCODE -ne 0) {
    Write-Host "Failed to push WebApp" -ForegroundColor Red
    exit 1
}

Write-Host "`nAll images successfully pushed to Docker Hub!" -ForegroundColor Green
Write-Host "`nAvailable images:" -ForegroundColor Cyan
Write-Host "kevykibbz/lawvu-webapi:latest" -ForegroundColor White
Write-Host "kevykibbz/lawvu-webapp:latest" -ForegroundColor White

Write-Host "`nTo pull and run the complete stack:" -ForegroundColor Cyan
Write-Host "docker-compose -f docker-compose.prod.yml up" -ForegroundColor White
