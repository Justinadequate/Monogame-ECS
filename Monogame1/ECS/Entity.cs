using System.Collections.Generic;
using System.Linq;

namespace Monogame1.ECS
{
    public class Entity
    {
        private List<Component> _components = new List<Component>();
        public int EntityId;
        public bool IsActive;
        public string EntityName;

        public Entity(string name)
        {
            EntityName = name;
            IsActive = true;

            EntityManager.Instance.AddEntity(this);
        }

        public void AddComponent(Component component)
        {
            if (!_components.Contains(component))
                _components.Add(component);
            EntityManager.Instance.ComponentAdded(this, component);
            component.EntityId = this.EntityId;
        }

        public void RemoveComponent(Component component)
        {
            if (_components.Contains(component))
                _components.Remove(component);
            EntityManager.Instance.ComponentRemoved(this, component);
        }

        public T GetComponent<T>() where T : Component
        {
            var component = _components.FirstOrDefault(c => c.GetType() == typeof(T));

            if (component != null)
                return (T)component;

            return null;
        }

        public List<Component> GetComponents()
        {
            return _components;
        }
        
        public void Destroy()
        {
            EntityManager.Instance.RemoveEntity(this);
        }
    }
}