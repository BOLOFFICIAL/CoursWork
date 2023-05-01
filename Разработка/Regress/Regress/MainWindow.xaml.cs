using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Regress
{
    public partial class MainWindow : Window
    {
        private PlotModel myModel;

        public event PropertyChangedEventHandler PropertyChanged;

        public PlotModel MyModel
        {
            get { return myModel; }
            set { myModel = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MyModel")); }
        }

        public MainWindow()
        {
            InitializeComponent();

            // Create the plot model
            MyModel = new PlotModel { Title = "Simple Linear Plot" };

            // Create the x and y axes
            LinearAxis xAxis = new LinearAxis { Position = AxisPosition.Bottom };
            LinearAxis yAxis = new LinearAxis { Position = AxisPosition.Left };

            // Add the axes to the plot model
            MyModel.Axes.Add(xAxis);
            MyModel.Axes.Add(yAxis);

            // Create the line series
            var points = new List<DataPoint>
            {
                new DataPoint(0, 1),
                new DataPoint(2, 3),
                new DataPoint(4, 20),
                new DataPoint(6, 4),
                new DataPoint(8, 3)
            };
            LineSeries lineSeries = new LineSeries { ItemsSource = points };

            // Add the line series to the plot model
            MyModel.Series.Add(lineSeries);

            // Set the data context for the plot view
            DataContext = this;
        }
    }
}
