FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Seminars/Seminars.csproj", "Seminars/"]
RUN dotnet restore "Seminars/Seminars.csproj"
COPY . .
WORKDIR "/src/Seminars"
RUN dotnet build "Seminars.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Seminars.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Seminars.dll"]
