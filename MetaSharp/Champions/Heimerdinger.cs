using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color2 = System.Drawing.Color;

namespace MetaSharp
{
    class Heimerdinger
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, W1, E, E1, R;
        private static Menu Menu;

        public Heimerdinger()
        {
            Load();
        }

        private void Load()
        {
            Q = new Spell(SpellSlot.Q, 325);
            W = new Spell(SpellSlot.W, 1100);
            E = new Spell(SpellSlot.E, 925);
            R = new Spell(SpellSlot.R, 100);

            W1 = new Spell(SpellSlot.W, 1100);
            E1 = new Spell(SpellSlot.E, 2155);

            Q.SetSkillshot(0.5f, 40f, 1100f, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(0.5f, 40f, 3000f, true, SkillshotType.SkillshotLine);
            E.SetSkillshot(0.5f, 120f, 1200f, false, SkillshotType.SkillshotCircle);

            W1.SetSkillshot(0.5f, 40f, 3000f, true, SkillshotType.SkillshotLine);
            E1.SetSkillshot(0.5f, 120f, 1200f, false, SkillshotType.SkillshotCircle);

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

            Menu ultMenu = Menu.AddSubMenu(new Menu("R Settings", "R Settings"));
            ultMenu.AddItem(new MenuItem("ultText", "Only Have 1 On!"));
            ultMenu.AddItem(new MenuItem("ultQ", "Use R + Q").SetValue(false));
            ultMenu.AddItem(new MenuItem("ultW", "Use R + W").SetValue(true));
            ultMenu.AddItem(new MenuItem("ultE", "Use R + E").SetValue(false));
            ultMenu.AddItem(new MenuItem("ultText2", "Only Have 1 On!"));

            Menu harassMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            harassMenu.AddItem(new MenuItem("harassQ", "Use Q").SetValue(false));
            harassMenu.AddItem(new MenuItem("harassW", "Use W").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassE", "Use E").SetValue(true));
            harassMenu.AddItem(new MenuItem("Harass", "Harass").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Menu ksMenu = Menu.AddSubMenu(new Menu("KS", "KS"));
            ksMenu.AddItem(new MenuItem("ksW", "Use W").SetValue(true));
            ksMenu.AddItem(new MenuItem("ksWR", "Use R + W").SetValue(false));
            ksMenu.AddItem(new MenuItem("ks", "KS").SetValue(true));

            Menu drawMenu = Menu.AddSubMenu(new Menu("Drawings", "Drawings"));
            drawMenu.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            drawMenu.AddItem(new MenuItem("drawW", "Draw W").SetValue(true));
            drawMenu.AddItem(new MenuItem("drawE", "Draw E").SetValue(true));
            drawMenu.AddItem(new MenuItem("drawE1", "Draw R + E").SetValue(true));

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
            if (Player.IsDead)
            {
                return;
            }

            var ultQ = (Menu.Item("ultQ").GetValue<bool>());
            var ultW = (Menu.Item("ultW").GetValue<bool>());
            var ultE = (Menu.Item("ultE").GetValue<bool>());
            var comboQ = (Menu.Item("comboQ").GetValue<bool>());
            var comboW = (Menu.Item("comboW").GetValue<bool>());
            var comboE = (Menu.Item("comboE").GetValue<bool>());
            var comboR = (Menu.Item("comboR").GetValue<bool>());

            if (comboR && R.IsReady() && ultQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                R.Cast();
                Q.Cast(Player.Position.Extend(target.Position, +200));
            }

            if (comboR && R.IsReady() && ultW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W1.Range, TargetSelector.DamageType.Magical);
                R.Cast();
                if (target.IsValidTarget(W1.Range))
                    W1.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

            if (comboR && R.IsReady() && ultE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E1.Range, TargetSelector.DamageType.Magical);
                R.Cast();
                if (target.IsValidTarget(E1.Range))
                    E1.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

            if (comboQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                Q.Cast(Player.Position.Extend(target.Position, +200));
            }

            if (comboE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(E.Range))
                    E.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

            if (comboW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(W.Range))
                    W.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

        }

        private static void harass()
        {
            if (Player.IsDead)
            {
                return;
            }

            var harassQ = (Menu.Item("harassQ").GetValue<bool>());
            var harassW = (Menu.Item("harassW").GetValue<bool>());
            var harassE = (Menu.Item("harassE").GetValue<bool>());

            if (harassQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                Q.Cast(Player.Position.Extend(target.Position, +200));
            }

            if (harassE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(E.Range))
                    E.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

            if (harassW && W.IsReady())
            {
                var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(W.Range))
                    W.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

        }

        private static void ks()
        {
            if (Player.IsDead)
            {
                return;
            }

            var target = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);

            if (Menu.Item("ksW").GetValue<bool>() && W.IsReady() && target.IsValidTarget(W.Range) && W.IsReady() && W.IsKillable(target))
            {
                W.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

            if (Menu.Item("ksWR").GetValue<bool>() && R.IsReady() && W.IsReady() && target.IsValidTarget(W1.Range) && W1.IsKillable(target))
            {
                R.Cast();
                W1.CastIfHitchanceEquals(target, HitChance.VeryHigh);
            }

        }

        private static void Drawing_OnDraw(EventArgs args)
        {
            {
                if (Player.IsDead)
                    return;

                if (Menu.Item("drawQ").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Player.Position, Q.Range, Color2.Aqua);
                }

                if (Menu.Item("drawW").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Player.Position, W.Range, Color2.Aqua);
                }

                if (Menu.Item("drawE").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Player.Position, E.Range, Color2.Aqua);
                }

                if (Menu.Item("drawE1").GetValue<bool>())
                {
                    Render.Circle.DrawCircle(Player.Position, 2155, Color2.Aqua);
                }
            }
        }

    }
}