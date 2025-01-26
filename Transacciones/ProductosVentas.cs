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
    public partial class ProductosVentas : Form
    {
        public ProductosVentas()
        {
            InitializeComponent();
        }

        private void pb_Producto_Click(object sender, EventArgs e)
        {
            if(!btn_Productos.Visible)
            btn_Productos.Visible = true;
            else
                btn_Productos.Visible = false;
        }
        private void pb_Venta_Click(object sender, EventArgs e)
        {
            if (!btn_Ventas.Visible)
                btn_Ventas.Visible = true;
            else
                btn_Ventas.Visible = false;
        }
     
        private void pb_Venta_MouseEnter(object sender, EventArgs e)
        {
            pb_Venta.Size = new Size(64, 61);

        }

        private void pb_Venta_MouseLeave(object sender, EventArgs e)
        {
            pb_Venta.Size = new Size(49, 46);

        }

        private void pb_Producto_MouseEnter(object sender, EventArgs e)
        {
            pb_Producto.Size = new Size(64, 61);
        }

        private void pb_Producto_MouseLeave(object sender, EventArgs e)
        {
            pb_Producto.Size = new Size(49, 46);

        }

        private void btn_Productos_Click(object sender, EventArgs e)
        {
            Producto p = new Producto();
                p.ShowDialog();

        }

        private void btn_Ventas_Click(object sender, EventArgs e)
        {
            Venta v = new Venta();
            v.ShowDialog();
        }

        private void ProductosVentas_FormClosed(object sender, FormClosedEventArgs e)
        {
            Application.Exit();
        }

        private void ProductosVentas_Load(object sender, EventArgs e)
        {

        }
    }
}
