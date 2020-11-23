# SQSDemoBackgroundService_Part3

Support code for the following article

https://medium.com/@martinmasse/sqs-background-service-with-net-5-part-3-3dfff137947d

# Prerequisites

You will need this to run the code.

- AWS Account
- An IAM user with SQS access rights
- .NET 5 (or 3.1, you will have to change the target framework)

# How to run the demo

- Clone this repository
- Change the value of AWS:Profile (or make sure you have a default profile setup in .aws) and set your AWS:Region in appsettings.json (default is us-east-1) 
- Optionnaly set JobWorkerService:QueueName to the name of your queue in appsettings.json
- cd SQSDemoBackgroundService_Part3/Demo.API
- dotnet watch run
- Go to https://localhost:5001/swagger/index.html
