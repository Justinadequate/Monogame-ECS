using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class Camera : Component
    {
        public Texture2D Texture { get; set; }
        public Vector2 Position { get; set; }
        public float Rotation { get; set; }
        public float MoveSpeed { get; set; }
        public float TurnSpeed { get; set; }

        public Camera(Vector2 position)
        {
            Position = position;
            Rotation = 0f;
            MoveSpeed = 1f;
            TurnSpeed = 0.05f;
        }
    }
}