# 🧪 API Testing Guide - LawVu Tech Test

## 📋 Prerequisites
- ✅ Application running on `http://localhost:9091`
- ✅ Swagger UI accessible at `http://localhost:9091/swagger`
- ✅ Aspire Dashboard at `http://localhost:15026`

---

## 🎯 Testing Plan Overview

### Phase 1: Basic CRUD Operations
1. Legal Matter Management
2. Lawyer Management
3. Currency Management
4. Event Types & Groups Management
5. People Management
6. Legal Matter Categories Management
7. Application Logging
8. Data Validation

### Phase 2: Advanced Features  
1. Lawyer-Matter Relationships
2. Contract Extraction with LLM
3. Error Handling

### Phase 3: Integration Testing
1. End-to-end workflows
2. Performance validation

---

## 🔍 Phase 1: Basic CRUD Operations

### 1.1 Legal Matter API Testing

#### ✅ **Test 1: Get All Legal Matters (Initially Empty)**
- **Method:** `GET`
- **Endpoint:** `/LegalMatter`
- **Expected Response:** `200 OK` with empty array `[]`

#### ✅ **Test 2: Create a Legal Matter**
- **Method:** `POST` 
- **Endpoint:** `/LegalMatter`
- **Request Body:** (You can omit `id`, `createdAt`, `lastModified` - they'll be auto-generated)
```json
{
  "matterName": "Contract Review - ABC Corp",
  "contractType": "Service Agreement",
  "parties": [
    "ABC Corporation",
    "XYZ Services LLC"
  ],
  "effectiveDate": "2024-01-15T00:00:00.000Z",
  "expirationDate": "2025-12-31T23:59:59.000Z",
  "governingLaw": "California State Law",
  "contractValue": 75000,
  "status": "Active",
  "description": "Review and analysis of service agreement for ABC Corporation",
  "lawyerId": null
}
```
- **Expected Response:** `200 OK` with created matter object
- **Note:** Save the `id` from response for next tests

#### ✅ **Test 3: Get Specific Legal Matter**
- **Method:** `GET`
- **Endpoint:** `/LegalMatter/{id}` (use ID from Test 2)
- **Expected Response:** `200 OK` with the matter details

#### ✅ **Test 4: Get Sample Legal Matter**
- **Method:** `GET`
- **Endpoint:** `/LegalMatter/sample`
- **Expected Response:** `200 OK` with sample matter data

#### ✅ **Test 5: Create Multiple Legal Matters**
Create additional matters for relationship testing:

**Matter 2:**
```json
{
  "matterName": "Employment Agreement Review",
  "contractType": "Employment Contract",
  "parties": [
    "TechStart Inc.",
    "John Doe (Employee)"
  ],
  "effectiveDate": "2024-03-01T00:00:00.000Z",
  "expirationDate": "2026-02-28T23:59:59.000Z",
  "governingLaw": "New York State Law",
  "contractValue": 120000,
  "status": "Pending",
  "description": "Review employment terms and conditions",
  "lawyerId": null
}
```

**Matter 3:**
```json
{
  "matterName": "NDA Analysis",
  "contractType": "Non-Disclosure Agreement",
  "parties": [
    "InnovateCorp",
    "Partner Solutions Ltd"
  ],
  "effectiveDate": "2024-02-01T00:00:00.000Z",
  "expirationDate": "2026-01-31T23:59:59.000Z",
  "governingLaw": "Delaware Corporate Law",
  "contractValue": 0,
  "status": "Active",
  "description": "Analysis of confidentiality terms",
  "lawyerId": null
}
```

---

### 1.2 Lawyer API Testing

#### ✅ **Test 6: Get All Lawyers**
- **Method:** `GET`
- **Endpoint:** `/Lawyer`
- **Expected Response:** `200 OK` with array of lawyers

#### ✅ **Test 7: Create a Lawyer**
- **Method:** `POST`
- **Endpoint:** `/Lawyer`  
- **Request Body:**
```json
{
  "firstName": "John",
  "lastName": "Smith",
  "companyName": "Smith & Associates"
}
```
- **Expected Response:** `200 OK` with created lawyer object
- **Note:** Save the `id` from response

#### ✅ **Test 8: Get Specific Lawyer**
- **Method:** `GET`
- **Endpoint:** `/Lawyer/{id}` (use ID from Test 7)
- **Expected Response:** `200 OK` with lawyer details

#### ✅ **Test 9: Create Additional Lawyers**

**Lawyer 2:**
```json
{
  "firstName": "Sarah",
  "lastName": "Johnson",
  "companyName": "Johnson Legal Group"
}
```

**Lawyer 3:**
```json
{
  "firstName": "Michael",
  "lastName": "Davis", 
  "companyName": "Davis Law Firm"
}
```

---

### 1.3 Currency API Testing

#### ✅ **Test 10: Get All Currencies**
- **Method:** `GET`
- **Endpoint:** `/Currencies`
- **Expected Response:** `200 OK` with array of supported currencies
- **Sample Response:**
```json
[
  {
    "id": "USD",
    "symbol": "$",
    "name": "US Dollar",
    "decimalDigits": 2
  },
  {
    "id": "EUR",
    "symbol": "€",
    "name": "Euro",
    "decimalDigits": 2
  }
]
```

#### ✅ **Test 11: Get Specific Currency**
- **Method:** `GET`
- **Endpoint:** `/Currencies/{id}` (e.g., `/Currencies/USD`)
- **Expected Response:** `200 OK` with currency details

---

### 1.4 Event Types & Groups API Testing

#### ✅ **Test 12: Get All Event Type Groups**
- **Method:** `GET`
- **Endpoint:** `/EventTypeGroups`
- **Expected Response:** `200 OK` with array of event type groups
- **Sample Response:**
```json
[
  {
    "id": "matter",
    "name": "Matter Events",
    "description": "Events related to legal matters"
  },
  {
    "id": "billing",
    "name": "Billing Events", 
    "description": "Events related to billing and finances"
  }
]
```

#### ✅ **Test 13: Get Specific Event Type Group**
- **Method:** `GET`
- **Endpoint:** `/EventTypeGroups/{id}` (e.g., `/EventTypeGroups/matter`)
- **Expected Response:** `200 OK` with group details

#### ✅ **Test 14: Get All Event Types**
- **Method:** `GET`
- **Endpoint:** `/EventTypes`
- **Expected Response:** `200 OK` with array of event types
- **Sample Response:**
```json
[
  {
    "id": "matter.created",
    "name": "Matter Created",
    "description": "A new legal matter was created",
    "groupId": "matter"
  },
  {
    "id": "billing.invoice",
    "name": "Invoice Generated",
    "description": "An invoice was generated",
    "groupId": "billing"
  }
]
```

#### ✅ **Test 15: Get Specific Event Type**
- **Method:** `GET`
- **Endpoint:** `/EventTypes/{id}` (e.g., `/EventTypes/matter.created`)
- **Expected Response:** `200 OK` with event type details

---

### 1.5 People API Testing

#### ✅ **Test 16: Get All People**
- **Method:** `GET`
- **Endpoint:** `/People`
- **Expected Response:** `200 OK` with array of people
- **Sample Response:**
```json
[
  {
    "id": "1",
    "firstName": "John",
    "lastName": "Doe",
    "fullName": "John Doe",
    "initials": "JD",
    "hasPicture": false,
    "pictureUrl": null
  }
]
```

#### ✅ **Test 17: Get Specific Person**
- **Method:** `GET`
- **Endpoint:** `/People/{id}` (e.g., `/People/1`)
- **Expected Response:** `200 OK` with person details

---

### 1.6 Legal Matter Categories API Testing

#### ✅ **Test 18: Get All Legal Matter Categories**
- **Method:** `GET`
- **Endpoint:** `/LegalMatterCategories`
- **Expected Response:** `200 OK` with array of categories
- **Sample Response:**
```json
[
  {
    "id": "corporate",
    "name": "Corporate Law"
  },
  {
    "id": "litigation",
    "name": "Litigation"
  },
  {
    "id": "intellectual-property",
    "name": "Intellectual Property"
  }
]
```

#### ✅ **Test 19: Get Specific Legal Matter Category**
- **Method:** `GET`
- **Endpoint:** `/LegalMatterCategories/{id}` (e.g., `/LegalMatterCategories/corporate`)
- **Expected Response:** `200 OK` with category details

---

### 1.7 Application Logging API Testing

#### ✅ **Test 20: Get Application Logs**
- **Method:** `GET`
- **Endpoint:** `/App/Log`
- **Query Parameters:** 
  - `entity_id` (optional): Filter by entity ID
  - `type_id` (optional): Filter by type ID  
  - `page` (optional): Page number for pagination
  - `page_size` (optional): Number of items per page
- **Expected Response:** `200 OK` with array of log entries
- **Sample Response:**
```json
[
  {
    "id": 1,
    "details": "Matter created successfully",
    "entityId": "matter-123",
    "typeId": "matter.created",
    "createdAt": "2024-08-30T10:30:00Z"
  }
]
```

#### ✅ **Test 21: Add Log Entry**
- **Method:** `POST`
- **Endpoint:** `/App/Log`
- **Request Body:**
```json
{
  "details": "User performed search operation",
  "entityId": "user-456",
  "typeId": "user.search"
}
```
- **Expected Response:** `200 OK` with success confirmation

---

## 🔗 Phase 2: Advanced Features

### 2.1 Lawyer-Matter Relationships

#### ✅ **Test 22: Assign Matters to Lawyer**
- **Method:** `POST`
- **Endpoint:** `/Lawyer/{lawyerId}/assign-matters`
- **Request Body:** Array of matter IDs
```json
[
  "matter-id-1",
  "matter-id-2"
]
```
- **Expected Response:** `200 OK` with success message

#### ✅ **Test 23: Get Lawyer's Assigned Matters**
- **Method:** `GET`
- **Endpoint:** `/Lawyer/{lawyerId}/matters`
- **Expected Response:** `200 OK` with array of assigned matters

---

### 2.2 Contract Extraction API Testing

#### ✅ **Test 24: Extract from Contract Text**
- **Method:** `POST`
- **Endpoint:** `/ContractExtraction/extract`
- **Request Body:**
```json
{
  "contractText": "This Service Agreement is entered into on January 15, 2024, between ABC Corporation, a Delaware corporation ('Client') and XYZ Services LLC, a California LLC ('Provider'). The contract value is $75,000 annually and expires on December 31, 2025. The governing law shall be the State of California."
}
```
- **Expected Response:** `200 OK` with extracted contract information
- **Check for:** Parties, dates, contract value, governing law

#### ✅ **Test 25: Extract from Simple Text**
- **Method:** `POST`
- **Endpoint:** `/ContractExtraction/extract-from-text`
- **Request Body:**
```json
{
  "text": "CONFIDENTIALITY AGREEMENT between TechStart Inc. and DevCorp Ltd. Effective Date: March 1, 2024. This agreement shall remain in effect for 2 years. Confidential information includes all technical specifications, business plans, and customer lists."
}
```
- **Expected Response:** `200 OK` with extracted information

#### ✅ **Test 26: Extract from File Upload**
- **Method:** `POST`
- **Endpoint:** `/ContractExtraction/extract-from-file`
- **Request:** Upload a sample contract file (PDF/TXT)
- **Expected Response:** `200 OK` with extracted contract data

---

## 🧪 Phase 3: Integration & Error Testing

### 3.1 Error Handling Tests

#### ✅ **Test 27: Invalid Legal Matter Creation**
```json
{
  "matterName": "",
  "contractType": null
}
```
- **Expected Response:** `400 Bad Request` with validation errors

#### ✅ **Test 28: Get Non-existent Matter**
- **Method:** `GET`
- **Endpoint:** `/LegalMatter/00000000-0000-0000-0000-000000000000`
- **Expected Response:** `404 Not Found`

#### ✅ **Test 29: Contract Extraction with Empty Text**
```json
{
  "contractText": ""
}
```
- **Expected Response:** `400 Bad Request` with appropriate error

#### ✅ **Test 30: Get Non-existent Currency**
- **Method:** `GET`
- **Endpoint:** `/Currencies/INVALID`
- **Expected Response:** `404 Not Found`

#### ✅ **Test 31: Get Non-existent Person**
- **Method:** `GET`
- **Endpoint:** `/People/999999`
- **Expected Response:** `404 Not Found`

### 3.2 End-to-End Workflow Tests

#### ✅ **Test 32: Complete Workflow**
1. Create a lawyer
2. Create multiple legal matters  
3. Assign matters to the lawyer
4. Extract contract information
5. Verify all relationships and data

---

## 📊 Success Criteria Checklist

### ✅ **Core API Functionality**
- [ ] Legal Matter CRUD operations working
- [ ] Lawyer CRUD operations working
- [ ] Currency endpoints working
- [ ] Event Types & Groups endpoints working
- [ ] People endpoints working
- [ ] Legal Matter Categories endpoints working
- [ ] Application logging endpoints working
- [ ] Sample data endpoints working
- [ ] Proper HTTP status codes returned

### ✅ **Advanced Features**
- [ ] Lawyer-matter assignment working
- [ ] Contract extraction with LLM working
- [ ] Ollama integration responding correctly
- [ ] File upload processing working
- [ ] Log entry creation and retrieval working

### ✅ **Data Validation**
- [ ] Required fields properly validated
- [ ] Invalid data rejected with proper errors
- [ ] Database relationships maintained
- [ ] Consistent data formats
- [ ] Seed data properly loaded for reference entities

### ✅ **Integration**
- [ ] All services communicating properly
- [ ] Database migrations applied successfully
- [ ] Swagger documentation complete and accurate
- [ ] Performance acceptable for demo purposes
- [ ] All new endpoints properly documented in Swagger

---

## 🚨 Troubleshooting

### If any endpoints are missing:
1. Check if controllers are properly registered
2. Verify dependency injection is configured
3. Restart the application
4. Check application logs for errors
5. Verify project references are correct

### If reference data endpoints return empty:
1. Check if database migrations were applied
2. Verify seed data was loaded properly
3. Check connection string configuration
4. Verify database permissions

### If Contract extraction fails:
1. Verify Ollama service is running (port 11434)
2. Check if llama3.2:1b model is loaded
3. Verify network connectivity to Ollama
4. Check application logs for LLM errors

### If Database errors occur:
1. Verify SQL Server is running
2. Check if migrations were applied
3. Verify connection string configuration
4. Check database permissions

---

## 📝 Testing Notes

**Record your results here:**

**Legal Matter Tests:**
- Test 1: ☐ Pass ☐ Fail - Notes: _______________
- Test 2: ☐ Pass ☐ Fail - Notes: _______________
- Test 3: ☐ Pass ☐ Fail - Notes: _______________
- Test 4: ☐ Pass ☐ Fail - Notes: _______________
- Test 5: ☐ Pass ☐ Fail - Notes: _______________

**Lawyer Tests:**
- Test 6: ☐ Pass ☐ Fail - Notes: _______________
- Test 7: ☐ Pass ☐ Fail - Notes: _______________
- Test 8: ☐ Pass ☐ Fail - Notes: _______________
- Test 9: ☐ Pass ☐ Fail - Notes: _______________

**Currency Tests:**
- Test 10: ☐ Pass ☐ Fail - Notes: _______________
- Test 11: ☐ Pass ☐ Fail - Notes: _______________

**Event Types & Groups Tests:**
- Test 12: ☐ Pass ☐ Fail - Notes: _______________
- Test 13: ☐ Pass ☐ Fail - Notes: _______________
- Test 14: ☐ Pass ☐ Fail - Notes: _______________
- Test 15: ☐ Pass ☐ Fail - Notes: _______________

**People Tests:**
- Test 16: ☐ Pass ☐ Fail - Notes: _______________
- Test 17: ☐ Pass ☐ Fail - Notes: _______________

**Legal Matter Categories Tests:**
- Test 18: ☐ Pass ☐ Fail - Notes: _______________
- Test 19: ☐ Pass ☐ Fail - Notes: _______________

**Application Logging Tests:**
- Test 20: ☐ Pass ☐ Fail - Notes: _______________
- Test 21: ☐ Pass ☐ Fail - Notes: _______________

**Lawyer-Matter Relationship Tests:**
- Test 22: ☐ Pass ☐ Fail - Notes: _______________
- Test 23: ☐ Pass ☐ Fail - Notes: _______________

**Contract Extraction Tests:**
- Test 24: ☐ Pass ☐ Fail - Notes: _______________
- Test 25: ☐ Pass ☐ Fail - Notes: _______________
- Test 26: ☐ Pass ☐ Fail - Notes: _______________

**Error Handling Tests:**
- Test 27: ☐ Pass ☐ Fail - Notes: _______________
- Test 28: ☐ Pass ☐ Fail - Notes: _______________
- Test 29: ☐ Pass ☐ Fail - Notes: _______________
- Test 30: ☐ Pass ☐ Fail - Notes: _______________
- Test 31: ☐ Pass ☐ Fail - Notes: _______________

**Integration Tests:**
- Test 32: ☐ Pass ☐ Fail - Notes: _______________

---

**🎯 Start with Test 1 and work through each test systematically!**
