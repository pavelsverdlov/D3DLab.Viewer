﻿using D3DLab.ECS.Input;
using D3DLab.Toolkit.Input;
using D3DLab.Toolkit.Input.Commands.Camera;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace D3DLab.App.Shell.D3D {
    class ViewerInputObserver : RotateZoomPanInputObserver {
        readonly ICameraInputHandler inputHandler;

        public ViewerInputObserver(FrameworkElement control, IInputPublisher publisher, ICameraInputHandler inputHandler)
            : base(publisher, inputHandler) {
            this.inputHandler = inputHandler;

            RotationSensitivity = 1.5f;
        }

        protected override InputState GetIdleState() {
            var states = new StateDictionary();
            states.Add((int)AllInputStates.Idle, s => new InputIdleState(s));
            states.Add((int)AllInputStates.Rotate, s => new InputRotateStateWithCursorReturning(s));
            states.Add((int)AllInputStates.Zoom, s => new InputZoomState(s));
            states.Add((int)AllInputStates.Pan, s => new InputPanState(s));
            states.Add((int)AllInputStates.Target, s => new InputTargetState(s));
            states.Add((int)AllInputStates.ChangeRotateCenter, s => new InputChangeRotateCenterState(s));

            var router = new StateHandleProcessor<ICameraInputHandler>(states, this);
            router.SwitchTo((int)AllInputStates.Idle, InputStateData.Create());
            return router;

        }
    }
}
