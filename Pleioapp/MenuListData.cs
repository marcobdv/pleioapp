using System;
using Xamarin.Forms;
using System.Collections.Generic;

namespace Pleioapp
{
	public class MenuListData: List<MenuItem>
	{
		public MenuListData ()
		{
			this.Add (new MenuItem () { 
				Title = "Activities", 
				IconSource = "activities.png", 
				TargetType = typeof(ActivityPage)
			});
			this.Add (new MenuItem () { 
				Title = "Files", 
				IconSource = "files.png", 
				TargetType = typeof(FilesPage)
			});

		}
	}
}

