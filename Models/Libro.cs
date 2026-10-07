using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Libro
{
    public int LibroId { get; set; }

    [Required(ErrorMessage = "Ingresa el título.")]
    [StringLength(180, ErrorMessage = "El título admite hasta 180 caracteres.")]
    [Display(Name = "Título")]
    public string Titulo { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el ISBN.")]
    [StringLength(20, MinimumLength = 10, ErrorMessage = "El ISBN debe tener entre 10 y 20 caracteres.")]
    [Display(Name = "ISBN")]
    public string ISBN { get; set; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "Selecciona un autor.")]
    [Display(Name = "Autor")]
    public int AutorId { get; set; }

    [Range(0, 10000, ErrorMessage = "Los ejemplares deben estar entre 0 y 10 000.")]
    public int Ejemplares { get; set; }

    public bool Activo { get; set; } = true;
    public string AutorNombre { get; set; } = string.Empty;
}
