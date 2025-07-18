using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ProjectRepo
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly GestorCMBEntities _context;

        public ProyectoRepository(GestorCMBEntities context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }
        public void Add(DomainModel.Proyecto entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DomainModel.Proyecto entity)
        {
            throw new NotImplementedException();
        }

        public List<DomainModel.Proyecto> GetAll()
        {
            var proyectos = _context.Proyecto
                                    .AsNoTracking()
                                    .ToList();

            return proyectos.Select(e => new DomainModel.Proyecto
            {
                IdProyecto = e.idProyecto,
                IdCliente = e.Cliente?.idCliente ?? Guid.Empty,
                Descripcion = e.descripcion,
                Estado = Enum.TryParse<DomainModel.EnumEstado>(e.estado.Replace(" ", ""), true, out var estado)
                    ? estado
                    : throw new Exception($"Estado desconocido: {e.estado}"),
                Ubicacion = e.ubicacion,
                FechaInicio = e.fechaInicio,
                FechaFin = e.fechaFin,
                Cliente = e.Cliente != null
                    ? new DomainModel.Cliente
                    {
                        IdCliente = e.Cliente.idCliente,
                        NombreContacto = e.Cliente.nombreContacto,
                        RazonSocial = e.Cliente.razonSocial,
                        Mail = e.Cliente.mail,
                        Telefono = e.Cliente.telefono,
                        IsActive = e.Cliente.isActive
                    }
                    : null,

                InformesCompra = e.Informe_compra?.Select(ic => new DomainModel.InformeDeCompra
                {
                    IdInformeCompra = ic.idInformeCompra, // Corregido el nombre de la propiedad
                    IdProyecto = ic.idProyecto,
                    FechaRealizacion = ic.fechaRealizacion, // Ajustado con el modelo correcto
                    Proyecto = null // Evitamos referencias circulares
                }).ToList() ?? new List<DomainModel.InformeDeCompra>(),

                InformesMontos = e.Informe_monto?.Select(im => new DomainModel.InformeMonto
                {
                    IdInformeMonto = im.idInformeMonto, // Corregido el nombre de la propiedad
                    IdProyecto = im.Proyecto?.idProyecto ?? Guid.Empty,
                    TotalMateriales = (float)im.totalMateriales, // Ajustado con el modelo correcto
                    TotalEmpleados = (float)im.totalEmpleados,
                    MontoTotal = (float)im.montoTotal,
                    Proyecto = null // Evitamos referencias circulares
                }).ToList() ?? new List<DomainModel.InformeMonto>(),

                DetallesEmpleados = e.Detalle_proyecto_empleado?.Select(de => new DomainModel.DetalleProyectoEmpleado
                {
                    IdDetalleProyectoEmpleado = de.idDetalleEmpleado,
                    IdProyecto = de.idProyecto,
                    IdEmpleado = de.idEmpleado,
                    FechaIngresoEmpleado = de.fechaIngresoEmpleado,
                    ValorGanancia = (float)de.valorGanancia,
                    Proyecto = null // Evitamos referencias circulares
                }).ToList() ?? new List<DomainModel.DetalleProyectoEmpleado>(),

                DetallesMateriales = e.Detalle_proyecto_material?.Select(de => new DomainModel.DetalleProyectoMaterial
                {
                    IdDetalleMaterial = de.idDetalleMaterial,
                    IdProyecto = de.idProyecto,
                    IdMaterial = de.idMaterial,
                    Cantidad = de.cantidad,
                    ValorGanancia = (float)de.valorGanancia,
                    Proyecto = null // Evitamos referencias circulares
                }).ToList() ?? new List<DomainModel.DetalleProyectoMaterial>()

            }).ToList();
        }



        public DomainModel.Proyecto GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(DomainModel.Proyecto entity)
        {
            throw new NotImplementedException();
        }
    }
}
