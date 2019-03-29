
namespace Vaper.OrbwalkingModes.Combo
{
    using System;
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Ensage;
    using Ensage.SDK.Extensions;
    using Ensage.SDK.Geometry;
    using Ensage.SDK.Handlers;
    using Ensage.SDK.Helpers;
    using Ensage.SDK.Prediction;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    using SharpDX;

    using Vaper.Heroes;

    public class PangoOrbWalker : ComboOrbwalkingMode
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Pangolier hero;

        public PangoOrbWalker(Pangolier hero)
           : base(hero,1500.0f)
        {
            this.hero = hero;
        }

        public override async Task ExecuteAsync(CancellationToken token)
        {
            if (!await this.ShouldExecute(token))
            {
                return;
            }

            var blink = this.hero.BlinkDagger;
            if (blink != null)
            {
                this.MaxTargetRange = blink.CastRange * 1.5f;
            }

            if ((this.CurrentTarget == null) || !this.CurrentTarget.IsVisible)
            {
                this.hero.Context.Orbwalker.Active.OrbwalkTo(null);
                return;
            }

            if (!this.CurrentTarget.IsIllusion)
            {
                var diffu = this.hero.Diffu;
                var euls = this.hero.Euls;
                var shield = this.hero.ShieldCrash;
                var swack = this.hero.Swack;
                var ultstart = this.hero.ultStart;
                var ultstop = this.hero.ultStop;
                var targetDistance = this.Owner.Distance2D(this.CurrentTarget);
                var attackRange = this.Owner.AttackRange(this.CurrentTarget);
                var healthPercent = this.Owner.HealthPercent();
                var GrdGrave = this.hero.GuardianGraves;


                if ((blink != null) && !this.CurrentTarget.IsIllusion && blink.CanBeCasted && blink.CanHit(this.CurrentTarget) && (((targetDistance >= 1200))))
                {
                    var blinkPos = this.CurrentTarget.IsMoving ? this.CurrentTarget.InFront(75) : this.CurrentTarget.Position;
                    blink.UseAbility(blinkPos);
                    await Task.Delay(blink.GetCastDelay(blinkPos), token);
                }

                if (!this.CurrentTarget.IsIllusion)
                {
                    if ((shield != null) && shield.CanBeCasted && (((targetDistance < attackRange))))
                    {
                        shield.UseAbility(this.CurrentTarget);
                        await Task.Delay(shield.GetCastDelay(this.CurrentTarget), token);
                    }//*/

                    if ((swack != null) && swack.CanBeCasted && swack.CanHit(this.CurrentTarget))
                    {
                        var possSw = this.CurrentTarget.IsMoving ? this.CurrentTarget.InFront(75) : this.CurrentTarget.Position;
                        swack.UseAbility(possSw);
                        await Task.Delay(swack.GetCastDelay(possSw), token);
                    }
                    if (!this.CurrentTarget.IsStunned() && !this.CurrentTarget.IsIllusion)
                    {
                        var abysal = this.hero.AbyssalBlade;
                        if ((abysal != null) && abysal.CanBeCasted && abysal.CanHit(this.CurrentTarget))
                        {
                            abysal.UseAbility(this.CurrentTarget);
                            await Task.Delay(abysal.GetCastDelay(this.CurrentTarget), token);
                        }
                    }
                    if (!ultstart )
                    {
                        if ((ultstart != null)&& ultstart.CanBeCasted && (((targetDistance < attackRange ))))
                        {                 
                            ultstart.UseAbility(this.CurrentTarget);
                            await Task.Delay(ultstart.GetCastDelay(this.CurrentTarget), token);
                        }
                        if ((euls != null) && euls.CanBeCasted)
                        {
                            euls.UseAbility(this.Owner);
                            await Task.Delay(euls.GetCastDelay(), token);
                        }   
                    }
                }

                var mjollnir = this.hero.Mjollnir;
                if ((mjollnir != null) && mjollnir.CanBeCasted && mjollnir.CanHit(this.CurrentTarget))
                {
                    mjollnir.UseAbility(this.Owner);
                    await Task.Delay(mjollnir.GetCastDelay(), token);
                }


                if ((GrdGrave != null) && GrdGrave.CanBeCasted)
                {
                    var recentDmgPercent = (float)this.hero.Owner.RecentDamage / this.hero.Owner.MaximumHealth;

                    // Log.Debug($"RecentDmgPercent: {recentDmgPercent}");
                    if ((healthPercent < 0.2f) || (recentDmgPercent > 0.2))
                    {
                        GrdGrave.UseAbility(this.Owner);
                        await Task.Delay(GrdGrave.GetCastDelay(), token);
                    }
                }

                if (this.hero.RollingMoveOnly && this.Owner.HasModifier(ultstart.ModifierName))
                {
                    this.hero.Context.Orbwalker.Active.OrbwalkTo(null);
                }

                this.OrbwalkToTarget();
            }                 
        }
    }
}
