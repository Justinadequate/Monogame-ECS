using System;
using System.Collections.Generic;
using System.Linq;
using Monogame1.ECS.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame1.ECS.Systems
{
    public class Raycast_System
    {
        private Dictionary<Camera, Transform> _components = new Dictionary<Camera, Transform>();
        // private List<Map> _mapComponents = new List<Map>();
        private List<Camera> _camerasToRemove = new List<Camera>();
        // private List<Map> _mapsToRemove = new List<Map>();

        public Raycast_System()
        {
            EntityManager.Instance.OnComponentAdded += Instance_OnComponentAdded;
            EntityManager.Instance.OnComponentRemoved += Instance_OnComponentRemoved;
            EntityManager.Instance.OnEntityRemoved += Instance_OnEntityRemoved;
        }

        public void Update(float deltaTime)
        {
            foreach(var item in _components)
            {
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                {
                    var dir = new Vector2((float)Math.Cos(item.Value.Rotation), (float)Math.Sin(item.Value.Rotation));
                    dir.Normalize();
                    item.Key.Velocity += dir * item.Key.MoveSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                {
                    var dir = new Vector2((float)Math.Acos(item.Value.Rotation), (float)Math.Asin(item.Value.Rotation));
                    dir.Normalize();
                    item.Key.Velocity -= dir * item.Key.MoveSpeed;
                }
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    item.Value.Rotation -= item.Key.TurnSpeed * deltaTime;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    item.Value.Rotation += item.Key.TurnSpeed * deltaTime;

                item.Value.Position += item.Key.Velocity * deltaTime;
                item.Key.Velocity = Vector2.Zero;
            }
        }

        #region privates

        private void HandleRemove()
        {
            foreach (var item in _camerasToRemove)
                _components.Remove(item);
            _camerasToRemove.Clear();
        }

        private void Instance_OnComponentAdded(Entity entity, Component component)
        {
            if (component is Camera)
            {
                var camera = (Camera)component;
                if (!_components.ContainsKey(camera))
                    _components.Add(camera, entity.GetComponent<Transform>());
            }
        }
        
        private void Instance_OnComponentRemoved(Entity entity, Component component)
        {
            if (component is Camera)
            {
                var camera = (Camera)component;
                if (_components.ContainsKey(camera))
                    _camerasToRemove.Add(camera);
            }
        }
        
        private void Instance_OnEntityRemoved(Entity entity)
        {
            var component = _components.FirstOrDefault(c => c.Key.EntityId == entity.EntityId);
            if (component.Key != null)
                _camerasToRemove.Add(component.Key);
        }
        #endregion
    }
}