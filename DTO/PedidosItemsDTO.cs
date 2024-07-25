using Org.BouncyCastle.Crypto.Agreement.Srp;

public class PedidosItemsDTO
{
    public string codigoPedido { get; set; } =string.Empty;
    public string codigoProducto{ get; set; }=string.Empty;
    public int Cantidad { get; set; }
    public double precio { get; set; }
    public string CODIGO { get; set; } = string.Empty;

}