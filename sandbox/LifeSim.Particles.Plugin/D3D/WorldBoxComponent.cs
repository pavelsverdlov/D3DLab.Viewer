using System;
using System.Numerics;

using D3DLab.ECS;

namespace LifeSim.Particles.Plugin.D3D {
    struct WorldBoxComponent : IGraphicComponent, IDisposable {
        public WorldBoxComponent(AxisAlignedBox box) {
            this.Tag = ElementTag.New("world");
            this.IsValid = true;
            this.Box = box;
        }
        public readonly ElementTag Tag { get; }
        public readonly bool IsValid { get; }
        public readonly AxisAlignedBox Box { get; }
        internal static WorldBoxComponent Create(AxisAlignedBox box) {
            return new WorldBoxComponent(box);
        }
        public void Dispose() {
        }
    }
}
