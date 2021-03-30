using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Models
{
    public class Animation
    {
        public Animation(string name, Texture2D texture, int frameCount, bool shouldLoop)
        {
            Name = name;
            Texture = texture;
            FrameCount = frameCount;
            ShouldLoop = shouldLoop;
            FrameSpeed = 0.2f;
            LoopCount = 0;
        }

        public string Name { get; set; }
        public Texture2D Texture { get; private set; }
        public int CurrentFrame { get; set; }
        public int FrameCount { get; set; }
        public int FrameHeight { get { return Texture.Height;} }
        public int FrameWidth { get { return Texture.Width / FrameCount; } }
        public float FrameSpeed { get; set; }
        public int LoopCount { get; set; }
        public bool ShouldLoop { get; set; }
        public bool IsComplete { get; set; }
    }
}