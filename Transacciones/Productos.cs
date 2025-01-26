using RDN;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Transacciones_AVR_501
{
    public partial class Producto : Form
    {
        public Producto()
        {
            InitializeComponent();
        }

        private void btn_Guardar_Click(object sender, EventArgs e)
        {
            opInventarios objInv = new opInventarios();
            
            if(objInv.ValidarCampos(tb_Producto.Text,tb_PrecioVenta.Text,tb_PrecioCompra.Text,tb_Descpricion.Text))
            {
                MessageBox.Show("Aseguresde ingresar los campos correspondientes");
                return;
            }
            //try
           // {
                if (objInv.ExisteProducto(tb_Producto.Text) == 1)
                {
                    MessageBox.Show("Ese producto ya existe");
                    return;
                }
                if (objInv.AgregarProducto(tb_Producto.Text, tb_Descpricion.Text, tb_PrecioCompra.Text, tb_PrecioVenta.Text))
                {
                    MessageBox.Show("Producto agrgado correctamente");
                }
                else
                {
                    MessageBox.Show(objInv.sLastError);
                }
            
           // catch (System.Data.Entity.Core.EntityException ex)
            //{
                
           // }
        }

        private void btn_Limpiar_Click(object sender, EventArgs e)
        {
            tb_Descpricion.Text = String.Empty;
            tb_PrecioCompra.Text = String.Empty;
            tb_PrecioVenta.Text = String.Empty;
            tb_Producto.Text = String.Empty;
        }

        private void btn_Eliminar_Click(object sender, EventArgs e)
        {
            opInventarios objInv = new opInventarios();

            if(objInv.ValidarCampos(tb_Producto.Text,tb_PrecioVenta.Text,tb_PrecioCompra.Text,tb_Descpricion.Text))
            {
                MessageBox.Show("Asegures de ingresar datos en los campos correspondientes");
                return;
            }

            if (objInv.EliminarProducto(tb_Producto.Text))
            {
                MessageBox.Show("Producto eliminado correctamente");
            }
            else
            {
                MessageBox.Show(objInv.sLastError);
            }

        }

        private void Producto_Load(object sender, EventArgs e)
        {

        }

        private void tb_Descpricion_TextChanged(object sender, EventArgs e)
        {

        }

      

        private void tb_Producto_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                try
                {

                    opInventarios objInv = new opInventarios();
                    DataTable data = new DataTable();
                    data = objInv.RellenarTextBox(tb_Producto.Text);

                    tb_Descpricion.Text = data.Rows[0][1].ToString();
                    tb_PrecioCompra.Text = data.Rows[0][2].ToString();
                    tb_PrecioVenta.Text = data.Rows[0][3].ToString();
                }
                catch(Exception exx)
                {

                }


            }
        }

        private void tb_Producto_Leave(object sender, EventArgs e)
        {

        }
    }
}
