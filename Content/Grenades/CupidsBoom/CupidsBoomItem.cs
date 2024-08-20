using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content.Grenades.CupidsBoom
{
    public class CupidsBoomItem : ModItem
    {
        readonly float Radius = 50;
        public override LocalizedText Tooltip => base.Tooltip.WithFormatArgs(Radius * 10);
        public override string Texture => "GrenadesExpanded/Content/PlaceholderItemSprite";
        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<CupidsBoomProjectile>();
            Item.width = 28;
            Item.height = 30;
            Item.shootSpeed = 15f;
            Item.noMelee = true;
            Item.damage = 40;
            Item.useTime = 60;
            Item.useAnimation = 60;
            Item.penetrate = 5;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.noUseGraphic = true;
            Item.useAmmo = ItemID.Grenade;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ammoUsed = 0;
            int newDamage = damage;
            int timeLeft;

            switch (type)
            {
                case ProjectileID.Grenade:
                    ammoUsed = 0;
                    break;
                case ProjectileID.BouncyGrenade:
                    ammoUsed = 1;
                    break;
                case ProjectileID.StickyGrenade:
                    ammoUsed = 2;
                    break;
                case ProjectileID.Beenade:
                    ammoUsed = 3;
                    break;
            }

            if (ammoUsed == 1)
            {
                timeLeft = 360;
            }
            else
            {
                timeLeft = 180;
            }

            if (ammoUsed == 3)
            {
                newDamage = damage / 10;
            }

            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<CupidsBoomProjectile>(), newDamage, knockback, player.whoAmI, Radius, ammoUsed);
            proj.timeLeft = timeLeft;
            return false;
        }
    }
}