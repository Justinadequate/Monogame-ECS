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
        private List<Camera> _cameraComponents = new List<Camera>();
        private List<Map> _mapComponents = new List<Map>();
        private List<Camera> _camerasToRemove = new List<Camera>();
        private List<Map> _mapsToRemove = new List<Map>();

        public Raycast_System()
        {
            EntityManager.Instance.OnComponentAdded += Instance_OnComponentAdded;
            EntityManager.Instance.OnComponentRemoved += Instance_OnComponentRemoved;
            EntityManager.Instance.OnEntityRemoved += Instance_OnEntityRemoved;
        }

        public void Update()
        {
            foreach(var item in _cameraComponents)
            {
                // TODO: Rotate on control and move forward in that direction
                var pos = item.Position;
                if (Keyboard.GetState().IsKeyDown(Keys.W))
                    pos.Y += -item.MoveSpeed;
                if (Keyboard.GetState().IsKeyDown(Keys.S))
                    pos.Y += item.MoveSpeed;
                if (Keyboard.GetState().IsKeyDown(Keys.A))
                    pos.X += -item.MoveSpeed;
                if (Keyboard.GetState().IsKeyDown(Keys.D))
                    pos.X += item.MoveSpeed;

                item.Position = new Vector2(pos.X, pos.Y);
                item.Rotation += item.TurnSpeed;
            }
        }

        public void Draw(SpriteBatch spriteBatch, GraphicsDevice graphics)
        {
            var texture1px = new Texture2D(graphics, 1, 1, false, SurfaceFormat.Color);
            texture1px.SetData<Color>(new Color[] {Color.White});

            foreach(var item in _mapComponents)
                DrawMap(spriteBatch, graphics, item, texture1px);

            foreach(var item in _cameraComponents)
                DrawArrow(spriteBatch, graphics, item);
        }

        public void LoadContent(ContentManager content)
        {
            foreach (var item in _cameraComponents)
                item.Texture = content.Load<Texture2D>("triangle");
        }

        #region privates
        private void DrawMap(SpriteBatch spriteBatch, GraphicsDevice graphics, Map item, Texture2D tex)
        {
            var pos = Vector2.Zero;
            var tileSize = 25;

            for (int x = 0; x < item.Height; x++)
            {
                for (int y = 0; y < item.Width; y++)
                {
                    if (item.Grid[x,y] == 1)
                        spriteBatch.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, tileSize, tileSize), Color.Black);
                    else if (item.Grid[x,y] == 0)
                        spriteBatch.Draw(tex, new Rectangle((int)pos.X, (int)pos.Y, tileSize, tileSize), Color.White);
                    
                    pos.X += tileSize;
                }
                pos.X = 0;
                pos.Y += tileSize;
            }
            pos = Vector2.Zero;

            for (int x = 0; x < item.Width; x++)
            {
                spriteBatch.Draw(tex, new Rectangle((int)pos.X, 0, 1, tileSize*item.Height), Color.Red);
                pos.X += tileSize;
            }
            for (int y = 0; y < item.Height; y++)
            {
                spriteBatch.Draw(tex, new Rectangle(0, (int)pos.Y, tileSize*item.Width, 1), Color.Red);
                pos.Y += tileSize;
            }
        }

        private void DrawArrow(SpriteBatch spriteBatch, GraphicsDevice graphics, Camera item)
        {
            spriteBatch.Draw(
                item.Texture,
                new Rectangle((int)item.Position.X, (int)item.Position.Y, 15, 15),
                null,
                Color.White,
                item.Rotation,
                new Vector2(item.Texture.Width/2, item.Texture.Height/2),
                SpriteEffects.None,
                0f
            );
        }

        private void HandleRemove()
        {
            foreach (var item in _camerasToRemove)
                _cameraComponents.Remove(item);
            foreach (var item in _mapComponents)
                _mapComponents.Remove(item);
            _camerasToRemove.Clear();
            _mapComponents.Clear();
        }

        private void Instance_OnComponentAdded(Entity entity, Component component)
        {
            if (component is Camera)
            {
                var camera = (Camera)component;
                if (!_cameraComponents.Contains(camera))
                    _cameraComponents.Add(camera);
            }
            if (component is Map)
            {
                var map = (Map)component;
                if (!_mapComponents.Contains(map))
                    _mapComponents.Add(map);
            }
        }
        
        private void Instance_OnComponentRemoved(Entity entity, Component component)
        {
            if (component is Camera)
            {
                var camera = (Camera)component;
                if (_cameraComponents.Contains(camera))
                    _camerasToRemove.Add(camera);
            }
            if (component is Map)
            {
                var map = (Map)component;
                if (_mapComponents.Contains(map))
                    _mapComponents.Add(map);
            }
        }
        
        private void Instance_OnEntityRemoved(Entity entity)
        {
            var cameraComponent = _cameraComponents.FirstOrDefault(c => c.EntityId == entity.EntityId);
            if (cameraComponent != null)
                _camerasToRemove.Add(cameraComponent);

            var mapComponent = _mapComponents.FirstOrDefault(c => c.EntityId == entity.EntityId);
            if (mapComponent != null)
                _mapsToRemove.Add(mapComponent);
        }
        #endregion
    }
}