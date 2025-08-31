# LawVu Tech Test TODO Checklist

## Pre-Requisites

- [ ] Docker Desktop + WSL2 installed and running
- [ ] Visual Studio 2022 (latest, ASP.NET workload)
- [ ] .NET 8 SDK installed
- [ ] `dotnet-ef` tool installed and updated

## Solution Startup

- [ ] Open Docker Desktop
- [ ] Open `LawVuTechTest.sln` in Visual Studio
- [ ] Set `TechTest.AppHost` as Startup Project
- [ ] Run the solution (`F5` or `Ctrl+F5`)
- [ ] Aspire dashboard opens in browser

## Service Verification

- [ ] SQL Server container running
- [ ] Ollama container (llama3.2:1b model) running
- [ ] Angular WebApp Docker container running (http://localhost:4200)
- [ ] WebApi running (http://localhost:9091)

## API Functionality

- [ ] Create a LegalMatter via `POST /legalmatter/`
- [ ] Retrieve LegalMatter via `GET /legalmatter/{id}`
- [ ] Test contract extraction via `GET /ContractExtraction/`
- [ ] Verify contract extraction returns structured JSON

## Assignment Features

- [ ] Endpoint to write a `DbLawyer` object to the database
- [ ] Endpoint to relate a list of `DBLegalMatters` to a `DbLawyer`
- [ ] GET LegalMatter returns `lawyerCompany` field
- [ ] Unit tests added for `LegalLogic.cs` in one of the test projects
- [ ] Improved LLM prompting strategy for contract extraction

## Frontend API Integration

- [ ] `PUT /legalmatter/{id}` updates a legal matter
- [ ] `DELETE /legalmatter/{id}` deletes a legal matter
- [ ] `GET /legalmatter/Total` returns total count
- [ ] Reference endpoints implemented:
  - [ ] `GET /api/v1/Currencies`
  - [ ] `GET /api/v1/LegalMatterCategories`
  - [ ] `GET /api/v1/People`
  - [ ] `GET /api/v1/People/{id}`
- [ ] System endpoints implemented:
  - [ ] `GET /api/v1/App/EventTypes`
  - [ ] `GET /api/v1/App/EventTypeGroups`
  - [ ] `GET /api/v1/App/Log`
  - [ ] `GET /api/v1/App/Parameters`
- [ ] LegalMatter model updated with all required fields

## Database & Migrations

- [ ] Database migrations applied automatically
- [ ] Can access SQL Server via SSMS or Azure Data Studio

## Architecture & Observability

- [ ] Service discovery, configuration, and logging available via Aspire dashboard
