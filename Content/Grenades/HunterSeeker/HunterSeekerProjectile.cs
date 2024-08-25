using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.Localization;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content.Grenades.HunterSeeker
{
    public class HunterSeekerProjectile : ModProjectile
    {
        private static Item falsePistol = null;
        int shootTimer = 0;
        public override string Texture => "GrenadesExpanded/Content/Grenades/Fastball/Fastball";
        enum Grenades{
            Normal,
            Bouncy,
            Sticky,
            Bee
        }

        Grenades AmmoUsed{
            get => (Grenades)Projectile.ai[1];
        }

        float Radius {
            get => Projectile.ai[0];
        }
        public static Item FalsePistol { get => falsePistol; set => falsePistol = value; }

        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.knockBack = 7f;
        }

        public override void AI()
        {
            Player owner = Main.player[Projectile.owner];
            NPC enemy = FindClosestNPC(1000);

            FalsePistol = new Item();

            FalsePistol.SetDefaults(ItemID.Revolver, true);

            FalsePistol.damage = Projectile.damage/5;
            FalsePistol.knockBack = Projectile.knockBack/5;

            if (shootTimer % 15 == 0 && AmmoUsed == Grenades.Bee && enemy != null){
                float speedX = Main.rand.Next(-35, 36) * 0.02f;
                float speedY = Main.rand.Next(-35, 36) * 0.02f;
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, speedX, speedY, Main.player[Projectile.owner].beeType(), Main.player[Projectile.owner].beeDamage(Projectile.damage), Main.player[Projectile.owner].beeKB(0f), Main.myPlayer);
            }

            if (owner.PickAmmo(FalsePistol, out int projToShoot, out float projSpeed, out int damage, out float knockBack, out int usedAmmoItemId)){
                if (shootTimer >= 60){
                    Vector2 velocity = Projectile.velocity;
                    velocity.Normalize();
                    velocity *= projSpeed;
                    
                    if (enemy != null){
                        velocity = Projectile.Center.DirectionTo(enemy.Center) * projSpeed;
                    }
                    
                    Projectile proj = Projectile.NewProjectileDirect(Projectile.GetSource_FromThis(), Projectile.Center, velocity, projToShoot, damage, knockBack);
                    proj.tileCollide = false;

                    shootTimer = 0;
                }
            }

            NPC target = FindClosestNPC(1000);
            
            if (target != null){
                Projectile.velocity = Vector2.Lerp(Projectile.velocity, Projectile.Center.DirectionTo(target.Center) * 5f, 0.05f);
            }

            Projectile.rotation = Projectile.velocity.ToRotation() - MathHelper.PiOver2;

            if (AmmoUsed == Grenades.Sticky)
            {
                Projectile.tileCollide = false;
                int blockPixelPosStartX = (int)(Projectile.position.X / 16f) - 1;
                int BlockPixelPosEndX = (int)((Projectile.position.X + Projectile.width) / 16f) + 2;
                int blockPixelPosStartY = (int)(Projectile.position.Y / 16f) - 1;
                int blockPixelPosEndY = (int)((Projectile.position.Y + Projectile.height) / 16f) + 2;
                if (blockPixelPosStartX < 0)
                {
                    blockPixelPosStartX = 0;
                }
                if (BlockPixelPosEndX > Main.maxTilesX)
                {
                    BlockPixelPosEndX = Main.maxTilesX;
                }
                if (blockPixelPosStartY < 0)
                {
                    blockPixelPosStartY = 0;
                }
                if (blockPixelPosEndY > Main.maxTilesY)
                {
                    blockPixelPosEndY = Main.maxTilesY;
                }

                Vector2 vector = default;
                for (int j = blockPixelPosStartX; j < BlockPixelPosEndX; j++)
                {
                    for (int k = blockPixelPosStartY; k < blockPixelPosEndY; k++)
                    {
                        if (Main.tile[j, k] == null || !Main.tile[j, k].HasUnactuatedTile || !Main.tileSolid[Main.tile[j, k].TileType] || Main.tileSolidTop[Main.tile[j, k].TileType])
                        {
                            continue;
                        }
                        vector.X = j * 16;
                        vector.Y = k * 16;
                        if (!(Projectile.position.X + Projectile.width - 4f > vector.X) || !(Projectile.position.X + 4f < vector.X + 16f) || !(Projectile.position.Y + Projectile.height - 4f > vector.Y) || !(Projectile.position.Y + 4f < vector.Y + 16f))
                        {
                            continue;
                        }
                        if (Projectile.localAI[0] == 0f)
                        {
                            Vector2 value = vector + new Vector2(8f, 8f);
                            if (Vector2.Distance(Projectile.Center, value) < 12f)
                            {
                                Projectile.Center += Projectile.velocity.SafeNormalize(Vector2.Zero) * -4f;
                            }
                            Projectile.localAI[0] = 1f;
                            Projectile.netUpdate = true;
                        }
                        Projectile.velocity *= 0f;
                    }
                }
            }

            shootTimer++;
        }

        void SpawnExplosion(float radius){
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ExplosionProj>(), Projectile.damage, Projectile.knockBack, Projectile.owner, radius);      
            if (AmmoUsed == Grenades.Bee){
                int rand = Main.rand.Next(15, 25);
                for (int i = 0; i < rand; i++)
                {
                    float speedX = Main.rand.Next(-35, 36) * 0.02f;
                    float speedY = Main.rand.Next(-35, 36) * 0.02f;
                    Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, speedX, speedY, Main.player[Projectile.owner].beeType(), Main.player[Projectile.owner].beeDamage(Projectile.damage), Main.player[Projectile.owner].beeKB(0f), Main.myPlayer);
                }
            }
        }

        public NPC FindClosestNPC(float maxDetectDistance) {
			NPC closestNPC = null;

			float sqrMaxDetectDistance = maxDetectDistance * maxDetectDistance;

			for (int k = 0; k < Main.maxNPCs; k++) {
				NPC target = Main.npc[k];
				if (target.CanBeChasedBy()) {
					float sqrDistanceToTarget = Vector2.DistanceSquared(target.Center, Projectile.Center);

					if (sqrDistanceToTarget < sqrMaxDetectDistance) {
						sqrMaxDetectDistance = sqrDistanceToTarget;
						closestNPC = target;
					}
				}
			}
            return closestNPC;
        }

        public override bool OnTileCollide(Vector2 oldVelocity)
        {
            if (AmmoUsed == Grenades.Bouncy){
                if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f){
                    Projectile.velocity.X = oldVelocity.X * -0.9f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f){
                    Projectile.velocity.Y = oldVelocity.Y * -0.9f;
                }
                return false;
            }

            return true;
        }

        public override void OnKill(int timeLeft)
        {
            SpawnExplosion(Radius);
        }
    }
}