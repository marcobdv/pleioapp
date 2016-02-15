using System;
using System.Collections.Generic;

using Xamarin.Forms;

namespace Pleioapp
{
	public partial class GroupPage :  TabbedPage
	{
		public GroupPage ()
		{
			Children.Add (new ActivityPage ());
			Children.Add (new FilesPage ());
		}
	}
}

