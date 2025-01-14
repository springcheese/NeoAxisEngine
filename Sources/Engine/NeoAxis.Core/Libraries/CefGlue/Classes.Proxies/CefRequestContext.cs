#if !NO_UI_WEB_BROWSER
namespace Internal.Xilium.CefGlue
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.InteropServices;
    using Internal.Xilium.CefGlue.Interop;

    /// <summary>
    /// A request context provides request handling for a set of related browser
    /// or URL request objects. A request context can be specified when creating a
    /// new browser via the CefBrowserHost static factory methods or when creating a
    /// new URL request via the CefURLRequest static factory methods. Browser objects
    /// with different request contexts will never be hosted in the same render
    /// process. Browser objects with the same request context may or may not be
    /// hosted in the same render process depending on the process model. Browser
    /// objects created indirectly via the JavaScript window.open function or
    /// targeted links will share the same render process and the same request
    /// context as the source browser. When running in single-process mode there is
    /// only a single render process (the main process) and so all browsers created
    /// in single-process mode will share the same request context. This will be the
    /// first request context passed into a CefBrowserHost static factory method and
    /// all other request context objects will be ignored.
    /// </summary>
    public sealed unsafe partial class CefRequestContext
    {
        /// <summary>
        /// Returns the global context object.
        /// </summary>
        public static CefRequestContext GetGlobalContext()
        {
            return CefRequestContext.FromNative(
                cef_request_context_t.get_global_context()
                );
        }

        /// <summary>
        /// Creates a new context object with the specified |settings| and optional
        /// |handler|.
        /// </summary>
        public static CefRequestContext CreateContext(CefRequestContextSettings settings, CefRequestContextHandler handler)
        {
            var n_settings = settings.ToNative();

            var result = CefRequestContext.FromNative(
                cef_request_context_t.create_context(
                    n_settings,
                    handler != null ? handler.ToNative() : null
                    )
                );

            CefRequestContextSettings.Free(n_settings);

            return result;
        }

        /// <summary>
        /// Creates a new context object that shares storage with |other| and uses an
        /// optional |handler|.
        /// </summary>
        public static CefRequestContext CreateContext(CefRequestContext other, CefRequestContextHandler handler)
        {
            return CefRequestContext.FromNative(
                cef_request_context_t.create_context(
                    other.ToNative(),
                    handler != null ? handler.ToNative() : null
                    )
                );
        }


        /// <summary>
        /// Returns true if this object is pointing to the same context as |that|
        /// object.
        /// </summary>
        public bool IsSame(CefRequestContext other)
        {
            if (other == null) return false;

            return cef_request_context_t.is_same(_self, other.ToNative()) != 0;
        }

        /// <summary>
        /// Returns true if this object is sharing the same storage as |that| object.
        /// </summary>
        public bool IsSharingWith(CefRequestContext other)
        {
            return cef_request_context_t.is_sharing_with(_self, other.ToNative()) != 0;
        }

        /// <summary>
        /// Returns true if this object is the global context. The global context is
        /// used by default when creating a browser or URL request with a NULL context
        /// argument.
        /// </summary>
        public bool IsGlobal
        {
            get
            {
                return cef_request_context_t.is_global(_self) != 0;
            }
        }

        /// <summary>
        /// Returns the handler for this context if any.
        /// </summary>
        public CefRequestContextHandler GetHandler()
        {
            return CefRequestContextHandler.FromNativeOrNull(
                cef_request_context_t.get_handler(_self)
                );
        }

        /// <summary>
        /// Returns the cache path for this object. If empty an "incognito mode"
        /// in-memory cache is being used.
        /// </summary>
        public string CachePath
        {
            get
            {
                var n_result = cef_request_context_t.get_cache_path(_self);
                return cef_string_userfree.ToString(n_result);
            }
        }

        /// <summary>
        /// Returns the default cookie manager for this object. This will be the global
        /// cookie manager if this object is the global request context. Otherwise,
        /// this will be the default cookie manager used when this request context does
        /// not receive a value via CefRequestContextHandler::GetCookieManager(). If
        /// |callback| is non-NULL it will be executed asnychronously on the IO thread
        /// after the manager's storage has been initialized.
        /// </summary>
        public CefCookieManager GetDefaultCookieManager(CefCompletionCallback callback)
        {
            var n_callback = callback != null ? callback.ToNative() : null;

            return CefCookieManager.FromNativeOrNull(
                cef_request_context_t.get_default_cookie_manager(_self, n_callback)
                );
        }

        /// <summary>
        /// Register a scheme handler factory for the specified |scheme_name| and
        /// optional |domain_name|. An empty |domain_name| value for a standard scheme
        /// will cause the factory to match all domain names. The |domain_name| value
        /// will be ignored for non-standard schemes. If |scheme_name| is a built-in
        /// scheme and no handler is returned by |factory| then the built-in scheme
        /// handler factory will be called. If |scheme_name| is a custom scheme then
        /// you must also implement the CefApp::OnRegisterCustomSchemes() method in all
        /// processes. This function may be called multiple times to change or remove
        /// the factory that matches the specified |scheme_name| and optional
        /// |domain_name|. Returns false if an error occurs. This function may be
        /// called on any thread in the browser process.
        /// </summary>
        public bool RegisterSchemeHandlerFactory(string schemeName, string domainName, CefSchemeHandlerFactory factory)
        {
            if (string.IsNullOrEmpty(schemeName)) throw new ArgumentNullException("schemeName");
            if (factory == null) throw new ArgumentNullException("factory");

            fixed (char* schemeName_str = schemeName)
            fixed (char* domainName_str = domainName)
            {
                var n_schemeName = new cef_string_t(schemeName_str, schemeName.Length);
                var n_domainName = new cef_string_t(domainName_str, domainName != null ? domainName.Length : 0);

                return cef_request_context_t.register_scheme_handler_factory(_self, &n_schemeName, &n_domainName, factory.ToNative()) != 0;
            }
        }

        /// <summary>
        /// Clear all registered scheme handler factories. Returns false on error. This
        /// function may be called on any thread in the browser process.
        /// </summary>
        public bool ClearSchemeHandlerFactories()
        {
            return cef_request_context_t.clear_scheme_handler_factories(_self) != 0;
        }

        /// <summary>
        /// Tells all renderer processes associated with this context to throw away
        /// their plugin list cache. If |reload_pages| is true they will also reload
        /// all pages with plugins. CefRequestContextHandler::OnBeforePluginLoad may
        /// be called to rebuild the plugin list cache.
        /// </summary>
        public void PurgePluginListCache(bool reloadPages)
        {
            cef_request_context_t.purge_plugin_list_cache(_self, reloadPages ? 1 : 0);
        }

        /// <summary>
        /// Returns true if a preference with the specified |name| exists. This method
        /// must be called on the browser process UI thread.
        /// </summary>
        public bool HasPreference(string name)
        {
            fixed (char* name_str = name)
            {
                var n_name = new cef_string_t(name_str, name != null ? name.Length : 0);
                return cef_request_context_t.has_preference(_self, &n_name) != 0;
            }
        }

        /// <summary>
        /// Returns the value for the preference with the specified |name|. Returns
        /// NULL if the preference does not exist. The returned object contains a copy
        /// of the underlying preference value and modifications to the returned object
        /// will not modify the underlying preference value. This method must be called
        /// on the browser process UI thread.
        /// </summary>
        public CefValue GetPreference(string name)
        {
            fixed (char* name_str = name)
            {
                var n_name = new cef_string_t(name_str, name != null ? name.Length : 0);
                var n_value = cef_request_context_t.get_preference(_self, &n_name);
                return CefValue.FromNativeOrNull(n_value);
            }
        }

        /// <summary>
        /// Returns all preferences as a dictionary. If |include_defaults| is true then
        /// preferences currently at their default value will be included. The returned
        /// object contains a copy of the underlying preference values and
        /// modifications to the returned object will not modify the underlying
        /// preference values. This method must be called on the browser process UI
        /// thread.
        /// </summary>
        public CefDictionaryValue GetAllPreferences(bool includeDefaults)
        {
            var n_result = cef_request_context_t.get_all_preferences(_self, includeDefaults ? 1 : 0);
            return CefDictionaryValue.FromNative(n_result);
        }

        /// <summary>
        /// Returns true if the preference with the specified |name| can be modified
        /// using SetPreference. As one example preferences set via the command-line
        /// usually cannot be modified. This method must be called on the browser
        /// process UI thread.
        /// </summary>
        public bool CanSetPreference(string name)
        {
            fixed (char* name_str = name)
            {
                var n_name = new cef_string_t(name_str, name != null ? name.Length : 0);
                return cef_request_context_t.can_set_preference(_self, &n_name) != 0;
            }
        }

        /// <summary>
        /// Set the |value| associated with preference |name|. Returns true if the
        /// value is set successfully and false otherwise. If |value| is NULL the
        /// preference will be restored to its default value. If setting the preference
        /// fails then |error| will be populated with a detailed description of the
        /// problem. This method must be called on the browser process UI thread.
        /// </summary>
        public bool SetPreference(string name, CefValue value, out string error)
        {
            fixed (char* name_str = name)
            {
                var n_name = new cef_string_t(name_str, name != null ? name.Length : 0);
                var n_value = value != null ? value.ToNative() : null;
                cef_string_t n_error;

                var n_result = cef_request_context_t.set_preference(_self, &n_name, n_value, &n_error);

                error = cef_string_t.ToString(&n_error);
                return n_result != 0;
            }
        }

        /// <summary>
        /// Clears all certificate exceptions that were added as part of handling
        /// CefRequestHandler::OnCertificateError(). If you call this it is
        /// recommended that you also call CloseAllConnections() or you risk not
        /// being prompted again for server certificates if you reconnect quickly.
        /// If |callback| is non-NULL it will be executed on the UI thread after
        /// completion.
        /// </summary>
        public void ClearCertificateExceptions(CefCompletionCallback callback)
        {
            var n_callback = callback != null ? callback.ToNative() : null;
            cef_request_context_t.clear_certificate_exceptions(_self, n_callback);
        }

        /// <summary>
        /// Clears all active and idle connections that Chromium currently has.
        /// This is only recommended if you have released all other CEF objects but
        /// don't yet want to call CefShutdown(). If |callback| is non-NULL it will be
        /// executed on the UI thread after completion.
        /// </summary>
        public void CloseAllConnections(CefCompletionCallback callback)
        {
            var n_callback = callback != null ? callback.ToNative() : null;
            cef_request_context_t.close_all_connections(_self, n_callback);
        }

        /// <summary>
        /// Attempts to resolve |origin| to a list of associated IP addresses.
        /// |callback| will be executed on the UI thread after completion.
        /// </summary>
        public void ResolveHost(string origin, CefResolveCallback callback)
        {
            if (string.IsNullOrEmpty(origin)) throw new ArgumentNullException("origin");
            if (callback == null) throw new ArgumentNullException("callback");

            fixed (char* origin_str = origin)
            {
                var n_origin = new cef_string_t(origin_str, origin != null ? origin.Length : 0);
                var n_callback = callback.ToNative();
                cef_request_context_t.resolve_host(_self, &n_origin, n_callback);
            }
        }

        /// <summary>
        /// Attempts to resolve |origin| to a list of associated IP addresses using
        /// cached data. |resolved_ips| will be populated with the list of resolved IP
        /// addresses or empty if no cached data is available. Returns ERR_NONE on
        /// success. This method must be called on the browser process IO thread.
        /// </summary>
        public CefErrorCode ResolveHostCached(string origin, out string[] resolvedIps)
        {
            if (string.IsNullOrEmpty(origin)) throw new ArgumentNullException("origin");

            fixed (char* origin_str = origin)
            {
                var n_origin = new cef_string_t(origin_str, origin != null ? origin.Length : 0);
                var n_resolved_ips = libcef.string_list_alloc();
                var result = cef_request_context_t.resolve_host_cached(_self, &n_origin, n_resolved_ips);
                resolvedIps = cef_string_list.ToArray(n_resolved_ips);
                libcef.string_list_free(n_resolved_ips);
                return result;
            }
        }
    }
}

#endif