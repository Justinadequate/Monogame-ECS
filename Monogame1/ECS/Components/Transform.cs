using Microsoft.Xna.Framework;

namespace Monogame1.ECS.Components
{
    public class Transform : Component
    {
        public Vector2 Position;
        public Vector2 Scale;
        public float Rotation;
        public Rectangle Destination;

        public Transform(Vector2 position)
        {
            Position = position;
            Scale = Vector2.One;
            Rotation = 0f;
        }
    }
}