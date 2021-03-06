Imports EnvDTE80
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Forms
Imports System
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Runtime.InteropServices
Imports EnvDTE

''' <summary>Wraps objects of type <see cref="CodeClass2"/> and <see cref="CodeStruct2"/></summary>
''' <version version="1.5.3">This class is new in version 1.5.3</version>
Public Class ClassProvider : Implements CodeElement, CodeElement2
    Private ReadOnly _element As CodeElement
#Region "CTors"
    ''' <summary>CTor from <see cref="CodeClass2"/></summary>
    ''' <param name="obj">A <see cref="CodeClass2"/></param>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> is null</exception>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public Sub New(ByVal obj As CodeClass)
        If obj Is Nothing Then Throw New ArgumentNullException("obj")
        _element = obj
    End Sub
    ''' <summary>CTor from <see cref="CodeStruct2"/></summary>
    ''' <param name="obj">A <see cref="CodeStruct2"/></param>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> is null</exception>
    ''' <version version="1.5.4">Fix: The <see cref="ArgumentNullException"/> is now throw as documented</version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public Sub New(ByVal obj As CodeStruct)
        If obj Is Nothing Then Throw New ArgumentNullException("obj")
        _element = obj
    End Sub
    ''' <summary>CTor from <see cref="CodeInterface2"/></summary>
    ''' <param name="obj">A <see cref="CodeInterface2"/></param>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> is null</exception>
    ''' <version version="1.5.4">This CTor is new in version 1.5.4</version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public Sub New(ByVal obj As CodeInterface)
        If obj Is Nothing Then Throw New ArgumentNullException("obj")
        _element = obj
    End Sub
    ''' <summary>CTor from <see cref="CodeElement2"/></summary>
    ''' <param name="obj">A <see cref="CodeElement2"/> representing either Class, Structure, or Interface</param>
    ''' <exception cref="ArgumentNullException"><paramref name="obj"/> is null</exception>
    ''' <exception cref="ArgumentException"><paramref name="obj"/>.<see cref="CodeElement2.Kind">Kind</see> is neither <see cref="vsCMElement.vsCMElementClass"/> nor <see cref="vsCMElement.vsCMElementInterface"/> nor <see cref="vsCMElement.vsCMElementStruct"/></exception>
    ''' <version version="1.5.4">This CTor is new in version 1.5.4</version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public Sub New(ByVal obj As CodeElement)
        If obj Is Nothing Then Throw New ArgumentNullException("obj")
        Select Case obj.Kind
            Case vsCMElement.vsCMElementClass, vsCMElement.vsCMElementInterface, vsCMElement.vsCMElementStruct
            Case Else : Throw New ArgumentException(String.Format("Object of type {0} is not supported", obj.Kind), "obj")
        End Select
        _element = obj
    End Sub
#End Region
#Region "Element"
    ''' <summary>Gets internal element this instance is wrapping</summary>
    ''' <version version="1.5.4">Type of property changed from <see cref="CodeElement2"/> to <see cref="CodeElement"/></version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property Element() As CodeElement
        Get
            Return _element
        End Get
    End Property

    ''' <summary>Gets <see cref="Element"/> as <see cref="CodeClass2"/></summary>
    ''' <version version="1.5.4">This property is new in version 1.5.4</version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property AsCodeClass() As CodeClass
        Get
            Return Element
        End Get
    End Property
    ''' <summary>Gets <see cref="Element"/> as <see cref="CodeStruct2"/></summary>
    ''' <version version="1.5.4">This property is new in version 1.5.4</version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property AsCodeStruct() As CodeStruct
        Get
            Return Element
        End Get
    End Property
    ''' <summary>Gets <see cref="Element"/> as <see cref="CodeInterface2"/></summary>
    ''' <version version="1.5.4">This property is new in version 1.5.4</version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property AsCodeInterface() As CodeInterface
        Get
            Return Element
        End Get
    End Property
#End Region
#Region "CodeElement"
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property Collection() As EnvDTE.CodeElements Implements EnvDTE.CodeElement.Collection, CodeElement2.Collection
        Get
            Return Element.Collection
        End Get
    End Property
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property DTE() As EnvDTE.DTE Implements EnvDTE.CodeElement.DTE, CodeElement2.DTE
        Get
            Return Element.DTE
        End Get
    End Property
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property EndPoint() As EnvDTE.TextPoint Implements EnvDTE.CodeElement.EndPoint, CodeElement2.EndPoint
        Get
            Return Element.EndPoint
        End Get
    End Property

    Public ReadOnly Property Extender(ByVal ExtenderName As String) As Object Implements EnvDTE.CodeElement.Extender, CodeElement2.Extender
        Get
            Return Element.Extender(ExtenderName)
        End Get
    End Property

    Public ReadOnly Property ExtenderCATID() As String Implements EnvDTE.CodeElement.ExtenderCATID, CodeElement2.ExtenderCATID
        Get
            Return Element.ExtenderCATID
        End Get
    End Property

    Public ReadOnly Property ExtenderNames() As Object Implements EnvDTE.CodeElement.ExtenderNames, CodeElement2.ExtenderNames
        Get
            Return Element.ExtenderNames
        End Get
    End Property

    Public ReadOnly Property FullName() As String Implements EnvDTE.CodeElement.FullName, CodeElement2.FullName
        Get
            Return Element.FullName
        End Get
    End Property
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public Function GetEndPoint(Optional ByVal Part As EnvDTE.vsCMPart = EnvDTE.vsCMPart.vsCMPartWholeWithAttributes) As EnvDTE.TextPoint Implements EnvDTE.CodeElement.GetEndPoint, CodeElement2.GetEndPoint
        Return Element.GetEndPoint(Part)
    End Function
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public Function GetStartPoint(Optional ByVal Part As EnvDTE.vsCMPart = EnvDTE.vsCMPart.vsCMPartWholeWithAttributes) As EnvDTE.TextPoint Implements EnvDTE.CodeElement.GetStartPoint, CodeElement2.GetStartPoint
        Return Element.GetStartPoint(Part)
    End Function
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property Children() As EnvDTE.CodeElements Implements EnvDTE.CodeElement.Children, CodeElement2.Children
        Get
            Return Element.Children
        End Get
    End Property
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property InfoLocation() As EnvDTE.vsCMInfoLocation Implements EnvDTE.CodeElement.InfoLocation, CodeElement2.InfoLocation
        Get
            Return Element.InfoLocation
        End Get
    End Property

    Public ReadOnly Property IsCodeType() As Boolean Implements EnvDTE.CodeElement.IsCodeType, CodeElement2.IsCodeType
        Get
            Return Element.IsCodeType
        End Get
    End Property
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property Kind() As EnvDTE.vsCMElement Implements EnvDTE.CodeElement.Kind, CodeElement2.Kind
        Get
            Return Element.Kind
        End Get
    End Property

    Public ReadOnly Property Language() As String Implements EnvDTE.CodeElement.Language, CodeElement2.Language
        Get
            Return Element.Language
        End Get
    End Property


    Public Overloads Property Name() As String Implements EnvDTE.CodeElement.Name, CodeElement2.Name
        Get
            Return Element.Name
        End Get
        Set(ByVal value As String)
            Element.Name = value
        End Set
    End Property
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property ProjectItem() As EnvDTE.ProjectItem Implements EnvDTE.CodeElement.ProjectItem, CodeElement2.ProjectItem
        Get
            Return Element.ProjectItem
        End Get
    End Property
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property StartPoint() As EnvDTE.TextPoint Implements EnvDTE.CodeElement.StartPoint, CodeElement2.StartPoint
        Get
            Return Element.StartPoint
        End Get
    End Property


#End Region
#Region "CodeElement2"
    Public ReadOnly Property ElementID() As String Implements EnvDTE80.CodeElement2.ElementID
        Get
            Return Element.ElementID
        End Get
    End Property

    Public Sub RenameSymbol(ByVal NewName As String) Implements EnvDTE80.CodeElement2.RenameSymbol
        Element.RenameSymbol(NewName)
    End Sub
#End Region
#Region "Common properties"
    ''' <summary>Gets a collection of classes from which this item derives.</summary>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property Bases() As CodeElements
        Get
            Select Case Element.Kind
                Case vsCMElement.vsCMElementClass : Return AsCodeClass.Bases
                Case vsCMElement.vsCMElementInterface : Return AsCodeInterface.Bases
                Case vsCMElement.vsCMElementStruct : Return AsCodeStruct.Bases
                Case Else : Return Nothing
            End Select
        End Get
    End Property
    ''' <summary>Gets value indicating if this type is generic type</summary>
    ''' <version version="1.5.4">This property is new in version 1.5.4</version>
    Public ReadOnly Property IsGeneric() As Boolean
        Get
            Select Case Element.Kind
                Case vsCMElement.vsCMElementClass : Return AsCodeClass.IsGeneric
                Case vsCMElement.vsCMElementInterface : Return AsCodeInterface.IsGeneric
                Case vsCMElement.vsCMElementStruct : Return AsCodeStruct.IsGeneric
                Case Else : Return Nothing
            End Select
        End Get
    End Property
    ''' <summary>Gets value indicatig if this type is abstract</summary>
    ''' <version version="1.5.4">This property is new in version 1.5.4</version>
    Public ReadOnly Property IsAbstract() As Boolean
        Get
            Select Case Element.Kind
                Case vsCMElement.vsCMElementClass : Return AsCodeClass.IsAbstract
                Case vsCMElement.vsCMElementInterface : Return True
                Case vsCMElement.vsCMElementStruct : Return AsCodeStruct.IsAbstract
                Case Else : Return Nothing
            End Select
        End Get
    End Property
    ''' <summary>Gets immediate parent of current code object.</summary>
    ''' <returns>Typically returns <see cref="CodeNamespace"/> but for for nested types other typeo of parent can be returned</returns>
    ''' <version version="1.5.4">This property is new in version 1.5.4</version>
    Public ReadOnly Property Parent() As Object
        Get
            Select Case Element.Kind
                Case vsCMElement.vsCMElementClass : Return AsCodeClass.Parent
                Case vsCMElement.vsCMElementInterface : Return AsCodeInterface.Parent
                Case vsCMElement.vsCMElementStruct : Return AsCodeStruct.Parent
                Case Else : Return Nothing
            End Select
        End Get
    End Property

    ''' <summary>Gets members of this type</summary>
    ''' <version version="1.5.4">This property is new in version 1.5.4</version>
    ''' <version version="1.5.4">Added <see cref="CLSCompliantAttribute"/>(False)</version>
    <CLSCompliant(False)>
    Public ReadOnly Property Members() As CodeElements
        Get
            Select Case Element.Kind
                Case vsCMElement.vsCMElementClass : Return AsCodeClass.Members
                Case vsCMElement.vsCMElementInterface : Return AsCodeInterface.Members
                Case vsCMElement.vsCMElementStruct : Return AsCodeStruct.Members
                Case Else : Return Nothing
            End Select
        End Get
    End Property
#End Region
End Class
