using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Biblioteca.Web.Controllers;

public class LibrosController : Controller
{
    private readonly LibroRepositorio _repositorio;

    public LibrosController(LibroRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Index(string? buscar)
    {
        ViewData["Title"] = "Catálogo de libros";
        ViewData["Buscar"] = buscar;
        return View(await _repositorio.ListarAsync(buscar));
    }

    public async Task<IActionResult> Details(int id)
    {
        ViewData["Title"] = "Detalle del libro";
        var libro = await _repositorio.ObtenerPorIdAsync(id);
        return libro is null ? NotFound() : View(libro);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Nuevo libro";
        await CargarAutoresAsync();
        return View(new Libro());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Libro libro)
    {
        ViewData["Title"] = "Nuevo libro";
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        await _repositorio.InsertarAsync(libro);
        TempData["Mensaje"] = "Libro registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        ViewData["Title"] = "Editar libro";
        var libro = await _repositorio.ObtenerPorIdAsync(id);
        if (libro is null) return NotFound();
        await CargarAutoresAsync(libro.AutorId);
        return View(libro);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Libro libro)
    {
        ViewData["Title"] = "Editar libro";
        if (id != libro.LibroId) return BadRequest();
        if (!ModelState.IsValid)
        {
            await CargarAutoresAsync(libro.AutorId);
            return View(libro);
        }

        await _repositorio.ActualizarAsync(libro);
        TempData["Mensaje"] = "Libro actualizado correctamente.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        ViewData["Title"] = "Retirar libro";
        var libro = await _repositorio.ObtenerPorIdAsync(id);
        return libro is null ? NotFound() : View(libro);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        await _repositorio.EliminarAsync(id);
        TempData["Mensaje"] = "Libro retirado del catálogo.";
        return RedirectToAction(nameof(Index));
    }

    private async Task CargarAutoresAsync(int? seleccionado = null)
    {
        ViewData["Autores"] = new SelectList(await _repositorio.ListarAutoresAsync(),
            nameof(Autor.AutorId), nameof(Autor.Nombre), seleccionado);
    }
}
