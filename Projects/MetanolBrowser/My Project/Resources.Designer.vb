﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.30319.454
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict On
Option Explicit On

Imports System

Namespace My.Resources
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute(),  _
     Global.Microsoft.VisualBasic.HideModuleNameAttribute()>  _
    Friend Module Resources
        
        Private resourceMan As Global.System.Resources.ResourceManager
        
        Private resourceCulture As Global.System.Globalization.CultureInfo
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Tools.Metanol.Resources", GetType(Resources).Assembly)
                    resourceMan = temp
                End If
                Return resourceMan
            End Get
        End Property
        
        '''<summary>
        '''  Overrides the current thread's CurrentUICulture property for all
        '''  resource lookups using this strongly typed resource class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to File {0} does not exist..
        '''</summary>
        Friend ReadOnly Property err_FileDoesNotExist() As String
            Get
                Return ResourceManager.GetString("err_FileDoesNotExist", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Error while loading IPTC data.
        '''</summary>
        Friend ReadOnly Property err_IptcReload() As String
            Get
                Return ResourceManager.GetString("err_IptcReload", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Error while saving IPTC data.
        '''</summary>
        Friend ReadOnly Property err_IptcSave() As String
            Get
                Return ResourceManager.GetString("err_IptcSave", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Error loading file {0}:.
        '''</summary>
        Friend ReadOnly Property err_LoadFile() As String
            Get
                Return ResourceManager.GetString("err_LoadFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to File {0} is not JPEG file..
        '''</summary>
        Friend ReadOnly Property err_NotAJpegFile() As String
            Get
                Return ResourceManager.GetString("err_NotAJpegFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Value must be -1 or 1.
        '''</summary>
        Friend ReadOnly Property ex_1orMinus1() As String
            Get
                Return ResourceManager.GetString("ex_1orMinus1", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to JPEG files (*.jpg, *.jpeg).
        '''</summary>
        Friend ReadOnly Property fil_Jpeg() As String
            Get
                Return ResourceManager.GetString("fil_Jpeg", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Beginning of folder reached. Do you want to continue in another folder?.
        '''</summary>
        Friend ReadOnly Property iq_BeginningOfFolderReached() As String
            Get
                Return ResourceManager.GetString("iq_BeginningOfFolderReached", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to (..).
        '''</summary>
        Friend ReadOnly Property txt_FolderUp() As String
            Get
                Return ResourceManager.GetString("txt_FolderUp", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Open File.
        '''</summary>
        Friend ReadOnly Property txt_OpenFile() As String
            Get
                Return ResourceManager.GetString("txt_OpenFile", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Reload IPTC.
        '''</summary>
        Friend ReadOnly Property txt_ReloadIptc() As String
            Get
                Return ResourceManager.GetString("txt_ReloadIptc", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Save IPTC.
        '''</summary>
        Friend ReadOnly Property txt_SaveIptc() As String
            Get
                Return ResourceManager.GetString("txt_SaveIptc", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to (.) - Současná složka.
        '''</summary>
        Friend ReadOnly Property txt_ThisFolder() As String
            Get
                Return ResourceManager.GetString("txt_ThisFolder", resourceCulture)
            End Get
        End Property
    End Module
End Namespace