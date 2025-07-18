using DomainModel.Interfaces;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DomainModel;

namespace DAL.ProjectRepo
{
    public class ProveedorRepo : IProveedorRepository
    {
        public void Add(DomainModel.Proveedor entity)
        {
            throw new NotImplementedException();
        }

        public void Delete(DomainModel.Proveedor entity)
        {
            throw new NotImplementedException();
        }

        public List<DomainModel.Proveedor> GetAll()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["GestorCMBConnection"].ConnectionString;
            SqlConnection connection = new SqlConnection(connectionString);
            List<DomainModel.Proveedor> proveedores = new List<DomainModel.Proveedor>();

            try
            {
                using (connection)
                {
                    connection.Open();
                    string query = "SELECT * FROM Proveedor";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                DomainModel.Proveedor proveedor = new DomainModel.Proveedor();
                                proveedor.IdProveedor = Guid.Parse(reader["IdProveedor"].ToString());
                                proveedor.Descripcion = reader["Descripcion"].ToString();
                                proveedor.Telefono = int.Parse(reader["Telefono"].ToString());
                                proveedor.IsActive = bool.Parse(reader["IsActive"].ToString());
                                proveedores.Add(proveedor);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return proveedores;
        }

        public DomainModel.Proveedor GetById(Guid id)
        {
            throw new NotImplementedException();
        }

        public void Update(DomainModel.Proveedor entity)
        {
            throw new NotImplementedException();
        }
    }
}
