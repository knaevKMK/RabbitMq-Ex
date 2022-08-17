using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using System;
using System.Threading.Tasks;

namespace Ticketing.Microservice.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TicketController : ControllerBase
    {
        private readonly IBus _bus;

        public TicketController(IBus bus)
        {
            _bus = bus;
        }

        [HttpGet("/")]
        public async Task<ActionResult> Ping()
        {
            await Task.CompletedTask;
            return Ok("Pong");
        }

        [HttpPost]
        public async Task<IActionResult> CreateTicket(Ticket ticket)
        {
            if (ticket != null)
            {
                ticket.BookedOn = DateTime.Now;
                Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
                var endPoint = await _bus.GetSendEndpoint(uri);
                await endPoint.Send(ticket);
                return Ok();
            }
            return BadRequest();
        }
    }
}