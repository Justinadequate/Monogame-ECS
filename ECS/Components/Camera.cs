using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class Camera : Component
    {
        public Vector2 Direction { get; set; }
        public Vector2 Velocity { get; set; }
        public float MoveSpeed { get; set; }
        public float TurnSpeed { get; set; }

        public Camera()
        {
            Direction = new Vector2(0, 1);
            MoveSpeed = 100f;
            TurnSpeed = 5f;
        }
    }
}