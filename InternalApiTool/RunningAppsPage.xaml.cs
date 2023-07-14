using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Xml;
using System.Xml.Linq;


namespace InternalApiTool;

public partial class RunningAppsPage : ContentPage
{
	public RunningAppsPage()
	{
		InitializeComponent();   
	}

	public RunningAppsPage(string header, string url)
	{   
        InitializeComponent();
        BindingContext = new RunningAppsViewModel(header, url);
	}
}

