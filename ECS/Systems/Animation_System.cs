using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Monogame1.ECS.Components;
using Monogame1.ECS.Models;

namespace Monogame1.ECS.Systems
{
    public class Animation_System
    {
        private List<(Animation Anim, Player Player, Rendering Rend)> _components = new List<(Animation, Player, Rendering)>();
        private List<Animation> _toRemove = new List<Animation>();
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
                if (_timer > item.Anim.FrameSpeed)
                {
                    _timer = 0f;
                    item.Anim.CurrentFrame++;

                    if(item.Anim.CurrentFrame >= item.Anim.FrameCount)
                        item.Anim.CurrentFrame = 0;
                }

                SetAnimation(item);
            }

            HandleRemove();
        }

        #region privates
        private void SetAnimation((Animation Anim, Player Player, Rendering Rend) item)
        {
            
        }

        private void HandleRemove()
        {
            foreach (var item in _toRemove)
                _components.Remove(_components.FirstOrDefault(c => c.Anim == item));
            _toRemove.Clear();
        }

        private void Instance_OnComponentAdded(Entity entity, Component component)
        {
            // if (component is Animation)
            // {
            //     var animation = (Animation)component;
            //     if (!_components.Any(c => c.Anim == animation))
            //         _components.Add((animation, entity.GetComponent<Player>(), entity.GetComponent<Rendering>()));
            // }
        }
        
        private void Instance_OnComponentRemoved(Entity entity, Component component)
        {
            // if (component is Animation)
            // {
            //     var animation = (Animation)component;
            //     if (_components.Any(c => c.Anim == animation))
            //         _toRemove.Add(animation);
            // }
        }
        
        private void Instance_OnEntityRemoved(Entity entity)
        {
            // var component = _components.FirstOrDefault(c => c.Anim.EntityId == entity.EntityId);
            // if (component.Anim != null)
            //     _toRemove.Add(component.Anim);
        }
        #endregion
    }
}