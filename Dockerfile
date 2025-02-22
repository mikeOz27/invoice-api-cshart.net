# Imagen base con el runtime de .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Imagen para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar los archivos del proyecto y restaurar paquetes
COPY InvoiceApiRest/*.csproj ./InvoiceApiRest/
WORKDIR /src/InvoiceApiRest
RUN dotnet restore

# Copiar el resto del código
COPY . .
RUN dotnet build -c Release -o /app/build

# Publicar la aplicación
RUN dotnet publish -c Release -o /app/publish

# Imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "InvoiceApiRest.dll"]
