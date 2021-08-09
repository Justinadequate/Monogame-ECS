using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Models
{
    public class Spritesheet
    {
        private Texture2D _texture;
        private int _rows;
        private int _columns;
        // private Rectangle[][] _frames;
        private int _frameWidth;
        private int _frameHeight;

        public Spritesheet(Texture2D texture, int rows, int columns)
        {
            _texture = texture;
            _rows = rows;
            _columns = columns;
            _frameWidth = _texture.Width / _columns;
            _frameHeight = _texture.Height / _rows;
        }
    }
}