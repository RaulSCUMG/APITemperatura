using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using APITemperatura.Models;

namespace APITemperatura.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ArduinoController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        public ArduinoController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost]
        public IActionResult RegistrarDatos([FromBody] Temperatura datos)
        {
            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (var connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                string query = "INSERT INTO tblTemperatura (fecha, temperatura, humedad) VALUES (@fecha, @temperatura, @humedad)";
                using (var command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@fecha", DateTime.Now); // fecha del servidor
                    command.Parameters.AddWithValue("@temperatura", datos.temperatura);
                    command.Parameters.AddWithValue("@humedad", datos.humedad);

                    command.ExecuteNonQuery();
                }
            }

            return Ok(new { mensaje = "Datos insertados correctamente" });
        }
    }
}
