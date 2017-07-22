using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Xml.Serialization;
using System.IO;

namespace Platformer
{
    [Serializable]
    public struct LevelData
    {
        public List<int[]> SolvedLayer;
        public Point sizeXbyY;
        public string songtitle;

        public LevelData(List<int[,]> numbers)
        {
            sizeXbyY = new Point(numbers[0].GetLength(0), numbers[0].GetLength(1));
            SolvedLayer = new List<int[]>();
            songtitle = "Track1";

            for (int i = 0; i < numbers.Count; i++)
            {
                SolvedLayer.Add(new int[numbers[i].GetLength(0) * numbers[i].GetLength(1)]);
                System.Buffer.BlockCopy(numbers[i], 0, SolvedLayer[i], 0, numbers[i].GetLength(0) * numbers[i].GetLength(1));
            }
        }
        public LevelData(int[,] numbers)
        {
            sizeXbyY = new Point(numbers.GetLength(0), numbers.GetLength(1));
            SolvedLayer = new List<int[]>();
            songtitle = "Track1";

                SolvedLayer.Add(new int[numbers.GetLength(0) * numbers.GetLength(1)]);
                System.Buffer.BlockCopy(numbers, 0, SolvedLayer[0], 0, numbers.GetLength(0) * numbers.GetLength(1));
            
        }
    }

    static class SaveDirector
    {

        private static bool usedSave;
        private static LevelData returnData;

        public static void SaveData(Level data, string name)
        {
            
            if (!File.Exists(name))
            {
                new FileInfo(name).Directory.Create();
            }
            else
                File.Delete(name);

            // Open the file, creating it if necessary
            FileStream stream = File.Open(name, FileMode.OpenOrCreate);
            try
            {
                XmlSerializerNamespaces ns = new XmlSerializerNamespaces();

                //Add an empty namespace and empty value
                ns.Add("", "");

                // Convert the object to XML data and put it in the stream
                XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                serializer.Serialize(stream, PhraseData(data), ns);

                usedSave = false;
            }
            finally
            {
                // Close the file
                stream.Close();
            }
        }

        public static Level LoadData(string name)
        {
            if (usedSave)
                return PhraseData(returnData);
            else
            {
                LevelData data;

                // Open the file
                FileStream stream = File.Open(name, FileMode.OpenOrCreate, FileAccess.Read);
                try
                {
                    // Read the data from the file
                    XmlSerializer serializer = new XmlSerializer(typeof(LevelData));
                    try
                    {
                        if (stream.Length > 0)
                        {
                            data = (LevelData)serializer.Deserialize(stream);
                        }
                        else
                        {
                            Level temperarylevel = new Level(null, new Point(100, 100));
                            temperarylevel.Map.Add(new int[100, 100]);

                            data = PhraseData(temperarylevel);
                        }
                    }
                    catch
                    {

                        Level temperarylevel = new Level(null, new Point(100, 100));
                        temperarylevel.Map.Add(new int[100, 100]);

                        data = PhraseData(temperarylevel);
                    }
                    finally
                    {



                    }
                }
                finally
                {
                    // Close the file
                    stream.Close();
                }

                returnData = data;

                usedSave = true;

                return PhraseData(data);
            }
        }

        public static Level PhraseData(LevelData data)
        {
            return new Level(data);
        }
        public static LevelData PhraseData(Level data)
        {
            return new LevelData(data.Map);
        }
    }

    class MapEdit : Pixelclass
    {
        private Level levelCreate;
        private bool m_viewHelpScreen;

        public MapEdit()
        {
            m_viewHelpScreen = true;
            levelCreate = new Level(null, new Point(100, 100));
            levelCreate.Map.Add(new int[100, 100]);
        }
        //public bool CreateLayer(int[,] ColMap)
        //{
        //    if (new Point(ColMap.GetLength(1), ColMap.GetLength(0)) == m_LayerSize)
        //    {
        //        m_SimpleLayer.Add(ColMap);
        //        return true;
        //    }
        //    else
        //        throw new SystemException("The current map does not have the same dimentions it excpects");
        //    //return false;
        //}
        //public void AddLayer()
        //{
        //    m_SimpleLayer.Add(new int[m_LayerSize.X, m_LayerSize.Y]);
        //}

        public void AddLayer(int[,] t)
        {
            int[,] temp = new int[t.GetLength(0),t.GetLength(1)];
            for (int x = 0; x < t.GetLength(0); x++)
                for (int y = 0; y < t.GetLength(1); y++)
                    temp[x, y] = t[x, y];
        
            levelCreate.Map.Add(temp);
        }

        public void UpdateMe(GameTime gt, InputManager Input)
        {
            if (Input.WasPressedFront(Keys.Add))
            {
                if (levelCreate.Layer + 1 >= levelCreate.Map.Count)
                    AddLayer(levelCreate.Map[levelCreate.Map.Count - 1]);
                levelCreate.Layer++;
            }
            else if (Input.WasPressedFront(Keys.Subtract) && levelCreate.Layer != 0)
            {
                levelCreate.Layer--;
            }
            else if (Input.WasPressedBack(Keys.F2))
            {
                SaveDirector.SaveData(levelCreate, "mylvl");
            }
            else if (Input.WasPressedBack(Keys.F1))
            {
                levelCreate = new Level(SaveDirector.PhraseData(SaveDirector.LoadData("mylvl")));
            }
            else if (Input.WasPressedBack(Keys.H))
            {
                m_viewHelpScreen = !m_viewHelpScreen;
            }


            for (int x = 0; x < levelCreate.Map[levelCreate.Layer].GetLength(1); x++)
                for (int y = 0; y < levelCreate.Map[levelCreate.Layer].GetLength(0); y++)
                {
                    if (Input.ThisMouse.LeftButton == ButtonState.Pressed && new Rectangle(x * levelCreate.TileSize, y * levelCreate.TileSize, levelCreate.TileSize, levelCreate.TileSize).Contains(Input.ThisMouse.Position))
                    {
                        levelCreate.Map[levelCreate.Layer][y, x] = 1;
                    }
                    else if (Input.ThisMouse.RightButton == ButtonState.Pressed && new Rectangle(x * levelCreate.TileSize, y * levelCreate.TileSize, levelCreate.TileSize, levelCreate.TileSize).Contains(Input.ThisMouse.Position))
                    {
                        levelCreate.Map[levelCreate.Layer][y, x] = 0;
                    }
                    else if (Input.WasPressedBack(Keys.A) && new Rectangle(x * levelCreate.TileSize, y * levelCreate.TileSize, levelCreate.TileSize, levelCreate.TileSize).Contains(Input.ThisMouse.Position))
                    {
                        if (levelCreate.Map[levelCreate.Layer][y, x] == 0 && levelCreate.Map[levelCreate.Layer][y + 1, x] == 0)
                        {
                            for (int x2 = 0; x2 < levelCreate.Map[levelCreate.Layer].GetLength(1); x2++)
                                for (int y2 = 0; y2 < levelCreate.Map[levelCreate.Layer].GetLength(0); y2++)
                                    if (levelCreate.Map[levelCreate.Layer][y2, x2] == 2)
                                        levelCreate.Map[levelCreate.Layer][y2, x2] = 0;

                            levelCreate.Map[levelCreate.Layer][y, x] = 2;
                        }
                    }
                    else if (Input.WasPressedBack(Keys.D) && new Rectangle(x * levelCreate.TileSize, y * levelCreate.TileSize, levelCreate.TileSize, levelCreate.TileSize).Contains(Input.ThisMouse.Position))
                    {
                        if (levelCreate.Map[levelCreate.Layer][y, x] == 0 && levelCreate.Map[levelCreate.Layer][y + 1, x] == 0)
                        {
                            for (int x2 = 0; x2 < levelCreate.Map[levelCreate.Layer].GetLength(1); x2++)
                                for (int y2 = 0; y2 < levelCreate.Map[levelCreate.Layer].GetLength(0); y2++)
                                    if (levelCreate.Map[levelCreate.Layer][y2, x2] == 3)
                                        levelCreate.Map[levelCreate.Layer][y2, x2] = 0;

                            levelCreate.Map[levelCreate.Layer][y, x] = 3;
                        }
                    }
                }



            levelCreate.UpdateMe(gt);

        }

        public void DrawMe(SpriteBatch sb)
        {
            
            
            levelCreate.DrawMe(sb);

            //for (int x = 0; x < m_SimpleLayer[CurrLayer].GetLength(1); x++)
            //    for (int y = 0; y < m_SimpleLayer[CurrLayer].GetLength(0); y++)
            //    {
            //        if (m_SimpleLayer[CurrLayer][y, x] > 0)
            //            sb.Draw(Pixel, new Rectangle(x *levelCreate.TileSize, y *levelCreate.TileSize,levelCreate.TileSize,levelCreate.TileSize), Color.Black);
            //    }

            if (m_viewHelpScreen)
            {
                sb.Draw(Pixel,
                    new Rectangle(Game1.graphics.PreferredBackBufferWidth / 8, Game1.graphics.PreferredBackBufferHeight / 8,
                    (Game1.graphics.PreferredBackBufferWidth / 8) * 6, (Game1.graphics.PreferredBackBufferHeight / 8) * 6),
                    Color.Gray);

                sb.DrawString(Font, "Current Layer: " + (levelCreate.Layer + 1) + "\nTile size: " + levelCreate.TileSize + "\nTotal Layers: " + levelCreate.Map.Count + "\nF1 To load Level \nF2 to save level \nYou must save before switching back to playtesting \nEnter to switch to playtesting" + 
                    "\n Press A to create the start pos \n Press D to create the end \nNote you do not need to have a start or end for the level to load", 
                    new Vector2(Game1.graphics.PreferredBackBufferWidth / 8, Game1.graphics.PreferredBackBufferHeight / 8), Color.Blue);

            }

            
        }

        public void ForceLoad()
        {
            levelCreate = new Level(SaveDirector.PhraseData(SaveDirector.LoadData("mylvl")));
        }
        public void ForceSave()
        {
            SaveDirector.SaveData(levelCreate,"mylvl");
        }

    }

}
