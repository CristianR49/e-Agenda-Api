﻿using eAgenda.Dominio.ModuloCompromisso;
using eAgenda.Dominio.ModuloContato;
using eAgenda.WebApi.ViewModels.ModuloContato;

namespace eAgenda.WebApi.ViewModels.ModuloCompromisso
{
    public class EditarCompromissoViewModel
    {
        public string Assunto { get; set; }
        public string Local { get; set; }
        public TipoLocalizacaoCompromissoEnum TipoLocal { get; set; }
        public string Link { get; set; }
        public DateTime Data { get; set; }
        public string HoraInicio { get; set; }
        public string HoraTermino { get; set; }
        public Guid ContatoId { get; set; }
    }
}