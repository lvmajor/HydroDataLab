FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src
COPY src/Etl/ src/Etl/
RUN dotnet restore src/Etl/HydroDataLab.Etl.csproj --locked-mode
RUN dotnet publish src/Etl/HydroDataLab.Etl.csproj -c Release --no-restore -o /app

FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build /app .
COPY data/fixtures/ /app/fixtures/
ENV ASPNETCORE_HTTP_PORTS=8080 HYDRO_FIXTURE_DIR=/app/fixtures HYDRO_OUTPUT_DIR=/app/outputs
EXPOSE 8080
ENTRYPOINT ["dotnet", "HydroDataLab.Etl.dll"]
