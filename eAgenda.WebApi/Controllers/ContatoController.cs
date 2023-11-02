using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApi.ViewModels.ModuloContato;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/contatos")]
    public class ContatoController : ControllerBase
    {
        private ServicoContato servicoContato;
        private IMapper mapeador;

        public ContatoController(ServicoContato servicoContato, IMapper mapeador)
        {
            this.mapeador = mapeador;
            this.servicoContato = servicoContato;
        }

        [HttpGet]
        public List<ListarContatoViewModel> SeleciontarTodos(StatusFavoritoEnum statusFavorito)
        {
            var contatos = servicoContato.SelecionarTodos(statusFavorito).Value;

            return mapeador.Map<List<ListarContatoViewModel>>(contatos);
        }

        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarContatoViewModel SeleciontarPorId(string id)
        {
            var contato = servicoContato.SelecionarPorId(Guid.Parse(id)).Value;

            return mapeador.Map<VisualizarContatoViewModel>(contato);
        }

        [HttpPost]
        public string Inserir(InserirContatoViewModel contatoViewModel)
        {
            var contato = mapeador.Map<Contato>(contatoViewModel);

            var resultado = servicoContato.Inserir(contato);

            if (resultado.IsSuccess)
                return "Contato inserido com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpPut("{id}")]
        public string Editar(Guid id, EditarContatoViewModel contatoViewModel)
        {
            var contatoEncontrado = servicoContato.SelecionarPorId(id).Value;

            var contato = mapeador.Map(contatoViewModel, contatoEncontrado);

            var resultado = servicoContato.Editar(contato);

            if (resultado.IsSuccess)
                return "Contato editado com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpDelete("{id}")]
        public string Excluir(string id)
        {
            var resultadoBusca = servicoContato.SelecionarPorId(Guid.Parse(id));

            if (resultadoBusca.IsFailed)

            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var contato = resultadoBusca.Value;

            var resultado = servicoContato.Excluir(contato);

            if (resultado.IsSuccess)
                return "Contato Excluído com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }
    }
}