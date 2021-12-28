using GraphDemo.Chart;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraphDemo
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class GraphWithChartJsPage : ContentPage
	{
        private string url = "https://ontrack-healthdemo.com/webapi/v3/api/Account/GetTempGraphReportData";
        public GraphWithChartJsPage ()
		{
			InitializeComponent ();
            this.BindingContext = new ChartReportPageViewModel("line", url);
            
        }
    }
}