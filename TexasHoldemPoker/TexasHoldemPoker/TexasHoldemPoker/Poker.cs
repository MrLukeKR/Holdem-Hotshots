using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Gui;
using Urho.Urho2D;

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
                Value = "Texas Hold 'em Poker",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };
            helloText.SetColor(new Color(1f, 0f, 0f));
            helloText.SetFont(font: cache.GetFont("Fonts/Anonymous Pro.ttf"), size: 30);
            UI.Root.AddChild(helloText);

            Graphics.SetWindowIcon(cache.GetImage("Textures/UrhoIcon.png"));
            Graphics.WindowTitle = "Texas Hold 'em Poker";



            var joinButton = new Button();
            var tex = new Texture2D();
            tex = cache.GetTexture2D("Textures/test.png");
            if (tex != null) { 

            joinButton.Texture = tex; // Set texture
            joinButton.BlendMode = BlendMode.Add;
            joinButton.SetSize(256, 128);
            joinButton.SetPosition((Graphics.Width - joinButton.Width) / 5, Graphics.Height - 300);
            joinButton.Name = "JoinGame";
        }

            var hostButton = new Button();
            hostButton.Texture = cache.GetTexture2D("Textures/test.png"); // Set texture
            hostButton.BlendMode = BlendMode.Add;
            hostButton.SetSize(256, 128);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, Graphics.Height - 300);
            hostButton.Name = "HostGame";

            joinButton.SetStyleAuto(null);
            hostButton.SetStyleAuto(null);

            UI.Root.AddChild(joinButton);
            UI.Root.AddChild(hostButton);
        }
    }
}