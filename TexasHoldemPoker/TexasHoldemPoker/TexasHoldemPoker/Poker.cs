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
          
            //Graphics.SetWindowIcon(cache.GetImage("Textures/test.png"));
            Graphics.WindowTitle = "Texas Hold 'em Poker";
            
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

            joinButton.SetStyleAuto(null);
            hostButton.SetStyleAuto(null);

            UI.Root.AddChild(joinButton);
            UI.Root.AddChild(hostButton);
        }
    }
}