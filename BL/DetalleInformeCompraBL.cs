using DAL;
using DomainModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class DetalleInformeCompraBL
    {
        public List<MaterialFaltante> GetMaterialesFaltantesDelInforme(Guid idInformeCompra)
        {
            if (idInformeCompra == Guid.Empty) throw new ArgumentException("idInformeCompra requerido.", nameof(idInformeCompra));

            using (var context = new GestorCMBEntities())
            {
                // Leemos directo con EF (rápido y seguro) para mostrar
                var ids = context.Detalle_informe_material_faltante
                    .Where(d => d.idInformeCompra == idInformeCompra)
                    .Select(d => d.idMaterialFaltante)
                    .ToList();

                if (ids.Count == 0) return new List<MaterialFaltante>();

                var faltantes = context.Material_faltante
                    .Where(m => ids.Contains(m.idMaterialFaltante))
                    .Select(m => new MaterialFaltante
                    {
                        IdMaterialFaltante = m.idMaterialFaltante,
                        DescripcionArticuloFaltante = m.descripcionArticuloFaltante,
                        TipoMaterialFaltante = m.tipoMaterialFaltante,
                        TipoUnidadMaterialFaltante = m.tipoUnidadMaterialFaltante,
                        IdProyecto = m.idProyecto,
                        CantidadFaltante = m.cantidadFaltante
                    })
                    .ToList();

                return faltantes;
            }
        }
    }
}
