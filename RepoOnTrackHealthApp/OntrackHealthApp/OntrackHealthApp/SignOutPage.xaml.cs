using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace OntrackHealthApp
{
	[XamlCompilation(XamlCompilationOptions.Compile)]
	public partial class SignOutPage : ContentPage
	{
		public SignOutPage ()
		{
			InitializeComponent ();
            ProcedureName.Text = "Sign Out";
        }
	}
}