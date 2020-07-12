using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Galaga
{
    static class Directions
    {
        public readonly static Vector2 UP = new Vector2(0, -1);
        public readonly static Vector2 DOWN = new Vector2(0, 1);
        public readonly static Vector2 LEFT = new Vector2(-1, 0);
        public readonly static Vector2 RIGHT = new Vector2(1, 0);
    }
}
