#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0-alpine AS base
USER app
WORKDIR /app
EXPOSE 80
ENV DOTNET_URLS=http://+:80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "."]
COPY ["Directory.Build.targets", "."]
COPY ["src/Vertr.Exchange.Server/Vertr.Exchange.Server.csproj", "src/Vertr.Exchange.Server/"]
COPY ["src/Vertr.Exchange.Adapters.SignalR/Vertr.Exchange.Adapters.SignalR.csproj", "src/Vertr.Exchange.Adapters.SignalR/"]
COPY ["src/Vertr.Exchange.Application/Vertr.Exchange.Application.csproj", "src/Vertr.Exchange.Application/"]
COPY ["src/Vertr.Exchange.Domain.Common/Vertr.Exchange.Domain.Common.csproj", "src/Vertr.Exchange.Domain.Common/"]
COPY ["src/Vertr.Exchange.Contracts/Vertr.Exchange.Contracts.csproj", "src/Vertr.Exchange.Contracts/"]
COPY ["src/Vertr.Exchange.Domain.MatchingEngine/Vertr.Exchange.Domain.MatchingEngine.csproj", "src/Vertr.Exchange.Domain.MatchingEngine/"]
COPY ["src/Vertr.Exchange.Domain.RiskEngine/Vertr.Exchange.Domain.RiskEngine.csproj", "src/Vertr.Exchange.Domain.RiskEngine/"]
COPY ["src/Vertr.Exchange.Domain.Accounts/Vertr.Exchange.Domain.Accounts.csproj", "src/Vertr.Exchange.Domain.Accounts/"]
RUN dotnet restore "./src/Vertr.Exchange.Server/./Vertr.Exchange.Server.csproj"
COPY . .
WORKDIR "/src/src/Vertr.Exchange.Server"
RUN dotnet build "./Vertr.Exchange.Server.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Vertr.Exchange.Server.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Vertr.Exchange.Server.dll"]
