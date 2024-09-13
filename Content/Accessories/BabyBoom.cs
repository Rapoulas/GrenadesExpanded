using GrenadesExpanded.Content.Players;
using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content.Accessories
{
    class BabyBoom : ModItem
    {
        public override string Texture => "GrenadesExpanded/Content/PlaceholderProjectileSprite";
        public override void SetDefaults() {
			Item.DefaultToAccessory(32, 32);
			Item.SetShopValues(ItemRarityColor.Orange3, Item.buyPrice(gold: 5));
		}

        public override void UpdateAccessory(Player player, bool hideVisual)
        {
            player.GetModPlayer<MyPlayer>().hasBabyBoom = Item;
        }

    }
}