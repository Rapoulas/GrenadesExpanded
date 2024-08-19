using GrenadesExpanded.Content.Grenades.RollingThunder;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content.Grenades
{
    public class RollingThunderItem : ModItem
    {
        public override string Texture => "GrenadesExpanded/Content/PlaceholderItemSprite";
        public override void SetDefaults()
        {
            Item.shoot = ModContent.ProjectileType<RollingThunderProjectile>();
            Item.width = 28;
            Item.height = 30;
            Item.shootSpeed = 7f;
            Item.noMelee = true;
            Item.damage = 75;
            Item.useTime = 5;
            Item.useAnimation = 5;
            Item.rare = ItemRarityID.LightRed;
            Item.DamageType = DamageClass.Ranged;
            Item.useStyle = ItemUseStyleID.Shoot;
            Item.useAmmo = ItemID.Grenade;
        }

        public override bool Shoot(Player player, EntitySource_ItemUse_WithAmmo source, Vector2 position, Vector2 velocity, int type, int damage, float knockback)
        {
            float ammoUsed = 0;
            int newDamage = damage;
            int timeLeft;

            switch (type){
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
            
            if (ammoUsed == 1){
                timeLeft = 360;
            }
            else {
                timeLeft = 180;
            }

            if (ammoUsed == 3){
                newDamage = damage/5;
            }

            Projectile proj = Projectile.NewProjectileDirect(source, position, velocity, ModContent.ProjectileType<RollingThunderProjectile>(), newDamage, knockback, player.whoAmI, 200, ammoUsed);
            proj.timeLeft = timeLeft;
            return false;
        }
    }
}