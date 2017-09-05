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
using MathNet.Numerics.LinearAlgebra.Complex;

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

        
            MathFunction.thermalDiffusivity = 3.0;
            MathFunction.deltaX = 0.00002;
            MathFunction.xNum = 20;
            MathFunction.density = 100;
            MathFunction.specific_heat_capacity = 20;
            MathFunction.thermalDiffusivity = 20;
            MathFunction.deltaT = 1.0 / MathFunction.xNum;
            MathFunction.p = MathFunction.thermalDiffusivity / MathFunction.deltaX * MathFunction.deltaX;

            initialTem = CreateVector.Dense<double>(MathFunction.xNum, 35);
            

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
        private Vector<double> createVectorTem()
        {
            Vector<double> temVector = CreateVector.Dense<double>(MathFunction.xNum, i =>
            {
                if (i == 0)
                {
                    return -(MathFunction.p * MathFunction.theta + MathFunction.beta);
                }
                if (i == 1)
                {
                    return (MathFunction.p * MathFunction.theta - MathFunction.gamma);
                }
                else
                {
                    return 0;
                }
            });
            return temVector;
        }

        
        private Vector<double> GetResult(Matrix<double> varMatrix,Vector<double> varTem,int count)
        {
            Vector<double> varResult;
            Vector<double> vectorTem = createVectorTem().Multiply(varMatrix.Column(4)[count]);

            Vector<double> vectorPower = CreateVector.Dense<double>(MathFunction.xNum, i =>
            {
                if (i == 0)
                {
                    return MathFunction.deltaT / (MathFunction.density * MathFunction.specific_heat_capacity *
                                                  MathFunction.deltaX);
                }
                else
                {
                    return 0;
                }
            }).Multiply(varMatrix.Column(0)[count]);
            varResult = varMatrix.Multiply(varTem).Add(vectorTem).Add(vectorPower);
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
            Matrix<double> calMatrix= MathFunction.DeferenceMethods.createMatrix();
            Matrix<double> resultMatrix=CreateMatrix.Dense<double>(1,MathFunction.xNum ) 
            resultMatrix.InsertColumn(resultMatrix.ColumnCount - 1, initialTem);
            for (int i = 0; i < dataNumber; i++)
            {
                Vector<double> resultVector = GetResult(matrix, initialTem, i);
                initialTem = CreateVector.Dense<double>(1, matrix.Column(4)[i+1])
                    .Add((resultVector.SubVector(0, MathFunction.xNum - 1)));
            }
            


            DelimitedWriter.Write("‪test.csv",resultMatrix, ",");
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            MathFunction.beta = System.Convert.ToDouble(comboBox1.SelectedValue.ToString());
            MathFunction.gamma = 0.5 - MathFunction.beta;
            MathFunction.theta= System.Convert.ToDouble(comboBox2.SelectedValue.ToString());
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                MathFunction.density = System.Convert.ToDouble(textBox1.Text);
            }
        }

        private void textBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)13)
            {
                MathFunction.specific_heat_capacity = System.Convert.ToDouble(textBox1.Text);
            }
        }
    }
}
