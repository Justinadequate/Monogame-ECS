using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class Rendering : Component
    {
        public Texture2D Texture;
        public Color DrawColor;

        public Rendering(Texture2D texture)
        {
            Texture = texture;
            DrawColor = Color.White;
        }

        public Rendering() 
        {
            DrawColor = Color.White;
        }
    }
}