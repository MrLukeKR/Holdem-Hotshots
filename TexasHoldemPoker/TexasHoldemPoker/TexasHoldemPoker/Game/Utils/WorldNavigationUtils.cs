using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Gui;

namespace TexasHoldemPoker.Game.Utils
{
    static class WorldNavigationUtils
    {
        static Graphics graphics;
        static UI ui;

        public static Node GetNodeAt(IntVector2 touchPosition, Scene scene)
        {
            var CameraNode = scene.GetChild("MainCamera", true);
            var camera = CameraNode.GetComponent<Camera>();

            var pos = Vector3.Zero;
            if (ui.GetElementAt(touchPosition, true) == null)
            {
                Ray cameraRay = camera.GetScreenRay(
                  (float)touchPosition.X / graphics.Width,
                  (float)touchPosition.Y / graphics.Height);
                var result = scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 10, DrawableFlags.Geometry, uint.MaxValue);
                if (result != null) return result.Value.Node;
            }
            return null;
        }



        public static Vector3 GetScreenToWorldPoint(int x, int y, float z, Camera camera)
        {
            Vector3 a = camera.ScreenToWorldPoint(new Vector3(x - (graphics.Width / 2), y - (graphics.Height / 2), 0));
            a.Z = z;
            return a;
        }

        public static Vector3 GetScreenToWorldPoint(IntVector2 ScreenPos, float z, Camera camera)
        {
            /*
            // Would this be more reliable and reduce code repetition? - GRT
            return GetScreenToWorldPoint(ScreenPos.X, ScreenPos.Y, z)
            */
            Vector3 a = camera
              .ScreenToWorldPoint(new Vector3(ScreenPos.X - (graphics.Width / 2),
                                  ScreenPos.Y - (graphics.Height / 2), 0));
            a.Z = z;
            return a;
        }
    }
}
