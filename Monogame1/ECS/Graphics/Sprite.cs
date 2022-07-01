using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class Sprite : Component
    {
        public Texture2D Texture { get; set; }
        public Color Color { get; set; }

        public Sprite (Texture2D texture)
        {
            Texture = texture;
            Color = Color.White;
        }
    }
}