using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Gui;

namespace TexasHoldemPoker
{
    class Poker : Urho.Application
    {
        public Poker(ApplicationOptions options = null) : base(options) { }
        Camera camera;
        Scene scene;
        protected override void Start()
        {
            base.Start();
            var cache = ResourceCache;
            var helloText = new Text()
            {
                Value = "Welcome to Texas Hold 'em Poker",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };
            helloText.SetColor(new Color(1f, 0f, 0f));
            helloText.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);
            UI.Root.AddChild(helloText);

            Graphics.SetWindowIcon(cache.GetImage("Textures/UrhoIcon.png"));
            Graphics.WindowTitle = "UrhoSharp Sample";

            // Subscribe to Esc key:
            Input.SubscribeToKeyDown(args => { if (args.Key == Key.Esc) Exit(); });
        }
    }
}