FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY StoneActionServer.WebApi/StoneActionServer.WebApi.csproj ./StoneActionServer.WebApi/
RUN dotnet restore "StoneActionServer.WebApi/StoneActionServer.WebApi.csproj"
COPY . .
WORKDIR "/src/StoneActionServer.WebApi"
RUN dotnet build "StoneActionServer.WebApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "StoneActionServer.WebApi.csproj" -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "StoneActionServer.WebApi.dll"]
