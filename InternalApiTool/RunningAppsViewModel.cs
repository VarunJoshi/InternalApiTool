using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Xml;
using System.Text;

namespace InternalApiTool
{
    public class RunningAppsViewModel : INotifyPropertyChanged
    {
        private bool _isLoading;
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                _isLoading = value;
                OnPropertyChanged();
                OnPropertyChanged(nameof(ContentIsVisible));
            }
        }

        public bool ContentIsVisible => !_isLoading;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        private static readonly HttpClient httpClient = new();

        readonly DialogService _dialogService = new();

        public ObservableCollection<RunningApp> RunningApps { get; private set; } = new ObservableCollection<RunningApp>();

        public RunningAppsViewModel(string header, string url)
        {
            CreateRunningAppCollection(header, url);
        }

        async Task CreateRunningAppCollection(string header, string url)
        {  
            IsLoading = true;
            try
            {         
                //Call GetRunningApplications API
                string xmlRequest = @$"<?xml version=""1.0"" encoding=""utf-8""?>
                        <soap12:Envelope xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"" 
                            xmlns:soap12=""http://www.w3.org/2003/05/soap-envelope"">  
                            {header}
                            <soap12:Body>    
                                <GetRunningApplications xmlns=""http://www.alarm.com/WebServices"">
                                    <input>
                                        <LoadApplications>true</LoadApplications>
                                        <LoadWebsites>false</LoadWebsites> 
                                    </input>    
                                </GetRunningApplications>
                             </soap12:Body>
                        </soap12:Envelope>
                    ";


                HttpContent content = new StringContent(xmlRequest, Encoding.UTF8, "text/xml");
                using HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, url);
                request.Headers.Add("SOAPAction", "<URL FOR YOUR WEBSERVICE>");
                request.Content = content;
                using HttpResponseMessage response = await httpClient.SendAsync(request);
                var result = response.Content.ReadAsStreamAsync().Result;

                var doc = new XmlDocument();
                doc.Load(result);
                XmlNodeList appName = doc.GetElementsByTagName("AppName");
                if (appName.Count > 0)
                {
                    for (int i = 0; i < appName.Count; i++)
                    {
                        RunningApps.Add(new RunningApp
                        {
                            Name = appName[i].InnerText,
                            Rank = appName[i].NextSibling.InnerText == "0" ? "Active" : appName[i].NextSibling.InnerText,
                            MachineName = appName[i].NextSibling.NextSibling.InnerText
                        });
                    }
                }
                else
                {
                    throw new Exception(doc.InnerText);
                }
            }
            catch (Exception ex)
            {              
                await _dialogService.ShowMessage("Alert", $"Error sending xml request: {ex.Message}");
            }
            finally
            {
                IsLoading = false;
            }
        }
        
    }
}
