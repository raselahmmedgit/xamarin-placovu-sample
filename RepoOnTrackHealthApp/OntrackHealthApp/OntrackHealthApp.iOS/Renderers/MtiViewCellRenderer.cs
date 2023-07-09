using OntrackHealthApp.iOS.Renderers;
using OntrackHealthApp.UserControls;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.Platform.iOS;

[assembly: ExportRenderer(typeof(MtiViewCell), typeof(ExViewCellRenderer))]
namespace OntrackHealthApp.iOS.Renderers
{
    public class ExViewCellRenderer : ViewCellRenderer
    {
        private UIView bgView;

        public override UITableViewCell GetCell(Cell item, UITableViewCell reusableCell, UITableView tv)
        {
            var cell = base.GetCell(item, reusableCell, tv);

            cell.BackgroundColor = Color.FromHex("#337ab7").ToUIColor();
            cell.TextLabel.TextColor = UIColor.Black;
            cell.Layer.BorderWidth = 10.0f;
            cell.Layer.BorderColor = Color.Transparent.ToCGColor();

            if (bgView == null)
            {
                bgView = new UIView(cell.SelectedBackgroundView.Bounds);
                bgView.Layer.BackgroundColor = Color.FromHex("#f7a50f").ToCGColor();
                bgView.Layer.BorderColor = Color.Transparent.ToCGColor();
                bgView.Layer.BorderWidth = 10.0f;
            }
            cell.SelectedBackgroundView = bgView;

            return cell;
        }
        public override void SetBackgroundColor(UITableViewCell tableViewCell, Cell cell, UIColor color)
        {
            base.SetBackgroundColor(tableViewCell, cell, color);
        }
    }
}