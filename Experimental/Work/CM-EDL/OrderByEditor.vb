Imports System.ComponentModel, System.Windows.Forms, System.Drawing
''' <summary>Edits list of sorting columns for SQL query</summary>
<DefaultBindingProperty("ORDER_BY")> _
<DefaultProperty("ORDER_BY")> _
<DefaultEvent("ORDER_BYChanged")> _
Public Class OrderByEditor
    ''' <summary>First character that deremines tah column is used for DESC sorting</summary>
    ''' <remarks>Text of <see cref="ListBox"/> item must ALWAYS begin with either <see cref="DESC"/> or <see cref="ASC"/>!!!</remarks>
    Private Const DESC As Char = "↓"c
    ''' <summary>First character that determines that column is used for ASC sorting</summary>
    ''' <remarks>Text of <see cref="ListBox"/> item must ALWAYS begin with either <see cref="DESC"/> or <see cref="ASC"/>!!!</remarks>
    Private Const ASC As Char = "↑"c
    ''' <summary>Text of the ORDER BY clausule (without 'ORDER BY')</summary>
    <Bindable(True), Category("Data"), Description("Text of ORDER BY clausule")> _
    Public Property ORDER_BY() As String
        Get
            Dim ret As New System.Text.StringBuilder
            For Each item As String In lstRight.Items
                If ret.Length <> 0 Then ret.Append(", ")
                If item.StartsWith(DESC) Then
                    ret.Append(DBName(item.Substring(1)) & " DESC")
                Else
                    ret.Append(DBName(item.Substring(1)) & " ASC")
                End If
            Next item
            Return ret.ToString
        End Get
        Set(ByVal value As String)
            Dim Old As String = ORDER_BY
            Dim parts As String() = value.Split(New Char() {","}, StringSplitOptions.RemoveEmptyEntries)
            lstRight.Items.Clear()
            For Each item As String In parts
                Dim Part As String = item.Trim
                Dim ItemName As String = DisplayName(item.Split(New Char() {" "}, StringSplitOptions.RemoveEmptyEntries)(0))
                If Part.ToLower.EndsWith("desc") Then
                    lstRight.Items.Add(DESC & ItemName)
                Else
                    lstRight.Items.Add(ASC & ItemName)
                End If
            Next item
            ApplyPossibleValues()
            If Old <> ORDER_BY Then OnORDER_BYChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Raises tha <see cref="ORDER_BYChanged"/> event</summary>
    ''' <param name="e">Event parameters</param>         
    Protected Overridable Sub OnORDER_BYChanged(ByVal e As EventArgs)
        RaiseEvent ORDER_BYChanged(Me, e)
    End Sub
    ''' <summary>Raised after value of <see cref="ORDER_BY"/> changes</summary>
    <Category("Value Changed")> _
    <Description("Raised after value of ORDER_BY changes")> _
    Public Event ORDER_BYChanged As EventHandler
    ''' <summary>Contains value of the <see cref="PossibleValues"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _PossibleValues As New Dictionary(Of String, String)
    ''' <summary>Possible columns for sorting</summary>
    ''' <remarks>
    ''' <para><see cref="KeyValuePair(Of String, String).Key">Keys</see> contains names of columns as used in database; <see cref="KeyValuePair(Of String, String).Value">Values</see> contains names of columns as displayed to user.</para>
    ''' <para>!!! Both, key and values, MUST be UNIQUE! Threre MUST NOT be two keys with same value, two values with same value or key with same value as any value.</para>
    ''' <para>It is necessary to call <see cref="ApplyPossibleValues"/> in oredr changes made in <see cref="PossibleValues"/> to be shown to user. This is not necessary when change of <see cref="ORDER_BY"/> immediatelly follows changes of <see cref="PossibleValues"/></para>
    ''' </remarks>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public ReadOnly Property PossibleValues() As Dictionary(Of String, String)
        Get
            Return _PossibleValues
        End Get
    End Property
    ''' <summary>Applyes changes in <see cref="PossibleValues"/> when taking care of columns currently selected</summary>
    ''' <remarks><para>
    ''' If right list contains value present in <see cref="PossibleValues"/> it is not added to left list.
    ''' If right list contains key present in <see cref="PossibleValues"/> it is not added to left list and the item in right list is replaced by corresponding value.
    ''' If right list contains neither key nor value value is added to the left list.
    ''' </para>
    ''' <para>This MUST becalled after updates in <see cref="PossibleValues"/> unless they are immediatelly followed by change of <see cref="ORDER_BY"/></para>
    ''' </remarks>
    Public Sub ApplyPossibleValues()
        lstLeft.Items.Clear()
        For Each item As KeyValuePair(Of String, String) In PossibleValues
            Dim SelI As Integer = 0
            Dim IsSelected As Boolean = False
            For Each selected As String In lstRight.Items
                If selected.Substring(1).ToLower = item.Key.ToLower Then
                    lstRight.Items(SelI) = selected(0) & item.Value
                    IsSelected = True
                ElseIf selected.Substring(1).ToLower = item.Value.ToLower Then
                    IsSelected = True
                End If
                SelI += 1
            Next selected
            If Not IsSelected Then
                lstLeft.Items.Add(item.Value)
            End If
        Next item
    End Sub
    ''' <summary>Convers column name into display name</summary>
    ''' <param name="DBName">Column name (as used in database)</param>
    ''' <returns>Display name (as displayed to user)</returns>
    Private Function DisplayName(ByVal DBName As String) As String
        For Each Pair As KeyValuePair(Of String, String) In PossibleValues
            If Pair.Key.ToLower = DBName.ToLower Then Return Pair.Value
        Next Pair
        Return DBName
    End Function
    ''' <summary>Converts display name into column name</summary>
    ''' <param name="DisplayName">Display name (as dsilayed to user)</param>
    ''' <returns>Column name (as used in database)</returns>
    Private Function DBName(ByVal DisplayName As String) As String
        For Each Pair As KeyValuePair(Of String, String) In PossibleValues
            If Pair.Value.ToLower = DisplayName.ToLower Then Return Pair.Key
        Next Pair
        Return DisplayName
    End Function

    Private Sub lstLeft_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstLeft.SelectedIndexChanged
        cmdRight.Enabled = lstLeft.SelectedItems.Count <> 0
    End Sub

    Private Sub lstRight_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstRight.KeyDown
        If e.KeyCode = Keys.Delete Then
            cmdLeft.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Up AndAlso e.Control Then
            cmdUp.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Down AndAlso e.Control Then
            cmdDown.PerformClick()
            e.Handled = True
        ElseIf e.KeyCode = Keys.Insert Then
            cmdChangeDir.PerformClick()
            e.Handled = True
        End If
    End Sub

    Private Sub lstRight_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lstRight.SelectedIndexChanged
        cmdLeft.Enabled = lstRight.SelectedIndex >= 0
        cmdUp.Enabled = lstRight.SelectedIndex > 0
        cmdDown.Enabled = lstRight.SelectedIndex < lstRight.Items.Count - 1 AndAlso lstRight.SelectedIndex >= 0
        cmdChangeDir.Enabled = lstRight.SelectedIndex >= 0
    End Sub

    Private Sub cmdUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdUp.Click
        If lstRight.SelectedIndex <= 0 Then Exit Sub
        Dim item As String = lstRight.SelectedItem
        Dim OldIndex As Integer = lstRight.SelectedIndex
        lstRight.Items.RemoveAt(OldIndex)
        lstRight.Items.Insert(OldIndex - 1, item)
        lstRight.SelectedIndex = OldIndex - 1
        OnORDER_BYChanged(EventArgs.Empty)
    End Sub

    Private Sub cmdDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdDown.Click
        If lstRight.SelectedIndex < 0 OrElse lstRight.SelectedIndex >= lstRight.Items.Count - 1 Then Exit Sub
        Dim item As String = lstRight.SelectedItem
        Dim OldIndex As Integer = lstRight.SelectedIndex
        lstRight.Items.RemoveAt(OldIndex)
        lstRight.Items.Insert(OldIndex + 1, item)
        lstRight.SelectedIndex = OldIndex + 1
        OnORDER_BYChanged(EventArgs.Empty)
    End Sub

    Private Sub cmdChangeDir_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdChangeDir.Click
        If lstRight.SelectedIndex < 0 Then Exit Sub
        lstRight.Items(lstRight.SelectedIndex) = CStr(IIf(CStr(lstRight.SelectedItem)(0) = ASC, DESC, ASC)) & CStr(lstRight.SelectedItem).Substring(1)
        OnORDER_BYChanged(EventArgs.Empty)
    End Sub

    Private Sub cmdRight_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdRight.Click
        If lstLeft.SelectedItems.Count = 0 Then Exit Sub
        Dim items As New List(Of String)
        For Each Item As String In lstLeft.SelectedItems
            lstRight.Items.Add(ASC & Item)
            items.Add(Item)
        Next Item
        For Each item As String In items
            lstLeft.Items.Remove(item)
        Next item
        OnORDER_BYChanged(EventArgs.Empty)
    End Sub

    Private Sub cmdLeft_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdLeft.Click
        If lstRight.SelectedIndex < 0 Then Exit Sub
        Dim Item As String = CStr(lstRight.SelectedItem).Substring(1)
        Dim index As Integer = lstRight.SelectedIndex
        lstRight.Items.RemoveAt(index)
        If lstRight.Items.Count > 0 AndAlso index > 0 Then
            lstRight.SelectedIndex = index - 1
        ElseIf lstRight.Items.Count > 0 Then
            lstRight.SelectedIndex = 0
        End If
        For Each Possible As KeyValuePair(Of String, String) In PossibleValues
            If Possible.Value.ToLower = Item.ToLower OrElse Possible.Key.ToLower = Item.ToLower Then
                lstLeft.Items.Add(Possible.Value)
                lstLeft.SelectedIndices.Clear()
                lstLeft.SelectedIndex = lstLeft.Items.Count - 1
                Exit For
            End If
        Next Possible
        OnORDER_BYChanged(EventArgs.Empty)
    End Sub
    ''' <summary>Gets or sets the font of the text displayed by the control.</summary>
    ''' <returns>The <see cref="System.Drawing.Font"/> to apply to the text displayed by the control. The default is Arial Unicode MS 8.25pt</returns>
    Public Overrides Property Font() As System.Drawing.Font
        <DebuggerStepThrough()> _
        Get
            Return MyBase.Font
        End Get
        Set(ByVal value As System.Drawing.Font)
            MyBase.Font = value
            For Each Button As Button In New Button() {cmdLeft, cmdRight, cmdUp, cmdDown, cmdChangeDir}
                Button.Font = New Font(Me.Font.FontFamily, Me.Font.SizeInPoints * (14 / 8), Me.Font.Style, GraphicsUnit.Point)
            Next Button
        End Set
    End Property
    ''' <summary>Resets <see cref="Font"/> to its defaul value</summary>
    Public Overrides Sub ResetFont()
        Me.Font = New Font("Arial Unicode MS", 8.25, FontStyle.Regular, GraphicsUnit.Point)
    End Sub
    ''' <summary>True when <see cref="Font"/> differs from its default value</summary>
    Private Function ShouldSerializeFont() As Boolean
        Return Me.Font.FontFamily.Name <> "Arial Unicode MS" OrElse Me.Font.Style <> FontStyle.Regular OrElse Me.Font.SizeInPoints <> 8.25
    End Function

    ''' <summary>CTor</summary>
    Public Sub New()
        CheckFont()
        ' This call is required by the Windows Form Designer.
        InitializeComponent()
        ' Add any initialization after the InitializeComponent() call.
        lblI.Text = String.Format("{0} ... vzestupně{2}{1} ... sestupně", ASC, DESC, vbCrLf)
    End Sub
    ''' <summary>If font of this control is Arial Unicode MS then checks if Arial Unicode MS is present in system. If not tryes to install it privatelly</summary>
    ''' <remarks>Requires file .\Resources\ARIALUNI.TTF</remarks>
    Private Sub CheckFont()
        Try
            Dim f As New Font("Arial Unicode MS", 12)
        Catch
            Dim fc As New Drawing.Text.PrivateFontCollection
            Try
                fc.AddFontFile(IO.Path.Combine(My.Application.Info.DirectoryPath, "Resources\ARIALUNI.TTF"))
            Catch : End Try
        End Try
    End Sub

    Private Sub lstLeft_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles lstLeft.KeyDown
        If e.KeyCode = Keys.Enter Then
            e.Handled = True
            cmdRight.PerformClick()
        End If
    End Sub
End Class
