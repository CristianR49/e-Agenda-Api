using eAgenda.WebApi.ViewModels.ModuloCategoria;
using eAgenda.Dominio.ModuloDespesa;

namespace eAgenda.WebApi.Config.AutomapperConfig
{
    public class CategoriaProfile : Profile
    {
        public CategoriaProfile()
        {
            CreateMap<Categoria, ListarCategoriaViewModel>();
            CreateMap<Categoria, VisualizarCategoriaViewModel>();
            CreateMap<InserirCategoriaViewModel, Categoria>();
            CreateMap<EditarCategoriaViewModel, Categoria>();
        }
    }
}
