# InvoiceApp

This project was generated using [Angular CLI](https://github.com/angular/angular-cli) version 19.1.7.

## Development server
```
To start a local development server, run:

bash# Proyecto API C# .NET 8

Este repositorio contiene dos proyectos:
1. **Backend**: API desarrollada en **C# .NET 8** con **Entity Framework Core** y **SQL Server**.
```

## Requisitos Previos
Antes de comenzar, asegúrate de tener instaladas las siguientes herramientas:

- [SQL Server](https://www.microsoft.com/en-us/sql-server/sql-server-downloads)

## Instalación y Ejecución del Proyecto .NET 8

1. Navega al directorio del backend:
   ```sh
   cd backend
   ```

2. Configura la cadena de conexión en el archivo `appsettings.json`:
   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Server=tu_servidor;Database=tu_base_de_datos;User Id=tu_usuario;Password=tu_contraseña;"
     }
   }
   ```

3. Aplica las migraciones a la base de datos:
   ```sh
   dotnet ef database update
   ```

4. Ejecuta el proyecto con Visual Studio 2022:
   - Abre `InvoiceApiRest.sln` en Visual Studio.
   - Presiona `Ctrl + F5` para ejecutar la API.
   
5. La API estará disponible en `https://localhost:7205`.

---

## Notas Adicionales
- Si realizas cambios en los modelos de la base de datos, ejecuta:
  ```sh
  dotnet ef migrations add NombreDeLaMigracion
  dotnet ef database update
  ```
- Para compilar la API desde la línea de comandos:
  ```sh
  dotnet build
  dotnet run
  ```

---

## Contacto
Si tienes dudas o problemas, abre un issue en el repositorio o contacta al equipo de desarrollo.
