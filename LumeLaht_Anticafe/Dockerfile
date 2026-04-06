FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

# Build React frontend
FROM node:22-alpine AS frontend-build
WORKDIR /frontend
COPY frontend/package*.json ./
RUN npm ci
COPY frontend/ .
RUN npm run build

# Build .NET backend
FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY LumeLaht_RoomApi.Core/LumeLaht_RoomApi.Core.csproj LumeLaht_RoomApi.Core/
COPY LumeLaht_RoomApi.Application/LumeLaht_RoomApi.Application.csproj LumeLaht_RoomApi.Application/
COPY LumeLaht_RoomApi.Infrastructure/LumeLaht_RoomApi.Infrastructure.csproj LumeLaht_RoomApi.Infrastructure/
COPY LumeLaht_Anticafe/LumeLaht_Anticafe.csproj LumeLaht_Anticafe/

RUN dotnet restore LumeLaht_Anticafe/LumeLaht_Anticafe.csproj

COPY LumeLaht_RoomApi.Core/ LumeLaht_RoomApi.Core/
COPY LumeLaht_RoomApi.Application/ LumeLaht_RoomApi.Application/
COPY LumeLaht_RoomApi.Infrastructure/ LumeLaht_RoomApi.Infrastructure/
COPY LumeLaht_Anticafe/ LumeLaht_Anticafe/

RUN dotnet publish LumeLaht_Anticafe/LumeLaht_Anticafe.csproj -c Release -o /app/publish --no-restore

# Final image: .NET + React static files
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
COPY --from=frontend-build /frontend/dist ./wwwroot
ENTRYPOINT ["dotnet", "LumeLaht_Anticafe.dll"]
