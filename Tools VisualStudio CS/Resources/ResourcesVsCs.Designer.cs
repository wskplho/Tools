﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:2.0.50727.3053
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Tools.ResourcesT {
    using System;
    
    
    /// <summary>
    /// A strongly-typed resource class, for looking up localized strings, formatting them, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilderEx class via the InternalResXFileCodeGeneratorEx custom tool.
    // To add or remove a member, edit your .ResX file then rerun the InternalResXFileCodeGeneratorEx custom tool or rebuild your VS.NET project.
    // Copyright (c) Dmytro Kryvko 2006-2008 (http://dmytro.kryvko.googlepages.com/)
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("DMKSoftware.CodeGenerators.Tools.StronglyTypedResourceBuilderEx", "2.1.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Naming", "CA1724:TypeNamesShouldNotMatchNamespaces")]
    internal class ResourcesVsCs {
        
        private static global::System.Resources.ResourceManager _resourceManager;
        
        private static object _internalSyncObject;
        
        private static global::System.Globalization.CultureInfo _resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal ResourcesVsCs() {
        }
        
        /// <summary>
        /// Thread safe lock object used by this class.
        /// </summary>
        internal static object InternalSyncObject {
            get {
                if (object.ReferenceEquals(_internalSyncObject, null)) {
                    global::System.Threading.Interlocked.CompareExchange(ref _internalSyncObject, new object(), null);
                }
                return _internalSyncObject;
            }
        }
        
        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(_resourceManager, null)) {
                    global::System.Threading.Monitor.Enter(InternalSyncObject);
                    try {
                        if (object.ReferenceEquals(_resourceManager, null)) {
                            global::System.Threading.Interlocked.Exchange(ref _resourceManager, new global::System.Resources.ResourceManager("Tools.Resources.ResourcesVsCs", typeof(ResourcesVsCs).Assembly));
                        }
                    }
                    finally {
                        global::System.Threading.Monitor.Exit(InternalSyncObject);
                    }
                }
                return _resourceManager;
            }
        }
        
        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return _resourceCulture;
            }
            set {
                _resourceCulture = value;
            }
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'This source code was auto-generated by {0}, Version {1}.'.
        /// </summary>
        internal static string ThisSourceCodeWasAutoGeneratedBy0Version1 {
            get {
                return ResourceManager.GetString("ThisSourceCodeWasAutoGeneratedBy0Version1", _resourceCulture);
            }
        }
        
        /// <summary>
        /// Formats a localized string similar to 'This source code was auto-generated by {0}, Version {1}.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <param name="arg1">An object (1) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal static string ThisSourceCodeWasAutoGeneratedBy0Version1Format(object arg0, object arg1) {
            return string.Format(_resourceCulture, ThisSourceCodeWasAutoGeneratedBy0Version1, arg0, arg1);
        }
    }
}