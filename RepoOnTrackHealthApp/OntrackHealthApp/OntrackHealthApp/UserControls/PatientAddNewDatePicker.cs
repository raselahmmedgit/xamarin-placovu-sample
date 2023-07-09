using System;
using Xamarin.Forms;

namespace OntrackHealthApp.UserControls
{
    public class PatientAddNewDatePicker : DatePicker
    {
        public static readonly BindableProperty BorderColorProperty = BindableProperty.Create(nameof(BorderColor), typeof(Color), typeof(PatientAddNewEntry), Color.LightGray);        
        // Gets or sets BorderColor value  
        public Color BorderColor
        {
            get => (Color)GetValue(BorderColorProperty);
            set => SetValue(BorderColorProperty, value);
        }

        public static readonly BindableProperty BorderWidthProperty = BindableProperty.Create(nameof(BorderWidth), typeof(int), typeof(PatientAddNewEntry), 1);
        // Gets or sets BorderWidth value  
        public int BorderWidth
        {
            get => (int)GetValue(BorderWidthProperty);
            set => SetValue(BorderWidthProperty, value);
        }
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(double), typeof(PatientAddNewEntry), 5.00);
        // Gets or sets CornerRadius value  
        public double CornerRadius
        {
            get => (double)GetValue(CornerRadiusProperty);
            set => SetValue(CornerRadiusProperty, value);
        }
        public static readonly BindableProperty IsCornerRadiusEnabledProperty = BindableProperty.Create(nameof(IsCornerRadiusEnabled), typeof(bool), typeof(PatientAddNewEntry), true);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool IsCornerRadiusEnabled
        {
            get => (bool)GetValue(IsCornerRadiusEnabledProperty);
            set => SetValue(IsCornerRadiusEnabledProperty, value);
        }
        public static readonly BindableProperty CornerRadiusToHeightEnabledProperty = BindableProperty.Create(nameof(CornerRadiusToHeight), typeof(bool), typeof(PatientAddNewEntry), false);
        // Gets or sets IsCurvedCornersEnabled value  
        public bool CornerRadiusToHeight
        {
            get => (bool)GetValue(IsCornerRadiusEnabledProperty);
            set => SetValue(IsCornerRadiusEnabledProperty, value);
        }
        public void SetDate(DateTime? value)
        {
            if (value != null)
            {
                this.Date = (DateTime)value;
            }
        }
    }
}
