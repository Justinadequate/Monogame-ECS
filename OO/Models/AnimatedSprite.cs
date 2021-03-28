using System;
using System.Collections.Generic;
using System.Linq;
using Monogame1.OO.Managers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Monogame1.OO.Models
{
    public abstract class AnimatedSprite
    {
        protected string _name;
        protected string _state = State.Idle;
        protected Vector2 _position;
        protected AnimationManager _animationManager;
        protected Dictionary<string, Animation> _animations;
        public Texture2D Texture;
        public float Speed = 1f;
        public Input Input;
        public Vector2 Velocity;
        public Vector2 Position
        {
            get { return _position; }
            set
            {
                _position = value;
                if (_animationManager != null)
                    _animationManager.Position = _position;
            }
        }
        public Rectangle Rectangle
        {
            get { return new Rectangle((int)_position.X, (int)_position.Y, Texture.Width, Texture.Height); }
        }

        public AnimatedSprite(Vector2 postition, string name)
        {
            _position = postition;
            _name = name;
        }

        public virtual void Move()
        {
            if (Keyboard.GetState().IsKeyDown(Input.Up))
                Velocity.Y = -Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Down))
                Velocity.Y = Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Left))
                Velocity.X = -Speed;
            if (Keyboard.GetState().IsKeyDown(Input.Right))
                Velocity.X = Speed;
        }

        public virtual void SetState()
        {
            if (_state == State.Idle && MovementKeyDown())
                _state = State.Walking;
            else if ( _state == State.Attacking || Keyboard.GetState().IsKeyDown(Input.Attack))
                _state = State.Attacking;
            else if (!(_state == State.Walking || _state == State.Attacking))
                _state = State.Idle;

            _animationManager.Play(_animations[$"{_name}{_state}"]);
        }
        
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (Texture != null)
                spriteBatch.Draw(Texture, Position, Color.White);
            else if (_animationManager != null)
                _animationManager.Draw(spriteBatch);
            else
                throw new Exception("bad");
        }

        public virtual void LoadContent(ContentManager contentManager)
        {
            _animations = new Dictionary<string, Animation>()
            {
                { $"{_name}Idle", new Animation(contentManager.Load<Texture2D>($"./Monsters_Creatures_Fantasy/{_name}/Idle"), 4) },
                { $"{_name}Walk", new Animation(contentManager.Load<Texture2D>($"./Monsters_Creatures_Fantasy/{_name}/Walk"), 4) },
                { $"{_name}Attack", new Animation(contentManager.Load<Texture2D>($"./Monsters_Creatures_Fantasy/{_name}/Attack"), 8) }
            };
            
            _animationManager = new AnimationManager(_animations.First().Value);
        }

        public virtual void Update(GameTime gameTime)
        {
            SetState();

            if (_state == State.Walking)
                Move();

            _animationManager.Update(gameTime);

            Position += Velocity;
            Velocity = Vector2.Zero;

            if (!MovementKeyDown() && _state != State.Attacking)
                _state = State.Idle;
            // TODO: if attacking twice in a row, the second one must be held because AnimationDone is still true
            if (_state == State.Attacking && _animationManager.AnimationDone)
                _state = State.Idle;
        }

        private bool MovementKeyDown()
        {
            return (Keyboard.GetState().IsKeyDown(Input.Up)
                    || Keyboard.GetState().IsKeyDown(Input.Down)
                    || Keyboard.GetState().IsKeyDown(Input.Left)
                    || Keyboard.GetState().IsKeyDown(Input.Right));
        }
    }
}