using EcsDemo.Desktop.Components;
using Microsoft.Xna.Framework;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace EcsDemo.Desktop.Systems
{
    public class BrickColorSystem : EntityProcessingSystem
    {
        private ComponentMapper<RenderableComponent> _renderableMapper;
        private ComponentMapper<BreakableComponent> _breakableMapper;

        public BrickColorSystem() : base(Aspect.All(typeof(CollidableComponent), typeof(RenderableComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _renderableMapper = mapperService.GetMapper<RenderableComponent>();
            _breakableMapper = mapperService.GetMapper<BreakableComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var renderable = _renderableMapper.Get(entityId);
            var breakable = _breakableMapper.Get(entityId);

            if (breakable == null)
            {
                renderable.Color = Color.Green;
            }

            else if (breakable.Health > 1)
            {
                renderable.Color = Color.Yellow;
            }

            else if (breakable.Health <= 1)
            {
                renderable.Color = Color.Red;
            }
        }
    }
}