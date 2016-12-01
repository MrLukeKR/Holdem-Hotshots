using System;
using System.Collections.Generic;
using System.Text;
using Urho.Gui;
using Urho.Resources;
using Urho;

namespace TexasHoldemPoker.Game
{
    static class UIGenerator
    {

        public static void LoadMenuUI(ResourceCache cache, UI UI, Graphics Graphics, Controller controller)
        {
            var copyrightNotice = new Text();
            var gameTitle = new BorderImage();
            var settingsButton = new Button();
            var joinButton = new Button();
            var hostButton = new Button();
            var infoButton = new Button();
            var createLobbyButton = new Button();
            var joinLobbyButton = new Button();

            copyrightNotice.Value = "Copyright © Advantage Software Group 2016. All Rights Reserved.";
            copyrightNotice.HorizontalAlignment = HorizontalAlignment.Center;
            copyrightNotice.VerticalAlignment = VerticalAlignment.Bottom;
            copyrightNotice.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            copyrightNotice.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);

            gameTitle.Texture = cache.GetTexture2D("Textures/gameTitle.png");
            gameTitle.BlendMode = BlendMode.Replace;
            gameTitle.SetSize((Graphics.Width / 5) * 4, (Graphics.Width / 5) * 2);
            gameTitle.SetPosition((Graphics.Width / 2) - (gameTitle.Width / 2), Graphics.Height / 8);

            settingsButton.Texture = cache.GetTexture2D("Textures/settingsButton.png"); // Set texture
            settingsButton.BlendMode = BlendMode.Replace;
            settingsButton.SetSize(Graphics.Width / 10, Graphics.Width / 10);
            settingsButton.SetPosition(Graphics.Width - settingsButton.Width - 20, 20);
            settingsButton.Name = "Settings";
            settingsButton.Pressed += controller.SettingsButton_Pressed;

            infoButton.Texture = cache.GetTexture2D("Textures/infoButton.png"); // Set texture
            infoButton.BlendMode = BlendMode.Replace;
            infoButton.SetSize(50, 50);
            infoButton.SetPosition(Graphics.Width - infoButton.Width - 20, Graphics.Height - infoButton.Height - 20);
            infoButton.Name = "About";
            infoButton.Pressed += controller.InfoButton_Pressed; ;

            joinButton.Texture = cache.GetTexture2D("Textures/joinGameButton.png"); // Set texture
            joinButton.BlendMode = BlendMode.Replace;
            joinButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            joinButton.SetPosition(((Graphics.Width - joinButton.Width) / 5), (Graphics.Height / 4) * 3);
            joinButton.Name = "JoinGame";
            joinButton.Pressed += controller.JoinButton_Pressed;

            hostButton.Texture = cache.GetTexture2D("Textures/hostGameButton.png"); // Set texture
            hostButton.BlendMode = BlendMode.Replace;
            hostButton.SetSize(Graphics.Width / 3, (Graphics.Width / 4) / 2);
            hostButton.SetPosition(((Graphics.Width - hostButton.Width) / 5) * 4, (Graphics.Height / 4) * 3);
            hostButton.Name = "HostGame";
            hostButton.Pressed += controller.HostButton_Pressed;

            createLobbyButton.Texture = cache.GetTexture2D("Textures/createLobbyButton.png"); // Set texture
            createLobbyButton.BlendMode = BlendMode.Replace;
            createLobbyButton.SetSize((Graphics.Width / 3) * 2, Graphics.Width / 5);
            createLobbyButton.SetPosition(Graphics.Width / 2 - createLobbyButton.Width / 2, (Graphics.Height / 4) * 3);
            createLobbyButton.Name = "CreateLobby";
            createLobbyButton.Pressed += controller.CreateLobbyButton_Pressed;

            createLobbyButton.Visible = false;
            createLobbyButton.Enabled = false;

            joinLobbyButton.Texture = cache.GetTexture2D("Textures/joinLobbyButton.png"); // Set texture
            joinLobbyButton.BlendMode = BlendMode.Replace;
            joinLobbyButton.SetSize((Graphics.Width / 3) * 2, Graphics.Width / 5);
            joinLobbyButton.SetPosition(Graphics.Width / 2 - createLobbyButton.Width / 2, (Graphics.Height / 4) * 3);
            joinLobbyButton.Name = "JoinLobby";
            joinLobbyButton.Pressed += controller.JoinLobbyButton_Pressed;

            joinLobbyButton.Visible = false;
            joinLobbyButton.Enabled = false;

            Text hostText = new Text()
            {
                Value = "HOST MENU WILL GO HERE",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };


            hostText.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            hostText.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            hostText.Visible = false;

            Text joinText = new Text()
            {
                Value = "JOIN MENU WILL GO HERE",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            joinText.SetColor(new Color(1.0f, 1.0f, 1.0f, 0.8f));
            joinText.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            joinText.Visible = false;

            Button backButton = new Button();
            backButton.Texture = cache.GetTexture2D("Textures/backButton.png"); // Set texture
            backButton.BlendMode = BlendMode.Add;
            backButton.SetSize(Graphics.Width / 10, Graphics.Width / 10);
            backButton.SetPosition(20, 20);
            backButton.Name = "Back";
            backButton.Pressed += controller.BackButton_Pressed;

            backButton.Visible = false;
            backButton.Enabled = false;

            var window = new Window()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            window.SetSize((Graphics.Width / 3) * 2, Graphics.Height / 3);

            var title = new Text()
            {
                Value = "About",
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };

            var scroller = new ScrollView()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom
            };

            scroller.SetSize(window.Width, (window.Height / 5) * 4);
            scroller.ScrollBarsAutoVisible = true;
            scroller.SetScrollBarsVisible(false, true);

            var logo = new BorderImage()
            {
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Top
            };


            logo.Texture = cache.GetTexture2D("Textures/advantageLogo.png");
            logo.BlendMode = BlendMode.Replace;
            logo.SetSize((window.Width / 2), (window.Height / 5));

            var aboutContent = new Text()
            {
                Value = "\n\n\n\n\n\nGAME NAME GOES HERE\nVersion 0.0.5\n\nA Mixed Reality Texas Hold 'em Game\nby\nAdvantage Software Group\n\nAuthors\nLuke Rose, Jack Nicholson, Xinyi Li, Michael Uzoka, George Thomas, Rick Jin\n\nCoordinator\nDr. Peter Blanchfield, The University of Nottingham\n\nSupervisor\nDr. Thorsten Altenkirch, The University of Nottingham",
                TextAlignment = HorizontalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Bottom,
                Wordwrap = true
            };

            aboutContent.SetSize(window.Width, 300);

            var about = new ListView();

            about.SetSize(window.Width, (window.Height / 5) * 4);
            about.SetStyleAuto(null);

            aboutContent.SetFont(cache.GetFont("Fonts/arial.ttf"), 10);
            aboutContent.SetColor(Color.Black);
            aboutContent.SetSize(window.Width, (window.Height / 4) * 3);

            about.AddItem(logo); //TODO: Figure out how to add items to the list one after the other, with a scrollbar

            about.AddItem(aboutContent);

            title.SetFont(cache.GetFont("Fonts/arial.ttf"), 25);
            title.SetPosition(0, 20);
            title.SetColor(Color.Black);


            window.AddChild(title);
            window.AddChild(about);
            window.Opacity = 0.5f;

            window.Visible = false;

            backButton.SetStyleAuto(null);
            settingsButton.SetStyleAuto(null);
            joinButton.SetStyleAuto(null);
            hostButton.SetStyleAuto(null);
            infoButton.SetStyleAuto(null);
            createLobbyButton.SetStyleAuto(null);
            joinLobbyButton.SetStyleAuto(null);

            UI.Root.AddChild(gameTitle);        //Index = 0
            UI.Root.AddChild(copyrightNotice);  //Index = 1
            UI.Root.AddChild(joinButton);       //Index = 2
            UI.Root.AddChild(hostButton);       //Index = 3
            UI.Root.AddChild(settingsButton);   //Index = 4
            UI.Root.AddChild(infoButton);        //Index = 5

            UI.Root.AddChild(hostText);         //Index = 6
            UI.Root.AddChild(joinText);         //Index = 7

            UI.Root.AddChild(backButton);       //Index = 8

            UI.Root.AddChild(window);           //Index = 9

            UI.Root.AddChild(createLobbyButton); //Index = 10
            UI.Root.AddChild(joinLobbyButton); //Index = 11
        }

        internal static void LoadMenuScene(UI UI)
        {
            for (uint i = 0; i < 4; i++)
                UI.Root.GetChild(i).Visible = true;


            UI.Root.GetChild(5).Visible = true;
            UI.Root.GetChild(5).Enabled = true;
            UI.Root.GetChild(6).Visible = false;
            UI.Root.GetChild(7).Visible = false;
            UI.Root.GetChild(8).Visible = false;
            UI.Root.GetChild(8).Enabled = false;

            UI.Root.GetChild("CreateLobby", true).Visible = false;
            UI.Root.GetChild("CreateLobby", true).Enabled = false;
            UI.Root.GetChild("JoinLobby", true).Visible = false;
            UI.Root.GetChild("JoinLobby", true).Enabled = false;
        }
    }
}
