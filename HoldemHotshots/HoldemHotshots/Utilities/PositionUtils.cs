using Urho;
using Urho.Gui;

namespace HoldemHotshots.Utilities
{
    /// <summary>
    /// Performs position-based calculations from the screen and 3D space
    /// </summary>
    static class PositionUtils
    {
        public static Graphics graphics { get; set; }
        public static UI ui {get; set;}

        /// <summary>
        /// Sets access references
        /// </summary>
        /// <param name="currGraphics">Graphics System</param>
        /// <param name="currUI">User Interface System</param>
		public static void SetReferences(Graphics currGraphics, UI currUI)
		{
			graphics = currGraphics;
			ui = currUI;
		}

        /// <summary>
        /// Returns the node at a given X,Y screen coordinate
        /// </summary>
        /// <param name="touchPosition">XY coordinate on screen</param>
        /// <param name="scene">Scene to check</param>
        /// <returns>Node found at given position</returns>
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

        /// <summary>
        /// Converts a 2D XY coordinate into 3D coordinates
        /// </summary>
        /// <param name="x">X coord</param>
        /// <param name="y">Y coord</param>
        /// <param name="z">Z coord (constant)</param>
        /// <param name="camera">Camera object to use for tracing</param>
        /// <returns>3D XYZ coordinates</returns>
        public static Vector3 GetScreenToWorldPoint(int x, int y, float z, Camera camera)
        {
            Vector3 a = camera.ScreenToWorldPoint(new Vector3(x - (graphics.Width / 2), y - (graphics.Height / 2), 0));
            a.Z = z;
            return a;
        }

        /// <summary>
        ///Overloaded version of the GetScreenToWorldPoint that uses a 2D coord and a depth value
        /// </summary>
        /// <param name="ScreenPos">XY coord</param>
        /// <param name="z">Depth value</param>
        /// <param name="camera">Camera to use for tracing</param>
        /// <returns>3D XYZ coordinates</returns>
        public static Vector3 GetScreenToWorldPoint(IntVector2 ScreenPos, float z, Camera camera)
        {
            Vector3 a = camera.ScreenToWorldPoint(new Vector3(ScreenPos.X - (graphics.Width / 2),
                                  ScreenPos.Y - (graphics.Height / 2), 0));
            a.Z = z;
            return a;
        }
    }
}
