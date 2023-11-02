using eAgenda.Aplicacao.ModuloCompromisso;
using eAgenda.Infra.Orm.ModuloCompromisso;
using eAgenda.Infra.Orm;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.WebApi.ViewModels.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApi.ViewModels.ModuloContato;
using eAgenda.Aplicacao.ModuloContato;
using eAgenda.Infra.Orm.ModuloContato;

namespace eAgenda.WebApi.Controllers
{
    [ApiController]
    [Route("api/compromissos")]
    public class CompromissoController : ControllerBase
    {
        private ServicoCompromisso servicoCompromisso;
        private ServicoContato servicoContato;
        private IMapper mapeador;

        public CompromissoController(ServicoCompromisso servicoCompromisso, IMapper mapeador)
        {
            this.mapeador = mapeador;
            this.servicoCompromisso = servicoCompromisso;

            IConfiguration configuracao = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json")
              .Build();

            var connectionString = configuracao.GetConnectionString("SqlServer");

            var builder = new DbContextOptionsBuilder<eAgendaDbContext>();

            builder.UseSqlServer(connectionString);

            var contextoPersistencia = new eAgendaDbContext(builder.Options);

            var repositorioCompromisso = new RepositorioCompromissoOrm(contextoPersistencia);

            servicoCompromisso = new ServicoCompromisso(repositorioCompromisso, contextoPersistencia);

            var repositorioContato = new RepositorioContatoOrm(contextoPersistencia);

            servicoContato = new ServicoContato(repositorioContato, contextoPersistencia);
        }

        [HttpPost]
        public string Inserir(InserirCompromissoViewModel compromissoViewModel)
        {
            var contato = servicoContato.SelecionarPorId(compromissoViewModel.ContatoId).Value;

            var compromisso = new Compromisso(compromissoViewModel.Assunto, compromissoViewModel.Local, compromissoViewModel.Link,
                compromissoViewModel.Data, TimeSpan.Parse(compromissoViewModel.HoraInicio), TimeSpan.Parse(compromissoViewModel.HoraTermino), contato);

            var resultado = servicoCompromisso.Inserir(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso inserido com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpGet]
        public List<ListarCompromissoViewModel> SeleciontarTodos()
        {
            var compromissos = servicoCompromisso.SelecionarTodos().Value;

            var compromissosViewModel = new List<ListarCompromissoViewModel>();

            foreach (var compromisso in compromissos)
            {
                var compromissoViewModel = new ListarCompromissoViewModel
                {
                    Id = compromisso.Id,
                    Assunto = compromisso.Assunto,
                    Data = compromisso.Data,
                    HoraInicio = compromisso.HoraInicio.ToString(@"hh\:mm\:ss"),
                    HoraTermino = compromisso.HoraTermino.ToString(@"hh\:mm\:ss"),
                    ContatoId = compromisso.Contato.Id,
                };

                compromissosViewModel.Add(compromissoViewModel);
            }

            return compromissosViewModel;
        }

        [HttpGet("visualizacao-completa/{id}")]
        public VisualizarCompromissoViewModel SeleciontarPorId(string id)
        {
            var compromisso = servicoCompromisso.SelecionarPorId(Guid.Parse(id)).Value;

            var compromissoViewModel = new VisualizarCompromissoViewModel
            {
                Id = compromisso.Id,
                Assunto = compromisso.Assunto,
                Data = compromisso.Data,
                HoraInicio = compromisso.HoraInicio.ToString(@"hh\:mm\:ss"),
                HoraTermino = compromisso.HoraTermino.ToString(@"hh\:mm\:ss"),
                Local = compromisso.Local,
                TipoLocal = compromisso.TipoLocal,
                Link = compromisso.Link,
                ContatoId = compromisso.Contato.Id
            };

            return compromissoViewModel;
        }

        [HttpPut("{id}")]
        public string Editar(string id, EditarCompromissoViewModel compromissoViewModel)
        {
            var compromisso = servicoCompromisso.SelecionarPorId(Guid.Parse(id)).Value;

            compromisso.Assunto = compromissoViewModel.Assunto;
            compromisso.Local = compromissoViewModel.Local;
            compromisso.TipoLocal = compromissoViewModel.TipoLocal;
            compromisso.Link = compromissoViewModel.Link;
            compromisso.Data = compromissoViewModel.Data;
            compromisso.HoraInicio = TimeSpan.Parse(compromissoViewModel.HoraInicio);
            compromisso.HoraTermino = TimeSpan.Parse(compromissoViewModel.HoraTermino);
            compromisso.Contato = servicoContato.SelecionarPorId(compromissoViewModel.ContatoId).Value;

        var resultado = servicoCompromisso.Editar(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso editado com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }

        [HttpDelete("{id}")]
        public string Excluir(string id)
        {
            var resultadoBusca = servicoCompromisso.SelecionarPorId(Guid.Parse(id));

            if (resultadoBusca.IsFailed)

            {
                string[] errosNaBusca = resultadoBusca.Errors.Select(x => x.Message).ToArray();

                return string.Join("\r\n", errosNaBusca);
            }

            var compromisso = resultadoBusca.Value;

            var resultado = servicoCompromisso.Excluir(compromisso);

            if (resultado.IsSuccess)
                return "Compromisso Excluído com sucesso";

            string[] erros = resultado.Errors.Select(x => x.Message).ToArray();

            return string.Join("\r\n", erros);
        }
    }
}
