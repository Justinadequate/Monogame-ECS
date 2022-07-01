using Microsoft.Xna.Framework;

namespace Monogame1.ECS.Components
{
    public class Physics : Component
    {
        public Vector2 Velocity;
        public float Speed = 1f;
        public float Friction = 1f;
        public float Gravity = 1f;
        public bool Collision;
        public bool IsStatic = false;
    }
}