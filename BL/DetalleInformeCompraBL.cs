using DAL.FactoryDAL;
using DomainModel;
using DomainModel.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BL.BL_Interfaces;

namespace BL
{
    public class DetalleInformeCompraBL : IDetalleInformeCompraBL
    {
        public List<MaterialFaltante> GetMaterialesFaltantesDelInforme(Guid idInformeCompra)
        {
            if (idInformeCompra == Guid.Empty) throw new AppException("err_detalle_informe_id_required");

            using (var uow = DalFactory.CreateUnitOfWork())
            {
                var detalleInfRepo = DalFactory.CreateDetalleInformeMaterialFaltanteRepository(uow);
                var materialFaltanteRepo = DalFactory.CreateMaterialFaltanteRepository(uow);

                var ids = detalleInfRepo.GetByInforme(idInformeCompra)
                                        .Select(d => d.IdMaterialFaltante)
                                        .ToList();

                if (ids.Count == 0) return new List<MaterialFaltante>();

                return materialFaltanteRepo.GetByIds(ids);
            }
        }
    }
}
