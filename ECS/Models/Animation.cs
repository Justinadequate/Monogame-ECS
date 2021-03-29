using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Models
{
    public class Animation
    {
        public Animation(string name, Texture2D texture, int frameCount)
        {
            Name = name;
            Texture = texture;
            FrameCount = frameCount;
            FrameSpeed = 0.2f;
        }

        public string Name { get; set; }
        public Texture2D Texture { get; private set; }
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public int FrameHeight { get { return Texture.Height;} }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public float FrameSpeed { get; set; }
        public bool IsComplete { get; set; }
    }
}