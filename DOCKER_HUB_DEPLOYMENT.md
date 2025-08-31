# 🐳 LawVu Tech Test - Docker Hub Deployment

This project is available on Docker Hub with all services containerized for easy deployment.

## 📦 **Available Images**

### **Your Custom Images**
- **`kevykibbz/lawvu-webapi:latest`** - .NET 9 WebAPI with Ollama integration
- **`kevykibbz/lawvu-webapp:latest`** - Angular 20 frontend (production build)
- **`kevykibbz/lawvu-webapp:dev`** - Angular 20 frontend (development with hot reload)

### **Third-Party Images**
- **`ollama/ollama:latest`** - AI service for contract extraction
- **`mcr.microsoft.com/mssql/server:2022-latest`** - SQL Server database
- **`portainer/portainer-ce:latest`** - Docker management dashboard

---

## 🚀 **Quick Deployment**

### **Option 1: Production Deployment (Pull from Docker Hub)**
```bash
# Clone the repo (for docker-compose files only)
git clone https://github.com/kevykibbz/Test-Tech.git
cd Test-Tech

# Deploy the entire stack from Docker Hub
docker-compose -f docker-compose.prod.yml up -d
```

### **Option 2: Development Build & Push**
```bash
# Build and push your custom images
./build-and-push.ps1

# Run with local builds
docker-compose up --build
```

---

## 🌐 **Service URLs**

After deployment, access these services:

- **🌍 Angular Frontend**: http://localhost:4200
- **🔧 .NET WebAPI**: http://localhost:9091
- **🤖 Ollama AI Service**: http://localhost:11434
- **📊 Docker Dashboard**: http://localhost:15026
- **🗄️ SQL Server**: localhost:1433

---

## 📋 **Docker Compose Configurations**

### **docker-compose.yml** (Build + Push)
- Builds images locally and tags them for Docker Hub
- Used for development and pushing updates

### **docker-compose.prod.yml** (Pull-Only)
- Pulls pre-built images from Docker Hub
- Perfect for production deployment
- No build dependencies required

### **docker-compose.dev.yml** (Development)
- Hot reload support for Angular
- Development environment with debugging

---

## 🛠️ **Development Workflow**

### **Building and Pushing Updates**
```powershell
# 1. Make your changes
# 2. Build and push to Docker Hub
./build-and-push.ps1

# 3. Test the production deployment
docker-compose -f docker-compose.prod.yml up -d
```

### **Local Development**
```bash
# For development with hot reload
docker-compose -f docker-compose.dev.yml up --build
```

---

## 📊 **Architecture Overview**

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Angular App   │    │   .NET WebAPI   │    │   Ollama AI     │
│   Port: 4200    │◄──►│   Port: 9091    │◄──►│   Port: 11434   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         └───────────────────────┼───────────────────────┘
                                 │
                    ┌─────────────────┐
                    │   SQL Server    │
                    │   Port: 1433    │
                    └─────────────────┘
                                 │
                    ┌─────────────────┐
                    │   Portainer     │
                    │   Port: 15026   │
                    └─────────────────┘
```

---

## 🔧 **Configuration**

### **Environment Variables**
```yaml
# WebAPI Configuration
ASPNETCORE_ENVIRONMENT: Production
ConnectionStrings__LawVuTechTestDB: Server=sqlserver,1433;Database=LawVuTechTestDB;...
OllamaEndpoint: http://ollama:11434

# Database Configuration
SA_PASSWORD: YourPassword123!
ACCEPT_EULA: Y

# Ollama Configuration
OLLAMA_HOST: 0.0.0.0
```

### **Required Models**
The system automatically downloads these AI models:
- `llama3.2:1b` (lightweight, fast responses)
- `llama3.2:3b` (better accuracy, more resource intensive)

---

## 🎯 **Features**

✅ **Complete Legal Matter Management**
- CRUD operations for legal matters
- Lawyer assignment with multi-select
- Contract extraction using AI
- Real-time updates via WebSockets

✅ **AI-Powered Contract Analysis**
- Automatic contract parsing
- Key information extraction
- Ollama integration for natural language processing

✅ **Modern Tech Stack**
- .NET 9 WebAPI
- Angular 20 with standalone components
- Entity Framework Core
- SQL Server database
- Docker containerization

✅ **Production Ready**
- Horizontal scaling support
- Health checks
- Logging and monitoring
- Secure database connections

---

## 🚨 **Troubleshooting**

### **Container Won't Start**
```bash
# Check logs
docker logs lawvu-techtest-webapi
docker logs lawvu-techtest-webapp

# Restart services
docker-compose restart
```

### **Database Connection Issues**
```bash
# Check SQL Server is running
docker logs sqlserver

# Verify connection string in container
docker exec lawvu-techtest-webapi env | grep ConnectionStrings
```

### **Ollama Not Responding**
```bash
# Check Ollama service
docker logs ollama

# Manually pull models
docker exec ollama ollama pull llama3.2:1b
```

---

## 📈 **Scaling**

### **Horizontal Scaling**
```yaml
# Add to docker-compose.prod.yml
deploy:
  replicas: 3
  resources:
    limits:
      memory: 1G
    reservations:
      memory: 512M
```

### **Load Balancing**
Add nginx or traefik for load balancing multiple WebAPI instances.

---

## 🔗 **Links**

- **Docker Hub WebAPI**: https://hub.docker.com/r/kevykibbz/lawvu-webapi
- **Docker Hub WebApp**: https://hub.docker.com/r/kevykibbz/lawvu-webapp
- **Portainer Dashboard**: http://localhost:15026
- **API Documentation**: http://localhost:9091/swagger

---

**🎉 Ready to deploy! Your complete LawVu Tech Test is now available on Docker Hub.**
