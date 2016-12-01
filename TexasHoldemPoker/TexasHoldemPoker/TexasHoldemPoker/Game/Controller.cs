using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Gui;

namespace TexasHoldemPoker.Game
{
    class Controller
    {
        Vector3 initialCameraPos;
        Node CameraNode;
        Node TargetNode;
        UI UI;

        public void SetCameraNode(Node Camera)
        {
            CameraNode = Camera;
        }

        public void SetInitialCameraPosition(Vector3 position)
        {
            initialCameraPos = position;
        }

        public void SetTargetNode(Node Target)
        {
            TargetNode = Target;
        }

        public void SetUI(UI ui)
        {
            UI = ui;
        }
        
        public void HostButton_Pressed(PressedEventArgs obj)
        {
            //Do host game stuff
            //TODO: Add intermediate host connection handling and setup
            SceneGenerator.LoadHostingScene(CameraNode, UI);

        }

        public void JoinButton_Pressed(PressedEventArgs obj)
        {
            //Do join game stuff
            //TODO: Add intermediate join connection handling and setup

            SceneGenerator.LoadJoiningScene(CameraNode, UI);
            CameraNode.RemoveAllActions();
            CameraHandler.panToJoin(CameraNode);
        }

        public void SettingsButton_Pressed(PressedEventArgs obj)
        {
            //Do settings stuff

        }

        public void BackButton_Pressed(PressedEventArgs obj)
        {
            UIGenerator.LoadMenuScene(UI);

            CameraHandler.panToOriginalPosition(initialCameraPos,CameraNode,TargetNode);
            CameraHandler.rotateCamera(CameraNode, TargetNode);
        }


        public void JoinLobbyButton_Pressed(PressedEventArgs obj)
        {

            //Load Lobby Scene
            //LoadLobbyScene();

            //Load Playing Scene

            //LoadPlayerScene();
        }

        public void CreateLobbyButton_Pressed(PressedEventArgs obj)
        {
            //Load Hosting Scene
            // LoadTableScene();
        }

        public  void InfoButton_Pressed(PressedEventArgs obj)
        {
            UI.Root.GetChild(9).Visible = !UI.Root.GetChild(9).Visible;

        }

    }
}
