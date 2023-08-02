﻿using D3DLab.ECS;
using D3DLab.ECS.Components;
using D3DLab.ECS.Context;
using D3DLab.Toolkit;
using D3DLab.Toolkit.D3Objects;
using D3DLab.Debugger.D3D;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Numerics;
using WPFLab.MVVM;
using D3DLab.FileFormats.GeoFormats;
using D3DLab.Toolkit.Host;
using D3DLab.Toolkit.Components;
using System.Windows;

namespace D3DLab.Debugger.Presentation.TDI.Scene {

    class SceneViewModel : BaseNotify {

        public WFScene Scene { get; private set; }
        public IContextState Context => context;
        public ObservableCollection<SingleVisualObject> GameObjects { get; }


        ContextStateProcessor context;
        EngineNotificator notificator;
        readonly IDockingTabManager dockingManager;
        FormsHost host;
        FrameworkElement overlay;

        public SceneViewModel(IDockingTabManager dockingManager) {
            this.dockingManager = dockingManager;
            GameObjects = new ObservableCollection<SingleVisualObject>();
            dockingManager.OpenSceneTab(this);
        }

        public void SetContext(ContextStateProcessor context, EngineNotificator notificator) {
            this.context = context;
            this.notificator = notificator;

        }
        public void SetSurfaceHost(FormsHost host, FrameworkElement overlay) {
            this.host = host;
            this.overlay = overlay;
            host.HandleCreated += SurfaceCreated;
        }


        public void SurfaceCreated(WinFormsD3DControl obj) {
            if (Scene != null) {
                Scene.ReCreate(obj);
                return;
            }
            
            Scene = new WFScene(host, overlay, context, notificator);
            //Scene.Init(obj);
            //Scene.InitContext();
        }



        public void LoadGameObject(IFileGeometry3D geo, FileInfo texture, string fileName) {
            var em = context.GetEntityManager();
            
            var box = AxisAlignedBox.CreateFrom(geo.Positions);
            var center = box.Center;
            
            var c = (System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#B3B598");

            GraphicEntity en;
            if (geo.TextureCoors.Any()) {
                en = EntityBuilders.BuildTextured(context, geo.Positions, geo.Indices, 
                        geo.TextureCoors.ToArray(), texture,
                        SharpDX.Direct3D11.CullMode.None);
            } else {
                en = EntityBuilders.BuildColored(context, geo.Positions, geo.Indices, geo.Normals,
                   ToVector4(c), SharpDX.Direct3D11.CullMode.Front);
                en.UpdateComponent(FlatShadingGeometryComponent.Create());
                
            }
            en.UpdateComponent(TransformComponent.Create(Matrix4x4.CreateTranslation(Vector3.Zero - center)));
            GameObjects.Add(new SingleVisualObject(en.Tag, fileName));
            
            //var boxlines = PolylineGameObject.Create(
            //    Context,
            //    new ElementTag("poly"),
            //    GeometryBuilder.BuildBox(
            //        new AxisAlignedBox(box.Minimum, box.Maximum)),
            //    V4Colors.Blue
            //    );

            //Context.GetEntityManager().GetEntity(boxlines.Tag)
            //    .UpdateComponent(TransformComponent.Create(Matrix4x4.CreateTranslation(Vector3.Zero - center)));

            //GameObjects.Add(boxlines);
        }


        static Vector4 ToVector4(System.Windows.Media.Color color) {
            color.Clamp();
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }
    }
}
