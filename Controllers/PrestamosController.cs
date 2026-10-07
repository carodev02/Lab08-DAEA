using Biblioteca.Web.Repositorios;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Web.Controllers;

public class PrestamosController : Controller
{
    private readonly PrestamoRepositorio _repositorio;

    public PrestamosController(PrestamoRepositorio repositorio) => _repositorio = repositorio;

    public async Task<IActionResult> Reporte(DateTime? desde, DateTime? hasta)
    {
        ViewData["Title"] = "Reporte de préstamos";
        var fechaHasta = (hasta ?? DateTime.Today).Date;
        var fechaDesde = (desde ?? fechaHasta.AddMonths(-2)).Date;
        ViewData["Desde"] = fechaDesde.ToString("yyyy-MM-dd");
        ViewData["Hasta"] = fechaHasta.ToString("yyyy-MM-dd");

        if (fechaDesde > fechaHasta)
        {
            ModelState.AddModelError(string.Empty, "La fecha inicial no puede ser posterior a la fecha final.");
            return View(Array.Empty<Models.PrestamoReporte>());
        }

        return View(await _repositorio.ReporteAsync(fechaDesde, fechaHasta));
    }
}
