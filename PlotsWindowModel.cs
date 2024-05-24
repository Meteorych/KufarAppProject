using OxyPlot;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Navigation;

namespace KufarAppProject
{
    public class PlotsWindowModel : INotifyPropertyChanged
    {
        private KufarApi _kufarApi;

        public PlotModel PriceByFloorModel { get; private set; }
        public PlotModel PriceByRoomsModel { get; private set; }
        public PlotModel PriceByMetroModel { get; private set; }

        public PlotsWindowModel(KufarApi kufarApi)
        {
            _kufarApi = kufarApi;

            //Construction of chart that illustrates dependency between floor and price
            PriceByFloorModel = new PlotModel() { Title = "Зависимость цены за м2 от этажа квартиры"};
            var data1 = _kufarApi.GetPriceByFloor();
            PriceByFloorModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Стоимость за м²", MajorStep = 100 });
            PriceByFloorModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Этаж квартиры"});
            PriceByFloorModel.Series.Add(CreateRoomsAndFloorsSeries(data1));

            //Construction of chart that illustrates dependency between room and price
            PriceByRoomsModel = new PlotModel() { Title = "Зависимость цены за м2 от количества комнат" };
            data1 = _kufarApi.GetPriceByNumberOfRooms();
            PriceByRoomsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Стоимость за м²", MajorStep = 100 });
            PriceByRoomsModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Title = "Количество комнат", MajorStep = 1 });
            PriceByRoomsModel.Series.Add(CreateRoomsAndFloorsSeries(data1));

            //Construction of chart that illustrates dependency between room and the nearest metro station
            PriceByMetroModel = new PlotModel() { Title = "Зависимость цены за м2 от станции метро" };
            var data2 = _kufarApi.GetPriceByMetroStation();
            PriceByMetroModel.Series.Add(CreatePriceByMetroStationSeries(data2));
        }

        private LineSeries CreateRoomsAndFloorsSeries(Dictionary<int, double> data)
        {
            var lineSeries = new LineSeries
            {
                Title = "Стоимость за м²",
                MarkerType = MarkerType.Circle
            };
            foreach (var entry in data.OrderBy(flat => flat.Key)) 
            {
                lineSeries.Points.Add(new DataPoint(entry.Key, entry.Value));
            }

            return lineSeries;
        }

        private BarSeries CreatePriceByMetroStationSeries(Dictionary<string, double> data)
        {
            var series = new BarSeries
            {
                Title = "Стоимость за м² по станциям метро",
                LabelPlacement = LabelPlacement.Inside,
                LabelFormatString = "{0:N2} $",
                XAxisKey = "Value",
                YAxisKey = "Category"
            };
            var categoryAxis = new CategoryAxis { Position = AxisPosition.Bottom, Title = "Станция метро" , Key = "Category"};
            foreach (var pair in data)
            {
                categoryAxis.Labels.Add(pair.Key);
                series.Items.Add(new BarItem(pair.Value));
            }
            PriceByMetroModel.Axes.Add(categoryAxis);
            PriceByMetroModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Title = "Стоимость за м²", Key = "Value"});
            

            return series;
        }

        public event PropertyChangedEventHandler? PropertyChanged;
        public void OnPropertyChanged([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
