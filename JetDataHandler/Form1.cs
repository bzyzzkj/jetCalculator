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
using MathNet.Numerics.Data.Text;
using MathNet.Numerics.LinearAlgebra;

namespace JetDataHandler
{
    public partial class Form1 : Form
    {

        public Vector<double> heat_flux_array;
        public Vector<double> temperature;
        public Matrix<double> matrix;

        private Vector<double> temperature2;
        private Vector<double> temperature3;
        private Vector<double> temperature4;
        private Vector<double> temperature5;

        private Vector<double> voltage;

        public Vector<double> initialTem;
        public Vector<double> result;



        public int dataNumber;//数据数量
        public int acqusitionFre;//采样频率

        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load_1(object sender, EventArgs e)
        {
            BindComboBox1();
            BindComboBox2();
            MathFunction.beta = 0.5;
            MathFunction.gamma = 1 - MathFunction.beta;
            MathFunction.theta = 0.5;

        
            thermalDiffusivity = 3.0;
            deltaX = 0.00002;
            xNum = 20;
            density = 100;
            specific_heat_capacity = 20;
            thermalDiffusivity = 20;
            deltaT = 1.0 / xNum;
            c = 1000;

            initialTem = CreateVector.Dense<double>(xNum, 35);
            

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
        private Vector<double> vectorTem(double varBeta, double varTheta)
        {
            Vector<double> temVector=CreateVector.Dense<double>(xNum, i=> 
            {
                if (i==0)
                {
                    return 
                }
            })
        }
        private Vector<double> GetResult(Matrix<double> varMatrix,Vector<double> varTem,Vector<double> varVoltage,int count)
        {
            Vector<double> varResult;
            Vector<double> vectorTem = CreateVector.Dense<double>(xNum, i =>
            {
                if (i==0||i==1)
                {
                    return varTem[count];
                }
                else
                {
                    return 0;
                }
            }).Multiply(varTem[count]);
            Vector<double> vectorPower = CreateVector.Dense<double>(xNum, i =>
            {
                if (i==0)
                {

                }
            })
            Vector<double> vectorPower;
            return varResult;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.ShowDialog();
            PathText.Text = file.FileName;
            if (file.FileName!=null)
            {
                matrix = DelimitedReader.Read<double>(file.FileName, false, ",", true);
            }
            temperature2 = matrix.Column(0);
            
            MessageBox.Show(temperature2.ToString());
        }

        private void calButton_Click(object sender, EventArgs e)
        {
            Matrix<double> calMatrix= MathFunction.DeferenceMethods.createMatrix(beta,theta,xNum,deltaX,thermalDiffusivity,deltaT,density,c);
            DelimitedWriter.Write("‪test.csv", calMatrix, ",");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            beta = System.Convert.ToDouble(comboBox1.SelectedValue.ToString());
            gamma = 0.5 - beta;
            theta= System.Convert.ToDouble(comboBox2.SelectedValue.ToString());
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                density = System.Convert.ToDouble(textBox1.Text);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                specific_heat_capacity = System.Convert.ToDouble(textBox1.Text);
            }
        }
    }
}
