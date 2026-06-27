using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DomainModel.Login
{
    public enum TipoPermiso
    {
        // Proyectos
        VER_PROYECTOS,
        GESTIONAR_PROYECTOS,
        // Inventario / Materiales
        VER_INVENTARIO,
        GESTIONAR_MATERIALES,
        // Empleados
        VER_EMPLEADOS,
        GESTIONAR_EMPLEADOS,
        // Clientes
        VER_CLIENTES,
        GESTIONAR_CLIENTES,
        // Proveedores
        VER_PROVEEDORES,
        GESTIONAR_PROVEEDORES,
        // Informes de compra
        VER_INFORMES_COMPRA,
        GESTIONAR_INFORMES_COMPRA,
        // Sistema
        VER_LOGS,
        CONFIGURAR_PARAMETROS,
        GESTIONAR_USUARIOS,
    }
}
