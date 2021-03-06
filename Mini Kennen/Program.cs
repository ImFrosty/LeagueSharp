﻿using System;
using LeagueSharp;
using LeagueSharp.Common;
using Color2 = System.Drawing.Color;

namespace Mini_Kennen
{
    class Program
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Menu;


        static void Main(string[] args)
        {
            CustomEvents.Game.OnGameLoad += Game_OnGameLoad;
        }

        private static void Game_OnGameLoad(EventArgs args)
        {
            if (Player.ChampionName != "Kennen")
            {
                return;
            }

            Q = new Spell(SpellSlot.Q, 1050);
            W = new Spell(SpellSlot.W, 800);
            E = new Spell(SpellSlot.E);
            R = new Spell(SpellSlot.R, 550);

            Q.SetSkillshot(0.125f, 50, 1700, true, SkillshotType.SkillshotLine);

            Menu = new Menu("Mini Kennen", "Mini Kennen", true);

            Menu orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);

            Menu ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(ts);

            Menu comboMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            comboMenu.AddItem(new MenuItem("comboQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboW", "Use W").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboE", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboR", "Use R").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboEx", "Use E if X Enemies").SetValue(new Slider(2, 1, 5)));
            comboMenu.AddItem(new MenuItem("comboRx", "Use R if X Enemies").SetValue(new Slider(2, 1, 5)));
            comboMenu.AddItem(new MenuItem("Combo", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            Menu harassMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            harassMenu.AddItem(new MenuItem("harassQ", "Use Q").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassW", "Use W").SetValue(true));
            harassMenu.AddItem(new MenuItem("Harass", "Harass").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Menu ksMenu = Menu.AddSubMenu(new Menu("KS", "KS"));
            ksMenu.AddItem(new MenuItem("ks", "KS").SetValue(true));

            Menu drawMenu = Menu.AddSubMenu(new Menu("Drawings", "Drawings"));
            drawMenu.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            drawMenu.AddItem(new MenuItem("drawW", "Draw W").SetValue(false));
            drawMenu.AddItem(new MenuItem("drawR", "Draw R").SetValue(false));

            Menu.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
            Notifications.AddNotification("Mini Kennen!", 10000);
            Game.PrintChat("<font color=\"#00FFFF\">Mini Kennen by Frosty</font> - <font color=\"#00FFFF\">Loaded!</font>");
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                combo();
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                harass();
            }

            if (Menu.Item("ks").GetValue<bool>())
            {
                ks();
            }
        }

        private static void combo()
        {
            var comboQ = (Menu.Item("comboQ").GetValue<bool>());
            var comboW = (Menu.Item("comboW").GetValue<bool>());
            var comboE = (Menu.Item("comboE").GetValue<bool>());
            var comboR = (Menu.Item("comboR").GetValue<bool>());
            var x = (Menu.Item("comboRx").GetValue<Slider>().Value);
            var y = (Menu.Item("comboEx").GetValue<Slider>().Value);

            if (comboQ && Q.IsReady() && !Player.HasBuff("KennenE"))
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(Q.Range))
                {
                    Q.CastIfHitchanceEquals(target, HitChance.High);
                }
            }

            if (comboW && W.IsReady() && !Player.HasBuff("KennenE"))
            {
                var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                if (target.HasBuff("kennenmarkofstorm") && target.IsValidTarget(W.Range))
                {
                    W.Cast();
                }
            }

            if (comboE && E.IsReady() && !Player.HasBuff("KennenE") && Player.CountEnemiesInRange(900) >= y)
            {
                E.Cast();
            }

            if (comboR && R.IsReady() && Player.CountEnemiesInRange(550) >= x)
            {
                R.Cast();
            }
        }

        private static void harass()
        {
            var harassQ = (Menu.Item("harassQ").GetValue<bool>());
            var harassW = (Menu.Item("harassW").GetValue<bool>());
            var harassE = (Menu.Item("harassE").GetValue<bool>());
            var harassR = (Menu.Item("harassR").GetValue<bool>());

            if (harassQ && Q.IsReady())
            {
                var targetQ = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (targetQ.IsValidTarget(Q.Range))
                {
                    Q.CastIfHitchanceEquals(targetQ, HitChance.VeryHigh);
                }

                if (harassW && W.IsReady())
                {
                    var targetW = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                    if (targetW.HasBuff("kennenmarkofstorm") && targetW.IsValidTarget(W.Range))
                    {
                        W.Cast();
                    }
                }
            }
        }

        private static void ks()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);

            if (Menu.Item("ks").GetValue<bool>() && Q.IsReady() && target.IsValidTarget(Q.Range) && Q.IsKillable(target))
            {
                Q.CastIfHitchanceEquals(target, HitChance.Medium);
            }

            if (Menu.Item("ks").GetValue<bool>() && target.HasBuff("kennenmarkofstorm") && target.IsValidTarget(W.Range) && W.IsReady() && W.IsKillable(target))
            {
                W.Cast();
            }

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Menu.Item("drawQ").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, Q.Range, Color2.Cyan);
            }

            if (Menu.Item("drawW").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, Color2.Cyan);
            }

            if (Menu.Item("drawR").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, R.Range, Color2.Cyan);
            }
        }
    }
}