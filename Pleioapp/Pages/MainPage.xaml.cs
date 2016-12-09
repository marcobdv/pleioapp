using Xamarin.Forms;

namespace Pleioapp
{
	public partial class MainPage : MasterDetailPage
	{
		public GroupPage GroupPage = new GroupPage ();
		public LeftMenu LeftMenu = new LeftMenu();
	    private readonly App _app = (App)Application.Current;
        public MainPage ()
		{
			InitializeComponent ();

			Master = LeftMenu;
            LeftMenu.SiteMenu.ItemSelected += SitesListViewOnItemSelected;
            LeftMenu.Menu.ItemSelected += MenuOnItemSelected;
            LeftMenu.IsVisible = true;
			Detail = new NavigationPage(GroupPage);
		}

        private async void SitesListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _app.CurrentSite = e.SelectedItem as Site;
            BindingContext = _app.CurrentSite;
            LeftMenu.Groups.Clear();
            LeftMenu.ToggleSubsiteMenu();
            await LeftMenu.GetGroups();
        }

        private void MenuOnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            _app.CurrentGroup = e.SelectedItem as Group;
            MessagingCenter.Send(Application.Current, "select_group");
            IsPresented = false;
        }
    }
}

