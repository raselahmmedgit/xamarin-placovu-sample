using Acr.UserDialogs;
using OntrackHealthApp.ApiHelper;
using OntrackHealthApp.AppCore;
using OntrackHealthApp.SurgicalConcierge.Helper;
using OntrackHealthApp.SurgicalConcierge.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp.SurgicalConcierge
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SurgicalConciergePatientAttendeePage : ContentPage
    {
        private readonly ITokenContainer _iTokenContainer;
        private SurgicalConciergeViewModel surgicalConciergeViewModel;
        private SurgicalConciergePatientViewModel patient;
        public SurgicalConciergePatientAttendeePage(SurgicalConciergeViewModel surgicalConciergeViewModel, SurgicalConciergePatientViewModel patient)
        {
            //DependencyService.Get<IAppPermissionChecker>().CheakPowerSaverPermission();
            InitializeComponent();
            _iTokenContainer = new TokenContainer();
            Title = _iTokenContainer.ApiPracticeName;
            this.surgicalConciergeViewModel = surgicalConciergeViewModel;
            this.patient = patient;
            if (patient != null)
            {
                this.BindingContext = this.patient;
            }
            BuildLayout(surgicalConciergeViewModel);
        }

        public void BuildLayout(SurgicalConciergeViewModel surgicalConciergeViewModel)
        {
            if (surgicalConciergeViewModel != null && surgicalConciergeViewModel.PatientAttendeeProfileViewModels != null && surgicalConciergeViewModel.PatientAttendeeProfileViewModels.Any())
            {
                int count = 1;
                foreach (var attendee in surgicalConciergeViewModel.PatientAttendeeProfileViewModels)
                {
                    var tapOnEditButton = new TapGestureRecognizer();
                    tapOnEditButton.Tapped += (sender, e) =>
                    {
                        OnAttendeeEditButtonTapped(attendee);
                    };

                    var tapOnDeleteButton = new TapGestureRecognizer();
                    tapOnDeleteButton.Tapped += (sender, e) =>
                    {
                        OnAttendeeDeleteButtonTapped(attendee);
                    };

                    StackLayout attendeeRowContainer = new StackLayout
                    {
                        Orientation = StackOrientation.Horizontal,
                        HeightRequest = 40,
                        Children =
                    {
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children =
                            {
                                new Label
                                {
                                    Text = $"{count}",
                                    TextColor = Color.FromHex("#FFF"),
                                    BackgroundColor = Color.FromHex("#0F4563"),
                                    WidthRequest = 20,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalTextAlignment = TextAlignment.Center,
                                },
                                new Label
                                {
                                    IsVisible = attendee.EmailAddress!=null,
                                    Text = attendee.EmailAddress,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalTextAlignment = TextAlignment.Center,
                                    FontSize = 18,
                                    TextColor = Color.FromHex("#0F4563")
                                },
                                new Label
                                {
                                    IsVisible = attendee.PrimaryPhone!=null,
                                    Text = attendee.PrimaryPhoneCode+attendee.PrimaryPhone,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalTextAlignment = TextAlignment.Center,
                                    FontSize = 18,
                                    TextColor = Color.FromHex("#0F4563")
                                }

                            }
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.End,
                            Children =
                            {
                                new Image
                                {
                                    Source = Device.RuntimePlatform == Device.Android ? ImageSource.FromFile("icon_edit.png") : ImageSource.FromFile("Images/icon_edit.png"),
                                    HeightRequest = 30,
                                    WidthRequest = 30,
                                    GestureRecognizers =
                                    {
                                        tapOnEditButton
                                    }
                                },
                                new Image
                                {
                                    Source = Device.RuntimePlatform == Device.Android ? ImageSource.FromFile("icon_delete.png") : ImageSource.FromFile("Images/icon_delete.png"),
                                    HeightRequest = 30,
                                    WidthRequest = 30,
                                    GestureRecognizers =
                                    {
                                        tapOnDeleteButton
                                    }
                                }
                            }
                        }
                    }
                    };
                    recipientContainerStackLayout.Children.Add(attendeeRowContainer);
                    count++;
                }

                StackLayout attendeeEntryFieldContainer = new StackLayout
                {
                    Orientation = StackOrientation.Horizontal,
                    HeightRequest = 40,
                    Children =
                    {
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children  =
                            {
                                new Label
                                {
                                    Text = $"{count}",
                                    TextColor = Color.FromHex("#FFF"),
                                    BackgroundColor = Color.FromHex("#0F4563"),
                                    WidthRequest = 20,
                                    HorizontalTextAlignment = TextAlignment.Center,
                                    VerticalTextAlignment = TextAlignment.Center,
                                },
                                new Entry
                                {
                                    HorizontalOptions = LayoutOptions.FillAndExpand,
                                    Placeholder = "type cell phone or email",
                                    FontSize = 18,
                                    TextColor = Color.FromHex("#0F4563")
                                }
                            }
                        },
                        new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            HorizontalOptions = LayoutOptions.FillAndExpand,
                            Children =
                            {
                                new Button
                                {
                                    Text="Save",
                                    BackgroundColor = Color.FromHex("#0F4563"),
                                    FontSize = 15,
                                    TextColor = Color.FromHex("#FFF")
                                }
                            }
                        }
                    }
                };

                recipientContainerStackLayout.Children.Add(attendeeEntryFieldContainer);

                StackLayout continueProgramContainer = new StackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Margin = 10,
                    Children =
                    {
                        new Button
                        {
                            Text="Continue to program",
                            BackgroundColor = Color.FromHex("#0F4563"),
                            FontSize = 18,
                            HeightRequest = 40,
                            CornerRadius = 20,
                            TextColor = Color.FromHex("#FFF")
                        }
                    }
                };

                recipientContainerStackLayout.Children.Add(continueProgramContainer);

            }

        }

        public void OnAttendeeEditButtonTapped(PatientAttendeeProfileViewModel attendee)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //Navigation.PushModalAsync(new SurgicalConciergePatientAttendeeEditPage(attendee));
                //ToastHelper.ShowToastMessage("Edit button tapped for "+GetAttendeeInfoType(attendee));
            }
        }
        
        public void OnAttendeeDeleteButtonTapped(PatientAttendeeProfileViewModel attendee)
        {
            using (UserDialogs.Instance.Loading(""))
            {
                //TODO Delete Method Call            
                ToastHelper.ShowToastMessage("Delete button tapped for " + GetAttendeeInfoType(attendee));
            }
        }

        private string GetAttendeeInfoType(PatientAttendeeProfileViewModel attendee)
        {
            string attendeeType = "";
            if (attendee != null && attendee.EmailAddress != null)
                attendeeType = "email: "+ attendee.EmailAddress;
            if (attendee != null && attendee.PrimaryPhone != null)
                attendeeType = "phone: " + attendee.PrimaryPhone;
            return attendeeType;
        }

    }
}