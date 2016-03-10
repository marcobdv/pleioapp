using System;
using System.Collections.Generic;

using Xamarin.Forms;
using System.Collections.ObjectModel;

namespace Pleioapp
{
	public partial class MemberPage : ContentPage
	{
		ObservableCollection<User> members = new ObservableCollection<User>();
		App app = (App) App.Current;
		User SelectedItem = null;

		public MemberPage ()
		{
			InitializeComponent ();
			MemberListView.ItemsSource = members;
			MemberListView.ItemTapped += (sender, e) => {
				if (MemberListView.SelectedItem == SelectedItem) {
					if (SelectedItem.url != null) {
						app.ssoService.OpenUrl(SelectedItem.url);
					}
				}

				SelectedItem = (User) MemberListView.SelectedItem;
			};
		}

		public async void setGroup(Group group)
		{
			members.Clear ();

			var app = (App)App.Current;
			var service = app.webService;

			foreach (User member in await service.GetMembers (group)) {
				members.Add (member);
			}
		}
	}
}

