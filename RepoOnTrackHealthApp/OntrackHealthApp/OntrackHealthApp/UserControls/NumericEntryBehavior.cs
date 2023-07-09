using System;
using Xamarin.Forms;

namespace OntrackHealthApp.UserControls
{
    public class NumericEntryBehavior : Behavior<MtiEntry>
    {
        protected Action<MtiEntry, string> AdditionalCheck;

        protected override void OnAttachedTo(MtiEntry bindable)
        {
            base.OnAttachedTo(bindable);
            bindable.TextChanged += TextChanged_Handler;
        }
        protected override void OnDetachingFrom(MtiEntry bindable)
        {
            base.OnDetachingFrom(bindable);
        }
        protected virtual void TextChanged_Handler(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrEmpty(e.NewTextValue))
            {
                ((Entry)sender).Text = 0.ToString();
                return;
            }

            double _;
            if (!double.TryParse(e.NewTextValue, out _))
                ((Entry)sender).Text = e.OldTextValue;
            else
                AdditionalCheck?.Invoke(((MtiEntry)sender), e.OldTextValue);
        }
    }

    public class MaximumAmountEntryBehavior : NumericEntryBehavior
    {
        public int MaximumAmount { get; set; } = 100;

        public MaximumAmountEntryBehavior()
        {
            AdditionalCheck = (e, ot) =>
            {
                e.Text = Convert.ToInt32(e.Text) > MaximumAmount ? ot : e.Text.ToString();
            };
        }
        protected override void OnAttachedTo(BindableObject bindable)
        {
            base.OnAttachedTo(bindable);
        }

        protected override void OnDetachingFrom(BindableObject bindable)
        {
            base.OnDetachingFrom(bindable);
        }

        protected override void TextChanged_Handler(object sender, TextChangedEventArgs e)
        {
            base.TextChanged_Handler(sender, e);
        }
    }
}
