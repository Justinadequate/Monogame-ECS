using System.Collections.Generic;
using Monogame1.ECS.Components;
using Monogame1.ECS.Models;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Monogame1.ECS.Factories
{
    public static class SkeletonFactory
    {
        public static void CreateSkeleton(ContentManager content, int spawnCount)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                Entity newSkeleton = new Entity($"Skeleton{i}");
                newSkeleton.AddComponent(new Transform(new Vector2(100 + (i*50), 100 + (i*50))));
                newSkeleton.AddComponent(new Rendering());
                newSkeleton.AddComponent(new Physics());
                newSkeleton.AddComponent(new Collider());
                newSkeleton.AddComponent(new Player()
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
                newSkeleton.AddComponent(new AnimatedSprite(new List<Animation>()
                {
                    new Animation("SkeletonIdle", content.Load<Texture2D>("Monsters_Creatures_Fantasy/Skeleton/Idle"), 4, true),
                    new Animation("SkeletonWalk", content.Load<Texture2D>("Monsters_Creatures_Fantasy/Skeleton/Walk"), 4, true),
                    new Animation("SkeletonAttack", content.Load<Texture2D>("Monsters_Creatures_Fantasy/Skeleton/Attack"), 8, false)
                }));
            }
        }
    }
}