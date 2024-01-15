using Npgsql;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace z1
{
    public partial class addForm : Form
    {
        public addForm()
        {
            InitializeComponent();
        }
        Database database = new Database();
        private void button2_Click(object sender, EventArgs e)
        {
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            database.openConnection();

            var firstName = fNameTB.Text;
            var secondName = sNameTB.Text;

            var addQuery = $"insert into bros (firstname, secondname) values ('{firstName}','{secondName}')";
            var comm = new NpgsqlCommand(addQuery, database.getConnection());
            comm.ExecuteNonQuery();

            MessageBox.Show("Запись добавлена", "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
            database.closeConnection();
            
            Form1 form1 = new Form1();
            form1.Show();
            Close();
        }
    }
}
