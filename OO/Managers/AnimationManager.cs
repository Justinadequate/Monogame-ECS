using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Monogame1.OO.Models;

namespace Monogame1.OO.Managers
{
    public class AnimationManager
    {
        private Animation _animation;
        private float _timer;
        private int _timesPlayed = 0;
        public Vector2 Position { get; set; }
        public bool AnimationDone = false;
        
        public AnimationManager(Animation animation)
        {
            _animation = animation;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_animation.Texture,
                            Position,
                            new Rectangle(_animation.CurrentFrame * _animation.FrameWidth,
                                            0,
                                            _animation.FrameWidth,
                                            _animation.FrameHeight),
                            Color.White);
        }

        public void Play(Animation animation, int playCount = -1)
        {
            if (_animation == animation && _timesPlayed != playCount)
            {
                if (_animation.CurrentFrame < _animation.FrameCount - 1)
                    return;
                else
                    AnimationDone = true; 
                return;
            }

            AnimationDone = false;
            _timesPlayed = 0;
            _animation = animation;
            _animation.CurrentFrame = 0;
            _timer = 0;
        }

        public void Stop(Animation animation)
        {
            _timer = 0f;
            _animation.CurrentFrame = 0;
            AnimationDone = true;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            if (_timer > _animation.FrameSpeed)
            {
                _timer = 0f;
                _animation.CurrentFrame++;

                if (_animation.CurrentFrame >= _animation.FrameCount)
                    _animation.CurrentFrame = 0;
            }
        }
    }
}