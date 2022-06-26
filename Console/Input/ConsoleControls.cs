// GENERATED AUTOMATICALLY FROM 'Assets/UnitySubmodules/Console/Input/ConsoleControls.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @ConsoleControls : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @ConsoleControls()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""ConsoleControls"",
    ""maps"": [
        {
            ""name"": ""Actions"",
            ""id"": ""fb1e2f40-a959-4ff3-99e6-bcb63c12b164"",
            ""actions"": [
                {
                    ""name"": ""Autofill"",
                    ""type"": ""Button"",
                    ""id"": ""7b7e47b6-407a-441a-8895-fcd5ccb08788"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Enter"",
                    ""type"": ""Button"",
                    ""id"": ""8073e190-ab37-4a55-a998-ccc9750642f0"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Toggle"",
                    ""type"": ""Button"",
                    ""id"": ""d65646e7-3167-4115-b924-db45df07650b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Up"",
                    ""type"": ""Button"",
                    ""id"": ""9fc72d83-98fc-4c52-97f6-93b3d2581377"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Down"",
                    ""type"": ""Button"",
                    ""id"": ""39471624-392a-420f-95b3-82412dec1c1f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""aaf689e5-d1d0-41be-9716-b7215cf231e0"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Autofill"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5382a815-e848-4143-92f5-6ae4ca639881"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Enter"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82914648-31be-4e0b-a8da-812e7d2cff3a"",
                    ""path"": ""<Keyboard>/#(½)"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""fbea43cd-57ae-4e8d-a286-6c878d710481"",
                    ""path"": ""<Keyboard>/backquote"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Toggle"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""6cbef74f-5747-4e3c-82d0-cdf831888906"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Up"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5179effc-9b4f-4ba3-b867-300d847917d5"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Down"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Actions
        m_Actions = asset.FindActionMap("Actions", throwIfNotFound: true);
        m_Actions_Autofill = m_Actions.FindAction("Autofill", throwIfNotFound: true);
        m_Actions_Enter = m_Actions.FindAction("Enter", throwIfNotFound: true);
        m_Actions_Toggle = m_Actions.FindAction("Toggle", throwIfNotFound: true);
        m_Actions_Up = m_Actions.FindAction("Up", throwIfNotFound: true);
        m_Actions_Down = m_Actions.FindAction("Down", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    // Actions
    private readonly InputActionMap m_Actions;
    private IActionsActions m_ActionsActionsCallbackInterface;
    private readonly InputAction m_Actions_Autofill;
    private readonly InputAction m_Actions_Enter;
    private readonly InputAction m_Actions_Toggle;
    private readonly InputAction m_Actions_Up;
    private readonly InputAction m_Actions_Down;
    public struct ActionsActions
    {
        private @ConsoleControls m_Wrapper;
        public ActionsActions(@ConsoleControls wrapper) { m_Wrapper = wrapper; }
        public InputAction @Autofill => m_Wrapper.m_Actions_Autofill;
        public InputAction @Enter => m_Wrapper.m_Actions_Enter;
        public InputAction @Toggle => m_Wrapper.m_Actions_Toggle;
        public InputAction @Up => m_Wrapper.m_Actions_Up;
        public InputAction @Down => m_Wrapper.m_Actions_Down;
        public InputActionMap Get() { return m_Wrapper.m_Actions; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ActionsActions set) { return set.Get(); }
        public void SetCallbacks(IActionsActions instance)
        {
            if (m_Wrapper.m_ActionsActionsCallbackInterface != null)
            {
                @Autofill.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnAutofill;
                @Autofill.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnAutofill;
                @Autofill.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnAutofill;
                @Enter.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnEnter;
                @Enter.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnEnter;
                @Enter.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnEnter;
                @Toggle.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnToggle;
                @Toggle.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnToggle;
                @Toggle.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnToggle;
                @Up.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnUp;
                @Up.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnUp;
                @Up.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnUp;
                @Down.started -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDown;
                @Down.performed -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDown;
                @Down.canceled -= m_Wrapper.m_ActionsActionsCallbackInterface.OnDown;
            }
            m_Wrapper.m_ActionsActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Autofill.started += instance.OnAutofill;
                @Autofill.performed += instance.OnAutofill;
                @Autofill.canceled += instance.OnAutofill;
                @Enter.started += instance.OnEnter;
                @Enter.performed += instance.OnEnter;
                @Enter.canceled += instance.OnEnter;
                @Toggle.started += instance.OnToggle;
                @Toggle.performed += instance.OnToggle;
                @Toggle.canceled += instance.OnToggle;
                @Up.started += instance.OnUp;
                @Up.performed += instance.OnUp;
                @Up.canceled += instance.OnUp;
                @Down.started += instance.OnDown;
                @Down.performed += instance.OnDown;
                @Down.canceled += instance.OnDown;
            }
        }
    }
    public ActionsActions @Actions => new ActionsActions(this);
    public interface IActionsActions
    {
        void OnAutofill(InputAction.CallbackContext context);
        void OnEnter(InputAction.CallbackContext context);
        void OnToggle(InputAction.CallbackContext context);
        void OnUp(InputAction.CallbackContext context);
        void OnDown(InputAction.CallbackContext context);
    }
}
