using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color2 = System.Drawing.Color;

namespace MetaSharp
{
    class Brand
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Menu;

        public Brand()
        {
            Load();
        }

        private void Load()
        {

            Q = new Spell(SpellSlot.Q, 1050);
            W = new Spell(SpellSlot.W, 900);
            E = new Spell(SpellSlot.E, 625);
            R = new Spell(SpellSlot.R, 750);

            Q.SetSkillshot(0.25f, 60, 1600, true, SkillshotType.SkillshotLine);
            W.SetSkillshot(1, 240, float.MaxValue, false, SkillshotType.SkillshotCircle);
            E.SetTargetted(0.25f, float.MaxValue);
            R.SetTargetted(0.25f, 1000);

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
            harassMenu.AddItem(new MenuItem("harassE", "Use E").SetValue(true));
            harassMenu.AddItem(new MenuItem("harassR", "Use R").SetValue(false));
            harassMenu.AddItem(new MenuItem("Harass", "Harass").SetValue(new KeyBind("C".ToCharArray()[0], KeyBindType.Press)));

            Menu clearMenu = Menu.AddSubMenu(new Menu("Lane Clear", "Lane Clear"));
            clearMenu.AddItem(new MenuItem("clearQ", "Use Q").SetValue(true));
            clearMenu.AddItem(new MenuItem("clearW", "Use W").SetValue(true));
            clearMenu.AddItem(new MenuItem("clearE", "Use E").SetValue(true));
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
                clearW();
                clearE();
                clearQ();
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

            if (comboW && W.IsReady())
            {
                var targetW = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                if (targetW.IsValidTarget(W.Range))
                {
                    W.CastIfHitchanceEquals(targetW, HitChance.VeryHigh);
                }
            }

            var targetE = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);

            if (targetE.HasBuff("brandblaze"))
            {
                if (comboE && E.IsReady())
                {
                    E.Cast(targetE);
                }

                if (comboQ && Q.IsReady())
                {
                    var targetQ = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                    Q.CastIfHitchanceEquals(targetQ, HitChance.VeryHigh);
                }
            }

            if (comboR && R.IsReady())
            {
                var targetR = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                R.Cast(targetR);
            }

        }

        private static void harass()
        {
            var harassQ = (Menu.Item("harassQ").GetValue<bool>());
            var harassW = (Menu.Item("harassW").GetValue<bool>());
            var harassE = (Menu.Item("harassE").GetValue<bool>());
            var harassR = (Menu.Item("harassR").GetValue<bool>());

            if (harassW && W.IsReady())
            {
                var targetW = TargetSelector.GetTarget(W.Range, TargetSelector.DamageType.Magical);
                if (targetW.IsValidTarget(W.Range))
                {
                    W.CastIfHitchanceEquals(targetW, HitChance.VeryHigh);
                }
            }

            var targetE = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.Magical);

            if (targetE.HasBuff("brandblaze"))
            {
                if (harassE && E.IsReady())
                {
                    E.Cast(targetE);
                }

                if (harassQ && Q.IsReady())
                {
                    var targetQ = TargetSelector.GetTarget(Q.Range, TargetSelector.DamageType.Magical);
                    Q.CastIfHitchanceEquals(targetQ, HitChance.VeryHigh);
                }
            }

            if (harassR && R.IsReady())
            {
                var targetR = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
                R.Cast(targetR);
            }
        }

        private static void clearW()
        {
            var minion = MinionManager.GetMinions(Player.ServerPosition, W.Range).FirstOrDefault();
            if (minion == null || minion.Name.ToLower().Contains("ward"))
            {
                return;
            }

            var farmLocation =
                MinionManager.GetBestCircularFarmLocation(
                    MinionManager.GetMinions(W.Range, MinionTypes.All, MinionTeam.Enemy)
                        .Select(m => m.ServerPosition.To2D())
                        .ToList(),
                    W.Width,
                    W.Range);

            if (Menu.Item("clearW").GetValue<bool>() && minion.IsValidTarget() && W.IsReady())
            {
                W.Cast(farmLocation.Position);
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
                E.Cast(minion);
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

        private static void ks()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.Magical);
            if (Menu.Item("ks").GetValue<bool>() && R.IsReady() && target.IsValidTarget(R.Range) && R.IsKillable(target))
            {
                R.Cast(target);
            }

            if (Menu.Item("ks").GetValue<bool>() && Q.IsReady() && target.IsValidTarget(Q.Range) && Q.IsKillable(target))
            {
                Q.CastIfHitchanceEquals(target, HitChance.Medium);
            }

            if (Menu.Item("ks").GetValue<bool>() && E.IsReady() && target.IsValidTarget(E.Range) && E.IsKillable(target))
            {
                E.Cast(target);
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