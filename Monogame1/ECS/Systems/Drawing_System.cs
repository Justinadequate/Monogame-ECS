using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
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
            foreach (var item in _components.OrderBy(c => c.Key.Layer))
            {
                item.Value.Destination = new Rectangle(
                    (int)item.Value.Position.X,
                    (int)item.Value.Position.Y,
                    (int)Math.Floor(item.Key.Source.Width * item.Value.Scale.X),
                    (int)Math.Floor(item.Key.Source.Height * item.Value.Scale.Y)
                );
                spriteBatch.Draw(
                    texture: item.Key.Texture,
                    destinationRectangle: item.Value.Destination,
                    sourceRectangle: item.Key.Source,
                    color: item.Key.DrawColor,
                    rotation: item.Value.Rotation,
                    origin: new Vector2(
                        item.Value.Destination.Width/2,
                        item.Value.Destination.Height/2
                    ),
                    effects: SpriteEffects.None,
                    layerDepth: item.Key.Layer
                );
            }

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