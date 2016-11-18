using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedRealityPoker.Game.Maths_Library
{
    class Vector2D
    {
        public float x, y;

        public Vector2D()
        {
            x = y = 0;
        }

        public Vector2D( float x, float y)
        {
            this.x = x;
            this.y = y;
        }

        public void normalise()
        {
            
        }

        public void zero()
        {
            x = 0;
            y = 0;
        }

        public static Vector2D operator+ (Vector2D vector1, Vector2D vector2)
        {
            return new Maths_Library.Vector2D(vector1.x + vector2.x, vector1.y + vector2.y);
        }

        public static Vector2D operator+ (Vector2D vector, int scalar)
        {
            return new Maths_Library.Vector2D(vector.x + scalar, vector.y + scalar);
        }
    }
}
