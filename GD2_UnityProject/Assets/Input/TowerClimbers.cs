// GENERATED AUTOMATICALLY FROM 'Assets/Input/TowerClimbers.inputactions'

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public class @TowerClimbers : IInputActionCollection, IDisposable
{
    public InputActionAsset asset { get; }
    public @TowerClimbers()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""TowerClimbers"",
    ""maps"": [
        {
            ""name"": ""Climber"",
            ""id"": ""d71b4317-dbe1-4f07-9cdc-ecf00444f023"",
            ""actions"": [
                {
                    ""name"": ""Move"",
                    ""type"": ""Value"",
                    ""id"": ""524ab0df-a8ed-4938-abee-5e21559fca35"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""93f8d1bf-ebff-4ea8-8018-737127390b4f"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Place Ladder"",
                    ""type"": ""Button"",
                    ""id"": ""940abd4d-46dc-4a81-a27a-4a710d02e5a3"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""ClimbLadder"",
                    ""type"": ""Value"",
                    ""id"": ""57ce97f3-f235-4baa-8ab7-8d66edec87f6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""PushBlock"",
                    ""type"": ""Button"",
                    ""id"": ""e834b0a9-0b30-499e-83b5-85b9fcc27dd9"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenOptions"",
                    ""type"": ""Button"",
                    ""id"": ""13ae6585-d22c-4ee2-a84f-f7789d4f5e9e"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""978bfe49-cc26-4a3d-ab7b-7d7a29327403"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""143bb1cd-cc10-4eca-a2f0-a3664166fe91"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""5e9832ad-1ee8-45a2-812a-1f4fb6425319"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""af7be050-996a-4652-b7c7-d701e3ea4dc3"",
                    ""path"": ""<Gamepad>/buttonWest"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Place Ladder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f67b721-c294-418d-8e89-16a37fe9473c"",
                    ""path"": ""<Keyboard>/q"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Place Ladder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""54c5121d-b209-4359-975f-a218681f8b23"",
                    ""path"": ""<Gamepad>/leftStick/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""ClimbLadder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""A/D"",
                    ""id"": ""c984946f-efb2-4fbd-9c1c-e69bf9a370b9"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""ddb54390-b3d9-411c-b53f-415c0d271d67"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""42332cbd-6c4a-44ee-b6b3-c8c223fcbf0e"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""W/S"",
                    ""id"": ""9313b485-a667-4b2b-b0bb-c4e89c629a36"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""ClimbLadder"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""adadd2fb-e689-4fea-9e06-6ce4cd412546"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ClimbLadder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""0fff01bf-3e4e-4179-b7c3-faaf2f2bd90f"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""ClimbLadder"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""55054bb9-5041-4f0e-bdc2-3eb5c09c688d"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""PushBlock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""08eb89e7-a4a6-4731-be9d-ca27fd4d05bc"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""PushBlock"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""40a981ef-f480-426b-8f7b-d1666d1f3d1f"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""OpenOptions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""0f98c629-1256-41d3-ad2f-92679d3255b7"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""OpenOptions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""UI"",
            ""id"": ""78d058cb-a331-4754-9074-86905cb411c4"",
            ""actions"": [
                {
                    ""name"": ""Navigate"",
                    ""type"": ""PassThrough"",
                    ""id"": ""ce38e218-aee9-4e46-9e51-e2ebada731a6"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Submit"",
                    ""type"": ""PassThrough"",
                    ""id"": ""25ccf795-c536-4068-a93f-44bd02b0baec"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Cancel"",
                    ""type"": ""PassThrough"",
                    ""id"": ""44dab2c9-2d1d-44e3-bbf3-c497a7541941"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Join"",
                    ""type"": ""PassThrough"",
                    ""id"": ""c297fb47-2b69-413b-ab22-f0b767633097"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press""
                }
            ],
            ""bindings"": [
                {
                    ""name"": ""Gamepad"",
                    ""id"": ""809f371f-c5e2-4e7a-83a1-d867598f40dd"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""14a5d6e8-4aaf-4119-a9ef-34b8c2c548bf"",
                    ""path"": ""<Gamepad>/leftStick/up"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2db08d65-c5fb-421b-983f-c71163608d67"",
                    ""path"": ""<Gamepad>/leftStick/down"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""8ba04515-75aa-45de-966d-393d9bbd1c14"",
                    ""path"": ""<Gamepad>/leftStick/left"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""fcd248ae-a788-4676-a12e-f4d81205600b"",
                    ""path"": ""<Gamepad>/leftStick/right"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""fb8277d4-c5cd-4663-9dc7-ee3f0b506d90"",
                    ""path"": ""<Gamepad>/dpad"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": "";Gamepad"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""Keyboard"",
                    ""id"": ""ff527021-f211-4c02-933e-5976594c46ed"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Navigate"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""563fbfdd-0f09-408d-aa75-8642c4f08ef0"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""up"",
                    ""id"": ""eb480147-c587-4a33-85ed-eb0ab9942c43"",
                    ""path"": ""<Keyboard>/upArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""2bf42165-60bc-42ca-8072-8c13ab40239b"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""85d264ad-e0a0-4565-b7ff-1a37edde51ac"",
                    ""path"": ""<Keyboard>/downArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""74214943-c580-44e4-98eb-ad7eebe17902"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""cea9b045-a000-445b-95b8-0c171af70a3b"",
                    ""path"": ""<Keyboard>/leftArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""8607c725-d935-4808-84b1-8354e29bab63"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""4cda81dc-9edd-4e03-9d7c-a71a14345d0b"",
                    ""path"": ""<Keyboard>/rightArrow"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Navigate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""9e92bb26-7e3b-4ec4-b06b-3c8f8e498ddc"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""3f1b3613-f609-44a0-bdc3-196a97c9e4b5"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Submit"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""82627dcc-3b13-4ba9-841d-e4b746d6553e"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""e5db7598-fe73-4075-811b-50894fc8e47f"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Cancel"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""f2bbd9b8-e144-479d-a349-028df7c81723"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Join"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""15d8e761-480b-43a5-99a1-a18e0cab46af"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Join"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""Overlord"",
            ""id"": ""e3dabeda-d3dc-4424-892b-86d5299a65f2"",
            ""actions"": [
                {
                    ""name"": ""Move Hand"",
                    ""type"": ""Value"",
                    ""id"": ""491360b6-e89f-434d-949b-4546812c345e"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Drop Block"",
                    ""type"": ""Button"",
                    ""id"": ""e41371db-698a-4911-a4d1-34e904dff476"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""Fast Drop Block"",
                    ""type"": ""Button"",
                    ""id"": ""5602630d-4525-4dad-bec0-0bb5681ddfae"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                },
                {
                    ""name"": ""OpenOptions"",
                    ""type"": ""Button"",
                    ""id"": ""04f524bf-66c0-4023-96a1-78c680914661"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """"
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""4f167e83-214d-4fae-9fc6-b951b1fc6b19"",
                    ""path"": ""<Gamepad>/leftStick/x"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Move Hand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""A/D"",
                    ""id"": ""251ef680-6da0-4217-974a-a253cf6fb2a8"",
                    ""path"": ""1DAxis"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Move Hand"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""negative"",
                    ""id"": ""efc1f46a-9521-496c-bedc-27f70c37e092"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move Hand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""positive"",
                    ""id"": ""9bc03e4c-771f-4eb5-ac66-33adf8c7fed4"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Move Hand"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""76af9229-87e5-49e4-a760-bc76ef58ac58"",
                    ""path"": ""<Gamepad>/buttonSouth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Drop Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""a61c34fe-ca16-4c5d-9da6-d26711b1189d"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Drop Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c07e62a3-0776-4af3-bbe7-e8177cf63d20"",
                    ""path"": ""<Gamepad>/buttonEast"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""Fast Drop Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""92fb6d3d-4eea-477e-87d8-99363839ea7f"",
                    ""path"": ""<Keyboard>/enter"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""Fast Drop Block"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""485617ee-168f-4010-bbf1-8deab3fd4e5f"",
                    ""path"": ""<Gamepad>/buttonNorth"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Gamepad"",
                    ""action"": ""OpenOptions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""98c85570-04d2-400e-a208-47c46716b6b8"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": ""Keyboard&Mouse"",
                    ""action"": ""OpenOptions"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": [
        {
            ""name"": ""Keyboard&Mouse"",
            ""bindingGroup"": ""Keyboard&Mouse"",
            ""devices"": [
                {
                    ""devicePath"": ""<Keyboard>"",
                    ""isOptional"": false,
                    ""isOR"": false
                },
                {
                    ""devicePath"": ""<Mouse>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Gamepad"",
            ""bindingGroup"": ""Gamepad"",
            ""devices"": [
                {
                    ""devicePath"": ""<Gamepad>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Touch"",
            ""bindingGroup"": ""Touch"",
            ""devices"": [
                {
                    ""devicePath"": ""<Touchscreen>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""Joystick"",
            ""bindingGroup"": ""Joystick"",
            ""devices"": [
                {
                    ""devicePath"": ""<Joystick>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        },
        {
            ""name"": ""XR"",
            ""bindingGroup"": ""XR"",
            ""devices"": [
                {
                    ""devicePath"": ""<XRController>"",
                    ""isOptional"": false,
                    ""isOR"": false
                }
            ]
        }
    ]
}");
        // Climber
        m_Climber = asset.FindActionMap("Climber", throwIfNotFound: true);
        m_Climber_Move = m_Climber.FindAction("Move", throwIfNotFound: true);
        m_Climber_Jump = m_Climber.FindAction("Jump", throwIfNotFound: true);
        m_Climber_PlaceLadder = m_Climber.FindAction("Place Ladder", throwIfNotFound: true);
        m_Climber_ClimbLadder = m_Climber.FindAction("ClimbLadder", throwIfNotFound: true);
        m_Climber_PushBlock = m_Climber.FindAction("PushBlock", throwIfNotFound: true);
        m_Climber_OpenOptions = m_Climber.FindAction("OpenOptions", throwIfNotFound: true);
        // UI
        m_UI = asset.FindActionMap("UI", throwIfNotFound: true);
        m_UI_Navigate = m_UI.FindAction("Navigate", throwIfNotFound: true);
        m_UI_Submit = m_UI.FindAction("Submit", throwIfNotFound: true);
        m_UI_Cancel = m_UI.FindAction("Cancel", throwIfNotFound: true);
        m_UI_Join = m_UI.FindAction("Join", throwIfNotFound: true);
        // Overlord
        m_Overlord = asset.FindActionMap("Overlord", throwIfNotFound: true);
        m_Overlord_MoveHand = m_Overlord.FindAction("Move Hand", throwIfNotFound: true);
        m_Overlord_DropBlock = m_Overlord.FindAction("Drop Block", throwIfNotFound: true);
        m_Overlord_FastDropBlock = m_Overlord.FindAction("Fast Drop Block", throwIfNotFound: true);
        m_Overlord_OpenOptions = m_Overlord.FindAction("OpenOptions", throwIfNotFound: true);
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

    // Climber
    private readonly InputActionMap m_Climber;
    private IClimberActions m_ClimberActionsCallbackInterface;
    private readonly InputAction m_Climber_Move;
    private readonly InputAction m_Climber_Jump;
    private readonly InputAction m_Climber_PlaceLadder;
    private readonly InputAction m_Climber_ClimbLadder;
    private readonly InputAction m_Climber_PushBlock;
    private readonly InputAction m_Climber_OpenOptions;
    public struct ClimberActions
    {
        private @TowerClimbers m_Wrapper;
        public ClimberActions(@TowerClimbers wrapper) { m_Wrapper = wrapper; }
        public InputAction @Move => m_Wrapper.m_Climber_Move;
        public InputAction @Jump => m_Wrapper.m_Climber_Jump;
        public InputAction @PlaceLadder => m_Wrapper.m_Climber_PlaceLadder;
        public InputAction @ClimbLadder => m_Wrapper.m_Climber_ClimbLadder;
        public InputAction @PushBlock => m_Wrapper.m_Climber_PushBlock;
        public InputAction @OpenOptions => m_Wrapper.m_Climber_OpenOptions;
        public InputActionMap Get() { return m_Wrapper.m_Climber; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(ClimberActions set) { return set.Get(); }
        public void SetCallbacks(IClimberActions instance)
        {
            if (m_Wrapper.m_ClimberActionsCallbackInterface != null)
            {
                @Move.started -= m_Wrapper.m_ClimberActionsCallbackInterface.OnMove;
                @Move.performed -= m_Wrapper.m_ClimberActionsCallbackInterface.OnMove;
                @Move.canceled -= m_Wrapper.m_ClimberActionsCallbackInterface.OnMove;
                @Jump.started -= m_Wrapper.m_ClimberActionsCallbackInterface.OnJump;
                @Jump.performed -= m_Wrapper.m_ClimberActionsCallbackInterface.OnJump;
                @Jump.canceled -= m_Wrapper.m_ClimberActionsCallbackInterface.OnJump;
                @PlaceLadder.started -= m_Wrapper.m_ClimberActionsCallbackInterface.OnPlaceLadder;
                @PlaceLadder.performed -= m_Wrapper.m_ClimberActionsCallbackInterface.OnPlaceLadder;
                @PlaceLadder.canceled -= m_Wrapper.m_ClimberActionsCallbackInterface.OnPlaceLadder;
                @ClimbLadder.started -= m_Wrapper.m_ClimberActionsCallbackInterface.OnClimbLadder;
                @ClimbLadder.performed -= m_Wrapper.m_ClimberActionsCallbackInterface.OnClimbLadder;
                @ClimbLadder.canceled -= m_Wrapper.m_ClimberActionsCallbackInterface.OnClimbLadder;
                @PushBlock.started -= m_Wrapper.m_ClimberActionsCallbackInterface.OnPushBlock;
                @PushBlock.performed -= m_Wrapper.m_ClimberActionsCallbackInterface.OnPushBlock;
                @PushBlock.canceled -= m_Wrapper.m_ClimberActionsCallbackInterface.OnPushBlock;
                @OpenOptions.started -= m_Wrapper.m_ClimberActionsCallbackInterface.OnOpenOptions;
                @OpenOptions.performed -= m_Wrapper.m_ClimberActionsCallbackInterface.OnOpenOptions;
                @OpenOptions.canceled -= m_Wrapper.m_ClimberActionsCallbackInterface.OnOpenOptions;
            }
            m_Wrapper.m_ClimberActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Move.started += instance.OnMove;
                @Move.performed += instance.OnMove;
                @Move.canceled += instance.OnMove;
                @Jump.started += instance.OnJump;
                @Jump.performed += instance.OnJump;
                @Jump.canceled += instance.OnJump;
                @PlaceLadder.started += instance.OnPlaceLadder;
                @PlaceLadder.performed += instance.OnPlaceLadder;
                @PlaceLadder.canceled += instance.OnPlaceLadder;
                @ClimbLadder.started += instance.OnClimbLadder;
                @ClimbLadder.performed += instance.OnClimbLadder;
                @ClimbLadder.canceled += instance.OnClimbLadder;
                @PushBlock.started += instance.OnPushBlock;
                @PushBlock.performed += instance.OnPushBlock;
                @PushBlock.canceled += instance.OnPushBlock;
                @OpenOptions.started += instance.OnOpenOptions;
                @OpenOptions.performed += instance.OnOpenOptions;
                @OpenOptions.canceled += instance.OnOpenOptions;
            }
        }
    }
    public ClimberActions @Climber => new ClimberActions(this);

    // UI
    private readonly InputActionMap m_UI;
    private IUIActions m_UIActionsCallbackInterface;
    private readonly InputAction m_UI_Navigate;
    private readonly InputAction m_UI_Submit;
    private readonly InputAction m_UI_Cancel;
    private readonly InputAction m_UI_Join;
    public struct UIActions
    {
        private @TowerClimbers m_Wrapper;
        public UIActions(@TowerClimbers wrapper) { m_Wrapper = wrapper; }
        public InputAction @Navigate => m_Wrapper.m_UI_Navigate;
        public InputAction @Submit => m_Wrapper.m_UI_Submit;
        public InputAction @Cancel => m_Wrapper.m_UI_Cancel;
        public InputAction @Join => m_Wrapper.m_UI_Join;
        public InputActionMap Get() { return m_Wrapper.m_UI; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(UIActions set) { return set.Get(); }
        public void SetCallbacks(IUIActions instance)
        {
            if (m_Wrapper.m_UIActionsCallbackInterface != null)
            {
                @Navigate.started -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Navigate.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnNavigate;
                @Submit.started -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Submit.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnSubmit;
                @Cancel.started -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Cancel.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnCancel;
                @Join.started -= m_Wrapper.m_UIActionsCallbackInterface.OnJoin;
                @Join.performed -= m_Wrapper.m_UIActionsCallbackInterface.OnJoin;
                @Join.canceled -= m_Wrapper.m_UIActionsCallbackInterface.OnJoin;
            }
            m_Wrapper.m_UIActionsCallbackInterface = instance;
            if (instance != null)
            {
                @Navigate.started += instance.OnNavigate;
                @Navigate.performed += instance.OnNavigate;
                @Navigate.canceled += instance.OnNavigate;
                @Submit.started += instance.OnSubmit;
                @Submit.performed += instance.OnSubmit;
                @Submit.canceled += instance.OnSubmit;
                @Cancel.started += instance.OnCancel;
                @Cancel.performed += instance.OnCancel;
                @Cancel.canceled += instance.OnCancel;
                @Join.started += instance.OnJoin;
                @Join.performed += instance.OnJoin;
                @Join.canceled += instance.OnJoin;
            }
        }
    }
    public UIActions @UI => new UIActions(this);

    // Overlord
    private readonly InputActionMap m_Overlord;
    private IOverlordActions m_OverlordActionsCallbackInterface;
    private readonly InputAction m_Overlord_MoveHand;
    private readonly InputAction m_Overlord_DropBlock;
    private readonly InputAction m_Overlord_FastDropBlock;
    private readonly InputAction m_Overlord_OpenOptions;
    public struct OverlordActions
    {
        private @TowerClimbers m_Wrapper;
        public OverlordActions(@TowerClimbers wrapper) { m_Wrapper = wrapper; }
        public InputAction @MoveHand => m_Wrapper.m_Overlord_MoveHand;
        public InputAction @DropBlock => m_Wrapper.m_Overlord_DropBlock;
        public InputAction @FastDropBlock => m_Wrapper.m_Overlord_FastDropBlock;
        public InputAction @OpenOptions => m_Wrapper.m_Overlord_OpenOptions;
        public InputActionMap Get() { return m_Wrapper.m_Overlord; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(OverlordActions set) { return set.Get(); }
        public void SetCallbacks(IOverlordActions instance)
        {
            if (m_Wrapper.m_OverlordActionsCallbackInterface != null)
            {
                @MoveHand.started -= m_Wrapper.m_OverlordActionsCallbackInterface.OnMoveHand;
                @MoveHand.performed -= m_Wrapper.m_OverlordActionsCallbackInterface.OnMoveHand;
                @MoveHand.canceled -= m_Wrapper.m_OverlordActionsCallbackInterface.OnMoveHand;
                @DropBlock.started -= m_Wrapper.m_OverlordActionsCallbackInterface.OnDropBlock;
                @DropBlock.performed -= m_Wrapper.m_OverlordActionsCallbackInterface.OnDropBlock;
                @DropBlock.canceled -= m_Wrapper.m_OverlordActionsCallbackInterface.OnDropBlock;
                @FastDropBlock.started -= m_Wrapper.m_OverlordActionsCallbackInterface.OnFastDropBlock;
                @FastDropBlock.performed -= m_Wrapper.m_OverlordActionsCallbackInterface.OnFastDropBlock;
                @FastDropBlock.canceled -= m_Wrapper.m_OverlordActionsCallbackInterface.OnFastDropBlock;
                @OpenOptions.started -= m_Wrapper.m_OverlordActionsCallbackInterface.OnOpenOptions;
                @OpenOptions.performed -= m_Wrapper.m_OverlordActionsCallbackInterface.OnOpenOptions;
                @OpenOptions.canceled -= m_Wrapper.m_OverlordActionsCallbackInterface.OnOpenOptions;
            }
            m_Wrapper.m_OverlordActionsCallbackInterface = instance;
            if (instance != null)
            {
                @MoveHand.started += instance.OnMoveHand;
                @MoveHand.performed += instance.OnMoveHand;
                @MoveHand.canceled += instance.OnMoveHand;
                @DropBlock.started += instance.OnDropBlock;
                @DropBlock.performed += instance.OnDropBlock;
                @DropBlock.canceled += instance.OnDropBlock;
                @FastDropBlock.started += instance.OnFastDropBlock;
                @FastDropBlock.performed += instance.OnFastDropBlock;
                @FastDropBlock.canceled += instance.OnFastDropBlock;
                @OpenOptions.started += instance.OnOpenOptions;
                @OpenOptions.performed += instance.OnOpenOptions;
                @OpenOptions.canceled += instance.OnOpenOptions;
            }
        }
    }
    public OverlordActions @Overlord => new OverlordActions(this);
    private int m_KeyboardMouseSchemeIndex = -1;
    public InputControlScheme KeyboardMouseScheme
    {
        get
        {
            if (m_KeyboardMouseSchemeIndex == -1) m_KeyboardMouseSchemeIndex = asset.FindControlSchemeIndex("Keyboard&Mouse");
            return asset.controlSchemes[m_KeyboardMouseSchemeIndex];
        }
    }
    private int m_GamepadSchemeIndex = -1;
    public InputControlScheme GamepadScheme
    {
        get
        {
            if (m_GamepadSchemeIndex == -1) m_GamepadSchemeIndex = asset.FindControlSchemeIndex("Gamepad");
            return asset.controlSchemes[m_GamepadSchemeIndex];
        }
    }
    private int m_TouchSchemeIndex = -1;
    public InputControlScheme TouchScheme
    {
        get
        {
            if (m_TouchSchemeIndex == -1) m_TouchSchemeIndex = asset.FindControlSchemeIndex("Touch");
            return asset.controlSchemes[m_TouchSchemeIndex];
        }
    }
    private int m_JoystickSchemeIndex = -1;
    public InputControlScheme JoystickScheme
    {
        get
        {
            if (m_JoystickSchemeIndex == -1) m_JoystickSchemeIndex = asset.FindControlSchemeIndex("Joystick");
            return asset.controlSchemes[m_JoystickSchemeIndex];
        }
    }
    private int m_XRSchemeIndex = -1;
    public InputControlScheme XRScheme
    {
        get
        {
            if (m_XRSchemeIndex == -1) m_XRSchemeIndex = asset.FindControlSchemeIndex("XR");
            return asset.controlSchemes[m_XRSchemeIndex];
        }
    }
    public interface IClimberActions
    {
        void OnMove(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnPlaceLadder(InputAction.CallbackContext context);
        void OnClimbLadder(InputAction.CallbackContext context);
        void OnPushBlock(InputAction.CallbackContext context);
        void OnOpenOptions(InputAction.CallbackContext context);
    }
    public interface IUIActions
    {
        void OnNavigate(InputAction.CallbackContext context);
        void OnSubmit(InputAction.CallbackContext context);
        void OnCancel(InputAction.CallbackContext context);
        void OnJoin(InputAction.CallbackContext context);
    }
    public interface IOverlordActions
    {
        void OnMoveHand(InputAction.CallbackContext context);
        void OnDropBlock(InputAction.CallbackContext context);
        void OnFastDropBlock(InputAction.CallbackContext context);
        void OnOpenOptions(InputAction.CallbackContext context);
    }
}
