using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content.Grenades.Pinecone
{
    public class PineconeShrapnelProj : ModProjectile
    {
        public override string Texture => "GrenadesExpanded/Content/Grenades/Pinecone/Pinecone_Spike";

        public override void SetStaticDefaults() {
            Main.projFrames[Projectile.type] = 3;
		}
        public override void SetDefaults()
        {
            Projectile.width = 8;
            Projectile.height = 8;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.timeLeft = 120;
            Projectile.usesIDStaticNPCImmunity = true;
            Projectile.idStaticNPCHitCooldown = 20;
        }

        public override void OnSpawn(IEntitySource source)
        {
            Projectile.frame = Main.rand.Next(0, 3);
        }

        public override void AI()
        {
            Projectile.rotation = Projectile.velocity.ToRotation() + MathHelper.PiOver2;
        }
    }
}