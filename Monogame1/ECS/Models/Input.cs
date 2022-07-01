using Microsoft.Xna.Framework.Input;

namespace Monogame1.ECS.Models
{
    public class Input
    {
        public Keys Up { get; set; }
        public Keys Down { get; set; }
        public Keys Left { get; set; }
        public Keys Right { get; set; }
        public Keys Attack { get; set; }
    }
    
    public static class State
    {
        public const string Idle = "Idle";
        public const string Walking = "Walk";
        public const string Attacking = "Attack";
    }
}