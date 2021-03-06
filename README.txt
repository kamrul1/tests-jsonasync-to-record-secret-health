
using the same project from youtube:

Creating a .NET 5 Microservice
https://www.youtube.com/watch?v=MIJJCR3ndQQ&t=217s

This demonstrate user secrets, which saves folder: %APPDATA%\microsoft\UserSecrets
C:\Users\kamrul\AppData\Roaming\Microsoft\UserSecrets

-- create secrets using Developer PowerShell tool

PS W:\GitLabLocalRepo\DotNet5ManageSecrets\WebApplication1> dotnet user-secrets init
Set UserSecretsId to 'c5ccdd3c-f83c-4a84-99a1-df07c1cf1442' for MSBuild project 'W:\GitLabLocalRepo\DotNet5ManageSecrets\WebApplication1\WebApplication1.csproj'.

--save value to secrets

PS W:\GitLabLocalRepo\DotNet5ManageSecrets\WebApplication1> dotnet user-secrets set ServiceSettings:ApiKey a4233d06d7124a9091d8ffc605e9c7fa
Successfully saved ServiceSettings:ApiKey = a4233d06d7124a9091d8ffc605e9c7fa to the secret store.

--this secret is readable from Configuration.GetSection or IOption see example in WeatherClient.cs

--Polly used for error handling and circut breaker

--healthchecks using ping




