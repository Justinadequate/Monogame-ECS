using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Graphics
{
    public class Camera
    {
        public Vector2 Position { get; set; }
        public float Z { get; set; }
        public float BaseZ { get; set; }

        public float AspectRatio { get; set; }
        public float FoV { get; set; }

        public Matrix View { get; set; }
        public Matrix Projection { get; set; }

        public Camera()
        {
            
        }
    }
}