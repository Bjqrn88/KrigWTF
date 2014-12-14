using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krig.Game;
using System.Threading;

namespace Krig.Game
{
    public class SaveLoad
    {
        private SaveGameData data;
        public SaveLoad(SaveGameData data) 
        {
            this.data = data;
        }

        public void saveGame()
        {
            Thread thread = new Thread(new ThreadStart(saveToXML));
            thread.Start();
        }

        private void saveToXML()
        {
            
            System.Xml.Serialization.XmlSerializer writer =
           new System.Xml.Serialization.XmlSerializer(typeof(SaveGameData));

            System.IO.StreamWriter file = new System.IO.StreamWriter(
                @"c:\Krig.xml");
            writer.Serialize(file, data);
            file.Close();
        }

        public SaveGameData loadFromXML(String path)
        {
            System.Xml.Serialization.XmlSerializer reader =
        new System.Xml.Serialization.XmlSerializer(typeof(SaveGameData));
            System.IO.StreamReader file = new System.IO.StreamReader(
                path);
            SaveGameData loadedData = new SaveGameData();
            loadedData = (SaveGameData)reader.Deserialize(file);

            Console.WriteLine(loadedData.NumberOfTurns);
            return loadedData;
        }
    }
}
