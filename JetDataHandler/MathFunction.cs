using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.Data.Text;


namespace JetDataHandler
{
    class MathFunction
    {
        public static double p{ get; set; }
        public static double beta { get; set; }
        public static double theta { get; set; }
        public static double thermalDiffusivity { get; set; }
        public static double deltaX { get; set; }
        public static double density { get; set; }
        public static double specific_heat_capacity { get; set; }
        public static double gamma { get; set; }
        public static double deltaT { get; set; }
        public static int xNum { get; set; }
        public static int dataNumber { get; set; }

        public class DeferenceMethods
        {
            public string Methods { get; set; }
            public double Value { get; set; }
            public static Matrix<double> createInverseMatrix()
            {
                p = thermalDiffusivity / (deltaX * deltaX);
                gamma = 0.5 - beta;
                double nominal = deltaT*10000 / (density * specific_heat_capacity * deltaX);

                double polyNominal_1 = p * theta + beta;
                double polyNominal_2 = -(p * theta - gamma);
                double polyNominal_3 = 2 * (p * theta + beta);

                Matrix<double> backTemMatrix = CreateMatrix.Dense<double>(xNum, xNum, (i, j) =>
                 {
                     if (i==j)
                     {
                         if (i==xNum-1)
                         {
                             return nominal;
                         }
                         else
                         {
                             return polyNominal_2;
                         }
                     }
                     else
                     {
                         if ((i-j)==1)
                         {
                             if (i==xNum-1)
                             {
                                 return polyNominal_1;
                             }
                             else
                             {
                                 return polyNominal_3;
                             }
                             
                         }
                         if ((i-j)==2)
                         {
                             return polyNominal_2;
                         }
                         else
                         {
                             return 0;
                         }
                     }
                 });

  
                DelimitedWriter.Write("‪backMatrix.csv", backTemMatrix, ",");
                return backTemMatrix.Inverse();
            }

            public static Matrix<double> CreateTemMatrix()
            {
                p = thermalDiffusivity / (deltaX * deltaX);
                gamma = 0.5 - beta;
                double theta_comma = 1 - theta;

                double polyNominal_4 = beta - p * theta_comma;
                double polyNominal_5 = gamma + p * theta_comma;
                double polyNominal_6 = 2 * (beta - p * theta_comma);
                Matrix<double> frontMatrix = CreateMatrix.Dense<double>(xNum, xNum, (i, j) =>
                {
                    if (i == j)
                    {
                        if (i == 0 || i == xNum - 1)
                        {
                            return polyNominal_4;
                        }
                        else
                        {
                            return polyNominal_6;
                        }
                    }
                    else
                    {
                        switch (Math.Abs(i - j))
                        {
                            case 1:
                                return polyNominal_5;
                            default:
                                return 0;
                        }
                    }
                });
                DelimitedWriter.Write("‪frontMatrix.csv", frontMatrix, ",");

                return frontMatrix;
            }
        }

    }

}
