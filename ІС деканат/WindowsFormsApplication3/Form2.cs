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
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            if (main != null)
            {
                main.add_avia(b1.Text, b2.Text, Airport1.Text
                 , Airport2.Text, listBox1.Text, dateTimePicker1.Text + dateTimePicker2.Text, b7.Text, b8.Text, b9.Text);
                main.write_avia();
            }
            Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            Form1 main = this.Owner as Form1;
            Airport1.Items.AddRange(main.airports);
            Airport2.Items.AddRange(main.airports);
            listBox1.Items.AddRange(main.airports);
        }
    }
}
