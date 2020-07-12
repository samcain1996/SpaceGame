using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;

namespace Galaga
{
    class StarGenerator
    {
        List<Star> stars;
        Texture2D starTexture;
        Random rand;
        int numOfStars;
        public StarGenerator(int numOfStars)
        {
            rand = new Random();
            stars = new List<Star>();
            this.numOfStars = numOfStars;
        }

        public void Load(ContentManager content)
        {
            starTexture = content.Load<Texture2D>("star");
            GenerateStars();
        }

        class Star
        {
            int speed; // speed stars travel
            const int height = 7, width = 5; // Star size
            Texture2D starTexture; // Star texture (white)
            Color color; // Color of star
            Rectangle destRect; // Location of star
            Random rand;

            public Star(Texture2D starTexture, Random rand)
            {
                speed = 2;
                this.rand = rand;
                this.starTexture = starTexture;

                // Generate a random color for star
                color = RandomColor();

                // Generate a random starting position
                destRect = new Rectangle(rand.Next(Game1.gameWindow.Width - width),
                    rand.Next(Game1.gameWindow.Height), width, height);
            }

            /**
             * Returns a random color
             */
            private Color RandomColor()
            {
                Vector3 colorData = new Vector3((float)rand.NextDouble()*.5f, 
                    (float)rand.NextDouble()*.5f, (float)rand.NextDouble()*.5f);
                return new Color(colorData);
            }

            public void Draw(SpriteBatch spriteBatch)
            {
                spriteBatch.Draw(starTexture, destRect, color);
            }

            public void Update()
            {
                destRect.Y += speed;

                // Bring star back to top and change color 
                if (destRect.Y > Game1.gameWindow.Height - height)
                {
                    destRect.Y = 0;
                    destRect.X = rand.Next(Game1.gameWindow.Width - width);
                    color = RandomColor();
                }
            }
        }

        /**
         * Create stars
         */
        private void GenerateStars()
        {
            for (int i = 0; i < numOfStars; i++)
            {
                stars.Add(new Star(starTexture, rand));
            }
        }

        public void Update(GameTime gameTime)
        {
            foreach (Star star in stars)
            {
                star.Update();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Star star in stars)
            {
                star.Draw(spriteBatch);
            }
        }

    }
}
