using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;

namespace Platformer
{
    class Point3
    {
        public Point To2D
        {
            get
            {
                return new Point(X, Y);
            }
        }
        public int X
        {
            get; set;
        }
        public int Y
        {
            get; set;
        }
        public int Z
        {
            get;set;
        }



        public Point3(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }
        
    }

    class Level : Pixelclass
    {
        private bool Compleate = false;
        public List<int[,]> Map = new List<int[,]>();
        public int Layer { get { return CurrLayer; } set { CurrLayer = value; } }

        private Point3 m_StartPos;
        private Point3 m_WinPos;

        private Point m_LayerSize;
        public int TileSize = 32;
        private int CurrLayer = 0;
        private Song m_theme;
        public Vector2 Gravity { get; set; }

        public Level(Song theme,Point dimentions)
        {
            m_LayerSize = dimentions;
            Gravity = new Vector2(0, 100);
        }
        public Level(LevelData lvd)
        {
            m_LayerSize = lvd.sizeXbyY;
            for (int i = 0; i < lvd.SolvedLayer.Count(); i++)
            {
                Map.Add(new int[m_LayerSize.X, m_LayerSize.Y]);
                for (int x = 0; x < lvd.sizeXbyY.X; x++)
                {
                    for (int y = 0; y < lvd.sizeXbyY.Y; y++)
                    {
                        Map[i][x, y] = lvd.SolvedLayer[i][y + (x * m_LayerSize.X)];

                        if (lvd.SolvedLayer[i][y + (x * m_LayerSize.X)] == 2)
                            m_StartPos = new Point3(y, x, i);
                        else if (lvd.SolvedLayer[i][y + (x * m_LayerSize.X)] == 3)
                            m_WinPos = new Point3(y, x, i);

                    }
                }
            }
            Gravity = new Vector2(0, 30);
            if (lvd.songtitle != null)
            {
                //m_theme = Content.Load<Song>("Music//" + lvd.songtitle);
            }

        }

        public bool CreateLayer(int[,] ColMap)
        {
            if (new Point(ColMap.GetLength(1), ColMap.GetLength(0)) == m_LayerSize)
            {
                Map.Add(ColMap);
                return true;
            }
            else
                throw new SystemException("The current map does not have the same dimentions it excpects");
                //return false;
        }

        public void UpdateMe(GameTime gt, Player p1, InputManager Input)
        {
            if (p1.IAMHERE == m_WinPos.To2D && CurrLayer == m_WinPos.Z)
                Compleate = true;



            //if (m_theme != null)
            //    if (MediaPlayer.State == MediaState.Stopped)
            //    {
            //        MediaPlayer.Play(m_theme);
            //        MediaPlayer.IsRepeating = true;
            //        MediaPlayer.Volume = 1f;
            //    }
        }
        public void UpdateMe(GameTime gt)
        {
            //if (m_theme != null)
            //    if (MediaPlayer.State == MediaState.Stopped)
            //    {
            //        MediaPlayer.Play(m_theme);
            //        MediaPlayer.IsRepeating = true;
            //        MediaPlayer.Volume = 1f;
            //    }
        }
        public void DrawMe(SpriteBatch sb)
        {
            if (!Compleate)
            {
                for (int x = 0; x < Map[CurrLayer].GetLength(1); x++)
                    for (int y = 0; y < Map[CurrLayer].GetLength(0); y++)
                    {
                        if (Map[CurrLayer][y, x] == 1)
                            sb.Draw(Pixel, new Rectangle(x * TileSize + 1 , y * TileSize + 1, TileSize - 2 , TileSize - 2 ), Color.Black);
                        else if (Map[CurrLayer][y, x] == 2)
                            sb.Draw(Pixel, new Rectangle((x * TileSize) + 2, y * TileSize + 2, TileSize - 4, (TileSize * 2) - 2), Color.Green);
                        else if (Map[CurrLayer][y, x] == 3)
                            sb.Draw(Pixel, new Rectangle((x * TileSize) + 2, y * TileSize + 2, TileSize - 4, (TileSize * 2) - 2), Color.Red);
                    }
            }
        }


    }
}
