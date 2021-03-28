using System.Collections.Generic;
using Monogame1.ECS.Models;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.ECS.Components
{
    public class AnimatedSprite : Sprite
    {
        public Dictionary<string, Animation> Animations { get; set; } // TODO: how give and use animations

        public AnimatedSprite(string name, Texture2D texture) : base(texture)
        {
            Texture = texture;
        }
    }
}