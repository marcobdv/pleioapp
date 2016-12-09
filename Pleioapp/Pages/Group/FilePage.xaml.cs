using System;
using System.Collections.ObjectModel;
using Xamarin.Forms;

namespace Pleioapp
{
	public partial class FilePage : ContentPage
	{
	    private readonly App _app = (App)Application.Current;
	    private readonly ObservableCollection<FSObject> _files = new ObservableCollection<FSObject>();
	    private readonly FSObject _parentFolder;
	    private Group _group;

	    /*The root of each tab in your TabbedPage should be a single NavigationPage, which wraps a ContentPage. From that ContentPage you navigate to other pages by using the Navigation property of the page.*/

	    public FilePage(Group group, FSObject parentFolder):this()
        {
            _parentFolder = parentFolder;
            _group = group;
            Title = parentFolder.title;
            Reload();
        }

	    public FilePage()
		{
			InitializeComponent();
			FileListView.ItemsSource = _files;

		    FileListView.ItemSelected += FileListViewOnItemSelected;



			CouldNotLoad.GestureRecognizers.Add(CreateReloadGesture());

			MessagingCenter.Subscribe<Application>(Application.Current, "select_group",GroupChangedCallback);
		}

	    private void GroupChangedCallback(Application application)
	    {
	        SetGroup(_app.CurrentGroup);

	    }

	    private TapGestureRecognizer CreateReloadGesture()
	    {
	        return new TapGestureRecognizer
	        {
	            Command = new Command(Reload),
	            NumberOfTapsRequired = 1
	        };
	    }

	    private void FileListViewOnItemSelected(object sender, SelectedItemChangedEventArgs e)
	    {
            var selectedEvent = e.SelectedItem as FSObject;
	        if (selectedEvent == null) return;
	        if (IsFolder(selectedEvent))
	        {
	            Navigation.PushAsync(new FilePage(_group, selectedEvent));
	        }
	        else
	        {
	            if (selectedEvent.url != null)
	            {
	                _app.SsoService.OpenUrl(selectedEvent.url);
	            }
	        }
	        ((ListView) sender).SelectedItem = null;
	    }

	    private static bool IsFolder(FSObject selectedEvent)
	    {
	        return selectedEvent.subtype.Equals("folder", StringComparison.OrdinalIgnoreCase);
	    }

	    public async void Reload()
		{
			CouldNotLoad.IsVisible = false;
			NoItems.IsVisible = false;
			_files.Clear();

			
			var service = _app.WebService;

			try
			{
				var webFiles = await service.GetFiles(_group,_parentFolder);

				foreach (var e in webFiles)
				{
					_files.Add(e);
				}

				if (_files.Count == 0)
				{
					NoItems.IsVisible = true;
				}
			}
			catch (Exception e)
			{
				CouldNotLoad.IsVisible = true;
				System.Diagnostics.Debug.WriteLine("Catched exception " + e);
			}
		}

	    public async void SetGroup(Group group)
		{
			_group = group;
			if (group == null)
			{
				_files.Clear();
				return;
			}

			Reload();
		}
	}
}

