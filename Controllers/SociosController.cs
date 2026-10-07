using Biblioteca.Web.Models;
using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class SociosController : Controller
{
    private readonly SocioRepositorio _repositorio;

    public SociosController(SocioRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Socios";
        return View(await _repositorio.ListarAsync());
    }

    public async Task<IActionResult> Create()
    {
        await Task.CompletedTask;
        ViewData["Title"] = "Nuevo socio";
        return View(new Socio());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Socio socio)
    {
        ViewData["Title"] = "Nuevo socio";
        if (!ModelState.IsValid) return View(socio);

        try
        {
            await _repositorio.InsertarAsync(socio);
        }
        catch (DniDuplicadoException)
        {
            ModelState.AddModelError(nameof(Socio.DNI), "Este DNI ya pertenece a un socio registrado.");
            return View(socio);
        }

        TempData["Mensaje"] = "Socio registrado correctamente.";
        return RedirectToAction(nameof(Index));
    }
}
