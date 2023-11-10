FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Inveon.Services.Email/Inveon.Services.Email.csproj", "Inveon.Services.Email/"]
RUN dotnet restore "Inveon.Services.Email/Inveon.Services.Email.csproj"
COPY . .
WORKDIR "/src/Inveon.Services.Email"
RUN dotnet build "Inveon.Services.Email.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Inveon.Services.Email.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Inveon.Services.Email.dll"]
