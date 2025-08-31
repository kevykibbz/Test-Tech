# 🚀 LawVu Tech Test - Quick Deployment Guide

> **Complete Legal Matter Management System with AI-Powered Contract Extraction**

This guide will help you deploy the entire LawVu Tech Test application stack using Docker containers. No development environment setup required!

---

## 📋 **What You'll Get**

After deployment, you'll have access to:

- ✅ **Legal Matter Management System** - Full CRUD operations for legal matters
- ✅ **Lawyer Assignment System** - Multi-select lawyer assignment functionality  
- ✅ **AI Contract Extraction** - Powered by Ollama for intelligent document processing
- ✅ **Real-time Updates** - WebSocket support for live data updates
- ✅ **API Documentation** - Interactive Swagger UI
- ✅ **Container Management** - Portainer dashboard for monitoring

---

## 🛠️ **Prerequisites**

Before you begin, ensure you have:

1. **Docker Desktop** installed and running
   - Download from: https://www.docker.com/products/docker-desktop/
   - Minimum 8GB RAM recommended
   - 10GB free disk space

2. **Internet Connection** (for downloading images)

3. **Available Ports** (ensure these ports are free):
   - `4200` - Angular Frontend
   - `9091` - .NET WebAPI
   - `11434` - Ollama AI Service
   - `15026` - Portainer Dashboard
   - `1433` - SQL Server Database

---

## 🚀 **Quick Start (Recommended)**

### **Option 1: One-Click Deployment Script**

1. **Download the deployment files:**
   ```bash
   # Create a new directory
   mkdir lawvu-techtest
   cd lawvu-techtest
   
   # Download deployment files
   curl -o docker-compose.prod.yml https://raw.githubusercontent.com/kevykibbz/Test-Tech/main/docker-compose.prod.yml
   curl -o deploy-from-hub.ps1 https://raw.githubusercontent.com/kevykibbz/Test-Tech/main/deploy-from-hub.ps1
   ```

2. **Run the deployment script:**
   ```powershell
   # On Windows (PowerShell)
   .\deploy-from-hub.ps1
   ```
   
   ```bash
   # On macOS/Linux
   chmod +x deploy-from-hub.ps1
   ./deploy-from-hub.ps1
   ```

3. **Wait for deployment** (first run may take 5-10 minutes to download images)

4. **Access the application:**
   - **Frontend**: http://localhost:4200
   - **API**: http://localhost:9091
   - **Dashboard**: http://localhost:15026

---

## 🐳 **Option 2: Manual Docker Compose Deployment**

### **Step 1: Create Deployment Directory**
```bash
mkdir lawvu-techtest
cd lawvu-techtest
```

### **Step 2: Create Docker Compose File**

Create a file named `docker-compose.prod.yml` with the following content:

```yaml
services:
  # .NET WebAPI service
  lawvu-techtest-webapi:
    image: kevykibbz/lawvu-webapi:latest
    container_name: lawvu-techtest-webapi
    ports:
      - "9091:80"
    environment:
      - ASPNETCORE_ENVIRONMENT=Production
      - ASPNETCORE_URLS=http://+:80
      - ConnectionStrings__LawVuTechTestDB=Server=sqlserver,1433;Database=LawVuTechTestDB;User Id=sa;Password=YourPassword123!;TrustServerCertificate=true;
      - OllamaEndpoint=http://ollama:11434
    depends_on:
      - sqlserver
      - ollama
    networks:
      - lawvu-network
    restart: unless-stopped

  # Angular WebApp service
  lawvu-techtest-webapp:
    image: kevykibbz/lawvu-webapp:latest
    container_name: lawvu-techtest-webapp
    ports:
      - "4200:4200"
    environment:
      - NODE_ENV=production
    networks:
      - lawvu-network
    restart: unless-stopped
    depends_on:
      - lawvu-techtest-webapi

  # Ollama AI service
  ollama:
    image: ollama/ollama:latest
    container_name: ollama
    ports:
      - "11434:11434"
    volumes:
      - ollama_models:/root/.ollama
    environment:
      - OLLAMA_HOST=0.0.0.0
    networks:
      - lawvu-network
    restart: unless-stopped

  # Ollama model initialization
  ollama-model-pull:
    image: ollama/ollama:latest
    container_name: ollama-model-pull
    depends_on:
      - ollama
    networks:
      - lawvu-network
    restart: "no"
    command: >
      sh -c "
        sleep 10 &&
        ollama pull llama3.2:1b &&
        ollama pull llama3.2:3b
      "
    environment:
      - OLLAMA_HOST=ollama:11434

  # SQL Server Database
  sqlserver:
    image: mcr.microsoft.com/mssql/server:2022-latest
    container_name: sqlserver
    environment:
      - ACCEPT_EULA=Y
      - SA_PASSWORD=YourPassword123!
      - MSSQL_PID=Express
    ports:
      - "1433:1433"
    volumes:
      - sqlserver_data:/var/opt/mssql
    networks:
      - lawvu-network
    restart: unless-stopped

  # Container Management Dashboard
  dashboard:
    image: portainer/portainer-ce:latest
    container_name: lawvu-dashboard
    ports:
      - "15026:9000"
    volumes:
      - /var/run/docker.sock:/var/run/docker.sock
      - portainer_data:/data
    networks:
      - lawvu-network
    restart: unless-stopped

networks:
  lawvu-network:
    driver: bridge

volumes:
  sqlserver_data:
    driver: local
  ollama_models:
    driver: local
  portainer_data:
    driver: local
```

### **Step 3: Deploy the Stack**
```bash
# Pull and start all services
docker-compose -f docker-compose.prod.yml up -d

# Monitor the deployment
docker-compose -f docker-compose.prod.yml logs -f
```

### **Step 4: Wait for Services to Start**
The deployment will:
1. Pull Docker images (may take 5-10 minutes first time)
2. Start all services
3. Initialize the database
4. Download AI models (llama3.2:1b and llama3.2:3b)
5. Set up networking between services

---

## 🌐 **Accessing the Application**

Once deployment is complete, you can access:

| Service | URL | Description |
|---------|-----|-------------|
| **Frontend** | http://localhost:4200 | Angular application (main interface) |
| **API** | http://localhost:9091 | .NET WebAPI backend |
| **API Docs** | http://localhost:9091/swagger | Interactive API documentation |
| **Ollama** | http://localhost:11434 | AI service for contract extraction |
| **Dashboard** | http://localhost:15026 | Container management (Portainer) |
| **Database** | localhost:1433 | SQL Server (for external tools) |

### **Default Credentials**
- **Database**: 
  - Server: `localhost:1433`
  - Username: `sa`
  - Password: `YourPassword123!`
  - Database: `LawVuTechTestDB`

---

## ✅ **Verifying Deployment**

### **Check All Services Are Running**
```bash
# View running containers
docker-compose -f docker-compose.prod.yml ps

# Should show all services as "Up"
```

### **Test Each Service**
1. **Frontend**: Visit http://localhost:4200 - Should show the Angular app
2. **API**: Visit http://localhost:9091/swagger - Should show API documentation  
3. **Ollama**: Visit http://localhost:11434 - Should return Ollama service info
4. **Dashboard**: Visit http://localhost:15026 - Should show Portainer login

### **Check Logs for Issues**
```bash
# View logs for all services
docker-compose -f docker-compose.prod.yml logs

# View logs for specific service
docker-compose -f docker-compose.prod.yml logs lawvu-techtest-webapi
docker-compose -f docker-compose.prod.yml logs lawvu-techtest-webapp
```

---

## 🎯 **Using the Application**

### **Main Features**

1. **Legal Matter Management**
   - Create, view, edit, and delete legal matters
   - Assign lawyers to legal matters
   - Multi-select operations

2. **Contract Extraction (AI-Powered)**
   - Upload PDF contracts
   - Automatic extraction of key information
   - AI-powered analysis using Ollama

3. **Real-time Updates**
   - WebSocket connections for live updates
   - Real-time notifications

### **Getting Started**
1. Open http://localhost:4200
2. Create a new legal matter
3. Try the lawyer assignment feature
4. Upload a contract for AI extraction
5. View API documentation at http://localhost:9091/swagger

---

## 🛠️ **Management Commands**

### **Start Services**
```bash
docker-compose -f docker-compose.prod.yml up -d
```

### **Stop Services**
```bash
docker-compose -f docker-compose.prod.yml down
```

### **Restart Services**
```bash
docker-compose -f docker-compose.prod.yml restart
```

### **Update to Latest Images**
```bash
docker-compose -f docker-compose.prod.yml pull
docker-compose -f docker-compose.prod.yml up -d
```

### **View Service Status**
```bash
docker-compose -f docker-compose.prod.yml ps
```

### **Clean Up Everything**
```bash
# Stop and remove containers, networks, and volumes
docker-compose -f docker-compose.prod.yml down -v

# Remove downloaded images (optional)
docker rmi kevykibbz/lawvu-webapi:latest kevykibbz/lawvu-webapp:latest
```

---

## 🚨 **Troubleshooting**

### **Services Won't Start**

**Check port conflicts:**
```bash
# Windows
netstat -an | findstr "4200\|9091\|11434\|15026\|1433"

# macOS/Linux  
netstat -an | grep -E "(4200|9091|11434|15026|1433)"
```

**Solution**: Stop services using those ports or modify port mappings in the compose file.

### **Database Connection Issues**

**Check SQL Server container:**
```bash
docker logs sqlserver
```

**Common fixes:**
- Ensure SA password meets complexity requirements
- Wait longer for SQL Server to fully start (can take 2-3 minutes)
- Check firewall settings

### **Ollama AI Not Working**

**Check Ollama service:**
```bash
docker logs ollama
docker logs ollama-model-pull
```

**Manual model installation:**
```bash
# Connect to Ollama container and pull models manually
docker exec -it ollama ollama pull llama3.2:1b
docker exec -it ollama ollama pull llama3.2:3b
```

### **Frontend Can't Connect to API**

**Check if API is responding:**
```bash
curl http://localhost:9091/swagger/index.html
```

**Check container networking:**
```bash
docker network ls
docker network inspect lawvu-techtest_lawvu-network
```

### **Performance Issues**

**System Requirements:**
- Minimum 8GB RAM (16GB recommended)
- 4+ CPU cores
- 10GB free disk space
- SSD storage recommended for database

**Resource monitoring:**
```bash
# Monitor container resource usage
docker stats
```

---

## 📊 **System Architecture**

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Angular App   │    │   .NET WebAPI   │    │   Ollama AI     │
│   Port: 4200    │◄──►│   Port: 9091    │◄──►│   Port: 11434   │
│                 │    │                 │    │                 │
│ Legal Matter UI │    │ Business Logic  │    │ Contract Extract│
│ Lawyer Assign   │    │ Data Access     │    │ AI Processing   │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         └───────────────────────┼───────────────────────┘
                                 │
                    ┌─────────────────┐
                    │   SQL Server    │
                    │   Port: 1433    │
                    │                 │
                    │ LawVuTechTestDB │
                    └─────────────────┘
                                 │
                    ┌─────────────────┐
                    │   Portainer     │
                    │   Port: 15026   │
                    │                 │
                    │ Container Mgmt  │
                    └─────────────────┘
```

---

## 📞 **Support**

If you encounter any issues:

1. **Check the logs** using the commands in the troubleshooting section
2. **Verify system requirements** (RAM, disk space, ports)
3. **Review Docker Desktop status** - ensure it's running properly
4. **Check network connectivity** - ensure internet access for image downloads
5. **Create an issue**: https://github.com/kevykibbz/Test-Tech/issues

---

## 🏗️ **For Developers**

If you want to modify the application:

1. **Clone the repository:**
   ```bash
   git clone https://github.com/kevykibbz/Test-Tech.git
   cd Test-Tech
   ```
2. **Use the development compose file:**
   ```bash
   docker-compose -f docker-compose.dev.yml up --build
   ```
3. **Make changes and rebuild:**
   ```bash
   ./build-and-push.ps1  # Push your changes to Docker Hub
   ```

---

**🎉 Enjoy exploring the LawVu Tech Test application!**

> **Need help?** Create an issue at https://github.com/kevykibbz/Test-Tech/issues

---

**Quick Links:**
- 📁 **GitHub Repository**: https://github.com/kevykibbz/Test-Tech
- 🐳 **Docker Hub WebAPI**: https://hub.docker.com/r/kevykibbz/lawvu-webapi
- 🐳 **Docker Hub WebApp**: https://hub.docker.com/r/kevykibbz/lawvu-webapp
- 🌍 **Frontend**: http://localhost:4200
- 🔧 **API Docs**: http://localhost:9091/swagger  
- 📊 **Dashboard**: http://localhost:15026
- 🤖 **AI Service**: http://localhost:11434
