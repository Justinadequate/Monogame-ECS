using Monogame1.OO.Models;

namespace Monogame1.ECS.Components
{
    public class Player : Component
    {
        public Input Input;
        public string PlayerState;

        public Player()
        {
            PlayerState = State.Idle;
        }
    }
}