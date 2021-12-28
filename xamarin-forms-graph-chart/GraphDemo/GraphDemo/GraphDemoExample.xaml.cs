using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace GraphDemo
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GraphDemoExample : TabbedPage
    {
        public GraphDemoExample()
        {
            InitializeComponent();

            var navigationPage = new NavigationPage(new GraphWithChartJsPage());
            navigationPage.Title = "ChartJs";

            Children.Add(new GraphWithMicrochartPage());
            Children.Add(navigationPage);
        }
    }
}