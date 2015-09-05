using System;
using System.Linq;
using LeagueSharp;
using LeagueSharp.Common;
using Color2 = System.Drawing.Color;

namespace MetaSharp
{
    class Garen
    {
        private static Obj_AI_Hero Player { get { return ObjectManager.Player; } }
        private static Orbwalking.Orbwalker Orbwalker;
        private static Spell Q, W, E, R;
        private static Menu Menu;

        public Garen()
        {
            Load();
        }

        private void Load()
        {
            Q = new Spell(SpellSlot.Q, 175);
            W = new Spell(SpellSlot.W);
            E = new Spell(SpellSlot.E, 300);
            R = new Spell(SpellSlot.R, 400);

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

            Menu clearMenu = Menu.AddSubMenu(new Menu("Lane Clear", "Lane Clear"));
            clearMenu.AddItem(new MenuItem("clearQ", "Use Q").SetValue(false));
            clearMenu.AddItem(new MenuItem("clearE", "Use E").SetValue(true));
            clearMenu.AddItem(new MenuItem("Harass", "Harass").SetValue(new KeyBind("V".ToCharArray()[0], KeyBindType.Press)));

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
            if (Player.IsDead)
            {
                return;
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.Combo)
            {
                combo();
            }

            if (Orbwalker.ActiveMode == Orbwalking.OrbwalkingMode.LaneClear)
            {
                clear();
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

            if (comboQ && Q.IsReady() && Player.CountEnemiesInRange(600) > 0)
            {
                Q.Cast();
            }

            if (comboW && W.IsReady() && Player.CountEnemiesInRange(550) > 0)
            {
                W.Cast();
            }

            var target = TargetSelector.GetTarget(E.Range, TargetSelector.DamageType.True);

            if (comboE && E.IsReady() && target.IsValidTarget(E.Range))
            {
                E.Cast();
            }

            var targetR = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.True);

            if (comboR && R.IsReady() && targetR.IsValidTarget(R.Range) && R.IsKillable(target))
            {
                R.Cast(targetR);
            }

        }

        private static void clear()
        {
            var minion = MinionManager.GetMinions(Player.ServerPosition, E.Range).FirstOrDefault();
            if (minion == null || minion.Name.ToLower().Contains("ward"))
            {
                return;
            }

            var farmLocation =
                MinionManager.GetBestCircularFarmLocation(
                    MinionManager.GetMinions(Q.Range, MinionTypes.All, MinionTeam.Enemy)
                        .Select(m => m.ServerPosition.To2D())
                        .ToList(),
                    E.Width,
                    E.Range);

            if (Menu.Item("clearQ").GetValue<bool>() && minion.IsValidTarget() && Q.IsReady())
            {
                Q.Cast();
            }

            if (Menu.Item("clearE").GetValue<bool>() && minion.IsValidTarget() && E.IsReady())
            {
                E.Cast();
            }
        }

        private static void ks()
        {
            var target = TargetSelector.GetTarget(R.Range, TargetSelector.DamageType.True);
            if (Menu.Item("ks").GetValue<bool>() && R.IsReady() && target.IsValidTarget(R.Range) && R.IsKillable(target))
            {
                R.Cast(target);
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