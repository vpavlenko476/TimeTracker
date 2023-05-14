####################################################################################
## Build
####################################################################################
#
# Base dotnet image for building
FROM mcr.microsoft.com/dotnet/sdk:6.0-alpine AS build

#для упрощения жизни копируем все исходники в образ билда
COPY ./ .

# Restore
RUN dotnet restore --no-cache TimeTracker.Api/TimeTracker.Api.csproj

## Build
RUN dotnet publish TimeTracker.Api/TimeTracker.Api.csproj -c Release -o /publish/TimeTracker.Api --no-restore

#####################################################################################
### Prepare and execution
#####################################################################################

FROM mcr.microsoft.com/dotnet/aspnet:6.0.3

RUN groupadd -g 1000 -r user && \
    useradd -u 1000 -r user -g user -b "/app"
USER user

WORKDIR /app
COPY --chown=user --from=build /publish/TimeTracker.Api .

RUN chmod -s -R /app

ENTRYPOINT ["dotnet", "TimeTracker.Api.dll"]