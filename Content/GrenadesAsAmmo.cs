using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content
{
    class GrenadesAsAmmo : GlobalItem
    {
        public override void SetDefaults(Item entity)
        {
            if (entity.type == ItemID.Grenade){
                entity.ammo = ItemID.Grenade; 
            }
            if (entity.type == ItemID.BouncyGrenade){
                entity.ammo = ItemID.Grenade; 
            }
            if (entity.type == ItemID.StickyGrenade){
                entity.ammo = ItemID.Grenade; 
            }
            if (entity.type == ItemID.Beenade){
                entity.ammo = ItemID.Grenade; 
            }
        }
    }
}