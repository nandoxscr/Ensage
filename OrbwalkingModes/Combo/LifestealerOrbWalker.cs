// <copyright file="PhantomAssassinComboOrbwalker.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Vaper.OrbwalkingModes.Combo
{
    using System.Threading;
    using System.Threading.Tasks;
    using Ensage.Common.Extensions;
    using System.Linq;
    using Ensage.SDK.Helpers;
    using Vaper.Heroes;
    using SharpDX;
    using Ensage;
    using Ensage.Common.Threading;

    using UnitExtensions = Ensage.SDK.Extensions.UnitExtensions;

    public class LifestealerOrbWalker : ComboOrbwalkingMode
    {
        private readonly Lifestealer hero;



        public LifestealerOrbWalker(Lifestealer hero)
            : base(hero, 1500.0f)
        {
            this.hero = hero;
        }

        private int GetAbilityDelay(Unit unit, Ability ability)
        {
            return (int)(((ability.FindCastPoint() + this.Owner.GetTurnTime(unit)) * 1000.0) + Game.Ping) + 50;
        }

        private int GetAbilityDelay(Vector3 pos, Ability ability)
        {
            return (int)(((ability.FindCastPoint() + this.Owner.GetTurnTime(pos)) * 1000.0) + Game.Ping) + 50;
        }

        public override async Task ExecuteAsync(CancellationToken token)
        {
            if (!await this.ShouldExecute(token))
            {
                return;
            }

            if ((this.CurrentTarget == null) || !this.CurrentTarget.IsVisible)
            {
                this.hero.Context.Orbwalker.Active.OrbwalkTo(null);
                return;
            }
            var items = this.hero.Items.Value;
            //---------------------Ability----------------------
            var Rage = this.hero._RageAbility;
            var openwnd = this.hero._OpenWondAbility;
            //------------------ITEM ABILITY-----------------
            var diff = this.hero.DiffBlade;
            var abysal = this.hero.AbyssalBlade;
            var mlonir = this.hero.Mjollnir;
            var blink = this.hero.BlinkDagger;
            var solar = this.hero.SolarCrest;
            var medalion = this.hero.Medalion;
            var armlet = this.hero.Armlet;
            //-----------------Function--------------------
            var blinkReaady = (blink != null) && items.IsEnabled(blink.Ability.Name) && blink.CanBeCasted;
            var duffusalReady = (diff != null) && items.IsEnabled(diff.Ability.Name) && diff.CanBeCasted;
            var mjonirReady = (mlonir != null) && items.IsEnabled(mlonir.Ability.Name) && mlonir.CanBeCasted;
            var AbisalReady = (abysal != null) && items.IsEnabled(abysal.Ability.Name) && abysal.CanBeCasted;
            var solarReady = (solar != null) && items.IsEnabled(solar.Ability.Name) && solar.CanBeCasted;
            var medalionReady = (medalion != null) && items.IsEnabled(medalion.Ability.Name) && medalion.CanBeCasted;
            //-------------------------------------------------------------------------------------------------
            var targetDistance = this.Owner.Distance2D(this.CurrentTarget);
            var attackRange = UnitExtensions.AttackRange(this.CurrentTarget) + 100;

            if (blinkReaady && !this.CurrentTarget.IsIllusion && blink.CanBeCasted && blink.CanHit(this.CurrentTarget) && (((targetDistance > attackRange))))
            {
                blink.UseAbility(this.CurrentTarget.Position);
                await Task.Delay(blink.GetCastDelay(this.CurrentTarget.Position), token);
            }

            if ((openwnd != null) && openwnd.CanBeCasted() && openwnd.CanHit(this.CurrentTarget))
            {
                openwnd.UseAbility(this.CurrentTarget);
                await Task.Delay(this.GetAbilityDelay(base.CurrentTarget, openwnd), token);
            }

            if ((Rage != null) && base.Owner.IsAttacking() && Rage.CanBeCasted())
            {
                Rage.UseAbility();
                await Task.Delay(this.GetAbilityDelay(base.Owner, Rage), token);
            }

            if (mjonirReady && mlonir.CanBeCasted && mlonir.CanHit(this.CurrentTarget))
            {
                mlonir.UseAbility(this.Owner);
                await Await.Delay(mlonir.GetCastDelay(), token);
            }

            if (!this.CurrentTarget.IsAttacking() && !this.CurrentTarget.IsIllusion)
            {

                if (AbisalReady && abysal.CanBeCasted && abysal.CanHit(this.CurrentTarget))
                {
                    abysal.UseAbility(this.CurrentTarget);
                    await Task.Delay(abysal.GetCastDelay(this.CurrentTarget), token);
                }
                if (duffusalReady && diff.CanBeCasted && this.Owner.IsVisible && diff.CanHit(this.CurrentTarget))
                {
                    diff.UseAbility(this.CurrentTarget);
                    await Await.Delay(diff.GetCastDelay(this.CurrentTarget), token);
                }
                if (solarReady && solar.CanBeCasted && this.Owner.IsVisible && solar.CanHit(this.CurrentTarget))
                {
                    solar.UseAbility(this.CurrentTarget);
                    await Await.Delay(solar.GetCastDelay(this.CurrentTarget), token);
                }
                if (medalionReady && medalion.CanBeCasted && this.Owner.IsVisible && medalion.CanHit(this.CurrentTarget))
                {
                    medalion.UseAbility(this.CurrentTarget);
                    await Await.Delay(medalion.GetCastDelay(this.CurrentTarget), token);
                }
            }

            if ((armlet != null) && armlet.CanBeCasted && armlet.CanHit(this.CurrentTarget))
            {
                armlet.Enabled = false;
            }
            else
            {
                armlet.Enabled = true;
            }
            await Task.Delay(armlet.GetCastDelay(), token);


            /*  var isInvisible = this.Owner.IsInvisible() || (this.Owner.InvisiblityLevel > 0);
              if ((armlet != null) && !isInvisible)
              {
                  var enemiesNear = EntityManager<Hero>.Entities.Any(
                      x => x.IsVisible 
                      && x.IsAlive 
                      && !x.IsIllusion 
                      && (x.Team != this.Owner.Team) 
                      && (x.Distance2D(this.Owner) < 1000));
                  if ((enemiesNear !=null) )
                  {
                      armlet.Enabled = false;
                  }
                  else
                  {
                      armlet.Enabled = true;
                  }

                  await Task.Delay(armlet.GetCastDelay(), token);
              }*/

            this.OrbwalkToTarget();
        }
    }
}
