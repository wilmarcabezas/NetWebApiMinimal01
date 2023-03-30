# NetWebApiMinimal01

Este es un proyecto de ejemplo que muestra cómo crear una API web en ASP.NET Core utilizando el enfoque de "Minimal APIs". En este proyecto se incluye un ejemplo de cómo definir rutas de API, utilizar la inyección de dependencias y trabajar con una base de datos SQLite.

## Requisitos

Para ejecutar este proyecto, necesitará tener instalado lo siguiente:

- .NET 6 SDK
- Visual Studio Code u otro editor de código

## Ejecución del proyecto

Para ejecutar este proyecto, siga estos pasos:

1. Clone el repositorio a su máquina local.
2. Abra una terminal y navegue hasta la carpeta del proyecto.
3. Ejecute el comando `dotnet run`.
4. Abra un navegador web y vaya a la dirección `https://localhost:5001`.

## Estructura del proyecto

El proyecto se organiza de la siguiente manera:

- `Program.cs` - Archivo principal del programa.
- `Startup.cs` - Archivo de configuración de la aplicación.
- `Controllers/PersonController.cs` - Control...
- `Controllers/PersonController.cs` - Controlador de ejemplo para la API de personas.
- `Data/NetCoreDbContext.cs` - Clase de contexto para la base de datos SQLite.
- `Models/Person.cs` - Modelo de datos para una persona.

## API

El proyecto incluye una API de ejemplo para consultar personas.

### Obtener todas las personas

Para obtener todas las personas, haga una solicitud GET a la siguiente dirección: https://localhost:5001/person


### Obtener una persona por ID

Para obtener una persona por ID, haga una solicitud GET a la siguiente dirección: https://localhost:5001/person/{id}


Reemplace `{id}` con el ID de la persona que desea consultar.

## Contribuir

Si desea contribuir a este proyecto, puede hacer lo siguiente:

- Crear una nueva rama de características (`git checkout -b my-new-feature`).
- Realizar cambios y hacer confirmaciones (`git commit -am 'Add some feature'`).
- Empujar a la rama (`git push origin my-new-feature`).
- Crear una solicitud de extracción en GitHub.

## Licencia

Este proyecto está licenciado bajo la Licencia MIT. Consulte el archivo `LICENSE` para obtener más detalles.





