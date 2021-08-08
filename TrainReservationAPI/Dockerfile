FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["./TrainReservationAPI.csproj", "src/"]
RUN dotnet restore "src/TrainReservationAPI.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "TrainReservationAPI.csproj" -c Release -o /app/build
FROM build AS publish
RUN dotnet publish "TrainReservationAPI.csproj" -c Release -o /app/publish
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "TrainReservationAPI.dll"]