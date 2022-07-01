using Microsoft.Xna.Framework;

namespace Monogame1.ECS.Components
{
    public class Physics : Component
    {
        public Vector2 Velocity;
        public float MaxSpeed = 5f;
        public float Acceleration = 0.2f;
    }
}