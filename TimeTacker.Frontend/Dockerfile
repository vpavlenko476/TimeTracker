#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["TimeTacker.Frontend/TimeTacker.Frontend.csproj", "TimeTacker.Frontend/"]
COPY ["TimeTracker.Client/TimeTracker.Client.csproj", "TimeTracker.Client/"]
COPY ["Services.Contracts/Services.Contracts.csproj", "Services.Contracts/"]
RUN dotnet restore "TimeTacker.Frontend/TimeTacker.Frontend.csproj"
COPY . .
WORKDIR "/src/TimeTacker.Frontend"
RUN dotnet build "TimeTacker.Frontend.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "TimeTacker.Frontend.csproj" -c Release -o /app/publish

FROM nginx:alpine AS final
WORKDIR /usr/share/nginx/html
COPY --from=publish /app/publish/wwwroot .
COPY nginx.conf /etc/nginx/nginx.conf