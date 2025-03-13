using API_Technical_Test.Modelos;
using API_Technical_Test.Modelos.DTO;
using AutoMapper;

namespace API_Technical_Test
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            //Esta es una forma
            CreateMap<Clientes, ClienteDTO>();
            CreateMap<ClienteDTO, Clientes>();

            //Esta es otra forma
            CreateMap<Clientes, ClienteUpdateDTO>().ReverseMap();
            CreateMap<Clientes, ClienteCreateDTO>().ReverseMap();
        }
    }
}
