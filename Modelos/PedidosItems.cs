using System.ComponentModel;
using System.Data;
using System.Xml.Serialization;

public class PedidosItems
{
    
    public int IdPedido_Item { get; set; }  
    public int IdPedido { get; set; }
    public int IdProducto { get; set; }
    public int Cantidad { get; set; }
    public double precio { get; set; }
    
}

