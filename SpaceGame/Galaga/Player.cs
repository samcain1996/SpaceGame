using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

namespace Galaga
{
    class Player : Sprite
    {
        List<Bullet> bullets;  // Collection of bullets fired
        int maxBullets = 2;    // Maximum number of bullets player can have on screen
        float timeSinceLastClick = 0f;  // Keeps track of how much time has passed since last shot
        private Texture2D playerBulletTexture, playerExplosion;
        private SoundEffect playerBulletSound;

        public Player(string texturePath)
        {
            this.texturePath = texturePath;
            Dead = false;
            bullets = new List<Bullet>();
            ResetPosition();
        }

        /*
         * Files needed for Player
         */
        public void Load(ContentManager content)
        {
            texture = base.Load<Texture2D>(content, texturePath);
            playerExplosion = base.Load<Texture2D>(content, "explosion");
            playerBulletTexture = base.Load<Texture2D>(content, "star"); 
            playerBulletSound = base.Load<SoundEffect>(content, "laser");
        }

        public void ResetPosition()
        {
            position = new Vector2(Game1.gameWindow.Width / 2 - Size.X / 2,
                Game1.gameWindow.Height - Size.Y);
        }

        public void Reset()
        {
            bullets.Clear();
        }

        /*
         * Draw player on screen
         */
        override public void Draw(SpriteBatch spriteBatch)
        {
            destRect = new Rectangle((int)position.X, (int)position.Y, (int)Size.X, (int)Size.Y);
            spriteBatch.Draw(Dead ? playerExplosion : texture, destRect, Color.White);
            foreach (Bullet bullet in bullets)
            {
                bullet.Draw(spriteBatch, playerBulletTexture, new Vector2(5, 7));
            }
            //spriteBatch.DrawString(font, Convert.ToString(bullets.Count), new Vector2(0, 0), Color.White);
        }

        public void Update(EnemyController ec, GameTime gameTime)
        {
            InputLogic(gameTime);

            if (bullets.Count > 0)
            {

                // Remove bullets once they are off screen
                for (int i = 0; i < bullets.Count(); i++)
                {
                    bullets[i].Update(Directions.UP);
                    if (bullets[i].Position.Y < 0)
                    {
                        bullets.RemoveAt(i);
                    }
                }

                // Remove enemy and bullet once a collision occurs
                for (int i = 0; i < bullets.Count; i++)
                {
                    for (int x = 0; x < ec.Enemies.Count; x++)
                    {
                        if (bullets[i].Collision(ec.Enemies[x]))
                        {
                            ec.Kill(ec.Enemies[x]);
                            bullets.RemoveAt(i);
                            break; // Less enemies so leave i guess
                        }
                    }
                }
            }
        }

        public bool Dead { get; set; }

        /*
         * Handle player inputs
         */
        private void InputLogic(GameTime gameTime)
        {
            // Add time elapsed since last method call
            timeSinceLastClick += (float)gameTime.ElapsedGameTime.TotalMilliseconds;

            // Get state of keyboard and handle keyboard inputs
            KeyboardState keyboard = Keyboard.GetState();
            if (keyboard.IsKeyDown(Keys.A))
            {
                position.X -= 3;
            }
            else if (keyboard.IsKeyDown(Keys.D))
            {
                position.X += 3;
            }

            // Spawn bullet when the space bar is pressed
            if (keyboard.IsKeyDown(Keys.Space) && timeSinceLastClick > 250)
            {
                timeSinceLastClick = 0f; // Reset time since space was last pressed
                if (bullets.Count() < maxBullets)
                {
                    Vector2 bulletPos = new Vector2(position.X, position.Y);

                    SoundEffectInstance sound = playerBulletSound.CreateInstance();
                    sound.Play();

                    bullets.Add(new Bullet(bulletPos));
                }
            }

            // Ensure player stays within screen bounds
            if (position.X < 0)
            {
                position.X = 0;
            }
            else if ((position.X + 32) > Game1.gameWindow.Width)
            {
                position.X = (Game1.gameWindow.Width - 32);
            }
        }

    }
}
