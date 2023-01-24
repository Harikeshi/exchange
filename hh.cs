using System;
using System.Web;
using System.Net;
using System.IO;

using System.Collections.Specialized;
using System.Linq;
using System.Text;


class Program
{
	/*
	public CookieAwareWebClient:WebClient
	{
		public void Login(string loginPageAddress, NameValueCollection loginData )
		{
			
			CookieContainer container;
			var request = (HttpWebRequest)WebRequest.Crate(loginPageAddress);
			request.Method = "POST";
			request.ContentType = "application/x-www-form-urlencoded";
			var query = string.Join("&", loginData.Cast<string>().Select(key=> $"{key}={loginData[key]}"));
			
			var buffer = Encoding.ASCII.GetBytes(query);
			request.ContentLength = buffer.Length;
			var requestStrem = request.GetRequestStream();
			requestStrem.Write(buffer, 0, buffer.Length);
			requestStrem.Close();
			
			CookieContainer = container;
			
		}
		public CookieAwareWebClient(CookieContainer container){
			CookieContainer = container;
		}
		public CookieAwareWebClient():this(new CookieContainer()){}
		
		public CookieContainer CookieContainer{get;private set;}
		
		protected override WebRequest GetWebRequest(Uri addressress)
		{
			var request = (HttpWebRequest)base.GetWebRequest(address);
			request.CookieContainer = CookieContainer;
			return request;
		}
	}
	*/
	
	
	static void Main(string[] args)
	{
		/*
		System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.ss | SecurityProtocolType.Tls12;
		
		var login = "www.mail.ru";
		var data = new NameValueCollection
		{
			{"User","SCHEGOLIHIN"},
				{"SessionKey","1814825902"},
					{"vLogN","schegolihin"},
						{"vLogP","Sergey12"}
			
		};
		var client = new CookieAwareWebClient();
		client.Login(login, data);
		*/
		
        if (args == null || args.Length == 0)
        {
            throw new ApplicationException ("Specify the URI of the resource to retrieve.");
        }
        WebClient client = new WebClient ();

        // Add a user agent header in case the
        // requested URI contains a query.

        client.Headers.Add ("user-agent", "Mozilla/4.0 (compatible; MSIE 7.0; Windows NT 10.0; WOW64; Trident/7.0; .NET4.0C; .NET4.0E; .NET CLR 2.0.50727; .NET CLR 3.0.30729; .NET CLR 3.5.30729; wbx 1.0.0; wbxapp 1.0.0)");
		client.Headers.Add(HttpRequestHeader.Cookie, "User=SCHEGOLIHIN;" + "SessionKey=1814825902");

		System.Net.ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3| SecurityProtocolType.Tls12;
        
		Stream data = client.OpenRead (@"https://alpha.rshb.ru/rshb/Web_ATMINC.Find_Incas?vTERM_ID=S1AM9463&vCUR=810&vDATE_FROM=09/08/2021&nDetal=6&vDATE_TO=16/09/2021");
        StreamReader reader = new StreamReader (data);
        string s = reader.ReadToEnd ();
        Console.WriteLine (s);
		
	
		
		
		
		
        data.Close ();
        reader.Close ();
	}
	
}