using LiveCharts;
using LiveCharts.Wpf;
using LiveCharts.WinForms;
using LiveCharts.Defaults;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsAppChart
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            // Define the pump curve data
            var pumpCurveData = new List<Tuple<double, double>>();
            //pumpCurveData.Add(Tuple.Create(10.0, 20.0));
            //pumpCurveData.Add(Tuple.Create(20.0, 15.0));
            //pumpCurveData.Add(Tuple.Create(30.0, 10.0));
            //pumpCurveData.Add(Tuple.Create(40.0, 5.0));

            pumpCurveData.Add(Tuple.Create(0.0, 2.0));
            pumpCurveData.Add(Tuple.Create(1.0, 1.5));
            pumpCurveData.Add(Tuple.Create(2.0, 0.0));

            // Fit the polynomial and get the coefficients
            int degree = 2;
            double[] coefficients = LeastSquares(pumpCurveData, degree);

            Console.WriteLine(coefficients[0]);
            Console.WriteLine(coefficients[1]);
            Console.WriteLine(coefficients[2]);

            // Calculate the sum of squared errors
            double sumOfSquaredErrors = 0.0;
            foreach (var point in pumpCurveData)
            {
                double fittedValue = PredictPolynomial(point.Item1, coefficients);
                double error = Math.Pow(point.Item2 - fittedValue, 2);
                sumOfSquaredErrors += error;
            }

            // Print the sum of squared errors
            Console.WriteLine("최소제곱값 : " + sumOfSquaredErrors);




            this.cartesianChart1 = new LiveCharts.WinForms.CartesianChart();
            // 테스트 데이터
            //this.cartesianChart1.Series = new SeriesCollection
            //{
            //    new LineSeries
            //    {
            //        Title = "Series 1",
            //        Values = new ChartValues<double> { 1, 1, 2, 3, 5 }
            //    }
            //};

            // 차트 그리는 부분
            this.cartesianChart1.Series = new SeriesCollection
            {
                new LineSeries
                {
                    Title = "Pump Curve",
                    Values = new ChartValues<ObservablePoint>(pumpCurveData.Select(point => new ObservablePoint(point.Item1, point.Item2))),
                    PointGeometrySize = 10
                },
                new LineSeries
                {
                    Title = "Fitted Curve",
                    Values = new ChartValues<ObservablePoint>(pumpCurveData.Select(point => new ObservablePoint(point.Item1, PredictPolynomial(point.Item1, coefficients)))),
                    PointGeometrySize = 10
                }
            };

            // Set the chart labels
            this.cartesianChart1.AxisX.Add(new Axis { Title = "Flow" });
            this.cartesianChart1.AxisY.Add(new Axis { Title = "Head" });

            // Show the chart
            //var form = new Form();
            //form.Text = "Pump Curve Chart";
            //form.Controls.Add(chart);
            //form.Width = 800;
            //form.Height = 600;
            //Application.Run(form);



            ///////////////////////////////////////////////////

            this.SuspendLayout();
            // 
            // cartesianChart1
            // 
            this.cartesianChart1.Location = new System.Drawing.Point(202, 170);
            this.cartesianChart1.Name = "cartesianChart1";
            this.cartesianChart1.Size = new System.Drawing.Size(1400, 809);
            this.cartesianChart1.TabIndex = 0;
            this.cartesianChart1.Text = "cartesianChart1";
            this.cartesianChart1.ChildChanged += new System.EventHandler<System.Windows.Forms.Integration.ChildChangedEventArgs>(this.cartesianChart1_ChildChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(11F, 21F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1875, 1075);
            this.Controls.Add(this.cartesianChart1);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private LiveCharts.WinForms.CartesianChart cartesianChart1;


        static double[] LeastSquares(List<Tuple<double, double>> data, int degree)
        {

            //return solution;
            // Create the coefficient matrix
            var coefficients = new double[degree + 1][];
            for (int i = 0; i <= degree; i++)
            {
                coefficients[i] = new double[degree + 1];
                for (int j = 0; j <= degree; j++)
                {
                    coefficients[i][j] = data.Sum(d => Math.Pow(d.Item1, i + j));
                    //Console.WriteLine(coefficients[i][j]);
                }
            }

            // Create the constant vector
            var constants = new double[degree + 1];
            for (int i = 0; i <= degree; i++)
            {
                constants[i] = data.Sum(d => d.Item2 * Math.Pow(d.Item1, i));
                //Console.WriteLine("constants : " + constants[i]);
            }

            // Solve the linear system using LinearSolver
            var solution = LinearSolver.Solve(coefficients, constants);

            return solution;
        }


        public static double PredictPolynomial(double x, double[] coefficients)
        {
            double result = 0.0;
            for (int i = 0; i < coefficients.Length; i++)
            {
                result += coefficients[i] * Math.Pow(x, i);
            }
            return result;
        }

        public static class LinearSolver
        {
            public static double[] Solve(double[][] coefficients, double[] constants)
            {
                int n = coefficients.Length;
                double[][] augmentedMatrix = new double[n][];

                // Create an augmented matrix [coefficients | constants]
                for (int i = 0; i < n; i++)
                {
                    augmentedMatrix[i] = new double[n + 1];
                    for (int j = 0; j < n; j++)
                    {
                        augmentedMatrix[i][j] = coefficients[i][j];
                    }
                    augmentedMatrix[i][n] = constants[i];
                }

                // Perform Gauss-Jordan elimination
                for (int i = 0; i < n; i++)
                {
                    // Find the pivot row
                    int pivotRow = i;
                    for (int j = i + 1; j < n; j++)
                    {
                        if (Math.Abs(augmentedMatrix[j][i]) > Math.Abs(augmentedMatrix[pivotRow][i]))
                        {
                            pivotRow = j;
                        }
                    }

                    // Swap pivot row with the current row
                    double[] temp = augmentedMatrix[i];
                    augmentedMatrix[i] = augmentedMatrix[pivotRow];
                    augmentedMatrix[pivotRow] = temp;

                    // Normalize the pivot row
                    double pivotElement = augmentedMatrix[i][i];
                    for (int j = i; j <= n; j++)
                    {
                        augmentedMatrix[i][j] /= pivotElement;
                    }

                    // Eliminate the other rows
                    for (int j = 0; j < n; j++)
                    {
                        if (j != i)
                        {
                            double factor = augmentedMatrix[j][i];
                            for (int k = i; k <= n; k++)
                            {
                                augmentedMatrix[j][k] -= factor * augmentedMatrix[i][k];
                            }
                        }
                    }
                }

                // Extract the solution vector from the augmented matrix
                double[] solution = new double[n];
                for (int i = 0; i < n; i++)
                {
                    solution[i] = augmentedMatrix[i][n];
                }

                return solution;
            }
        }


    }
}

