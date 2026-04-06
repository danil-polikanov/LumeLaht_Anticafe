FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY UserService.Core/UserService.Core.csproj UserService.Core/
COPY UserService.Application/UserService.Application.csproj UserService.Application/
COPY UserService.Infrastructure/UserService.Infrastructure.csproj UserService.Infrastructure/
COPY UserService.API/UserService.API.csproj UserService.API/

RUN dotnet restore UserService.API/UserService.API.csproj

COPY . .

WORKDIR /src/UserService.API
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "UserService.API.dll"]
