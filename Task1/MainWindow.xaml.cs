using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows;
using Microsoft.Office.Interop.Excel;
using Microsoft.Win32;
using Application = Microsoft.Office.Interop.Excel.Application;
using DataTable = System.Data.DataTable;

namespace Task1
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private Core _core;

        private Thread _t;

        private ExcelData _data;

        private Loading _loading = new Loading();

        private bool _dataIsReady;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Right_Button_Click(object sender, RoutedEventArgs e)
        {
            TabControl.SelectedIndex++;
        }

        private void Left_Button_Click(object sender, RoutedEventArgs e)
        {
            if (TabControl.SelectedIndex == 0)
            {
                return;
            }
            TabControl.SelectedIndex--;
        }

        private void OpenFile(object sender, RoutedEventArgs e)
        {
            IsHitTestVisible = false;

            var openfile = new OpenFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "(.xlsx)|*.xlsx"
            };
            string filepath;
            var result = openfile.ShowDialog();
            if (result == true)
            {
                filepath = openfile.FileName;
                _dataIsReady = false;
            }
            else
            {
                IsHitTestVisible = true;
                return;
            }

            _t = new Thread(delegate ()
            {
                var excelData = new ExcelData(filepath);
                _data = excelData;
                _dataIsReady = true;
            });
            _t.Start();

            try
            {
                Main();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при обработке данных. Информация: " + ex);
                MainWindow w = new MainWindow();
                w.Show();
                Close();
            }

        }

        private void Exit(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void Main()
        {
            _loading = new Loading();
            _loading.Show();



            while (true)
            {
                if (_dataIsReady)
                {
                    break;
                }
            }



            DataGrid.DataContext = _data;
            var data = _data.Table;
            var n = data.Rows.Count;
            var t = new int[n];
            for (var i = 0; i < n; i++)
            {
                t[i] = i + 1;
            }
            var y = new double[n];
            for (var i = 0; i < n; i++)
            {
                y[i] = double.Parse(data.Rows[i][1].ToString().Replace('.',','));
            }

            var w = new Size(n);

            w.ShowDialog();
            var size = int.Parse(w.Combobox.Text);

            _core = new Core(t,y,size);

            ShowSlides();
            ShowSeason();
            ShowTrand();
            ShowRandom();
            ShowGraph(_core.T,_core.Random.T, _core.T,_core.Y);
            _loading.Close();
            IsHitTestVisible = true;
        }

        private void ShowSlides()
        {
            var n = _core.Count;

            var sc = _core.SeasonComponent;
            var scSingle = new SeasonComponentSingle[n];

            var table = new DataTable();
            table.Columns.Add(new DataColumn("t"));
            table.Columns.Add(new DataColumn("Y"));
            table.Columns.Add(new DataColumn("Всего за " + _core.Size + " промежутков"));
            table.Columns.Add(new DataColumn("Скользящая средняя"));
            table.Columns.Add(new DataColumn("Центрированная скользящая"));
            table.Columns.Add(new DataColumn("Оценка сезонной компоненты"));

            int i;
            for (i = 0; i < n; i++)
            {
                scSingle[i] = new SeasonComponentSingle(sc.Value[i],sc.ThreeMonthOverall[i],sc.SlideAverage[i],sc.SlideAverageCenter[i]);
            }

            i = 0;
            var format = "0.00";
            foreach (var s in scSingle)
            {
                var dr = table.NewRow();
                var tmOverall = s.ThreeMonthOverall.ToString(format);
                if (Math.Abs(s.ThreeMonthOverall) < double.Epsilon)
                {
                    tmOverall = "-";
                }

                var sa = s.SlideAverage.ToString(format);
                if (Math.Abs(s.SlideAverage) < double.Epsilon)
                {
                    sa = "-";
                }

                var sac = s.SlideAverageCenter.ToString(format);
                if (Math.Abs(s.SlideAverageCenter) < double.Epsilon)
                {
                    sac = "-";
                }

                var v = s.Value.ToString(format);
                if (Math.Abs(s.Value) < double.Epsilon)
                {
                    v = "-";
                }

                dr.ItemArray = new object[]
                {_core.T[i], _core.Y[i], tmOverall, sa, sac, v};
                table.Rows.Add(dr);
                i++;
            }

            DgSlides.DataContext = table.DefaultView;
        }

        private void ShowSeason()
        {
            var sc = _core.SeasonContext;

            var table = new DataTable();
            table.Columns.Add(new DataColumn("Промежуток времени"));

            for (var j = 0; j < _core.Size; j++)
            {
                table.Columns.Add(new DataColumn((j+1) + " часть промежутка"));
            }

            int i;
            var format = "0.00";
            var k = _core.Count/_core.Size;
            for (i = 0; i < (k)+5; i++)
            {
                var dr = table.NewRow();
                var text = new string[_core.Size];
                if (i < k)
                {
                    for (var j = 0; j < _core.Size; j++)
                    {
                        if (Math.Abs(sc.SeasonComponent[i][j] - default(double)) < double.Epsilon)
                        {
                            text[j] = "-";
                        }
                        else
                        {
                            text[j] = sc.SeasonComponent[i][j].ToString(format);
                        }
                    }

                    var array = new object[_core.Size+1];
                    
                    for (var j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (j == 0)
                        {
                            array[j] = i + 1;
                        }
                        else
                        {
                            array[j] = text[j - 1];
                        }

                    }
                    dr.ItemArray = array;
                }
                else if (i == k)
                {
                    for (var j = 0; j < _core.Size; j++)
                    {
                        text[j] = sc.Overall[j].ToString(format);
                    }

                    var array = new object[_core.Size + 1];
                    for (var j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (j == 0)
                        {
                            array[j] = "Итого за промежуток";
                        }
                        else
                        {
                            array[j] = text[j - 1];
                        }

                    }
                    dr.ItemArray = array;
                }
                else if (i == k+1)
                {
                    for (var j = 0; j < _core.Size; j++)
                    {
                        text[j] = sc.SeasonComponentAverage[j].ToString(format);
                    }

                    var array = new object[_core.Size + 1];
                    for (var j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (j == 0)
                        {
                            array[j] = "Средняя оценка сезонной компоненты";
                        }
                        else
                        {
                            array[j] = text[j - 1];
                        }

                    }
                    dr.ItemArray = array;
                }
                else if (i == k+2)
                {
                    for (var j = 0; j < _core.Size; j++)
                    {
                        text[j] = sc.SeasonComponentCorrected[j].ToString(format);
                    }

                    var array = new object[_core.Size + 1];
                    for (var j = 0; j < dr.ItemArray.Length; j++)
                    {
                        if (j == 0)
                        {
                            array[j] = "Скорректированная сезонная компонента St";
                        }
                        else
                        {
                            array[j] = text[j - 1];
                        }

                    }
                    dr.ItemArray = array;
                }
                else if (i == k+3)
                {
                    dr.ItemArray = new object[] {};
                }
                else if (i == k+4)
                {
                    dr.ItemArray = new object[]
                    {
                        "Корректирующий коэффициент", sc.KCorrecting.ToString(format)
                    };
                }
                table.Rows.Add(dr);
            }
           

            DgSeason.DataContext = table.DefaultView;
        }

        private void ShowTrand()
        {
            var n = _core.Count;

            var trand = _core.Trand;

            var table = new DataTable();
            table.Columns.Add(new DataColumn("-"));
            table.Columns.Add(new DataColumn("t"));
            table.Columns.Add(new DataColumn("Yt"));
            table.Columns.Add(new DataColumn("St"));
            table.Columns.Add(new DataColumn("Yi = Y - St"));
            table.Columns.Add(new DataColumn("ti - tср"));
            table.Columns.Add(new DataColumn("Yi - Yср"));
            table.Columns.Add(new DataColumn("(ti - tср)^2"));
            table.Columns.Add(new DataColumn("Произведение Δt и ΔY"));
            table.Columns.Add(new DataColumn("(Yi - Yср)^2"));


            const string format = "0.00";

            var index = 0;

            for (var i = 0; i < n+5; i++)
            {
                var dr = table.NewRow();

                if (i < n)
                {
                    dr.ItemArray = new object[]
                    {
                        "", _core.T[i], _core.Y[i], _core.SeasonContext.SeasonComponentCorrected[index].ToString(format),
                        trand.Yi[i].ToString(format), trand.Deltat[i], trand.DeltaY[i].ToString(format),
                        trand.DeltatSquare[i], trand.DeltatOndeltaY[i].ToString(format),
                        trand.DeltaYSquare[i].ToString(format)
                    };
                    index++;
                    if (index == _core.SeasonContext.SeasonComponentCorrected.Length)
                    {
                        index = 0;
                    }
                }
                else if (i == n)
                {
                    dr.ItemArray = new object[]
                    {
                        "Сумма ", _core.T.Sum(), "", "", trand.Yi.Sum().ToString(format), "", "", trand.DeltatSquare.Sum().ToString(format),
                        trand.DeltatOndeltaY.Sum().ToString(format), trand.DeltaYSquare.Sum().ToString(format) 
                    };
                }
                else if (i == n +1)
                {
                    dr.ItemArray = new object[]
                    {
                        "Среднее значение ", _core.T.Average(), "", "", trand.Yi.Average().ToString(format), "", "", trand.DeltatSquare.Average().ToString(format),
                        trand.DeltatOndeltaY.Average().ToString(format), trand.DeltaYSquare.Average().ToString(format)
                    };
                }
                else if (i == n + 2)
                {
                    dr.ItemArray = new object[] {};
                }
                else if (i == n + 3)
                {
                    dr.ItemArray = new object[]
                    {
                        "b = ", trand.B.ToString(format)
                    };
                }
                else if (i == n + 4)
                {
                    dr.ItemArray = new object[]
                    {
                        "a = ", trand.A.ToString(format)
                    };
                }

                table.Rows.Add(dr);
            }

            

            DgTrand.DataContext = table.DefaultView;
        }

        private void ShowRandom()
        {
            var n = _core.Count;

            var r = _core.Random;

            var table = new DataTable();
            table.Columns.Add(new DataColumn("t"));
            table.Columns.Add(new DataColumn("Y"));
            table.Columns.Add(new DataColumn("St"));
            table.Columns.Add(new DataColumn("T + E = Y - S"));
            table.Columns.Add(new DataColumn("T"));
            table.Columns.Add(new DataColumn("T + S"));
            table.Columns.Add(new DataColumn("E = Y - (T + S)"));
            table.Columns.Add(new DataColumn("E^2"));


            var format = "0.00";

            var index = 0;

            for (var i = 0; i < n+3; i++)
            {
                var dr = table.NewRow();

                if (i < n)
                {
                    dr.ItemArray = new object[]
                    {
                        _core.T[i], _core.Y[i], _core.SeasonContext.SeasonComponentCorrected[index].ToString(format),
                        r.TplusE[i].ToString(format), r.T[i].ToString(format), r.TplusS[i].ToString(format),
                        r.E[i].ToString(format), r.ESquare[i].ToString(format)
                    };
                    index++;
                    if (index == _core.SeasonContext.SeasonComponentCorrected.Length)
                    {
                        index = 0;
                    }
                }
                else if (i == n)
                {
                    dr.ItemArray = new object[]
                    {
                        "", "", "", "", "", "", "Сумма квадратов абсолютных ошибок", r.ESquare.Sum().ToString(format)
                    };
                }
                else if (i == n+1)
                {
                    dr.ItemArray = new object[] { };
                }
                else if (i == n+2)
                {
                    dr.ItemArray = new object[]
                    {
                        "Качество модели", r.Quality.ToString("P1")
                    };
                }

                table.Rows.Add(dr);
            }



            DgRandom.DataContext = table.DefaultView;
        }

        private void ShowGraph(int[] x ,double[] y, int[] x1, double[] y1)
        {
            var points = new ObservableCollection<Coord>();

            for (var i = 0; i < x.Length; i++)
            {
                points.Add(new Coord(x[i],y[i]));
            }

            var dataSourceList = new List<ObservableCollection<Coord>> {points};
            //LineSeries.ItemsSource = points;

            points = new ObservableCollection<Coord>();
            for (var i = 0; i < x1.Length; i++)
            {
                points.Add(new Coord(x1[i], y1[i]));
            }

            dataSourceList.Add(points);

            Chart.DataContext = dataSourceList;
            //LineSeriesY.ItemsSource = points;
        }

        private class ExcelData
        {
            public ExcelData(string path)
            {
                Data = SetData(path);
            }

            private DataView SetData(string path)
            {
                var excelApp = new Application();
                // workbook = excelApp.Workbooks.Open(Environment.CurrentDirectory + "\\Анализ временных рядов.xlsx");
                var workbook = excelApp.Workbooks.Open(path);
                var worksheet = (Worksheet)workbook.Sheets[1];

                int column;
                int row;

                var range = worksheet.UsedRange;
                var dt = new DataTable();
                for (column = 1; column <= range.Columns.Count; column++)
                {
                    var o = range.Cells[1, column] as Range;
                    if (o == null) continue;
                    string text = o.Value2.ToString();
                    dt.Columns.Add(text);
                }
                for (row = 2; row <= range.Rows.Count; row++)
                {
                    var dr = dt.NewRow();
                    for (column = 1; column <= range.Columns.Count; column++)
                    {
                        var o = range.Cells[row, column] as Range;
                        if (o != null)
                            dr[column - 1] = o.Value2.ToString();
                    }
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }
                workbook.Close(true, Missing.Value, Missing.Value);
                excelApp.Quit();

                Table = dt;
                return dt.DefaultView;
            }

            public DataView Data { get; }
            public DataTable Table { get; private set; }
        }

        private static void SaveToExcel(IReadOnlyList<DataTable> tbl, string excelFilePath = null)
        {
            try
            {
                if (tbl.Any(t => t == null || t.Columns.Count == 0))
                {
                    throw new Exception("Экспорт в Excel: Null or empty input tables!\n");
                }


                // load excel, and create a new workbook
                var excelApp = new Application();
                var workbook = excelApp.Workbooks.Add();
                excelApp.Sheets.Add();
                excelApp.Sheets.Add();
                ((Worksheet) excelApp.Sheets[1]).Name = "Исходные данные";
                ((Worksheet) excelApp.Sheets[2]).Name = "Скользящие средние";
                ((Worksheet) excelApp.Sheets[3]).Name = "Аддитивная модель";


                for (var index = 0; index < tbl.Count; index++)
                {
                    // single worksheet
                    //Excel._Worksheet workSheet = excelApp.ActiveSheet;
                    _Worksheet workSheet = excelApp.Sheets[index + 1];

                    // column headings
                    for (var i = 0; i < tbl[index].Columns.Count; i++)
                    {
                        workSheet.Cells[1, (i + 1)] = tbl[index].Columns[i].ColumnName;
                    }

                    // rows
                    for (var i = 0; i < tbl[index].Rows.Count; i++)
                    {
                        // to do: format datetime values before printing
                        for (var j = 0; j < tbl[index].Columns.Count; j++)
                        {
                            workSheet.Cells[(i + 2), (j + 1)] = tbl[index].Rows[i][j];
                        }
                    }
                }
                

                // check fielpath
                if (!string.IsNullOrEmpty(excelFilePath))
                {
                    try
                    {
                        workbook.SaveAs(excelFilePath);
                        excelApp.Quit();
                        MessageBox.Show("Файл успешно сохранён!");
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Экспорт в Excel: Excel файл не может быть сохранён! Проверьте путь к файлу.\n"
                            + ex.Message);
                    }
                }
                else    // no filepath is given
                {
                    excelApp.Visible = true;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Экспорт в Excel: \n" + ex.Message);
            }
        }

        private void save_Click(object sender, RoutedEventArgs e)
        {
            if (!_dataIsReady)
            {
                return;
            }
            var sfd = new SaveFileDialog
            {
                DefaultExt = ".xlsx",
                Filter = "(.xlsx)|*.xlsx"
            };
            string path;
            if (sfd.ShowDialog() == true)
            {
                path = sfd.FileName;
            }
            else
            {
                return;
            }

            _loading = new Loading();
            _loading.Show();
            var t = new DataTable[3];
            var excelData = DataGrid.DataContext as ExcelData;
            if (excelData != null)
            {
                var v = excelData.Data;
                t[0] = v.Table;
                v = (DataView)DgSlides.DataContext;
                t[1] = v.Table;
                v = (DataView)DgTrand.DataContext;
                t[2] = v.Table;
            }
            SaveToExcel(t,path);
            _loading.Close();
        }

        private void MenuItem_Click(object sender, RoutedEventArgs e)
        {

            var proc = new System.Diagnostics.Process
            {
                StartInfo =
                {
                    FileName = @"help.chm",
                    UseShellExecute = true
                }
            };
            proc.Start();
        }
    }
}
