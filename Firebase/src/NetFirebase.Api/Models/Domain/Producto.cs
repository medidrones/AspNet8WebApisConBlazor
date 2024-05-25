using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetFirebase.Api.Models.Domain;

public class Producto
{
    [Key]
    [Required]
    public int Id { get; set; }
    public string? Nombre { get; set; }
    public string? Descripcion { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,4)")]
    public decimal Precio { get; set; }
}
