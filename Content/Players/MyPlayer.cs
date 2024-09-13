using Terraria;
using Terraria.Enums;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content.Players
{
    class MyPlayer : ModPlayer
    {
        public Item hasBabyBoom;

        public override void ResetEffects()
        {
            hasBabyBoom = null;
        }
    }
}