using Npgsql;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace z1
{
    enum RowState
    {
        Exited,
        New,
        Modifided,
        ModifidedNew
    }
    public partial class Form1 : Form
    {       
        Database database = new Database(); //подключение к бд
        int selectedRow;
        public Form1()
        {
            InitializeComponent();
        }
       

        private void createColumns () //колонки для datagrid
        {
            dataGridView1.Columns.Add("id", "id");
            dataGridView1.Columns.Add("firstname", "Имя");
            dataGridView1.Columns.Add("secondname", "Фамилия");
            dataGridView1.Columns.Add("isNew", String.Empty);
        }

        private void ClearFields() //очищение textbox
        {
            idTB.Text = "";
            fNameTB.Text = "";
            sNameTB.Text = "";
        }

        private void ReadSingleRow(DataGridView dgw, IDataRecord record)
        {

            dgw.Rows.Add(record.GetInt32(0), record.GetString(1), record.GetString(2), RowState.ModifidedNew);
        }

        private void refreshDataGrid(DataGridView dgw) //обновление datagrid
        {
            dgw.Rows.Clear();

            string querryString = $"select * from bros";

            NpgsqlCommand comm = new NpgsqlCommand(querryString, database.getConnection());

            database.openConnection();

            NpgsqlDataReader reader = comm.ExecuteReader();

            while (reader.Read())
            {
                ReadSingleRow(dgw, reader);
            }
            reader.Close();
            database.closeConnection();
        }

        //вывод информации из dataGridView1 в textbox'ы
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedRow = e.RowIndex;

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = dataGridView1.Rows[selectedRow];

                idTB.Text = row.Cells[0].Value.ToString();
                fNameTB.Text = row.Cells[1].Value.ToString();
                sNameTB.Text = row.Cells[2].Value.ToString();
            }
        }

        private void Edit()
        {
            var selectedRowIndex = dataGridView1.CurrentCell.RowIndex;

            var id = idTB.Text;
            var fName = fNameTB.Text;
            var sName = sNameTB.Text;

            if (dataGridView1.Rows[selectedRowIndex].Cells[0].Value.ToString() != string.Empty)
            {
                dataGridView1.Rows[selectedRowIndex].SetValues(id, fName, sName);
                dataGridView1.Rows[selectedRowIndex].Cells[3].Value = RowState.Modifided;
            }

        }

        //метод для проверки RowState
        private void Update()
        {
            database.openConnection();

            for (int index = 0; index < dataGridView1.Rows.Count; index++)
            {
                var rowState = (RowState)dataGridView1.Rows[index].Cells[3].Value;

                if (rowState == RowState.Exited)
                    continue;

                if (rowState == RowState.Modifided)
                {
                    var id = dataGridView1.Rows[index].Cells[0].Value.ToString();
                    var fName = dataGridView1.Rows[index].Cells[1].Value.ToString();
                    var sName = dataGridView1.Rows[index].Cells[2].Value.ToString();

                    var changeQuery = $"update bros set firstname = '{fName}', secondname = '{sName}' where id = '{id}'";

                    var comm = new NpgsqlCommand(changeQuery, database.getConnection());
                    comm.ExecuteNonQuery();
                }

            }
            database.closeConnection();
        }

        private void saveBut_Click(object sender, EventArgs e)
        {
            Update();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            createColumns();
            refreshDataGrid(dataGridView1);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            addForm addForm = new addForm();
            addForm.Show();
            Hide();
        }

        private void editBut_Click(object sender, EventArgs e)
        {
            Edit();
            ClearFields();
        }



        private void button1_Click(object sender, EventArgs e)
        {
            var text = richTextBox1.ToString();

            saveFileDialog1.Filter = "pdf files (*.pdf)|*.pdf";
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK && saveFileDialog1.FileName.Length > 0)
            {
                System.IO.File.WriteAllText(saveFileDialog1.FileName, text);
                richTextBox1.SaveFile(saveFileDialog1.FileName, RichTextBoxStreamType.PlainText);
            }
        }
    }
}
