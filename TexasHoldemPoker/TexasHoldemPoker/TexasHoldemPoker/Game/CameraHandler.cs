using System;
using System.Collections.Generic;
using System.Text;
using Urho;
using Urho.Actions;

namespace TexasHoldemPoker.Game
{
    static class CameraHandler
    {

        public static void panToOriginalPosition(Vector3 initialCameraPos,Node CameraNode, Node TargetNode)
        {
            CameraNode.RunActions(
                 new Parallel(
                     new MoveTo(1, initialCameraPos), new RotateTo(1, 20f, 0f, 0f)
                 )
             );
            CameraNode.LookAt(TargetNode.Position, Vector3.Up, TransformSpace.World);
        }

        public static void panToHost(Node CameraNode)
        {
            CameraNode.RunActions(
                 new Parallel(
                     new MoveTo(1, new Vector3(0.00544398f, 0.176587f, 0.159439f)), new RotateTo(1, 60f, -180f, 0f)
                 )
             );
        }

        public static void panToJoin(Node CameraNode)
        {
            CameraNode.RunActions(
               new Parallel(
                   new MoveTo(1, new Vector3(0f, 0.106208f, -0.139909f)), new RotateTo(1, 20f, 0f, 0f)
               )
           );
        }

        public static async void rotateCamera(Node CameraNode, Node TargetNode)
        {
            await CameraNode.RunActionsAsync(
             new RepeatForever(
                 new RotateAroundBy(60, TargetNode.Position, 0.0f, 360.0f, 0.0f, TransformSpace.World)
             )
            );
        }
    }
}
