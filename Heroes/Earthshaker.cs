namespace Vaper.Heroes
{
    using System.Linq;
    using System.Threading;
    using System.Threading.Tasks;

    using Ensage;
    using Ensage.SDK.Abilities.Items;
    using Ensage.SDK.Abilities.npc_dota_hero_earthshaker;
    using Ensage.SDK.Extensions;
    using Ensage.SDK.Helpers;
    using Ensage.SDK.Inventory.Metadata;

    using PlaySharp.Toolkit.Helper.Annotations;

    using Vaper.OrbwalkingModes;
    using Vaper.OrbwalkingModes.Combo;

    [PublicAPI]
    [ExportHero(HeroId.npc_dota_hero_earthshaker)]
    public class Earthshaker : BaseHero
    {


        [ItemBinding]
        public earthshaker_echo_slam EchoSlam { get; private set; }

        [ItemBinding]
        public earthshaker_enchant_totem Totem { get; private set; }

        [ItemBinding]
        public earthshaker_fissure Fissure { get; private set; }

        [ItemBinding]
        public item_blink BlinkDagger { get; private set; }

        [ItemBinding]
        public item_force_staff ForceStaff { get; private set; }

        [ItemBinding]
        public item_lotus_orb Lotus { get; private set; }

        [ItemBinding]
        public item_veil_of_discord Veil { get; private set; }

        [ItemBinding]
        public item_shivas_guard Shiva { get; private set; }

        protected override ComboOrbwalkingMode GetComboOrbwalkingMode()
        {
            return new EarthshakerOrbWalker(this);
        }

        protected override void OnActivate()
        {
            base.OnActivate();

            this.EchoSlam = this.Context.AbilityFactory.GetAbility<earthshaker_echo_slam>();
            this.Totem = this.Context.AbilityFactory.GetAbility<earthshaker_enchant_totem>();
            this.Fissure = this.Context.AbilityFactory.GetAbility<earthshaker_fissure>();
        }
        protected override async Task OnKillsteal(CancellationToken token)
        {
            if (Game.IsPaused || !this.Owner.IsAlive || !this.Fissure.CanBeCasted )
            {
                await Task.Delay(125, token);
                return;
            }

            var killstealTarget = EntityManager<Hero>.Entities.FirstOrDefault(
                x => x.IsAlive
                     && this.Owner.IsEnemy(x)
                     && !x.IsIllusion
                     && this.Fissure.CanHit(x)
                     && (this.Fissure.GetDamage(x) > x.Health));

            if (killstealTarget != null)
            {
                if (this.Fissure.UseAbility(killstealTarget))
                {
                    var castDelay = this.Fissure.GetCastDelay(killstealTarget);
                    await this.AwaitKillstealDelay(castDelay, token);
                }
            }

            await Task.Delay(125, token);
        }
    }
}