using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication3
{
    public partial class Form4 : Form
    {
        public Form4()
        {
            InitializeComponent();
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            if (main != null)
            {
                dataGrid.DataSource = main.ds.Tables["Aviareis"];
                dataGrid.Columns["Компания"].Visible = false;
                dataGrid.Columns["Аеропорт отправления"].Visible = false;
                dataGrid.Columns["Аеропорт назначения"].Visible = false;
                dataGrid.Columns["Промежуточные"].Visible = false;
                dataGrid.Columns["Куплено билетов"].Visible = false;
                (dataGrid.DataSource as DataTable).DefaultView.RowFilter =
                String.Format("([Аеропорт отправления] like '{0}%' OR [Промежуточные] like '{0}%') AND ([Аеропорт назначения] like '{1}%' OR [Промежуточные] like '{1}%')", main.airport_in, main.airport_end);
                this.dataGrid.Sort(this.dataGrid.Columns["Дата и время"], ListSortDirection.Descending);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string temp_number;
            temp_number = dataGrid[6, dataGrid.CurrentRow.Index].Value.ToString();
            dataGrid.CurrentRow.Cells[6].Value = Int16.Parse(temp_number) - 1;
            temp_number = dataGrid[7, dataGrid.CurrentRow.Index].Value.ToString();
            dataGrid.CurrentRow.Cells[7].Value = Int16.Parse(temp_number) + 1 ;
            temp_number = dataGrid[0, dataGrid.CurrentRow.Index].Value.ToString();
            Form1 main = this.Owner as Form1;
            if (main != null)
            {
                main.add_ticket(temp_number, main.pass_id);
                main.write_tickets();
                main.write_avia();
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }
    }
}
