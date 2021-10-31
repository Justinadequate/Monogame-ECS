using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class TileSheet : Component
    {
        public Texture2D Texture { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public int RowCount { get; set; }
        public int ColumnCount { get; set; }

        public TileSheet(ContentManager content, string texture, int rows, int columns)
        {
            Texture = content.Load<Texture2D>(texture);
            Width = Texture.Width;
            Height = Texture.Height;
            RowCount = rows;
            ColumnCount = columns;
        }
    }
}