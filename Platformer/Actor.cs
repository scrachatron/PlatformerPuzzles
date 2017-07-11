using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Platformer
{
    class Actor : Pixelclass
    {
        public Rectangle Rect
        {
            get { return m_rect; }
            set
            {
                m_rect = value;
                m_pos.X = value.X;
                m_pos.Y = value.Y;
            }

        }
        public Vector2 Position
        {
            get { return m_pos; }
            set
            {
                m_pos = value;
                m_rect.X = (int)value.X;
                m_rect.Y = (int)value.Y;
            }
        }

        private Color m_tint;
        private Rectangle m_rect;
        private Vector2 m_pos;
        protected Vector2 m_vel;
        private int Currlayer = 0;
        protected Point IAM = new Point(0, 0);
        protected bool m_Airborne;


        public Actor(Rectangle rect, Color tint, int layer)
        {
            Rect = rect;
            m_vel = Vector2.Zero;
            m_tint = tint;
            Currlayer = layer;
        }
        public virtual void UpdateMe(GameTime gt, Level level)
        {
            Position += m_vel;
            if (m_vel.Y > 0)
                m_Airborne = true;
            m_vel += level.Gravity * (float)gt.ElapsedGameTime.TotalSeconds;
            IAM.X = (int)((m_pos.X + (m_rect.Width/2)) / level.TileSize);
            IAM.Y = (int)((m_pos.Y + (m_rect.Height/4)) / level.TileSize);

            Collision(level.Map[Currlayer],level.TileSize);

        }
        public virtual void DrawMe(SpriteBatch sb)
        {
            sb.Draw(Pixel, m_rect, m_tint);
            for (int x = IAM.X -1; x < IAM.X +2; x++)
                for (int y = IAM.Y - 1; y < IAM.Y + 3; y++)
                {
                    sb.Draw(Pixel, new Rectangle(x * 32, y * 32, 32, 32), Color.Blue * 0.1f);
                }
            sb.Draw(Pixel, new Rectangle((int)m_pos.X + (m_rect.Width / 2), (int)m_pos.Y + (m_rect.Height/4) , 1, 16), Color.Black);
            sb.Draw(Pixel, new Rectangle(IAM.X * 32, IAM.Y * 32, 32, 32), Color.Purple * 0.5f);
        }
        private void Collision(int[,] col,int tilesize)
        {
            for (int x = IAM.X -1; x < IAM.X +2; x++)
                for (int y = IAM.Y - 1; y < IAM.Y + 3; y++)
                {
                    if ((y > -1 && y < col.GetLength(0)) && (x > -1 && x < col.GetLength(1)))
                    if (col[y, x] == 1)
                    {
                        Rectangle newrect = new Rectangle(x*tilesize,y*tilesize,tilesize,tilesize);

                        if (m_rect.TouchTopOf(newrect))
                        {
                            m_Airborne = false;
                            if (m_vel.Y > 0)
                                m_vel.Y = 0;
                            m_pos.Y = newrect.Top - m_rect.Height;
                            m_rect.Y = (int)m_pos.Y;
                        }
                        if (m_rect.TouchLeftOf(newrect))
                        {
                            m_pos.X = newrect.X - m_rect.Width;
                            m_vel.X = 0;
                            m_rect.X = (int)m_pos.X;
                        }
                        else if (m_rect.TouchRightOf(newrect))
                        {
                            m_pos.X = newrect.X + newrect.Width;
                            m_vel.X = 0;
                            m_rect.X = (int)m_pos.X;
                        }

                        if (m_rect.TouchBottomOf(newrect))
                        {
                            if (m_vel.Y < 0)
                                m_vel.Y = -m_vel.Y;
                            m_pos.Y = newrect.Bottom;
                            m_rect.Y = (int)m_pos.Y;
                        }
                        
                    }
                }
        }

    }

    class Player : Actor
    {
        public Point IAMHERE
        {
            get { return IAM; }
        }

        public Player()
            :base(new Rectangle(64,128,24,58),Color.Red, 0)
        {

        }
        public bool SetStartPosition(Level lvl)
        {
            for (int i = 0; i < lvl.Map.Count; i++)
            {
                for (int x = 0; x < lvl.Map[i].GetLength(0); x++)
                    for (int y = 0; y < lvl.Map[i].GetLength(1); y++)
                        if (lvl.Map[i][y,x] == 2)
                        {
                            Position = new Vector2(x*lvl.TileSize + (lvl.TileSize/2) - (this.Rect.Width / 2), y* lvl.TileSize);
                            m_vel = Vector2.Zero;
                            return true;
                        }
            }
            return false;
        }
        public void UpdateMe(GameTime gt, Level level, InputManager input)
        {
            #region Gamepad controlls
            if (input.CurrPadState.ThumbSticks.Left.X < -0.1)
                m_vel.X = ((float)gt.ElapsedGameTime.TotalSeconds * 200) * (float)input.CurrPadState.ThumbSticks.Left.X;
            else if (input.CurrPadState.ThumbSticks.Left.X > 0.1)
                m_vel.X = ((float)gt.ElapsedGameTime.TotalSeconds * 200) * (float)input.CurrPadState.ThumbSticks.Left.X;
            else
                m_vel.X = 0;

            if (input.WasPressedFront(Buttons.A) && !m_Airborne)
            {
                m_vel.Y = - 8.5f;
                m_Airborne = true;
            }
            #endregion

            #region Keyboard controlls 
            if (input.IsDown(Keys.Left))
                m_vel.X = -((float)gt.ElapsedGameTime.TotalSeconds * 200);
            else if (input.IsDown(Keys.Right))
                m_vel.X = ((float)gt.ElapsedGameTime.TotalSeconds * 200);
            else
                m_vel.X = 0;

            if (input.WasPressedFront(Keys.Space) && !m_Airborne)
            {
                m_vel.Y = -8.5f;
                m_Airborne = true;
            }
            #endregion


            base.UpdateMe(gt,level);
        }
        public override void DrawMe(SpriteBatch sb)
        {
            base.DrawMe(sb);
        }
    }
}
