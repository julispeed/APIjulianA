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
    public class PruebaController : ControllerBase
    {
        private readonly MySqlConnection _conn;
        private readonly IMapper _mapper;
        public PruebaController(IConfiguration config, IMapper mapper)
        {
            _mapper = mapper;
            _conn = new MySqlConnection("server=localhost;database=JulianM;port=3306;uid=root;pwd=Real12341");
        }


        [HttpGet]
        public ActionResult<Producto> ObtenerAlgo()
        {
            try
            {
                var producto = _conn.Query<Producto>("SELECT * FROM PRODUCTO").First();
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Error al traer el producto: {ex.Message}"); 
            }
        }
        [HttpPost]
        public async Task<IActionResult> InsertarProducto([FromBody] ProductoInsertDTO prod) 
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

        private string ObtenerQueryInsert(ProductoInsertDTO prod)
        {
            var sql = string.Format(@"
            INSERT INTO PRODUCTO (NOMBRE, CODIGO) VALUES ('{0}', '{1}')
            ", prod.NOMBRE, prod.CODIGO);
            return sql;
        }

        private bool Validar(ProductoInsertDTO prod)
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
