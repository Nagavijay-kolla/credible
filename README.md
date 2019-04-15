# README #

### Build Steps ###
** Api **

```
	dotnet restore CBH.ChatApi\CBH.ChatApi.sln -s CBH.ChatApi\Nuget.config
	dotnet build CBH.ChatApi\CBH.Chat.Web.Services\CBH.Chat.Web.Services.csproj -c Release
	dotnet test CBH.ChatApi\CBH.Chat.Business.Tests
	dotnet publish -c Release CBH.ChatApi\CBH.Chat.Web.Services\CBH.Chat.Web.Services.csproj
```

### Who do I talk to? ###

* Rohit Vipin Mathews