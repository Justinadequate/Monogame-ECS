using System.Collections.Generic;
using System.Diagnostics;
using Monogame1.ECS;
using Monogame1.ECS.Factories;
using Monogame1.ECS.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;

namespace Monogame1
{
    public class Game1 : Game
    {
        // TODO: cannot import tiled map and pipeline dll not in build folder !!!!
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Drawing_System _drawSystem;
        private Animation_System _animationSystem;
        private PlayerControl_System _playerSystem;
        private Physics_System _physicsSystem;
        private TiledMap _tiledMap;
        private TiledMapRenderer _tiledMapRenderer;

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
            _tiledMap = Content.Load<TiledMap>("Maps/test1");
            _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            new EntityManager();
            
            _drawSystem = new Drawing_System();
            _physicsSystem = new Physics_System();
            _playerSystem = new PlayerControl_System();
            _animationSystem = new Animation_System();

            SkeletonFactory.CreateSkeleton(Content, 1);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            _spriteBatch.Begin();
            _tiledMapRenderer.Draw();
            _drawSystem.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = ((float)gameTime.ElapsedGameTime.Milliseconds) / 1000f;

            _tiledMapRenderer.Update(gameTime);
            _animationSystem.Update(deltaTime);
            _playerSystem.Update();
            _physicsSystem.Update();

            base.Update(gameTime);
        }
    }
}
