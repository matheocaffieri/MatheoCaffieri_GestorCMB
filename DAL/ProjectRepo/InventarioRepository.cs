using DomainModel;
using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;

namespace DAL.ProjectRepo
{
    public class InventarioRepository : IInventarioRepository
    {

        private readonly GestorCMBEntities _context;

        public InventarioRepository(GestorCMBEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void Add(DomainModel.Inventario entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DomainModel.Inventario entity)
        {
            throw new NotImplementedException();
        }

        public List<DomainModel.Inventario> GetAll()
        {
            return (from inv in _context.Inventario
                    join mat in _context.Material on inv.idMaterial equals mat.idMaterial
                    join prov in _context.Proveedor on mat.idProveedor equals prov.idProveedor
                    select new DomainModel.Inventario
                    {
                        IdMaterialInventario = inv.idMaterialInventario,
                        Cantidad = inv.cantidad,
                        IdMaterial = inv.idMaterial,

                        Material = new DomainModel.Material
                        {
                            IdMaterial = mat.idMaterial,
                            DescripcionArticulo = mat.descripcionArticulo,
                            TipoMaterial = mat.tipoMaterial,
                            TipoUnidad = mat.tipoUnidad,
                            CostoPorUnidad = (float)mat.costoPorUnidad,
                            IdProveedor = mat.idProveedor,

                            Proveedor = new DomainModel.Proveedor
                            {
                                IdProveedor = prov.idProveedor,
                                Descripcion = prov.descripcion,
                                Telefono = prov.telefono
                            }
                        }
                    }).ToList();
        }




        public DomainModel.Inventario GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(DomainModel.Inventario entity)
        {
            throw new NotImplementedException();
        }
    }
}
