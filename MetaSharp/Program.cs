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
                    case "Amumu":
                        new Heimerdinger();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Brand":
                        new Nautilus();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Corki":
                        new Skarner();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Darius":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Evelynn":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Galio":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Garen":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Heimerdinger":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Irelia":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Kayle":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Kennen":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "KogMaw":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Nautilus":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Olaf":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Shyvana":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Skarner":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Talon":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Tristana":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Twitch":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "XinZhao":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Yorick":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Zilean":
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
