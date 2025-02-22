using AutoMapper;
using NetFirebase.Api.Models.Domain;
using NetFirebase.Api.Vms;

namespace NetFirebase.Api.Mappings;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Producto, ProductoVm>();
    }
}
