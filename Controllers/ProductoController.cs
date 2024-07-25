using System.Collections.Frozen;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Dapper;
using Dapper.Contrib.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace aspnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductoController : ControllerBase
    {
        private readonly MySqlConnection _conn;
        private readonly IMapper _mapper;
        public ProductoController(IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _conn = new MySqlConnection("server=localhost;database=JulianM;port=3306;uid=root;pwd=Real12341");
        }


        [HttpGet]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Producto> ObtenerProductos()
        {
            try
            {
                var producto = _conn.Query<Producto>("SELECT * FROM PRODUCTO");
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al traer el producto: {ex.Message}"); 
            }
        }

        [HttpGet ("codigo: string")]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public ActionResult<Producto> ObtenerProductos(string codigo)
        {
            try
            {
                var producto= _conn.Query<Producto>($"Select * from Producto where codigo like '%{codigo}%'").First();
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al traer el producto: {ex.Message}");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertarProducto([FromBody] ProductoDTO prod) 
        {
            if (!Validar(prod))
            {
                return BadRequest("No se puede insertar un codigo o nombre nulo");
            }

            try
            {
                
                await _conn.QueryAsync(ObtenerQueryInsert(prod));
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al insertar el producto: {ex.Message}"); 
            }
        }

        [HttpDelete]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletearP(string codigo)
        {
            try
            {
                _conn.Query<PedidosItems>($"Delete from Producto where codigo='{codigo}'");
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,$"No se pudo eliminar el pedido {ex.Message}");
            }
        }
        private string ObtenerQueryInsert(ProductoDTO prod)
        {
            var sql = string.Format(@"
            INSERT INTO PRODUCTO (NOMBRE, CODIGO) VALUES ('{0}', '{1}')
            ", prod.NOMBRE, prod.CODIGO);
            return sql;
        }

        private bool Validar(ProductoDTO prod)
        {
            return 
            (
                prod.NOMBRE != ""
                &&
                prod.CODIGO != ""
            );
        }
    }
}
