using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using System.Data;
using APITemperatura.Models;

namespace APITemperatura.Controllers
{
    public class TemperaturaController : Controller
    {
        private readonly IConfiguration _configuration;

        public TemperaturaController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var lista = new List<Temperatura>();
            string connectionString = _configuration.GetConnectionString("MySqlConnection");

            using (var conn = new MySqlConnection(connectionString))
            {
                conn.Open();
                var cmd = new MySqlCommand("SELECT fecha, temperatura, humedad FROM tblTemperatura ORDER BY fecha DESC LIMIT 20", conn);
                var reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    lista.Add(new Temperatura
                    {
                        fecha = reader.GetDateTime("fecha"),
                        temperatura = reader.GetDouble("temperatura"),
                        humedad = reader.GetDouble("humedad")
                    });
                }
            }

            return View(lista);
        }
    }
}
