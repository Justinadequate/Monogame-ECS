using System.Collections.Generic;
//using Monogame1.OO.Models;
using Monogame1.ECS;
using Monogame1.ECS.Models;
using Monogame1.ECS.Components;
using Monogame1.ECS.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        // private List<AnimatedSprite> _sprites;
        private Drawing_System _drawSystem;
        private Animation_System _animationSystem;
        private PlayerControl_System _playerSystem;
        private Physics_System _physicsSystem;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // _sprites = new List<AnimatedSprite>()                // OO approach
            // {
            //     new Character(new Vector2(100, 100), "Skeleton")
            //     {
            //         Input = new Input()
            //         {
            //             Up = Keys.W,
            //             Down = Keys.S,
            //             Left = Keys.A,
            //             Right = Keys.D,
            //             Attack = Keys.Space
            //         }
            //     }
            // };
            
            // foreach (var sprite in _sprites)
            //     sprite.LoadContent(Content);

            new EntityManager();                                    // ECS approach
            
            _drawSystem = new Drawing_System();
            _physicsSystem = new Physics_System();
            _playerSystem = new PlayerControl_System();
            _animationSystem = new Animation_System();
            // TODO: find out how to manage animations etc in ECS

            Entity entity = new Entity("First");
            entity.AddComponent(new Transform(new Vector2(100, 100)));
            entity.AddComponent(new Rendering());
            entity.AddComponent(new Physics());
            entity.AddComponent(new Player()
            {
                Input = new Input()
                {
                    Up = Keys.W,
                    Down = Keys.S,
                    Left = Keys.A,
                    Right = Keys.D,
                    Attack = Keys.Space
                }
            });
            entity.AddComponent(new AnimatedSprite(new List<Animation>()
            {
                new Animation("SkeletonIdle", Content.Load<Texture2D>("Monsters_Creatures_Fantasy/Skeleton/Idle"), 4, true),
                new Animation("SkeletonWalk", Content.Load<Texture2D>("Monsters_Creatures_Fantasy/Skeleton/Walk"), 4, true),
                new Animation("SkeletonAttack", Content.Load<Texture2D>("Monsters_Creatures_Fantasy/Skeleton/Attack"), 8, false)
            }));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            _spriteBatch.Begin();
            // foreach (var sprite in _sprites)
            //     sprite.Draw(_spriteBatch);
            _drawSystem.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            // foreach (var sprite in _sprites)
            //     sprite.Update(gameTime);

            _animationSystem.Update(gameTime);
            _playerSystem.Update();
            _physicsSystem.Update();

            base.Update(gameTime);
        }
    }
}
