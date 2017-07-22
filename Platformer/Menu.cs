using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Platformer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    class Menu : Pixelclass
    {
        public List<MenuOption> Options
        {
            get
            {
                return m_options;
            }
            set
            {
                m_options = value;
            }
        }
        List<MenuOption> m_options = new List<MenuOption>();
        int m_selecton;
        protected Vector2 m_startingLocation;
        private MenuSelector m_selector;

        public Menu(Vector2 StartingLocation)
        {
            m_startingLocation = StartingLocation;
            m_selecton = 0;
            m_selector = new MenuSelector();
        }
        public virtual void UpdateMe(GameTime gt,InputManager input,ref GameState globalgs)
        {
            for (int i = 0; i < m_options.Count; i++)
                m_options[i].UpdateMe();

            if (input.WasPressedBack(Keys.Down))
            {
                m_selecton++;
                if (m_selecton >= m_options.Count)
                    m_selecton -= m_options.Count;
            }
            else if (input.WasPressedBack(Keys.Up))
            {
                m_selecton--;
                if (m_selecton < 0)
                    m_selecton += m_options.Count;
            }
            m_options[m_selecton].Selected = true;

            m_selector.UpdateMe(gt, m_options[m_selecton].m_targetPos);

            if (input.WasPressedBack(Keys.Enter))
            {
                globalgs = m_options[m_selecton].ChangeMeTo;
            }

        }
        public virtual void Drawme(SpriteBatch sb)
        {
            m_selector.Drawme(sb);
            for (int i = 0; i < m_options.Count; i++)
                m_options[i].DrawMe(sb);
        }
    }


    class MenuOption : Pixelclass
    {
        public Vector2 m_targetPos;
        public GameState ChangeMeTo
        {
            get { return ChangeTo; }
        }

        GameState ChangeTo;
        private string m_message;
        Vector2 m_pos;
        public bool Selected;

        public MenuOption(string message, Vector2 pos,GameState targetgs)
        {
            ChangeTo = targetgs;
            m_pos = pos;
            m_message = message;
            m_targetPos = new Vector2(m_pos.X + Font.MeasureString(message).X + 30, m_pos.Y + Font.MeasureString(message).Y / 2);
            Selected = false;
        }
        public void UpdateMe()
        {
            Selected = false;
        }

        public void DrawMe(SpriteBatch sb)
        {
            
            if (Selected)
                sb.DrawString(Font, m_message, m_pos, Color.Black);
            else
                sb.DrawString(Font, m_message, m_pos, Color.Gray);
        }

    }

    class MenuSelector : Pixelclass
    {
        
        private Vector2 m_pos;
      
        public MenuSelector ()
        {
            m_pos = new Vector2(Game1.graphics.PreferredBackBufferWidth / 2, Game1.graphics.PreferredBackBufferHeight / 2);
        }
        public void UpdateMe(GameTime gt, Vector2 Target)
        {
            m_pos = Vector2.Lerp(m_pos, Target, (float)gt.ElapsedGameTime.TotalSeconds * 10);
        }
        public void Drawme(SpriteBatch sb)
        {
            sb.Draw(Pixel, m_pos, null, Color.Black, (float)Math.PI / 4, new Vector2(0.5f, 0.5f),10, SpriteEffects.None, 0);
        }

    }
}
