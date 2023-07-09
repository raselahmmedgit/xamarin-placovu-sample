using Xamarin.Forms;

namespace OntrackHealthApp.Extensions
{
    public static class ContenPageBaseExtensions
    {
        public static void AddProgressDisplay(this ContentPage contentPage, bool showToast = false, string toastText = "")
        {
            var content = contentPage.Content;
            var grid = new Grid { Padding = new Thickness(0), Margin = new Thickness(0) };
            grid.Children.Add(content);
            var gridProgress = new Grid { BackgroundColor = Color.FromHex("#64000000"), Padding = new Thickness(0), Margin = new Thickness(0) };
            gridProgress.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridProgress.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Auto) });
            gridProgress.RowDefinitions.Add(new RowDefinition { Height = new GridLength(1, GridUnitType.Star) });
            gridProgress.SetBinding(VisualElement.IsVisibleProperty, "IsBusy");

            StackLayout innerFrame = new StackLayout { };

            var activity = new ActivityIndicator
            {
                IsEnabled = true,
                IsVisible = true,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsRunning = true,   
                Color = Color.Pink,
                WidthRequest = 40,
                HeightRequest = 40
            };
            StackLayout activityFrame = new StackLayout { Padding = new Thickness(10) };
            activityFrame.Children.Add(activity);

            innerFrame.Children.Add(activityFrame);

            if (showToast)
            {
                StackLayout lblFrame = new StackLayout { Padding = new Thickness(10) };
                Label lbl = new Label
                {
                    Text = "Loading...",
                    TextColor = Color.FromHex("#FFFFFF"),
                    FontSize = 16,
                    VerticalOptions = LayoutOptions.Center,
                    HorizontalOptions = LayoutOptions.Center,
                };
                if (!string.IsNullOrEmpty(toastText))
                {
                    lbl.Text = toastText;
                }
                lblFrame.Children.Add(lbl);
                innerFrame.Children.Add(lblFrame);
            }
            Frame frm = new Frame {
                Padding = new Thickness(5),
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                BackgroundColor = Color.FromRgba(0, 0, 0, .8),
                Margin = new Thickness(0, -60, 0, 0),
                Content = innerFrame,
                CornerRadius = 16
            };
            gridProgress.Children.Add(frm, 0, 1);
            grid.Children.Add(gridProgress);
            contentPage.Content = grid;
        }


        public static void DisplayContentViewAsModal(this ContentView contentView, ContentPage contentPage)
        {
            //
        }
    }
}
