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
        DesignALevel,
        MainMenu,
        PauseMenu,
        LevelEdit,
        TestPlay,
        Exiting,
        GamePlay
    }


    public class Game1 : Game
    {

        GameState gs;
        public static GraphicsDeviceManager graphics;

        MainMenu m_mainmenu;
        LevelEdit m_levelEdit;

        SpriteBatch spriteBatch;
        InputManager m_input;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            //Window.IsBorderless = true;

            IsMouseVisible = true;
        }

        protected override void Initialize()
        {

            gs = GameState.MainMenu;
            Pixelclass.Content = Content;
            Pixelclass.Pixel = Content.Load<Texture2D>("Pixel");
            Pixelclass.Font = Content.Load<SpriteFont>("Font1");

            //if (gs == GameState.LevelEdit)
            //    m_input = new InputManager();
            //else
            m_input = new InputManager(PlayerIndex.One);

            m_levelEdit = new LevelEdit();

            base.Initialize();
        }

        protected override void LoadContent()
        {
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //Pixelclass.Pixel = Content.Load<Texture2D>("Pixel");
            //Pixelclass.Font = Content.Load<SpriteFont>("Font1");

            //Have to innitalise here because font needs to exist for setup
            m_mainmenu = new MainMenu(new Vector2 (20,30));


        }

        protected override void UnloadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (gs == GameState.Exiting)
            {
                Exit();
            }
            if (gs == GameState.DesignALevel)
            {
                m_levelEdit.UpdateMe(gameTime, m_input, ref gs);
            }
            else if (gs == GameState.MainMenu)
            {
                m_mainmenu.UpdateMe(gameTime, m_input, ref gs);
            }



            m_input.UpdateMe();
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            spriteBatch.Begin();
            if (gs == GameState.DesignALevel)
            {
                m_levelEdit.DrawMe(spriteBatch);
            }
            else if (gs == GameState.MainMenu)
            {
                m_mainmenu.Drawme(spriteBatch);
            }

            spriteBatch.End();

            base.Draw(gameTime);
        }
    }

}
