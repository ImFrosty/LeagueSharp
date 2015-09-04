using System;
using System.Linq;
using System.Text;
using LeagueSharp;
using LeagueSharp.Common;
using SharpDX;
using Color = SharpDX.Color;
using Color2 = System.Drawing.Color;

namespace MetaSharp
{
    class Program
    {
        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += OnGameLoad;
        }

        static void OnGameLoad(EventArgs args)
        {
            try
            {
                var cs = ObjectManager.Player.ChampionName;
                var say = ("Meta# Loaded : ");
                var def = ("Meta# Doesn't Support : ");
                switch (cs)
                {
                    case "Heimerdinger":
                        new Heimerdinger();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Nautilus":
                        new Nautilus();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Skarner":
                        new Skarner();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Garen":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    default:
                        Notifications.AddNotification(say + def, 5000).SetTextColor(Color2.Crimson);
                        break;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
