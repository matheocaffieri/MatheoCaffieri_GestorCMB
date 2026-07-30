using BL.BL_Interfaces;
using DAL.DAL_Interfaces;
using DAL.FactoryDAL;
using DomainModel.Integridad;
using System.Collections.Generic;

namespace BL
{
    public class IntegridadBL : IIntegridadBL
    {
        private readonly IIntegridadRepository _repo;

        public IntegridadBL()
        {
            _repo = DalFactory.CreateIntegridadRepository();
        }

        public bool HayLineaBase() => _repo.HayLineaBase();

        public void RecalcularLineaBase() => _repo.RecalcularTodo();

        public List<IntegridadAnomalia> Verificar() => _repo.Verificar();

        public List<IntegridadAnomalia> VerificarOInicializar()
        {
            if (!_repo.HayLineaBase())
            {
                _repo.RecalcularTodo();
                return new List<IntegridadAnomalia>();
            }
            return _repo.Verificar();
        }
    }
}
