# Imagen base para el runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

# Imagen para compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar el archivo .csproj antes del resto del código
COPY ["InvoiceApiRest\InvoiceApiRest.csproj", "InvoiceApiRest/"]
WORKDIR "/InvoiceApiRest"
RUN dotnet restore "InvoiceApiRest\InvoiceApiRest.csproj"

# Copiar el resto del código y compilar
COPY . .
RUN dotnet build "InvoiceApiRest\InvoiceApiRest.csproj" -c Release -o /app/build

# Publicar la aplicación
FROM build AS publish
RUN dotnet publish "InvoiceApiRest\InvoiceApiRest.csproj" -c Release -o /app/publish

# Imagen final que ejecutará la app
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "InvoiceApiRest.dll"]
