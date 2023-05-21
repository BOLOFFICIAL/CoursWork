﻿using iTextSharp.text;
using iTextSharp.text.pdf;
using Microsoft.Win32;
using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using Regress.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace Regress
{
    /// <summary>
    /// Логика взаимодействия для AnalisePage.xaml
    /// </summary>
    public partial class AnalisePage : Page
    {
        private CorrelationRegression _regression;

        public AnalisePage(bool hasfile = false)
        {
            InitializeComponent();

            ComboBoxRegression.ItemsSource = new List<string>()
            {
                "Линейная",
                "Степенная",
                "Квадратичная",
                "Логарифмическая",
                "Гиперболическая",
            };

            ComboBoxParameter.ItemsSource = ProgramData.csv.GetNames().Where(name => name != ProgramData.resultcolumn).ToList();

            if (hasfile)
            {
                Analize(false, hasfile);
            }
        }

        private void PrintOXY(LineSeries equation)
        {
            var plotModel = new PlotModel();

            var scatterSeries = new LineSeries
            {
                MarkerType = MarkerType.Circle,
                MarkerSize = 5,
                MarkerFill = OxyColor.FromRgb(50, 50, 50),
                Color = OxyColor.FromRgb(50, 50, 50),
                LineStyle = LineStyle.Dot,
                StrokeThickness = 2
            };

            for (int i = 0; i < ProgramData.csv.RowCount; i++)
            {
                scatterSeries.Points.Add(new DataPoint(ProgramData.X[i], ProgramData.Y[i]));
            }

            plotModel.Series.Add(scatterSeries);
            plotModel.Series.Add(equation);
            plotModel.Title = $"Dependence {ProgramData.resultcolumn} - {ProgramData.parametercolumn}";

            plotModel.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Bottom,
                    Title = ProgramData.parametercolumn.ToUpper(),
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    TitleColor = OxyColor.FromRgb(50, 50, 50),
                    TitleFontSize = 15,
                    FontWeight = OxyPlot.FontWeights.Bold
                });

            plotModel.Axes.Add(
                new LinearAxis
                {
                    Position = AxisPosition.Left,
                    Title = ProgramData.resultcolumn.ToUpper(),
                    TitleColor = OxyColor.FromRgb(50, 50, 50),
                    TitleFontWeight = OxyPlot.FontWeights.Bold,
                    TitleFontSize = 15,
                    FontWeight = OxyPlot.FontWeights.Bold
                });

            PlotViewAnalise.Model = plotModel;
        }

        private void ToEditFile(object sender, RoutedEventArgs e)
        {
            ProgramData.regressionindex = -1;
            NavigationService.Navigate(new ChosePage());
        }

        private void Analize(object sender, RoutedEventArgs e)
        {
            Analize(false);
        }

        private void AutoAnalize(object sender, RoutedEventArgs e)
        {
            Analize(true);
        }

        private void Analize(bool auto, bool hasfile = false)
        {
            if (!hasfile)
            {
                ProgramData.regressionindex = ComboBoxRegression.SelectedIndex;
                ProgramData.parametercolumn = ComboBoxParameter.SelectedValue.ToString();
            }
            else
            {
                ComboBoxRegression.SelectedIndex = ProgramData.regressionindex;
                ComboBoxParameter.SelectedValue = ProgramData.parametercolumn;
            }
            ProgramData.X = ProgramData.csv.GetColumn(ProgramData.parametercolumn).Value
                .Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0)
                .ToList();
            ProgramData.Y = ProgramData.csv.GetColumn(ProgramData.resultcolumn).Value
                .Select(el => double.TryParse(el.Replace(".", ","), out double element) ? element : 0)
                .ToList();

            var data = ProgramData.X.Zip(ProgramData.Y, (x, y) => new { X = x, Y = y }).ToList();

            data.Sort((a, b) => a.X.CompareTo(b.X));

            ProgramData.X = data.Select(pair => pair.X).ToList();
            ProgramData.Y = data.Select(pair => pair.Y).ToList();

            _regression = new CorrelationRegression(ProgramData.regressionindex, auto);

            List<string> results = _regression.Results;

            Label1.Content = results[0].Replace("+ -", "- ");
            Label2.Content = results[1];
            Label3.Content = results[2];
            Label4.Content = results[3];
            Label5.Content = $"{results[4]}%";
            Label6.Content = results[5];
            Label7.Content = results[6];
            Label8.Content = results[7];

            PrintOXY(_regression.Line);

            if (auto)
            {
                ComboBoxRegression.SelectedIndex = _regression.TitleIndex;
                ProgramData.regressionindex = _regression.TitleIndex;
            }

            GridData.Height = double.NaN;
            ButtonSave.Visibility = Visibility.Visible;
            BorderAnalize.Visibility = Visibility.Visible;

            GridAnalize.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
            GridAnalize.ColumnDefinitions[1].Width = new GridLength(0, GridUnitType.Auto);
            GridDataAnalize.RowDefinitions[0].Height = new GridLength(0, GridUnitType.Auto);
            GridDataAnalize.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);
            GridDataAnalize.RowDefinitions[2].Height = new GridLength(0, GridUnitType.Auto);
        }

        private void SavePdf(object sender, RoutedEventArgs e)
        {
            string p1 = "";
            string p2 = "";

            if (!(_regression?.Title is null))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog();
                saveFileDialog.FileName = _regression.Title + "Regression";
                saveFileDialog.DefaultExt = ".pdf";
                saveFileDialog.Filter = "Regressions (.pdf)|*.pdf";

                if (saveFileDialog.ShowDialog() == true)
                {

                    p1 = Path.Combine(Path.GetDirectoryName(saveFileDialog.FileName), DateTime.Now.ToString("fffffff") + ".pdf");

                    var model = PlotViewAnalise.Model;

                    SavePlotViewToPdf(p1, model);

                    string[] data = new string[]
                   {
                        "Привет Bolofficial",
                        "Привет Bolofficial",
                        "Привет Bolofficial",
                        "Привет Bolofficial",
                        "Bolofficial",
                        "Bolofficial",
                        "Bolofficial",
                        "Bolofficial",
                        "Bolofficial",
                        "Bolofficial",
                   };

                    p2 = Path.Combine(Path.GetDirectoryName(saveFileDialog.FileName), DateTime.Now.ToString("fffffff") + ".pdf");

                    SaveTextToPdf(p2, data);

                    MergePdfFiles(p1, p2, saveFileDialog.FileName);

                    var result = MessageBox.Show("Отчет успешно сохранен.\nХотите открыть сохраненный файл?", "Сохранение завершено", MessageBoxButton.YesNo, MessageBoxImage.Question);

                    if (result == MessageBoxResult.Yes)
                    {
                        Process.Start("cmd", "/c start \"\" \"" + saveFileDialog.FileName + "\"");
                    }
                }
            }
            else
            {
                MessageBox.Show("Нет данных для формирования отчета");
            }
        }

        private void SavePlotViewToPdf(string p1, PlotModel plotModel)
        {
            plotModel.Title = "Correlation - regression analysis in RiverCorr";
            var exporter = new PdfExporter();

            exporter.Width = 595;
            exporter.Height = (595 * 842) / plotModel.Height;

            using (var stream = File.Create(p1))
            {
                exporter.Export(plotModel, stream);
            }
        }

        private void SaveTextToPdf(string filePath, string[] data)
        {
            Aspose.Pdf.Document document = new Aspose.Pdf.Document();
            try
            {
                Aspose.Pdf.Page page = document.Pages.Add();

                foreach (var el in data)
                {
                    page.Paragraphs.Add(new Aspose.Pdf.Text.TextFragment(el));
                }

                document.Save(filePath);
            }
            catch (iTextSharp.text.DocumentException ex)
            {
                Console.Error.WriteLine("Ошибка при создании документа PDF: " + ex.Message);
            }
            finally
            {
                document.Dispose();
            }
        }

        private void MergePdfFiles(string filePath1, string filePath2, string outputFilePath)
        {
            iTextSharp.text.Document document = new iTextSharp.text.Document();

            try
            {
                PdfCopy copy = new PdfCopy(document, new FileStream(outputFilePath, FileMode.Create));
                document.Open();

                PdfReader reader1 = new PdfReader(filePath1);
                PdfReader reader2 = new PdfReader(filePath2);

                document.NewPage();

                for (int i = 1; i <= reader1.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(reader1, i));
                }

                for (int i = 1; i <= reader2.NumberOfPages; i++)
                {
                    copy.AddPage(copy.GetImportedPage(reader2, i));
                }

                copy.Close();
                reader1.Close();
                reader2.Close();

                File.Delete(filePath1);
                File.Delete(filePath2);
            }
            catch (DocumentException ex)
            {
                Console.Error.WriteLine("Ошибка при объединении PDF файлов: " + ex.Message);
                File.Delete(filePath1);
                File.Delete(filePath2);
            }
            finally
            {
                document.Close();
            }
        }

        private void ToChosePage(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "CSV files (*.csv)|*.csv";
            if (openFileDialog.ShowDialog() == true)
            {
                new ProgramData(openFileDialog.FileName);
                NavigationService.Navigate(new ChosePage());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new StartPage());
        }
    }
}
