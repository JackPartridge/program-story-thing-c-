using System;
using System.Collections.Generic;
using System.Text;

namespace Text_based_game_oofer
{
    public class Items
    {
        public int itemID;
        public string itemName;
        public string itemDesc;
        public int swordIncrease;
        public int rangedIncrease;
        public int magicIncrease;

        public static implicit operator int(Items v)
        {
            throw new NotImplementedException();
        }
    }
}
