using EcsDemo.Desktop.Components;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace EcsDemo.Desktop.Systems
{
    public class RenderSystem : EntityDrawSystem
    {
        private ComponentMapper<RenderableComponent> _renderableMapper;
        private ComponentMapper<PositionComponent> _positionMapper;
        private readonly GraphicsDevice _graphicsDevice;
        private readonly SpriteBatch _spriteBatch;
        private ComponentMapper<SpatialComponent> _spatialMapper;

        public RenderSystem(GraphicsDevice graphicsDevice) : base(Aspect.All(typeof(RenderableComponent), typeof(PositionComponent), typeof(SpatialComponent)))
        {
            _graphicsDevice = graphicsDevice;
            _spriteBatch = new SpriteBatch(graphicsDevice);
        }

        public override void Draw(GameTime gameTime)
        {
            _graphicsDevice.Clear(Color.Black);
            _spriteBatch.Begin();
            foreach (var entityId in ActiveEntities)
            {
                var renderable = _renderableMapper.Get(entityId);
                var position = _positionMapper.Get(entityId);
                var spatial = _spatialMapper.Get(entityId);
                _spriteBatch.FillRectangle(position.Position, new Size2(spatial.Width, spatial.Height), renderable.Color);
            }
            _spriteBatch.End();
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _renderableMapper = mapperService.GetMapper<RenderableComponent>();
            _positionMapper = mapperService.GetMapper<PositionComponent>();
            _spatialMapper = mapperService.GetMapper<SpatialComponent>();
        }
    }
}