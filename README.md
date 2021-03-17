# Checkout.Challenge

### Assumptions
 - The merchant is authenticated
 
### Improvements 
 - Treat different Exception types (eg: PaymentService.cs)
 - Reorganising the DTOs and Models as they are shared across multiple projects
 - Retry mechanism for failed requests (messaging)
 - The implemented data storage is minimalistic 

### Run locally
 - Settings: update BankApiUrl value in Checkout.Challenge.Bank.Api appSettings
 - Both Checkout.Challenge.Api and Checkout.Challenge.Bank.Api must run at the same time

### Testing
 - Postman collection Checkout.Challenge.postman_collection.json in the root. I could not find a way to export the environment variables from postman, so the url must be changed manually.
 - Swagger also available for both APIs
 
 ### Logging
 - Log files in the Log folder

 ### Data Storage
 - Data is saved to a JSON file in the Data folder
