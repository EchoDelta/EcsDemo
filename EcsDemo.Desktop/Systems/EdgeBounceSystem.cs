using EcsDemo.Desktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace EcsDemo.Desktop.Systems
{
    public class EdgeBounceSystem : EntityProcessingSystem
    {
        private readonly float _screenWidth;
        private readonly float _screenHeight;
        private ComponentMapper<VelocityComponent> _velocityMapper;
        private ComponentMapper<PositionComponent> _postitionMapper;
        private ComponentMapper<SpatialComponent> _spatialMapper;

        public EdgeBounceSystem(float screenWidth, float screenHeight) : base(Aspect.All(typeof(VelocityComponent), typeof(PositionComponent)))
        {
            _screenWidth = screenWidth;
            _screenHeight = screenHeight;
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _velocityMapper = mapperService.GetMapper<VelocityComponent>();
            _postitionMapper = mapperService.GetMapper<PositionComponent>();
            _spatialMapper = mapperService.GetMapper<SpatialComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var velocity = _velocityMapper.Get(entityId);
            var position = _postitionMapper.Get(entityId);
            var spatial = _spatialMapper.Get(entityId);

            if (position.Position.X < 0)
            {
                position.Position.SetX(0);
                velocity.Velocity *= new Vector2(-1, 1);
            }
            
            if (position.Position.X > _screenWidth - (spatial?.Width ?? 0))
            {
                position.Position.SetX(_screenWidth);
                velocity.Velocity *= new Vector2(-1, 1);
            }
            
            if (position.Position.Y < 0)
            {
                position.Position.SetY(0);
                velocity.Velocity *= new Vector2(1, -1);
            }

            if (position.Position.Y > _screenHeight - (spatial?.Height ?? 0))
            {
                position.Position.SetY(_screenHeight);
                velocity.Velocity *= new Vector2(1, -1);
            }
        }
    }
}