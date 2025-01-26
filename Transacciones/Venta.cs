using RDN;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Odbc;
using System.Drawing;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transacciones_AVR_501
{
    public partial class Venta : Form
    {
        public Venta()
        {
            InitializeComponent();
        }

        public String ProductoId = String.Empty;
        public String PrecioCompra = String.Empty;
        public String PrecioVenta = String.Empty;
        public String Descripcion = String.Empty;
        public float Total = 0;
        ArrayList lista = new ArrayList();
        datosInventarios oDatosInventarios = new datosInventarios();
        opInventarios opInventarios = new opInventarios();

        private void btn_Guardar_Click(object sender, EventArgs e)
        {
            if (Conexion.AbrirConexion())
            {
                try
                {


                    if (cb_TipoMovimiento.Text.Equals("") || tb_Sucursal.Text.Equals("") || tb_Cantidad.Text.Equals("") || tb_Folio.Text.Equals(""))
                    {
                        MessageBox.Show("Asegurese de   ingresar el tipo de movimiento, la sucursal, la cantidad  y el folio");
                        return;
                    }

                    oDatosInventarios.Row(tb_Producto.Text, Convert.ToDouble(tb_Precio.Text), Convert.ToDouble(tb_Cantidad.Text));

                    oDatosInventarios.nFolio = Convert.ToInt32(tb_Folio.Text);
                    oDatosInventarios.dtFecha = dateTimePicker1.Value;
                    oDatosInventarios.sSucursal = tb_Sucursal.Text;
                    oDatosInventarios.sTipoMovimiento = cb_TipoMovimiento.Text;

                    Double Cantidad = 0;
                    Double Precio = 0;

                    if (cb_TipoMovimiento.Text.ToUpper() == "ENTRADA")
                        Precio = Convert.ToDouble(tb_Precio.Text);

                    if (cb_TipoMovimiento.Text.ToUpper() == "SALIDA")
                        Precio = Convert.ToDouble(tb_PrecioVenta.Text);

                    Cantidad = Convert.ToDouble(tb_Cantidad.Text);

                    Double Importe = Cantidad * Precio;

                    if (cb_TipoMovimiento.Text.ToUpper() == "ENTRADA")
                        dgv_Datos.Rows.Add(tb_Producto.Text, tb_Descripcion.Text, tb_Cantidad.Text, tb_Precio.Text, Importe);

                    if (cb_TipoMovimiento.Text.ToUpper() == "SALIDA")
                        dgv_Datos.Rows.Add(tb_Producto.Text, tb_Descripcion.Text, tb_Cantidad.Text, tb_PrecioVenta.Text, Importe);


                    for (int i = 0; i < dgv_Datos.Rows.Count; i++)
                    {
                        Total += Convert.ToSingle(dgv_Datos.Rows[i].Cells[4].Value);
                    }

                    tb_Total.Text = Convert.ToString(Total);
                    oDatosInventarios.dTotal = Convert.ToSingle(tb_Total.Text);
                    Total = 0;

                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void Venta_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
            {
                try
                {
                    BuscarProducto oBuscar = new BuscarProducto();
                    
                    oBuscar.ShowDialog();

                    tb_Producto.Text=oBuscar.ProductoId;
                    tb_Descripcion.Text = oBuscar.Descripcion;
                    tb_PrecioVenta.Text = oBuscar.PrecioVenta;
                    tb_Precio.Text = oBuscar.PrecioCompra;
                    
                }
                catch (Exception exx)
                {

                }


            }

        }

        public void Limpiar()
        {
            tb_Total.Text = String.Empty;
            tb_Precio.Text = String.Empty;
            tb_Cantidad.Text = String.Empty;
            tb_Descripcion.Text = String.Empty;
            tb_Producto.Text = String.Empty;
            tb_Sucursal.Text = String.Empty;
            tb_PrecioVenta.Text = String.Empty;
            dgv_Datos.Rows.Clear();
        }

        public void LimpiarR()
        {
            tb_Total.Text = String.Empty;
            tb_Precio.Text = String.Empty;
            tb_Cantidad.Text = String.Empty;
            tb_Descripcion.Text = String.Empty;
            tb_Producto.Text = String.Empty;
            tb_Sucursal.Text = String.Empty;
            tb_PrecioVenta.Text = String.Empty;

            if (oDatosInventarios.renglonesDetalle.Count > 0)
                oDatosInventarios.renglonesDetalle.Clear();


            if (opInventarios.ObtenerMax() == 0)
            {
                tb_Folio.Text = 1.ToString();
            }
            else
            {

                tb_Folio.Text = (opInventarios.ObtenerMax() + 1).ToString();
            }

            dgv_Datos.Rows.Clear();
        }

        private void btn_Limpiar_Click(object sender, EventArgs e)
        {
            Limpiar();
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            opInventarios oOpInventarios = new opInventarios();

            if (tb_Producto.Text.Equals("") || tb_Folio.Text.Equals("") || tb_Sucursal.Text.Equals(""))
            {
                MessageBox.Show("Asegurese de que los campos de Producto, Folio y Sucursal no estén vacios");
                return;
            }

            if (oOpInventarios.Baja(tb_Folio.Text, tb_Sucursal.Text,tb_Producto.Text))
            {
                MessageBox.Show("Se ha eliminado correctamente");
                Limpiar();
            }
            else
            {
                MessageBox.Show("Ha ocurrido un error" + oOpInventarios.sLastError);
            }
        }

        private void tb_Cantidad_TextChanged(object sender, EventArgs e)
        {

        }

        private void tb_Producto_TextChanged(object sender, EventArgs e)
        {

        }

        private void pictureBox9_Click(object sender, EventArgs e)
        {

        }

        private void btn_Registrar_Click(object sender, EventArgs e)
        {
            if (Conexion.AbrirConexion())
            {
                opInventarios oInv = new opInventarios();
                datosInventarios dtos = new datosInventarios();

                try
                {
                    if (cb_TipoMovimiento.Text.Equals("") || tb_Sucursal.Text.Equals("") || tb_Cantidad.Text.Equals("") || tb_Folio.Text.Equals(""))
                    {
                        MessageBox.Show("Asegurese de   ingresar el tipo de movimiento, la sucursal, la cantidad  y el folio");
                        return;
                    }


                    if (oInv.ExisteFolio(Convert.ToInt32(tb_Folio.Text)) == 1)
                    {
                        MessageBox.Show("El folio ingresado ya existe");
                        LimpiarR();
                        return;
                    }

                    if (oInv.Alta(oDatosInventarios))
                    {
                        MessageBox.Show("El movimiento se ha registrado");
                        oDatosInventarios.renglonesDetalle.Clear();
                        LimpiarR();
                    }
                    else
                    {
                        MessageBox.Show("Saldo Insuficiente");
                        Limpiar();
                    }
                }
                catch (Exception error)
                {
                    MessageBox.Show(error.Message);
                }
            }
        }

        private void Venta_Load(object sender, EventArgs e)
        {
            if (opInventarios.ObtenerMax() == 0)
            {
                tb_Folio.Text = 1.ToString();
            }
            else
            {

                tb_Folio.Text = (opInventarios.ObtenerMax() + 1).ToString();
            }
        }
    }
}
