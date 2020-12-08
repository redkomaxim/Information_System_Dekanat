using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Printing;
using System.Globalization;


namespace WindowsFormsApplication3
{
    public partial class Form1 : Form
    {
        public DataSet ds;
        public string[] header_avia;
        public string[] header_pass;
        public string[] header_tick;
        public string[] airports;
        public string[] fio_array;
        public string[] pasport_array;
        public string pass_id;
        public string airport_in;
        public string airport_end;
        public DataTable ticketsTable;
        string file_avia_path = @"C:\\kurs\\avia.txt";
        string file_airport_path = @"C:\\kurs\\airport.txt";
        string file_pass_path = @"C:\\kurs\\passenger.txt";
        string file_ticket_path = @"C:\\kurs\\tickets.txt";
        string file_print_path = @"C:\\kurs\\print.txt";
        private PrintDocument printDocument1 = new PrintDocument();
        private string stringToPrint;
        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            read_avia();
            read_airport();
            read_passenger();
            create_table_tickets();
            read_tickets();
            FioBox.DataSource = ds.Tables[1];
            FioBox.DisplayMember = "ФИО";
            FioBox.ValueMember = "Номер регистрации";
            Airports1.Items.AddRange(airports);
            Airports2.Items.AddRange(airports);
            
        }

        public void button1_Click(object sender, EventArgs e)
        {
            write_avia();
        }
        //
        // Reading Aviareis of txt file
        //
        public void read_avia() {
            StreamReader rd = new StreamReader(file_avia_path);
            ds = new DataSet();
            ds.Tables.Add("Aviareis");
            string header = rd.ReadLine();
            header_avia = System.Text.RegularExpressions.Regex.Split(header, ";");
            for (int c = 0; c < header_avia.Length; c++)
            {
                ds.Tables[0].Columns.Add(header_avia[c]);
            }
            string row = rd.ReadLine();
            while (row != null)
            {
                string[] rvalue = System.Text.RegularExpressions.Regex.Split(row, ";");
                string date = rvalue[5];
                DateTime dateTime = DateTime.Parse(date);
                ds.Tables[0].Rows.Add(rvalue[0], rvalue[1], rvalue[2], rvalue[3], rvalue[4], dateTime, rvalue[6], rvalue[7], rvalue[8]);
                row = rd.ReadLine();
            }
            dataGridView1.DataSource = ds.Tables[0];
            rd.Close();
        }
        //
        // Reading Passenger of txt file
        //
        public void read_passenger()
        {
            int count = System.IO.File.ReadAllLines(file_pass_path).Length;
            fio_array = new string[count];
            pasport_array = new string[count];
            StreamReader rd = new StreamReader(file_pass_path);
            ds.Tables.Add("Passenger");
            string header = rd.ReadLine();
            header_pass = System.Text.RegularExpressions.Regex.Split(header, ";");
            for (int c = 0; c < header_pass.Length; c++)
            {
                ds.Tables["Passenger"].Columns.Add(header_pass[c]);
            }
            string row = rd.ReadLine();
            while (row != null)
            {
                string[] rvalue = System.Text.RegularExpressions.Regex.Split(row, ";");
                ds.Tables["Passenger"].Rows.Add(rvalue);
                fio_array[Int32.Parse(rvalue[0])] = rvalue[2];
                pasport_array[Int32.Parse(rvalue[0])] = rvalue[1];
                row = rd.ReadLine();
            }
            dataGridView2.DataSource = ds.Tables["Passenger"];
            rd.Close();
        }
        //
        //
        public void read_tickets()
        {
            StreamReader rd = new StreamReader(file_ticket_path);
            ds.Tables.Add("Tickets");
            string header = rd.ReadLine();
            header_tick = System.Text.RegularExpressions.Regex.Split(header, ";");
            for (int c = 0; c < header_tick.Length; c++)
            {
                ds.Tables["Tickets"].Columns.Add(header_tick[c]);
            }
            string row = rd.ReadLine();
            while (row != null)
            {
                string[] rvalue = System.Text.RegularExpressions.Regex.Split(row, ";");
                ds.Tables["Tickets"].Rows.Add(rvalue);
                ticketsTable.Rows.Add(rvalue[0], rvalue[1], pasport_array[Int16.Parse(rvalue[2])], fio_array[Int16.Parse(rvalue[2])]);
                row = rd.ReadLine();
            }
          //  dataGridView2.DataSource = ds.Tables["Tickets"];
            dataGridView3.DataSource = ticketsTable;
            rd.Close();
        }
        //
        // Create local datatable Tickets
        //
        public void create_table_tickets() {
            ticketsTable = new DataTable("Tickets");
            DataColumn nomColumn = new DataColumn("Номер билета", Type.GetType("System.String"));
            DataColumn nom_rColumn = new DataColumn("Номер рейса", Type.GetType("System.String"));
            DataColumn nom_pColumn = new DataColumn("Номер паспорта", Type.GetType("System.String"));
            DataColumn fioColumn = new DataColumn("ФИО", Type.GetType("System.String"));
            ticketsTable.Columns.Add(nomColumn);
            ticketsTable.Columns.Add(nom_rColumn);
            ticketsTable.Columns.Add(nom_pColumn);
            ticketsTable.Columns.Add(fioColumn);
            dataGridView3.DataSource = ticketsTable;

        }
        //
        // Read Airports of file
        //
        public void read_airport()
        {
            int count = System.IO.File.ReadAllLines(file_airport_path).Length;
            StreamReader rda = new StreamReader(file_airport_path);
            airports = new string[count];
            string rows = rda.ReadLine();
            int i = 0;
            while (rows != null)
            {
                airports[i] = rows;
                rows = rda.ReadLine();
                i++;
            }
            rda.Close();
        }
        //
        // Creating new aviareis
        //
        public void add_avia(string numb, string company, string start_airport, string last_airport, string additional, string date, string cols, string buyed, string cost) {
            DataRow row = ds.Tables[0].NewRow();
            row[header_avia[0]] = numb;
            row[header_avia[1]] = company;
            row[header_avia[2]] = start_airport;
            row[header_avia[3]] = last_airport;
            row[header_avia[4]] = additional;
            row[header_avia[5]] = date;
            row[header_avia[6]] = cols;
            row[header_avia[7]] = buyed;
            row[header_avia[8]] = cost;
            ds.Tables[0].Rows.Add(row);
            dataGridView1.DataSource = null;
            dataGridView1.DataSource = ds.Tables[0];

        }
        public void add_pass(string numb_seria, string fio, string date)
        {
            DataRow row = ds.Tables["Passenger"].NewRow();
            int count = ds.Tables["Passenger"].Rows.Count;
            count++;
            row[header_pass[0]] = count;
            row[header_pass[1]] = numb_seria;
            row[header_pass[2]] = fio;
            row[header_pass[3]] = date;
            ds.Tables["Passenger"].Rows.Add(row);
            dataGridView2.DataSource = null;
            dataGridView2.DataSource = ds.Tables["Passenger"];

        }
        public void add_ticket(string number_reis, string number_pass)
        {
            DataRow row = ds.Tables["Tickets"].NewRow();
            int count = ds.Tables["Tickets"].Rows.Count;
            count++;
            row[header_tick[0]] = count;
            row[header_tick[1]] = number_reis;
            row[header_tick[2]] = number_pass;
            ds.Tables["Tickets"].Rows.Add(row);
            ticketsTable.Rows.Add(count, number_reis, pasport_array[Int16.Parse(number_pass)], fio_array[Int16.Parse(number_pass)]);

        }
        public void write_tickets()
        {
            // File.Delete(file_avia_path);
            StreamWriter wr = new StreamWriter(file_ticket_path, false);
            wr.WriteLine(header_tick[0] + ";" + header_tick[1] + ";" + header_tick[2] );
            for (int i = 0; i < ds.Tables["Tickets"].Rows.Count; i++)
            {
                wr.WriteLine((ds.Tables["Tickets"].Rows[i][0]).ToString() + ";" + (ds.Tables["Tickets"].Rows[i][1]).ToString() + ";"
                    + (ds.Tables["Tickets"].Rows[i][2]).ToString());
            }
            wr.Close();
        }
        public void write_pass() {
           // File.Delete(file_avia_path);
            StreamWriter wr = new StreamWriter(file_pass_path, false);
            wr.WriteLine(header_pass[0] + ";" + header_pass[1] + ";" + header_pass[2] + ";" + header_pass[3]);
            for (int i = 0; i < ds.Tables["Passenger"].Rows.Count; i++)
            {
                wr.WriteLine((ds.Tables["Passenger"].Rows[i][0]).ToString() + ";" + (ds.Tables["Passenger"].Rows[i][1]).ToString() + ";"
                    + (ds.Tables["Passenger"].Rows[i][2]).ToString() + ";" + (ds.Tables["Passenger"].Rows[i][3]).ToString()
                    );
            }
            wr.Close();
        }
        public void write_avia()
        {
            StreamWriter wr = new StreamWriter(file_avia_path, false);
            wr.WriteLine(header_avia[0] + ";" + header_avia[1] + ";" + header_avia[2] + ";" + header_avia[3] + ";" + header_avia[4] + ";" + header_avia[5] + ";" + header_avia[6] + ";" +
                header_avia[7] + ";" + header_avia[8]
                    );
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                wr.WriteLine((ds.Tables[0].Rows[i][0]).ToString() + ";" + (ds.Tables[0].Rows[i][1]).ToString() + ";"
                    + (ds.Tables[0].Rows[i][2]).ToString() + ";" + (ds.Tables[0].Rows[i][3]).ToString() + ";"
                    + (ds.Tables[0].Rows[i][4]).ToString() + ";" + (ds.Tables[0].Rows[i][5]).ToString() + ";"
                    + (ds.Tables[0].Rows[i][6]).ToString() + ";" + (ds.Tables[0].Rows[i][7]).ToString() + ";"
                    + (ds.Tables[0].Rows[i][8]).ToString()
                    );
            }
            wr.Close();
        }
        public void write_airport(string airportss)
        {
            StreamWriter wr = new StreamWriter(file_airport_path, false);
            for (int i = 0; i < airports.Length; i++)
            {
                wr.WriteLine(airports[i]);
            }
            wr.WriteLine(airportss);
            wr.Close();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form2 af = new Form2();
            af.Owner = this;
            af.Show();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Remove(dataGridView1.CurrentRow);
            write_avia();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            dataGridView2.Rows.Remove(dataGridView2.CurrentRow);
            write_pass();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            Form3 af = new Form3();
            af.Owner = this;
            af.Show();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DataTable Tickets = ds.Tables["Tickets"];
            string temp = dataGridView1[0, dataGridView1.CurrentRow.Index].Value.ToString();

            IEnumerable<DataRow> TicketsQuery =
                from tickets in Tickets.AsEnumerable()
                select tickets;

            IEnumerable<DataRow> tickets_qu =
                TicketsQuery.Where(p => p.Field<string>("Номер рейса") == temp);

            StreamWriter wr = new StreamWriter(file_print_path, false);
            wr.WriteLine("___________________________________________________");
            wr.WriteLine("Посадочная ведомость на рейс " + temp);
            wr.WriteLine("___________________________________________________");
            wr.WriteLine( "    |" + "Номер Билета" + "|  " + header_pass[2] + "  |   " + header_pass[1] + " |  ");
            wr.WriteLine("___________________________________________________");
            foreach (DataRow ticket in tickets_qu)
            {
                string temps = ticket.Field<string>("Номер пасажира");
                wr.WriteLine("    |     " + ticket.Field<string>("Номер билета") + "    |     " + fio_array[Int16.Parse(temps)] + "    |     " + pasport_array[Int16.Parse(temps)] + "    |     ");
                wr.WriteLine("___________________________________________________");
            }             
            wr.Close();
            print("print.txt", @"C:\\kurs\\");
            File.Delete(file_print_path);

        }

        private void printDocument1_PrintPage(object sender, PrintPageEventArgs e)
        {
            int charactersOnPage = 0;
            int linesPerPage = 0;

            // Sets the value of charactersOnPage to the number of characters 
            // of stringToPrint that will fit within the bounds of the page.
            e.Graphics.MeasureString(stringToPrint, this.Font,
                e.MarginBounds.Size, StringFormat.GenericTypographic,
                out charactersOnPage, out linesPerPage);

            // Draws the string within the bounds of the page
            e.Graphics.DrawString(stringToPrint, this.Font, Brushes.Black,
                e.MarginBounds, StringFormat.GenericTypographic);

            // Remove the portion of the string that has been printed.
            stringToPrint = stringToPrint.Substring(charactersOnPage);

            // Check to see if more pages are to be printed.
            e.HasMorePages = (stringToPrint.Length > 0);
        }

        public void print(string docName, string docPath) {
            printDocument1.DocumentName = docName;
            using (FileStream stream = new FileStream(docPath + docName, FileMode.Open))
            using (StreamReader reader = new StreamReader(stream))
            {
                stringToPrint = reader.ReadToEnd();
            }
            printDocument1.Print();
        }

        private void button7_Click(object sender, EventArgs e)
        {
            pass_id = FioBox.SelectedValue.ToString(); ;
            airport_in = this.Airports1.Items[this.Airports1.SelectedIndex].ToString();
            airport_end = this.Airports2.Items[this.Airports2.SelectedIndex].ToString();
            Form4 af = new Form4();
            af.Owner = this;
            af.Show();
        }

        private void tabControl1_Selecting(object sender, TabControlCancelEventArgs e)
        {
          dataGridView1.DataSource = null;
          dataGridView1.DataSource = ds.Tables[0];
          (dataGridView1.DataSource as DataTable).DefaultView.RowFilter = null;
        }

        private void button8_Click(object sender, EventArgs e)
        {
            Form5 af = new Form5();
            af.Owner = this;
            af.Show();
        }
    }
}
