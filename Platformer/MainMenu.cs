using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{

    class MainMenu : Menu
    {
        private string m_TitleCard;
        private Vector2 m_pos;
        private SpriteFont m_titleFont;

        public MainMenu(Vector2 start)
            : base(start)
        {
            
            base.Options.Add(new MenuOption("Play Campaign", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), GameState.GamePlay));
            base.Options.Add(new MenuOption("Design a Custom Campaign", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), GameState.LevelEdit));
            base.Options.Add(new MenuOption("Play Custom Campaign", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), GameState.LevelEdit));
            base.Options.Add(new MenuOption("Quit", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), GameState.Exiting));
            m_TitleCard = "Picksl";
            m_pos = new Vector2(((Game1.graphics.PreferredBackBufferWidth / 3) * 2 ) - 100
                , (Game1.graphics.PreferredBackBufferHeight / 3) * 2
                );
            m_titleFont = Content.Load<SpriteFont>("LargeFont");
        }
        public override void UpdateMe(GameTime gt, InputManager input, ref GameState globalgs)
        {
            base.UpdateMe(gt, input, ref globalgs);
        }
        public override void Drawme(SpriteBatch sb)
        {
            sb.DrawString(m_titleFont, m_TitleCard, m_pos, Color.Black, ((float)Math.PI / 32) * 0, Vector2.Zero, 2, SpriteEffects.None, 0);
            base.Drawme(sb);
        }
    }
}
