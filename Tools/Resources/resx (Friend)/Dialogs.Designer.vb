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

Namespace WindowsT.FormsT
    
    'This class was auto-generated by the StronglyTypedResourceBuilder
    'class via a tool like ResGen or Visual Studio.
    'To add or remove a member, edit your .ResX file then rerun ResGen
    'with the /str option, or rebuild your VS project.
    '''<summary>
    '''  A strongly-typed resource class, for looking up localized strings, etc.
    '''</summary>
    <Global.System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "2.0.0.0"),  _
     Global.System.Diagnostics.DebuggerNonUserCodeAttribute(),  _
     Global.System.Runtime.CompilerServices.CompilerGeneratedAttribute()>  _
    Friend Class Dialogs
        
        Private Shared resourceMan As Global.System.Resources.ResourceManager
        
        Private Shared resourceCulture As Global.System.Globalization.CultureInfo
        
        <Global.System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")>  _
        Friend Sub New()
            MyBase.New
        End Sub
        
        '''<summary>
        '''  Returns the cached ResourceManager instance used by this class.
        '''</summary>
        <Global.System.ComponentModel.EditorBrowsableAttribute(Global.System.ComponentModel.EditorBrowsableState.Advanced)>  _
        Friend Shared ReadOnly Property ResourceManager() As Global.System.Resources.ResourceManager
            Get
                If Object.ReferenceEquals(resourceMan, Nothing) Then
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Tools.Dialogs", GetType(Dialogs).Assembly)
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
        Friend Shared Property Culture() As Global.System.Globalization.CultureInfo
            Get
                Return resourceCulture
            End Get
            Set
                resourceCulture = value
            End Set
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to About {0}.
        '''</summary>
        Friend Shared ReadOnly Property About0() As String
            Get
                Return ResourceManager.GetString("About0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No encoding selected.
        '''</summary>
        Friend Shared ReadOnly Property NoEncodingSelected() As String
            Get
                Return ResourceManager.GetString("NoEncodingSelected", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Raised after user clicks OK and dialogs allows him to select encoding.
        '''</summary>
        Friend Shared ReadOnly Property OKClicked_d() As String
            Get
                Return ResourceManager.GetString("OKClicked_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Specifies code page of encoding that will be preselected when dialog is shown (if available). Set to negative value in order to preselect no encoding.
        '''</summary>
        Friend Shared ReadOnly Property Preselected_d() As String
            Get
                Return ResourceManager.GetString("Preselected_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Text to decode using selected encoding and show as preview.
        '''</summary>
        Friend Shared ReadOnly Property PreviewBytes_d() As String
            Get
                Return ResourceManager.GetString("PreviewBytes_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Text to encode using selected encoding and inform user if all characters can be encoded.
        '''</summary>
        Friend Shared ReadOnly Property PreviewString_d() As String
            Get
                Return ResourceManager.GetString("PreviewString_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Defines if user can select encoding that can be used only with problems on given PreviewString on PreviewBytes.
        '''</summary>
        Friend Shared ReadOnly Property RequireCorrect_d() As String
            Get
                Return ResourceManager.GetString("RequireCorrect_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Select encoding please..
        '''</summary>
        Friend Shared ReadOnly Property SelectEncodingPlease() As String
            Get
                Return ResourceManager.GetString("SelectEncodingPlease", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Determines if help button will be shown.
        '''</summary>
        Friend Shared ReadOnly Property ShowHelp_d() As String
            Get
                Return ResourceManager.GetString("ShowHelp_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Text displayed in title of window.
        '''</summary>
        Friend Shared ReadOnly Property Text_d() As String
            Get
                Return ResourceManager.GetString("Text_d", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Select encoding.
        '''</summary>
        Friend Shared ReadOnly Property Text_dv() As String
            Get
                Return ResourceManager.GetString("Text_dv", resourceCulture)
            End Get
        End Property
    End Class
End Namespace