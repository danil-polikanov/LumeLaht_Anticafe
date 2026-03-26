FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY LumeLaht_RoomApi.Core/LumeLaht_RoomApi.Core.csproj LumeLaht_RoomApi.Core/
COPY LumeLaht_RoomApi.Application/LumeLaht_RoomApi.Application.csproj LumeLaht_RoomApi.Application/
COPY LumeLaht_RoomApi.Infrastructure/LumeLaht_RoomApi.Infrastructure.csproj LumeLaht_RoomApi.Infrastructure/
COPY LumeLaht_RoomApi/LumeLaht_RoomApi.csproj LumeLaht_RoomApi/

RUN dotnet restore LumeLaht_RoomApi/LumeLaht_RoomApi.csproj

COPY LumeLaht_RoomApi.Core/ LumeLaht_RoomApi.Core/
COPY LumeLaht_RoomApi.Application/ LumeLaht_RoomApi.Application/
COPY LumeLaht_RoomApi.Infrastructure/ LumeLaht_RoomApi.Infrastructure/
COPY LumeLaht_RoomApi/ LumeLaht_RoomApi/

RUN dotnet publish LumeLaht_RoomApi/LumeLaht_RoomApi.csproj -c Release -o /app/publish --no-restore

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "LumeLaht_RoomApi.dll"]
