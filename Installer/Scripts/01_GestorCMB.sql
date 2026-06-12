-- Script generado para el instalador de GestorCMB
-- Base: GestorCMB
IF DB_ID(N'GestorCMB') IS NULL CREATE DATABASE [GestorCMB];
GO
USE [GestorCMB];
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Cliente]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Cliente](
	[idCliente] [uniqueidentifier] NOT NULL,
	[nombreContacto] [varchar](50) NOT NULL,
	[razonSocial] [varchar](50) NOT NULL,
	[mail] [varchar](50) NOT NULL,
	[telefono] [int] NOT NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_Cliente_1] PRIMARY KEY CLUSTERED 
(
	[idCliente] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proveedor]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Proveedor](
	[idProveedor] [uniqueidentifier] NOT NULL,
	[descripcion] [varchar](max) NOT NULL,
	[telefono] [int] NOT NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_Proveedor] PRIMARY KEY CLUSTERED 
(
	[idProveedor] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Empleado]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Empleado](
	[idEmpleado] [uniqueidentifier] NOT NULL,
	[nombre] [varchar](50) NOT NULL,
	[apellido] [varchar](50) NOT NULL,
	[nroDocumento] [int] NOT NULL,
	[sueldo] [float] NOT NULL,
	[cantidadProyectosActivos] [int] NOT NULL,
	[isActive] [bit] NOT NULL,
 CONSTRAINT [PK_Empleado] PRIMARY KEY CLUSTERED 
(
	[idEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Material]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Material](
	[idMaterial] [uniqueidentifier] NOT NULL,
	[descripcionArticulo] [varchar](max) NOT NULL,
	[tipoMaterial] [varchar](50) NOT NULL,
	[tipoUnidad] [varchar](50) NOT NULL,
	[costoPorUnidad] [float] NOT NULL,
	[idProveedor] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Material] PRIMARY KEY CLUSTERED 
(
	[idMaterial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Proyecto]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Proyecto](
	[idProyecto] [uniqueidentifier] NOT NULL,
	[idCliente] [uniqueidentifier] NOT NULL,
	[descripcion] [varchar](50) NOT NULL,
	[estado] [varchar](50) NOT NULL,
	[fechaInicio] [date] NOT NULL,
	[fechaFin] [date] NOT NULL,
	[ubicacion] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Proyecto] PRIMARY KEY CLUSTERED 
(
	[idProyecto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Material_faltante]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Material_faltante](
	[idMaterialFaltante] [uniqueidentifier] NOT NULL,
	[descripcionArticuloFaltante] [varchar](max) NOT NULL,
	[tipoMaterialFaltante] [varchar](max) NOT NULL,
	[tipoUnidadMaterialFaltante] [varchar](max) NOT NULL,
	[idProyecto] [uniqueidentifier] NOT NULL,
	[cantidadFaltante] [int] NOT NULL,
 CONSTRAINT [PK_Material_faltante] PRIMARY KEY CLUSTERED 
(
	[idMaterialFaltante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Inventario]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Inventario](
	[idMaterialInventario] [uniqueidentifier] NOT NULL,
	[cantidad] [int] NOT NULL,
	[idMaterial] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Inventario] PRIMARY KEY CLUSTERED 
(
	[idMaterialInventario] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Informe_monto]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Informe_monto](
	[idInformeMonto] [uniqueidentifier] NOT NULL,
	[idProyecto] [uniqueidentifier] NOT NULL,
	[totalMateriales] [float] NOT NULL,
	[totalEmpleados] [float] NOT NULL,
	[montoTotal] [float] NOT NULL,
 CONSTRAINT [PK_Informe_monto] PRIMARY KEY CLUSTERED 
(
	[idInformeMonto] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Informe_compra]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Informe_compra](
	[idInformeCompra] [uniqueidentifier] NOT NULL,
	[idProyecto] [uniqueidentifier] NOT NULL,
	[fechaRealizacion] [date] NOT NULL,
	[estado] [varchar](20) NOT NULL,
 CONSTRAINT [PK_Informe_compra] PRIMARY KEY CLUSTERED 
(
	[idInformeCompra] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_material]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Detalle_proyecto_material](
	[idDetalleMaterial] [uniqueidentifier] NOT NULL,
	[idProyecto] [uniqueidentifier] NOT NULL,
	[idMaterial] [uniqueidentifier] NOT NULL,
	[cantidad] [int] NOT NULL,
	[valorGanancia] [float] NOT NULL,
	[fechaIngresoMaterial] [date] NOT NULL,
 CONSTRAINT [PK_Detalle_proyecto_material] PRIMARY KEY CLUSTERED 
(
	[idDetalleMaterial] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_empleado]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Detalle_proyecto_empleado](
	[idDetalleEmpleado] [uniqueidentifier] NOT NULL,
	[idProyecto] [uniqueidentifier] NOT NULL,
	[idEmpleado] [uniqueidentifier] NOT NULL,
	[fechaIngresoEmpleado] [date] NOT NULL,
	[valorGanancia] [float] NOT NULL,
	[estado] [varchar](max) NOT NULL,
 CONSTRAINT [PK_Detalle_proyecto_empleado] PRIMARY KEY CLUSTERED 
(
	[idDetalleEmpleado] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
END
GO
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[Detalle_informe_material_faltante]') AND type in (N'U'))
BEGIN
CREATE TABLE [dbo].[Detalle_informe_material_faltante](
	[idDetalleMaterialFaltante] [uniqueidentifier] NOT NULL,
	[idInformeCompra] [uniqueidentifier] NOT NULL,
	[idMaterialFaltante] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_Detalle_informe_material_faltante] PRIMARY KEY CLUSTERED 
(
	[idDetalleMaterialFaltante] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON, OPTIMIZE_FOR_SEQUENTIAL_KEY = OFF) ON [PRIMARY]
) ON [PRIMARY]
END
GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Cliente_idCliente]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Cliente] ADD  CONSTRAINT [DF_Cliente_idCliente]  DEFAULT (newsequentialid()) FOR [idCliente]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Proveedor_idProveedor]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Proveedor] ADD  CONSTRAINT [DF_Proveedor_idProveedor]  DEFAULT (newsequentialid()) FOR [idProveedor]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Empleado_idEmpleado]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Empleado] ADD  CONSTRAINT [DF_Empleado_idEmpleado]  DEFAULT (newsequentialid()) FOR [idEmpleado]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Material_idMaterial]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Material] ADD  CONSTRAINT [DF_Material_idMaterial]  DEFAULT (newsequentialid()) FOR [idMaterial]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Proyecto_idProyecto]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Proyecto] ADD  CONSTRAINT [DF_Proyecto_idProyecto]  DEFAULT (newsequentialid()) FOR [idProyecto]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Material_faltante_idMaterialFaltante]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Material_faltante] ADD  CONSTRAINT [DF_Material_faltante_idMaterialFaltante]  DEFAULT (newsequentialid()) FOR [idMaterialFaltante]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Inventario_idMaterialInventario]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Inventario] ADD  CONSTRAINT [DF_Inventario_idMaterialInventario]  DEFAULT (newsequentialid()) FOR [idMaterialInventario]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF__Informe_c__estad__607251E5]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Informe_compra] ADD  DEFAULT ('pendiente') FOR [estado]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Detalle_proyecto_material_idDetalleMaterial]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Detalle_proyecto_material] ADD  CONSTRAINT [DF_Detalle_proyecto_material_idDetalleMaterial]  DEFAULT (newsequentialid()) FOR [idDetalleMaterial]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Detalle_proyecto_empleado_idDetalleEmpleado]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Detalle_proyecto_empleado] ADD  CONSTRAINT [DF_Detalle_proyecto_empleado_idDetalleEmpleado]  DEFAULT (newsequentialid()) FOR [idDetalleEmpleado]
END

GO
IF NOT EXISTS (SELECT * FROM sys.objects WHERE object_id = OBJECT_ID(N'[dbo].[DF_Detalle_informe_material_faltante_idDetalleMaterialFaltante]') AND type = 'D')
BEGIN
ALTER TABLE [dbo].[Detalle_informe_material_faltante] ADD  CONSTRAINT [DF_Detalle_informe_material_faltante_idDetalleMaterialFaltante]  DEFAULT (newsequentialid()) FOR [idDetalleMaterialFaltante]
END

GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Material_Proveedor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Material]'))
ALTER TABLE [dbo].[Material]  WITH CHECK ADD  CONSTRAINT [FK_Material_Proveedor] FOREIGN KEY([idProveedor])
REFERENCES [dbo].[Proveedor] ([idProveedor])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Material_Proveedor]') AND parent_object_id = OBJECT_ID(N'[dbo].[Material]'))
ALTER TABLE [dbo].[Material] CHECK CONSTRAINT [FK_Material_Proveedor]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Proyecto_Cliente]') AND parent_object_id = OBJECT_ID(N'[dbo].[Proyecto]'))
ALTER TABLE [dbo].[Proyecto]  WITH CHECK ADD  CONSTRAINT [FK_Proyecto_Cliente] FOREIGN KEY([idCliente])
REFERENCES [dbo].[Cliente] ([idCliente])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Proyecto_Cliente]') AND parent_object_id = OBJECT_ID(N'[dbo].[Proyecto]'))
ALTER TABLE [dbo].[Proyecto] CHECK CONSTRAINT [FK_Proyecto_Cliente]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Material_faltante_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Material_faltante]'))
ALTER TABLE [dbo].[Material_faltante]  WITH CHECK ADD  CONSTRAINT [FK_Material_faltante_Proyecto] FOREIGN KEY([idProyecto])
REFERENCES [dbo].[Proyecto] ([idProyecto])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Material_faltante_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Material_faltante]'))
ALTER TABLE [dbo].[Material_faltante] CHECK CONSTRAINT [FK_Material_faltante_Proyecto]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Inventario_Material]') AND parent_object_id = OBJECT_ID(N'[dbo].[Inventario]'))
ALTER TABLE [dbo].[Inventario]  WITH CHECK ADD  CONSTRAINT [FK_Inventario_Material] FOREIGN KEY([idMaterial])
REFERENCES [dbo].[Material] ([idMaterial])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Inventario_Material]') AND parent_object_id = OBJECT_ID(N'[dbo].[Inventario]'))
ALTER TABLE [dbo].[Inventario] CHECK CONSTRAINT [FK_Inventario_Material]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Informe_monto_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Informe_monto]'))
ALTER TABLE [dbo].[Informe_monto]  WITH CHECK ADD  CONSTRAINT [FK_Informe_monto_Proyecto] FOREIGN KEY([idProyecto])
REFERENCES [dbo].[Proyecto] ([idProyecto])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Informe_monto_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Informe_monto]'))
ALTER TABLE [dbo].[Informe_monto] CHECK CONSTRAINT [FK_Informe_monto_Proyecto]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Informe_compra_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Informe_compra]'))
ALTER TABLE [dbo].[Informe_compra]  WITH CHECK ADD  CONSTRAINT [FK_Informe_compra_Proyecto] FOREIGN KEY([idProyecto])
REFERENCES [dbo].[Proyecto] ([idProyecto])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Informe_compra_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Informe_compra]'))
ALTER TABLE [dbo].[Informe_compra] CHECK CONSTRAINT [FK_Informe_compra_Proyecto]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_material_Material]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_material]'))
ALTER TABLE [dbo].[Detalle_proyecto_material]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_proyecto_material_Material] FOREIGN KEY([idMaterial])
REFERENCES [dbo].[Material] ([idMaterial])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_material_Material]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_material]'))
ALTER TABLE [dbo].[Detalle_proyecto_material] CHECK CONSTRAINT [FK_Detalle_proyecto_material_Material]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_material_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_material]'))
ALTER TABLE [dbo].[Detalle_proyecto_material]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_proyecto_material_Proyecto] FOREIGN KEY([idProyecto])
REFERENCES [dbo].[Proyecto] ([idProyecto])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_material_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_material]'))
ALTER TABLE [dbo].[Detalle_proyecto_material] CHECK CONSTRAINT [FK_Detalle_proyecto_material_Proyecto]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_empleado_Empleado]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_empleado]'))
ALTER TABLE [dbo].[Detalle_proyecto_empleado]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_proyecto_empleado_Empleado] FOREIGN KEY([idEmpleado])
REFERENCES [dbo].[Empleado] ([idEmpleado])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_empleado_Empleado]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_empleado]'))
ALTER TABLE [dbo].[Detalle_proyecto_empleado] CHECK CONSTRAINT [FK_Detalle_proyecto_empleado_Empleado]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_empleado_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_empleado]'))
ALTER TABLE [dbo].[Detalle_proyecto_empleado]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_proyecto_empleado_Proyecto] FOREIGN KEY([idProyecto])
REFERENCES [dbo].[Proyecto] ([idProyecto])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_proyecto_empleado_Proyecto]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_proyecto_empleado]'))
ALTER TABLE [dbo].[Detalle_proyecto_empleado] CHECK CONSTRAINT [FK_Detalle_proyecto_empleado_Proyecto]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_informe_material_faltante_Informe_compra]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_informe_material_faltante]'))
ALTER TABLE [dbo].[Detalle_informe_material_faltante]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_informe_material_faltante_Informe_compra] FOREIGN KEY([idInformeCompra])
REFERENCES [dbo].[Informe_compra] ([idInformeCompra])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_informe_material_faltante_Informe_compra]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_informe_material_faltante]'))
ALTER TABLE [dbo].[Detalle_informe_material_faltante] CHECK CONSTRAINT [FK_Detalle_informe_material_faltante_Informe_compra]
GO
IF NOT EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_informe_material_faltante_Material_faltante]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_informe_material_faltante]'))
ALTER TABLE [dbo].[Detalle_informe_material_faltante]  WITH CHECK ADD  CONSTRAINT [FK_Detalle_informe_material_faltante_Material_faltante] FOREIGN KEY([idMaterialFaltante])
REFERENCES [dbo].[Material_faltante] ([idMaterialFaltante])
GO
IF  EXISTS (SELECT * FROM sys.foreign_keys WHERE object_id = OBJECT_ID(N'[dbo].[FK_Detalle_informe_material_faltante_Material_faltante]') AND parent_object_id = OBJECT_ID(N'[dbo].[Detalle_informe_material_faltante]'))
ALTER TABLE [dbo].[Detalle_informe_material_faltante] CHECK CONSTRAINT [FK_Detalle_informe_material_faltante_Material_faltante]
GO
