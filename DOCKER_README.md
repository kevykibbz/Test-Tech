# LawVu Tech Test - Docker Setup

This project contains a .NET 8 WebAPI backend and an Angular 20 frontend with Docker support.

## 🐳 Docker Setup

### Prerequisites
- Docker Desktop installed and running
- .NET 8 SDK (for local development)
- Node.js 20+ (for local development)

### Quick Start with Docker Compose

#### Production Build
```bash
# Build and run all services (API + Angular + SQL Server)
docker-compose up --build

# Run in detached mode
docker-compose up -d --build
```

#### Development Build (with hot reload)
```bash
# Build and run with development settings
docker-compose -f docker-compose.dev.yml up --build

# Run in detached mode
docker-compose -f docker-compose.dev.yml up -d --build
```

### Individual Service Builds

#### .NET WebAPI
```bash
# Build the API Docker image
docker build -f WebApi/Dockerfile -t lawvu-webapi .

# Run the API container
docker run -p 9091:80 lawvu-webapi
```

#### Angular WebApp
```bash
# Build the Angular Docker image
cd WebApp
docker build -t lawvu-webapp .

# Run the Angular container
docker run -p 4200:4200 lawvu-webapp
```

### Service URLs
- **Angular Frontend**: http://localhost:4200
- **.NET WebAPI**: http://localhost:9091
- **Ollama AI Service**: http://localhost:11434
- **Docker Dashboard**: http://localhost:15026
- **SQL Server**: localhost:1433 (SA password: `YourPassword123!`)

### Services Included
- **lawvu-techtest-webapp**: Angular frontend with hot reload support
- **lawvu-techtest-webapi**: .NET 9 WebAPI with Ollama integration
- **ollama**: AI service for contract extraction and analysis
- **ollama-model-pull**: Initialization service to download required AI models
- **sqlserver**: SQL Server database
- **dashboard**: Portainer dashboard for container management

### Environment Variables

#### WebAPI Environment Variables
- `ASPNETCORE_ENVIRONMENT`: Development/Production
- `ConnectionStrings__DefaultConnection`: Database connection string
- `ASPNETCORE_URLS`: Binding URLs
- `OllamaEndpoint`: Ollama service endpoint URL

#### Ollama Environment Variables
- `OLLAMA_HOST`: Host binding for Ollama service
- `OllamaConfiguration__DefaultModel`: Default AI model to use
- `OllamaConfiguration__TimeoutMinutes`: Request timeout

#### Angular Environment Variables
- `NODE_ENV`: development/production
- `CHOKIDAR_USEPOLLING`: true (for Docker file watching)

### Docker Commands

#### Stop all services
```bash
docker-compose down
```

#### Stop and remove volumes
```bash
docker-compose down -v
```

#### View logs
```bash
# All services
docker-compose logs

# Specific service
docker-compose logs webapi
docker-compose logs webapp
docker-compose logs sqlserver
```

#### Rebuild specific service
```bash
docker-compose up --build webapi
```

### Development Workflow

1. **Local Development**: Use `docker-compose.dev.yml` for development with hot reload
2. **Production Testing**: Use `docker-compose.yml` for production builds
3. **Database**: SQL Server runs in a container with persistent volume

### Troubleshooting

#### Port Conflicts
If ports are already in use, modify the port mappings in docker-compose.yml:
```yaml
ports:
  - "YOUR_PORT:80"  # Change YOUR_PORT to available port
```

#### Database Connection Issues
- Ensure SQL Server container is running: `docker-compose logs sqlserver`
- Check connection string in appsettings.json
- Verify firewall settings

#### Angular Hot Reload Not Working
- Ensure `CHOKIDAR_USEPOLLING=true` is set
- Check volume mounts in docker-compose.dev.yml
- Restart the webapp service

#### Build Issues
```bash
# Clean Docker cache
docker system prune -a

# Rebuild without cache
docker-compose build --no-cache
```

### File Structure
```
├── docker-compose.yml          # Production compose file
├── docker-compose.dev.yml      # Development compose file
├── .dockerignore               # Docker ignore patterns
├── WebApi/
│   └── Dockerfile             # .NET API Dockerfile
├── WebApp/
│   ├── Dockerfile             # Angular production Dockerfile
│   └── Dockerfile.dev         # Angular development Dockerfile
└── README.md                  # This file
```

### Notes
- The SQL Server container includes a persistent volume for data
- Development mode includes volume mounts for hot reload
- Production builds are optimized for smaller image sizes
- All services are connected via a Docker network
