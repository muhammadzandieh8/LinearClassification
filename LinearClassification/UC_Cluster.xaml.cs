using System;
using System.IO;
using System.Windows;
using System.Linq;
using System.Threading;
using System.Windows.Media;
using System.Windows.Controls;
using System.Text.RegularExpressions;
using System.Collections.ObjectModel;
using SciChart.Charting.Model.DataSeries;
using SciChart.Examples.ExternalDependencies.Data;

namespace LinearClassifiation
{
    /// <summary>
    /// Interaction logic for ScatterChartExampleView.xaml
    /// </summary>
    public partial class UC_Cluster : UserControl
    {
        #region Parameter
        public class Weights_cs
        {
            public int RowNumber { get; set; }
            public double Weight0 { get; set; }
            public double Weight1 { get; set; }
        }
        public class a_cs
        {
            public int RowNumber { get; set; }
            public double A { get; set; }
        }
        public class ValuesXY_cs
        {
            public int RowNumber { get; set; }
            public double ValueX { get; set; }
            public double ValueY { get; set; }
        }
        public class SummaryLog
        {
            public int RowNumber { get; set; }
            public String WeightState { get; set; }
            public double T { get; set; }
            public double A { get; set; }
            public double ValueX { get; set; }
            public double ValueY { get; set; }
            public double X_VerticalLine { get; set; }
            public double Y_VerticalLine { get; set; }
            public double Weight0 { get; set; }
            public double Weight1 { get; set; }
            public int Bias { get; set; }
            public int Error { get; set; }
        }

        public static XyDataSeries<double, double> dataSeries = new XyDataSeries<double, double>();
        public static XyDataSeries<double, double> dataSeries_a = new XyDataSeries<double, double>();
        public static XyDataSeries<double, double> dataSeries_Weights = new XyDataSeries<double, double>();
        //ObservableCollection
        //DataGrid_Values
        ObservableCollection<Weights_cs> Weights_ar = new ObservableCollection<Weights_cs>();
        ObservableCollection<a_cs> A_ar = new ObservableCollection<a_cs>();
        ObservableCollection<ValuesXY_cs> ValuesXY_ar = new ObservableCollection<ValuesXY_cs>();
        ObservableCollection<SummaryLog> Summary_ar = new ObservableCollection<SummaryLog>();
        double[] Xvector = new double[200];
        double[] Yvector = new double[200];
        int[] Tvector = new int[200];
        double[] Distancevector;

        //indx of inserted to data
        int[] IdxData = new int[200];
        int[] a_array = new int[200];

        public static double[] Weight = new double[2];
        public static double[] MinVector = new double[2];
        public static double[] MaxVector = new double[2];
        //Weight[X,Y]

        double[] XTrainArray = new double[200];
        double[] YTrainArray = new double[200];
        StreamWriter streamWriter = new StreamWriter(System.IO.Directory.GetCurrentDirectory() + "/Log.txt");
        #endregion
        #region UserControl
        public UC_Cluster()
        {
            InitializeComponent();
            txtbox_Guassian1_X_StdDev.Text = "5";
            txtbox_Guassian1_Y_StdDev.Text = "5";
            txtbox_Guassian1_X_Mean.Text = "100";
            txtbox_Guassian1_Y_Mean.Text = "100";

            txtbox_Guassian2_X_StdDev.Text = "5";
            txtbox_Guassian2_Y_StdDev.Text = "5";
            txtbox_Guassian2_X_Mean.Text = "50";
            txtbox_Guassian2_Y_Mean.Text = "50";

            for (int i = 0; i < 200; i++)
            {
                IdxData[i] = i;
            }


            datagrid_Weights.ItemsSource = Weights_ar;
            datagrid_As.ItemsSource = A_ar;
            DataGrid_Values.ItemsSource = ValuesXY_ar;
            DataGrid_Summary.ItemsSource = Summary_ar;
        }
        private void UC_Cluster_Loaded(object sender, System.Windows.RoutedEventArgs e)
        {

            dataSeries.AcceptsUnsortedData = true;

            var dataSeries2 = new XyDataSeries<double, double>();
            #region dataSeries Two
            //dataSeries Two

            dataSeries2.AcceptsUnsortedData = true;
            PointerColor2.Fill = Colors.Blue;
            for (int i = 0; i < 100; i++)
            {
                var datax = DataManager.Instance.GetGaussianRandomNumber(50, 05);
                var datay = DataManager.Instance.GetGaussianRandomNumber(50, 05);
                dataSeries2.Append(datax, datay);
                dataSeries.Append(datax, datay);

                Xvector[i] = datax;
                Yvector[i] = datay;
                Tvector[i] = 0;
            }
            scatterRenderSeries2.DataSeries = dataSeries2;
            #endregion
            #region DataSeries One
            var dataSeries1 = new XyDataSeries<double, double>();
            dataSeries1.AcceptsUnsortedData = true;

            PointerColor.Fill = Colors.Red;
            for (int i = 0; i < 100; i++)
            {
                var datax = DataManager.Instance.GetGaussianRandomNumber(100, 05);
                var datay = DataManager.Instance.GetGaussianRandomNumber(100, 05);
                dataSeries1.Append(datax, datay);
                dataSeries.Append(datax, datay);
                Xvector[i + 100] = datax;
                Yvector[i + 100] = datay;
                Tvector[i + 100] = 1;

            }

            //MessageBox.Show(dataSeries1.XValues[0].ToString()+ "\n" + Xvector[100].ToString());


            // Append data to series. SciChart automatically redraws
            scatterRenderSeries.DataSeries = dataSeries1;
            #endregion
            sciChart.ZoomExtents();

        }
        #endregion
        #region Sample
        private void Btn_Guassian1_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_Guassian1_X_Mean.Text != "" && txtbox_Guassian1_Y_Mean.Text != "" && txtbox_Guassian1_X_StdDev.Text != "" && txtbox_Guassian1_Y_StdDev.Text != "" && txtbox_Guassian1_Count.Text != "")
            {
                dataSeries.Clear();
                #region DataSeries One
                var dataSeries1 = new XyDataSeries<double, double>
                {
                    AcceptsUnsortedData = true
                };
                PointerColor.Fill = Colors.Red;
                for (int i = 0; i < Convert.ToInt32(txtbox_Guassian1_Count.Text); i++)
                {


                    var datax = DataManager.Instance.GetGaussianRandomNumber(Convert.ToInt32(txtbox_Guassian1_X_Mean.Text), Convert.ToInt32(txtbox_Guassian1_X_StdDev.Text));
                    var datay = DataManager.Instance.GetGaussianRandomNumber(Convert.ToInt32(txtbox_Guassian1_Y_Mean.Text), Convert.ToInt32(txtbox_Guassian1_Y_StdDev.Text));
                    dataSeries1.Append(datax, datay);
                    dataSeries.Append(datax, datay);
                    Xvector[i + 100] = datax;
                    Yvector[i + 100] = datay;
                    Tvector[i + 100] = 1;

                }

                //MessageBox.Show(dataSeries1.XValues[0].ToString()+ "\n" + Xvector[100].ToString());


                // Append data to series. SciChart automatically redraws
                scatterRenderSeries.DataSeries.Clear();
                scatterRenderSeries.DataSeries = dataSeries1;

                #endregion
                Snackbar_Guassian1.MessageQueue?.Enqueue($"Your Data Appiled!!!", null, null, null, false, true, TimeSpan.FromSeconds(2));
            }
            else
            {
                Snackbar_Guassian1.MessageQueue?.Enqueue($"Fill Correctly...", null, null, null, false, true, TimeSpan.FromSeconds(2));
            }
        }
        private void Btn_Guassian2_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_Guassian2_X_Mean.Text != "" && txtbox_Guassian2_Y_Mean.Text != "" && txtbox_Guassian2_X_StdDev.Text != "" && txtbox_Guassian2_Y_StdDev.Text != "" && txtbox_Guassian2_Count.Text != "")
            {
                #region DataSeries One
                var dataSeries2 = new XyDataSeries<double, double>
                {
                    AcceptsUnsortedData = true
                };
                PointerColor2.Fill = Colors.Blue;
                for (int i = 0; i < Convert.ToInt32(txtbox_Guassian2_Count.Text); i++)
                {
                    var datax = DataManager.Instance.GetGaussianRandomNumber(Convert.ToInt32(txtbox_Guassian2_X_Mean.Text), Convert.ToInt32(txtbox_Guassian2_X_StdDev.Text));
                    var datay = DataManager.Instance.GetGaussianRandomNumber(Convert.ToInt32(txtbox_Guassian2_Y_Mean.Text), Convert.ToInt32(txtbox_Guassian2_Y_StdDev.Text));
                    dataSeries2.Append(datax, datay);
                    dataSeries.Append(datax, datay);

                    Xvector[i] = datax;
                    Yvector[i] = datay;
                    Tvector[i] = 0;

                }

                //MessageBox.Show(dataSeries1.XValues[0].ToString()+ "\n" + Xvector[100].ToString());


                // Append data to series. SciChart automatically redraws
                scatterRenderSeries2.DataSeries.Clear();
                scatterRenderSeries2.DataSeries = dataSeries2;
                #endregion
                Snackbar_Guassian2.MessageQueue?.Enqueue($"Your Data Appiled!!!", null, null, null, false, true, TimeSpan.FromSeconds(2));

            }
            else
            {
                Snackbar_Guassian2.MessageQueue?.Enqueue($"Fill Correctly...", null, null, null, false, true, TimeSpan.FromSeconds(2));
            }
        }
        #endregion
        #region Train
        double a;
        //represent baias;
        int b;
        string lbl_Weights = "";
        // lOW High OK
        string tempstate;
        //Slop of  Line
        double m;
        //The coordinates of the point on which the input data is perpendicular to the line
        double X_VerticalLine;
        double Y_VerticalLine;

        int Error;
        public void Train(double[] _Weight, double _X, double _Y, int _T)
        {
            int e;
            #region Calculate A 
            //a = (_Weight[0] * _X) + (_Weight[1] * _Y) + b;

            m = -(_Weight[1] / _Weight[0]);
            X_VerticalLine = (1 / m * _X + _Y - _Weight[1] + b) / (m + 1 / m);
            Y_VerticalLine = m * X_VerticalLine + _Weight[1] + b;

            //a = (_Weight[0] * _X) + (_Weight[1] * _Y) ;

            //Actually a means Y
            a = m * _X + _Weight[1] - _Y + b;



            double Distance_VerticalLine = X_VerticalLine * X_VerticalLine + Y_VerticalLine * Y_VerticalLine;
            double r1 = _X * _X + _Y * _Y;
            double rW = (_X - X_VerticalLine) * (_X - X_VerticalLine) + (_Y - Y_VerticalLine) * (_Y - Y_VerticalLine);


            if (Distance_VerticalLine > r1)
            {
                a = 0;
            }
            else if (Distance_VerticalLine < r1)
            {
                a = 1;
            }
            else
            {
                a = 0;
            }
            #region Activation Function

            //if (a > 0)
            //{
            //    a = 1;
            //}
            //else if (a <= 0)
            //{
            //    a = 0;
            //}
            //else
            //{
            //    // do noting
            //}

            #endregion
            #endregion
            #region E base 

            e = _T - (int)a;
            b += e;
            Weight[0] += (e * _X);
            Weight[1] += (e * _Y);
            if (e == 1)
            {
                tempstate = "Low";
                Error++;
            }
            else if (e == -1)
            {
                tempstate = "High";
                Error++;
            }
            else
            {
                tempstate = "OK";
                //if (rW > r1)
                //{
                //    Weight[0] *= -1;
                //    Weight[1] *= -1;
                //}
            }

            #region Handle 1 by 1
            //Must Edit
            //if (Line_X1 > _X || Line_X1 <= _X)
            //{
            //    Weight[0] += (e * Math.Abs(_X - Line_X1));
            //}
            //else if (Line_Y1 > _Y || Line_Y1 <= _Y)
            //{
            //    Weight[1] += (e * Math.Abs(_Y - Line_Y1));
            //}
            #endregion





            #region Handle With T & A
            //if (_T == 0 && a == 1)
            //{
            //    tempstate = "High";
            //    Weight[0] -= _X;
            //    Weight[1] -= _Y;
            //}
            //else if (_T == 1 && a == 0)
            //{
            //    tempstate = "Low";
            //    Weight[0] += _X;
            //    Weight[1] += _Y;
            //}
            //else
            //{
            //    tempstate = "OK";
            //    //T = a
            //    //Do Nothing
            //}
            #endregion
            #endregion
            #region Log
            string temp =
              "X:    " + _X.ToString("000.00").PadRight(10) +
              "Y:    " + _Y.ToString("000.00").PadRight(10) +
              "T:    " + _T.ToString("+0;-#").PadRight(05) +
              "A:    " + ((int)a).ToString("+0;-#").PadRight(05) +
              "W1:   " + _Weight[0].ToString("000.00").PadRight(10) +
              "W2:   " + _Weight[1].ToString("000.00").PadRight(10) +
              "State:" + tempstate.ToString().PadRight(10)
              ;
            streamWriter.WriteLine(temp);

            #endregion
        }
        public void PerMutate()
        {
            Random random = new Random();
            for (int i = 0; i < 200; i++)
            {
                int temp = IdxData[i];
                int tempidx = random.Next(0, 199);
                IdxData[i] = IdxData[tempidx];
                IdxData[tempidx] = temp;
            }
        }

        public void Epoche()
        {
            PerMutate();
            #region init Params
            string temp_a = "";
            lbl_Weights = "";
            Weights_cs _Weights_Cs = new Weights_cs();
            a_cs _a_Cs = new a_cs();
            ValuesXY_cs _valuesXY_Cs = new ValuesXY_cs();
            SummaryLog _summaryLog = new SummaryLog();
            #region Clear DataGrid & DataSeries
            A_ar.Clear();
            Weights_ar.Clear();
            ValuesXY_ar.Clear();
            dataSeries_a.Clear();
            dataSeries_Weights.Clear();
            Summary_ar.Clear();
            #endregion
            dataSeries_Weights.AcceptsUnsortedData = true;
            b = Convert.ToInt32(txtbox_bias.Text);
            Error = 0;

            Distancevector = new double[(int)((Convert.ToInt32(txtbox_Guassian1_Count.Text) + Convert.ToInt32(txtbox_Guassian2_Count.Text)) * Convert.ToUInt32(txtbox_TrainRate.Text)) / 100];
            for (int i = 0; i < (int)((Convert.ToInt32(txtbox_Guassian1_Count.Text) + Convert.ToInt32(txtbox_Guassian2_Count.Text)) * Convert.ToUInt32(txtbox_TrainRate.Text)) / 100; i++)
            {
                Distancevector[i] = Xvector[i] * Xvector[i] + Yvector[i] * Yvector[i];

            }


            #region Summary
            _summaryLog = new SummaryLog
            {
                RowNumber = -1,
                ValueX = 0,
                ValueY = 0,
                T = 0,
                A = 0,
                Weight0 = Weight[0],
                Weight1 = Weight[1],
                Bias = b,
                WeightState = "Init",
                X_VerticalLine = 0,
                Y_VerticalLine = 0
            };
            Summary_ar.Add(_summaryLog);
            #endregion
            #endregion
            for (int i = 0; i < (int)((Convert.ToInt32(txtbox_Guassian1_Count.Text) + Convert.ToInt32(txtbox_Guassian2_Count.Text)) * Convert.ToUInt32(txtbox_TrainRate.Text)) / 100; i++)
            {
                Train(Weight, Xvector[IdxData[i]], Yvector[IdxData[i]], Tvector[IdxData[i]]);

                #region Show Result
                #region Log
                //string temp =
                //              "i: " +  i.ToString("000").PadRight(04) +
                //              "Value[X]:   " + dataSeries.XValues[IdxData[i]].ToString("###.00").PadRight(10) +
                //              "Value[Y]:   " + dataSeries.YValues[IdxData[i]].ToString("###.00").PadRight(10) +
                //              "T:          " + Tvector[IdxData[i]].ToString("+0;-#").PadRight(05)+
                //              "A:          " + ((int)a).ToString("+0;-#").PadRight(05)+ 
                //              "Weight[0]:  " + Weight[0].ToString("###.00").PadRight(10) + 
                //              "Weight[1]:  " + Weight[1].ToString("###.00").PadRight(10) 
                //              ;
                //streamWriter.WriteLine(temp);
                #endregion

                #region Values
                _valuesXY_Cs = new ValuesXY_cs();
                _valuesXY_Cs.RowNumber = i;
                _valuesXY_Cs.ValueX = Xvector[IdxData[i]];
                _valuesXY_Cs.ValueY = Yvector[IdxData[i]];
                ValuesXY_ar.Add(_valuesXY_Cs);
                #endregion

                #region A
                a_array[i] = (int)a;
                temp_a += "indx:    " + i.ToString("000") + "    " + a.ToString() + "\n";
                _a_Cs = new a_cs
                {
                    RowNumber = i,
                    A = a
                };
                A_ar.Add(_a_Cs);
                dataSeries_a.Append(_a_Cs.RowNumber, _a_Cs.A);
                #endregion

                #region Weights
                lbl_Weights += "Weight[0]:  " + Weight[0].ToString("###.00") + "    Weight[1]: " + Weight[1].ToString("###.00") + "\n";
                lbl_Weight0.Content = Weight[0].ToString("###.00");
                lbl_Weight1.Content = Weight[1].ToString("###.00");
                txtbox_bias.Text = b.ToString();

                _Weights_Cs = new Weights_cs
                {
                    RowNumber = i,
                    Weight0 = Weight[0],
                    Weight1 = Weight[1]
                };
                Weights_ar.Add(_Weights_Cs);

                dataSeries_Weights.Append(Weight[0], Weight[1]);
                #endregion

                lbl_Error.Content = Error.ToString();
                #region Summary
                _summaryLog = new SummaryLog
                {
                    RowNumber = i,
                    ValueX = Math.Round(Xvector[IdxData[i]], 2),
                    ValueY = Math.Round(Yvector[IdxData[i]], 2),
                    T = Tvector[IdxData[i]],
                    A = a,
                    Weight0 = Math.Round(Weight[0], 2),
                    Weight1 = Math.Round(Weight[1], 2),
                    Bias = b,
                    WeightState = tempstate,
                    X_VerticalLine = Math.Round(X_VerticalLine, 2),
                    Y_VerticalLine = Math.Round(Y_VerticalLine, 2),
                    Error = Error
                };
                Summary_ar.Add(_summaryLog);
                #endregion

                #endregion
            }

            #region OutPut (Weight,Line,a)

            double Guassian1_X = Convert.ToDouble(txtbox_Guassian1_X_Mean.Text);
            double Guassian1_Y = Convert.ToDouble(txtbox_Guassian1_Y_Mean.Text);

            double Guassian2_X = Convert.ToDouble(txtbox_Guassian2_X_Mean.Text);
            double Guassian2_Y = Convert.ToDouble(txtbox_Guassian2_Y_Mean.Text);


            double m = -(Weight[1] / Weight[0]);
            double X = (Guassian2_Y - Weight[1]) / (m - ((Guassian2_Y - Guassian1_Y) / (Guassian2_X - Guassian1_X)));
            double Y = m * X + Weight[1];


            int Minindex = Array.FindIndex(Distancevector, s => s == Distancevector.Min());
            int Maxindex = Array.FindIndex(Distancevector, s => s == Distancevector.Max());
            double minvalue = Distancevector.Min();
            double maxvalue = Distancevector.Max();

            if (!(X > Xvector[Minindex] && X < Xvector[Maxindex] && Y > Yvector[Minindex] && Y < Yvector[Maxindex]))
            {
                Weight[0] *= -1;
                Weight[1] *= -1;
                Snackbar_Train.MessageQueue?.Enqueue($"Local answer.", null, null, null, false, true, TimeSpan.FromSeconds(2));

            }
            //if (!(X > Xvector.Min() && X < Xvector.Max() && Y > Yvector.Min() && Y < Yvector.Max()))
            //{
            //    Weight[0] *= -1;
            //    Weight[1] *= -1;
            //    MessageBox.Show("Error");
            //}

            DrawLine((Weight[0] - 10000), m * (Weight[0] - 10000) + Weight[1], (Weight[0] + 10000), m * (Weight[0] + 10000) + Weight[1]);
            lineRenderSeries_Weights.DataSeries = dataSeries_Weights;
            lineRenderSeries_a.DataSeries = dataSeries_a;
            txt_a.Text = temp_a;
            txt_Weights.Text = lbl_Weights;
            txtbox_W1.Text = ((int)Weight[0]).ToString();
            txtbox_W2.Text = ((int)Weight[1]).ToString();
            #endregion
        }
        private void btn_StepByStep_Click(object sender, RoutedEventArgs e)
        {
            if (radio_Random.IsChecked == true)
            {
                radio_Random_Click(null, null);
                Epoche();

            }
            else if (radio_User.IsChecked == true)
            {
                if (txtbox_W1.Text != "" && txtbox_W2.Text != "")
                {
                    radio_User_Click(null, null);
                    Epoche();
                }
            }
            else if (radio_FirstInputData.IsChecked == true)
            {
                radio_FirstInputData_Click(null, null);
                Epoche();
            }

        }

        public void Func_Automatic()
        {
            for (int i = 0; i < Convert.ToInt32(btn_EpockeNumber.Text); i++)
            {
                Epoche();
                Thread.Sleep(1000);
            }
        }
        private void btn_Automatic_Click(object sender, RoutedEventArgs e)
        {
            if (btn_EpockeNumber.Text != "")
            {
                Func_Automatic();
            }
            else
            {
                MessageBox.Show("Fill Epocke Number...");
            }
        }
        private void radio_Random_Click(object sender, RoutedEventArgs e)
        {
            Random random = new Random();
            Weight[0] = random.Next((int)Xvector.Min(), (int)Xvector.Max());
            Weight[1] = random.Next((int)Yvector.Min(), (int)Yvector.Max());
            txtbox_bias.Text = "0";
        }
        private void radio_User_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_W1.Text != "" && txtbox_W2.Text != "")
            {
                Weight[0] = Convert.ToDouble(txtbox_W1.Text.ToString());
                Weight[1] = Convert.ToDouble(txtbox_W2.Text.ToString());

            }
            else
            {
                MessageBox.Show("Fill Weights...");
            }
        }
        private void radio_FirstInputData_Click(object sender, RoutedEventArgs e)
        {
            Weight[0] = Xvector[IdxData[0]];
            Weight[1] = Yvector[IdxData[0]];
            txtbox_bias.Text = "0";
        }
        private void PreviewTextInputNumber(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            Regex regex = new Regex("-[^0-9]+");
            e.Handled = regex.IsMatch(e.Text);
        }
        #endregion
        #region Line
        public void DrawLine(double _X1, double _Y1, double _X2, double _Y2)
        {
            LineAnnotion.X1 = (int)_X1;
            LineAnnotion.Y1 = (int)_Y1;

            LineAnnotion.X2 = (int)_X2;
            LineAnnotion.Y2 = (int)_Y2;
        }
        private void btn_DrawLine_Click(object sender, RoutedEventArgs e)
        {
            if (txtbox_X1.Text != "" && txtbox_Y1.Text != "" && txtbox_X2.Text != "" && txtbox_Y2.Text != "")
            {
                double _X1 = Convert.ToDouble(txtbox_X1.Text);
                double _Y1 = Convert.ToDouble(txtbox_Y1.Text);

                double _X2 = Convert.ToDouble(txtbox_X2.Text);
                double _Y2 = Convert.ToDouble(txtbox_Y2.Text);

                DrawLine(_X1, _Y1, _X2, _Y2);
            }
            else
            {
                MessageBox.Show("Fill Points");
            }
        }
        #endregion
        #region Test 
        private void btn_Test_Click(object sender, RoutedEventArgs e)
        {
            int stop = (Convert.ToInt32(txtbox_Guassian1_Count.Text) + Convert.ToInt32(txtbox_Guassian2_Count.Text));
            int start = stop - ((int)((Convert.ToInt32(txtbox_Guassian1_Count.Text) + Convert.ToInt32(txtbox_Guassian2_Count.Text)) * Convert.ToUInt32(txtbox_TestRate.Text)) / 100);
            for (int i = start; i < stop; i++)
            {
                Test(Weight, Xvector[IdxData[i]], Yvector[IdxData[i]]);

            }
        }
        public void Test(double[] _Weight, double _X, double _Y)
        {
            m = -(_Weight[1] / _Weight[0]);
            X_VerticalLine = (1 / m * _X + _Y - _Weight[1]) / (m + 1 / m);
            Y_VerticalLine = m * X_VerticalLine + _Weight[1];

            //a = (_Weight[0] * _X) + (_Weight[1] * _Y) ;
            a = m * _X + _Weight[1] - _Y + b;



            double r_Line = X_VerticalLine * X_VerticalLine + Y_VerticalLine * Y_VerticalLine;
            double r1 = _X * _X + _Y * _Y;
            double rW = (_X - X_VerticalLine) * (_X - X_VerticalLine) + (_Y - Y_VerticalLine) * (_Y - Y_VerticalLine);


            if (r_Line > r1)
            {
                a = 0;
            }
            else if (r_Line < r1)
            {
                a = 1;
            }
            else
            {
                a = 0;
            }
        }
        #endregion

    }
}