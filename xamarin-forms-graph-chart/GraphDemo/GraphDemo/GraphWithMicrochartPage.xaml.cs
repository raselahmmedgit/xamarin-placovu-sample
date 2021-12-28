using Microcharts;
using SkiaSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Entry = Microcharts.Entry;

namespace GraphDemo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GraphWithMicrochartPage : ContentPage
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
        public GraphWithMicrochartPage ()
		{
			InitializeComponent ();
            LoadData();
		}
	}
}