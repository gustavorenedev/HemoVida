# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["HemoVida.Notifiers/HemoVida.Notifiers.csproj", "HemoVida.Notifiers/"]
RUN dotnet restore "./HemoVida.Notifiers/HemoVida.Notifiers.csproj"
COPY . .
WORKDIR "/src/HemoVida.Notifiers"
RUN dotnet build "./HemoVida.Notifiers.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./HemoVida.Notifiers.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HemoVida.Notifiers.dll"]