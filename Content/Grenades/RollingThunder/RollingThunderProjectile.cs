using System;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content.Grenades.RollingThunder
{
    public class RollingThunderProjectile : ModProjectile
    {
        int explosionCooldown = 0;
        public override string Texture => $"Terraria/Images/Projectile_{ProjectileID.Grenade}";
        enum Grenades
        {
            Normal,
            Bouncy,
            Sticky,
            Bee
        }

        Grenades AmmoUsed
        {
            get => (Grenades)Projectile.ai[1];
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.knockBack = 20f;
        }

        public override void AI()
        {
            if (Projectile.velocity.Y == 0f && Projectile.velocity.X != 0f)
            {
                if (AmmoUsed == Grenades.Bouncy)
                {
                    Projectile.velocity.X *= 0.99f;
                }
                else
                {
                    Projectile.velocity.X *= 0.97f;
                }

                if (Projectile.velocity.X > -0.01 && Projectile.velocity.X < 0.01)
                {
                    Projectile.velocity.X = 0f;
                }
            }

            if (AmmoUsed == Grenades.Bouncy)
            {
                Projectile.velocity.Y += 0.2f;
            }
            else
            {
                Projectile.velocity.Y += 0.4f;
            }
            Projectile.rotation += Projectile.velocity.X * 0.1f;

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

            if (IsInsideTile() && explosionCooldown <= 0 && AmmoUsed == Grenades.Sticky){
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ExplosionProj>(), Projectile.damage, Projectile.knockBack);
                explosionCooldown = 30;
            }

            explosionCooldown--;
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
            }
            else{
                if (Projectile.velocity.X != oldVelocity.X && Math.Abs(oldVelocity.X) > 1f){
                    Projectile.velocity.X = oldVelocity.X * -0.5f;
                }
                if (Projectile.velocity.Y != oldVelocity.Y && Math.Abs(oldVelocity.Y) > 1f){
                    Projectile.velocity.Y = oldVelocity.Y * -0.5f;
                }
            }

            if (explosionCooldown <= 0){
                Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ExplosionProj>(), Projectile.damage, Projectile.knockBack);
                explosionCooldown = 30;

                if (AmmoUsed == Grenades.Bee){
                    int rand = Main.rand.Next(15, 25);
                    for (int i = 0; i < rand; i++){
                        float speedX = Main.rand.Next(-35, 36) * 0.02f;
                        float speedY = Main.rand.Next(-35, 36) * 0.02f;
                        Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.position.X, Projectile.position.Y, speedX, speedY, Main.player[Projectile.owner].beeType(), Main.player[Projectile.owner].beeDamage(Projectile.damage), Main.player[Projectile.owner].beeKB(0f), Main.myPlayer);
                    }
                }
            }
            return false;
        }

        public override void OnKill(int timeLeft)
        {
            Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Vector2.Zero, ModContent.ProjectileType<ExplosionProj>(), Projectile.damage, Projectile.knockBack);
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

        bool IsInsideTile() => Collision.SolidCollision(Projectile.TopLeft, Projectile.width, Projectile.height);
    }
}