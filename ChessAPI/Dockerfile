FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

COPY . .

RUN dotnet restore ChessAPI.sln
RUN dotnet publish ChessAPI.sln -c Release -o /app/publish

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5001

ENTRYPOINT ["dotnet", "ChessAPI.dll"]
