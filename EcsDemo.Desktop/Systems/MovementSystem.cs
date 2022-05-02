using EcsDemo.Desktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace EcsDemo.Desktop.Systems
{
    public class MovementSystem : EntityProcessingSystem
    {
        private ComponentMapper<VelocityComponent> _velocityMapper;
        private ComponentMapper<PositionComponent> _postitionMapper;

        public MovementSystem() : base(Aspect.All(typeof(VelocityComponent), typeof(PositionComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _velocityMapper = mapperService.GetMapper<VelocityComponent>();
            _postitionMapper = mapperService.GetMapper<PositionComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var velocity = _velocityMapper.Get(entityId);
            var position = _postitionMapper.Get(entityId);

            position.Position += velocity.Velocity * gameTime.GetElapsedSeconds();
        }
    }
}