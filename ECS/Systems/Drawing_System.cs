using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Graphics;
using Monogame1.ECS.Components;

namespace Monogame1.ECS.Systems
{
    public class Drawing_System
    {
        private Dictionary<Rendering, Transform> _components = new Dictionary<Rendering, Transform>();
        private List<Rendering> _toRemove = new List<Rendering>();

        public Drawing_System()
        {
            EntityManager.Instance.OnComponentAdded += Instance_OnComponentAdded;
            EntityManager.Instance.OnComponentRemoved += Instance_OnComponentRemoved;
            EntityManager.Instance.OnEntityRemoved += Instance_OnEntityRemoved;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (var item in _components)
                spriteBatch.Draw(item.Key.Texture, item.Value.Position, item.Key.DrawColor);

            HandleRemove();
        }

        #region privates
        private void HandleRemove()
        {
            foreach (var item in _toRemove)
                _components.Remove(item);
            _toRemove.Clear();
        }

        private void Instance_OnComponentAdded(Entity entity, Component component)
        {
            if (component is Rendering)
            {
                var renderer = (Rendering)component;
                if (!_components.ContainsKey(renderer))
                    _components.Add(renderer, entity.GetComponent<Transform>());
            }
        }
        
        private void Instance_OnComponentRemoved(Entity entity, Component component)
        {
            if (component is Rendering)
            {
                var renderer = (Rendering)component;
                if (_components.ContainsKey(renderer))
                    _toRemove.Add(renderer);
            }
        }
        
        private void Instance_OnEntityRemoved(Entity entity)
        {
            var component = _components.FirstOrDefault(c => c.Key.EntityId == entity.EntityId);
            if (component.Key != null)
                _toRemove.Add(component.Key);
        }
        #endregion
    }
}