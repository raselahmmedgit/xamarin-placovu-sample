using System;
using System.Runtime.CompilerServices;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace OntrackHealthApp.UserControls
{
    public enum GradientColorStackMode
    {
        ToRight,
        ToLeft,
        ToTop,
        ToBottom,
        ToTopLeft,
        ToTopRight,
        ToBottomLeft,
        ToBottomRight
    }

    public enum ButtonTextEnum
    {
        Search,
        Add_Patient,
        Continue,
        Continue_Program,
        Delete,
        Edit,
        Cancel,
        Ok,
        Default
    }

    public class PatientAddNewStackLayoutForm : StackLayout
    {
        public PatientAddNewStackLayoutForm()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12);
                //BackgroundColor = Color.FromHex("#E6FFFF");
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(12, 12, 12, 24);
                //BackgroundColor = Color.FromHex("#E6FFFF");
                Spacing = 0;
                Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }

    public class StackLayoutForm : StackLayout
    {
        public StackLayoutForm()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12);
                BackgroundColor = Color.FromHex("#F2F2F2");
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(12,12,12,24);
                BackgroundColor = Color.FromHex("#E6FFFF");
                Spacing = 0;
                Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }

    public class PatientAddNewStackLayoutFormGroup : StackLayout
    {
        public static readonly BindableProperty ShowBorderTopProperty = BindableProperty.Create(nameof(ShowBorderTop), typeof(bool), typeof(StackLayoutFormGroup), defaultValue: false, defaultBindingMode: BindingMode.TwoWay);


        public bool ShowBorderTop
        {
            get { return (bool)GetValue(ShowBorderTopProperty); }
            set { SetValue(ShowBorderTopProperty, value); }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == ShowBorderTopProperty.PropertyName)
            {
                base.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.FromHex("#FFFFFF"), HorizontalOptions = LayoutOptions.FillAndExpand });
            }
        }

        public PatientAddNewStackLayoutFormGroup()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12, 6);
                //BackgroundColor = Color.FromHex("#E6FFFF");
                //Margin = new Thickness(0, 0, 0, 1);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(0, 6);
                //BackgroundColor = Color.FromHex("#E6FFFF");
                //Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }

    public class StackLayoutFormGroup : StackLayout
    {
        public static readonly BindableProperty ShowBorderTopProperty = BindableProperty.Create(nameof(ShowBorderTop), typeof(bool), typeof(StackLayoutFormGroup), defaultValue: false, defaultBindingMode: BindingMode.TwoWay);

       
        public bool ShowBorderTop
        {
            get { return (bool)GetValue(ShowBorderTopProperty); }
            set { SetValue(ShowBorderTopProperty, value); }
        }

        protected override void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            base.OnPropertyChanged(propertyName);
            if (propertyName == ShowBorderTopProperty.PropertyName)
            {
                base.Children.Add(new BoxView { HeightRequest = 1, BackgroundColor = Color.FromHex("#FFFFFF"), HorizontalOptions = LayoutOptions.FillAndExpand });
            }           
        }

        public StackLayoutFormGroup()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12, 6);
                BackgroundColor = Color.FromHex("#E6FFFF");
                Margin = new Thickness(0, 0, 0, 1);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(0, 6);
                BackgroundColor = Color.FromHex("#E6FFFF");
                Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }
    public class StackLayoutRootContent : StackLayout
    {
        public StackLayoutRootContent()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(0);
                Spacing = 0;
                Spacing = 0;
                Margin = new Thickness(0);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(0);
                Spacing = 0;
                Margin = new Thickness(0,-20,0,0);
            }
        }
    }
    public class StackLayoutPageTitle : StackLayout
    {
        public StackLayoutPageTitle()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12);
                BackgroundColor = Color.FromHex("#426276");
                Spacing = 0;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(12,22,12,12);
                BackgroundColor = Color.FromHex("#426276");
                Spacing = 0;
                Margin = new Thickness(0);
            }
        }
    }

    public class StackLayoutPageTitleLabel : Label
    {
        public StackLayoutPageTitleLabel()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                FontSize = 22;
                TextColor = Color.FromHex("#FFFFFF");
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                FontSize = 22;
                TextColor = Color.FromHex("#FFFFFF");
            }
        }
    }

    public class StackLayoutFormTitle : StackLayout
    {
        public StackLayoutFormTitle()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12);
                BackgroundColor = Color.FromHex("#0F4563");
                Spacing = 0;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(10);
                BackgroundColor = Color.FromHex("#00829E");
                Spacing = 0;
                Margin = new Thickness(0, 0, 0, 0);
            }
        }
    }

    public class StackLayoutFormTitleLabel : Label
    {
        public StackLayoutFormTitleLabel()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                FontSize = 22;
                TextColor = Color.FromHex("#FFFFFF");
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                FontSize = 18;
                TextColor = Color.FromHex("#FFFFFF");
            }
        }
    }

    public class FrameForm : Frame
    {
        public FrameForm()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12);
                BackgroundColor = Color.FromHex("#F2F2F2");
                Margin = new Thickness(12);
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(12);
                BackgroundColor = Color.FromHex("#F2F2F2");
                Margin = new Thickness(12);
            }
        }
    }

    public class GradiantStackLayout : StackLayout
    { 
        /// <summary>
        /// IOS
        /// </summary>
        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadowProperty), typeof(bool), typeof(GradiantStackLayout), defaultValue: default(bool));
        public bool HasShadow
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }

        public static readonly BindableProperty GradiantModeProperty = BindableProperty.Create(nameof(GradientColorStackMode), typeof(GradientColorStackMode), typeof(GradiantStackLayout), defaultValue: default(GradientColorStackMode));
        public GradientColorStackMode GradiantMode
        {
            get { return (GradientColorStackMode)GetValue(GradiantModeProperty); }
            set { SetValue(GradiantModeProperty, value); }
        }
        /// <summary>
        /// IOS
        /// </summary>
        public static readonly BindableProperty GradiantColorsProperty = BindableProperty.Create(nameof(GradiantColors), typeof(string), typeof(GradiantStackLayout), defaultValue: default(string));
        public string GradiantColors
        {
            get { return (string)GetValue(GradiantColorsProperty); }
            set { SetValue(GradiantColorsProperty, value); }
        }
        /// <summary>
        /// IOS
        /// </summary>
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(GradiantStackLayout), defaultValue: 10f);
        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public GradiantStackLayout()
        {
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    Padding = new Thickness(12);
            //    BackgroundColor = Color.FromHex("#F2F2F2");
            //    Margin = new Thickness(12);
            //}
            //else if (Device.RuntimePlatform == Device.iOS)
            //{
            //    Padding = new Thickness(12);
            //    BackgroundColor = Color.FromHex("#F2F2F2");
            //    Margin = new Thickness(12);
            //}
        }
    }

    public class GradiantFrame : Frame
    {
        public GradientColorStackMode GradiantMode { get; set; }
        public string GradiantColors { get; set; }
        public GradiantFrame()
        {
            //if (Device.RuntimePlatform == Device.Android)
            //{
            //    Padding = new Thickness(12);
            //    BackgroundColor = Color.FromHex("#F2F2F2");
            //    Margin = new Thickness(12);
            //}
            //else if (Device.RuntimePlatform == Device.iOS)
            //{
            //    Padding = new Thickness(12);
            //    BackgroundColor = Color.FromHex("#F2F2F2");
            //    Margin = new Thickness(12);
            //}
        }
    }

    public class StackLayoutIsBusy : StackLayout
    {
        public StackLayoutIsBusy()
        {
            this.SetBinding(IsVisibleProperty, "IsBusy");
            Label lbl = new Label
            {
                Text = "Loading More...",
                TextColor = Color.FromHex("#FFFFFF"),
                FontSize = 16,
                VerticalOptions = LayoutOptions.Center,
                HorizontalOptions = LayoutOptions.Center,
                //HorizontalTextAlignment = TextAlignment.Center,
                //VerticalTextAlignment = TextAlignment.Center,
            };
            this.Children.Add(lbl);
            if (Device.RuntimePlatform == Device.Android)
            {
                Padding = new Thickness(12,22);
                BackgroundColor = Color.FromHex("#004D80");
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                Padding = new Thickness(12, 22);
                BackgroundColor = Color.FromHex("#004D80");
            }
        }
    }




    public class ButtonContinueExtended : Button
    {
        public ButtonContinueExtended()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#F8BA00");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#DFA802");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#F8BA00");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#DFA802");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
        }
        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonContinueExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
    }

    public class ButtonEditExtended : Button
    {
        public ButtonEditExtended()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#0F4563");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#2e6da4");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#0F4563");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#2e6da4");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
        }
        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonEditExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
    }

    public class ButtonDeleteExtended : Button
    {
        public ButtonDeleteExtended()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#D51F35");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#B2040B");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#D51F35");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#B2040B");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
        }
        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonDeleteExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
    }

    public class ButtonExtended : Button
    {
        private string tmp = "";
        public ButtonExtended()
        {           
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;

            }
            base.Clicked += ButtonExtended_Clicked;
        }

        private void ButtonExtended_Clicked(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;
           
            if (IsChecked)
            {
                tmp = this.Image;
                base.Image = this.PressedImage;
            }
            else
            {
                this.PressedImage = base.Image;
                this.Image = tmp;
            }
        }

        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
        public static readonly BindableProperty PressedImageProperty = BindableProperty.Create(nameof(PressedImage), typeof(string), typeof(ButtonExtended), string.Empty);
        // Gets or sets Pressed image path  
        public string PressedImage
        {
            get => (string)GetValue(PressedImageProperty);
            set => SetValue(PressedImageProperty, value);
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ButtonExtended), false);
        // Gets or sets is checked value  
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }

       
    }

    public class ButtonLoginExtended : Button
    {
        private string tmp = "";
        public ButtonLoginExtended()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#e8001b");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#e8001b");
                HeightRequest = 48;
                CornerRadius = 0;
                HorizontalOptions = LayoutOptions.FillAndExpand;
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#e8001b");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#e8001b");
                HeightRequest = 48;
                CornerRadius = 0;
                HorizontalOptions = LayoutOptions.FillAndExpand;

            }
            base.Clicked += ButtonExtended_Clicked;
        }

        private void ButtonExtended_Clicked(object sender, EventArgs e)
        {
            IsChecked = !IsChecked;

            if (IsChecked)
            {
                tmp = this.Image;
                base.Image = this.PressedImage;
            }
            else
            {
                this.PressedImage = base.Image;
                this.Image = tmp;
            }
        }

        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
        public static readonly BindableProperty PressedImageProperty = BindableProperty.Create(nameof(PressedImage), typeof(string), typeof(ButtonExtended), string.Empty);
        // Gets or sets Pressed image path  
        public string PressedImage
        {
            get => (string)GetValue(PressedImageProperty);
            set => SetValue(PressedImageProperty, value);
        }

        public static readonly BindableProperty IsCheckedProperty = BindableProperty.Create(nameof(IsChecked), typeof(bool), typeof(ButtonExtended), false);
        // Gets or sets is checked value  
        public bool IsChecked
        {
            get => (bool)GetValue(IsCheckedProperty);
            set => SetValue(IsCheckedProperty, value);
        }


    }

    public class ButtonAddNewExtended : Button
    {
        public ButtonAddNewExtended()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                Text = "Add New";
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                Text = "Add New";
            }
        }
        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonAddNewExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
    }

    public class ButtonSearchExtended : Button
    {
        public ButtonSearchExtended()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                Text = "Search";
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                Text = "Search";
            }
        }
        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonSearchExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
    }

    public class ButtonAddPatientExtended : Button
    {
        public ButtonAddPatientExtended()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                Text = "Add Patient";
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                BackgroundColor = Color.FromHex("#004D80");
                FontSize = 14;
                TextColor = Color.FromHex("#FFFFFF");
                BorderColor = Color.FromHex("#004D80");
                HeightRequest = 48;
                CornerRadius = 24;
                HorizontalOptions = LayoutOptions.FillAndExpand;
                Text = "Add Patient";
            }
        }
        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonAddPatientExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ButtonMenuBottom : Button
    {
        public ButtonMenuBottom()
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                
            }
            else if (Device.RuntimePlatform == Device.iOS)
            {
                #region iOS Apps Base Code
                var deviceDisplay = DeviceDisplay.MainDisplayInfo;
                if (deviceDisplay.Width == 640)
                {
                    FontSize = 12;
                }
                else
                {
                    FontSize = 14;
                }
                #endregion

            }
        }
        public static readonly BindableProperty SelectedDataItemProperty = BindableProperty.Create(nameof(SelectedDataItem), typeof(object), typeof(ButtonExtended), null);
        public object SelectedDataItem
        {
            get => GetValue(SelectedDataItemProperty);
            set => SetValue(SelectedDataItemProperty, value);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    public class ButtonMultiLine : Button
    {

    }
}
