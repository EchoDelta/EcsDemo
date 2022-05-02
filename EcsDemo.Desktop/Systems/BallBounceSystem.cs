using System.Collections.Generic;
using System.Linq;
using EcsDemo.Desktop.Components;
using EcsDemo.Desktop.Extensions;
using Microsoft.Xna.Framework;
using MonoGame.Extended;
using MonoGame.Extended.Entities;
using MonoGame.Extended.Entities.Systems;

namespace EcsDemo.Desktop.Systems
{
    public class BallBounceSystem : EntityProcessingSystem
    {
        private ComponentMapper<VelocityComponent> _velocityMapper;
        private ComponentMapper<PositionComponent> _postitionMapper;
        private ComponentMapper<SpatialComponent> _spatialMapper;
        private ComponentMapper<CollidableComponent> _collidableMapper;

        private readonly Dictionary<int, RectangleF> _collidables = new Dictionary<int, RectangleF>();

        public BallBounceSystem() : base(Aspect.All(typeof(VelocityComponent), typeof(PositionComponent)))
        {
        }

        public override void Initialize(IComponentMapperService mapperService)
        {
            _velocityMapper = mapperService.GetMapper<VelocityComponent>();
            _postitionMapper = mapperService.GetMapper<PositionComponent>();
            _spatialMapper = mapperService.GetMapper<SpatialComponent>();
            _collidableMapper = mapperService.GetMapper<CollidableComponent>();
        }

        public override void Process(GameTime gameTime, int entityId)
        {
            var ballVelocity = _velocityMapper.Get(entityId);
            var ballPosition = _postitionMapper.Get(entityId);
            var ballSpatial = _spatialMapper.Get(entityId);
            var ball = new RectangleF(ballPosition.Position, new Size2(ballSpatial.Width, ballSpatial.Height));

            var ballBroadPhase = ball.GetBroadPhaseBox(ballVelocity.Velocity);

            foreach (var (collidableEnityId, collidable) in _collidables.Where(pair => pair.Value.Intersects(ballBroadPhase)))
            {
                var (entrytime, normal) = ball.GetSweptAABB(ballVelocity.Velocity*gameTime.GetElapsedSeconds(), collidable);
                if (normal != Vector2.Zero)
                {
                    // Deflect
                    ballPosition.Position += ballVelocity.Velocity * gameTime.GetElapsedSeconds() * entrytime;
                    ballVelocity.Velocity = ballVelocity.Velocity.Deflect(normal);
                    ballPosition.Position -= ballVelocity.Velocity * gameTime.GetElapsedSeconds() * (1-entrytime);
                }
            }
        }

        protected override void OnEntityAdded(int entityId)
        {
            if (_collidableMapper.Has(entityId))
            {
                AddCollidable(entityId);
            }
        }

        protected override void OnEntityChanged(int entityId)
        {
            var collidableComponent = _collidableMapper.Get(entityId);
            if (_collidables.ContainsKey(entityId) && collidableComponent == null)
            {
                _collidables.Remove(entityId);
            }
            else if(!_collidables.ContainsKey(entityId) && collidableComponent != null)
            {
                AddCollidable(entityId);
            }
        }

        protected override void OnEntityRemoved(int entityId)
        {
            _collidables.Remove(entityId);
        }

        private void AddCollidable(int entityId)
        {
            var collidablePosition = _postitionMapper.Get(entityId);
            var collidableSpatial = _spatialMapper.Get(entityId);
            var collidable = new RectangleF(collidablePosition.Position,
                new Size2(collidableSpatial.Width, collidableSpatial.Height));
            _collidables.Add(entityId, collidable);
        }
    }
}