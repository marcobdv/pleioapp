using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pleioapp
{
	public partial class GroupPage : TabbedPage
	{
		ActivityPage activityPage;
		EventPage eventPage;
		MemberPage memberPage;
		FilePage filePage;

		public GroupPage ()
		{
			InitializeComponent ();

			activityPage = new ActivityPage ();
			eventPage = new EventPage ();
			memberPage = new MemberPage ();
			filePage = new FilePage();

			Children.Add (activityPage);
			Children.Add (eventPage);
			Children.Add (memberPage);
			Children.Add (filePage);

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "select_group", async(sender) => {
				var app = (App)App.Current;
				BindingContext = app.CurrentGroup;
			});
		}
	}
}

