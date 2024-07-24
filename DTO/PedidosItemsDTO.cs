using Org.BouncyCastle.Crypto.Agreement.Srp;

public class PedidosItemsDTO
{
    public string IdPedido { get; set; } =string.Empty;
    public string IdProducto{ get; set; }=string.Empty;
    public int Cantidad { get; set; }
    public double precio { get; set; }

}