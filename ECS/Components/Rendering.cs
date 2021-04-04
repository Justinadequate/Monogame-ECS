using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class Rendering : Component
    {
        public Texture2D Texture { get; set; }
        public Color DrawColor { get; set;}
        public Rectangle SourceRectangle { get; set; }

        public Rendering(Texture2D texture)
        {
            Texture = texture;
            DrawColor = Color.White;
            SourceRectangle = Texture.Bounds;
        }

        public Rendering()
        {
            DrawColor = Color.White;
        }
    }
}