using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Producer.Models;

namespace Producer.Controllers
{
    [ApiController]
    [Route("")]
    public class WeatherForecastController : ControllerBase
    {


        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IBus _bus;
        public WeatherForecastController(ILogger<WeatherForecastController> logger, IBus bus)
        {
            _logger = logger;
            _bus = bus;
        }

        [HttpGet]
        public async Task<ActionResult> Get()
        {
            var ticket = new Ticket();
            _logger.LogInformation($"New Ticket created with msg {ticket.Message}");
            Uri uri = new Uri("rabbitmq://localhost/ticketQueue");
       //     var response = await _bus.Request<Ticket, Ticket>(uri,ticket);
            var endPoint = await _bus.GetSendEndpoint(uri);
            await endPoint.Send(ticket);
            return Ok(ticket);
        }
    }
}