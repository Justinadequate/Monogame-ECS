using Microsoft.Xna.Framework;

namespace Monogame1.OO.Models
{
    public class Character : AnimatedSprite
    {
        public Character(Vector2 position, string name) : base(position, name)
        {
            _position = position;
        }
    }
}