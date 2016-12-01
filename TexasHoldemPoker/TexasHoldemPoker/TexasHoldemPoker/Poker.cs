using TexasHoldemPoker.Game;
using Urho;
using Urho.Actions;
using Urho.Audio;
using Urho.Gui;

namespace TexasHoldemPoker
{
    class Poker : Urho.Application
    {
        public Poker() : base(new ApplicationOptions(assetsFolder: "Data")) { }
        
        Scene scene;
        Camera camera;
        Node CameraNode;
        Node TargetNode;
        Vector3 initialCameraPos;

        protected override void Start()
        {
            base.Start();
            scene = SceneGenerator.LoadMenuScene(ResourceCache);

            CameraNode = scene.GetChild("MainCamera", true);
            TargetNode = scene.GetChild("PokerTable", true);

            camera = CameraNode.GetComponent<Camera>();
            initialCameraPos = CameraNode.Position;

            Controller controller = new Controller();

            controller.SetCameraNode(CameraNode);
            controller.SetInitialCameraPosition(initialCameraPos);
            controller.SetTargetNode(TargetNode);
            controller.SetUI(UI);

            CameraHandler.rotateCamera(CameraNode, TargetNode); //Figure out a way of playing this on devices that can handle it
            UIGenerator.LoadMenuUI(ResourceCache,UI,Graphics, controller);
            SetupViewport();
        }
        
      

        private void SetupViewport()
        {
            Renderer.SetViewport(0, new Viewport(Context, scene, camera, null));
        }

      
    }
}