using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pong
{
    public class CollisionDetector
    {
        public static bool Overlaps(IPhysicalObject2D a, IPhysicalObject2D b)
        {
             
            MyPoint topLeftPoint = new MyPoint(a.X, a.Y);
            MyPoint topRightPoint = new MyPoint(a.X + a.Width, a.Y);
            MyPoint bottomLeftPoint = new MyPoint(a.X, a.Y + a.Height);
            MyPoint bottomRightPoint = new MyPoint(a.X + a.Width, a.Y + a.Height);
           
            if (checkCollision(topLeftPoint, b)) return true;
            if (checkCollision(topRightPoint, b)) return true;
            if (checkCollision(bottomLeftPoint, b)) return true;
            if (checkCollision(bottomRightPoint, b)) return true;

            return false;

        }
        private static bool checkCollision(MyPoint point, IPhysicalObject2D b)
        {
            if ((b.X <= point.X && point.X <= (b.X + b.Width)) && (b.Y <= point.Y && point.Y <= (b.Y + b.Height)))
            {
                return true;
            }

            return false;
        }

    }
}
