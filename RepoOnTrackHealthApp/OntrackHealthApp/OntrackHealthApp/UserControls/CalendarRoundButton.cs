using System;
using Xamarin.Forms;

namespace OntrackHealthApp.UserControls
{
    public class CalendarRoundButton : Button
    {
        public static readonly BindableProperty EventDayProperty = BindableProperty.Create(nameof(EventDay), typeof(int), typeof(CalendarRoundButton), default(int));
        public static readonly BindableProperty EventMonthProperty = BindableProperty.Create(nameof(EventMonth), typeof(int), typeof(CalendarRoundButton), default(int));
        public static readonly BindableProperty EventMonthNameProperty = BindableProperty.Create(nameof(EventMonthName), typeof(string), typeof(CalendarRoundButton), string.Empty);
        public static readonly BindableProperty EventYearProperty = BindableProperty.Create(nameof(EventYear), typeof(int), typeof(CalendarRoundButton), default(int));
        public static readonly BindableProperty EventIdProperty = BindableProperty.Create(nameof(EventId), typeof(long), typeof(CalendarRoundButton), default(long));
        public static readonly BindableProperty EventDetailItemProperty = BindableProperty.Create(nameof(EventDetailItem), typeof(object), typeof(CalendarRoundButton), default(object));
        public static readonly BindableProperty EventTitleProperty = BindableProperty.Create(nameof(EventTitle), typeof(string), typeof(CalendarRoundButton), string.Empty);

        public int EventDay
        {
            get => (int)GetValue(EventDayProperty);
            set => SetValue(EventDayProperty, value);
        }
        public int EventMonth
        {
            get => (int)GetValue(EventMonthProperty);
            set => SetValue(EventMonthProperty, value);
        }
        public string EventMonthName
        {
            get => (string)GetValue(EventMonthNameProperty);
            set => SetValue(EventMonthNameProperty, value);
        }
        public int EventYear
        {
            get => (int)GetValue(EventYearProperty);
            set => SetValue(EventYearProperty, value);
        }
        public long EventId
        {
            get => (long)GetValue(EventIdProperty);
            set => SetValue(EventIdProperty, value);
        }
        public object EventDetailItem
        {
            get => GetValue(EventDetailItemProperty);
            set => SetValue(EventDetailItemProperty, value);
        }
        public string EventTitle
        {
            get => (string)GetValue(EventTitleProperty);
            set => SetValue(EventTitleProperty, value);
        }
        protected override void OnSizeAllocated(double width, double height)
        {
            base.OnSizeAllocated(width, height);
            var hw = Math.Min(width, height);
            base.HeightRequest = this.WidthRequest = hw;
            base.CornerRadius = (int)(hw / 2);
            base.IsVisible = true;
        }

    }
}
