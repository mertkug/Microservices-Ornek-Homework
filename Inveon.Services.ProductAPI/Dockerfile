FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Inveon.Services.ProductAPI/Inveon.Services.ProductAPI.csproj", "Inveon.Services.ProductAPI/"]
RUN dotnet restore "Inveon.Services.ProductAPI/Inveon.Services.ProductAPI.csproj"
COPY . .
WORKDIR "/src/Inveon.Services.ProductAPI"
RUN dotnet build "Inveon.Services.ProductAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inveon.Services.ProductAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inveon.Services.ProductAPI.dll"]
