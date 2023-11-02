using eAgenda.Aplicacao.ModuloDespesa;
using eAgenda.Dominio.ModuloDespesa;
using eAgenda.WebApi.ViewModels.ModuloCategoria;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/categorias")]
    public class CategoriaController : ControllerBase
    {
        private ServicoCategoria servicoCategoria;
        private IMapper mapeador;

        public CategoriaController(ServicoCategoria servicoCategoria, IMapper mapeador)
        {
            this.mapeador = mapeador;
            this.servicoCategoria = servicoCategoria;
        }

        [HttpGet]
        public List<ListarCategoriaViewModel> SeleciontarTodos()
        {
            var categorias = servicoCategoria.SelecionarTodos().Value;

            return mapeador.Map<List<ListarCategoriaViewModel>>(categorias);
        }

        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarCategoriaViewModel SeleciontarPorId(string id)
        {
            var categoria = servicoCategoria.SelecionarPorId(Guid.Parse(id)).Value;

            return mapeador.Map<VisualizarCategoriaViewModel>(categoria);
        }

        [HttpPut]
        public string Inserir(InserirCategoriaViewModel categoriaViewModel)
        {
            var categoria = mapeador.Map<Categoria>(categoriaViewModel);

            var resultado = servicoCategoria.Inserir(categoria);

            if (resultado.IsSuccess)
                return "Categoria inserida com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpPost("{id}")]
        public string Editar(Guid id, EditarCategoriaViewModel categoriaViewModel)
        {
            var categoriaEncontrada = servicoCategoria.SelecionarPorId(id).Value;

            var categoria = mapeador.Map(categoriaViewModel, categoriaEncontrada);

            var resultado = servicoCategoria.Editar(categoria);

            if (resultado.IsSuccess)
                return "Categoria editada com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpDelete("{id}")]
        public string Excluir(string id)
        {
            var resultadoBusca = servicoCategoria.SelecionarPorId(Guid.Parse(id));

            if (resultadoBusca.IsFailed)

            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var categoria = resultadoBusca.Value;

            var resultado = servicoCategoria.Excluir(categoria);

            if (resultado.IsSuccess)
                return "Categoria Excluída com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }
    }
}