using Microcharts;
using SkiaSharp;
using Xamarin.Forms;
using Entry = Microcharts.Entry;
namespace GraphDemo
{
    public partial class MainPage : ContentPage
    {
        private void LoadData()
        {
            var entries = new[]
            {
                new Entry(200)
                {
                    Label = "January",
                    ValueLabel = "200",
                    Color = SKColor.Parse("#266489")
                },
                new Entry(400)
                {
                    Label = "February",
                    ValueLabel = "400",
                    Color = SKColor.Parse("#68B9C0")
                },
                new Entry(100)
                {
                    Label = "March",
                    ValueLabel = "100",
                    Color = SKColor.Parse("#90D585")
                },
                new Entry(150)
                {
                    Label = "April",
                    ValueLabel = "150",
                    Color = SKColor.Parse("#266489")
                },
                new Entry(90)
                {
                    Label = "May",
                    ValueLabel = "90",
                    Color = SKColor.Parse("#68B9C0")
                },
                new Entry(180)
                {
                    Label = "June",
                    ValueLabel = "180",
                    Color = SKColor.Parse("#90D585")
                }
            };

            myChart.Chart = new LineChart
            {
                Entries = entries,
                LineMode = LineMode.Straight,
                PointMode = PointMode.Circle
            };

        }
        public MainPage()
        {
            InitializeComponent();
            LoadData();
        }
    }
}
