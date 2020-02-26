using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Text_based_game_oofer
{
    public class ItemFinder
{
    public static List<Items> itemList = new List<Items>();
    public static void Main1(string[] args)
    {
        LoadJson1();
        MainSpace.Main2(null);
    }
    public static void LoadJson1()
    {
        using (StreamReader r = new StreamReader("itemsList.json"))
        {
            string json = r.ReadToEnd();
            itemList = JsonConvert.DeserializeObject<List<Items>>(json);
        }

    }
}
}
