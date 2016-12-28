using System;

namespace Wincognize.Hooking
{
    public enum WindowsHook : Int32
    {
        WH_MSGFILTER = -1,
        WH_JOURNALRECORD,
        WH_JOURNALPLAYBACK,
        WH_KEYBOARD,
        WH_GETMESSAGE,
        WH_CALLWNDPROC,
        WH_CBT,
        WH_SYSMSGFILTER,
        WH_MOUSE,
        WH_DEBUG = 9,
        WH_SHELL,
        WH_FOREGROUNDIDLE,
        WH_CALLWNDPROCRET,
        WH_KEYBOARD_LL,
        WH_MOUSE_LL
    }
}
