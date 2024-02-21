# Use the official Microsoft .NET image.
# https://hub.docker.com/_/microsoft-dotnet
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["Grooveyard.Web/Grooveyard.Web.csproj", "Grooveyard.Web/"]
COPY ["Grooveyard.Common/Grooveyard.Common.csproj", "Grooveyard.Common/"]
COPY ["Grooveyard.Domain/Grooveyard.Domain.csproj", "Grooveyard.Domain/"]
COPY ["Grooveyard.Infrastructure/Grooveyard.Infrastructure.csproj", "Grooveyard.Infrastructure/"]
COPY ["Grooveyard.Services/Grooveyard.Services.csproj", "Grooveyard.Services/"]
RUN dotnet restore "Grooveyard.Web/Grooveyard.Web.csproj"
COPY . .
WORKDIR "/src/Grooveyard.Web"
RUN dotnet build "Grooveyard.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Grooveyard.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Grooveyard.Web.dll"]
