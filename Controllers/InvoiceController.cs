using InvoiceApiRest.Data;
using InvoiceApiRest.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using YourNamespace.Models.DTOs;

namespace InvoiceApiRest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceDbContext _context;

        public InvoiceController(InvoiceDbContext context)
        {
            _context = context;
        }

        // 📌 OBTENER FACTURAS CON DETALLES
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Invoice>>> GetInvoices()
        {
            return await _context.Invoices.Include(f => f.Details).ToListAsync();
        }

        // 📌 OBTENER UNA FACTURA POR ID
        [HttpGet("{id}")]
        public async Task<ActionResult<Invoice>> GetInvoice(int id)
        {
            var invoice = await _context.Invoices.Include(f => f.Details).FirstOrDefaultAsync(f => f.Id == id);
            if (invoice == null) return NotFound();
            return invoice;
        }

        // 📌 CREAR UNA FACTURA CON DETALLES
        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] InvoiceDto invoiceDto)
        {
            if (invoiceDto.Details == null || invoiceDto.Details.Count == 0)
            {
                return BadRequest(new
                {
                    status = 400,
                    message = "La factura debe tener al menos un detalle."
                });
            }

            // Calcular el total de la factura sumando los subtotales de cada detalle
            decimal total = invoiceDto.Details.Sum(d => d.Quantity * d.UnitPrice);

            // Crear la factura sin recibir el Total del DTO
            var invoice = new Invoice
            {
                Customer = invoiceDto.Customer,
                Date = invoiceDto.Date,
                Total = total // Asignamos el total calculado
            };

            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync(); // Guardamos para obtener el Id

            // Asignamos el ID a los detalles y los agregamos
            var details = invoiceDto.Details.Select(d => new InvoiceDetail
            {
                InvoiceId = invoice.Id,
                Product = d.Product,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            }).ToList();

            _context.InvoiceDetails.AddRange(details);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInvoice), new { id = invoice.Id }, new { message = "Factura creada exitosamente", invoice });
        }

        // 📌 ACTUALIZAR UNA FACTURA Y SUS DETALLES
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, InvoiceDto invoiceDto)
        {
            // Calcular el total de la factura sumando los subtotales de cada detalle
            decimal total = invoiceDto.Details.Sum(d => d.Quantity * d.UnitPrice);
            var invoice = await _context.Invoices
                .Include(i => i.Details)
                .FirstOrDefaultAsync(i => i.Id == id);

            if (invoice == null)
            {
                return NotFound();
            }

            // Actualizar los campos de la factura
            invoice.Customer = invoiceDto.Customer;
            invoice.Date = invoiceDto.Date;
            invoice.Total = total;

            // Eliminar detalles anteriores y agregar nuevos
            _context.InvoiceDetails.RemoveRange(invoice.Details);
            invoice.Details = invoiceDto.Details.Select(d => new InvoiceDetail
            {
                InvoiceId = invoice.Id,
                Product = d.Product,
                Quantity = d.Quantity,
                UnitPrice = d.UnitPrice
            }).ToList();

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { message = "Factura actualizada correctamente", invoice });
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return NotFound(new { message = "Factura no encontrada después de la actualización" });
                }
                throw;
            }
        }

        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.Id == id);
        }

        // 📌 ELIMINAR UNA FACTURA Y SUS DETALLES
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var factura = await _context.Invoices.FindAsync(id);
            if (factura == null) return NotFound(new { message = "No se encontro la factura" });

            _context.Invoices.Remove(factura);
            await _context.SaveChangesAsync();
            return Ok(new { message = "Factura eliminada correctamente" });
        }

        // 📌 ELIMINAR UN DETALLE POR SU ID
        [HttpDelete("detailDelete/{detailId}")]
        public async Task<IActionResult> DeleteDetail(int detailId)
        {
            var detail = await _context.InvoiceDetails.FindAsync(detailId);

            if (detail == null)
            {
                return NotFound(new { message = "Detalle no encontrado" });
            }

            _context.InvoiceDetails.Remove(detail);
     
            await _context.SaveChangesAsync();

            // Actualizar el total de la factura
            var invoice = await _context.Invoices.Include(i => i.Details).FirstOrDefaultAsync(i => i.Id == detail.InvoiceId);
            if (invoice != null)
            {
                invoice.CalculateTotal();
                await _context.SaveChangesAsync();
            }

            return Ok(new { message = "Detalle eliminado correctamente" });
        }
    }
}
