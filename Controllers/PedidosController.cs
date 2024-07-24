using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using MySql.Data.MySqlClient;

namespace aspnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosController : ControllerBase
    {
        private readonly MySqlConnection _conn;
        private readonly IMapper _mapper;
    
    public PedidosController(IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _conn = new MySqlConnection("server=localhost;database=JulianM;port=3306;uid=root;pwd=Real12341");
        }

        [HttpGet (Name="ObtenerPedidos")]
    public ActionResult<Pedidos> ObtenerPedidos()
        {
            try
            {
                var pedido = _conn.Query<Pedidos>("SELECT * FROM PEDIDOS");
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al traer el Pedido: {ex.Message}"); 
            }
        }

        [HttpGet ("Codigo: string")]
        public ActionResult<Pedidos> ObtenerPedido(string codigo)
        {
            try
            {
                var pedido= _conn.Query<Pedidos>($"Select * From Pedidos where codigo like '%{codigo}%'").First();
                return Ok(pedido);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"Error al traer el Pedido: {ex.Message}");
            }

        }

        [HttpPost (Name = "Crear_Pedido")]
        public async Task<IActionResult> GenerarPedido([FromBody] PedidoDTO pedido)
        {
            if(!Validar(pedido))
            {
                return BadRequest("No se puede insertar un codigo o fecha nula");
            }
            try
            {
                await _conn.QueryAsync(ObtenerQueryInsert(pedido));
                return Ok();
            }
             catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al insertar el Pedido: {ex.Message}"); 
            }
        }
            private bool Validar(PedidoDTO pedido)
        {
            return 
            (
                pedido.Fecha != ""
                &&
                pedido.CODIGO != ""
            );
        }
        private string ObtenerQueryInsert(PedidoDTO pedido)
        {
            var sql = string.Format(@"
            INSERT INTO PEDIDOS (FECHA, CODIGO) VALUES ('{0}', '{1}')
            ", pedido.Fecha, pedido.CODIGO);
            return sql;
        }
    }
}