using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color2 = System.Drawing.Color;

namespace MetaSharp
{
    class Nautilus
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Menu;

        public Nautilus()
        {
            Load();
        }

        private void Load()
        {
            Q = new Spell(SpellSlot.Q, 1100);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 600);
            R = new Spell(SpellSlot.R, 825);

            Q.SetSkillshot(250, 90, 2000, true, SkillshotType.SkillshotLine);

            Menu = new Menu("Meta#", "Meta#", true);

            Menu orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);

            Menu ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(ts);

            Menu comboMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            comboMenu.AddItem(new MenuItem("comboQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboW", "Use W").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboE", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboR", "Use R").SetValue(true));
            comboMenu.AddItem(new MenuItem("Combo", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            Menu drawMenu = Menu.AddSubMenu(new Menu("Drawings", "Drawings"));
            drawMenu.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            drawMenu.AddItem(new MenuItem("drawE", "Draw E").SetValue(true));
            drawMenu.AddItem(new MenuItem("drawR", "Draw R").SetValue(true));

            Menu.AddToMainMenu();

            Game.OnUpdate += Game_OnUpdate;
            Drawing.OnDraw += Drawing_OnDraw;
        }

        private static void Game_OnUpdate(EventArgs args)
        {
            if (Player.IsDead)
            {
                return;
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                combo();
            }
        }

        private static void combo()
        {
            var comboQ = (Menu.Item("comboQ").GetValue<bool>());
            var comboW = (Menu.Item("comboW").GetValue<bool>());
            var comboE = (Menu.Item("comboE").GetValue<bool>());
            var comboR = (Menu.Item("comboR").GetValue<bool>());

            if (comboR && R.IsReady())
            {
                var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(R.Range))
                    R.Cast(target);
            }

            if (comboQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(Q.Range))
                    Q.CastIfHitchanceEquals(target, HitChance.High);
            }

            if (comboE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(E.Range))
                    E.Cast();
            }

            if (comboW && W.IsReady() && Player.CountEnemiesInRange(600) > 0)
            {
                W.Cast();
            }

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            {
                if (Player.IsDead)
                    return;

                if (Menu.Item("drawQ").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Player.Position, Q.Range, Color2.Goldenrod);
                }

                if (Menu.Item("drawE").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Player.Position, E.Range, Color2.Goldenrod);
                }

                if (Menu.Item("drawR").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Player.Position, R.Range, Color2.Goldenrod);
                }
            }
        }

    }
}