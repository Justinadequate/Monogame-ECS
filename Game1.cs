using System.Collections.Generic;
using System.Diagnostics;
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
            new EntityManager();
            
            _drawSystem = new Drawing_System();
            _physicsSystem = new Physics_System();
            _playerSystem = new PlayerControl_System();
            _animationSystem = new Animation_System();

            SkeletonFactory.CreateSkeleton(Content, 1);

            Entity floor1 = new Entity("Floor1");
            floor1.AddComponent(new Transform(new Vector2(0, (_graphics.GraphicsDevice.Viewport.Height / 3) * 2)));
            floor1.AddComponent(new Rendering(Content.Load<Texture2D>("FloorA/spr_PisoA_strip18")));
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin();
            _drawSystem.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = ((float)gameTime.ElapsedGameTime.Milliseconds) / 1000f;

            _animationSystem.Update(deltaTime);
            _playerSystem.Update();
            _physicsSystem.Update();

            base.Update(gameTime);
        }
    }
}
