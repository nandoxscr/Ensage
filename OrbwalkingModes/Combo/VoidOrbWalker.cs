// <copyright file="PhantomAssassinComboOrbwalker.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Vaper.OrbwalkingModes.Combo
{
    using System.Linq;
    using System.Reflection;
    using System.Threading;
    using System.Threading.Tasks;

    using Ensage;
    using Ensage.SDK.Extensions;
    using Ensage.SDK.Helpers;

    using log4net;

    using PlaySharp.Toolkit.Logging;

    using Vaper.Heroes;

    public class VoidOrbWalker : ComboOrbwalkingMode
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Void hero;

        public VoidOrbWalker(Void hero)
            : base(hero)
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
            var crono = this.hero.Chrono;
            var dilation = this.hero.Dilation;
            var timewalk = this.hero.TimeWalk;
            var targetDistance = this.Owner.Distance2D(this.CurrentTarget);
            var healthPercent = this.Owner.HealthPercent();
            var attackRange = this.Owner.AttackRange(this.CurrentTarget);
            var isInvisible = this.Owner.IsInvisible() || (this.Owner.InvisiblityLevel > 0);
            var bkb = this.hero.BKB;
            var manta = this.hero.Manta;

            if (this.Owner.IsSilenced() || this.Owner.IsRooted())
            {
                if ((manta != null) && manta.CanBeCasted)
                {
                    manta.UseAbility();
                    await Task.Delay(manta.GetCastDelay(), token);
                }
            }

        /*    if ((timewalk != null) && timewalk.CanBeCasted)
            {
                var recentDmgPercent = (float)this.hero.Owner.RecentDamage / this.hero.Owner.MaximumHealth;
                if ((healthPercent < 0.5f ) || (recentDmgPercent > 0.2))
                {
                    timewalk.UseAbility(this.Owner);
                    await Task.Delay(timewalk.GetCastDelay(), token);
                }
            }*/

            var satanic = this.hero.Satanic;
            if ((satanic != null) && satanic.CanBeCasted)
            {
                var recentDmgPercent = (float)this.hero.Owner.RecentDamage / this.hero.Owner.MaximumHealth;
                if ((healthPercent < 0.2f) || (recentDmgPercent > 0.2))
                {
                    satanic.UseAbility(this.Owner);
                    await Task.Delay(satanic.GetCastDelay(), token);
                }
            }


            if ((blink != null) && !this.CurrentTarget.IsIllusion && blink.CanBeCasted && blink.CanHit(this.CurrentTarget)&&((targetDistance > attackRange + 500)))
            {
                var blinkPos = this.CurrentTarget.IsMoving ? this.CurrentTarget.InFront(75) : this.CurrentTarget.Position;
                blink.UseAbility(blinkPos);
                await Task.Delay(blink.GetCastDelay(blinkPos), token);
            }


            if ((crono != null) && crono.CanBeCasted && crono.CanHit(this.CurrentTarget)  )
            {
                if ((targetDistance < 700))
                {
                    if (isInvisible && (targetDistance <= this.Owner.AttackRange(this.CurrentTarget)))
                    {
                        this.hero.Context.Orbwalker.Active.Attack(this.CurrentTarget);
                        await Task.Delay((int)(this.Owner.GetAutoAttackArrivalTime(this.CurrentTarget) * 1000.0f * 2f), token);

                    }

                    if (crono.UseAbility(this.CurrentTarget))
                    {
                        await Task.Delay(crono.GetCastDelay(this.CurrentTarget), token);
                        return;
                    }


                }
            }//*/


            if ((dilation != null) && dilation.CanBeCasted && dilation.CanHit(this.CurrentTarget))
            {
                dilation.UseAbility(this.Owner);
                await Task.Delay(dilation.GetCastDelay(), token);
            }

            var blod = this.hero.Blod;
                if ((blod != null) && blod.CanBeCasted && blod.CanHit(this.CurrentTarget))
                {
                      blod.UseAbility(this.CurrentTarget);
                      await Task.Delay(blod.GetCastDelay(this.CurrentTarget), token);
                }
            var mjollnir = this.hero.Mjollnir;
                if ((mjollnir != null) && mjollnir.CanBeCasted && mjollnir.CanHit(this.CurrentTarget))
                {
                    mjollnir.UseAbility(this.Owner);
                    await Task.Delay(mjollnir.GetCastDelay(), token);
                }
                if ((manta != null) && manta.CanBeCasted && (((targetDistance < attackRange)) || this.Owner.IsSilenced() || this.Owner.IsRooted()))
                {
                    var isSilenced = this.Owner.IsSilenced();
                    manta.UseAbility();
                    await Task.Delay(manta.GetCastDelay(), token);

                    // so we can immediately check for omni again
                    if (isSilenced)
                    {
                        return;
                    }
                }
            if ((bkb != null) && bkb.CanBeCasted && (((targetDistance < attackRange))))
            {
                bkb.UseAbility();
                await Task.Delay(bkb.GetCastDelay(), token);
                return;
            }

            this.OrbwalkToTarget();
            
        }                    
    }
}
