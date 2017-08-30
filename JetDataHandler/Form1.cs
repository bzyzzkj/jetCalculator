using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;

namespace JetDataHandler
{
    public partial class Form1 : Form
    {
        public DataTable data;
        public double beta;
        public double gamma;
        public double theta;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            BindComboBox1();
            BindComboBox2();
            beta = 1 / 2;
            gamma = 1 - beta;
            theta = 0;
        }
        private void BindComboBox1()
        {
            List<MathFunction.DeferenceMethods> infoList = new List<MathFunction.DeferenceMethods>();
            MathFunction.DeferenceMethods info1 = new MathFunction.DeferenceMethods() { Methods = "LC", Value = 0.5 };
            MathFunction.DeferenceMethods info2 = new MathFunction.DeferenceMethods() { Methods = "FCV", Value = 0.375 };
            MathFunction.DeferenceMethods info3 = new MathFunction.DeferenceMethods() { Methods = "FE", Value = 1.0/3.0 };
            infoList.Add(info1);
            infoList.Add(info2);
            infoList.Add(info3);
            comboBox1.DataSource = infoList;
            comboBox1.DisplayMember = "Methods";
            comboBox1.ValueMember = "Value";
        }
        private void BindComboBox2()
        {
            List<double> infoList = new List<double>();
            infoList.Add(0);
            infoList.Add(0.5);
            infoList.Add(1);
            comboBox2.DataSource = infoList;
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            PathText.Text = file.FileName;
            if (file.FileName!=null)
            {
                data = ExcelHandler.ExcelToDS(file.FileName);
            }  
        }

        private void calButton_Click(object sender, EventArgs e)
        {
            
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            beta = System.Convert.ToDouble(comboBox1.SelectedValue.ToString());
            gamma = 0.5 - beta;
            theta= System.Convert.ToDouble(comboBox2.SelectedValue.ToString());
        }
    }
}
