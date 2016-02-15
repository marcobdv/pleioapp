using System;
using System.Collections.Generic;
using Xamarin.Forms;

namespace Pleioapp
{
	public partial class ActivityPage : ContentPage
	{
		public ActivityPage ()
		{
			Title = "Activiteiten";
			InitializeComponent ();
		}

		async void OnGetGroups(object sender, EventArgs e) {
			var service = new WebService ();
			await service.GetGroups ();
		}
	}
}

