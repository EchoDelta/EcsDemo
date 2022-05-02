using System;
using Microsoft.Xna.Framework;
using MonoGame.Extended;

namespace EcsDemo.Desktop.Extensions
{
    public static class CollisionExtensions
    {
        public static RectangleF GetBroadPhaseBox(this RectangleF collidable, Vector2 velocity)
        {
            var posX = velocity.X > 0 ? collidable.X : collidable.X + velocity.X;
            var posY = velocity.Y > 0 ? collidable.Y : collidable.Y + velocity.Y;
            var width = velocity.X > 0 ? collidable.Width + velocity.X : collidable.Width - velocity.X;
            var height = velocity.Y > 0 ? collidable.Height + velocity.Y : collidable.Height - velocity.Y;
            return new RectangleF(posX, posY, width, height);
        }
        
        public static (float, Vector2) GetSweptAABB(this RectangleF movingObject, Vector2 velocity, RectangleF collisionObject)
        {
            var invEntry = new Vector2(
                velocity.X > 0
                    ? collisionObject.X - movingObject.X - movingObject.Width
                    : collisionObject.X + collisionObject.Width - movingObject.X,
                velocity.Y > 0
                    ? collisionObject.Y - movingObject.Y - movingObject.Height
                    : collisionObject.Y + collisionObject.Height - movingObject.Y);

            var invExit = new Vector2(
                velocity.X > 0
                    ? collisionObject.X + collisionObject.Width - movingObject.X
                    : collisionObject.X - movingObject.X - movingObject.Width,
                velocity.Y > 0
                    ? collisionObject.Y + collisionObject.Height - movingObject.Y
                    : collisionObject.Y - movingObject.Y - movingObject.Height);

            var entry = new Vector2(
                velocity.X == 0.0f ? float.NegativeInfinity : invEntry.X / velocity.X,
                velocity.Y == 0.0f ? float.NegativeInfinity : invEntry.Y / velocity.Y);
            var exit = new Vector2(
                velocity.X == 0.0f ? float.PositiveInfinity : invExit.X / velocity.X,
                velocity.Y == 0.0f ? float.PositiveInfinity : invExit.Y / velocity.Y);

            var entryTime = Math.Max(entry.X, entry.Y);
            var exitTime = Math.Min(exit.X, exit.Y);

            if (entryTime > exitTime || entry.X < 0.0f && entry.Y < 0.0f || entry.X > 1.0f || entry.Y > 1.0f)
            {
                return (1.0f, Vector2.Zero);
            }

            var normal = Vector2.Zero;
            if (entry.X > entry.Y)
            {
                normal = -Math.Sign(invEntry.X) * Vector2.UnitX;
            }
            else
            {
                normal = -Math.Sign(invEntry.Y) * Vector2.UnitY;
            }
            return (entryTime, normal);
        }
    }
}