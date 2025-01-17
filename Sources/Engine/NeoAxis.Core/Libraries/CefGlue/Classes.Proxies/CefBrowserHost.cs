#if !NO_UI_WEB_BROWSER
namespace Internal.Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Internal.Xilium.CefGlue.Interop;

    /// <summary>
    /// Class used to represent the browser process aspects of a browser window. The
    /// methods of this class can only be called in the browser process. They may be
    /// called on any thread in that process unless otherwise indicated in the
    /// comments.
    /// </summary>
    public sealed unsafe partial class CefBrowserHost
    {
        /// <summary>
        /// Create a new browser window using the window parameters specified by
        /// |windowInfo|. All values will be copied internally and the actual window
        /// will be created on the UI thread. If |request_context| is empty the
        /// global request context will be used. This method can be called on any
        /// browser process thread and will not block.
        /// </summary>
        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url, CefRequestContext requestContext)
        {
            if (windowInfo == null) throw new ArgumentNullException("windowInfo");
            if (client == null) throw new ArgumentNullException("client");
            if (settings == null) throw new ArgumentNullException("settings");
            // TODO: [ApiUsage] if windowInfo.WindowRenderingDisabled && client doesn't provide RenderHandler implementation -> throw exception

            var n_windowInfo = windowInfo.ToNative();
            var n_client = client.ToNative();
            var n_settings = settings.ToNative();
            var n_requestContext = requestContext != null ? requestContext.ToNative() : null;

            fixed (char* url_ptr = url)
            {
                cef_string_t n_url = new cef_string_t(url_ptr, url != null ? url.Length : 0);
                var n_success = cef_browser_host_t.create_browser(n_windowInfo, n_client, &n_url, n_settings, n_requestContext);
                if (n_success != 1) throw ExceptionBuilder.FailedToCreateBrowser();
            }

            // TODO: free n_ structs ?
        }

        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url)
        {
            CreateBrowser(windowInfo, client, settings, url, null);
        }

        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, Uri url, CefRequestContext requestContext)
        {
            CreateBrowser(windowInfo, client, settings, url.ToString(), requestContext);
        }

        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, Uri url)
        {
            CreateBrowser(windowInfo, client, settings, url, null);
        }

        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, CefRequestContext requestContext)
        {
            CreateBrowser(windowInfo, client, settings, string.Empty, requestContext);
        }

        public static void CreateBrowser(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings)
        {
            CreateBrowser(windowInfo, client, settings, string.Empty, null);
        }


        /// <summary>
        /// Create a new browser window using the window parameters specified by
        /// |windowInfo|. If |request_context| is empty the global request context
        /// will be used. This method can only be called on the browser process UI
        /// thread.
        /// </summary>
        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url, CefRequestContext requestContext)
        {
            if (windowInfo == null) throw new ArgumentNullException("windowInfo");
            if (client == null) throw new ArgumentNullException("client");
            if (settings == null) throw new ArgumentNullException("settings");
            // TODO: [ApiUsage] if windowInfo.WindowRenderingDisabled && client doesn't provide RenderHandler implementation -> throw exception

            var n_windowInfo = windowInfo.ToNative();
            var n_client = client.ToNative();
            var n_settings = settings.ToNative();
            var n_requestContext = requestContext != null ? requestContext.ToNative() : null;

            fixed (char* url_ptr = url)
            {
                cef_string_t n_url = new cef_string_t(url_ptr, url != null ? url.Length : 0);
                var n_browser = cef_browser_host_t.create_browser_sync(n_windowInfo, n_client, &n_url, n_settings, n_requestContext);
                return CefBrowser.FromNative(n_browser);
            }

            // TODO: free n_ structs ?
        }

        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, string url)
        {
            return CreateBrowserSync(windowInfo, client, settings, url, null);
        }

        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, Uri url, CefRequestContext requestContext)
        {
            return CreateBrowserSync(windowInfo, client, settings, url.ToString(), requestContext);
        }

        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, Uri url)
        {
            return CreateBrowserSync(windowInfo, client, settings, url, null);
        }

        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings, CefRequestContext requestContext)
        {
            return CreateBrowserSync(windowInfo, client, settings, string.Empty, requestContext);
        }

        public static CefBrowser CreateBrowserSync(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings settings)
        {
            return CreateBrowserSync(windowInfo, client, settings, string.Empty);
        }


        /// <summary>
        /// Returns the hosted browser object.
        /// </summary>
        public CefBrowser GetBrowser()
        {
            return CefBrowser.FromNative(cef_browser_host_t.get_browser(_self));
        }

        /// <summary>
        /// Request that the browser close. The JavaScript 'onbeforeunload' event will
        /// be fired. If |force_close| is false the event handler, if any, will be
        /// allowed to prompt the user and the user can optionally cancel the close.
        /// If |force_close| is true the prompt will not be displayed and the close
        /// will proceed. Results in a call to CefLifeSpanHandler::DoClose() if the
        /// event handler allows the close or if |force_close| is true. See
        /// CefLifeSpanHandler::DoClose() documentation for additional usage
        /// information.
        /// </summary>
        public void CloseBrowser(bool forceClose = false)
        {
            cef_browser_host_t.close_browser(_self, forceClose ? 1 : 0);
        }

        /// <summary>
        /// Helper for closing a browser. Call this method from the top-level window
        /// close handler. Internally this calls CloseBrowser(false) if the close has
        /// not yet been initiated. This method returns false while the close is
        /// pending and true after the close has completed. See CloseBrowser() and
        /// CefLifeSpanHandler::DoClose() documentation for additional usage
        /// information. This method must be called on the browser process UI thread.
        /// </summary>
        public bool TryCloseBrowser()
        {
            return cef_browser_host_t.try_close_browser(_self) != 0;
        }

        /// <summary>
        /// Set whether the browser is focused.
        /// </summary>
        public void SetFocus(bool focus)
        {
            cef_browser_host_t.set_focus(_self, focus ? 1 : 0);
        }

        /// <summary>
        /// Retrieve the window handle for this browser. If this browser is wrapped in
        /// a CefBrowserView this method should be called on the browser process UI
        /// thread and it will return the handle for the top-level native window.
        /// </summary>
        public IntPtr GetWindowHandle()
        {
            return cef_browser_host_t.get_window_handle(_self);
        }

        /// <summary>
        /// Retrieve the window handle of the browser that opened this browser. Will
        /// return NULL for non-popup windows or if this browser is wrapped in a
        /// CefBrowserView. This method can be used in combination with custom handling
        /// of modal windows.
        /// </summary>
        public IntPtr GetOpenerWindowHandle()
        {
            return cef_browser_host_t.get_opener_window_handle(_self);
        }

        /// <summary>
        /// Returns true if this browser is wrapped in a CefBrowserView.
        /// </summary>
        public bool HasView
        {
            get { return cef_browser_host_t.has_view(_self) != 0; }
        }

        /// <summary>
        /// Returns the client for this browser.
        /// </summary>
        public CefClient GetClient()
        {
            return CefClient.FromNative(
                cef_browser_host_t.get_client(_self)
                );
        }


        /// <summary>
        /// Returns the request context for this browser.
        /// </summary>
        public CefRequestContext GetRequestContext()
        {
            return CefRequestContext.FromNative(
                cef_browser_host_t.get_request_context(_self)
                );
        }


        /// <summary>
        /// Get the current zoom level. The default zoom level is 0.0. This method can
        /// only be called on the UI thread.
        /// </summary>
        public double GetZoomLevel()
        {
            return cef_browser_host_t.get_zoom_level(_self);
        }

        /// <summary>
        /// Change the zoom level to the specified value. Specify 0.0 to reset the
        /// zoom level. If called on the UI thread the change will be applied
        /// immediately. Otherwise, the change will be applied asynchronously on the
        /// UI thread.
        /// </summary>
        public void SetZoomLevel(double value)
        {
            cef_browser_host_t.set_zoom_level(_self, value);
        }

        /// <summary>
        /// Call to run a file chooser dialog. Only a single file chooser dialog may be
        /// pending at any given time. |mode| represents the type of dialog to display.
        /// |title| to the title to be used for the dialog and may be empty to show the
        /// default title ("Open" or "Save" depending on the mode). |default_file_path|
        /// is the path with optional directory and/or file name component that will be
        /// initially selected in the dialog. |accept_filters| are used to restrict the
        /// selectable file types and may any combination of (a) valid lower-cased MIME
        /// types (e.g. "text/*" or "image/*"), (b) individual file extensions (e.g.
        /// ".txt" or ".png"), or (c) combined description and file extension delimited
        /// using "|" and ";" (e.g. "Image Types|.png;.gif;.jpg").
        /// |selected_accept_filter| is the 0-based index of the filter that will be
        /// selected by default. |callback| will be executed after the dialog is
        /// dismissed or immediately if another dialog is already pending. The dialog
        /// will be initiated asynchronously on the UI thread.
        /// </summary>
        public void RunFileDialog(CefFileDialogMode mode, string title, string defaultFilePath, string[] acceptFilters, int selectedAcceptFilter, CefRunFileDialogCallback callback)
        {
            if (callback == null) throw new ArgumentNullException("callback");

            fixed (char* title_ptr = title)
            fixed (char* defaultFilePath_ptr = defaultFilePath)
            {
                var n_title = new cef_string_t(title_ptr, title != null ? title.Length : 0);
                var n_defaultFilePath = new cef_string_t(defaultFilePath_ptr, defaultFilePath != null ? defaultFilePath.Length : 0);
                var n_acceptFilters = cef_string_list.From(acceptFilters);

                cef_browser_host_t.run_file_dialog(_self, mode, &n_title, &n_defaultFilePath, n_acceptFilters, selectedAcceptFilter, callback.ToNative());

                libcef.string_list_free(n_acceptFilters);
            }
        }

        /// <summary>
        /// Download the file at |url| using CefDownloadHandler.
        /// </summary>
        public void StartDownload(string url)
        {
            if (string.IsNullOrEmpty(url)) throw new ArgumentNullException("url");

            fixed (char* url_ptr = url)
            {
                var n_url = new cef_string_t(url_ptr, url.Length);

                cef_browser_host_t.start_download(_self, &n_url);
            }
        }

        /// <summary>
        /// Download |image_url| and execute |callback| on completion with the images
        /// received from the renderer. If |is_favicon| is true then cookies are not
        /// sent and not accepted during download. Images with density independent
        /// pixel (DIP) sizes larger than |max_image_size| are filtered out from the
        /// image results. Versions of the image at different scale factors may be
        /// downloaded up to the maximum scale factor supported by the system. If there
        /// are no image results &lt;= |max_image_size| then the smallest image is resized
        /// to |max_image_size| and is the only result. A |max_image_size| of 0 means
        /// unlimited. If |bypass_cache| is true then |image_url| is requested from the
        /// server even if it is present in the browser cache.
        /// </summary>
        public void DownloadImage(string imageUrl, bool isFavIcon, uint maxImageSize, bool bypassCache, CefDownloadImageCallback callback)
        {
            if (string.IsNullOrEmpty(imageUrl)) throw new ArgumentNullException("imageUrl");

            fixed (char* imageUrl_ptr = imageUrl)
            {
                var n_imageUrl = new cef_string_t(imageUrl_ptr, imageUrl.Length);
                var n_callback = callback.ToNative();
                cef_browser_host_t.download_image(_self, &n_imageUrl, isFavIcon ? 1 : 0, maxImageSize, bypassCache ? 1 : 0, n_callback);
            }
        }

        /// <summary>
        /// Print the current browser contents.
        /// </summary>
        public void Print()
        {
            cef_browser_host_t.print(_self);
        }

        /// <summary>
        /// Print the current browser contents to the PDF file specified by |path| and
        /// execute |callback| on completion. The caller is responsible for deleting
        /// |path| when done. For PDF printing to work on Linux you must implement the
        /// CefPrintHandler::GetPdfPaperSize method.
        /// </summary>
        public void PrintToPdf(string path, CefPdfPrintSettings settings, CefPdfPrintCallback callback)
        {
            fixed (char* path_ptr = path)
            {
                var n_path = new cef_string_t(path_ptr, path.Length);

                var n_settings = settings.ToNative();

                cef_browser_host_t.print_to_pdf(_self,
                    &n_path,
                    n_settings,
                    callback.ToNative()
                    );

                cef_pdf_print_settings_t.Clear(n_settings);
                cef_pdf_print_settings_t.Free(n_settings);
            }
        }

        /// <summary>
        /// Search for |searchText|. |identifier| can be used to have multiple searches
        /// running simultaniously. |forward| indicates whether to search forward or
        /// backward within the page. |matchCase| indicates whether the search should
        /// be case-sensitive. |findNext| indicates whether this is the first request
        /// or a follow-up. The CefFindHandler instance, if any, returned via
        /// CefClient::GetFindHandler will be called to report find results.
        /// </summary>
        public void Find(int identifier, string searchText, bool forward, bool matchCase, bool findNext)
        {
            fixed (char* searchText_ptr = searchText)
            {
                var n_searchText = new cef_string_t(searchText_ptr, searchText.Length);

                cef_browser_host_t.find(_self, identifier, &n_searchText, forward ? 1 : 0, matchCase ? 1 : 0, findNext ? 1 : 0);
            }
        }

        /// <summary>
        /// Cancel all searches that are currently going on.
        /// </summary>
        public void StopFinding(bool clearSelection)
        {
            cef_browser_host_t.stop_finding(_self, clearSelection ? 1 : 0);
        }

        /// <summary>
        /// Open developer tools (DevTools) in its own browser. The DevTools browser
        /// will remain associated with this browser. If the DevTools browser is
        /// already open then it will be focused, in which case the |windowInfo|,
        /// |client| and |settings| parameters will be ignored. If |inspect_element_at|
        /// is non-empty then the element at the specified (x,y) location will be
        /// inspected. The |windowInfo| parameter will be ignored if this browser is
        /// wrapped in a CefBrowserView.
        /// </summary>
        public void ShowDevTools(CefWindowInfo windowInfo, CefClient client, CefBrowserSettings browserSettings, CefPoint inspectElementAt)
        {
            var n_inspectElementAt = new cef_point_t(inspectElementAt.X, inspectElementAt.Y);
            cef_browser_host_t.show_dev_tools(_self, windowInfo.ToNative(), client.ToNative(), browserSettings.ToNative(),
                &n_inspectElementAt);
        }

        /// <summary>
        /// Explicitly close the associated DevTools browser, if any.
        /// </summary>
        public void CloseDevTools()
        {
            cef_browser_host_t.close_dev_tools(_self);
        }

        /// <summary>
        /// Returns true if this browser currently has an associated DevTools browser.
        /// Must be called on the browser process UI thread.
        /// </summary>
        public bool HasDevTools
        {
            get
            {
                return cef_browser_host_t.has_dev_tools(_self) != 0;
            }
        }

        /// <summary>
        /// Retrieve a snapshot of current navigation entries as values sent to the
        /// specified visitor. If |current_only| is true only the current navigation
        /// entry will be sent, otherwise all navigation entries will be sent.
        /// </summary>
        public void GetNavigationEntries(CefNavigationEntryVisitor visitor, bool currentOnly)
        {
            cef_browser_host_t.get_navigation_entries(_self, visitor.ToNative(), currentOnly ? 1 : 0);
        }

        /// <summary>
        /// Set whether mouse cursor change is disabled.
        /// </summary>
        public void SetMouseCursorChangeDisabled(bool disabled)
        {
            cef_browser_host_t.set_mouse_cursor_change_disabled(_self, disabled ? 1 : 0);
        }

        /// <summary>
        /// Returns true if mouse cursor change is disabled.
        /// </summary>
        public bool IsMouseCursorChangeDisabled
        {
            get
            {
                return cef_browser_host_t.is_mouse_cursor_change_disabled(_self) != 0;
            }
        }

        /// <summary>
        /// If a misspelled word is currently selected in an editable node calling
        /// this method will replace it with the specified |word|.
        /// </summary>
        public void ReplaceMisspelling(string word)
        {
            fixed (char* word_str = word)
            {
                var n_word = new cef_string_t(word_str, word != null ? word.Length : 0);
                cef_browser_host_t.replace_misspelling(_self, &n_word);
            }
        }

        /// <summary>
        /// Add the specified |word| to the spelling dictionary.
        /// </summary>
        public void AddWordToDictionary(string word)
        {
            fixed (char* word_str = word)
            {
                var n_word = new cef_string_t(word_str, word != null ? word.Length : 0);
                cef_browser_host_t.add_word_to_dictionary(_self, &n_word);
            }
        }

        /// <summary>
        /// Returns true if window rendering is disabled.
        /// </summary>
        public bool IsWindowRenderingDisabled
        {
            get
            {
                return cef_browser_host_t.is_window_rendering_disabled(_self) != 0;
            }
        }

        /// <summary>
        /// Notify the browser that the widget has been resized. The browser will first
        /// call CefRenderHandler::GetViewRect to get the new size and then call
        /// CefRenderHandler::OnPaint asynchronously with the updated regions. This
        /// method is only used when window rendering is disabled.
        /// </summary>
        public void WasResized()
        {
            cef_browser_host_t.was_resized(_self);
        }

        /// <summary>
        /// Notify the browser that it has been hidden or shown. Layouting and
        /// CefRenderHandler::OnPaint notification will stop when the browser is
        /// hidden. This method is only used when window rendering is disabled.
        /// </summary>
        public void WasHidden(bool hidden)
        {
            cef_browser_host_t.was_hidden(_self, hidden ? 1 : 0);
        }

        /// <summary>
        /// Send a notification to the browser that the screen info has changed. The
        /// browser will then call CefRenderHandler::GetScreenInfo to update the
        /// screen information with the new values. This simulates moving the webview
        /// window from one display to another, or changing the properties of the
        /// current display. This method is only used when window rendering is
        /// disabled.
        /// </summary>
        public void NotifyScreenInfoChanged()
        {
            cef_browser_host_t.notify_screen_info_changed(_self);
        }

        /// <summary>
        /// Invalidate the view. The browser will call CefRenderHandler::OnPaint
        /// asynchronously. This method is only used when window rendering is
        /// disabled.
        /// </summary>
        public void Invalidate(CefPaintElementType type)
        {
            cef_browser_host_t.invalidate(_self, type);
        }

        /// <summary>
        /// Send a key event to the browser.
        /// </summary>
        public void SendKeyEvent(CefKeyEvent keyEvent)
        {
            if (keyEvent == null) throw new ArgumentNullException("keyEvent");

            var n_event = new cef_key_event_t();
            keyEvent.ToNative(&n_event);
            cef_browser_host_t.send_key_event(_self, &n_event);
        }

        /// <summary>
        /// Send a mouse click event to the browser. The |x| and |y| coordinates are
        /// relative to the upper-left corner of the view.
        /// </summary>
        public void SendMouseClickEvent(CefMouseEvent @event, CefMouseButtonType type, bool mouseUp, int clickCount)
        {
            var n_event = @event.ToNative();
            cef_browser_host_t.send_mouse_click_event(_self, &n_event, type, mouseUp ? 1 : 0, clickCount);
        }

        /// <summary>
        /// Send a mouse move event to the browser. The |x| and |y| coordinates are
        /// relative to the upper-left corner of the view.
        /// </summary>
        public void SendMouseMoveEvent(CefMouseEvent @event, bool mouseLeave)
        {
            var n_event = @event.ToNative();
            cef_browser_host_t.send_mouse_move_event(_self, &n_event, mouseLeave ? 1 : 0);
        }

        /// <summary>
        /// Send a mouse wheel event to the browser. The |x| and |y| coordinates are
        /// relative to the upper-left corner of the view. The |deltaX| and |deltaY|
        /// values represent the movement delta in the X and Y directions respectively.
        /// In order to scroll inside select popups with window rendering disabled
        /// CefRenderHandler::GetScreenPoint should be implemented properly.
        /// </summary>
        public void SendMouseWheelEvent(CefMouseEvent @event, int deltaX, int deltaY)
        {
            var n_event = @event.ToNative();
            cef_browser_host_t.send_mouse_wheel_event(_self, &n_event, deltaX, deltaY);
        }

        /// <summary>
        /// Send a focus event to the browser.
        /// </summary>
        public void SendFocusEvent(bool setFocus)
        {
            cef_browser_host_t.send_focus_event(_self, setFocus ? 1 : 0);
        }

        /// <summary>
        /// Send a capture lost event to the browser.
        /// </summary>
        public void SendCaptureLostEvent()
        {
            cef_browser_host_t.send_capture_lost_event(_self);
        }

        /// <summary>
        /// Notify the browser that the window hosting it is about to be moved or
        /// resized. This method is only used on Windows and Linux.
        /// </summary>
        public void NotifyMoveOrResizeStarted()
        {
            cef_browser_host_t.notify_move_or_resize_started(_self);
        }

        /// <summary>
        /// Returns the maximum rate in frames per second (fps) that CefRenderHandler::
        /// OnPaint will be called for a windowless browser. The actual fps may be
        /// lower if the browser cannot generate frames at the requested rate. The
        /// minimum value is 1 and the maximum value is 60 (default 30). This method
        /// can only be called on the UI thread.
        /// </summary>
        public int GetWindowlessFrameRate()
        {
            return cef_browser_host_t.get_windowless_frame_rate(_self);
        }

        /// <summary>
        /// Set the maximum rate in frames per second (fps) that CefRenderHandler::
        /// OnPaint will be called for a windowless browser. The actual fps may be
        /// lower if the browser cannot generate frames at the requested rate. The
        /// minimum value is 1 and the maximum value is 60 (default 30). Can also be
        /// set at browser creation via CefBrowserSettings.windowless_frame_rate.
        /// </summary>
        public void SetWindowlessFrameRate(int frameRate)
        {
            cef_browser_host_t.set_windowless_frame_rate(_self, frameRate);
        }

        /// <summary>
        /// Get the NSTextInputContext implementation for enabling IME on Mac when
        /// window rendering is disabled.
        /// </summary>
        public IntPtr GetNSTextInputContext()
        {
            return cef_browser_host_t.get_nstext_input_context(_self);
        }

        /// <summary>
        /// Handles a keyDown event prior to passing it through the NSTextInputClient
        /// machinery.
        /// </summary>
        public void HandleKeyEventBeforeTextInputClient(IntPtr keyEvent)
        {
            cef_browser_host_t.handle_key_event_before_text_input_client(_self, keyEvent);
        }

        /// <summary>
        /// Performs any additional actions after NSTextInputClient handles the event.
        /// </summary>
        public void HandleKeyEventAfterTextInputClient(IntPtr keyEvent)
        {
            cef_browser_host_t.handle_key_event_after_text_input_client(_self, keyEvent);
        }

        /// <summary>
        /// Call this method when the user drags the mouse into the web view (before
        /// calling DragTargetDragOver/DragTargetLeave/DragTargetDrop).
        /// |drag_data| should not contain file contents as this type of data is not
        /// allowed to be dragged into the web view. File contents can be removed using
        /// CefDragData::ResetFileContents (for example, if |drag_data| comes from
        /// CefRenderHandler::StartDragging).
        /// This method is only used when window rendering is disabled.
        /// </summary>
        public void DragTargetDragEnter(CefDragData dragData, CefMouseEvent mouseEvent, CefDragOperationsMask allowedOps)
        {
            var n_mouseEvent = mouseEvent.ToNative();
            cef_browser_host_t.drag_target_drag_enter(_self,
                dragData.ToNative(),
                &n_mouseEvent,
                allowedOps);
        }

        /// <summary>
        /// Call this method each time the mouse is moved across the web view during
        /// a drag operation (after calling DragTargetDragEnter and before calling
        /// DragTargetDragLeave/DragTargetDrop).
        /// This method is only used when window rendering is disabled.
        /// </summary>
        public void DragTargetDragOver(CefMouseEvent mouseEvent, CefDragOperationsMask allowedOps)
        {
            var n_mouseEvent = mouseEvent.ToNative();
            cef_browser_host_t.drag_target_drag_over(_self, &n_mouseEvent, allowedOps);
        }

        /// <summary>
        /// Call this method when the user drags the mouse out of the web view (after
        /// calling DragTargetDragEnter).
        /// This method is only used when window rendering is disabled.
        /// </summary>
        public void DragTargetDragLeave()
        {
            cef_browser_host_t.drag_target_drag_leave(_self);
        }

        /// <summary>
        /// Call this method when the user completes the drag operation by dropping
        /// the object onto the web view (after calling DragTargetDragEnter).
        /// The object being dropped is |drag_data|, given as an argument to
        /// the previous DragTargetDragEnter call.
        /// This method is only used when window rendering is disabled.
        /// </summary>
        public void DragTargetDrop(CefMouseEvent mouseEvent)
        {
            var n_mouseEvent = mouseEvent.ToNative();
            cef_browser_host_t.drag_target_drop(_self, &n_mouseEvent);
        }

        /// <summary>
        /// Call this method when the drag operation started by a
        /// CefRenderHandler::StartDragging call has ended either in a drop or
        /// by being cancelled. |x| and |y| are mouse coordinates relative to the
        /// upper-left corner of the view. If the web view is both the drag source
        /// and the drag target then all DragTarget* methods should be called before
        /// DragSource* mthods.
        /// This method is only used when window rendering is disabled.
        /// </summary>
        public void DragSourceEndedAt(int x, int y, CefDragOperationsMask op)
        {
            cef_browser_host_t.drag_source_ended_at(_self, x, y, op);
        }

        /// <summary>
        /// Call this method when the drag operation started by a
        /// CefRenderHandler::StartDragging call has completed. This method may be
        /// called immediately without first calling DragSourceEndedAt to cancel a
        /// drag operation. If the web view is both the drag source and the drag
        /// target then all DragTarget* methods should be called before DragSource*
        /// mthods.
        /// This method is only used when window rendering is disabled.
        /// </summary>
        public void DragSourceSystemDragEnded()
        {
            cef_browser_host_t.drag_source_system_drag_ended(_self);
        }
    }
}

#endif