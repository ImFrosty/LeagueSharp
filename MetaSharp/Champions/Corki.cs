using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color2 = System.Drawing.Color;

namespace MetaSharp
{
    class Corki
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Menu;

        public Corki()
        {
            Load();
        }

        private void Load()
        {
            Q = new Spell(SpellSlot.Q, 825);
            W = new Spell(SpellSlot.W, 800);
            E = new Spell(SpellSlot.E, 600);
            R = new Spell(SpellSlot.R, 1225);

            Q.SetSkillshot(0.35f, 250f, 1000f, false, SkillshotType.SkillshotCircle);
            E.SetSkillshot(0f, (float)(45 * Math.PI / 180), 1500, false, SkillshotType.SkillshotCone);
            R.SetSkillshot(0.2f, 40f, 2000f, true, SkillshotType.SkillshotLine);

            Menu = new Menu("Meta#", "Meta#", true);

            Menu orbwalkerMenu = Menu.AddSubMenu(new Menu("Orbwalker", "Orbwalker"));
            Orbwalker = new Orbwalking.Orbwalker(orbwalkerMenu);

            Menu ts = Menu.AddSubMenu(new Menu("Target Selector", "Target Selector"));
            TargetSelector.AddToMenu(ts);

            Menu comboMenu = Menu.AddSubMenu(new Menu("Combo", "Combo"));
            comboMenu.AddItem(new MenuItem("comboQ", "Use Q").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboE", "Use E").SetValue(true));
            comboMenu.AddItem(new MenuItem("comboR", "Use R").SetValue(true));
            comboMenu.AddItem(new MenuItem("Combo", "Combo").SetValue(new KeyBind(32, KeyBindType.Press)));

            Menu harassMenu = Menu.AddSubMenu(new Menu("Harass", "Harass"));
            harassMenu.AddItem(new MenuItem("harassQ", "Use Q").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassE", "Use E").SetValue(false));
            harassMenu.AddItem(new MenuItem("harassR", "Use R").SetValue(true));
            harassMenu.AddItem(new MenuItem("Harass", "Harass").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Menu clearMenu = Menu.AddSubMenu(new Menu("Lane Clear", "Lane Clear"));
            clearMenu.AddItem(new MenuItem("clearQ", "Use Q").SetValue(true));
            clearMenu.AddItem(new MenuItem("clearE", "Use E").SetValue(true));
            clearMenu.AddItem(new MenuItem("clearR", "Use R").SetValue(true));
            clearMenu.AddItem(new MenuItem("Lane Clear", "Lane Clear").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

            Menu ksMenu = Menu.AddSubMenu(new Menu("KS", "KS"));
            ksMenu.AddItem(new MenuItem("ks", "KS").SetValue(true));

            Menu drawMenu = Menu.AddSubMenu(new Menu("Drawings", "Drawings"));
            drawMenu.AddItem(new MenuItem("drawQ", "Draw Q").SetValue(true));
            drawMenu.AddItem(new MenuItem("drawW", "Draw W").SetValue(true));
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

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Mixed)
            {
                harass();
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                clearQ();
                clearE();
                clearR();
            }

            if (Menu.Item("ks").GetValue<bool>())
            {
                ks();
            }
        }

        private static void combo()
        {
            var comboQ = (Menu.Item("comboQ").GetValue<bool>());
            var comboE = (Menu.Item("comboE").GetValue<bool>());
            var comboR = (Menu.Item("comboR").GetValue<bool>());


            if (comboR && R.IsReady())
            {
                var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(R.Range))
                {
                    R.CastIfHitchanceEquals(target, HitChance.High);
                }
            }

            if (comboQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(Q.Range))
                {
                    Q.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                }
            }

            if (comboE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(E.Range))
                {
                    E.CastIfHitchanceEquals(target, HitChance.Medium);
                }
            }

        }

        private static void harass()
        {
            var harassQ = (Menu.Item("harassQ").GetValue<bool>());
            var harassE = (Menu.Item("harassE").GetValue<bool>());
            var harassR = (Menu.Item("harassR").GetValue<bool>());

            if (harassR && R.IsReady())
            {
                var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(R.Range))
                {
                    R.CastIfHitchanceEquals(target, HitChance.High);
                }
            }

            if (harassQ && Q.IsReady())
            {
                var target = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(Q.Range))
                {
                    Q.CastIfHitchanceEquals(target, HitChance.VeryHigh);
                }
            }

            if (harassE && E.IsReady())
            {
                var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);
                if (target.IsValidTarget(E.Range))
                {
                    E.CastIfHitchanceEquals(target, HitChance.Medium);
                }
            }

        }

        private static void clearQ()
        {
            var minion = MinionManager.GetMinions(Player.ServerPosition, Q.Range).FirstOrDefault();
            if (minion == null || minion.Name.ToLower().Contains("ward"))
            {
                return;
            }

            var farmLocation =
                MinionManager.GetBestCircularFarmLocation(
                    MinionManager.GetMinions(Q.Range, MinionTypes.All, MinionTeam.Enemy)
                        .Select(m => m.ServerPosition.To2D())
                        .ToList(),
                    Q.Width,
                    Q.Range);

            if (Menu.Item("clearQ").GetValue<bool>() && minion.IsValidTarget() && Q.IsReady())
            {
                Q.Cast(farmLocation.Position);
            }
        }

        private static void clearE()
        {
            var minion = MinionManager.GetMinions(Player.ServerPosition, E.Range).FirstOrDefault();
            if (minion == null || minion.Name.ToLower().Contains("ward"))
            {
                return;
            }

            var farmLocation =
                MinionManager.GetBestCircularFarmLocation(
                    MinionManager.GetMinions(E.Range, MinionTypes.All, MinionTeam.Enemy)
                        .Select(m => m.ServerPosition.To2D())
                        .ToList(),
                    E.Width,
                    E.Range);

            if (Menu.Item("clearE").GetValue<bool>() && minion.IsValidTarget() && E.IsReady())
            {
                E.Cast(farmLocation.Position);
            }
        }

        private static void clearR()
        {
            var minion = MinionManager.GetMinions(Player.ServerPosition, R.Range).FirstOrDefault();
            if (minion == null || minion.Name.ToLower().Contains("ward"))
            {
                return;
            }

            var farmLocation =
                MinionManager.GetBestCircularFarmLocation(
                    MinionManager.GetMinions(R.Range, MinionTypes.All, MinionTeam.Enemy)
                        .Select(m => m.ServerPosition.To2D())
                        .ToList(),
                    R.Width,
                    R.Range);

            if (Menu.Item("clearR").GetValue<bool>() && minion.IsValidTarget() && R.IsReady())
            {
                R.Cast(farmLocation.Position);
            }
        }

        private static void ks()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.True);
            if (Menu.Item("ks").GetValue<bool>() && R.IsReady() && target.IsValidTarget(R.Range) && R.IsKillable(target))
            {
                R.CastIfHitchanceEquals(target, HitChance.Medium);
            }

            if (Menu.Item("ks").GetValue<bool>() && Q.IsReady() && target.IsValidTarget(Q.Range) && Q.IsKillable(target))
            {
                Q.CastIfHitchanceEquals(target, HitChance.Medium);
            }

            if (Menu.Item("ks").GetValue<bool>() && E.IsReady() && target.IsValidTarget(E.Range) && E.IsKillable(target))
            {
                E.CastIfHitchanceEquals(target, HitChance.Medium);
            }
        }

        private static void Drawing_OnDraw (EventArgs args)
        {
            if (Player.IsDead)
                return;

            if (Menu.Item("drawQ").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, Q.Range, Color2.Goldenrod);
            }

            if (Menu.Item("drawW").GetValue<bool>())
            {
                Render.Circle.DrawCircle(Player.Position, W.Range, Color2.Goldenrod);
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