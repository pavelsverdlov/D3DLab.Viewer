﻿using System;
using D3DLab.ECS;
using D3DLab.ECS.Input;
using D3DLab.ECS.Context;
using D3DLab.ECS.Systems;
using D3DLab.Toolkit.Systems;
using D3DLab.Toolkit.Render;
using D3DLab.Toolkit.Techniques.Lines;
using D3DLab.Toolkit.Input;
using D3DLab.Toolkit.Host;
using System.Windows;
using D3DLab.Toolkit.Input.Publishers;
using D3DLab.Toolkit.D3Objects;
using D3DLab.Toolkit.Techniques.TriangleColored;
using System.Numerics;
using D3DLab.Toolkit;
using D3DLab.Viewer.D3D.Systems;
using D3DLab.Toolkit.Techniques.TriangleTextured;
using D3DLab.Toolkit.Techniques.SpherePoint;
using D3DLab.App.Shell.D3D.Systems;
using D3DLab.Plugin;

namespace D3DLab.App.Shell.D3D {
    public class WFScene : D3DWFScene {
        class EmptyHandler : DefaultInputObserver.ICameraInputHandler {
            public void ChangeRotateCenter(InputStateData state) {
                // Method intentionally left empty.
            }

            public void ChangeTransparencyOnObjectUnderCursor(InputStateData state, bool isMmbHolded2sec) {
                // Method intentionally left empty.
            }

            public void FocusToObject(InputStateData state) {
                // Method intentionally left empty.
            }

            public void HideOrShowObjectUnderCursor(InputStateData state) {
                // Method intentionally left empty.
            }
            public void Idle() {
                // Method intentionally left empty.
            }
            public void KeywordMove(InputStateData state) {
                // Method intentionally left empty.
            }
            public void Pan(InputStateData state) {
                // Method intentionally left empty.
            }
            public bool Rotate(InputStateData state) {
                return true;
            }
            public void Zoom(InputStateData state) {
                // Method intentionally left empty.
            }
        }

        protected CameraObject cameraObject;
        protected BaseInputPublisher publisher;

        public event Action Loaded;
        public CoordinateSystemLinesGameObject CoordinateSystem { get; private set; }

        public WFScene(FormsHost host, FrameworkElement overlay, ContextStateProcessor context, EngineNotificator notify)
            : base(host, overlay, context, notify) {
        }

        protected override DefaultInputObserver CreateInputObserver(WinFormsD3DControl win, FrameworkElement overlay) {
            publisher = new WinFormInputPublisher(win);
            return new ViewerInputObserver(overlay, publisher, new EmptyHandler());
        }

        protected override void SceneInitialization(IContextState context, RenderEngine engine, ElementTag camera) {
            var smanager = Context.GetSystemManager();

            smanager.CreateSystem<DefaultInputSystem>();
            smanager.CreateSystem<ZoomToAllObjectsSystem>();
            smanager.CreateSystem<ZoomToObjectSystem>();
            smanager.CreateSystem<PluginContainerSystem>();
            smanager.CreateSystem<MovingSystem>();
            smanager.CreateSystem<CollidingSystem>();
            smanager.CreateSystem<DefaultOrthographicCameraSystem>();
            smanager.CreateSystem<LightsSystem>();

            smanager
                .CreateSystem<RenderSystem>()
                .Init(engine.Graphics)
                .CreateNested<TriangleColoredVertexRenderTechnique<ToolkitRenderProperties>>()
                .CreateNested<TriangleTexturedVertexRenderTechnique<ToolkitRenderProperties>>()
                .CreateNested<LineVertexRenderTechnique<ToolkitRenderProperties>>()
                .CreateNested<SpherePointRenderTechnique<ToolkitRenderProperties>>()
                ;

            var manager = Context.GetEntityManager();
            cameraObject = CameraObject.UpdateOrthographic<RenderSystem>(camera, Context, Surface);

            LightObject.CreateAmbientLight(manager, 0.2f);//0.05f
            LightObject.CreateFollowCameraDirectLight(manager, System.Numerics.Vector3.UnitZ, 0.8f);//0.95f

            CoordinateSystem = CoordinateSystemLinesGameObject.Create(context, false);

            Loaded?.Invoke();


            //VisualSphereObject.SphereGeo(Context, ElementTag.New($"Point"), new VisualSphereObject.Data {
            //    Center = new Vector3(10, 20, 10),
            //    Color = V4Colors.Yellow,
            //    Radius = 10
            //});
        }

        static Vector4 ToVector4(System.Windows.Media.Color color) {
            color.Clamp();
            return new Vector4(color.R / 255f, color.G / 255f, color.B / 255f, color.A / 255f);
        }


        public void ZoomToAllObjects() {
            Context.GetComponentManager().UpdateComponents(Engine.WorldTag, ZoomToAllCompponent.Create());
        }

        public ElementTag GetWorldTag() => Engine.WorldTag;

        public override void Dispose() {
            base.Dispose();
            publisher?.Dispose();
        }

        protected void InvokeLoaded() => Loaded?.Invoke();

    }
}
