using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using DAL.FactoryDAL;

using DomainModel;
using DomainModel.Interfaces;

// Aliases EF (ajusta si tu namespace difiere)
using ProyectoEf = DAL.Proyecto;
using ClienteEf = DAL.Cliente;
using InfCompraEf = DAL.Informe_compra;
using InfMontoEf = DAL.Informe_monto;
using DetEmpEf = DAL.Detalle_proyecto_empleado;
using DetMatEf = DAL.Detalle_proyecto_material;

namespace DAL.ProjectRepo
{
    public class ProyectoRepository : IProyectoRepository
    {
        private readonly IUnitOfWork _uow;
        private readonly GestorCMBEntities _context;
        private readonly DbSet<ProyectoEf> _set;

        public ProyectoRepository(IUnitOfWork uow)
        {
            if (uow == null) throw new ArgumentNullException(nameof(uow));
            _uow = uow;


            _context = uow.Context;
            _set = _context.Set<ProyectoEf>();
        }

        // ===== CRUD básicos (sin grafo) =====

        private static void MapToEf(DomainModel.Proyecto src, ProyectoEf dst)
        {
            dst.idProyecto = src.IdProyecto;
            dst.descripcion = src.Descripcion;
            dst.estado = src.Estado.ToString(); // guardás el nombre del enum; si en DB usás otro formato, normalizá acá
            dst.ubicacion = src.Ubicacion;
            dst.fechaInicio = src.FechaInicio;
            dst.fechaFin = src.FechaFin;
            dst.idCliente = src.IdCliente; // FK
        }

        public void Add(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = new ProyectoEf();
            MapToEf(entity, ef);
            _set.Add(ef);
        }

        public void Update(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = _set.Find(entity.IdProyecto);
            if (ef == null) throw new InvalidOperationException("Proyecto no encontrado.");

            MapToEf(entity, ef);
            _context.Entry(ef).State = EntityState.Modified;
        }

        public void Delete(DomainModel.Proyecto entity)
        {
            if (entity == null) throw new ArgumentNullException(nameof(entity));
            var ef = _set.Find(entity.IdProyecto);
            if (ef == null) return;

            _set.Remove(ef);
        }

        // ===== Lecturas =====

        public List<DomainModel.Proyecto> GetAll()
        {
            // Paso 1: proyección SQL-friendly (sin Enum.TryParse), materializamos a memoria.
            var rows = _set.AsNoTracking()
                           .Select(e => new
                           {
                               e.idProyecto,
                               e.descripcion,
                               e.estado,          // string
                               e.ubicacion,
                               e.fechaInicio,
                               e.fechaFin,
                               IdCliente = (Guid?)e.idCliente,
                               Cliente = new
                               {
                                   e.Cliente.idCliente,
                                   e.Cliente.nombreContacto,
                                   e.Cliente.razonSocial,
                                   e.Cliente.mail,
                                   e.Cliente.telefono,
                                   e.Cliente.isActive
                               },
                               InformesCompra = e.Informe_compra.Select(ic => new
                               {
                                   ic.idInformeCompra,
                                   ic.idProyecto,
                                   ic.fechaRealizacion
                               }).ToList(),
                               InformesMontos = e.Informe_monto.Select(im => new
                               {
                                   im.idInformeMonto,
                                   ProyId = (Guid?)im.idProyecto,
                                   im.totalMateriales,
                                   im.totalEmpleados,
                                   im.montoTotal
                               }).ToList(),
                               DetallesEmpleados = e.Detalle_proyecto_empleado.Select(de => new
                               {
                                   de.idDetalleEmpleado,
                                   de.idProyecto,
                                   de.idEmpleado,
                                   de.fechaIngresoEmpleado,
                                   de.valorGanancia
                               }).ToList(),
                               DetallesMateriales = e.Detalle_proyecto_material.Select(dm => new
                               {
                                   dm.idDetalleMaterial,
                                   dm.idProyecto,
                                   dm.idMaterial,
                                   dm.cantidad,
                                   dm.valorGanancia
                               }).ToList()
                           })
                           .ToList();

            // Paso 2: mapping en memoria, incluyendo el enum.
            var list = new List<DomainModel.Proyecto>(rows.Count);
            foreach (var r in rows)
            {
                var proj = new DomainModel.Proyecto
                {
                    IdProyecto = r.idProyecto,
                    IdCliente = r.IdCliente ?? Guid.Empty,
                    Descripcion = r.descripcion,
                    Estado = ParseEstado(r.estado),
                    Ubicacion = r.ubicacion,
                    FechaInicio = r.fechaInicio,
                    FechaFin = r.fechaFin,
                    Cliente = (r.Cliente.idCliente != Guid.Empty)
                                  ? new DomainModel.Cliente
                                  {
                                      IdCliente = r.Cliente.idCliente,
                                      NombreContacto = r.Cliente.nombreContacto,
                                      RazonSocial = r.Cliente.razonSocial,
                                      Mail = r.Cliente.mail,
                                      Telefono = r.Cliente.telefono,
                                      IsActive = r.Cliente.isActive
                                  }
                                  : null,
                    InformesCompra = r.InformesCompra.Select(ic => new DomainModel.InformeDeCompra
                    {
                        IdInformeCompra = ic.idInformeCompra,
                        IdProyecto = ic.idProyecto,
                        FechaRealizacion = ic.fechaRealizacion,
                        Proyecto = null
                    }).ToList(),
                    InformesMontos = r.InformesMontos.Select(im => new DomainModel.InformeMonto
                    {
                        IdInformeMonto = im.idInformeMonto,
                        IdProyecto = im.ProyId ?? Guid.Empty,
                        TotalMateriales = (float)im.totalMateriales,
                        TotalEmpleados = (float)im.totalEmpleados,
                        MontoTotal = (float)im.montoTotal,
                        Proyecto = null
                    }).ToList(),
                    DetallesEmpleados = r.DetallesEmpleados.Select(de => new DomainModel.DetalleProyectoEmpleado
                    {
                        IdDetalleProyectoEmpleado = de.idDetalleEmpleado,
                        IdProyecto = de.idProyecto,
                        IdEmpleado = de.idEmpleado,
                        FechaIngresoEmpleado = de.fechaIngresoEmpleado,
                        ValorGanancia = (float)de.valorGanancia,
                        Proyecto = null
                    }).ToList(),
                    DetallesMateriales = r.DetallesMateriales.Select(dm => new DomainModel.DetalleProyectoMaterial
                    {
                        IdDetalleMaterial = dm.idDetalleMaterial,
                        IdProyecto = dm.idProyecto,
                        IdMaterial = dm.idMaterial,
                        Cantidad = dm.cantidad,
                        ValorGanancia = (float)dm.valorGanancia,
                        Proyecto = null
                    }).ToList()
                };

                list.Add(proj);
            }

            return list;
        }

        public DomainModel.Proyecto GetById(Guid id)
        {
            // Idem GetAll pero filtrado por id
            var r = _set.AsNoTracking()
                        .Where(e => e.idProyecto == id)
                        .Select(e => new
                        {
                            e.idProyecto,
                            e.descripcion,
                            e.estado,
                            e.ubicacion,
                            e.fechaInicio,
                            e.fechaFin,
                            IdCliente = (Guid?)e.idCliente,
                            Cliente = new
                            {
                                e.Cliente.idCliente,
                                e.Cliente.nombreContacto,
                                e.Cliente.razonSocial,
                                e.Cliente.mail,
                                e.Cliente.telefono,
                                e.Cliente.isActive
                            },
                            InformesCompra = e.Informe_compra.Select(ic => new
                            {
                                ic.idInformeCompra,
                                ic.idProyecto,
                                ic.fechaRealizacion
                            }).ToList(),
                            InformesMontos = e.Informe_monto.Select(im => new
                            {
                                im.idInformeMonto,
                                ProyId = (Guid?)im.idProyecto,
                                im.totalMateriales,
                                im.totalEmpleados,
                                im.montoTotal
                            }).ToList(),
                            DetallesEmpleados = e.Detalle_proyecto_empleado.Select(de => new
                            {
                                de.idDetalleEmpleado,
                                de.idProyecto,
                                de.idEmpleado,
                                de.fechaIngresoEmpleado,
                                de.valorGanancia
                            }).ToList(),
                            DetallesMateriales = e.Detalle_proyecto_material.Select(dm => new
                            {
                                dm.idDetalleMaterial,
                                dm.idProyecto,
                                dm.idMaterial,
                                dm.cantidad,
                                dm.valorGanancia
                            }).ToList()
                        })
                        .FirstOrDefault();

            if (r == null) return null;

            return new DomainModel.Proyecto
            {
                IdProyecto = r.idProyecto,
                IdCliente = r.IdCliente ?? Guid.Empty,
                Descripcion = r.descripcion,
                Estado = ParseEstado(r.estado),
                Ubicacion = r.ubicacion,
                FechaInicio = r.fechaInicio,
                FechaFin = r.fechaFin,
                Cliente = (r.Cliente.idCliente != Guid.Empty)
                              ? new DomainModel.Cliente
                              {
                                  IdCliente = r.Cliente.idCliente,
                                  NombreContacto = r.Cliente.nombreContacto,
                                  RazonSocial = r.Cliente.razonSocial,
                                  Mail = r.Cliente.mail,
                                  Telefono = r.Cliente.telefono,
                                  IsActive = r.Cliente.isActive
                              }
                              : null,
                InformesCompra = r.InformesCompra.Select(ic => new DomainModel.InformeDeCompra
                {
                    IdInformeCompra = ic.idInformeCompra,
                    IdProyecto = ic.idProyecto,
                    FechaRealizacion = ic.fechaRealizacion,
                    Proyecto = null
                }).ToList(),
                InformesMontos = r.InformesMontos.Select(im => new DomainModel.InformeMonto
                {
                    IdInformeMonto = im.idInformeMonto,
                    IdProyecto = im.ProyId ?? Guid.Empty,
                    TotalMateriales = (float)im.totalMateriales,
                    TotalEmpleados = (float)im.totalEmpleados,
                    MontoTotal = (float)im.montoTotal,
                    Proyecto = null
                }).ToList(),
                DetallesEmpleados = r.DetallesEmpleados.Select(de => new DomainModel.DetalleProyectoEmpleado
                {
                    IdDetalleProyectoEmpleado = de.idDetalleEmpleado,
                    IdProyecto = de.idProyecto,
                    IdEmpleado = de.idEmpleado,
                    FechaIngresoEmpleado = de.fechaIngresoEmpleado,
                    ValorGanancia = (float)de.valorGanancia,
                    Proyecto = null
                }).ToList(),
                DetallesMateriales = r.DetallesMateriales.Select(dm => new DomainModel.DetalleProyectoMaterial
                {
                    IdDetalleMaterial = dm.idDetalleMaterial,
                    IdProyecto = dm.idProyecto,
                    IdMaterial = dm.idMaterial,
                    Cantidad = dm.cantidad,
                    ValorGanancia = (float)dm.valorGanancia,
                    Proyecto = null
                }).ToList()
            };
        }

        // ===== Utils =====
        private static DomainModel.EnumEstado ParseEstado(string estadoDb)
        {
            var norm = (estadoDb ?? "").Replace(" ", "").Trim();
            DomainModel.EnumEstado e;
            if (!Enum.TryParse(norm, ignoreCase: true, result: out e))
                throw new Exception($"Estado desconocido: {estadoDb}");
            return e;
        }
    }
}
