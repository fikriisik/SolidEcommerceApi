FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
RUN apt-get update && apt-get install -y redis-tools
WORKDIR /src
COPY ["SolidEcommerceApi.csproj", "./"]
RUN dotnet restore "./SolidEcommerceApi.csproj"

COPY . .
WORKDIR "/src"
RUN dotnet build "SolidEcommerceApi.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SolidEcommerceApi.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SolidEcommerceApi.dll"]
