using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Galaga
{
    class Bullet : Sprite
    {
        int speed; // speed of bullet
        const int width = 5, height = 7;
        public Bullet(Vector2 position)
        {
            this.position = position;
            speed = 4;
        }

        public void Update(Vector2 dir)
        {
            position += dir * speed;
        }
    }
}
