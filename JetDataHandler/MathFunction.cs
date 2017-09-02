using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;


namespace JetDataHandler
{
    class MathFunction
    {
        public class DeferenceMethods
        {
            public string Methods { get; set; }
            public double Value { get; set; }
            public static Matrix<double> createMatrix(double beta,double theta,int xNum,double deltaX,double thermalDiffusivity,double deltaT,double density,double c)
            {
                double p = thermalDiffusivity / (deltaX * deltaX);
                double gamma = 0.5 - beta;
                double theta_comma = 1 - theta;

                double nominal = deltaT / (density * c * deltaX);

                double polyNominal_1 = p * theta + beta;
                double polyNominal_2 = -(p * theta - gamma);
                double polyNominal_3 = 2 * (p * theta + beta);

                double polyNominal_4= beta-p*theta_comma;
                double polyNominal_5 = gamma+p*theta_comma;
                double polyNominal_6 = 2*(beta - p * theta_comma);
                Matrix<double> backTemMatrix = CreateMatrix.Dense<double>(xNum, xNum, (i, j) =>
                 {
                     if (i==j)
                     {
                         if (i==xNum)
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
                             if (i==xNum)
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
                Matrix<double> frontMatrix=CreateMatrix.Dense<double>(xNum, xNum, (i, j) =>
                {
                    if (i == j)
                    {
                        if (i == 1 || i == xNum)
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
                        if (Math.Abs(i - j) == 1)
                        {
                            return polyNominal_5;
                        }
                        else
                        {
                            return 0;
                        }
                    }
                });
                return backTemMatrix;
            }
        }

    }

}
