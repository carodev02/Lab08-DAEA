# Biblioteca Web LAB 08

Aplicación ASP.NET Core MVC para administrar la base `BibliotecaDB` creada en el LAB 07. El proyecto usa Dapper, procedimientos almacenados, inyección de dependencias y operaciones asíncronas.

## Tecnologías

- .NET 8 y ASP.NET Core MVC
- Dapper 2.1.66
- Microsoft.Data.SqlClient 6.1.1
- SQL Server LocalDB
- Razor, Bootstrap y estilos propios

## Ejecución

1. Verifica que la base `BibliotecaDB` del LAB 07 exista en `(localdb)\MSSQLLocalDB`.
2. Si aún no existe, ejecuta `Scripts/01-base-semana07.sql`.
3. Ejecuta `Scripts/02-procedimientos-semana08.sql` para crear o actualizar los procedimientos.
4. Abre `Biblioteca.Web.csproj` en Visual Studio y ejecuta el proyecto.

La cadena de conexión se encuentra en `appsettings.json`. Si tu instancia tiene otro nombre, cambia únicamente `Data Source`.

## Estructura

- `Models`: entidades y validaciones con DataAnnotations.
- `Repositorios`: acceso a datos con Dapper y procedimientos almacenados.
- `Controllers`: flujo MVC, validación, TempData y patrón Post/Redirect/Get.
- `Views`: vistas Razor fuertemente tipadas y parcial `_LibroFila`.
- `Scripts`: base del LAB 07 y procedimientos del LAB 08.

## Recorrido de una petición

En la búsqueda de libros, la petición `GET /Libros?buscar=soledad` llega a `LibrosController.Index`. El controlador recibe el texto y llama de forma asíncrona a `LibroRepositorio.ListarAsync`. Si existe un término, el repositorio abre una conexión y ejecuta `sp_Libros_BuscarPorTitulo` con Dapper mediante `QueryAsync`, `CommandType.StoredProcedure` y un parámetro. El resultado vuelve como una colección de `Libro` y se entrega a `Views/Libros/Index.cshtml`, que renderiza cada registro mediante `_LibroFila.cshtml`.

El modelo transporta los registros porque representa los datos principales de la vista. `ViewData["Buscar"]` conserva el criterio escrito en el campo de búsqueda y `ViewData["Title"]` define el título de la página. Después de crear, editar o retirar un libro, `TempData["Mensaje"]` conserva la confirmación durante la redirección del patrón Post/Redirect/Get.

## Observaciones y conclusiones

El acceso a SQL Server está concentrado en repositorios; los controladores no contienen SQL ni crean conexiones. La eliminación de libros es lógica, por lo que los registros históricos siguen disponibles. Todas las operaciones de datos usan `async/await` de extremo a extremo y la validación ocurre tanto en el cliente como en el servidor.
