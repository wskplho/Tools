﻿'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:4.0.21205.1
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
                    Dim temp As Global.System.Resources.ResourceManager = New Global.System.Resources.ResourceManager("Tools.VisualStudioT.GeneratorsT.ResXFileGenerator.Resources", GetType(Resources).Assembly)
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
        '''  Looks up a localized string similar to Error: {0}.
        '''</summary>
        Friend ReadOnly Property Error0() As String
            Get
                Return ResourceManager.GetString("Error0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Error while generating code:.
        '''</summary>
        Friend ReadOnly Property ErrorWhileGeneratingCode() As String
            Get
                Return ResourceManager.GetString("ErrorWhileGeneratingCode", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Invalid insert index {0}.
        '''</summary>
        Friend ReadOnly Property InvalidInsertIndex0() As String
            Get
                Return ResourceManager.GetString("InvalidInsertIndex0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Invalid part specifier for /l {0}.
        '''</summary>
        Friend ReadOnly Property InvalidPartSpecifierForL0() As String
            Get
                Return ResourceManager.GetString("InvalidPartSpecifierForL0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Usage:
        '''    {0} parameters
        '''Parameters:
        '''    /in &lt;infile&gt; - required, path of input resx file
        '''    /out &lt;outfile&gt; - required, path of output file
        '''    /lang &lt;language&gt; - optional, name of output lanuage. If not specified inferred form outfile extension
        '''        supported values (case-insensitive) are: vb, cs, c, cpp, h, cpp.7, cpp.vs, js, java, jsl
        '''        cpp.7 and cpp.vs are two alternative C++ providers
        '''        c, cpp and h have same meaning (C++)
        '''        java and jsl have same meaning (J#)
        '''    /nolo [rest of string was truncated]&quot;;.
        '''</summary>
        Friend ReadOnly Property Legend() As String
            Get
                Return ResourceManager.GetString("Legend", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Missing infile specification.
        '''</summary>
        Friend ReadOnly Property MissingInfileSpecification() As String
            Get
                Return ResourceManager.GetString("MissingInfileSpecification", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Missing outfile specification.
        '''</summary>
        Friend ReadOnly Property MissingOutfileSpecification() As String
            Get
                Return ResourceManager.GetString("MissingOutfileSpecification", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Missing parameter /in.
        '''</summary>
        Friend ReadOnly Property MissingParameterIn() As String
            Get
                Return ResourceManager.GetString("MissingParameterIn", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Missing parameter /out.
        '''</summary>
        Friend ReadOnly Property MissingParameterOut() As String
            Get
                Return ResourceManager.GetString("MissingParameterOut", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to No output was generated.
        '''</summary>
        Friend ReadOnly Property NoOutputWasGenerated() As String
            Get
                Return ResourceManager.GetString("NoOutputWasGenerated", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to /spliton and /out2 must be specified both or none..
        '''</summary>
        Friend ReadOnly Property SplitonAndOut2MustBeSpecifiedBothOrNone() As String
            Get
                Return ResourceManager.GetString("SplitonAndOut2MustBeSpecifiedBothOrNone", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Unable to split generated text.
        '''</summary>
        Friend ReadOnly Property UnableToSplitGeneratedText() As String
            Get
                Return ResourceManager.GetString("UnableToSplitGeneratedText", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Unknown language {0}.
        '''</summary>
        Friend ReadOnly Property UnknownLanguage0() As String
            Get
                Return ResourceManager.GetString("UnknownLanguage0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Warning: {0}.
        '''</summary>
        Friend ReadOnly Property Warning0() As String
            Get
                Return ResourceManager.GetString("Warning0", resourceCulture)
            End Get
        End Property
        
        '''<summary>
        '''  Looks up a localized string similar to Warning: Incomplete /l argument ignored..
        '''</summary>
        Friend ReadOnly Property WarningIncompleteLArgumentIgnored() As String
            Get
                Return ResourceManager.GetString("WarningIncompleteLArgumentIgnored", resourceCulture)
            End Get
        End Property
    End Module
End Namespace
