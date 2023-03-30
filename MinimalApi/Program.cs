// Importación de los paquetes y librerías necesarios para crear la aplicación web
// * Microsoft.EntityFrameworkCore: Esel marco de trabajo ORM (Object-Relational Mapping) que permite interactuar con bases de datos relacionales utilizando
//   objetos en lugar de código SQL.
// * MinimalApi: La Api (proyecto) sobre el que estamos trabajando
// * MinimalApi.Models: Directorio donde encontramos los modelos generados a partir del ORM (Entity Framework)
using Microsoft.EntityFrameworkCore;
using MinimalApi;
using MinimalApi.Models;


// Creación del objeto builder: En si este objeto es la aplicacion Web.
var builder = WebApplication.CreateBuilder(args);

// Agregación de la funcionalidad de Endpoints API Explorer: Permite generar la pagina de documentacion que usaremos con Swagger.
builder.Services.AddEndpointsApiExplorer();

// Agregación de la base de datos a través del DbContext: La clase NetCoreDbContext fue creada al momento de correr el comando:
// Scaffold-DbContext "ConnectionString aqui" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
builder.Services.AddDbContext<NetCoreDbContext>();

// Agregación de la inyección de dependencias para la clase Demo:
// Esta clase esta creada tambien dentro del directorio Models.
builder.Services.AddTransient<Demo>();

// Agregación de la funcionalidad de Swagger para la documentación de la API.
// Se agrega en si el servicio de Swager a la aplicacion
builder.Services.AddSwaggerGen();

// Construcción de la aplicación web a través del objeto builder
// Crea la instancia a partir del objeto WebApplicationBuilder
var app = builder.Build();

// Uso de Swagger para la documentación de la API
// * UseSwagger crea la docuentacion en formato json
// * UseSwaggerUI crea la interfaz
app.UseSwagger();
app.UseSwaggerUI();


// Endpoint raiz, metodo get y devuelve un mensaje
app.MapGet("/", () => "Hello World!");

// Edpoint que recibe un parámetro llamda name y lo muestra concatenando. 
// Esta es una manera de recibir parametros, al invocarlo se usuario algo como: http://localhost:3000/hello?name=TUNombre
app.MapGet("/hello", (string name) => $"Hola {name}");

// Edpoint que recibe dos parámetros
app.MapGet("/hellowithname/{name}/{lastname}",
(string name, string lastname) =>
    $"Hola {name} {lastname}"
);

// Este endpoint hace una solicitud Get Asycrona al API de https://jsonplaceholder.typicode.com/todos
// Crea un objeto HttpClient, que utiliza el metodo GetASync para extraer la respuesta en response.
app.MapGet("/response", async () =>
{
    // Este objeto "HttpClient" Permite hacer solicitudes Http
    HttpClient client = new HttpClient();
    var response = await client.GetAsync("https://jsonplaceholder.typicode.com/todos");
    // Comprobación del código de estado de la respuesta mediante el metodo,
    // Si el codigo no es satisfactorio se genera una excepcion.
    response.EnsureSuccessStatusCode();

    // Extracción del cuerpo de la respuesta como una caena de texto y es devuelta 
    string responseBody = await response.Content.ReadAsStringAsync();
    return responseBody;
});

// Definición del endpoint que retorna una lista de objetos a través de una conexión a la base de datos
// Mediante el objeto creado por Entity Framework
app.MapGet("/person", () =>
{
    using (var context = new NetCoreDbContext())
        // "ToList()" es un método de extensión de la clase "IEnumerable<T>" que se utiliza para convertir una secuencia de elementos en una lista genérica "List<T>".
        // O sea convierte la coleccion de elementos de la tabla en una lista.
        return context.TblPeople.ToList();
});

// Definición del endpoint que utiliza la inyección de dependencias para obtener la lista de objetos
// En este caso usamos inyeccion de dependencias, evitando redundancia y traemos los datos con el metodo asyncrono TolistAsync()
app.MapGet("/personinject", async (NetCoreDbContext context) =>
await context.TblPeople.ToListAsync()
);

// Definición del endpoint que utiliza la inyección de dependencias para obtener una instancia de la clase Demo
app.MapGet("/personclass", (Demo demo) => demo.Hi());

// Definición del endpoint que recibe una solicitud POST y retorna un mensaje
app.MapPost("/post", (Data data) => $"{data.Id} {data.Name}");

// Definición del endpoint que retorna un objeto a través de una conexión a la base de datos
// Este metodo tiene un parametro que es enviado al metodo del context FindAsync.
// El metodo recibe un decimal, pues en la base de datos asi ha definido y la clase TblPersons.cs 
// Ha mapeado esta tabla y creado la propiedad decimal Id
app.MapGet("/personA/{id}", async (Decimal id, NetCoreDbContext context) =>
{
    var person = await context.TblPeople.FindAsync(id);
    return person != null ? Results.Ok(person) : Results.NotFound();
});

// Ejecución de la aplicación web
app.Run();



//