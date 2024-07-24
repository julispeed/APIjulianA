using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace aspnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosItemsController : ControllerBase
    {
        private readonly MySqlConnection _conn;
        private readonly IMapper _mapper;

        public PedidosItemsController(IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _conn = new MySqlConnection("server=localhost;database=JulianM;port=3306;uid=root;pwd=Real12341");
        }

        [HttpGet (Name ="ObtenerPedidosItems")]
        public ActionResult<PedidosItems> ObtenerPedidosItems()
        {
            try
            {
                var pedidoItem= _conn.Query<PedidosItems>("Select * from PedidosItems");
                return Ok(pedidoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al traer el PedidoItem: {ex.Message}"); 
            }
        }
        
        [HttpGet ("Codigo_Pedido: string")]
        public ActionResult<PedidosItems> ObtenerPedidoItemporCodigoPedido(string codigoPe)
        {
            try
            {
                var pedidoItem= _conn.Query<PedidosItems>($"Select * from PedidosItems where  IdPedido in (select Id_Pedido from pedidos where codigo like '%{codigoPe}%')").First();
                return Ok(pedidoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"No se pudo traer el PedidoItem: {ex.Message}");
            }
        }
        [HttpGet ("Codigo_Producto: string")]
        public ActionResult<PedidosItems> ObtenerPedidoItemPorCodigoProducot(string codigoP)
        {
            try
            {
                var pedidoItem= _conn.Query<PedidosItems>($"Select * from PedidosItems where  IdPedido in (select Id_Producto from producto where codigo like '%{codigoP}%')").First();
                return Ok(pedidoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"No se pudo traer el PedidoItem: {ex.Message}");
            }
        }
        


    }
}