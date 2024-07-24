using AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<ProductoDTO, Producto>();
        CreateMap<PedidoDTO, Pedidos>();
        CreateMap<PedidosItemsDTO, PedidosItems>();
    }
}