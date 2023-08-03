using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D3DLab.ECS;

namespace LifeSim.Particles.Plugin.D3D {
    class ParticleRelationComponent : IGraphicComponent, IDisposable {
        public Dictionary<ParticleTypes, float> Relationships { get; }
        public ParticleRelationComponent(Dictionary<ParticleTypes, float> rules) {
            this.Tag = (this.Tag = ElementTag.New());
            this.IsValid = true;
            this.Relationships = rules;
        }

        public ElementTag Tag { get; }
        public bool IsValid { get; }
        public void Dispose() {
        }
    }
}
