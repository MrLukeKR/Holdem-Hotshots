using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Forms;
using Xamarin.Forms;

namespace TexasHoldemPoker
{
    class GamePage : ContentPage
    {
        Poker pokerApp;
        UrhoSurface gameSurface;

        public GamePage()
        {
            gameSurface = new UrhoSurface();
            gameSurface.VerticalOptions = LayoutOptions.FillAndExpand;
            Title = "Mixed Reality Poker";
            Content = new StackLayout
            {
                Children = { gameSurface }
            };
            
        }

        protected override async void OnAppearing()
        {
            pokerApp = await gameSurface.Show<Poker>(new ApplicationOptions(assetsFolder: null));
        }
    }
}