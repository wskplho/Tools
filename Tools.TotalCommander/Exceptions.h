﻿#pragma once

#using <mscorlib.dll>
#using <System.dll>

using namespace System::Security::Permissions;
[assembly:SecurityPermissionAttribute(SecurityAction::RequestMinimum, SkipVerification=false)];
namespace Tools {
    namespace TotalCommanderT {
        namespace ResourcesT {
    using namespace System;
    using namespace System;
    ref class Exceptions;
    
    
    /// <summary>
    /// A strongly-typed resource class, for looking up localized strings, formatting them, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilderEx class via the InternalResXFileCodeGeneratorEx custom tool.
    // To add or remove a member, edit your .ResX file then rerun the InternalResXFileCodeGeneratorEx custom tool or rebuild your VS.NET project.
    // Copyright (c) Dmytro Kryvko 2006-2009 (http://dmytro.kryvko.googlepages.com/)
    [System::CodeDom::Compiler::GeneratedCodeAttribute(L"DMKSoftware.CodeGenerators.Tools.StronglyTypedResourceBuilderEx", L"2.2.5.0"), 
    System::Diagnostics::DebuggerNonUserCodeAttribute, 
    System::Diagnostics::CodeAnalysis::SuppressMessageAttribute(L"Microsoft.Naming", L"CA1724:TypeNamesShouldNotMatchNamespaces")]
    ref class Exceptions {
        
        private: static ::System::Resources::ResourceManager^  _resourceManager;
        
        private: static ::System::Object^  _internalSyncObject;
        
        private: static ::System::Globalization::CultureInfo^  _resourceCulture;
        
        internal: [System::Diagnostics::CodeAnalysis::SuppressMessageAttribute(L"Microsoft.Performance", L"CA1811:AvoidUncalledPrivateCode")]
        Exceptions();
        /// <summary>
        /// Thread safe lock object used by this class.
        /// </summary>
        internal: static property ::System::Object^  InternalSyncObject {
            ::System::Object^  get();
        }
        
        /// <summary>
        /// Returns the cached ResourceManager instance used by this class.
        /// </summary>
        internal: [System::ComponentModel::EditorBrowsableAttribute(::System::ComponentModel::EditorBrowsableState::Advanced)]
        static property ::System::Resources::ResourceManager^  ResourceManager {
            ::System::Resources::ResourceManager^  get();
        }
        
        /// <summary>
        /// Overrides the current thread's CurrentUICulture property for all
        /// resource lookups using this strongly typed resource class.
        /// </summary>
        internal: [System::ComponentModel::EditorBrowsableAttribute(::System::ComponentModel::EditorBrowsableState::Advanced)]
        static property ::System::Globalization::CultureInfo^  Culture {
            ::System::Globalization::CultureInfo^  get();
            System::Void set(::System::Globalization::CultureInfo^  value);
        }
        
        /// <summary>
        /// Looks up a localized string similar to '{0} cannot be represented as {1}, use {2} and {3} instead.'.
        /// </summary>
        internal: static property System::String^  CannotBeRepresented {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Default text is too long.'.
        /// </summary>
        internal: static property System::String^  DefaultTextTooLong {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Macor name &quot;{0}&quot; is invalid.'.
        /// </summary>
        internal: static property System::String^  InvalidMacroName {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'The path {0} has invalid format.'.
        /// </summary>
        internal: static property System::String^  InvalidPathFormat {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Name too long. Mamximum allowed length is {0}'.
        /// </summary>
        internal: static property System::String^  NameTooLong {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'The {0} parameter of the {1} method was assigned to long string.'.
        /// </summary>
        internal: static property System::String^  ParamAssignedTooLong {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'String returned by plugin is longer than maximal path length alowed.'.
        /// </summary>
        internal: static property System::String^  PathTooLong {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'Plugin was not initialized.'.
        /// </summary>
        internal: static property System::String^  PluginNotInitialized {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'The {0} property have already been initialized.'.
        /// </summary>
        internal: static property System::String^  PropertyWasInitialized {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to 'The {0} property have not been initialized yet.'.
        /// </summary>
        internal: static property System::String^  PropertyWasNotInitialized {
            System::String^  get();
        }
        
        /// <summary>
        /// Looks up a localized string similar to '{0} was null.'.
        /// </summary>
        internal: static property System::String^  PropertyWasNull {
            System::String^  get();
        }
        
        /// <summary>
        /// Formats a localized string similar to '{0} cannot be represented as {1}, use {2} and {3} instead.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <param name="arg1">An object (1) to format.</param>
        /// <param name="arg2">An object (2) to format.</param>
        /// <param name="arg3">An object (3) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: [System::Diagnostics::CodeAnalysis::SuppressMessageAttribute(L"Microsoft.Design", L"CA1025:ReplaceRepetitiveArgumentsWithParamsArray")]
        static System::String^  CannotBeRepresentedFormat(System::Object^  arg0, System::Object^  arg1, System::Object^  arg2, 
                    System::Object^  arg3);
        
        /// <summary>
        /// The stub formatting method returning the DefaultTextTooLong property value.
        /// </summary>
        /// <returns>The DefaultTextTooLong property value.</returns>
        internal: static System::String^  DefaultTextTooLongFormat();
        
        /// <summary>
        /// Formats a localized string similar to 'Macor name &quot;{0}&quot; is invalid.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: static System::String^  InvalidMacroNameFormat(System::Object^  arg0);
        
        /// <summary>
        /// Formats a localized string similar to 'The path {0} has invalid format.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: static System::String^  InvalidPathFormatFormat(System::Object^  arg0);
        
        /// <summary>
        /// Formats a localized string similar to 'Name too long. Mamximum allowed length is {0}'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: static System::String^  NameTooLongFormat(System::Object^  arg0);
        
        /// <summary>
        /// Formats a localized string similar to 'The {0} parameter of the {1} method was assigned to long string.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <param name="arg1">An object (1) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: static System::String^  ParamAssignedTooLongFormat(System::Object^  arg0, System::Object^  arg1);
        
        /// <summary>
        /// The stub formatting method returning the PathTooLong property value.
        /// </summary>
        /// <returns>The PathTooLong property value.</returns>
        internal: static System::String^  PathTooLongFormat();
        
        /// <summary>
        /// The stub formatting method returning the PluginNotInitialized property value.
        /// </summary>
        /// <returns>The PluginNotInitialized property value.</returns>
        internal: static System::String^  PluginNotInitializedFormat();
        
        /// <summary>
        /// Formats a localized string similar to 'The {0} property have already been initialized.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: static System::String^  PropertyWasInitializedFormat(System::Object^  arg0);
        
        /// <summary>
        /// Formats a localized string similar to 'The {0} property have not been initialized yet.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: static System::String^  PropertyWasNotInitializedFormat(System::Object^  arg0);
        
        /// <summary>
        /// Formats a localized string similar to '{0} was null.'.
        /// </summary>
        /// <param name="arg0">An object (0) to format.</param>
        /// <returns>A copy of format string in which the format items have been replaced by the String equivalent of the corresponding instances of Object in arguments.</returns>
        internal: static System::String^  PropertyWasNullFormat(System::Object^  arg0);
    };
        }
    }
}