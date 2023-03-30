// Importaci�n de los paquetes y librer�as necesarios para crear la aplicaci�n web
// * Microsoft.EntityFrameworkCore: Esel marco de trabajo ORM (Object-Relational Mapping) que permite interactuar con bases de datos relacionales utilizando
//   objetos en lugar de c�digo SQL.
// * MinimalApi: La Api (proyecto) sobre el que estamos trabajando
// * MinimalApi.Models: Directorio donde encontramos los modelos generados a partir del ORM (Entity Framework)
using Microsoft.EntityFrameworkCore;
using MinimalApi;
using MinimalApi.Models;


// Creaci�n del objeto builder: En si este objeto es la aplicacion Web.
var builder = WebApplication.CreateBuilder(args);

// Agregaci�n de la funcionalidad de Endpoints API Explorer: Permite generar la pagina de documentacion que usaremos con Swagger.
builder.Services.AddEndpointsApiExplorer();

// Agregaci�n de la base de datos a trav�s del DbContext: La clase NetCoreDbContext fue creada al momento de correr el comando:
// Scaffold-DbContext "ConnectionString aqui" Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models
builder.Services.AddDbContext<NetCoreDbContext>();

// Agregaci�n de la inyecci�n de dependencias para la clase Demo:
// Esta clase esta creada tambien dentro del directorio Models.
builder.Services.AddTransient<Demo>();

// Agregaci�n de la funcionalidad de Swagger para la documentaci�n de la API.
// Se agrega en si el servicio de Swager a la aplicacion
builder.Services.AddSwaggerGen();

// Construcci�n de la aplicaci�n web a trav�s del objeto builder
// Crea la instancia a partir del objeto WebApplicationBuilder
var app = builder.Build();

// Uso de Swagger para la documentaci�n de la API
// * UseSwagger crea la docuentacion en formato json
// * UseSwaggerUI crea la interfaz
app.UseSwagger();
app.UseSwaggerUI();


// Endpoint raiz, metodo get y devuelve un mensaje
app.MapGet("/", () => "Hello World!");

// Edpoint que recibe un par�metro llamda name y lo muestra concatenando. 
// Esta es una manera de recibir parametros, al invocarlo se usuario algo como: http://localhost:3000/hello?name=TUNombre
app.MapGet("/hello", (string name) => $"Hola {name}");

// Edpoint que recibe dos par�metros
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
    // Comprobaci�n del c�digo de estado de la respuesta mediante el metodo,
    // Si el codigo no es satisfactorio se genera una excepcion.
    response.EnsureSuccessStatusCode();

    // Extracci�n del cuerpo de la respuesta como una caena de texto y es devuelta 
    string responseBody = await response.Content.ReadAsStringAsync();
    return responseBody;
});

// Definici�n del endpoint que retorna una lista de objetos a trav�s de una conexi�n a la base de datos
// Mediante el objeto creado por Entity Framework
app.MapGet("/person", () =>
{
    using (var context = new NetCoreDbContext())
        // "ToList()" es un m�todo de extensi�n de la clase "IEnumerable<T>" que se utiliza para convertir una secuencia de elementos en una lista gen�rica "List<T>".
        // O sea convierte la coleccion de elementos de la tabla en una lista.
        return context.TblPeople.ToList();
});

// Definici�n del endpoint que utiliza la inyecci�n de dependencias para obtener la lista de objetos
// En este caso usamos inyeccion de dependencias, evitando redundancia y traemos los datos con el metodo asyncrono TolistAsync()
app.MapGet("/personinject", async (NetCoreDbContext context) =>
await context.TblPeople.ToListAsync()
);

// Definici�n del endpoint que utiliza la inyecci�n de dependencias para obtener una instancia de la clase Demo
app.MapGet("/personclass", (Demo demo) => demo.Hi());

// Definici�n del endpoint que recibe una solicitud POST y retorna un mensaje
app.MapPost("/post", (Data data) => $"{data.Id} {data.Name}");

// Definici�n del endpoint que retorna un objeto a trav�s de una conexi�n a la base de datos
// Este metodo tiene un parametro que es enviado al metodo del context FindAsync.
// El metodo recibe un decimal, pues en la base de datos asi ha definido y la clase TblPersons.cs 
// Ha mapeado esta tabla y creado la propiedad decimal Id
app.MapGet("/personA/{id}", async (Decimal id, NetCoreDbContext context) =>
{
    var person = await context.TblPeople.FindAsync(id);
    return person != null ? Results.Ok(person) : Results.NotFound();
});

// Ejecuci�n de la aplicaci�n web
app.Run();



//