﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.3031
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
        '''  Looks up a localized string similar to Display.
        '''</summary>
        Friend ReadOnly Property catDisplay() As String
            Get
                Return ResourceManager.GetString("catDisplay", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Error.
        '''</summary>
        Friend ReadOnly Property Error_() As String
            Get
                Return ResourceManager.GetString("Error_", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property GoLtrHS() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("GoLtrHS", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Does preview window float over main window?.
        '''</summary>
        Friend ReadOnly Property LargeFloating_d() As String
            Get
                Return ResourceManager.GetString("LargeFloating_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Floating preview.
        '''</summary>
        Friend ReadOnly Property LargeFloating_n() As String
            Get
                Return ResourceManager.GetString("LargeFloating_n", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property NavBack() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("NavBack", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property NavForward() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("NavForward", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Refresh() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Refresh", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        Friend ReadOnly Property Symbol_Delete() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Symbol-Delete", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Size of image preview in main window.
        '''</summary>
        Friend ReadOnly Property ThumbSize_d() As String
            Get
                Return ResourceManager.GetString("ThumbSize_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Thumbnail size.
        '''</summary>
        Friend ReadOnly Property ThumbSize_n() As String
            Get
                Return ResourceManager.GetString("ThumbSize_n", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Unknown splitter.
        '''</summary>
        Friend ReadOnly Property UnknownSplitter() As String
            Get
                Return ResourceManager.GetString("UnknownSplitter", resourceCulture)
            End Get
        End Property
        
        Friend ReadOnly Property Up() As System.Drawing.Bitmap
            Get
                Dim obj As Object = ResourceManager.GetObject("Up", resourceCulture)
                Return CType(obj,System.Drawing.Bitmap)
            End Get
        End Property
    End Module
End Namespace