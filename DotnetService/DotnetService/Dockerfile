# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
WORKDIR /app

# Install Python and WeasyPrint dependencies
RUN apt-get update && apt-get install -y python3 python3-pip weasyprint \
    && apt-get install -y build-essential libpango1.0-dev libcairo2-dev

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["DotnetService/DotnetService.csproj", "DotnetService/"]
COPY ["../DotnetPython/DotnetPython.csproj", "../DotnetPython/"]
COPY ["../DotnetServiceTasks/DotnetServiceTasks.csproj", "../DotnetServiceTasks/"]
RUN dotnet restore "./DotnetService/DotnetService.csproj"
COPY . .
WORKDIR "/src/DotnetService"
RUN dotnet build "./DotnetService.csproj" -c Release -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
RUN dotnet publish "./DotnetService.csproj" -c Release -o /app/publish

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

# Copy the python script if necessary
# Example: COPY ["script.py", "/app/script.py"]

ENTRYPOINT ["dotnet", "DotnetService.dll"]