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
            Q = new Spell(SpellSlot.Q, 400);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 540);
            R = new Spell(SpellSlot.R, 460);

            E.SetSkillshot(0.1f, 50f, float.MaxValue, false, SkillshotType.SkillshotLine);

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

            Menu harassMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            harassMenu.AddItem(new MenuItem("harassQ", "Use Q").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassW", "Use W").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassE", "Use E").SetValue(false));
            harassMenu.AddItem(new MenuItem("Harass", "Harass").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Menu ksMenu = Menu.AddSubMenu(new Menu("KS", "KS"));
            ksMenu.AddItem(new MenuItem("ks", "KS").SetValue(true));

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

            if (comboE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                E.CastIfHitchanceEquals(target, HitChance.High);
            }

            if (comboQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (target.IsValidTarget(Q.Range))
                {
                    Q.Cast();
                }
            }

            if (comboW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(175, TargetSelector.DamageType.Physical);
                if (target.IsValidTarget(175))
                {
                    W.Cast();
                }
            }

            if (comboR && R.IsReady())
            {
                var target = TargetSelector.GetTarget(175, TargetSelector.DamageType.True);
                if (R.IsKillable(target))
                {
                    R.Cast(target);
                }
            }
        }

        private static void harass()
        {
            var harassQ = (Menu.Item("harassQ").GetValue<bool>());
            var harassW = (Menu.Item("harassW").GetValue<bool>());
            var harassE = (Menu.Item("harassE").GetValue<bool>());

            if (harassE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                E.CastIfHitchanceEquals(target, HitChance.High);
            }

            if (harassQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Physical);
                if (target.IsValidTarget(Q.Range))
                {
                    Q.Cast();
                }
            }

            if (harassW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(175, TargetSelector.DamageType.Physical);
                if (target.IsValidTarget(175))
                {
                    W.Cast();
                }
            }
        }

        private static void ks()
        {
            if (Menu.Item("ks").GetValue<bool>() && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(Q.Range) && Q.IsKillable(target))
                {
                    Q.CastIfHitchanceEquals(target, HitChance.Medium);
                }
            }

            if (Menu.Item("ks").GetValue<bool>() && R.IsReady())
            {
                var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(R.Range) && R.IsKillable(target))
                {
                    R.Cast(target);
                }
            }

            if (Menu.Item("ks").GetValue<bool>() && W.IsReady())
            {
                var target = TargetSelector.GetTarget(175, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(W.Range) && W.IsKillable(target))
                {
                    W.Cast();
                }
            }
        }

        private static void Drawing_OnDraw(EventArgs args)
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