# 🏛️ LawVu Tech Test - Legal Matter Management System

> **Full-stack legal matter management application with AI-powered contract extraction**

[![Docker](https://img.shields.io/badge/docker-%230db7ed.svg?style=for-the-badge&logo=docker&logoColor=white)](https://hub.docker.com/u/kevykibbz)
[![.NET](https://img.shields.io/badge/.NET-9.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Angular](https://img.shields.io/badge/Angular-20-DD0031?style=for-the-badge&logo=angular&logoColor=white)](https://angular.io/)
[![SQL Server](https://img.shields.io/badge/SQL%20Server-2022-CC2927?style=for-the-badge&logo=microsoft-sql-server&logoColor=white)](https://www.microsoft.com/en-us/sql-server)

![LawVu Logo](/Documentation/lawvu-logo.png?raw=true "LawVu")

A comprehensive legal matter management system built with modern technologies, featuring AI-powered contract extraction, real-time updates, and multi-lawyer assignment capabilities.

---

## 🚀 **Quick Start**

### **🐳 Deploy with Docker (Recommended)**

```bash
# Download deployment script
curl -o deploy-from-hub.ps1 https://raw.githubusercontent.com/kevykibbz/Test-Tech/main/deploy-from-hub.ps1

# Run deployment
./deploy-from-hub.ps1
```

**Access the application:**
- 🌍 **Frontend**: http://localhost:4200
- 🔧 **API Docs**: http://localhost:9091/swagger
- 📊 **Dashboard**: http://localhost:15026

### **📖 Detailed Setup**
For complete deployment instructions, see: [**DEPLOYMENT_GUIDE.md**](DEPLOYMENT_GUIDE.md)

---

## ✨ **Features**

### 🏛️ **Legal Matter Management**
- ✅ **Full CRUD Operations** - Create, read, update, delete legal matters
- ✅ **Advanced Search & Filtering** - Find matters quickly
- ✅ **Multi-select Operations** - Bulk actions on multiple matters
- ✅ **Data Validation** - Comprehensive input validation and error handling

### 👨‍💼 **Lawyer Assignment System**
- ✅ **Individual Assignment** - Assign lawyers to specific matters
- ✅ **Bulk Assignment** - Assign multiple matters to lawyers simultaneously
- ✅ **Assignment History** - Track lawyer assignment changes
- ✅ **Search & Filter Lawyers** - Easy lawyer selection interface

### 🤖 **AI-Powered Contract Extraction**
- ✅ **PDF Processing** - Upload and analyze contract documents
- ✅ **Intelligent Extraction** - AI-powered key information extraction
- ✅ **Contract Analysis** - Automated contract type detection
- ✅ **Ollama Integration** - Local AI processing for data privacy

### 🔄 **Real-time Features**
- ✅ **WebSocket Support** - Real-time updates across clients
- ✅ **Live Notifications** - Instant feedback on actions
- ✅ **Concurrent User Support** - Multi-user environment ready

### 📊 **Data Management**
- ✅ **SQL Server Database** - Robust data storage
- ✅ **Entity Framework Core** - Modern ORM with migrations
- ✅ **Data Relationships** - Proper foreign key constraints
- ✅ **Pagination** - Efficient large dataset handling

---

## 🏗️ **Technology Stack**

### **Backend**
- **🔧 .NET 9** - Latest .NET framework
- **🗄️ Entity Framework Core** - Data access layer
- **📊 SQL Server 2022** - Database engine
- **📝 Swagger/OpenAPI** - API documentation
- **🔗 SignalR/WebSockets** - Real-time communication

### **Frontend**
- **🅰️ Angular 20** - Modern web framework
- **📱 Standalone Components** - Latest Angular architecture
- **🎨 Responsive Design** - Mobile-friendly interface
- **🔄 Reactive Forms** - Advanced form handling
- **📡 HTTP Interceptors** - Centralized API communication

### **AI & Services**
- **🤖 Ollama** - Local AI model hosting
- **🦙 Llama 3.2** - Language models (1B and 3B parameters)
- **📄 PDF Processing** - Contract document analysis
- **🧠 Natural Language Processing** - Intelligent text extraction

### **DevOps & Infrastructure**
- **🐳 Docker & Docker Compose** - Containerization
- **🔧 Multi-stage Builds** - Optimized container images
- **📊 Portainer** - Container management
- **🔄 Health Checks** - Service monitoring
- **📝 Comprehensive Logging** - Application observability

---

## 📁 **Project Structure**

```
Test-Tech/
├── 🌐 WebApp/                     # Angular 20 Frontend
│   ├── src/app/
│   │   ├── legal-matters/         # Legal matter management
│   │   ├── lawyer-assignment/     # Lawyer assignment modal
│   │   └── services/              # API services
│   ├── Dockerfile                 # Production build
│   └── Dockerfile.dev             # Development build
│
├── 🔧 WebApi/                     # .NET 9 WebAPI
│   ├── Controllers/               # API endpoints
│   ├── Program.cs                 # Application startup
│   └── Dockerfile                 # API container build
│
├── 🧠 AppLogic/                   # Business Logic Layer
│   ├── Contracts/                 # Contract extraction services
│   ├── Ollama/                    # AI service integration
│   └── DependencyInjection/       # Service registration
│
├── 🗄️ DataAccess/                 # Data Access Layer
│   ├── TechTestDbContext.cs       # EF Core context
│   ├── Repositories/              # Data repositories
│   ├── Migrations/                # Database migrations
│   └── DataModel/                 # Entity models
│
├── 📋 ServiceModel/               # Shared models
│
├── 🧪 Tests/                      # Test projects
│   ├── AppLogic.Tests.XUnit/
│   ├── AppLogic.Tests.NUnit/
│   └── AppLogic.Tests.MsTest/
│
├── 🐳 Docker Configuration
│   ├── docker-compose.yml         # Development build
│   ├── docker-compose.prod.yml    # Production deployment
│   ├── docker-compose.dev.yml     # Development with hot reload
│   └── .dockerignore              # Docker ignore patterns
│
├── 📚 Documentation
│   ├── DEPLOYMENT_GUIDE.md        # Complete deployment guide
│   ├── DOCKER_HUB_DEPLOYMENT.md   # Docker Hub instructions
│   ├── API_TESTING_GUIDE.md       # API testing documentation
│   └── IMPLEMENTATION_SUMMARY.md  # Development summary
│
└── 🛠️ Scripts
    ├── build-and-push.ps1         # Build and push to Docker Hub
    └── deploy-from-hub.ps1        # Quick deployment script
```

---

## 🐳 **Docker Hub Images**

Pre-built images available on Docker Hub:

| Image | Description | Size |
|-------|-------------|------|
| [`kevykibbz/lawvu-webapi:latest`](https://hub.docker.com/r/kevykibbz/lawvu-webapi) | .NET 9 WebAPI with Ollama integration | ~200MB |
| [`kevykibbz/lawvu-webapp:latest`](https://hub.docker.com/r/kevykibbz/lawvu-webapp) | Angular 20 production build | ~50MB |

---

## 🎯 **API Endpoints**

### **Legal Matters**
- `GET /legalmatter` - Get all legal matters (paginated)
- `POST /legalmatter` - Create new legal matter
- `PUT /legalmatter/{id}` - Update legal matter
- `DELETE /legalmatter/{id}` - Delete legal matter
- `GET /legalmatter/{id}` - Get specific legal matter

### **Lawyer Management**
- `GET /lawyer` - Get all lawyers
- `POST /legalmatter/{id}/assign-lawyer` - Assign lawyer to matter
- `POST /legalmatter/bulk-assign-lawyer` - Bulk assign lawyer

### **Contract Extraction**
- `POST /contract/extract` - Extract data from contract PDF
- `POST /contract/analyze` - Analyze contract content
- `GET /contract/extraction-history` - Get extraction history

### **Real-time**
- `WebSocket /ws` - Real-time updates and notifications

**📖 Full API Documentation**: http://localhost:9091/swagger

---

## 🚦 **Quick Commands**

### **Development**
```bash
# Clone repository
git clone https://github.com/kevykibbz/Test-Tech.git
cd Test-Tech

# Run with hot reload
docker-compose -f docker-compose.dev.yml up --build

# Build and push to Docker Hub
./build-and-push.ps1
```

### **Production**
```bash
# Deploy from Docker Hub
docker-compose -f docker-compose.prod.yml up -d

# Check service status
docker-compose -f docker-compose.prod.yml ps

# View logs
docker-compose -f docker-compose.prod.yml logs -f
```

### **Management**
```bash
# Stop all services
docker-compose down

# Clean up everything
docker-compose down -v

# Update to latest images
docker-compose pull && docker-compose up -d
```

---

## 🧪 **Testing**

The project includes comprehensive test suites:

```bash
# Run all tests
dotnet test

# Run specific test project
dotnet test Tests/AppLogic.Tests.XUnit/
dotnet test Tests/AppLogic.Tests.NUnit/
dotnet test Tests/AppLogic.Tests.MsTest/
```

### **Test Coverage**
- ✅ **Unit Tests** - Business logic testing
- ✅ **Integration Tests** - API endpoint testing
- ✅ **Contract Extraction Tests** - AI service testing
- ✅ **Database Tests** - Repository layer testing

---

## 📊 **Architecture Overview**

```
┌─────────────────┐    ┌─────────────────┐    ┌─────────────────┐
│   Angular App   │    │   .NET WebAPI   │    │   Ollama AI     │
│   Port: 4200    │◄──►│   Port: 9091    │◄──►│   Port: 11434   │
│                 │    │                 │    │                 │
│ • Legal Matters │    │ • Business Logic│    │ • Contract Extract
│ • Lawyer Assign │    │ • Data Access   │    │ • AI Processing │
│ • Real-time UI  │    │ • WebSocket Hub │    │ • Model Hosting │
└─────────────────┘    └─────────────────┘    └─────────────────┘
         │                       │                       │
         └───────────────────────┼───────────────────────┘
                                 │
                    ┌─────────────────┐
                    │   SQL Server    │
                    │   Port: 1433    │
                    │                 │
                    │ • Legal Matters │
                    │ • Lawyers       │
                    │ • Assignments   │
                    └─────────────────┘
```

---

## 🔧 **Configuration**

### **Environment Variables**
```yaml
# WebAPI
ASPNETCORE_ENVIRONMENT: Development/Production
ConnectionStrings__LawVuTechTestDB: [SQL Connection String]
OllamaEndpoint: http://ollama:11434

# Database
SA_PASSWORD: YourPassword123!
ACCEPT_EULA: Y

# Ollama
OLLAMA_HOST: 0.0.0.0
```

### **Default Credentials**
- **Database**: `sa` / `YourPassword123!`
- **Portainer**: Create admin account on first access

---

## 🤝 **Contributing**

1. **Fork the repository**
2. **Create feature branch**: `git checkout -b feature/amazing-feature`
3. **Commit changes**: `git commit -m 'Add amazing feature'`
4. **Push to branch**: `git push origin feature/amazing-feature`
5. **Open Pull Request**

---

## 📄 **License**

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

---

## 🙋‍♂️ **Support**

- **📖 Documentation**: [DEPLOYMENT_GUIDE.md](DEPLOYMENT_GUIDE.md)
- **🐛 Issues**: [GitHub Issues](https://github.com/kevykibbz/Test-Tech/issues)
- **💬 Discussions**: [GitHub Discussions](https://github.com/kevykibbz/Test-Tech/discussions)

---

## 🎯 **Roadmap**

- [ ] **Enhanced AI Models** - Support for larger language models
- [ ] **Advanced Analytics** - Legal matter reporting and insights
- [ ] **Mobile App** - React Native mobile application
- [ ] **Multi-tenant Support** - Organization-based access control
- [ ] **Advanced Workflow** - Approval processes and notifications
- [ ] **API Rate Limiting** - Enhanced security features
- [ ] **Kubernetes Deployment** - Container orchestration support

---

**⭐ If you find this project useful, please give it a star!**

[![GitHub stars](https://img.shields.io/github/stars/kevykibbz/Test-Tech?style=social)](https://github.com/kevykibbz/Test-Tech/stargazers)
[![GitHub forks](https://img.shields.io/github/forks/kevykibbz/Test-Tech?style=social)](https://github.com/kevykibbz/Test-Tech/network/members)

---

**🚀 Ready to explore legal tech innovation? [Deploy now!](DEPLOYMENT_GUIDE.md)**

## 📋 **Original Requirements**

This project was initially a .NET 8 WebAPI tech test that has been comprehensively extended with modern features. Below are the original setup requirements for reference:

### **Original Pre-Requisites**

#### **1. Docker Desktop + WSL2**
Follow the instructions in the ```Download``` and ```Install``` sections. [Download + Install link](https://docs.docker.com/desktop/windows/wsl/#download)

#### **2. Visual Studio 2022**
Use the [download link](https://visualstudio.microsoft.com/vs/). Note: only the ```ASP.NET and web development``` workload is required.

#### **3. .NET SDK**
The original project used .NET 8, now upgraded to .NET 9. Download from: https://dotnet.microsoft.com/download/dotnet/9.0

#### **4. dotnet-ef

The WebApi relies on a database, and as such we'll use [Entity Framework migrations](https://docs.microsoft.com/en-us/ef/core/managing-schemas/migrations/?tabs=dotnet-core-cli).

For running migrations please make sure you have the latest ```dotnet-ef``` tool installed.

Open any command line and run ```dotnet tool install --global dotnet-ef```

If you already have ```dotnet-ef``` installed, you can run ```dotnet tool update --global dotnet-ef```

## Running the Solution

1. Open Docker Desktop

2. Find and open the ```LawVuTechTest.sln``` file.

3. Once Visual Studio is open, right-click on the ```TechTest.AppHost``` project and select the ```Set as Startup Project``` option. This .NET Aspire App Host project will orchestrate the WebApi, SQL Server database, Ollama LLM service, and Angular WebApp.

4. You can now run the solution by hitting ```CTRL+F5``` or just ```F5```. The Aspire App Host will automatically:
   - Start a SQL Server container
   - Start an Ollama container with the Mistral model
   - Build and start the Angular WebApp Docker container with live reload
   - Launch the WebApi project
   - Your browser should open the Aspire dashboard where you can monitor all services and access their endpoints

### Development with Live Reload

The Angular WebApp is configured for development with **live reload**:
- Your local `WebApp` folder is mounted into the Docker container
- Any changes you make to TypeScript, HTML, CSS, or other source files will automatically trigger a rebuild
- The browser will automatically refresh to show your changes
- No need to rebuild the Docker container during development

The Aspire dashboard will show:
- **lawvu-techtest-webapi**: The .NET WebAPI backend (accessible at http://localhost:9091)
- **lawvu-techtest-webapp**: The Angular frontend (accessible at http://localhost:4200)
- **sqlserver**: SQL Server database
- **ollama**: LLM service with Mistral model

Try creating a matter via the WebAPI ```POST``` method, and then retrieving it via any ```GET``` method. You can also test the new contract extraction functionality via the ```/ContractExtraction``` endpoint. The Angular app provides a user-friendly interface to interact with these APIs.

![Swagger UI](/Documentation/SwaggerUI.jpeg?raw=true "LawVu")

## Accessing the Database

When running the solution, the .NET Aspire App Host will automatically spin up a SQL Server Docker container. You can view this container by opening Docker Desktop.

You can access the containerized SQL Server just as you would with any local DB server.

The DB credentials are configured in the Aspire App Host project.

See below example screenshots.

### SQL Server Management Studio (SSMS)

![SQL Server Management Studio Login Screenshot](/Documentation/SQLServerManagementStudio.png?raw=true "SSMS")

### Azure Data Studio

![Azure Data Studio Login Screenshot](/Documentation/AzureDataStudio.png?raw=true "Azure Data Studio")

## Database Migrations

Database migrations migrations are automatically applied when the solution is run.

Refer to the "Running the Solution" section above.

## The API

A LegalMatter object can be created by issuing the following request. 

```
POST http://localhost:9091/legalmatter/
{
    "id": "317f0ccc-9001-4df3-96ec-be6d11fbc125",
    "matterName": "theName"
}
```

It can be retrieved by issuing the following request.

```
GET http://localhost:9091/legalmatter/317f0ccc-9001-4df3-96ec-be6d11fbc125
```

### Contract Extraction API

The solution now includes advanced contract extraction capabilities using Large Language Models (LLMs). The following new endpoints are available:

#### Extract Contract Information

```
GET http://localhost:9091/ContractExtraction/
```

This endpoint uses the Ollama LLM service with the Mistral model to extract structured information from legal contracts, including:
- Contract parties
- Key dates and deadlines
- Financial terms
- Key obligations
- Termination clauses  
- Intellectual property clauses
- Confidentiality terms
- Governing law
- Contract summary

The extraction process:
1. Retrieves a sample contract PDF from the database
2. Extracts text from the PDF using iText
3. Sends the text to the Ollama LLM service for analysis
4. Returns structured contract information in JSON format

## The Assignment:

A new database has been added automatically when running the WebApi project. You can interact with the database via the endpoints listed in the Swagger UI.

### Backend API Enhancement

Enhance the WebApi as follows:

1. **Add an endpoint** that will write a DbLawyer object to the database.

2. **Add an endpoint** that will relate a list of existing DBLegalMatters to an existing DbLawyer

3. **Return the company name** of the related lawyer when GETing a matter as follows

```json
{
    "id": "317f0ccc-9001-4df3-96ec-be6d11fbc125",
    "matterName": "theName",
    "lawyerCompany": "lawyerCompanyName"
}
```

4. **The ```LegalLogic.cs``` object test file** in the Tests/ folder is incomplete. Add some unit tests to verify functionality. Also feel free to modify the ```LegalLogic``` implementation should you uncover deficiencies. **For convenience 3 identical test projects are included in the Tests/ folder. Simply choose your preferred testing framework (MS Test, XUnit, NUnit)**

5. **Improve the LLM prompting strategy** for contract extraction. The current implementation provides a basic structure - enhance the prompts to extract the nuanced legal information from the contract provided.

### Frontend API Integration

The Angular frontend expects additional API endpoints to provide a complete legal matter management experience. Based on the home component requirements, implement the following endpoints to support the frontend functionality:

#### Core CRUD Operations
- **PUT** `/legalmatter/{id}` - Update legal matter
- **DELETE** `/legalmatter/{id}` - Delete legal matter  
- **GET** `/legalmatter/Total` - Get total count of legal matters

#### Reference Data Endpoints
- **GET** `/api/v1/Currencies` - Get available currencies
- **GET** `/api/v1/LegalMatterCategories` - Get legal matter categories  
- **GET** `/api/v1/People` - Get all people
- **GET** `/api/v1/People/{id}` - Get specific person by ID

#### System Endpoints
- **GET** `/api/v1/App/EventTypes` - Get available event types
- **GET** `/api/v1/App/EventTypeGroups` - Get event type groupings
- **GET** `/api/v1/App/Log` - Get application logs with filtering parameters
- **GET** `/api/v1/App/Parameters` - Get application parameters/configuration

#### Enhanced Legal Matter Model
Update the legal matter model to include all fields expected by the frontend:
- `id` (UUID v4)
- `name` (string, required)
- `category_id` (UUID v4, nullable)
- `manager_id` (UUID v4, nullable)
- `due_date` (Date, nullable)
- `estimated_cost` (decimal, nullable)
- `currency` (string, nullable)
- `created_at` (Date, auto-generated)
- `updated_at` (Date, auto-updated)

## Architecture

The solution now uses .NET Aspire for orchestration, which provides:

- **Service Discovery**: Automatic service registration and discovery
- **Configuration Management**: Centralized configuration across services
- **Observability**: Built-in logging, metrics, and tracing
- **Development Experience**: Simplified local development with the Aspire dashboard

### Key Components:

- **TechTest.AppHost**: .NET Aspire App Host that orchestrates all services
- **WebApi**: Main API project with endpoints for legal matters and contract extraction
- **WebApp**: Angular frontend application providing a modern UI for interacting with the API
- **AppLogic**: Business logic layer including LLM integration via Ollama
- **DataAccess**: Entity Framework data access layer
- **ServiceModel**: Shared models and contracts

### LLM Integration:

The solution integrates with Ollama for local LLM capabilities:
- **Ollama Service**: Containerized LLM service with Mistral model
- **ContractExtractionService**: Service that orchestrates PDF text extraction and LLM analysis
- **OllamaClient**: HTTP client for communicating with the Ollama API

## Note

Feel free to refactor any logic or design that you think could be improved.
