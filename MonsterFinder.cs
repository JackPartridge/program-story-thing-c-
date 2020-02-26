using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Text_based_game_oofer
{
    public class MonsterFinder
    {
        public static List<Mobs> monsters = new List<Mobs>();
        public static void Main(string[] args)
        {
            LoadJson();
            ItemFinder.Main1(null);
        }
        public static void LoadJson()
        {
            using (StreamReader r = new StreamReader("monsters.json"))
            {
                string json = r.ReadToEnd();
                monsters = JsonConvert.DeserializeObject<List<Mobs>>(json);
            }
        }

    }
}
