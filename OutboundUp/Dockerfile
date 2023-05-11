#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
RUN apt-get update && apt-get install -y curl
WORKDIR /tools
## download ookla CLI
RUN curl https://install.speedtest.net/app/cli/ookla-speedtest-1.2.0-linux-x86_64.tgz | tar -xzv 
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
RUN apt-get update && apt-get install -y curl
RUN curl -fsSL https://deb.nodesource.com/setup_16.x | bash - && apt-get install -y nodejs
WORKDIR /src
COPY ["OutboundUp/OutboundUp.csproj", "OutboundUp/"]
RUN dotnet restore "OutboundUp/OutboundUp.csproj"
COPY . .
WORKDIR "/src/OutboundUp"
RUN dotnet build "OutboundUp.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "OutboundUp.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "OutboundUp.dll"]