using System;
using Urho;
using Urho.Gui;

namespace HoldemHotshots
{
    static class PositionUtils
    {
        public static Graphics graphics { get; set; }
        public static UI ui {get; set;}

		public static void SetReferences(Graphics currGraphics, UI currUI)
		{
			graphics = currGraphics;
			ui = currUI;
		}

        public static Node GetNodeAt(IntVector2 touchPosition, Scene scene)
        {
            var CameraNode = scene.GetChild("MainCamera", true);
			var camera = CameraNode.GetComponent<Camera>();
            if (ui.GetElementAt(touchPosition, true) == null)
            {
                Ray cameraRay = camera.GetScreenRay(
                  (float)touchPosition.X / graphics.Width,
                  (float)touchPosition.Y / graphics.Height);
                var result = scene.GetComponent<Octree>().RaycastSingle(cameraRay, RayQueryLevel.Triangle, 15, DrawableFlags.Geometry, uint.MaxValue);
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
            Vector3 a = camera.ScreenToWorldPoint(new Vector3(ScreenPos.X - (graphics.Width / 2),
                                  ScreenPos.Y - (graphics.Height / 2), 0));
            a.Z = z;
            return a;
        }

			public static void InitPlayerCardPositions(Camera camera)
		{
            Card.card1ViewingPos = new Vector3(-1.1f, 2, 0);
            Card.card1HoldingPos = new Vector3(2.65f, -2.1f, 0.1f);

            Card.card2ViewingPos = new Vector3(1.1f, 2, 0);
            Card.card2HoldingPos = new Vector3(3.1f, -2, 0.0f);
        }

		public static void InitTableCardPositions(Camera camera)
		{
			Card.cardTableDealingPos = GetScreenToWorldPoint(0, graphics.Height / 2, 0.065f, camera);

			Card.card1TablePos = GetScreenToWorldPoint((graphics.Width / 2), (graphics.Height / 2) - 2, 0.065f, camera);
			Card.card1TablePos.Y += (1.4f * 0.009f) * 1.5f;
			Card.card1TablePos.X += 0.009f * 1.5f;

			Card.card2TablePos = GetScreenToWorldPoint((graphics.Width / 2), (graphics.Height / 2) - 1, 0.065f, camera);
			Card.card2TablePos.Y += (1.4f * 0.009f) * 1.5f;
			Card.card2TablePos.X += 0.009f * 1.5f;

			Card.card3TablePos = GetScreenToWorldPoint((graphics.Width / 2), (graphics.Height / 2), 0.065f, camera);
			Card.card3TablePos.Y += (1.4f * 0.009f) * 1.5f;
			Card.card3TablePos.X += 0.009f * 1.5f;

			Card.card4TablePos = GetScreenToWorldPoint((graphics.Width / 2), (graphics.Height / 2) + 1, 0.065f, camera);
			Card.card4TablePos.Y += (1.4f * 0.009f) * 1.5f;
			Card.card4TablePos.X += 0.009f * 1.5f;

			Card.card5TablePos = GetScreenToWorldPoint((graphics.Width / 2), (graphics.Height / 2) + 2, 0.065f, camera);
			Card.card5TablePos.Y += (1.4f * 0.009f) * 1.5f;
			Card.card5TablePos.X += 0.009f * 1.5f;
		}
    }
}
