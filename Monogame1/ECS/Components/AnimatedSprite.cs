using System.Collections.Generic;
using System.Linq;
using Monogame1.ECS.Models;

namespace Monogame1.ECS.Components
{
    public class AnimatedSprite : Component
    {
        string Name { get; set; }
        public List<Animation> Animations { get; set; }
        public Animation CurrentAnimation { get; set; }
        public float Timer { get; set; }

        public AnimatedSprite(List<Animation> animations)
        {
            Animations = animations;
            CurrentAnimation = animations.FirstOrDefault();
        }
    }
}