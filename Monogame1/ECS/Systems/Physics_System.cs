using System.Collections.Generic;
using System.Linq;
using Monogame1.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Monogame1.ECS.Systems
{
    public class Physics_System
    {
        private Dictionary<Physics, Transform> _components = new Dictionary<Physics, Transform>();
        private List<Physics> _toRemove = new List<Physics>();

        public Physics_System()
        {
            EntityManager.Instance.OnComponentAdded += Instance_OnComponentAdded;
            EntityManager.Instance.OnComponentRemoved += Instance_OnComponentRemoved;
            EntityManager.Instance.OnEntityRemoved += Instance_OnEntityRemoved;
        }

        public void Update()
        {
            foreach (var item in _components)
            {
                item.Value.Position += item.Key.Velocity;
                // item.Key.Velocity = Vector2.Zero;
            }
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
            if (component is Physics)
            {
                var physics = (Physics)component;
                if (!_components.ContainsKey(physics))
                    _components.Add(physics, entity.GetComponent<Transform>());
            }
        }
        
        private void Instance_OnComponentRemoved(Entity entity, Component component)
        {
            if (component is Physics)
            {
                var physics = (Physics)component;
                if (_components.ContainsKey(physics))
                    _toRemove.Add(physics);
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