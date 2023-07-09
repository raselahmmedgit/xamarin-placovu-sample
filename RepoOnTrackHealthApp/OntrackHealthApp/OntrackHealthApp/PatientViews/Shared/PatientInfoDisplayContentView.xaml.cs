
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.Views.Shared
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PatientInfoDisplayContentView : ContentView
    {
        public PatientInfoDisplayContentView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public static readonly BindableProperty PatientFullNameProperty = BindableProperty.Create(nameof(PatientFullName), typeof(double), typeof(PatientInfoDisplayContentView), "");
        // Gets or sets PatientFullName value  
        public string PatientFullName
        {
            get => (string)GetValue(PatientFullNameProperty);
            set => SetValue(PatientFullNameProperty, value);
        }
        public static readonly BindableProperty ProcedureNameProperty = BindableProperty.Create(nameof(ProcedureName), typeof(bool), typeof(PatientInfoDisplayContentView), "");
        // Gets or sets ProcedureName value  
        public string ProcedureName
        {
            get => (string)GetValue(ProcedureNameProperty);
            set => SetValue(ProcedureNameProperty, value);
        }
        public static readonly BindableProperty ProfessionalNameProperty = BindableProperty.Create(nameof(ProfessionalName), typeof(bool), typeof(PatientInfoDisplayContentView), "");
        // Gets or sets ProfessionalName value  
        public string ProfessionalName
        {
            get => (string)GetValue(ProfessionalNameProperty);
            set => SetValue(ProfessionalNameProperty, value);
        }
    }
}