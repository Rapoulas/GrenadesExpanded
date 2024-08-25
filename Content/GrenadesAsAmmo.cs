using System.Collections.Generic;
using System.Linq;
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

        public override void ModifyTooltips(Item item, List<TooltipLine> tooltips)
        {
            if (item.type == ItemID.Beenade){
                tooltips.Add(new TooltipLine(ModContent.GetInstance<GrenadesExpanded>(), "BeenadeTooltipChange", "0.1x Damage if used as Ammo for grenades"));
            }
        }
    }
}