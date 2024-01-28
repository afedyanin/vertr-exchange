FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
#EXPOSE 8080 8081 
EXPOSE 8080

ENV ASPNETCORE_HTTP_PORTS=80;8080
# ENV ASPNETCORE_HTTPS_PORTS=443;8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build

COPY /src ./src
COPY /Directory.Build.props .
COPY /LICENSE .
COPY /README.md .

FROM build AS publish

WORKDIR  /src/Vertr.Exchange.Server
RUN dotnet publish "Vertr.Exchange.Server.csproj" -c release -o /app/publish /p:UseAppHost=false
# RUN dotnet dev-certs https --clean
# RUN dotnet dev-certs https --trust

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vertr.Exchange.Server.dll"]
