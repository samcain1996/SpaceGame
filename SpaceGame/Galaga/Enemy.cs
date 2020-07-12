using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

namespace Galaga
{
    class Enemy : Sprite
    {
        SoundEffect explosionSound;
        List<Bullet> bullets;
        int maxBullets = 1;
        private bool isAttacking, spawning, offscreen, start;
        private int speed;

        public Enemy(string texturePath, Random r, int posX)
        {
            speed = r.Next(3, 5);
            isAttacking = false;
            spawning = true;
            this.texturePath = texturePath;
            position.Y = 0;
            position.X = posX;
        }

        public void Kill()
        {
            SoundEffectInstance s = explosionSound.CreateInstance();
            s.Play();
        }

        public void Start(bool start)
        {
            this.start = start;
        }

        /*
         * Reset values
         */
        public void Reset()
        {
            position.Y = 0;
            spawning = true;
            offscreen = false;
            isAttacking = false;
            start = false;
        }

        public bool OffScreen
        {
            get { return offscreen; }
        } 

        /*
         * Load data needed for class
         */
        public void Load(ContentManager content)
        {
            texture = base.Load<Texture2D>(content,"enemy");
            explosionSound = base.Load<SoundEffect>(content, "deathSound");
        }


        public bool Spawning
        {
            get { return spawning; }
        }

        public void StartAttack()
        {
            isAttacking = true;
        }

        public void Update(Player player)
        {
            #region Spawning


            // If the enemy is spawning fly in until halfway down screen
            if (spawning)
            {
                if (position.Y < Game1.gameWindow.Height / 2 - Size.Y / 4)
                {
                    position.Y += speed;
                }
                else
                {
                    spawning = false;
                }
            }

            #endregion

            #region Alive Logic
            // If the enemy is alive and the wave has started begin wave
            if (start && !spawning)
            {
                // If enemy is attacking move towards player
                if (isAttacking && !offscreen)
                {
                    if (position.Y > Game1.gameWindow.Height)
                    {
                        //position.Y = 0;
                        offscreen = true;
                    }
                    position.Y += speed;
                    if (position.X + Size.X < player.Position.X)
                    {
                        position.X++;
                    }
                    else
                    {
                        position.X--;
                    }
                }
            }

            #endregion
        }

        /*
         * Returns whether the enemy is attacking or not
         */
        public bool IsAttacking
        {
            get { return isAttacking; }
        }

    }
}
