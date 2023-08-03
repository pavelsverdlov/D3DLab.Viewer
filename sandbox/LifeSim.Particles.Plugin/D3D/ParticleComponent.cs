using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

using D3DLab.ECS;

namespace LifeSim.Particles.Plugin.D3D {
    struct ParticleComponent : IGraphicComponent, IDisposable {
        public static ParticleComponent Create(ParticleTypes type, Vector3 position, Vector3 dir, float velocity, float radius) {
            return new ParticleComponent(type, ElementTag.New(), position, dir, velocity, radius);
        }

        public ParticleComponent(ParticleTypes type, ElementTag tag, Vector3 position, Vector3 dir, float velocity, float radius) {
            this = default(ParticleComponent);
            this.Tag = tag;
            this.Position = position;
            this.Direction = dir;
            this.Velocity = velocity;
            this.Radius = radius;
            this.Type = type;
        }

        public ParticleTypes Type { readonly get; set; }

        public ElementTag Tag { readonly get; set; }
        public Vector3 Position { readonly get; set; }
        public bool IsValid { readonly get; set; }
        public Vector3 Direction { readonly get; set; }
        public float Velocity { readonly get; set; }
        public float Radius { readonly get; set; }
        public void Dispose() {
        }
    }
}
