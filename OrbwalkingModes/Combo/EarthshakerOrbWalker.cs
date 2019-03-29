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

    public class EarthshakerOrbWalker : ComboOrbwalkingMode
    {
        private static readonly ILog Log = AssemblyLogs.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        private readonly Earthshaker hero;

        public EarthshakerOrbWalker(Earthshaker hero)

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

            var forceStaff = this.hero.ForceStaff;
            var forceStaffReady = (forceStaff != null) && forceStaff.CanBeCasted;

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
            var useBlink = false;
            var veil = this.hero.Veil;
            var shiva = this.hero.Shiva;
            var ult = this.hero.EchoSlam;
            var totem = this.hero.Totem;
            var fissure = this.hero.Fissure;

            if ((blink != null) && blink.CanBeCasted && blink.CanHit(this.CurrentTarget) )
            {
                

                if ((ult != null) && ult.CanBeCasted && !ult.CanHit(this.CurrentTarget))
                {
                    // Log.Debug($"bf dmg vs autoattack: {bladeFury.GetTickDamage(this.CurrentTarget)} > {this.Owner.GetAttackDamage(this.CurrentTarget) * bladeFury.TickRate}");
                    var enemyCount = EntityManager<Hero>.Entities.Count(
                        x => x.IsAlive
                             && x.IsVisible
                             && (x != this.CurrentTarget)
                             && this.Owner.IsEnemy(x)
                             && !x.IsIllusion
                             && (x.Distance2D(this.CurrentTarget) <= ult.Radius));


                   useBlink = ((this.CurrentTarget.Health * 0.5f) > ult.GetDamage(this.CurrentTarget)) && (enemyCount > 1);

                     Log.Debug($"{useBlink} - {this.CurrentTarget.Health * 0.5f} > {ult.GetDamage(this.CurrentTarget)} && {enemyCount}");
                    if (!useBlink)
                    {
                        var allyCount = EntityManager<Hero>.Entities.Count(
                            x => x.IsAlive
                                 && x.IsVisible
                                 && (x != this.Owner)
                                 && (this.Owner.Team == x.Team)
                                 && !x.IsIllusion
                                 && (x.Distance2D(this.CurrentTarget) <= ult.Radius));

                        useBlink = (enemyCount > 1);

                    }
                    if (useBlink)
                    {

                        var blinkPos = this.CurrentTarget.IsMoving ? this.CurrentTarget.InFront(5) : this.CurrentTarget.Position;
                        blink.UseAbility(blinkPos);
                        await Task.Delay(blink.GetCastDelay(blinkPos), token);

                        if ((veil != null) && this.Owner.CanCastAbilities(veil, ult) && veil.CanHit(this.CurrentTarget))
                        {
                            veil.UseAbility(this.CurrentTarget.Position);
                            await Task.Delay(veil.GetCastDelay(this.CurrentTarget.Position), token);
                        }
 

                        ult.UseAbility();
                        await Task.Delay(ult.GetCastDelay(), token);
                        return;
                    }

                }
            }
           /* var notDisabled = !this.CurrentTarget.IsStunned() && !this.CurrentTarget.IsRooted();
            if (notDisabled)
            {

                    if ((totem != null) && totem.CanBeCasted)
                    {
                        if (totem.UseAbility(this.CurrentTarget))
                        {
                            await Task.Delay(totem.GetCastDelay(this.CurrentTarget), token);
                        }
                    }
                    if ((fissure != null) && fissure.CanBeCasted)
                    {
                        if (fissure.UseAbility(this.CurrentTarget))
                        {
                            await Task.Delay(fissure.GetCastDelay(this.CurrentTarget), token);
                        }
                    }   

            }//*/

            this.OrbwalkToTarget();
        }
    }
}
