#if !NO_UI_WEB_BROWSER
namespace Internal.Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Internal.Xilium.CefGlue.Interop;
    using Internal.Xilium.CefGlue.Platform;
    using Internal.Xilium.CefGlue.Platform.Windows;

    /// <summary>
    /// Class representing window information.
    /// </summary>
    /// <remarks>
    /// Meanings for handles:
    /// <list type="table">
    ///     <listheader>
    ///         <item>Platform</item>
    ///         <description>Description</description>
    ///     </listheader>
    ///     <item>
    ///         <item>Windows</item>
    ///         <description>Window handle (HWND)</description>
    ///     </item>
    ///     <item>
    ///         <item>Mac OS X</item>
    ///         <description>NSView pointer for the parent view (NSView*)</description>
    ///     </item>
    ///     <item>
    ///         <item>Linux</item>
    ///         <description>Pointer for the new browser widget (GtkWidget*)</description>
    ///     </item>
    /// </list>
    /// </remarks>
    public unsafe abstract class CefWindowInfo
    {
        public static CefWindowInfo Create()
        {
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows: return new CefWindowInfoWindowsImpl();
                case CefRuntimePlatform.Linux: return new CefWindowInfoLinuxImpl();
                case CefRuntimePlatform.MacOSX: return new CefWindowInfoMacImpl();
                default: throw new NotSupportedException();
            }
        }

        internal static CefWindowInfo FromNative(cef_window_info_t* ptr)
        {
            switch (CefRuntime.Platform)
            {
                case CefRuntimePlatform.Windows: return new CefWindowInfoWindowsImpl(ptr);
                case CefRuntimePlatform.Linux: return new CefWindowInfoLinuxImpl(ptr);
                case CefRuntimePlatform.MacOSX: return new CefWindowInfoMacImpl(ptr);
                default: throw new NotSupportedException();
            }
        }

        private bool _own;
        private bool _disposed;

        protected internal CefWindowInfo(bool own)
        {
            _own = own;
        }

        ~CefWindowInfo()
        {
            Dispose();
        }

        internal void Dispose()
        {
            _disposed = true;
            if (_own)
            {
                DisposeNativePointer();
            }
            GC.SuppressFinalize(this);
        }

        internal cef_window_info_t* ToNative()
        {
            var ptr = GetNativePointer();
            _own = false;
            return ptr;
        }

        protected internal void ThrowIfDisposed()
        {
            if (_disposed) throw ExceptionBuilder.ObjectDisposed();
        }

        public bool Disposed { get { return _disposed; } }

        internal abstract cef_window_info_t* GetNativePointer();
        protected internal abstract void DisposeNativePointer();

        // Common properties for all platforms
        /// <summary>
        /// Handle for the parent window.
        /// </summary>
        public abstract IntPtr ParentHandle { get; set; }

        /// <summary>
        /// Handle for the new browser window.
        /// </summary>
        public abstract IntPtr Handle { get; set; }

        // Common properties for windows & macosx
        public abstract string Name { get; set; }
        public abstract int X { get; set; }
        public abstract int Y { get; set; }
        public abstract int Width { get; set; }
        public abstract int Height { get; set; }

        // Windows-specific
        public abstract WindowStyle Style { get; set; }
        public abstract WindowStyleEx StyleEx { get; set; }
        public abstract IntPtr MenuHandle { get; set; }

        // Mac-specific
        public abstract bool Hidden { get; set; }

        /// <summary>
        /// Set to true (1) to create the browser using windowless (off-screen)
        /// rendering. No window will be created for the browser and all rendering will
        /// occur via the CefRenderHandler interface. The |parent_window| value will be
        /// used to identify monitor info and to act as the parent window for dialogs,
        /// context menus, etc. If |parent_window| is not provided then the main screen
        /// monitor will be used and some functionality that requires a parent window
        /// may not function correctly. In order to create windowless browsers the
        /// CefSettings.windowless_rendering_enabled value must be set to true.
        /// </summary>
        public abstract bool WindowlessRenderingEnabled { get; set; }

        /// <summary>
        /// Set to true (1) to enable transparent painting in combination with
        /// windowless rendering. When this value is true a transparent background
        /// color will be used (RGBA=0x00000000). When this value is false the
        /// background will be white and opaque.
        /// </summary>
        public abstract bool TransparentPaintingEnabled { get; set; }

        public void SetAsChild(IntPtr parentHandle, CefRectangle rect)
        {
            ThrowIfDisposed();

            Style = WindowStyle.WS_CHILD
                  | WindowStyle.WS_CLIPCHILDREN
                  | WindowStyle.WS_CLIPSIBLINGS
                  | WindowStyle.WS_TABSTOP
                  | WindowStyle.WS_VISIBLE;

            ParentHandle = parentHandle;

            X = rect.X;
            Y = rect.Y;
            Width = rect.Width;
            Height = rect.Height;
        }

        public void SetAsPopup(IntPtr parentHandle, string name)
        {
            ThrowIfDisposed();

            Style = WindowStyle.WS_OVERLAPPEDWINDOW
                  | WindowStyle.WS_CLIPCHILDREN
                  | WindowStyle.WS_CLIPSIBLINGS
                  | WindowStyle.WS_VISIBLE;

            ParentHandle = parentHandle;

            // CW_USEDEFAULT
            X = int.MinValue;
            Y = int.MinValue;
            Width = int.MinValue;
            Height = int.MinValue;

            Name = name;
        }

        public void SetAsWindowless(IntPtr parentHandle, bool transparent)
        {
            WindowlessRenderingEnabled = true;
            ParentHandle = parentHandle;
            TransparentPaintingEnabled = transparent;
        }
    }
}

#endif