FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /source
COPY src/Api/ src/Api/
RUN dotnet publish src/Api/HydroDataLab.Api.csproj -c Release -o /app
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
ENV ASPNETCORE_HTTP_PORTS=8080
ENV Hydro__DataDir=/state
EXPOSE 8080
ENTRYPOINT ["dotnet", "HydroDataLab.Api.dll"]
