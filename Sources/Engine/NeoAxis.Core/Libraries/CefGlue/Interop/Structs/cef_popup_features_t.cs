#if !NO_UI_WEB_BROWSER
//
// This file manually written from cef/include/internal/cef_types.h.
//
namespace Internal.Xilium.CefGlue.Interop
{
    using System;
    using System.Runtime.InteropServices;

    [StructLayout(LayoutKind.Sequential, Pack = libcef.ALIGN)]
    internal unsafe struct cef_popup_features_t
    {
        public int x;
        public int xSet;
        public int y;
        public int ySet;
        public int width;
        public int widthSet;
        public int height;
        public int heightSet;

        public int menuBarVisible;
        public int statusBarVisible;
        public int toolBarVisible;
        public int locationBarVisible;
        public int scrollbarsVisible;
        public int resizable;

        public int fullscreen;
        public int dialog;
        public cef_string_list* additionalFeatures;
    }
}

#endif