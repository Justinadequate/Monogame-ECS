using Microsoft.Xna.Framework;

namespace Monogame1.ECS.Components
{
    public class Physics : Component
    {
        public Vector2 Velocity;
        public float Speed = 1f;
        public float Friction;
        public bool IsStatic = false;
    }
}