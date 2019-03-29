
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

    public class AntiMageOrbWalker : ComboOrbwalkingMode
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly AntiMage hero;

        public AntiMageOrbWalker(AntiMage hero)
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

            var blink = this.hero.BLink;
            if (blink != null)
            {
                this.MaxTargetRange = blink.CastRange * 1.5f;
            }

            if ((this.CurrentTarget == null) || !this.CurrentTarget.IsVisible)
            {
                this.hero.Context.Orbwalker.Active.OrbwalkTo(null);
                return;
            }
            var manta = this.hero.Manta;
            var targetDistance = this.Owner.Distance2D(this.CurrentTarget);
            var attackRange = this.Owner.AttackRange(this.CurrentTarget);

            if ((blink != null) && !this.CurrentTarget.IsIllusion && blink.CanBeCasted && blink.CanHit(this.CurrentTarget) && (((targetDistance > attackRange + 2 ))))
            {   
                    var blinkPos = this.CurrentTarget.IsMoving ? this.CurrentTarget.InFront(120) : this.CurrentTarget.Position;
                    blink.UseAbility(blinkPos);
                    await Task.Delay(blink.GetCastDelay(blinkPos), token);
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

            if ((manta != null) && manta.CanBeCasted && (((targetDistance < attackRange )) || this.Owner.IsSilenced() || this.Owner.IsRooted()))
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

                this.OrbwalkToTarget();
            
        }
       
    }
}
