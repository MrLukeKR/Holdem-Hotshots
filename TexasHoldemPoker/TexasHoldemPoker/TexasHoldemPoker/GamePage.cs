using System;
using System.Collections.Generic;
using System.Text;
using Urho.Forms;
using Xamarin.Forms;

namespace TexasHoldemPoker
{
    class GamePage : ContentPage
    {

        Slider selectedBarSlider, rotationSlider;
        UrhoSurface gameSurface;

        public GamePage()
        {
            gameSurface = new UrhoSurface();


            rotationSlider = new Slider(0, 500, 250);

            selectedBarSlider = new Slider(0, 5, 2.5);

            Title = " UrhoSharp + Xamarin.Forms";
            Content = new StackLayout
            {
                Padding = new Thickness(12, 12, 12, 40),
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = {
        rotationSlider,
        new Label { Text = "SELECTED VALUE:" },
        selectedBarSlider,
      }
            };
        }
    }
    }

