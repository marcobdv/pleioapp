using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pleioapp
{
	public partial class GroupPage : TabbedPage
	{
		Group currentGroup;

		ActivityPage activityPage;
		EventPage eventPage;
		FilePage filePage;
		ProfilePage profilePage;

		public GroupPage ()
		{
			InitializeComponent ();

			activityPage = new ActivityPage ();
			eventPage = new EventPage ();
			filePage = new FilePage ();
			profilePage = new ProfilePage ();

			Children.Add (activityPage);
			Children.Add (eventPage);
			Children.Add (filePage);
			Children.Add (profilePage);
		}

		public void setGroup(Group group)
		{
			currentGroup = group;

			activityPage.setGroup (group);
			eventPage.setGroup (group);
			filePage.setGroup (group);
			profilePage.setGroup (group);
		}
	}
}

