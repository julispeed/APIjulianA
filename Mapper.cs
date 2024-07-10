using AutoMapper;

public class Mapper : Profile
{
    public Mapper()
    {
        CreateMap<ProductoInsertDTO, Producto>();
    }
}