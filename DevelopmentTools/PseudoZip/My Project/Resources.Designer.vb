﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3074
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
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Tools.IOt.PackagingT.Resources", GetType(Resources).Assembly)
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
        '''  Looks up a localized string similar to {0} files extracted..
        '''</summary>
        Friend ReadOnly Property FilesExtracted() As String
            Get
                Return ResourceManager.GetString("FilesExtracted", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to {0} files packed..
        '''</summary>
        Friend ReadOnly Property FilesPacked() As String
            Get
                Return ResourceManager.GetString("FilesPacked", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Unknown command {0}.
        '''</summary>
        Friend ReadOnly Property UnknownCommand0() As String
            Get
                Return ResourceManager.GetString("UnknownCommand0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Usage: {0} command archive folder [-r] [masks]
        '''Commands:
        '''    a - create archive
        '''    e - extract archive
        '''Archive: Path to pseudo zip file to compress files to | extract files from
        '''Folder: Path to folder to compres files from | extract files to
        '''-r  Only with command a.
        '''    Pack files recursivelly from folder.
        '''Macks: Only with command a.
        '''       Masks of files to include in archive. When ommitted * is used..
        '''</summary>
        Friend ReadOnly Property Usage() As String
            Get
                Return ResourceManager.GetString("Usage", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
