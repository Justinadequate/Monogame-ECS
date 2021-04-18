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
        private int[,] _map;
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

            _map = Map.GetMap();

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

            Entity camera = new Entity("camera");
            camera.AddComponent(new Transform(new Vector2(100, 100)));
            camera.AddComponent(new Rendering(Content.Load<Texture2D>("triangle")) {Width = 15, Height = 15, Layer = 1});
            camera.AddComponent(new Camera());

            // SkeletonFactory.CreateSkeleton(Content, 2);

            // Entity floor1 = new Entity("Floor1");
            // floor1.AddComponent(new Transform(new Vector2(0, (_graphics.GraphicsDevice.Viewport.Height / 3) * 2)));
            // floor1.AddComponent(new Rendering(Content.Load<Texture2D>("FloorA/spr_PisoA_strip18")));

            #region Initialize Map
            var texture1px = new Texture2D(_graphics.GraphicsDevice, 1, 1, false, SurfaceFormat.Color);
            texture1px.SetData<Color>(new Color[] {Color.White});
            var pos = Vector2.Zero;
            var tileSize = 25;
            for (int x = 0; x < _map.GetLength(0); x++)
            {
                for (int y = 0; y < _map.GetLength(1); y++)
                {
                    Entity tile = new Entity("tile");
                    if (_map[x,y] == 1)
                    {
                        tile.AddComponent(new Tile(_graphics.GraphicsDevice, tileSize, tileSize) {Tangible = true});
                        tile.AddComponent(new Transform(pos));
                        tile.AddComponent(new Rendering(texture1px)
                        {
                            Width = tileSize,
                            Height = tileSize,
                            DrawColor = Color.Black,
                            Layer = 0
                        });
                    }
                    else if (_map[x,y] == 0)
                    {
                        tile.AddComponent(new Tile(_graphics.GraphicsDevice, tileSize, tileSize) {Tangible = false});
                        tile.AddComponent(new Transform(pos));
                        tile.AddComponent(new Rendering(texture1px)
                        {
                            Width = tileSize,
                            Height = tileSize,
                            DrawColor = Color.White,
                            Layer = 0
                        });
                    }
                    pos.X += tileSize;
                }
                pos.X = 0;
                pos.Y += tileSize;
            }

            // DRAW GRID:
            // pos = Vector2.Zero;
            // for (int x = 0; x < _map.GetLength(0); x++)
            // {
            //     _spriteBatch.Draw(texture1px, new Rectangle((int)pos.X, 0, 1, tileSize*_map.GetLength(0)), Color.Red);
            //     pos.X += tileSize;
            // }
            // for (int y = 0; y < _map.GetLength(1); y++)
            // {
            //     _spriteBatch.Draw(texture1px, new Rectangle(0, (int)pos.Y, tileSize*_map.GetLength(1), 1), Color.Red);
            //     pos.Y += tileSize;
            // }
            #endregion
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Transparent);

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
            float deltaTime = ((float)gameTime.ElapsedGameTime.Milliseconds) / 1000f;

            // foreach (var sprite in _sprites)
            //     sprite.Update(gameTime);

            _animationSystem.Update(deltaTime);
            _playerSystem.Update();
            _physicsSystem.Update();
            _raycastSystem.Update(deltaTime);

            base.Update(gameTime);
        }
    }
}
