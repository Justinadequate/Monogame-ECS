using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework.Input;
using Monogame1.ECS.Components;
using Monogame1.ECS.Models;
using MonoGame.Extended;

namespace Monogame1.ECS.Systems
{
    public class PlayerControl_System
    {
        private Dictionary<Player, Physics> _components = new Dictionary<Player, Physics>();
        private List<Player> _toRemove = new List<Player>();
        private OrthographicCamera _camera;

        public PlayerControl_System(OrthographicCamera camera)
        {
            _camera = camera;
            EntityManager.Instance.OnComponentAdded += Instance_OnComponentAdded;
            EntityManager.Instance.OnComponentRemoved += Instance_OnComponentRemoved;
            EntityManager.Instance.OnEntityRemoved += Instance_OnEntityRemoved;
        }

        public void Update()
        {
            foreach (var item in _components)
            {
                Move(item.Key, item.Value);
                SetState(item.Key);

                var playerEntity = EntityManager.Instance._entities.FirstOrDefault(e => e.EntityId == item.Value.EntityId);
                if (playerEntity != null)
                    _camera.LookAt(playerEntity.GetComponent<Transform>().Position);
            }

            HandleRemove();
        }

        private void Move(Player playerControl, Physics playerPhysics)
        {
            var keyboard = Keyboard.GetState();

            //* Go
            if (keyboard.IsKeyDown(playerControl.Input.Up))
                playerPhysics.Velocity.Y = playerPhysics.Velocity.Y <= -playerPhysics.MaxSpeed ?
                    -playerPhysics.MaxSpeed : playerPhysics.Velocity.Y - playerPhysics.Acceleration;
            if (keyboard.IsKeyDown(playerControl.Input.Down))
                playerPhysics.Velocity.Y = playerPhysics.Velocity.Y >= playerPhysics.MaxSpeed ?
                    playerPhysics.MaxSpeed : playerPhysics.Velocity.Y + playerPhysics.Acceleration;
            if (keyboard.IsKeyDown(playerControl.Input.Left))
                playerPhysics.Velocity.X = playerPhysics.Velocity.X <= -playerPhysics.MaxSpeed ?
                    -playerPhysics.MaxSpeed : playerPhysics.Velocity.X - playerPhysics.Acceleration;
            if (keyboard.IsKeyDown(playerControl.Input.Right))
                playerPhysics.Velocity.X = playerPhysics.Velocity.X >= playerPhysics.MaxSpeed ?
                    playerPhysics.MaxSpeed : playerPhysics.Velocity.X + playerPhysics.Acceleration;

            //* Stop
            if (keyboard.IsKeyUp(playerControl.Input.Up) && keyboard.IsKeyUp(playerControl.Input.Down))
                playerPhysics.Velocity.Y = playerPhysics.Velocity.Y == 0 ? 0 
                    : playerPhysics.Velocity.Y > 0 ? playerPhysics.Velocity.Y - playerPhysics.Acceleration
                        : playerPhysics.Velocity.Y + playerPhysics.Acceleration;
            if (keyboard.IsKeyUp(playerControl.Input.Left) && keyboard.IsKeyUp(playerControl.Input.Right))
                playerPhysics.Velocity.X = playerPhysics.Velocity.X == 0 ? 0 
                    : playerPhysics.Velocity.X > 0 ? playerPhysics.Velocity.X - playerPhysics.Acceleration
                        : playerPhysics.Velocity.X + playerPhysics.Acceleration;

            if (playerPhysics.Velocity.Y < 0.1f && playerPhysics.Velocity.Y > -0.1f)
                playerPhysics.Velocity.Y = 0;
            if (playerPhysics.Velocity.X < 0.1f && playerPhysics.Velocity.X > -0.1f)
                playerPhysics.Velocity.X = 0;
        }

        private void SetState(Player player)
        {
            if (player.PlayerState == State.Idle && MovementKeyDown())
                player.PlayerState = State.Walking;
            else if (player.PlayerState == State.Attacking || Keyboard.GetState().IsKeyDown(player.Input.Attack))
                player.PlayerState = State.Attacking;
            else if (!MovementKeyDown())
                player.PlayerState = State.Idle;

            bool MovementKeyDown()
            {
                return (Keyboard.GetState().IsKeyDown(player.Input.Up)
                        || Keyboard.GetState().IsKeyDown(player.Input.Down)
                        || Keyboard.GetState().IsKeyDown(player.Input.Left)
                        || Keyboard.GetState().IsKeyDown(player.Input.Right));
            }
        }

        #region System Methods
        private void HandleRemove()
        {
            foreach (var item in _toRemove)
                _components.Remove(item);
            _toRemove.Clear();
        }

        private void Instance_OnComponentAdded(Entity entity, Component component)
        {
            if (component is Player)
            {
                var renderer = (Player)component;
                if (!_components.ContainsKey(renderer))
                    _components.Add(renderer, entity.GetComponent<Physics>());
            }
        }
        
        private void Instance_OnComponentRemoved(Entity entity, Component component)
        {
            if (component is Player)
            {
                var renderer = (Player)component;
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