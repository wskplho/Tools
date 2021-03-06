'' Copyright (c) Microsoft Corporation.  All rights reserved.
''
'' These samples demonstrate how to use the VS Automation Object Model.
'' They are unsupported, but you can use them as-is.  They are not intended 
'' to demonstrate how to implement fully rigorous commands.  For example, 
'' if a command expects there to be selected text, but you invoke
'' the macro on a blank line, the macro's behavior is undefined.
''
'Option Strict Off
'Option Explicit Off

Imports EnvDTE
Imports EnvDTE80
Imports System.Diagnostics
Imports System.Windows
Imports System.Windows.Forms
Imports System
Imports System.Text.RegularExpressions
Imports System.ComponentModel
Imports System.Runtime.InteropServices

''' <summary>Helper functions for this and other modules.</summary>
Friend Module Utilities

    ''' <summary>Gets start of line of XML commend for given or actual document</summary>
    ''' <param name="document">Document to get comment start for. Null to get it for current one.</param>
    ''' <returns>XML comment start-line string used by document identified by file extension</returns>
    ''' <exception cref="NotSupportedException">File name extension is not recognized</exception>
    ''' <version version="1.5.3">Fix: .vb extension is case-sensitive</version>
    Function XMLCommentStart(Optional ByVal document As Document = Nothing) As String
        Dim extension As String

        If (document Is Nothing) Then
            document = DTE.ActiveDocument
        End If

        extension = document.Name.ToLower
        If (extension.EndsWith(".cs") OrElse extension.EndsWith(".cpp") OrElse extension.EndsWith(".h") OrElse extension.EndsWith(".idl") OrElse extension.EndsWith(".jsl")) OrElse extension.EndsWith(".java") OrElse extension.EndsWith(".php") Then
            Return "/// "
        ElseIf (extension.EndsWith(".vb")) Then
            Return "''' "
        Else
            Throw New NotSupportedException("Unrecognized file type. You can add this file type by modifying the function Utilities.XMLCommentStart to include the extension of this file.")
        End If
    End Function
    ''' <summary>Regular expression for parsing Object explorer XML comment text</summary>
    Public regexOB As Text.RegularExpressions.Regex = New Text.RegularExpressions.Regex( _
        "^(\r\n)*                                                                 " & vbCrLf & _
        "(Summary:\r\n(?<Summary>[^\r\n]*))?                                      " & vbCrLf & _
        "((\r\n){2}                                                               " & vbCrLf & _
        "Type\sparameters:                                                        " & vbCrLf & _
        "(\r\n(?<TypeParam>                                                       " & vbCrLf & _
        "(?<TP_Name>[^\r\n:]+):\s*(?<TP_Desc>[^\r\n]*)))*                         " & vbCrLf & _
        ")?                                                                       " & vbCrLf & _
        "((\r\n){2}                                                               " & vbCrLf & _
        "Parameters:                                                              " & vbCrLf & _
        "(\r\n(?<Param>(?<P_Name>[^\r\n:]+):\s*(?<P_Desc>[^\r\n]*)))*             " & vbCrLf & _
        ")?                                                                       " & vbCrLf & _
        "((\r\n){2}                                                               " & vbCrLf & _
        "((Returns)|(Return\sValues)):\r\n(?<Returns>[^\r\n]*))?                  " & vbCrLf & _
        "((\r\n){2}                                                               " & vbCrLf & _
        "Exceptions:                                                              " & vbCrLf & _
        "(\r\n(?<Exception>(?<E_Name>[^\r\n:]+):\s*(?<E_Desc>[^\r\n]*)))*         " & vbCrLf & _
        ")?                                                                       " & vbCrLf & _
        "((\r\n){2}                                                               " & vbCrLf & _
        "Remarks:\r\n(?<Remarks>[^\r\n]*))?                                       " & vbCrLf & _
        "(\r\n\s*)*$", _
        RegexOptions.IgnoreCase _
        Or RegexOptions.ExplicitCapture _
        Or RegexOptions.CultureInvariant _
        Or RegexOptions.IgnorePatternWhitespace _
        Or RegexOptions.Compiled _
        )

    ''' <summary>Gets text stored in clipboard</summary>
    ''' <returns>Text from clkipboard. An empty string if there was not text in clipboard</returns>
    Friend Function GetClipboardText$()
        Dim txt As New TextBox
        txt.Multiline = True
        txt.Paste()
        Dim ret = txt.Text
        If ret IsNot Nothing Then Return ret
        'Try again
        txt.Paste()
        ret = txt.Text
        If ret IsNot Nothing Then Return ret
        'Try another approach
        ret = My.Computer.Clipboard.GetText
        Return ret
    End Function
End Module

''' <summary>An empty <see cref="CodeElements"/> collection</summary>
Friend Class EmptyElements
    Implements CodeElements

    Private ReadOnly _parent As Object
    Public Sub New()
    End Sub
    Public Sub New(ByVal parent As Object)
        Me._parent = parent
    End Sub

    Public ReadOnly Property Count() As Integer Implements EnvDTE.CodeElements.Count
        Get
            Return 0
        End Get
    End Property

    Public Function CreateUniqueID(ByVal Prefix As String, Optional ByRef NewName As String = "0") As Boolean Implements EnvDTE.CodeElements.CreateUniqueID
        Return False
    End Function

    Public ReadOnly Property DTE() As EnvDTE.DTE Implements EnvDTE.CodeElements.DTE
        Get
            Return Nothing
        End Get
    End Property

    Public Function GetEnumerator() As System.Collections.IEnumerator Implements EnvDTE.CodeElements.GetEnumerator, System.Collections.IEnumerable.GetEnumerator
        Return New CodeElement() {}.GetEnumerator
    End Function

    Public Function Item(ByVal index As Object) As EnvDTE.CodeElement Implements EnvDTE.CodeElements.Item
        Throw New IndexOutOfRangeException("index")
    End Function

    Public ReadOnly Property Parent() As Object Implements EnvDTE.CodeElements.Parent
        Get
            Return _parent
        End Get
    End Property

    Private Sub Reserved1(ByVal Element As Object) Implements EnvDTE.CodeElements.Reserved1
        Throw New NotImplementedException
    End Sub

End Class
