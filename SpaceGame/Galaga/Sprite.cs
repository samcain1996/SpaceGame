using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Galaga
{
    abstract class Sprite
    {
        protected Texture2D texture; // Texture to use for sprite
        protected string texturePath; // Path to texture file
        protected Vector2 position; // X and Y position
        protected Rectangle destRect; // Rectangle to render

        /*
         * Return X and Y position of sprite
         */
        public Vector2 Position
        {
            get { return position; }
        }

        /*
         * Return the width and height of the sprite
         */
        public static Vector2 Size
        {
            get { return new Vector2(32, 32); }
        }


        /*
         * Load texture for sprite and bullet
         */
        virtual protected T Load<T>(ContentManager content, string path)
        {
            return content.Load<T>(path);
        }



        /*
         * Draw sprite
         */
        virtual public void Draw(SpriteBatch spriteBatch)
        {
            destRect = new Rectangle((int)position.X, (int)position.Y, 32, 32);
            spriteBatch.Draw(texture , destRect, Color.White);
        }

        virtual public void Draw(SpriteBatch spriteBatch, Texture2D texture, Vector2 size)
        {
            destRect = new Rectangle((int)position.X, (int)position.Y, (int)size.X, (int)size.Y);
            spriteBatch.Draw(texture, destRect, Color.White);
        }

        /*
         * Check if Sprite is colliding with another Sprite
         */
        public bool Collision(Sprite sprite)
        {
            if (position.X + Size.X >= sprite.Position.X && position.X <= sprite.Position.X + Size.X)
            {
                if (position.Y + Size.Y >= sprite.Position.Y && position.Y <= sprite.Position.Y + Size.Y)
                {
                    return true;
                }
            }
            return false;
        }

        /*
         * Check if sprite is colliding with another sprite after a buffer is applied
         */
        public bool Collision(Sprite sprite, int buffer)
        {
            if ((position.X + Size.X + buffer >= sprite.Position.X && position.X <= sprite.Position.X) ||
                (position.X - buffer <= sprite.Position.X + Size.X && position.X + Size.X >= sprite.Position.X + Size.X))
            {
                if ((position.Y + Size.Y >= sprite.Position.Y && position.Y <= sprite.Position.Y) ||
                    position.Y <= sprite.Position.Y + Size.Y && position.Y + Size.Y >= sprite.Position.Y + Size.Y)
                {
                    return true;
                }
            }
            return false;
        }

        public bool CollisionX(Sprite sprite, int buffer)
        {
            if ((position.X + Size.X + buffer >= sprite.Position.X && position.X <= sprite.Position.X) ||
                (position.X - buffer <= sprite.Position.X + Size.X && position.X + Size.X >= sprite.Position.X + Size.X))
            {

                return true;
            }
            return false;
        }

        public bool CollisionX(int posX, int buffer)
        {
            if ((position.X + Size.X + buffer >= posX && position.X <= posX) ||
                (position.X - buffer <= posX + Size.X && position.X + Size.X >= posX + Size.X))
            {

                return true;
            }
            return false;
        }

    }
}
