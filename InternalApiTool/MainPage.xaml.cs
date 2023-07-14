
using System.Windows.Input;

namespace InternalApiTool;

public partial class MainPage : ContentPage
{
	private string Url, Header;

	public MainPage()
	{
		InitializeComponent();
		BindingContext = this;
	}

	private void Environment_SelectedIndexChanged(object sender, EventArgs e)
	{
		string selectedOption = Environment.SelectedItem.ToString();
		if (selectedOption.Equals("Test"))
		{
			Url = "<HIDDEN FOR SECURITY REASONS>";
		} 
		else if(selectedOption.Equals("Prod"))
		{
			Url = "<HIDDEN FOR SECURITY REASONS>";
		}
	}

	//This webservice requires login using username, password and 2fa id to be accessed
	private async void OnLoginClicked(object sender, EventArgs e)
	{
		if(! string.IsNullOrEmpty(Username.Text) && 
			!string.IsNullOrEmpty(Password.Text) && Environment.SelectedItem != null)
		{
			Header = $@"
					<soap12:Header>
						<Authentication xmlns=""http://www.<domainname>.com/WebServices"">
							<User>{Username.Text}</User>
							<Password>{Password.Text}</Password>
							<TwoFactorDeviceId>{Twofaid.Text}</TwoFactorDeviceId>
						</Authentication>  
                    </soap12:Header>
            ".Replace("\r\n\t","").Trim();

			await Navigation.PushAsync(new RunningAppsPage(Header, Url));
        } 
		else
		{
			await DisplayAlert("Alert", "Username, Password and Environment cannot be blank", "OK");
		}
		
	}
}

