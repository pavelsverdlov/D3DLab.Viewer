using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using D3DLab.ECS;

namespace LifeSim.Particles.Plugin {
    class ParticleGameObject {
        public ParticleGameObject(ElementTag tag, GameObject gameObject) {
            this.Tag = tag;
            this.GameObject = gameObject;
        }
        public ElementTag Tag { get; }
        public GameObject GameObject { get; }
    }
}
