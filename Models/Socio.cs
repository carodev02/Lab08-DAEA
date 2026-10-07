using System.ComponentModel.DataAnnotations;

namespace Biblioteca.Web.Models;

public class Socio
{
    public int SocioId { get; set; }

    [Required(ErrorMessage = "Ingresa el DNI.")]
    [StringLength(15, MinimumLength = 8, ErrorMessage = "El DNI debe tener entre 8 y 15 caracteres.")]
    [RegularExpression(@"^[0-9]+$", ErrorMessage = "El DNI solo admite números.")]
    [Display(Name = "DNI")]
    public string DNI { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el nombre.")]
    [StringLength(140, ErrorMessage = "El nombre admite hasta 140 caracteres.")]
    public string Nombre { get; set; } = string.Empty;

    [Required(ErrorMessage = "Ingresa el correo.")]
    [EmailAddress(ErrorMessage = "Ingresa un correo válido.")]
    [StringLength(180)]
    [Display(Name = "Correo electrónico")]
    public string Email { get; set; } = string.Empty;

    public bool Activo { get; set; } = true;
}
