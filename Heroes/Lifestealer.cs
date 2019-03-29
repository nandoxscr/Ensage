// <copyright file="PhantomAssassin.cs" company="Ensage">
//    Copyright (c) 2017 Ensage.
// </copyright>

namespace Vaper.Heroes
{
    using System.Threading;
    using System.Threading.Tasks;
    using System.Linq;
    using Ensage;
    using PlaySharp.Toolkit.Helper.Annotations;
    using Vaper.OrbwalkingModes.Combo;
    using Ensage.SDK.Abilities.Items;
    using Ensage.SDK.Inventory.Metadata;
    using Ensage.SDK.Menu;
    using Ensage.Common.Menu;
    using Ensage.Common.Extensions;
    using Ensage.SDK.Helpers;
    using Ensage.SDK.Handlers;
    using System.Collections.Generic;



    using AbilityId = Ensage.AbilityId;

    using UnitExtensions = Ensage.SDK.Extensions.UnitExtensions;

    [PublicAPI]
    [ExportHero(HeroId.npc_dota_hero_life_stealer)]
    public class Lifestealer : BaseHero
    {
        public MenuItem<AbilityToggler> Items { get; private set; }
        public Ability _OpenWondAbility { get; private set; }
        public Ability _RageAbility { get; private set; }
        public bool HasUserEnabledArmlet { get; private set; }
        public TaskHandler LSController { get; set; }
        [ItemBinding]
        public item_armlet Armlet { get; private set; }

        [ItemBinding]
        public item_abyssal_blade AbyssalBlade { get; private set; }

        [ItemBinding]
        public item_diffusal_blade DiffBlade { get; private set; }

        [ItemBinding]
        public item_mjollnir Mjollnir { get; private set; }

        [ItemBinding]
        public item_blink BlinkDagger { get; private set; }

        [ItemBinding]
        public item_solar_crest SolarCrest { get; private set; }

        [ItemBinding]
        public item_medallion_of_courage Medalion { get; private set; }

        protected override ComboOrbwalkingMode GetComboOrbwalkingMode()
        {
            return new LifestealerOrbWalker(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            this._OpenWondAbility = UnitExtensions.GetAbilityById(this.Owner, AbilityId.life_stealer_open_wounds);
            this._RageAbility = UnitExtensions.GetAbilityById(this.Owner, AbilityId.life_stealer_rage);
            this.Context.Inventory.Attach(this);

            var factory = this.Menu.Hero.Factory;
            var items = new List<AbilityId>
            {
                AbilityId.item_medallion_of_courage,
                AbilityId.item_solar_crest,
                AbilityId.item_mjollnir,
                AbilityId.item_abyssal_blade,
                AbilityId.item_diffusal_blade,
                AbilityId.item_blink
            };
            this.Items = factory.Item("Items", new AbilityToggler(items.ToDictionary(x => x.ToString(), x => true)));

          //  this.LSController = UpdateManager.Run(this.OnUpdate);

        }

        protected override void OnDeactivate()
        {
          //  this.LSController.Cancel();
            base.OnDeactivate();
        }

      /*  private async Task OnUpdate(CancellationToken token)
        {
            if (!this.HasUserEnabledArmlet)
            {
                var isInvisible = this.Owner.IsInvisible() || (this.Owner.InvisiblityLevel > 0);
                if ((this.Armlet != null) && !isInvisible)
                {
                    var enemiesNear = EntityManager<Hero>.Entities.Any(
                        x => x.IsVisible && x.IsAlive && !x.IsIllusion && (x.Team != this.Owner.Team) && (x.Distance2D(this.Owner) < 1000));
                    if (enemiesNear && (UnitExtensions.HealthPercent(this.Owner) <= 0.75))
                    {
                        this.Armlet.Enabled = true;
                    }
                    else
                    {
                        this.Armlet.Enabled = false;
                    }

                    await Task.Delay(Armlet.GetCastDelay(), token);
                }
            }
            await Task.Delay(125, token);
        }*/
    }
}
        
    
