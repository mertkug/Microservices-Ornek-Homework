FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Inveon.Services.OrderAPI/Inveon.Services.OrderAPI.csproj", "Inveon.Services.OrderAPI/"]
RUN dotnet restore "Inveon.Services.OrderAPI/Inveon.Services.OrderAPI.csproj"
COPY . .
WORKDIR "/src/Inveon.Services.OrderAPI"
RUN dotnet build "Inveon.Services.OrderAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inveon.Services.OrderAPI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inveon.Services.OrderAPI.dll"]
