using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class Tile : Component
    {
        public Texture2D Texture { get; set; }
        public bool Tangible { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public Tile(GraphicsDevice graphics, int width, int height)
        {
            Texture = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            Texture.SetData<Color>(new Color[] {Color.White});

            Width = width;
            Height = height;
        }
    }
}