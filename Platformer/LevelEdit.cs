using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Platformer
{
    enum EditState
    {
        Editing,
        Metadata,
        Testing,
        Saving
    }

    class LevelEdit
    {
        
        EditState m_state;
        EditState m_laststate;
        Player m_Player;
        MapEdit m_edit;
        List<Level> m_levels;
        LevelEditMenu m_menu;
        bool m_editMenuOpen;

        public LevelEdit()
        {
            m_Player = new Player();
            m_state = EditState.Editing;
            m_laststate = EditState.Testing;
            m_levels = new List<Level>();
            m_edit = new MapEdit();
            m_editMenuOpen = false;
            m_menu = new LevelEditMenu();
        }
        public void UpdateMe(GameTime gt, InputManager input, ref GameState gs )
        {
            if (input.WasPressedBack(Keys.Escape))
            {
                m_editMenuOpen = !m_editMenuOpen;
            }

            #region menu opening code
            if (m_editMenuOpen == true)
                m_menu.UpdateMe(gt, input, ref gs, ref m_state, ref m_editMenuOpen);
            #endregion

            #region edge detection managment for switching between modes
            if (m_laststate != m_state)
            {
                m_editMenuOpen = false;
                if (m_laststate == EditState.Editing)
                {

                    m_edit.ForceSave();
                    m_levels.Clear();
                    m_levels.Add(SaveDirector.LoadData("mylvl"));
                    m_Player.SetStartPosition(m_levels[0]);
                }
                else if (m_laststate == EditState.Testing)
                {
                    m_edit.ForceLoad();
                }
            }
            #endregion

            m_laststate = m_state;

            #region Actual Updating of features
            if (m_editMenuOpen == false)
            {
                if (m_state == EditState.Testing)
                {
                    m_Player.UpdateMe(gt, m_levels[0], input);
                    m_levels[0].UpdateMe(gt, m_Player, input);
                }
                else if (m_state == EditState.Editing)
                    m_edit.UpdateMe(gt, input);
            }
            #endregion

        }
        public void DrawMe(SpriteBatch sb)
        {
            
            if (m_state == EditState.Testing)
            {
                m_levels[0].DrawMe(sb);
                m_Player.DrawMe(sb);
            }
            else if (m_state == EditState.Editing)
            {
                m_edit.DrawMe(sb);
            }

            if (m_editMenuOpen == true)
                m_menu.Drawme(sb);
        }


    }

    class LevelEditMenu : Menu
    {

        public LevelEditMenu() : base(Vector2.Zero)
        {
            base.Options.Add(new EditMenuOption("Play Test", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), EditState.Testing));
            base.Options.Add(new EditMenuOption("Edit Metadata", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), EditState.Metadata));
            base.Options.Add(new EditMenuOption("Save", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), EditState.Saving));
            base.Options.Add(new MenuOption("MainMenu", new Vector2(base.m_startingLocation.X, base.m_startingLocation.Y + (Font.MeasureString("Level").Y + 5) * Options.Count), GameState.MainMenu));
        }
        public virtual void UpdateMe(GameTime gt, InputManager input, ref GameState globalgs, ref EditState globabes, ref bool open)
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
                if (m_selecton == 3)
                    globalgs = m_options[m_selecton].ChangeMeTo;
                else
                {
                    EditMenuOption temp;
                    temp = (EditMenuOption)m_options[m_selecton];
                    globabes = temp.ChangeEditState;
                }
            }

        }
        public override void Drawme(SpriteBatch sb)
        {
            sb.Draw(Pixel, new Rectangle((int)m_startingLocation.X, (int)m_startingLocation.Y, 300, 300), Color.White);
            base.Drawme(sb);
        }

    }
}
