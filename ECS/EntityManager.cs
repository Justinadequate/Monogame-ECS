using System;
using System.Collections.Generic;

namespace Monogame1.ECS
{
    public class EntityManager
    {
        public List<Entity> _entities = new List<Entity>();
        public static EntityManager Instance;
        public event Action<Entity, Component> OnComponentAdded;
        public event Action<Entity, Component> OnComponentRemoved;
        public event Action<Entity> OnEntityAdded;
        public event Action<Entity> OnEntityRemoved;

        public EntityManager()
        {
            Instance = this;
        }

        public void AddEntity(Entity entity)
        {
            if (!_entities.Contains(entity))
            {
                _entities.Add(entity);
                entity.EntityId = _entities.Count - 1;
                OnEntityAdded?.Invoke(entity);
            }
        }
        
        public void RemoveEntity(Entity entity)
        {
            if (_entities.Contains(entity))
            {
                _entities.Remove(entity);
                ReIndex();
                OnEntityRemoved?.Invoke(entity);
            }
        }

        public void ComponentAdded(Entity entity, Component component)
        {
            OnComponentAdded?.Invoke(entity, component);
        }

        public void ComponentRemoved(Entity entity, Component component)
        {
            OnComponentRemoved?.Invoke(entity, component);
        }

        private void ReIndex()
        {
            for (int i = 0; i < _entities.Count; i++)
            {
                _entities[i].EntityId = i;

                foreach (var component in _entities[i].GetComponents())
                {
                    component.EntityId = i;
                }
            }
        }
    }
}