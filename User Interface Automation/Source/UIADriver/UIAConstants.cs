using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Provider;
using System.Windows.Forms;
using System.Diagnostics;
using System.Threading;
using System.IO;
using UITesting.Automated.WindowsInput;
using UITesting.Automated.ControlInf;
using UITesting.Automated.JSonSerializer;

namespace UITesting.Automated.UIADriver
{
    public interface IUIAConstants
    {
        int WindowInteractionState_Running
        {
            get;
        }

        int WindowInteractionState_Closing
        {
            get;
        }

        int WindowInteractionState_ReadyForUserInteraction
        {
            get;
        }

        int WindowInteractionState_BlockedByModalWindow
        {
            get;
        }

        int WindowInteractionState_NotResponding
        {
            get;
        }


        int WindowVisualState_Normal
        {
            get;
        }

        int WindowVisualState_Maximized
        {
            get;
        }

        int WindowVisualState_Minimized
        {
            get;
        }


        int ToggleState_Off
        {
            get;
        }

        int ToggleState_On
        {
            get;
        }

        int ToggleState_Indeterminate
        {
            get;
        }

        int RowOrColumnMajor_RowMajor
        {
            get;
        }

        int RowOrColumnMajor_ColumnMajor
        {
            get;
        }

        int RowOrColumnMajor_Indeterminate
        {
            get;
        }


        int ExpandCollapseState_Collapsed
        {
            get;
        }

        int ExpandCollapseState_Expanded
        {
            get;
        }

        int ExpandCollapseState_PartiallyExpanded
        {
            get;
        }

        int ExpandCollapseState_LeafNode
        {
            get;
        }

        int ScrollAmount_LargeDecrement
        {
            get;
        }

        int ScrollAmount_SmallDecrement
        {
            get;
        }

        int ScrollAmount_NoAmount
        {
            get;
        }

        int ScrollAmount_LargeIncrement
        {
            get;
        }

        int ScrollAmount_SmallIncrement
        {
            get;
        }

        int AutomationProperty_AcceleratorKey
        {
            get ;
        }

        int AutomationProperty_AccessKey
        {
            get ;
        }

        int AutomationProperty_AutomationId
        {
            get ;
        }

        int AutomationProperty_BoundingRectangle
        {
            get ;
        }

        int AutomationProperty_ClassName
        {
            get ;
        }

        int AutomationProperty_ClickablePoint
        {
            get ;
        }

        int AutomationProperty_ControlType
        {
            get ;
        }

        int AutomationProperty_Culture
        {
            get ;
        }

        int AutomationProperty_FrameworkId
        {
            get ;
        }

        int AutomationProperty_HasKeyboardFocus
        {
            get ;
        }

        int AutomationProperty_HelpText
        {
            get ;
        }

        int AutomationProperty_IsContentElement
        {
            get ;
        }

        int AutomationProperty_IsControlElement
        {
            get ;
        }

        int AutomationProperty_IsDockPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsEnabled
        {
            get ;
        }

        int AutomationProperty_IsExpandCollapsePatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsGridItemPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsGridPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsKeyboardFocusable
        {
            get ;
        }

        int AutomationProperty_IsMultipleViewPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsOffscreen
        {
            get ;
        }

        int AutomationProperty_IsPassword
        {
            get ;
        }

        int AutomationProperty_IsRangeValuePatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsRequiredForForm
        {
            get ;
        }

        int AutomationProperty_IsScrollItemPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsScrollPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsSelectionItemPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsSelectionPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsTableItemPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsTablePatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsTextPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsTogglePatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsTransformPatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsValuePatternAvailable
        {
            get ;
        }

        int AutomationProperty_IsWindowPatternAvailable
        {
            get ;
        }

        int AutomationProperty_ItemStatus
        {
            get ;
        }

        int AutomationProperty_ItemType
        {
            get ;
        }

        int AutomationProperty_LabeledBy
        {
            get ;
        }

        int AutomationProperty_LocalizedControlType
        {
            get ;
        }

        int AutomationProperty_Name
        {
            get ;
        }

        int AutomationProperty_NativeWindowHandle
        {
            get ;
        }

        int AutomationProperty_Orientation
        {
            get ;
        }

        int AutomationProperty_ProcessId
        {
            get ;
        }

        int AutomationProperty_RuntimeId
        {
            get ;
        }

        string SearchOptions_UseValue
        {
            get;
        }

        string SearchOptions_UseAutomationId
        {
            get;
        }

        string SearchOptions_UsePath
        {
            get;
        }

        string SearchOptions_UseClickPoint
        {
            get;
        }

        string SearchOptions_SearchTree
        {
            get;
        }

        string SearchOptions_Default
        {
            get;
        }

        int MSAASTATE_SYSTEM_UNAVAILABLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_SELECTED
        {
            get;
        }

        int MSAASTATE_SYSTEM_FOCUSED
        {
            get;
        }

        int MSAASTATE_SYSTEM_PRESSED
        {
            get;
        }

        int MSAASTATE_SYSTEM_CHECKED
        {
            get;
        }

        int MSAASTATE_SYSTEM_MIXED
        {
            get;
        }

        int MSAASTATE_SYSTEM_INDETERMINATE
        {
            get;
        }

        int MSAASTATE_SYSTEM_READONLY
        {
            get;
        }

        int MSAASTATE_SYSTEM_HOTTRACKED
        {
            get;
        }

        int MSAASTATE_SYSTEM_DEFAULT
        {
            get;
        }

        int MSAASTATE_SYSTEM_EXPANDED
        {
            get;
        }

        int MSAASTATE_SYSTEM_COLLAPSED
        {
            get;
        }

        int MSAASTATE_SYSTEM_BUSY
        {
            get;
        }

        int MSAASTATE_SYSTEM_FLOATING
        {
            get;
        }

        int MSAASTATE_SYSTEM_MARQUEED
        {
            get;
        }

        int MSAASTATE_SYSTEM_ANIMATED
        {
            get;
        }

        int MSAASTATE_SYSTEM_INVISIBLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_OFFSCREEN
        {
            get;
        }

        int MSAASTATE_SYSTEM_SIZEABLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_MOVEABLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_SELFVOICING
        {
            get;
        }

        int MSAASTATE_SYSTEM_FOCUSABLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_SELECTABLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_LINKED
        {
            get;
        }

        int MSAASTATE_SYSTEM_TRAVERSED
        {
            get;
        }

        int MSAASTATE_SYSTEM_MULTISELECTABLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_EXTSELECTABLE
        {
            get;
        }

        int MSAASTATE_SYSTEM_ALERT_LOW
        {
            get;
        }

        int MSAASTATE_SYSTEM_ALERT_MEDIUM
        {
            get;
        }

        int MSAASTATE_SYSTEM_ALERT_HIGH
        {
            get;
        }

        int MSAASTATE_SYSTEM_PROTECTED
        {
            get;
        }

        int MSAASTATE_SYSTEM_HAS_POPUP
        {
            get;
        }

        int MSAASTATE_SYSTEM_VALID
        {
            get;
        }

        int MSAASELFLAG_NONE
        {
            get;
        }

        int MSAASELFLAG_TAKEFOCUS
        {
            get;
        }

        int MSAASELFLAG_TAKESELECTION
        {
            get;
        }

        int MSAASELFLAG_EXTENDSELECTION
        {
            get;
        }

        int MSAASELFLAG_ADDSELECTION
        {
            get;
        }

        int MSAASELFLAG_REMOVESELECTION
        {
            get;
        }

    }

    class UIAConstants : IUIAConstants
    {
        public int WindowInteractionState_Running
        {
            get { return 0; }
        }

        public int WindowInteractionState_Closing
        {
            get { return 1; }
        }

        public int WindowInteractionState_ReadyForUserInteraction
        {
            get { return 2; }
        }

        public int WindowInteractionState_BlockedByModalWindow
        {
            get { return 3; }
        }

        public int WindowInteractionState_NotResponding
        {
            get { return 0; }
        }

        public int WindowVisualState_Normal
        {
            get { return 1; }
        }

        public int WindowVisualState_Maximized
        {
            get { return 2; }
        }

        public int WindowVisualState_Minimized
        {
            get { return 3; }
        }

        public int ToggleState_Off
        {
            get { return 0; }
        }

        public int ToggleState_On
        {
            get { return 1; }
        }

        public int ToggleState_Indeterminate
        {
            get { return 2; }
        }

        public int RowOrColumnMajor_RowMajor
        {
            get { return 0; }
        }

        public int RowOrColumnMajor_ColumnMajor
        {
            get { return 1; }
        }

        public int RowOrColumnMajor_Indeterminate
        {
            get { return 2; }
        }

        public int ExpandCollapseState_Collapsed
        {
            get { return 0; }
        }

        public int ExpandCollapseState_Expanded
        {
            get { return 1; }
        }

        public int ExpandCollapseState_PartiallyExpanded
        {
            get { return 2; }
        }

        public int ExpandCollapseState_LeafNode
        {
            get { return 3; }
        }


        public int ScrollAmount_LargeDecrement
        {
            get { return 0; }
        }

        public int ScrollAmount_SmallDecrement
        {
            get { return 1; }
        }

        public int ScrollAmount_NoAmount
        {
            get { return 2; }
        }

        public int ScrollAmount_LargeIncrement
        {
            get { return 3; }
        }

        public int ScrollAmount_SmallIncrement
        {
            get { return 4; }
        }

        public int MSAASTATE_SYSTEM_UNAVAILABLE
        {
            get { return 0x00000001; }
        }

        public int MSAASTATE_SYSTEM_SELECTED
        {
            get { return 0x00000002; }
        }

        public int MSAASTATE_SYSTEM_FOCUSED
        {
            get { return 0x00000008; }
        }

        public int MSAASTATE_SYSTEM_PRESSED
        {
            get { return 0x00000010; }
        }

        public int MSAASTATE_SYSTEM_CHECKED
        {
            get { return 0x00000020; }
        }

        public int MSAASTATE_SYSTEM_MIXED
        {
            get { return 0x00000020; }
        }

        public int MSAASTATE_SYSTEM_INDETERMINATE
        {
            get { return 0x00000020; }
        }

        public int MSAASTATE_SYSTEM_READONLY
        {
            get { return 0x00000040; }
        }

        public int MSAASTATE_SYSTEM_HOTTRACKED
        {
            get { return 0x00000080; }
        }

        public int MSAASTATE_SYSTEM_DEFAULT
        {
            get { return 0x00000100; }
        }

        public int MSAASTATE_SYSTEM_EXPANDED
        {
            get { return 0x00000200; }
        }

        public int MSAASTATE_SYSTEM_COLLAPSED
        {
            get { return 0x00000400; }
        }

        public int MSAASTATE_SYSTEM_BUSY
        {
            get { return 0x00000800; }
        }

        public int MSAASTATE_SYSTEM_FLOATING
        {
            get { return 0x00001000; }
        }

        public int MSAASTATE_SYSTEM_MARQUEED
        {
            get { return 0x00002000; }
        }

        public int MSAASTATE_SYSTEM_ANIMATED
        {
            get { return 0x00004000; }
        }

        public int MSAASTATE_SYSTEM_INVISIBLE
        {
            get { return 0x00008000; }
        }

        public int MSAASTATE_SYSTEM_OFFSCREEN
        {
            get { return 0x00010000; }
        }

        public int MSAASTATE_SYSTEM_SIZEABLE
        {
            get { return 0x00020000; }
        }

        public int MSAASTATE_SYSTEM_MOVEABLE
        {
            get { return 0x00040000; }
        }

        public int MSAASTATE_SYSTEM_SELFVOICING
        {
            get { return 0x00080000; }
        }

        public int MSAASTATE_SYSTEM_FOCUSABLE
        {
            get { return 0x00100000; }
        }

        public int MSAASTATE_SYSTEM_SELECTABLE
        {
            get { return 0x00200000; }
        }

        public int MSAASTATE_SYSTEM_LINKED
        {
            get { return 0x00400000; }
        }

        public int MSAASTATE_SYSTEM_TRAVERSED
        {
            get { return 0x00800000; }
        }

        public int MSAASTATE_SYSTEM_MULTISELECTABLE
        {
            get { return 0x01000000; }
        }

        public int MSAASTATE_SYSTEM_EXTSELECTABLE
        {
            get { return 0x02000000; }
        }

        public int MSAASTATE_SYSTEM_ALERT_LOW
        {
            get { return 0x04000000; }
        }

        public int MSAASTATE_SYSTEM_ALERT_MEDIUM
        {
            get { return 0x08000000; }
        }

        public int MSAASTATE_SYSTEM_ALERT_HIGH
        {
            get { return 0x10000000; }
        }

        public int MSAASTATE_SYSTEM_PROTECTED
        {
            get { return 0x20000000; }
        }

        public int MSAASTATE_SYSTEM_HAS_POPUP
        {
            get { return 0x40000000; }
        }

        public int MSAASTATE_SYSTEM_VALID
        {
            get { return 0x3FFFFFFF; }
        }


        public int AutomationProperty_AcceleratorKey
        {
            get { return 30006; }
        }

        public int AutomationProperty_AccessKey
        {
            get { return 30007; }
        }

        public int AutomationProperty_AutomationId
        {
            get { return 30011; }
        }

        public int AutomationProperty_BoundingRectangle
        {
            get { return 30001; }
        }

        public int AutomationProperty_ClassName
        {
            get { return 30012; }
        }

        public int AutomationProperty_ClickablePoint
        {
            get { return 30014; }
        }

        public int AutomationProperty_ControlType
        {
            get { return 30003; }
        }

        public int AutomationProperty_Culture
        {
            get { return 30015; }
        }

        public int AutomationProperty_FrameworkId
        {
            get { return 30024; }
        }

        public int AutomationProperty_HasKeyboardFocus
        {
            get { return 30008; }
        }

        public int AutomationProperty_HelpText
        {
            get { return 30013; }
        }

        public int AutomationProperty_IsContentElement
        {
            get { return 30017; }
        }

        public int AutomationProperty_IsControlElement
        {
            get { return 30016; }
        }

        public int AutomationProperty_IsDockPatternAvailable
        {
            get { return 30027; }
        }

        public int AutomationProperty_IsEnabled
        {
            get { return 30010; }
        }

        public int AutomationProperty_IsExpandCollapsePatternAvailable
        {
            get { return 30028; }
        }

        public int AutomationProperty_IsGridItemPatternAvailable
        {
            get { return 30029; }
        }

        public int AutomationProperty_IsGridPatternAvailable
        {
            get { return 30030; }
        }

        public int AutomationProperty_IsKeyboardFocusable
        {
            get { return 30009; }
        }

        public int AutomationProperty_IsMultipleViewPatternAvailable
        {
            get { return 30032; }
        }

        public int AutomationProperty_IsOffscreen
        {
            get { return 30022; }
        }

        public int AutomationProperty_IsPassword
        {
            get { return 30019; }
        }

        public int AutomationProperty_IsRangeValuePatternAvailable
        {
            get { return 30033; }
        }

        public int AutomationProperty_IsRequiredForForm
        {
            get { return 30025; }
        }

        public int AutomationProperty_IsScrollItemPatternAvailable
        {
            get { return 30035; }
        }

        public int AutomationProperty_IsScrollPatternAvailable
        {
            get { return 30034; }
        }

        public int AutomationProperty_IsSelectionItemPatternAvailable
        {
            get { return 30036; }
        }

        public int AutomationProperty_IsSelectionPatternAvailable
        {
            get { return 30037; }
        }

        public int AutomationProperty_IsTableItemPatternAvailable
        {
            get { return 30039; }
        }

        public int AutomationProperty_IsTablePatternAvailable
        {
            get { return 30038; }
        }

        public int AutomationProperty_IsTextPatternAvailable
        {
            get { return 30040; }
        }

        public int AutomationProperty_IsTogglePatternAvailable
        {
            get { return 30041; }
        }

        public int AutomationProperty_IsTransformPatternAvailable
        {
            get { return 30042; }
        }

        public int AutomationProperty_IsValuePatternAvailable
        {
            get { return 30043; }
        }

        public int AutomationProperty_IsWindowPatternAvailable
        {
            get { return 30044; }
        }

        public int AutomationProperty_ItemStatus
        {
            get { return 30026; }
        }

        public int AutomationProperty_ItemType
        {
            get { return 30021; }
        }

        public int AutomationProperty_LabeledBy
        {
            get { return 30018; }
        }

        public int AutomationProperty_LocalizedControlType
        {
            get { return 30004; }
        }

        public int AutomationProperty_Name
        {
            get { return 30005; }
        }

        public int AutomationProperty_NativeWindowHandle
        {
            get { return 30020; }
        }

        public int AutomationProperty_Orientation
        {
            get { return 30023; }
        }

        public int AutomationProperty_ProcessId
        {
            get { return 30002; }
        }

        public int AutomationProperty_RuntimeId
        {
            get { return 30000; }
        }

        public string SearchOptions_UseValue
        {
            get { return "V"; }
        }

        public string SearchOptions_UseAutomationId
        {
            get { return "A"; }
        }

        public string SearchOptions_UsePath
        {
            get { return "P"; }
        }

        public string SearchOptions_UseClickPoint
        {
            get { return "C"; }
        }

        public string SearchOptions_SearchTree
        {
            get { return "S"; }
        }

        public string SearchOptions_Default
        {
            get { return "VAPCS"; }
        }


        public int MSAASELFLAG_NONE
        {
            get {return 0;}
        }

        public int MSAASELFLAG_TAKEFOCUS
        {
            get {return 1;}
        }

        public int MSAASELFLAG_TAKESELECTION
        {
            get {return 2;}
        }

        public int MSAASELFLAG_EXTENDSELECTION
        {
            get {return 4;}
        }

        public int MSAASELFLAG_ADDSELECTION
        {
            get {return 8;}
        }

        public int MSAASELFLAG_REMOVESELECTION
        {
            get {return 16;}
        }
    }

}

