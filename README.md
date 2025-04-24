# Minitab Customer API – Take-Home Assignment

This project is a simple RESTful Web API built with .NET Core that accepts customer information and integrates with a fake CRM repository. It performs address validation using the Geoapify Geocoding API before persisting the data to a JSON file. This JSON implementation could easily be replaced with persistence to a database or cloud storage.

---

## Features

- `POST /api/customer` endpoint to receive customer info
- Validates US addresses using the Geoapify API
- Upserts customers to a local JSON file (simulating CRM persistence)
- Automatically strips out invalid addresses before saving
- Includes unit and integration tests using xUnit and Moq

---

## Tech Stack

- .NET 8 Web API
- `System.Text.Json` for serialization
- Geoapify Geocoding API (https://www.geoapify.com/)
- xUnit + Moq for testing
- Swagger (Swashbuckle) for API docs

---

## Running the Project

### 1. Clone the repository
```bash
git clone https://github.com/zachauker/Auker_Minitab_Assignment
cd Auker_Minitab_Assignment
```

### 2. Add Geoapify API Key

Update appsettings.json:
```
{
  "Geoapify": {
    "ApiKey": "INSERT_YOUR_API_KEY_HERE"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft": "Warning"
    }
  }
}
```

You can get a free key at https://www.geoapify.com/

### 3. Run the API

```dotnet run --project Auker_Minitab_Interview_Assignment```

- The API will launch at: https://localhost:5088
- Swagger docs:
    https://localhost:5088/swagger

## Testing the API 

Option 1: Swagger UI

- Open https://localhost:5001/swagger
- Send a sample request to POST /api/customer

Sample Payload:
```
{
  "CustomerName": "Fred Flintstone",
  "CustomerEmail": "fred.flintstone@bedrock-llc.com",
  "Address": {
    "Line1": "10 South LaSalle St.",
    "City": "Chicago",
    "State": "IL",
    "PostalCode": "60603",
    "Country": "US"
  }
}
```
Option 2: Postman

- Create a new POST request to:

    ```https://localhost:5001/api/customer```

- Add Content-Type: application/json header

- Paste the JSON body above

- Disable SSL verification in Postman settings (optional)

## Running Unit Tests

```dotnet test```

This runs the Minitab.CustomerApi.Tests project and includes:

- CustomerController behavior tests
- Geoapify address validation logic (mocked)
- Scenarios with valid and invalid addresses

## Assumptions
 
- Only US addresses are supported. 
- Address validation is based on Geoapify’s confidence score being above 0.8. 
- The "CRM" is simulated by writing/updating customers.json in the root directory. 
- If address validation fails, customer is saved without the address.