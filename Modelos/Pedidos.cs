using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;

public class Pedidos
{
    
    public int ID_PEDIDO { get; set; }
    public DateTime FECHA { get; set; }
    public string CODIGO { get; set; }=string.Empty;
    
}

