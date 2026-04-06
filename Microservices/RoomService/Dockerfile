FROM mcr.microsoft.com/dotnet/aspnet:10.0-preview AS base
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:10.0-preview AS build
WORKDIR /src

COPY RoomService.Core/RoomService.Core.csproj RoomService.Core/
COPY RoomService.Application/RoomService.Application.csproj RoomService.Application/
COPY RoomService.Infrastructure/RoomService.Infrastructure.csproj RoomService.Infrastructure/
COPY RoomService.API/RoomService.API.csproj RoomService.API/

RUN dotnet restore RoomService.API/RoomService.API.csproj

COPY . .

WORKDIR /src/RoomService.API
RUN dotnet publish -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "RoomService.API.dll"]
