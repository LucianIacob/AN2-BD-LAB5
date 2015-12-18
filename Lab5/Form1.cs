using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab5
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                sqlDataAdapter2.Fill(dataSet, "sectii");
                sqlDataAdapter1.Fill(dataSet, "studenti");
                listBox1.Items.Clear();
                for (int i = 0; i < dataSet.Tables["sectii"].Rows.Count; i++)
                {
                    listBox1.Items.Add(dataSet.Tables["sectii"].Rows[i]["cods"].ToString() + "." + dataSet.Tables["sectii"].Rows[i]["denumires"].ToString());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            refreshStudents();
        }

        private void refreshStudents()
        {
            listBox2.Items.Clear();
            for (int i = 0; i < dataSet.Tables["studenti"].Rows.Count; i++)
            {
                string cods = listBox1.Items[listBox1.SelectedIndex].ToString().Split('.')[0];
                if (dataSet.Tables["studenti"].Rows[i]["cods"].ToString() == cods)
                    listBox2.Items.Add(dataSet.Tables["studenti"].Rows[i]["nume"].ToString() + "." + dataSet.Tables["studenti"].Rows[i]["grupa"].ToString());
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                string nume = listBox2.Items[listBox2.SelectedIndex].ToString().Split('.')[0];
                string grupa = listBox2.Items[listBox2.SelectedIndex].ToString().Split('.')[1];

                for (int i = 0; i < dataSet.Tables["studenti"].Rows.Count; i++)
                    if (dataSet.Tables["studenti"].Rows[i]["nume"].ToString() == nume)
                    {
                        textCods.Text = dataSet.Tables["studenti"].Rows[i]["cods"].ToString();
                        textDataN.Text = dataSet.Tables["studenti"].Rows[i]["datan"].ToString();
                        textNRM.Text = dataSet.Tables["studenti"].Rows[i]["nrmatricol"].ToString();
                        textNume.Text = nume;
                        textGrupa.Text = grupa;
                    }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            golire();
        }

        public void golire()
        {
            textNume.Clear();
            textNRM.Clear();
            textDataN.Clear();
            textCods.Clear();
            textGrupa.Clear();
        }

        private void buttonAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int changes = 0;
                if (textCods.Text == "")
                    changes++;
                if (textNRM.Text == "")
                    changes++;
                if (textNume.Text == "")
                    changes++;
                if (textDataN.Text == "")
                    changes++;
                if (changes == 4)
                {
                    MessageBox.Show("Fields empty!");
                    return;
                }


                for (int i = 0; i < dataSet.Tables["studenti"].Rows.Count; i++)
                    if (dataSet.Tables["studenti"].Rows[i]["nrmatricol"].ToString() == textNRM.Text)
                    {
                        MessageBox.Show("Exista deja un student cu acest numar matricol!");
                        return;
                    }
                DataRow newStudent = dataSet.Tables["studenti"].NewRow();
                dataSet.Tables["studenti"].AcceptChanges();
                newStudent["cods"] = textCods.Text;
                newStudent["nrmatricol"] = textNRM.Text;
                newStudent["nume"] = textNume.Text;
                newStudent["grupa"] = textGrupa.Text;
                newStudent["datan"] = textDataN.Text;

                sqlDataAdapter1.InsertCommand = new SqlCommand("insert into studenti (cods, nrmatricol, nume, grupa, datan) values (" + textCods.Text + ", '" + textNRM.Text + "', '" + textNume.Text + "', '" + textGrupa.Text + "', null)", sqlConnection1);
                dataSet.Tables["studenti"].Rows.Add(newStudent);
                sqlDataAdapter1.Update(dataSet, "studenti");
                refreshStudents();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            int changes = 0;
            if (textCods.Text == "")
                changes++;
            if (textNRM.Text == "")
                changes++;
            if (textNume.Text == "")
                changes++;
            if (textDataN.Text == "")
                changes++;
            if (changes == 4)
            {
                MessageBox.Show("Fields empty!");
                return;
            }

            for (int i = 0; i < dataSet.Tables["studenti"].Rows.Count; i++)
                if (dataSet.Tables["studenti"].Rows[i]["nrmatricol"].ToString() == textNRM.Text)
                {
                    changes=0;
                    if (dataSet.Tables["studenti"].Rows[i]["cods"].ToString() == textCods.Text)
                        changes++;
                    if (dataSet.Tables["studenti"].Rows[i]["nume"].ToString() == textNume.Text)
                        changes++;
                    if (dataSet.Tables["studenti"].Rows[i]["grupa"].ToString() == textGrupa.Text)
                        changes++;

                    if (changes == 3)
                    {
                        MessageBox.Show("Nothing to update!");
                        return;
                    }

                    dataSet.Tables["studenti"].AcceptChanges();
                    dataSet.Tables["studenti"].Rows[i]["cods"] = textCods.Text;
                    dataSet.Tables["studenti"].Rows[i]["nume"] = textNume.Text;
                    dataSet.Tables["studenti"].Rows[i]["grupa"] = textGrupa.Text;

                    sqlDataAdapter1.UpdateCommand = new SqlCommand("UPDATE studenti SET cods='" + textCods.Text + "', nume='" + textNume.Text + "', grupa='" + textGrupa.Text + "' WHERE nrmatricol='" + textNRM.Text + "'", sqlConnection1);
                    sqlDataAdapter1.Update(dataSet, "studenti");

                    refreshStudents();
                    golire();
                    return;
                }
            MessageBox.Show("Student neselectat corect!");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int changes = 0;
            if (textCods.Text == "")
                changes++;
            if (textNRM.Text == "")
                changes++;
            if (textNume.Text == "")
                changes++;
            if (textDataN.Text == "")
                changes++;
            if (changes == 4)
            {
                MessageBox.Show("Fields empty!");
                return;
            }

            for (int i = 0; i < dataSet.Tables["studenti"].Rows.Count; i++)
                if (dataSet.Tables["studenti"].Rows[i]["nrmatricol"].ToString() == textNRM.Text)
                {
                    dataSet.Tables["studenti"].AcceptChanges();
                    dataSet.Tables["studenti"].Rows[i].Delete();
                }

            sqlDataAdapter1.DeleteCommand = new SqlCommand("delete from studenti where nrmatricol='" + textNRM.Text + "'", sqlConnection1);
            sqlDataAdapter1.Update(dataSet, "studenti");
            
            refreshStudents();
            golire();
        }
    }
}
