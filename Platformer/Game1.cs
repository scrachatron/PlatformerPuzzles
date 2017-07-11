using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace Platformer
{
    /// <summary>
    /// This is the main type for your game.
    /// </summary>
    //public class Game1 : Game
    //{
    //GraphicsDeviceManager graphics;
    //SpriteBatch spriteBatch;

    //public Game1()
    //{
    //graphics = new GraphicsDeviceManager(this);
    //Content.RootDirectory = "Content";
    //}

    ///// <summary>
    ///// Allows the game to perform any initialization it needs to before starting to run.
    ///// This is where it can query for any required services and load any non-graphic
    ///// related content.  Calling base.Initialize will enumerate through any components
    ///// and initialize them as well.
    ///// </summary>
    //protected override void Initialize()
    //{
    //// TODO: Add your initialization logic here

    //base.Initialize();
    //}

    ///// <summary>
    ///// LoadContent will be called once per game and is the place to load
    ///// all of your content.
    ///// </summary>
    //protected override void LoadContent()
    //{
    //// Create a new SpriteBatch, which can be used to draw textures.
    //spriteBatch = new SpriteBatch(GraphicsDevice);

    //// TODO: use this.Content to load your game content here
    //}

    ///// <summary>
    ///// UnloadContent will be called once per game and is the place to unload
    ///// game-specific content.
    ///// </summary>
    //protected override void UnloadContent()
    //{
    //// TODO: Unload any non ContentManager content here
    //}

    ///// <summary>
    ///// Allows the game to run logic such as updating the world,
    ///// checking for collisions, gathering input, and playing audio.
    ///// </summary>
    ///// <param name="gameTime">Provides a snapshot of timing values.</param>
    //protected override void Update(GameTime gameTime)
    //{
    //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
    //Exit();

    //// TODO: Add your update logic here

    //base.Update(gameTime);
    //}

    ///// <summary>
    ///// This is called when the game should draw itself.
    ///// </summary>
    ///// <param name="gameTime">Provides a snapshot of timing values.</param>
    //protected override void Draw(GameTime gameTime)
    //{
    //GraphicsDevice.Clear(Color.CornflowerBlue);

    //// TODO: Add your drawing code here

    //base.Draw(gameTime);
    //}
    //}

    public enum GameState
    {
        MainMenu,
        PauseMenu,
        LevelEdit,
        GamePlay
    }


    public class Game1 : Game
    {

        GameState gs;
        public static GraphicsDeviceManager graphics;
        
        SpriteBatch spriteBatch;
        Dictionary<string, Level> m_levels;
        Player m_Player;
        InputManager m_input;
        MapEdit edit;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Window.IsBorderless = true;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            gs = GameState.LevelEdit;
            Pixelclass.Content = Content;
            m_levels = new Dictionary<string, Level>();
            m_Player = new Player();
            //if (gs == GameState.LevelEdit)
            //    m_input = new InputManager();
            //else
                m_input = new InputManager(PlayerIndex.One);
            edit = new MapEdit();
            string fileLocation = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + @"\Pixel Studios\Platformer\levels.xml";




            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            Pixelclass.Pixel = Content.Load<Texture2D>("Pixel");
            Pixelclass.Font = Content.Load<SpriteFont>("Font1");

            m_levels.Add("World1", SaveDirector.LoadData("mylvl"));

            m_Player.SetStartPosition(m_levels["World1"]);
        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
            {

            }

            if (m_input.WasPressedBack(Keys.Enter))
            {
                if (gs == GameState.GamePlay)
                {
                    gs = GameState.LevelEdit;
                    edit.ForceLoad();
                }
                else
                {
                    m_levels.Clear();
                    m_levels.Add("World1", SaveDirector.LoadData("mylvl"));
                    m_Player.SetStartPosition(m_levels["World1"]);
                    gs = GameState.GamePlay;
                }
            }


            if (gs == GameState.GamePlay)
            {
                m_Player.UpdateMe(gameTime, m_levels["World1"], m_input);
                m_levels["World1"].UpdateMe(gameTime, m_Player, m_input);
            }
            else if (gs == GameState.LevelEdit)
                edit.UpdateMe(gameTime, m_input);


            m_input.UpdateMe();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();

            if (gs == GameState.GamePlay)
            {
                m_levels["World1"].DrawMe(spriteBatch);
                m_Player.DrawMe(spriteBatch);
            }
            else if (gs == GameState.LevelEdit)
            {
                edit.DrawMe(spriteBatch);

            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
