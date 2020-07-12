using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace Galaga
{
    class EnemyController
    {
        private List<Enemy> enemies;  // List of all enemies on screen
        private List<Vector2> explosions;
        private Texture2D explosionTexture;
        private uint maxAllowedToAttack = 2;
        private int timeBetweenWaves = 3;
        private int spaceBetweenShips = 10;
        private bool startWave = false;
        private bool HasNotBeenCalledYet = true;
        bool once = true;
        int numOffScreen = 0;
        Random rand;  // Random number generator used for determining enemy locations

        public EnemyController()
        {
            // Initialize
            enemies = new List<Enemy>();
            explosions = new List<Vector2>();
            rand = new Random();
        }

        public void SpawnEnemies(ContentManager content, uint numberOfEnemies, uint maxAttacking)
        {
            maxAllowedToAttack = maxAttacking;
            if (once)
            {
                explosionTexture = content.Load<Texture2D>("explosion");
                once = false; 
            }
            // Spawn enemies
            for (int i = 0; i < numberOfEnemies; i++)
            {
                Add(content);
            }

        }

        public bool StageCleared()
        {
            return enemies.Count == 0 && explosions.Count == 0;
        }

        /*
         * Clear all enemies instantly (no explosion)
         */
        public void Reset()
        {
            enemies.Clear();
            startWave = false;
            numOffScreen = 0;
        }

        /*
         * Returns list of enemies
         */
        public List<Enemy> Enemies
        {
            get { return enemies; }
        }

        /*
         * Create a new enemy
         */
        public void Add(ContentManager content)
        {
            Enemy enemy = new Enemy("enemy", rand, PickSpawnLocation());
            enemy.Load(content);
            enemies.Add(enemy);
        }

        private int PickSpawnLocation()
        {
            int posX = 0; // Spawn Location variable
            bool positionFound = false; // Variable to see if a valid position is found

            // Loop until position is found
            while (!positionFound)
            {
                bool collision = false; 

                // Pick a random location
                posX = rand.Next(0, Game1.gameWindow.Width - (int)Sprite.Size.X);

             //   Enemy newEnemy = new Enemy("enemy", rand, posX); // Instantiate enemy at location
                foreach (Enemy enemy in enemies)
                {
                    // If there is a collision set collision to true and break
                    if (enemy.CollisionX(posX, spaceBetweenShips))
                    {
                        collision = true;
                        break;
                    }
                }

                // If there is no collision break from loop
                if (!collision)
                {
                    positionFound = true;
                }
            }

            return posX;
        }

        /*
         * Remove enemy from list
         */
        public void Kill(Enemy enemy)
        {
            enemy.Kill(); // Trigger explosion
            
            explosions.Add(enemy.Position);
            
            enemies.Remove(enemy);

            if (enemy.IsAttacking) { numOffScreen++; }

            // Remove explosion after 1 second
            Task.Delay(new TimeSpan(0, 0, 1)).ContinueWith(o => { explosions.Remove(enemy.Position); });

        }

        /*
         * Draw each enemy
         */
        public void Draw(SpriteBatch spriteBatch)
        {
            foreach (Enemy enemy in enemies)
            {
                enemy.Draw(spriteBatch);
            }

            foreach (Vector2 explosion in explosions)
            {
                spriteBatch.Draw(explosionTexture, explosion, Color.White);
            }

        }

        /*
         * Determine if the enemies are spawning
         */
        private bool Spawning()
        {
            bool spawning = false;
            foreach (Enemy enemy in enemies)
            {
                // If any enemy is spawning return true
                if (enemy.Spawning) { spawning = true; break; }
            }
            return spawning;
        }

        /*
         * Returns number of enemies attacking player
         */
        public int GetNumOfAttackers()
        {
            int attackers = 0;
            foreach (Enemy enemy in enemies)
            {
                if (enemy.IsAttacking) { attackers++; }
            }
            return attackers;
        }

        /*
         * Choose which enemies should attack the player
         */
        private void ChooseAttackers()
        {
            // If there are more allowed to attack than enemies on screen make all enemies attack
            if (maxAllowedToAttack >= enemies.Count)
            {
                foreach (Enemy enemy in enemies)
                {
                    enemy.StartAttack();
                }
            }
            // If there are more enemies than allowed to attack randomly select some to attack
            else
            {
                while (GetNumOfAttackers() < maxAllowedToAttack )
                {
                    int enemyIndex = rand.Next(0, enemies.Count); // Randomly select enemy index
                    if (!enemies[enemyIndex].IsAttacking)
                    {
                        enemies[enemyIndex].StartAttack();
                    }
                }
            }

            if (GetNumOfAttackers() == 0 && enemies.Count > 0)
            {
                ChooseAttackers();
            }
        }

        public bool StartWave()
        {
            return startWave;
        }

        // Just for drawing to screen
        public int GetNumOffScreen()
        {
            return numOffScreen;
        }

        private void WaveLogic(Player player)
        {


            #region Update Enemies
            // Update enemies

            for (int i = 0; i < enemies.Count; i++)
            {

                if (enemies[i].IsAttacking && enemies[i].OffScreen)
                {
                    numOffScreen++;
                    enemies[i].Reset();
                }

                // Check if any enemy collides with player but only if they are attacking
                if (enemies[i].IsAttacking && enemies[i].Collision(player))
                {
                    Kill(enemies[i]);
                    player.Dead = true;
                }

            }

            // Buggy I think
            // If all attacking enemies go off screen, end wave
            if (enemies.Count > maxAllowedToAttack)
            {
                if (numOffScreen >= maxAllowedToAttack)
                {
                    startWave = false;
                    numOffScreen = 0;
                }
            }

            else
            {
                if (numOffScreen >= enemies.Count)
                {
                    startWave = false;
                    numOffScreen = 0;
                }
            }

        }


        public void Update(Player player)
        {
            // Allow enemies to Spawn in
            foreach (Enemy enemy in enemies)
            {
                enemy.Update(player);
                enemy.Start(startWave);
            }

            // Once all enemies finish spawning start wave
            if (!startWave && HasNotBeenCalledYet && !Spawning())
            {
                HasNotBeenCalledYet = false;

                // If no enemies are attacking, choose some to attack
                ChooseAttackers();

                Task.Delay(new TimeSpan(0, 0, timeBetweenWaves)).ContinueWith(o =>
                { startWave = true; HasNotBeenCalledYet = true;});
            }

            if (startWave)
            {
                WaveLogic(player);
            }


            #endregion
        }
    }
}
