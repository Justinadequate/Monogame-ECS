using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Monogame1.ECS.Components;
using Monogame1.ECS.Models;

namespace Monogame1.ECS.Systems
{
    public class Animation_System
    {
        private List<(AnimatedSprite Anim, Player Player, Rendering Rend)> _components = new List<(AnimatedSprite, Player, Rendering)>();
        private List<AnimatedSprite> _toRemove = new List<AnimatedSprite>();
        private float _timer;

        public Animation_System()
        {
            EntityManager.Instance.OnComponentAdded += Instance_OnComponentAdded;
            EntityManager.Instance.OnComponentRemoved += Instance_OnComponentRemoved;
            EntityManager.Instance.OnEntityRemoved += Instance_OnEntityRemoved;
        }

        public void Update(GameTime gameTime)
        {
            _timer += (float)gameTime.ElapsedGameTime.TotalSeconds;

            foreach (var item in _components)
            {
                if (_timer > item.Anim.CurrentAnimation.FrameSpeed)
                {
                    _timer = 0f;
                    item.Anim.CurrentAnimation.CurrentFrame++;

                    if(item.Anim.CurrentAnimation.CurrentFrame >= item.Anim .CurrentAnimation.FrameCount)
                        item.Anim.CurrentAnimation.CurrentFrame = 0;
                }

                SetAnimation(item);
                item.Rend.Texture = item.Anim.CurrentAnimation.Texture;
                item.Rend.SourceRectangle = new Rectangle(
                    item.Anim.CurrentAnimation.CurrentFrame * item.Anim.CurrentAnimation.FrameWidth,
                    0,
                    item.Anim.CurrentAnimation.FrameWidth,
                    item.Anim.CurrentAnimation.FrameHeight);
            }

            HandleRemove();
        }

        #region privates
        private void SetAnimation((AnimatedSprite Anim, Player Player, Rendering Rend) item)
        {
            Play(item.Anim.Animations.FirstOrDefault(a => a.Name.Contains(item.Player.PlayerState)), item);
        }

        private void Play(Animation animation, (AnimatedSprite Anim, Player Player, Rendering Rend) item)
        {
            if (item.Anim.CurrentAnimation == animation)
            {
                if (item.Anim.CurrentAnimation.IsComplete && !item.Anim.CurrentAnimation.ShouldLoop)
                {
                    item.Player.PlayerState = State.Idle;
                    item.Anim.CurrentAnimation.IsComplete = false;
                }

                if (item.Anim.CurrentAnimation.CurrentFrame < item.Anim.CurrentAnimation.FrameCount - 1)
                    return;
                else
                    item.Anim.CurrentAnimation.IsComplete = true;
                return;
            }

            item.Anim.CurrentAnimation = animation;
            item.Anim.CurrentAnimation.IsComplete = false;
            item.Anim.CurrentAnimation.CurrentFrame = 0;
            _timer = 0;
        }

        private void HandleRemove()
        {
            foreach (var item in _toRemove)
                _components.Remove(_components.FirstOrDefault(c => c.Anim == item));
            _toRemove.Clear();
        }

        private void Instance_OnComponentAdded(Entity entity, Component component)
        {
            if (component is AnimatedSprite)
            {
                var animatedSprite = (AnimatedSprite)component;
                if (!_components.Any(c => c.Anim == animatedSprite))
                    _components.Add((animatedSprite, entity.GetComponent<Player>(), entity.GetComponent<Rendering>()));
            }
        }
        
        private void Instance_OnComponentRemoved(Entity entity, Component component)
        {
            if (component is AnimatedSprite)
            {
                var animatedSprite = (AnimatedSprite)component;
                if (_components.Any(c => c.Anim == animatedSprite))
                    _toRemove.Add(animatedSprite);
            }
        }
        
        private void Instance_OnEntityRemoved(Entity entity)
        {
            var component = _components.FirstOrDefault(c => c.Anim.EntityId == entity.EntityId);
            if (component.Anim != null)
                _toRemove.Add(component.Anim);
        }
        #endregion
    }
}