namespace OntrackHealthApp.ChartHelper.Models
{
    public class GraphProperty
    {
		public GraphProperty()
		{
            //ctr
        }
        public string backgroundColor { get; set; }
        public bool fill { get; set; }
        public double pointRadius { get; set; }
        public string pointBorderColor { get; set; }
        public string pointBackgroundColor { get; set; }
        public double pointBorderWidth { get; set; }
        public double lineTension { get; set; }
        public string borderColor { get; set; }
        public int graphPropertyFor { get; set; }
    }
}
