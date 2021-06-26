using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class Rendering : Component
    {
        public Texture2D Texture { get; set; }
        public Color DrawColor { get; set;}
        public Rectangle Source { get; set; }
        public int Height { get { return Source.Height; } set { Height = value; } }
        public int Width { get { return Source.Width; } set { Width = value; } }
        public float Layer { get; set; }

        public Rendering(Texture2D texture)
        {
            Texture = texture;
            DrawColor = Color.White;
            Source = Texture.Bounds;
        }

        public Rendering()
        {
            DrawColor = Color.White;
        }
    }
}