using GrenadesExpanded.Content.Players;
using Microsoft.Xna.Framework;
using Terraria;
using Terraria.Audio;
using Terraria.ID;
using Terraria.ModLoader;

namespace GrenadesExpanded.Content
{
    class ExplosionProj : ModProjectile
    {
        bool BabyBoomProcced = false;
        bool delayedExplosion = false;
        float Radius {
            get => Projectile.ai[0];
            set => Projectile.ai[0] = value;
        }
        public override void SetDefaults()
        {
            Projectile.width = 16;
            Projectile.height = 16;
            Projectile.friendly = true;
            Projectile.hostile = false;
            Projectile.penetrate = -1;
            Projectile.DamageType = DamageClass.Ranged;
            Projectile.tileCollide = false;
            Projectile.timeLeft = 2;
            Projectile.alpha = 255;
            Projectile.usesLocalNPCImmunity = true;
        }

        public override void AI()
        {   
            if (delayedExplosion){
                Projectile.timeLeft += 15;
                delayedExplosion = false;
                Projectile.friendly = false;
                return;
            }

            if (Projectile.timeLeft <= 2){
                Projectile.friendly = true;
                int diameter;
                diameter = (int)Projectile.ai[0] * 2;
                Projectile.Resize(diameter, diameter);
            }
        }

        public override void ModifyHitNPC(NPC target, ref NPC.HitModifiers modifiers)
        {
            if (Main.expertMode) {
				if (target.type >= NPCID.EaterofWorldsHead && target.type <= NPCID.EaterofWorldsTail) {
					modifiers.FinalDamage /= 5;
				}
			}
        }

        public override void OnKill(int timeLeft) {
			SoundEngine.PlaySound(SoundID.Item14, Projectile.position);
            Player player = Main.player[Projectile.owner];

			for (int i = 0; i < Radius/2; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Smoke, 0f, 0f, 100, default, 2f);
				dust.velocity *= 1.4f;
			}

			for (int i = 0; i < 80; i++) {
				Dust dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 3f);
				dust.noGravity = true;
				dust.velocity *= 5f;
				dust = Dust.NewDustDirect(Projectile.position, Projectile.width, Projectile.height, DustID.Torch, 0f, 0f, 100, default, 2f);
				dust.velocity *= 3f;
			}

			for (int g = 0; g < 2; g++) {
				var goreSpawnPosition = new Vector2(Projectile.position.X + Projectile.width / 2 - 24f, Projectile.position.Y + Projectile.height / 2 - 24f);
				Gore gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X += 1.5f;
				gore.velocity.Y += 1.5f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X -= 1.5f;
				gore.velocity.Y += 1.5f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X += 1.5f;
				gore.velocity.Y -= 1.5f;
				gore = Gore.NewGoreDirect(Projectile.GetSource_FromThis(), goreSpawnPosition, default, Main.rand.Next(61, 64), 1f);
				gore.scale = 1.5f;
				gore.velocity.X -= 1.5f;
				gore.velocity.Y -= 1.5f;
			}

            if (!BabyBoomProcced && player.GetModPlayer<MyPlayer>().hasBabyBoom != null && Main.rand.NextFloat() <= 0.35f){
                int newDamage = (int)(Projectile.damage * 0.5f);
                int whoAmI = Projectile.NewProjectile(Projectile.GetSource_FromThis(), Projectile.Center, Projectile.velocity, Projectile.type, newDamage, Projectile.knockBack, Projectile.owner, Projectile.ai[0] * 2f);
                var proj = (ExplosionProj)Main.projectile[whoAmI].ModProjectile;
                proj.BabyBoomProcced = true;
                proj.delayedExplosion = true;
            }
        }

        public override bool? Colliding(Rectangle projHitbox, Rectangle targetHitbox)
        {
            Rectangle center = new((int)Projectile.Center.X, (int)Projectile.Center.Y, 1, 1);
            if (center.Intersects(targetHitbox)) 
                return true;

            float topLeftDistance = Vector2.Distance(Projectile.Center, targetHitbox.TopLeft());
            float topRightDistance = Vector2.Distance(Projectile.Center, targetHitbox.TopRight());
            float bottomLeftDistance = Vector2.Distance(Projectile.Center, targetHitbox.BottomLeft());
            float bottomRightDistance = Vector2.Distance(Projectile.Center, targetHitbox.BottomRight());

            float distanceToClosestPoint = topLeftDistance;
            if (topRightDistance < distanceToClosestPoint)
                distanceToClosestPoint = topRightDistance;
            if (bottomLeftDistance < distanceToClosestPoint)
                distanceToClosestPoint = bottomLeftDistance;
            if (bottomRightDistance < distanceToClosestPoint)
                distanceToClosestPoint = bottomRightDistance;

            return distanceToClosestPoint <= Radius;
        }
    }
}