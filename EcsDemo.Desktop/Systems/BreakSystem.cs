using EcsDemo.Desktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace EcsDemo.Desktop.Systems
{
    public class BreakSystem : EntityProcessingSystem
    {
        private ComponentMapper<BreakableComponent> _breakableMapper;

        public BreakSystem() : base(Aspect.All(typeof(BreakableComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _breakableMapper = mapperService.GetMapper<BreakableComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var breakable = _breakableMapper.Get(entityId);
            if (breakable.Health <= 0)
            {
                DestroyEntity(entityId);
            }
        }
    }
}