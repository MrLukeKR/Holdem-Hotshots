using Urho;
using Urho.Forms;

using Xamarin.Forms;

namespace TexasHoldemPoker
{
    public class App : Xamarin.Forms.Application
    {
        public App()
        {
            MainPage = new GamePage();
        }
    }

    public class GamePage : ContentPage
    {
        UrhoSurface urhoSurface;
        Poker pokerApp;

         
        public GamePage()
        {
            urhoSurface = new UrhoSurface();
            urhoSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            Title = "Mixed Reality Texas Hold 'em Poker";
            Content = new StackLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {urhoSurface}
            };
        }

        protected override void OnDisappearing()
        {
            UrhoSurface.OnDestroy();
            base.OnDisappearing();
        }

        protected override async void OnAppearing()
        {
            StartPokerApp();
        }

        async void StartPokerApp()
        {
            pokerApp = await urhoSurface.Show<Poker>(new ApplicationOptions(assetsFolder: null) { Orientation = ApplicationOptions.OrientationType.Portrait });
        }
    }
}
