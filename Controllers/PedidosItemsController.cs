using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto.Parameters;

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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PedidosItems> ObtenerPedidoItemPorCodigoProductp(string codigoP)
        {
            try
            {
                var pedidoItem= _conn.Query<PedidosItems>($"Select * from PedidosItems where  IdProducto in (select Id_Producto from producto where codigo like '%{codigoP}%')").First();
                return Ok(pedidoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"No se pudo traer el PedidoItem: {ex.Message}");
            }
        }

        [HttpGet ("Codigo: string")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<PedidosItems> ObtenerPedidoItemPorCodigo(string codigo)
        {
            try
            {
                var pedidoItem= _conn.Query<PedidosItems>($"Select * from PedidosItems where  codigo like '%{codigo}%')").First();
                return Ok(pedidoItem);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"No se pudo traer el PedidoItem: {ex.Message}");
            }
        }
        
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertarPedidosItems([FromBody] PedidosItemsDTO pedidoI)

        {
            try
            {
                await _conn.QueryAsync(ObtenerInsert(pedidoI));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"No se pudo insertar el pedido {ex.Message}");
            }
        }
    
        [HttpDelete ("codigo: string")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletearP(string codigo)
        {
            try
            {
                _conn.Query<PedidosItems>($"Delete from PedidosItems where IdProducto in (Select Id_Producto from Producto where codigo='{codigo}')");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"No se pudo eliminar el pedido Item {ex.Message}");
            }
        }
    private string ObtenerInsert(PedidosItemsDTO pedidoI)
    {
        var pedido= _conn.Query<Pedidos>($"Select Id_pedido from Pedidos where  codigo='{pedidoI.codigoPedido}'").First();
        var producto= _conn.Query<Producto>($"Select Id_producto from Producto where  codigo='{pedidoI.codigoProducto}'").First();
        var sql= string.Format($"Insert into PedidosItems (IdPedido,IdProducto,cantidad,precio,codigo   ) values ('{pedido.ID_PEDIDO}','{producto.ID_PRODUCTO}','{pedidoI.Cantidad}','{pedidoI.precio}', '{pedidoI.CODIGO}')");
        return sql;
    }

    private bool Validar(PedidosItemsDTO pedidoI)
    {
        return 
            (
                pedidoI.codigoPedido != ""
                &&
                pedidoI.codigoProducto != ""
                &&
                pedidoI.Cantidad!=0
                &&
                pedidoI.precio!=0

            );
    }
    

    }
}