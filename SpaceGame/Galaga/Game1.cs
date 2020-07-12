using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Timers;

namespace Galaga
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    /// 
    public class Game1 : Game
    {
        public static StringBuilder sb;  // Used for debugging
        bool newLevel = true, gameOver = false;
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        public static Rectangle gameWindow;
        Player p;
        EnemyController ec;
        LevelManager levelManager;
        StarGenerator sg;
        SpriteFont font; 

        public Game1()
        {
            sb = new StringBuilder();
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            gameWindow = Window.ClientBounds; // Bounds of window
            p = new Player("ship");
            sg = new StarGenerator(100);
            ec = new EnemyController();
            levelManager = new LevelManager();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {

            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            p.Load(Content);
            sg.Load(Content);
            font = Content.Load<SpriteFont>("Fonts/Ass");
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            sg.Update(gameTime);

            if (p.Dead) { gameOver = true; }

            #region Main Loop

            if (!gameOver)
            {
                // If there are no enemies the level has been beaten
                if (ec.StageCleared() && !newLevel)
                {
                    levelManager.BeatLevel(); // Level has been beaten, increment level number
                    p.Reset(); // Clear bullets from screen
                    newLevel = true;  // Level has been beaten
                }

                // If new level, wait until enter is hit
                if (newLevel)
                {
                    if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                    {
                        newLevel = false;
                        p.ResetPosition(); // Reset player to middle of screen
                        ec.Reset();
                        ec.SpawnEnemies(Content, levelManager.GetEnemyNumber,
                            levelManager.GetAttackers); // Get new number of enemies to spawn
                    }
                }

                // Update EnemyController and Player if level has not ended
                else
                {
                    ec.Update(p);
                    p.Update(ec, gameTime);
                }
            }

            #endregion

            #region Game Over 

            else
            {
                if (Keyboard.GetState().IsKeyDown(Keys.Enter))
                {
                    levelManager.Reset();
                    ec.Reset();
                    gameOver = false;
                    p.Dead = false;
                }
            }

            #endregion

            File.AppendAllText("log.txt", sb.ToString());

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            spriteBatch.Begin();

            // Draw background
            sg.Draw(spriteBatch);

            if (!gameOver)
            {
                p.Draw(spriteBatch);
                spriteBatch.DrawString(font, System.Convert.ToString(ec.GetNumOffScreen()) + ", " +
                    System.Convert.ToString(ec.StartWave()) + ", " + System.Convert.ToString(ec.GetNumOfAttackers()),
                        new Vector2(gameWindow.Width - 250, gameWindow.Height / 2), Color.White);
                // If new level draw text telling player to hit enter
                if (newLevel)
                {
                    spriteBatch.DrawString(font, "Level: " + levelManager.GetLevel + "\nEnter to continue",
                        new Vector2(gameWindow.Width / 4, gameWindow.Height / 2), Color.White);
                }

                // Draw player and level number
                else
                {
                    ec.Draw(spriteBatch);
                    spriteBatch.DrawString(font, "Level: " + levelManager.GetLevel, new Vector2(0, 0), Color.White);
                }
            }

            else
            {
                spriteBatch.DrawString(font, "Game Over\nPress Enter to start over", 
                    new Vector2(gameWindow.Width / 3, gameWindow.Height / 2), Color.White);
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
    }
}
