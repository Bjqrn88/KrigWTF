﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Krig.Game;
using System.Threading;
using System.IO;

namespace Krig.Game
{
    public class SaveLoad
    {
        private SaveGameData data;
        private String path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
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
                Path.Combine(path, @"krig.xml"));
            Console.WriteLine(Path.Combine(path, @"krig.xml"));
            writer.Serialize(file, data);
            file.Close();
        }

        public SaveGameData loadFromXML(String path)
        {
            Console.WriteLine(path);
            System.Xml.Serialization.XmlSerializer reader =
        new System.Xml.Serialization.XmlSerializer(typeof(SaveGameData));
            System.IO.StreamReader file = new System.IO.StreamReader(@path);
            SaveGameData loadedData = new SaveGameData();
            loadedData = (SaveGameData)reader.Deserialize(file);

            Console.WriteLine(loadedData.NumberOfTurns);
            return loadedData;
        }
    }
}
