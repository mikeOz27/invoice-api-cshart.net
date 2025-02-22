using InvoiceApiRest.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace InvoiceApiRest.Models
{
    public class InvoiceDetail
    {
        [Key]
        public int Id { get; set; }

        [Required(ErrorMessage = "El campo InvoiceId es obligatorio.")]
        public int InvoiceId { get; set; }

        [Required(ErrorMessage = "El campo Producto es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre del producto no puede superar los 100 caracteres.")]
        [Display(Name = "Producto")]  // Esto cambia "Product" por "Producto"
        public string Product { get; set; }

        [Required(ErrorMessage = "El campo Cantidad es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "La cantidad debe ser mayor a 0.")]
        [Display(Name = "Cantidad")]
        public int Quantity { get; set; }

        [Required(ErrorMessage = "El campo Precio Unitario es obligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio unitario debe ser mayor a 0.")]
        [Display(Name = "Precio Unitario")]
        public decimal UnitPrice { get; set; }

        [NotMapped]
        [JsonIgnore(Condition = JsonIgnoreCondition.Never)]  // ✅ Asegura que siempre se incluya en JSON
        public decimal Subtotal => Quantity * UnitPrice;

        [JsonIgnore]
        public Invoice Invoice { get; set; }
    }
}
