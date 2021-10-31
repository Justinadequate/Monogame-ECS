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
using MonoGame.Extended;
using MonoGame.Extended.Tiled;
using MonoGame.Extended.Tiled.Renderers;
using MonoGame.Extended.ViewportAdapters;

namespace Monogame1
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Drawing_System _drawSystem;
        private Animation_System _animationSystem;
        private PlayerControl_System _playerControlSystem;
        private Physics_System _physicsSystem;
        // private TiledMap _tiledMap;
        // private TiledMapRenderer _tiledMapRenderer;
        private OrthographicCamera _camera;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            var viewportadapter = new BoxingViewportAdapter(Window, GraphicsDevice, 800, 600);
            _camera = new OrthographicCamera(viewportadapter);
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // _tiledMap = Content.Load<TiledMap>("Maps/test1");
            // _tiledMapRenderer = new TiledMapRenderer(GraphicsDevice, _tiledMap);
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            new EntityManager();
            
            _drawSystem = new Drawing_System();
            _physicsSystem = new Physics_System();
            _playerControlSystem = new PlayerControl_System(_camera);
            _animationSystem = new Animation_System();

            SkeletonFactory.CreateSkeleton(Content, 1);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

            // TODO: using pointclamp to get rid of screen tearing, but makes sprites look worse
            //       pad each tile in the tilesheet with 1px of the same color around it
            _spriteBatch.Begin(
                transformMatrix: _camera.GetViewMatrix(),
                samplerState: SamplerState.PointClamp
            );
            // _tiledMapRenderer.Draw(_camera.GetViewMatrix());
            _drawSystem.Draw(_spriteBatch);
            _spriteBatch.End();

            base.Draw(gameTime);
        }        

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            float deltaTime = ((float)gameTime.ElapsedGameTime.Milliseconds) / 1000f;

            // _tiledMapRenderer.Update(gameTime);
            _animationSystem.Update(deltaTime);
            _playerControlSystem.Update();
            _physicsSystem.Update();

            base.Update(gameTime);
        }
    }
}
