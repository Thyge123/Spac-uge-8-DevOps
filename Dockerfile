FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 7226

ENV ASPNETCORE_URLS=http://+:7226


FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG configuration=Release
WORKDIR /src
COPY ["CerealAPI/CerealAPI.csproj", "CerealAPI/"]
RUN dotnet restore "CerealAPI/CerealAPI.csproj"
COPY . .
WORKDIR "/src/CerealAPI"
RUN dotnet build "CerealAPI.csproj" -c $configuration -o /app/build

FROM build AS publish
ARG configuration=Release
RUN dotnet publish "CerealAPI.csproj" -c $configuration -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CerealAPI.dll"]