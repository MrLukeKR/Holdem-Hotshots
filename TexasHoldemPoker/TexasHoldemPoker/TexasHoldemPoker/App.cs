using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using Urho.Forms;

using Xamarin.Forms;

namespace TexasHoldemPoker
{
	public class App : Application
    {
	    public App ()
		{
            MainPage = new GamePage();
            // The root page of your application

        }

		protected override void OnStart ()
		{
            // Handle when your app starts
            Debug.WriteLine("Starting the App");
        }

		protected override void OnSleep ()
		{
			// Handle when your app sleeps
		}

		protected override void OnResume ()
		{
			// Handle when your app resumes
		}
	}
}
