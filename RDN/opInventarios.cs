using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using RDN.Models;

namespace RDN
{
    public class opInventarios
    {

        public String sLastError = String.Empty;
        public String mensaje = String.Empty;
    
        public Boolean Alta(datosInventarios datos)
        {
            Boolean bAllOk = false;
            try
            {
                using (SqlConnection conn = new SqlConnection($"Server={Conexion.servidor}; Database = Inventarios; User Id = {Conexion.usuario}; Password = {Conexion.password}"))
                {
                    conn.Open();
                    SqlTransaction transaccion = conn.BeginTransaction();
                    try
                    {
                        DataTable dtExisteSaldo = new DataTable();
                        SqlCommand cmdExiste = new SqlCommand();

                        DataTable dataTable2= new DataTable();
                        dataTable2.Columns.Add("Folio");
                        dataTable2.Columns.Add("Sucursal");
                        dataTable2.Columns.Add("ProductoID");
                        dataTable2.Columns.Add("Precio");
                        dataTable2.Columns.Add("Cantidad");

                        for (int i=0;i<datos.NumRow();i++) 
                        {
                            String sProductoId = String.Empty;
                            String sDescripcion = String.Empty;
                            Double dPrecio = 0;
                            Double dCantidad = 0;
                            
                            datos.GetRow(i, ref sProductoId, ref dPrecio, ref dCantidad);

                            cmdExiste.Connection = conn;
                            cmdExiste.Transaction = transaccion;
                            cmdExiste.CommandText = $"select count(*) from Saldos where ProductoID = '{sProductoId}' and Sucursal = '{datos.sSucursal}'";

                            dataTable2.Rows.Add(datos.nFolio,datos.sSucursal,sProductoId, dPrecio, dCantidad);
                        }

                        int nFilas = datos.NumRow();

                        //{datos.nFolio}, '{datos.sSucursal}', '{datos.dtFecha.ToString("yyyy-MM-dd HH:mm:ss.fff")}', {datos.dTotal}, '{datos.sTipoMovimiento}

                        SqlCommand cmd2 = new SqlCommand($"sp_Alta", conn);
                        cmd2.CommandType = CommandType.StoredProcedure;
                        cmd2.Transaction= transaccion;
                        cmd2.Parameters.Add("@Folio", SqlDbType.Int);
                        cmd2.Parameters.Add("@Sucursal", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@Fecha", SqlDbType.DateTime);
                        cmd2.Parameters.Add("@Total", SqlDbType.Float);
                        cmd2.Parameters.Add("@TipoMovimiento", SqlDbType.VarChar);
                        cmd2.Parameters.Add("@nRegistros", SqlDbType.Int);
                        cmd2.Parameters.Add("@INFO_ARRAY", SqlDbType.Structured);

                        cmd2.Parameters["@Folio"].Value = datos.nFolio;
                        cmd2.Parameters["@Sucursal"].Value = datos.sSucursal;
                        cmd2.Parameters["@Fecha"].Value = datos.dtFecha.ToString("yyyy-MM-dd HH:mm:ss.fff");
                        cmd2.Parameters["@Total"].Value = datos.dTotal;
                        cmd2.Parameters["@TipoMovimiento"].Value = datos.sTipoMovimiento;
                        cmd2.Parameters["@nRegistros"].Value = nFilas;
                        cmd2.Parameters["@INFO_ARRAY"].Value = dataTable2;
                        
                        dtExisteSaldo.Load(cmdExiste.ExecuteReader());

                        if (Int32.Parse(dtExisteSaldo.Rows[0][0].ToString()) > 0)
                        {

                            cmd2.ExecuteNonQuery();
                            bAllOk = true;
                        }




                        transaccion.Commit();
                        conn.Close();
                    }
                    catch (Exception error)
                    {
                        bAllOk = false;
                        sLastError = error.Message;
                        
                        transaccion.Rollback();
                    }
                }
            }
            catch (Exception ex)
            {
                sLastError = ex.Message;
                bAllOk = false;

            }
            return bAllOk;
        }
        //LISTO
        public Boolean Baja(String nFolio, String sSucursal, String sProductoID)
        {
            Boolean bAllOk = true;
            using (SqlConnection conn = new SqlConnection($"Server={Conexion.servidor}; Database = Inventarios; User Id = {Conexion.usuario}; Password = {Conexion.password}"))
            {
                conn.Open();
                SqlTransaction transaccion = conn.BeginTransaction();
                try
                {
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.Transaction = transaccion;
                    cmd.CommandText = $"execute sp_Baja {nFolio}, '{sSucursal}', '{sProductoID}'";
                    cmd.ExecuteNonQuery();
                    transaccion.Commit();
                }
                catch (Exception error)
                {
                    transaccion.Rollback();                    
                    bAllOk = false;
                    sLastError = error.Message;
                }
                return bAllOk;
            }
        }

        public Int32 ExisteFolio(Int32 folio)
        {
            InventariosEntities3 oInvFolio = new InventariosEntities3();
            var consulta = (from range in oInvFolio.Inventarios
                            where range.Folio == folio
                            select new
                            {
                                range.Folio,
                            }).ToList().Count();
            return consulta;
        }

        public DataTable FiltrarPorDescripcion(String sDescripcion)
        {
            DataTable dt = new DataTable();
            InventariosEntities3 oInvEnt = new InventariosEntities3();
            var consulta = (from range in oInvEnt.Productos
                            where range.Descripcion.Contains(sDescripcion)
                            select new
                            {
                                range.ProductoID,
                                range.Descripcion,
                                range.PrecioCompra,
                                range.PrecioVenta,
                            }).ToList();

            dt.Columns.Add("ProductoID");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("PrecioCompra");
            dt.Columns.Add("PrecioVenta");

            foreach (var item in consulta)
            {
                DataRow row = dt.NewRow();
                row["ProductoID"] = item.ProductoID;
                row["Descripcion"] = item.Descripcion;
                row["PrecioCompra"] = item.PrecioCompra;
                row["PrecioVenta"] = item.PrecioVenta;
                dt.Rows.Add(row);
            }

            return dt;
        }

        public Int32 ExisteProducto(String sProducto)
        {
            Int32 o = 0; 
            DataTable dt = new DataTable();
            
           //  var hola =o1.Productos.ToList();
            InventariosEntities3 oInvEnt = new InventariosEntities3();
            oInvEnt.Database.Connection.Open();

            try
            {


                o = (from range in oInvEnt.Productos
                     where range.ProductoID.Contains(sProducto)
                     select new
                     {
                         range.ProductoID,
                         range.Descripcion,
                         range.PrecioCompra,
                         range.PrecioVenta,
                     }).ToList().Count;

              
            }
            catch (System.Data.Entity.Core.EntityException ex ) 
            {
                sLastError=ex.Message;
            }
            return o;
        }

        public DataTable FiltrarPorClave(String Clave)  
        {
            DataTable dt = new DataTable();
            InventariosEntities3 oInvEnt = new InventariosEntities3();
            var consulta = (from range in oInvEnt.Productos
                            where range.ProductoID.Contains(Clave)
                            select new
                            {
                                range.ProductoID,
                                range.Descripcion,
                                range.PrecioCompra,
                                range.PrecioVenta,
                            }).ToList();

            dt.Columns.Add("ProductoID");
            dt.Columns.Add("Descripcion");
            dt.Columns.Add("PrecioCompra");
            dt.Columns.Add("PrecioVenta");

            foreach (var item in consulta)
            {
                DataRow row = dt.NewRow();
                row["ProductoID"] = item.ProductoID;
                row["Descripcion"] = item.Descripcion;
                row["PrecioCompra"] = item.PrecioCompra;
                row["PrecioVenta"] = item.PrecioVenta;
                dt.Rows.Add(row);
            }

            return dt;
        }

        public DataTable RellenarTextBox(String ProductoID)
        {
            DataTable data = new DataTable();

            using (SqlConnection conn = new SqlConnection($"Server={Conexion.servidor}; Database=Inventarios; User Id = {Conexion.usuario}; Password = {Conexion.password}"))
            {
                try
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = $"select ProductoID, Descripcion, PrecioCompra, PrecioVenta from Productos where ProductoID = '{ProductoID}'";
                    data.Load(cmd.ExecuteReader());
                }
                catch(Exception eerr)
                {
                    sLastError= eerr.Message;
                }

            }
            return data;

        }
        // LISTO
        public Boolean AgregarProducto(String ProductoID, String Descripcion, String PreciCompra, String PrecioVenta)
        {
            Boolean bALLOk = true;  
            try
            {
                using (SqlConnection conn = new SqlConnection($"Server={Conexion.servidor}; Database=Inventarios; User Id = {Conexion.usuario}; Password = {Conexion.password}"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();
                    
                    cmd.Connection = conn;
                  //  cmd.CommandText = $"execute sp_AgregarProducto {ProductoID}, {Descripcion}, {PreciCompra}, '{PrecioVenta}' ";
                    cmd.CommandText = $"execute sp_AgregarProducto '{ProductoID}', '{Descripcion}', '{PreciCompra}','{PrecioVenta}'";
                    cmd.ExecuteNonQuery();
                }

                }catch(Exception error)
            {

                bALLOk = false;
                sLastError = error.Message;
            }
            return bALLOk;

        }
        //PENDIENTE
        public Boolean EliminarSaldo(String sProductoID, String sSucursal)
        {
            Boolean bALLOk = true;
            try
            {
                using (SqlConnection conn = new SqlConnection($"Server={Conexion.servidor}; Database=Inventarios; User Id = {Conexion.usuario}; Password = {Conexion.password}"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = $"delete from Saldos where  ProductoID='{sProductoID}' and Sucursal = {sSucursal}";
                    cmd.ExecuteNonQuery();

                }

            }
            catch (Exception error)
            {

                bALLOk = false;
                sLastError = error.Message;
            }
            return bALLOk;


        }
        //PENDIENTE
        public Boolean EliminarProducto(String ProductoID)
        {
            Boolean bALLOk = true;
            try
            {
                using (SqlConnection conn = new SqlConnection($"Server={Conexion.servidor}; Database=Inventarios; User Id = {Conexion.usuario}; Password = {Conexion.password}"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = $"delete from Productos where ProductoID = '{ProductoID}'";
                    cmd.ExecuteNonQuery();

                }

            }
            catch (Exception error)
            {

                bALLOk = false;
                sLastError = error.Message;
            }
            return bALLOk;

        }
        //LISTO
        public Int32 ObtenerMax()
        {
            int Max = 0;
            try
            {
                DataTable dt = new DataTable();
                using (SqlConnection conn = new SqlConnection($"Server={Conexion.servidor}; Database=Inventarios; User Id = {Conexion.usuario}; Password = {Conexion.password}"))
                {
                    conn.Open();
                    SqlCommand cmd = new SqlCommand();

                    cmd.Connection = conn;
                    cmd.CommandText = $"execute sp_ObtenerMaximo";
                    dt.Load(cmd.ExecuteReader());
                    Max= Convert.ToInt32(dt.Rows[0][0].ToString());
                    conn.Close();
                }

            }
            catch (Exception error)
            {              
                sLastError = error.Message;
            }
            return Max;
        }

        public Boolean ValidarCampos(String sProducto, String sPrecioVenta, String sPrecioCompra, String sDescripcion)
        {
            Boolean bALLOk = false;
            if (sProducto == "" || sPrecioVenta == "" || sPrecioCompra == "" || sDescripcion == "")
            {
                bALLOk = true;
            }
            return bALLOk;
        }


    }
}