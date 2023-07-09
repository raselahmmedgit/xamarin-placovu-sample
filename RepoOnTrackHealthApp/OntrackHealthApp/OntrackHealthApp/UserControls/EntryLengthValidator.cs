using Xamarin.Forms;

namespace OntrackHealthApp.UserControls
{
    public class EntryLengthValidator : Behavior<Entry>
    {
        public int MaxLength { get; set; }
        public int MinLength { get; set; } = 0;

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
            var entry = (Entry)sender;
            if (entry.Text.Length > this.MaxLength)
            {
                string providedText = entry.Text;
                providedText = providedText.Remove(providedText.Length - 1);
                entry.Text = providedText;
            }

            if (MinLength > 0)
            {
                if (entry.Text.Length < this.MinLength)
                {
                    ((Entry)sender).TextColor = Color.Red;
                }
                else
                {
                    ((Entry)sender).TextColor = Color.Black;
                }
            }
        }
    }
}
