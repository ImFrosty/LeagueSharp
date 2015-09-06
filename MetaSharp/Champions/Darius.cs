using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color2 = System.Drawing.Color;

namespace MetaSharp
{
    class Darius
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Menu;

        public Darius()
        {
            Load();
        }

        private void Load()
        {

        }
    }
}