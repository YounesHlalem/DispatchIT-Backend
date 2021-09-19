FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build

COPY . /src/BackendFunctions
RUN cd /src/BackendFunctions && \
     mkdir -p /home/site/backendapi && \
     dotnet restore IT.Netic.DispatchIt.Web.Backend.Functions/IT.Netic.DispatchIt.Web.Backend.Functions.csproj && \
     dotnet publish IT.Netic.DispatchIt.Web.Backend.Functions/IT.Netic.DispatchIt.Web.Backend.Functions.csproj --self-contained -r linux-x64 --configuration release --output /home/site/backendapi

FROM mcr.microsoft.com/azure-functions/dotnet:3.0-dotnet3-appservice as runtime
ENV AzureWebJobsStorage "DefaultEndpointsProtocol=https;AccountName=-;AccountKey=-;EndpointSuffix=core.windows.net"
ENV AzureWebJobsScriptRoot=/home/site/wwwroot \ AzureFunctionsJobHost__Logging__Console__IsEnabled=true 
ENV WEBSITE_HOSTNAME localhost
ENV WEBSITE_SITE_NAME BackendAPI
ENV AZURE_FUNCTIONS_ENVIRONMENT Development
ENV FUNCTIONS_WORKER_RUNTIME dotnet
ENV ConnectionStrings__CosmosDbHost "......mongo.cosmos.azure.com"
ENV ConnectionStrings__CosmosDbUserName "-"
ENV ConnectionStrings__CosmosDbPassword "-"
ENV ConnectionStrings__SqlServerName "......mysql.database.azure.com"
ENV ConnectionStrings__SqlDatabaseName "dispatchit"
ENV ConnectionStrings__SqlUserName "dispatchit@....."
ENV ConnectionStrings__SqlPassword "-"
ENV ConnectionStrings__EventHubName "dispatchit-eventhub"
ENV ConnectionStrings__EventHubConnectionString "Endpoint=sb://.....windows.net/;SharedAccessKeyName=....;SharedAccessKey=....."
ENV ConnectionStrings__TopicEndpoint "https://....azure.net/api/events"
ENV ConnectionStrings__TopicKey "//-="
ENV ConnectionStrings__Instance "https://login.microsoftonline.com/"
ENV ConnectionStrings__TenantId "-"
ENV ConnectionStrings__ClientId "-"

COPY --from=build ["/home/site/backendapi", "/home/site/wwwroot"]