#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IS.ScaleModelsShop.API/IS.ScaleModelsShop.API.csproj", "IS.ScaleModelsShop.API/"]
COPY ["IS.ScaleModelsShop.Application/IS.ScaleModelsShop.Application.csproj", "IS.ScaleModelsShop.Application/"]
COPY ["IS.ScaleModelsShop.API.Contracts/IS.ScaleModelsShop.API.Contracts.csproj", "IS.ScaleModelsShop.API.Contracts/"]
COPY ["IS.ScaleModelsShop.Domain/IS.ScaleModelsShop.Domain.csproj", "IS.ScaleModelsShop.Domain/"]
COPY ["IS.ScaleModelsShop.Infrastructure/IS.ScaleModelsShop.Infrastructure.csproj", "IS.ScaleModelsShop.Infrastructure/"]
RUN dotnet restore "IS.ScaleModelsShop.API/IS.ScaleModelsShop.API.csproj"
COPY . .
WORKDIR "/src/IS.ScaleModelsShop.API"
RUN dotnet build "IS.ScaleModelsShop.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IS.ScaleModelsShop.API.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IS.ScaleModelsShop.API.dll"]