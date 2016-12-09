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
		Group Group;

		public MemberPage ()
		{
			InitializeComponent ();
			MemberListView.ItemsSource = members;

			MemberListView.ItemSelected += (sender, e) => {
				var member = e.SelectedItem as User;
				if (member.url != null) {
					app.SsoService.OpenUrl(member.url);
				}
			};
				
			CouldNotLoad.GestureRecognizers.Add (new TapGestureRecognizer {
				Command = new Command (() => {
					Reload();
				}),
				NumberOfTapsRequired = 1
			});

			MessagingCenter.Subscribe<Xamarin.Forms.Application> (App.Current, "select_group", async(sender) => {
				var app = (App)App.Current;
				setGroup(app.CurrentGroup);
			});
		}

		public async void Reload()
		{
			CouldNotLoad.IsVisible = false;
			NoItems.IsVisible = false;
			members.Clear ();

			var app = (App)App.Current;
			var service = app.WebService;

			try {
				var webMembers = await service.GetMembers (Group);

				foreach (User member in webMembers) {
					members.Add (member);
				}

				if (members.Count  == 0) {
					NoItems.IsVisible = true;
				}
			} catch (Exception e) {
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine ("Catched exception " + e);
			}

		}

		public async void setGroup(Group group)
		{
			Group = group;
			if (group == null) {
				members.Clear ();
				return;
			}
				
			Reload ();
		}
	}
}

