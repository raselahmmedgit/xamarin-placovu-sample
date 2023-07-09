using Xamarin.Forms;

namespace OntrackHealthApp.UserControls
{
    public class PhoneNumberMaskValidator : Behavior<Entry>
    {
        protected override void OnAttachedTo(Entry entry)
        {
            entry.TextChanged += OnEntry_TextChanged;
            base.OnAttachedTo(entry);
        }
        protected override void OnDetachingFrom(Entry entry)
        {
            entry.TextChanged -= OnEntry_TextChanged;
            base.OnDetachingFrom(entry);
        }

        private void OnEntry_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(e.NewTextValue))
            {
                if (e.OldTextValue != null && e.NewTextValue.Length < e.OldTextValue.Length)
                    return;

                var value = e.NewTextValue;
                if (!string.IsNullOrEmpty(value) && value.Length > 3 && !value.Contains("-"))
                {
                    value = value.Insert(3, "-");
                }
                if (!string.IsNullOrEmpty(value) && value.Length > 7 && !value.Substring(6, value.Length - 6).Contains("-"))
                {
                    value = value.Insert(7, "-");
                }

                if (value.Length == 3)
                {
                    ((Entry)sender).Text += "-";
                    return;
                }
                if (value.Length == 7)
                {
                    ((Entry)sender).Text += "-";
                    return;
                }
                //For Read
                if (!string.IsNullOrEmpty(value) && value.Length >= 10 && !value.Contains("-"))
                {
                    var valueN = "";
                    valueN = value.Substring(0, 3) + "-";
                    valueN += value.Substring(3, 3) + "-";
                    valueN += value.Substring(6, value.Length - 6);
                    ((Entry)sender).Text = valueN;
                    return;
                }
                ((Entry)sender).Text = value;
            }
        }

    }
}
