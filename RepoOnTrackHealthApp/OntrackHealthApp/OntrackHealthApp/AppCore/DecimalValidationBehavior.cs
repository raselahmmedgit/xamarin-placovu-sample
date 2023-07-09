using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace OntrackHealthApp.AppCore
{
    public class DecimalValidationBehavior : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntryTextChanged;
            base.OnAttachedTo(entry);
        }

        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntryTextChanged;
            base.OnDetachingFrom(entry);
        }

        void OnEntryTextChanged(object sender, TextChangedEventArgs args)
        {
            decimal result;

            bool isValid = decimal.TryParse(args.NewTextValue, out result);

            ((Entry)sender).TextColor = isValid ? Color.Default : Color.Red;
        }
    }
}