using InvoiceApiRest.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.CompilerServices;

namespace InvoiceApiRest.Models
{
    [Table("Invoice")] // Asegura que la tabla se llame 'Invoice'
    public class Invoice
    {
        public int Id { get; set; }

        public string NumInvoice { get; set; } = GenerateRandomInvoiceNumber();


        [Required(ErrorMessage = "El campo cliente es requerido.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar los 100 caracteres.")]
        public string Customer { get; set; }

        [Required(ErrorMessage = "El campo fecha es obligatorio.")]
        //public DateTime Date { get; set; }

        //CONGIGURACIÓN PARA POSTGRES
        private DateTime? _date;
        public DateTime? Date
        {
            get => _date;
            set => _date = value.HasValue ? DateTime.SpecifyKind(value.Value, DateTimeKind.Utc) : null;
        }

        public string Status { get; set; } = "active";  // ✅ Definir valor por defecto aquí

        public decimal Total { get; set; }

        // 🔥 Inicializamos la lista para evitar que sea null
        public List<InvoiceDetail> Details { get; set; } = new List<InvoiceDetail>();

        public static string GenerateRandomInvoiceNumber()
        {
            return $"INV-{DateTime.Now:yyyyMMdd}{new Random().Next(100000, 999999)}";
        }

        // 📌 Método para recalcular el total de la factura
        public void CalculateTotal()
        {
            Total = Details.Sum(d => d.Quantity * d.UnitPrice);
        }
    }
}
