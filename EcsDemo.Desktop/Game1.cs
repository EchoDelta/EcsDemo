using EcsDemo.Desktop.Components;
using EcsDemo.Desktop.Systems;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using MonoGame.Extended.Entities;

namespace EcsDemo.Desktop
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        private World _world;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void LoadContent()
        {
            _world = new WorldBuilder()
                .AddSystem(new RenderSystem(GraphicsDevice))
                .AddSystem(new BallBounceSystem())
                .AddSystem(new MovementSystem())
                .AddSystem(new EdgeBounceSystem(_graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight))
                .Build();

            var ball = _world.CreateEntity();
            ball.Attach(new PositionComponent { Position = Vector2.Zero });
            ball.Attach(new RenderableComponent { Color = Color.Blue });
            ball.Attach(new SpatialComponent { Width = 10, Height = 10 });
            ball.Attach(new VelocityComponent { Velocity = new Vector2(100, 100) });

            for (var i = 0; i < 10; i++)
            {
                var brick = _world.CreateEntity();
                brick.Attach(new CollidableComponent());
                brick.Attach(new PositionComponent { Position = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 10 * i) });
                brick.Attach(new RenderableComponent { Color = Color.Red });
                brick.Attach(new SpatialComponent { Width = 40, Height = 20 });
            }
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            _world.Update(gameTime);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            _world.Draw(gameTime);
            base.Draw(gameTime);
        }
    }
}
