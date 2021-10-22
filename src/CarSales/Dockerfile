#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["src/CarSales/CarSales.csproj", "src/CarSales/"]
COPY ["src/CarSales.Services/CarSales.Services.csproj", "src/CarSales.Services/"]
COPY ["src/CarSales.Domain/CarSales.Domain.csproj", "src/CarSales.Domain/"]
COPY ["src/CarSales.Repository/CarSales.Repository.csproj", "src/CarSales.Repository/"]
RUN dotnet restore "src/CarSales/CarSales.csproj"
COPY . .
WORKDIR "/src/src/CarSales"
RUN dotnet build "CarSales.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CarSales.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CarSales.dll"]