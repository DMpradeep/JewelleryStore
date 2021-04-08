# Links
Api is deployed as "Azure App Service"  
**Swagger Url** - https://jewellerystoreapi.azurewebsites.net/swagger/index.html  

# Introduction 
Jeweellery store Api.  
Part of technical assigment - https://github.com/AUNewGen/jewelry-store-challenge-  

Rest api on Asp.Net core.  
.Net version - 5.0  

# Getting Started
Download solution and run the web project.  
Default site url - https://localhost:5009 . Configurable in LaunchSettings.json file  

# Build and Test
Build: Make user to have .net 5 sdk installed.  

Unit test:  
MS test is used for unit test project.  
https://github.com/moq/moq4 is used for mocking.  

ORM:  
**Dapper** is used to connect between c# and sql server  

DB  
Local db : **SQLite**  

Validator:  
FluentValidator: To validate messages / models sent to rest api.  
https://github.com/FluentValidation/FluentValidation  

# Deployement
1. CI : Configured corresponding Azure Devops pipeline.   
Link - https://dmpradeep.visualstudio.com/JewelleryStore/_build?definitionId=4    
2. CD : Configured corresponding Azure Devops release to deploy the api to "Azure App Service".   
Link - https://dmpradeep.visualstudio.com/JewelleryStore/_release?_a=releases&view=mine&definitionId=1    

# @Todo  
1. Add integration tests (Will add it by April 8th EOD)
2. Postman test cases (I have no prior experience. Will learn and complete it by April 10th 2021)
