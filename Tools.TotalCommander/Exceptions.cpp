﻿
#include "Exceptions.h"
namespace Tools {
    namespace TotalCommanderT {
        namespace ResourcesT {
    
    
    inline Exceptions::Exceptions() {
    }
    
    inline ::System::Object^  Exceptions::InternalSyncObject::get() {
        if (System::Object::ReferenceEquals(_internalSyncObject, nullptr)) {
            ::System::Threading::Interlocked::CompareExchange(_internalSyncObject, (gcnew ::System::Object()), nullptr);
        }
        return _internalSyncObject;
    }
    
    inline ::System::Resources::ResourceManager^  Exceptions::ResourceManager::get() {
        if (System::Object::ReferenceEquals(_resourceManager, nullptr)) {
            ::System::Threading::Monitor::Enter(InternalSyncObject);
            try {
                if (System::Object::ReferenceEquals(_resourceManager, nullptr)) {
                    ::System::Threading::Interlocked::Exchange(_resourceManager, (gcnew ::System::Resources::ResourceManager(L"ToolsTotalCommander.Exceptions.resources", 
                            Tools::TotalCommanderT::ResourcesT::Exceptions::typeid->Assembly)));
                }
            }
            finally {
                ::System::Threading::Monitor::Exit(InternalSyncObject);
            }
        }
        return _resourceManager;
    }
    
    inline ::System::Globalization::CultureInfo^  Exceptions::Culture::get() {
        return _resourceCulture;
    }
    inline System::Void Exceptions::Culture::set(::System::Globalization::CultureInfo^  value) {
        _resourceCulture = value;
    }
    
    inline System::String^  Exceptions::CannotBeRepresented::get() {
        return ResourceManager->GetString(L"CannotBeRepresented", _resourceCulture);
    }
    
    inline System::String^  Exceptions::DefaultTextTooLong::get() {
        return ResourceManager->GetString(L"DefaultTextTooLong", _resourceCulture);
    }
    
    inline System::String^  Exceptions::InvalidMacroName::get() {
        return ResourceManager->GetString(L"InvalidMacroName", _resourceCulture);
    }
    
    inline System::String^  Exceptions::NameTooLong::get() {
        return ResourceManager->GetString(L"NameTooLong", _resourceCulture);
    }
    
    inline System::String^  Exceptions::PathTooLong::get() {
        return ResourceManager->GetString(L"PathTooLong", _resourceCulture);
    }
    
    inline System::String^  Exceptions::PluginNotInitialized::get() {
        return ResourceManager->GetString(L"PluginNotInitialized", _resourceCulture);
    }
    
    inline System::String^  Exceptions::CannotBeRepresentedFormat(System::Object^  arg0, System::Object^  arg1, System::Object^  arg2, 
                System::Object^  arg3) {
        return System::String::Format(_resourceCulture, CannotBeRepresented, arg0, arg1, arg2, arg3);
    }
    
    inline System::String^  Exceptions::DefaultTextTooLongFormat() {
        return DefaultTextTooLong;
    }
    
    inline System::String^  Exceptions::InvalidMacroNameFormat(System::Object^  arg0) {
        return System::String::Format(_resourceCulture, InvalidMacroName, arg0);
    }
    
    inline System::String^  Exceptions::NameTooLongFormat(System::Object^  arg0) {
        return System::String::Format(_resourceCulture, NameTooLong, arg0);
    }
    
    inline System::String^  Exceptions::PathTooLongFormat() {
        return PathTooLong;
    }
    
    inline System::String^  Exceptions::PluginNotInitializedFormat() {
        return PluginNotInitialized;
    }
        }
    }
}
 