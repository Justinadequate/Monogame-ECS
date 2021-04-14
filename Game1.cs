using System.Collections.Generic;
using System.Diagnostics;
//using Monogame1.OO.Models;
using Monogame1.ECS;
using Monogame1.ECS.Components;
using Monogame1.ECS.Factories;
using Monogame1.ECS.Models;
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
        private Raycast_System _raycastSystem;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 25*25;
            _graphics.PreferredBackBufferWidth = 25*25;
            _graphics.ApplyChanges();
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
            _raycastSystem = new Raycast_System();

            Entity map = new Entity("map");
            map.AddComponent(new Map());
            Entity camera = new Entity("camera");
            camera.AddComponent(new Camera(new Vector2(100, 100)));

            _raycastSystem.LoadContent(Content);

            // SkeletonFactory.CreateSkeleton(Content, 2);

            // Entity floor1 = new Entity("Floor1");
            // floor1.AddComponent(new Transform(new Vector2(0, (_graphics.GraphicsDevice.Viewport.Height / 3) * 2)));
            // floor1.AddComponent(new Rendering(Content.Load<Texture2D>("FloorA/spr_PisoA_strip18")));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin();
            // foreach (var sprite in _sprites)
            //     sprite.Draw(_spriteBatch);
            _drawSystem.Draw(_spriteBatch);
            _raycastSystem.Draw(_spriteBatch, _graphics.GraphicsDevice);

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
            _raycastSystem.Update();

            base.Update(gameTime);
        }
    }
}
