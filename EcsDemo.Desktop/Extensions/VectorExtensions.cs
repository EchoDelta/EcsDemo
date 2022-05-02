using System;
using Microsoft.Xna.Framework;

namespace EcsDemo.Desktop.Extensions
{
    public static class VectorExtensions
    {
        public static Vector2 Deflect(this Vector2 vector, Vector2 normal)
        {
            var x = vector.X;
            var y = vector.Y;
            if (Math.Abs(normal.X) > 0.0001f)
            {
                x *= -1;
            }
            
            if (Math.Abs(normal.Y) > 0.0001f)
            {
                y *= -1;
            }

            return new Vector2(x, y);
        }
    }
}