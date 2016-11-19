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
        public Poker() : base(new ApplicationOptions(assetsFolder: "Data")) { }

        protected override void Start()
        {
            base.Start();
            var cache = ResourceCache;
          
            Graphics.SetWindowIcon(cache.GetImage("Textures/GameIcon.png"));
            Graphics.WindowTitle = "Texas Hold 'em Poker";

            var copyrightNotice = new Text()
            {
                Value = "Copyright © Advantage Software Group 2016. All Rights Reserved.",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 15);

            var gameTitle = new BorderImage();
            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Add;
            gameTitle.SetSize(512, 256);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), gameTitle.Height / 2);

            var settingsButton = new Button();
            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Add;
            settingsButton.SetSize(50, 50);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "Settings";
            settingsButton.Opacity = 0.6f;

            var joinButton = new Button();
            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Add;
            joinButton.SetSize(256, 128);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), Graphics.Height - 300);
            joinButton.Name = "JoinGame";

            var hostButton = new Button();
            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Add;
            hostButton.SetSize(256, 128);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, Graphics.Height - 300);
            hostButton.Name = "HostGame";

            settingsButton.SetStyleAuto(null);
            joinButton.SetStyleAuto(null);
            hostButton.SetStyleAuto(null);

            UI.Root.AddChild(gameTitle);
            UI.Root.AddChild(settingsButton);
            UI.Root.AddChild(joinButton);
            UI.Root.AddChild(hostButton);
            UI.Root.AddChild(copyrightNotice);
        }
    }
}