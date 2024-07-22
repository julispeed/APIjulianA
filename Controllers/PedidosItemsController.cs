using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MySql.Data.MySqlClient;

namespace aspnetcore.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PedidosItems : ControllerBase
    {
        private readonly MySqlConnection _conn;
        private readonly IMapper _mapper;

        public PedidosItems(MySqlConnection conn, IMapper mapper)
        {
            _mapper=mapper;
            _conn= new MySqlConnection("server=localhost;database=JulianM;port=3306;uid=root;pwd=Real12341");
        }
    }
}