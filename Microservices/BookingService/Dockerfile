FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY BookingService.Core/BookingService.Core.csproj BookingService.Core/
COPY BookingService.Application/BookingService.Application.csproj BookingService.Application/
COPY BookingService.Infrastructure/BookingService.Infrastructure.csproj BookingService.Infrastructure/
COPY BookingService.API/BookingService.API.csproj BookingService.API/

RUN dotnet restore BookingService.API/BookingService.API.csproj

COPY . .

RUN dotnet publish BookingService.API/BookingService.API.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "BookingService.API.dll"]
