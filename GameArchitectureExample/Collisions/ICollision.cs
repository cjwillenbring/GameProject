using System;
using System.Collections.Generic;
using System.Text;

namespace GameArchitectureExample.Collisions
{
    public interface ICollision
    {
        public float X { get; set; }
        public float Y { get; set; }
        public bool CollidesWith(BoundingPoint p);
        public bool CollidesWith(BoundingCircle c);
        public bool CollidesWith(BoundingRectangle c);
    }
}
