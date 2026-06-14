using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LogisticsApp.Data;
using LogisticsApp.Models;

namespace LogisticsApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ShipmentController : ControllerBase
    {
        private readonly LogisticsDbContext _context;

        // Demostración de Inyección de Dependencias
        public ShipmentController(LogisticsDbContext context)
        {
            _context = context;
        }

        // GET: api/shipment/tracking/{number}
        [HttpGet("tracking/{trackingNumber}")]
        public async Task<ActionResult<Shipment>> GetShipmentByTracking(string trackingNumber)
        {
            if (string.IsNullOrEmpty(trackingNumber))
            {
                return BadRequest("El número de rastreo es obligatorio.");
            }

            // Consulta asíncrona optimizada usando Entity Framework Core
            var shipment = await _context.Shipments
                .Include(s => s.ContainerDetails)
                .FirstOrDefaultAsync(s => s.TrackingNumber == trackingNumber);

            if (shipment == null)
            {
                return NotFound($"No se encontró ningún embarque con el número: {trackingNumber}");
            }

            return Ok(new { success = true, data = shipment });
        }
    }
}
