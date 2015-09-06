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
                        new Amumu();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Brand":
                        new Brand();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Corki":
                        new Corki();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Darius":
                        new Darius();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Evelynn":
                        new Evelynn();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Galio":
                        new Galio();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Garen":
                        new Garen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Heimerdinger":
                        new Heimerdinger();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Irelia":
                        new Irelia();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Kayle":
                        new Kayle();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Kennen":
                        new Kennen();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "KogMaw":
                        new KogMaw();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Nautilus":
                        new Nautilus();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Olaf":
                        new Olaf();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Shyvana":
                        new Shyvana();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Skarner":
                        new Skarner();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Talon":
                        new Talon();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Tristana":
                        new Tristana();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Twitch":
                        new Twitch();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "XinZhao":
                        new XinZhao();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Yorick":
                        new Yorick();
                        Notifications.AddNotification(say + cs, 5000).SetTextColor(Color2.LawnGreen);
                        break;

                    case "Zilean":
                        new Zilean();
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
