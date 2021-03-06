Imports System.ComponentModel
Imports Tools.CollectionsT.GenericT, Tools.ComponentModelT
Imports Tools.VisualBasicT
Imports System.Windows.Forms
Imports System.Drawing

''' <summary>Denní kalendář</summary>
<DefaultEvent("ItemDoubleClick")> _
<DefaultProperty("RowsDataSource")> _
Partial Public Class DayView : Inherits UserControl
#Region "CTors"
    ''' <summary>CTor</summary>
    Public Sub New()
        'Me.SetStyle(ControlStyles.Selectable, False)
        'Me.SetStyle(ControlStyles.ContainerControl, True)
        InitializeComponent()
        tlpMain.RowCount = 0
        _Start = Now.Date + New TimeSpan(7, 30, 0)
        IsStartChanged = False
        _TimeWindow = New TimeSpan(13, 30, 0)
        SlotWidth = 30
        SetHSBProperties()
        ApplyFirstColumn()
        OldFont = Me.Font
    End Sub
#End Region

#Region "Controls"
    ''' <summary>Labely záhlaví sloupců</summary>
    Private Headers As Label()
    ''' <summary>Labely reprezentující záhlaví řádků</summary>
    Private RowHeaders As New List(Of Label)
    ''' <summary>Místa pro obsahy řádků</summary>
    Private RowPanes As New List(Of Panel)
    ''' <summary>Seznam seznamů ovládacích prvků reprezentujících zobrazené položky</summary>
    Private RowItems As New List(Of List(Of ItemToolStrip))
#End Region

#Region "Vlastnosti zobrazení"
#Region "MinimumSize"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="MinimumSize"/></summary>
    Private _MinimumSize As Size
    ''' <summary>Gets or sets the size that is the lower limit that System.Windows.Forms.Control.GetPreferredSize(System.Drawing.Size) can specify.</summary>
    ''' <returns>An ordered pair of type System.Drawing.Size representing the width and height of a rectangle.</returns>
    ''' <remarks>This is minimum size set from outside. Control's nature requires that actual size is always greater than or equal to <seealso cref="MinimumMinimumSize"/>. Actual minimum size can be obtained via non-shadowed <see cref="UserControl.MinimumSize"/></remarks>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Layout)> _
    <SRDescription("ControlMinimumSizeDescr")> _
    Public Shadows Property MinimumSize() As System.Drawing.Size
        <DebuggerStepThrough()> Get
            Return _MinimumSize
        End Get
        Set(ByVal value As System.Drawing.Size)
            With MinimumSize
                _MinimumSize = New Size(Math.Max(value.Width, .Width), Math.Max(value.Height, .Height))
            End With
            ApplyMinimumSize()
        End Set
    End Property
    ''' <summary>Minimální povolená hodnota pro <see cref="UserControl.MinimumSize"/></summary>
    <Browsable(False), KnownCategory(KnownCategoryAttribute.KnownCategories.Layout)> _
    Public ReadOnly Property MinimumMinimumSize() As Size
        Get
            Return New Size( _
                32 * SlotsPerwindow + vsbVert.Width + tlpHead.GetColumnWidths(0), _
                16 + tlpHead.Height + hsbHoriz.Height)
        End Get
    End Property
#End Region
#Region "Start"
    ''' <summary>Nastane po změně vlastnosti <see cref="Start"/></summary>
    <Category("Property Changed")> _
    <Description("Raised after value of the Star property is changed.")> _
    Public Event StartChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="StartChanged"/></summary>
    Protected Overridable Sub OnStartChanged(ByVal e As EventArgs)
        For Each rp As Panel In Me.RowPanes : rp.Invalidate() : Next rp
        RaiseEvent StartChanged(Me, e)
    End Sub
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="Start"/></summary>
    Dim _Start As Date
    ''' <summary>Datum a čas začátku zobrazení</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Date and time when shown interval starts")> _
    Public Property Start() As Date
        <DebuggerStepThrough()> Get
            Return _Start
        End Get
        Set(ByVal value As Date)
            If value <> Start Then
                SetStart(value)
            End If
        End Set
    End Property
    ''' <summary>Nastaví hodnotu vlastnosti <see cref="Start"/> bez ohledu ne její původní hodnotu</summary>
    Private Sub SetStart(ByVal value As Date)
        Dim NewStart As Date = value.Date
        While NewStart.AddMinutes(SlotWidth) <= value
            NewStart = NewStart.AddMinutes(SlotWidth)
        End While
        Dim EndD As Date = Start + TimeWindow
        If EndD.Date <> Start.Date AndAlso Not (EndD.Date.AddDays(-1) = Start.Date AndAlso EndD.TimeOfDay = TimeSpan.Zero) Then
            NewStart = value.Date.AddDays(1) - TimeWindow
        End If
        If NewStart <> Start Then
            _Start = NewStart
            SetLabelTexts()
            IsStartChanged = True
            Me.HorizontalScroll.Value = Start.Hour * 60 + Start.Minute
            'Přesouvání/zobrazování/skrývání položek v závislosti na scrollování
            Dim RowI As Integer = 0
            For Each Row As DayViewRow In Me.Rows
                For Each Item As DayViewItem In Row.Records
                    Dim ItemToolStrip As ItemToolStrip = GetItemToolStrip(RowI, Item)
                    If ItemToolStrip IsNot Nothing Then
                        Dim OtherDay As Boolean = Item.Start.Date <> Me.Start.Date
                        Dim Right As Integer
                        Dim Left As Integer
                        If Not OtherDay Then
                            Left = LeftFromStart(Item.Start)
                            Right = RightFromEnd(Item.End)
                        End If
                        If OtherDay OrElse (Left < 0 AndAlso Right <= 0 OrElse Left >= RowPanes(RowI).Width AndAlso Right > RowPanes(RowI).Width) Then 'Odstranit co není v průzoru
                            RowPanes(RowI).Controls.Remove(ItemToolStrip)
                            RowItems(RowI).Remove(ItemToolStrip)
                            RemoveItemHandlers(ItemToolStrip)
                        Else
                            ItemToolStrip.Left = Left
                            ItemToolStrip.Width = Right - Left
                        End If
                    Else
                        AddNewItemToolStripIfItIsInWindow(Item, RowI)
                    End If
                Next Item
                RowI += 1
            Next Row
            OnStartChanged(EventArgs.Empty)
        End If
    End Sub
    ''' <summary>Určuje jestli vlastnost <see cref="Start"/> byla změněna</summary>
    Private IsStartChanged As Boolean
    ''' <summary>Určuje jestli designer má serializovat vlastnost <see cref="Start"/></summary>
    <DebuggerStepThrough()> Private Function ShouldSerializeStart() As Boolean
        Return IsStartChanged
    End Function
    ''' <summary>Resetuje hodnotu vlastnosti <see cref="Start"/></summary>
    Private Sub ResetStart()
        Start = Now.Date + New TimeSpan(7, 30, 0)
        IsStartChanged = False
    End Sub
#End Region
#Region "SlotWidth a TimeWindow"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="TimeWindow"/></summary>
    Dim _TimeWindow As TimeSpan
    ''' <summary>Šířka slotu (buňky) v minutách</summary>
    ''' <remarks>Musí být dělitelem počtu minut <see cref="TimeWindow"/></remarks>
    ''' <exception cref="ArgumentException">Nastavovaná hodnota není dělitelem počtu minut <see cref="TimeWindow"/></exception>
    <DefaultValue(GetType(UShort), "30")> _
    <Category("Appearance")> _
    <Description("Width of time slot - each time slot has its own header with time printed")> _
    <CLSCompliant(False)> _
    Public Property SlotWidth() As UShort
        Get
            If Headers Is Nothing OrElse Headers.Length = 0 Then Return 0
            Return TimeWindow.TotalMinutes / Headers.Length
        End Get
        Set(ByVal value As UShort)
            If TimeWindow.TotalMinutes Mod value <> 0 Then Throw New ArgumentException("value", "SlotWidth musí být dělitelem počtu minut TimeWindow")
            If value <> SlotWidth Then
                Me.SuspendLayout()
                Try
                    RemoveLabels()
                    ReDim Headers(TimeWindow.TotalMinutes / value - 1)
                    ChangeColumnNo()
                    SetStart(Start)
                    SetHSBProperties()
                Finally
                    Me.ResumeLayout()
                End Try
                If SlotWidth Mod MinimalUserChange <> 0 Then MinimalUserChange = SlotWidth
                OnSlotWidthChanged(EventArgs.Empty)
            End If
        End Set
    End Property
    ''' <summary>Nastane po změně vlastnosti <see cref="SlotWidth"/></summary>
    <Category("Property Changed")> _
    <Description("Raised after value of the SlotWidth property is changed.")> _
    Public Event SlotWidthChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="SlotWidthChanged"/></summary>
    <DebuggerStepThrough()> Protected Overridable Sub OnSlotWidthChanged(ByVal e As EventArgs)
        RaiseEvent SlotWidthChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně vlastnosti <see cref="TimeWindow"/></summary>
    <Category("Property Changed")> _
    <Description("Raised after value of the TimeWindow property is changed.")> _
    Public Event TimeWindowChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="TimeWindowChanged"/></summary>
    <DebuggerStepThrough()> Protected Overridable Sub OnTimeWindow(ByVal e As EventArgs)
        RaiseEvent TimeWindowChanged(Me, e)
    End Sub
    ''' <summary>Počet slotů šířky <see cref="SlotWidth"/>, které se vejdou do <see cref="TimeWindow"/></summary>
    ''' <returns><see cref="TimeWindow">Timewindow</see>.<see cref="TimeSpan.TotalMinutes">TotalMinutes</see> / <see cref="SlotWidth">SlotWidth</see></returns>
    <Browsable(False)> _
    Public ReadOnly Property SlotsPerwindow() As Integer
        Get
            If SlotWidth = 0 Then Return 0
            Return TimeWindow.TotalMinutes / SlotWidth
        End Get
    End Property
    ''' <summary>Časová šířka zobrazené oblasti</summary>
    ''' <remarks>Počet minut musý být dělitelný <see cref="SlotWidth"/>, počet sub-minutových částí (sekundy, ...) musí být 0</remarks>
    ''' <exception cref="ArgumentException">Nastavovaná hodnota není v celých minutách nebo není dělitelná <see cref="SlotWidth"/></exception>
    ''' <exception cref="ArgumentOutOfRangeException">Nastavovaná hodnota je větší než 24h nebo menší nebo rovna <see cref="TimeSpan.Zero"/></exception>
    <DefaultValue(GetType(TimeSpan), "13:30:00")> _
    <Category("Appearance")> _
    <Description("Width of time window that is displayed by the control")> _
    Public Property TimeWindow() As TimeSpan
        <DebuggerStepThrough()> Get
            Return _TimeWindow
        End Get
        Set(ByVal value As TimeSpan)
            If value.Seconds <> 0 OrElse value.Milliseconds <> 0 Then Throw New ArgumentException("Šířka časového okna musí být určena v celých minutách")
            If value.TotalMinutes Mod SlotWidth <> 0 Then Throw New ArgumentException("Šířka časového okna v minutách musí být dělitelná šířkou slotu")
            If value.TotalHours > 24 OrElse value < TimeSpan.Zero Then Throw New ArgumentOutOfRangeException("value", "Šířka časového okna může být maximálně 24 hodin a musí být větší něž 0")

            If value <> TimeWindow Then
                Dim OSW As UShort = SlotWidth
                _TimeWindow = value
                Me.SuspendLayout()
                Try
                    RemoveLabels()
                    ReDim Headers(value.TotalMinutes / OSW - 1)
                    ChangeColumnNo()
                    SetHSBProperties()
                    SetStart(Start)
                Finally
                    Me.ResumeLayout()
                End Try

            End If
        End Set
    End Property
#End Region
#Region "MinimalUserChange"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="MinimalUserChange"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _MinimalUserChange As UShort = 15US
    ''' <summary>Minimální změna v minutách kterou může uživatel udělat</summary>
    ''' <remarks>Musí se jednat o dělitele <see cref="SlotWidth"/></remarks>
    ''' <exception cref="ArgumentException">Nastavovaná hodnota není dělitelem <see cref="SlotWidth"/></exception>
    ''' <exception cref="ArgumentOutOfRangeException">Nastavovaná hodnota je 0</exception>
    <DefaultValue(GetType(UShort), "15")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Behavior)> _
    <Description("Minimal change (in minutes) that can be done by user. This causes that user can place items only in discrete steps from start to end. Minimal value is 1 minute")> _
    <CLSCompliant(False)> _
    Public Property MinimalUserChange() As UShort
        <DebuggerStepThrough()> Get
            Return _MinimalUserChange
        End Get
        Set(ByVal value As UShort)
            If value = 0 Then Throw New ArgumentOutOfRangeException("value", value, "MinimalUserChange nemůže být 0")
            If SlotWidth Mod value <> 0 Then Throw New ArgumentException("MinimalUserChange musí být dělitelem SlotWidth")
            _MinimalUserChange = value
            OnMinimalUserChangeChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Vyvolává událost <see cref="MinimalUserChangeChanged"/></summary>
    ''' <param name="e">Parametry události</param>
    <DebuggerStepThrough()> Protected Overridable Sub OnMinimalUserChangeChanged(ByVal e As EventArgs)
        RaiseEvent MinimalUserChangeChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně vlastnosti <see cref="MinimalUserChange"/></summary>
    <Description("Raised after value of the MinimalUserChnage property is changed.")> _
    <Category("Property Changed")> Public Event MinimalUserChangeChanged As EventHandler
#End Region
#Region "MinimumItemWidth"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="MinimumItemwidth"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _MinimumItemWidth As UShort = 15US
    ''' <summary>Minimální povolená šířka položky (v minutách)</summary>
    <DefaultValue(GetType(UShort), "15")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Behavior)> _
    <Description("Minimal width of item in minutes. This causes that user cannot create shorter record than specified amount of minutes")> _
    <CLSCompliant(False)> _
    Public Property MinimumItemWidth() As UShort
        <DebuggerStepThrough()> Get
            Return _MinimumItemWidth
        End Get
        Set(ByVal value As UShort)
            If value = 0 Then Throw New ArgumentOutOfRangeException("value", value, "MinimumItemWidth nemůže být 0")
            Dim old As UShort = MinimumItemWidth
            _MinimumItemWidth = value
            If old <> value Then OnMinimumItemWidthChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Vyvolává událost <see cref="MinimumItemWidthChanged"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnMinimumItemWidthChanged(ByVal e As EventArgs)
        RaiseEvent MinimumItemWidthChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně vlastnosti <see  cref="MinimumItemWidth"/></summary>
    <Category("Property Changed")> _
    <Description("Raised after value of the MinimumItemWidth property is changed.")> _
    Public Event MinimumItemWidthChanged As EventHandler
#End Region
#Region "Morning a Evening"
#Region "Morning"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="Morning"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _Morning As TimeSpan = New TimeSpan(7, 30, 0)
    ''' <summary>Čas kdy končí období označené jako ráno</summary>
    ''' <remarks>Pokud nechcete používat nastavte na 0:00:00 nebo nižší.</remarks>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Behavior)> _
    <DefaultValue(GetType(TimeSpan), "7:30:00")> _
    <Description("Time when so called morning period ends")> _
    Public Property Morning() As TimeSpan
        <DebuggerStepThrough()> Get
            Return _Morning
        End Get
        Set(ByVal value As TimeSpan)
            If value > Evening Then Throw New ArgumentException("Morning must be smaller (or equal to) than evening")
            Dim old As TimeSpan = Morning
            _Morning = value
            If Morning <> old Then OnMorningChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Vyvolává událost <see cref="MorningChanged"/></summary>
    ''' <param name="e">Parametry</param>
    Protected Overridable Sub OnMorningChanged(ByVal e As EventArgs)
        For Each rp As Panel In Me.RowPanes : rp.Invalidate() : Next rp
        tlpHead.Invalidate()
        RaiseEvent MorningChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně hodnoty vlastnosti <see cref="Morning"/></summary>
    <Description("Raised after value of the Morning property is changed.")> _
    <Category("Property Changed")> Public Event MorningChanged As EventHandler
#End Region
#Region "Evening"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="Evening"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _Evening As TimeSpan = New TimeSpan(21, 0, 0)
    ''' <summary>Čas kdy začíná období označené jako večer</summary>
    ''' <remarks>Pokud nechcete používat nastavte na 24:00:00 nebo vyšší</remarks>
    ''' <exception cref="ArgumentException">Nastavovaná hodnota je menší než <see cref="Morning"/></exception>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Behavior)> _
    <DefaultValue(GetType(TimeSpan), "21:00:00")> _
    <Description("Time when so-called evening period starts.")> _
    Public Property Evening() As TimeSpan
        <DebuggerStepThrough()> Get
            Return _Evening
        End Get
        Set(ByVal value As TimeSpan)
            If value < Morning Then Throw New ArgumentException("Evening must be greater (or equal to) than morning.")
            Dim old As TimeSpan = Evening
            _Evening = value
            If Evening <> old Then OnEveningChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Vyvolává událost <see cref="EveningChanged"/></summary>
    ''' <param name="e">Parametry</param>
    Protected Overridable Sub OnEveningChanged(ByVal e As EventArgs)
        For Each rp As Panel In Me.RowPanes : rp.Invalidate() : Next rp
        tlpHead.Invalidate()
        RaiseEvent EveningChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně hodnoty vlastnosti <see cref="Evening"/></summary>
    <Description("Fired after value of the Evening property is changed.")> _
    <Category("Property Changed")> Public Event EveningChanged As EventHandler

#End Region
#Region "BlockEvMorn"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="BlockEvMorn"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _BlockEvMorn As Boolean = True
    ''' <summary>Určuje jestli uživatel může nasouvat položky do období <see cref="Evening"/> a <see cref="Morning"/></summary>
    <DefaultValue(True), KnownCategory(KnownCategoryAttribute.KnownCategories.Behavior)> _
    <Description("Determines if it is disabled to move items into morning and evening periods or not. True to disable; False to enable.")> _
    Public Property BlockEvMorn() As Boolean
        <DebuggerStepThrough()> Get
            Return _BlockEvMorn
        End Get
        Set(ByVal value As Boolean)
            Dim old As Boolean = BlockEvMorn
            _BlockEvMorn = value
            If old <> value Then OnBlockEvMornChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Vyvolává událost<see cref="BlockEvMornChanged"/></summary>
    ''' <param name="e">Parametry události</param>
    <DebuggerStepThrough()> Protected Overridable Sub OnBlockEvMornChanged(ByVal e As EventArgs)
        RaiseEvent BlockEvMornChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně hodnoty vlastnosti <see cref="BlockEvMorn"/></summary>
    <Description("Fired after value of the BlockEvMorn property is changed.")> _
    <Category("Property Changed")> Public Event BlockEvMornChanged As EventHandler
#End Region
#End Region
#Region "Pomocné metody"
    ''' <summary>Nastaví texty záhlaví při změně pozice časového okna</summary>
    Private Sub SetLabelTexts()
        If Headers IsNot Nothing Then
            Dim CurrDate As Date = Start
            For i As Integer = 0 To Headers.Length - 1
                If Headers(i) IsNot Nothing Then Headers(i).Text = CurrDate.ToString("H:mm")
                CurrDate = CurrDate.AddMinutes(SlotWidth)
            Next i
        End If
    End Sub
    ''' <summary>Změnit počet sloupců</summary>
    Private Sub ChangeColumnNo()
        'Odstranění labelů rezervací
        tlpMain.SuspendLayout()
        Try
            For Each row As List(Of ItemToolStrip) In Me.RowItems
                For Each item As ItemToolStrip In row
                    item.Parent.Controls.Remove(item)
                    RemoveItemHandlers(item)
                Next item
                row.Clear()
            Next row
            tlpMain.ColumnCount = Headers.Length + 1
            tlpHead.ColumnCount = Headers.Length + 1
            'Hlavičky
            For i As Integer = 1 To tlpMain.ColumnCount - 1
                For Each tlp As TableLayoutPanel In New TableLayoutPanel() {tlpHead, tlpMain}
                    With tlp
                        If i >= .ColumnStyles.Count Then
                            .ColumnStyles.Add(New ColumnStyle(SizeType.Percent, 100)) '1 / Headers.Length * 100))
                        Else
                            With .ColumnStyles(i)
                                .SizeType = SizeType.Percent
                                .Width = 100 ' 1 / Headers.Length * 100
                            End With
                        End If
                    End With
                Next tlp
            Next i
            InitHeaders()
            'Zobrazení labelů rezervací
            Dim RowI As Integer = 0
            For Each row As DayViewRow In Me.Rows
                For Each rec As DayViewItem In row.Records
                    AddNewItemToolStripIfItIsInWindow(rec, RowI)
                Next rec
                RowI += 1
            Next row
        Finally
            tlpMain.ResumeLayout()
        End Try
    End Sub
    ''' <summary>Odstraní labely záhlaví z kontejneru</summary>
    Private Sub RemoveLabels()
        If Headers IsNot Nothing Then
            For Each Header As Label In Headers
                If Header IsNot Nothing Then _
                    Header.Parent.Controls.Remove(Header)
            Next Header
        End If
    End Sub
    ''' <summary>Inicializuje záhlaví sloupců</summary>
    Private Sub InitHeaders()
        Dim TimeStart As Date = Start
        Dim SlotWidth As UShort = Me.SlotWidth
        For i As Integer = 0 To Headers.Length - 1
            Headers(i) = New Label
            With Headers(i)
                .Anchor = AnchorStyles.Top Or AnchorStyles.Bottom Or AnchorStyles.Left Or AnchorStyles.Right
                .TextAlign = ContentAlignment.MiddleLeft
                .Text = TimeStart.ToString("H:mm")
                .Margin = New Padding(0)
                .AutoSize = True
                .BackColor = Color.Transparent
            End With
            tlpHead.Controls.Add(Headers(i), i + 1, 0)
            TimeStart = TimeStart.AddMinutes(SlotWidth)
        Next i
    End Sub
#End Region

#Region "Colors"
#Region "HorizontalLineColor"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="HorizontalLineColor"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _HorizontalLineColor As Color = SystemColors.ControlText
    ''' <summary>Barva vodorovné čáry oddělující jednotlivé řádky</summary>
    <DefaultValue(GetType(Color), "ControlText")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Color of line between rows")> _
    Public Property HorizontalLineColor() As Color
        <DebuggerStepThrough()> Get
            Return _HorizontalLineColor
        End Get
        Set(ByVal value As Color)
            Dim old As Color = HorizontalLineColor
            _HorizontalLineColor = value
            If value <> old Then OnHorizontalLineColorChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Nastává po změně hodnoty vlastnosti <see cref="HorizontalLineColor"/></summary>
    <Description("Fired after value of the HorizontalLineColor is changed.")> _
    <Category("Property Changed")> Public Event HorizontalLineColorChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="HorizontalLineColorChanged"/></summary>
    Protected Overridable Sub OnHorizontalLineColorChanged(ByVal e As EventArgs)
        For Each rp As Panel In RowPanes
            rp.Invalidate(New Rectangle(0, rp.ClientRectangle.Height - 1, rp.ClientRectangle.Width, 1))
        Next rp
        tlpHead.Invalidate(New Rectangle(0, tlpHead.ClientRectangle.Height - 1, tlpHead.ClientRectangle.Width, 1))
        RaiseEvent HorizontalLineColorChanged(Me, e)
    End Sub
#End Region
#Region "MorEvColor"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="MorEvColor"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _MorEvColor As Color = Color.FromArgb(10, 0, 0, 0)
    ''' <summary>Barva pozadí oblastí <see cref="Morning"/> a <see cref="Evening"/></summary>
    <DefaultValue(GetType(Color), "10,0,0,0")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Defines background color of mornign and evening area. Feel free to use alpha-channel-enabled colors")> _
    Public Property MorEvColor() As Color
        <DebuggerStepThrough()> Get
            Return _MorEvColor
        End Get
        Set(ByVal value As Color)
            Dim old As Color = MorEvColor
            _MorEvColor = value
            If value <> old Then OnMorEvColorChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Nastává po změně hodnoty vlastnosti <see cref="MorEvColor"/></summary>
    <Description("Raised after value of the MorEvColor property is changed.")> _
    <Category("Property Changed")> Public Event MorEvColorChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="MorEvColorChanged"/></summary>
    Protected Overridable Sub OnMorEvColorChanged(ByVal e As EventArgs)
        Dim MorX As Integer = LeftFromStart(Me.Start.Date + Morning)
        Dim EvX As Integer = RightFromEnd(Me.Start.Date + Evening)
        For Each rp As Panel In Me.RowPanes
            If MorX > 0 Then rp.Invalidate(New Rectangle(0, 0, MorX, rp.ClientRectangle.Height))
            If EvX < rp.ClientRectangle.Width Then rp.Invalidate(New Rectangle(EvX, 0, rp.ClientRectangle.Width - EvX, rp.ClientRectangle.Height))
        Next rp
        Dim Col1W As Integer = tlpHead.GetColumnWidths()(0)
        If MorX > 0 Then tlpMain.Invalidate(New Rectangle(Col1W, 0, MorX, tlpMain.ClientRectangle.Height))
        If EvX + Col1W < tlpHead.ClientRectangle.Width Then tlpHead.Invalidate(New Rectangle(Col1W + EvX, 0, tlpHead.ClientRectangle.Width - EvX - Col1W, tlpHead.ClientRectangle.Height))
        RaiseEvent MorEvColorChanged(Me, e)
    End Sub
#End Region
#Region "SelectedItemBorderColor"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="SelectedItemBorderColor"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _SelectedItemBorderColor As Color = Color.FromArgb(128, 255, 255, 0)
    ''' <summary><see cref="ItemToolStrip.BorderColor"/> položek</summary>
    <DefaultValue(GetType(Color), "128,255,255,0")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Border color of selected item")> _
    Public Property SelectedItemBorderColor() As Color
        <DebuggerStepThrough()> Get
            Return _SelectedItemBorderColor
        End Get
        Set(ByVal value As Color)
            Dim old As Color = SelectedItemBorderColor
            _SelectedItemBorderColor = value
            If value <> old Then OnSelectedItemBorderColorChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Nastává po změně hodnoty vlastnosti <see cref="SelectedItemBorderColor"/></summary>
    <Description("Raised after value of the SelectedItemBorderColor is changed.")> _
    <Category("Property Changed")> Public Event SelectedItemBorderColorChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="SelectedItemBorderColorChanged"/></summary>
    Protected Overridable Sub OnSelectedItemBorderColorChanged(ByVal e As EventArgs)
        For Each items As List(Of ItemToolStrip) In Me.RowItems
            For Each item As ItemToolStrip In items
                item.BorderColor = Me.SelectedItemBorderColor
            Next item
        Next items
        RaiseEvent SelectedItemBorderColorChanged(Me, e)
    End Sub
#End Region
#Region "NewItemBackColor"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="NewItemBackColor"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _NewItemBackColor As Color = SystemColors.Control
    ''' <summary><see cref="ItemToolStrip.BackColor"/> nových položek</summary>
    <DefaultValue(GetType(Color), "Control")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Background color of newly added items")> _
    Public Property NewItemBackColor() As Color
        <DebuggerStepThrough()> Get
            Return _NewItemBackColor
        End Get
        Set(ByVal value As Color)
            Dim old As Color = NewItemBackColor
            _NewItemBackColor = value
            If value <> old Then OnNewItemBackColorChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Nastává po změně hodnoty vlastnosti <see cref="NewItemBackColor"/></summary>
    <Description("Raised after value of the NewItemBackColor property is changed.")> _
    <Category("Property Changed")> Public Event NewItemBackColorChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="NewItemBackColorChanged"/></summary>
    Protected Overridable Sub OnNewItemBackColorChanged(ByVal e As EventArgs)
        RaiseEvent NewItemBackColorChanged(Me, e)
    End Sub
#End Region
#Region "NewItemForeColor"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="NewItemForeColor"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _NewItemForeColor As Color = SystemColors.ControlText
    ''' <summary><see cref="ItemToolStrip.ForeColor"/> nových položek</summary>
    <DefaultValue(GetType(Color), "ControlText")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Fore color of newly added items")> _
    Public Property NewItemForeColor() As Color
        <DebuggerStepThrough()> Get
            Return _NewItemForeColor
        End Get
        Set(ByVal value As Color)
            Dim old As Color = NewItemForeColor
            _NewItemForeColor = value
            If value <> old Then OnNewItemForeColorChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Nastává po změně hodnoty vlastnosti <see cref="NewItemForeColor"/></summary>
    <Description("Raised after value of the NewItemForeColor property is changed.")> _
    <Category("Property Changed")> Public Event NewItemForeColorChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="NewItemForeColorChanged"/></summary>
    Protected Overridable Sub OnNewItemForeColorChanged(ByVal e As EventArgs)
        RaiseEvent NewItemForeColorChanged(Me, e)
    End Sub
#End Region
#End Region
#Region "Other new item"
#Region "NewItemFont"
    ''' <summary>Předchozí hodnota vlastnosti <see cref="Font"/> pro kontrolu změny v <see cref="OnFontChanged"/></summary>
    Private OldFont As Font
    ''' <summary>Raises the <see cref="System.Windows.Forms.Control.FontChanged"/> event.</summary>
    ''' <param name="e">An <see cref="System.EventArgs"/> that contains the event data.</param>
    Protected Overrides Sub OnFontChanged(ByVal e As System.EventArgs)
        If OldFont Is Nothing OrElse OldFont Is NewItemFont Then NewItemFont = Font
        OldFont = Font
        MyBase.OnFontChanged(e)
    End Sub
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="NewItemFont"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _NewItemFont As Font
    ''' <summary><see cref="ItemToolStrip.ForeColor"/> nových položek</summary>
    <DefaultValue(GetType(Color), "ControlText")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Fore color of newly added items")> _
    Public Property NewItemFont() As Font
        <DebuggerStepThrough()> Get
            Return _NewItemFont
        End Get
        Set(ByVal value As Font)
            Dim old As Font = NewItemFont
            _NewItemFont = value
            If value IsNot old Then OnNewItemFontChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Indikuje jestli vlastnost <see cref="NewItemFont"/> má být serializována</summary>
    Protected Overridable Function ShouldSerializeNewItemFont() As Boolean
        Return NewItemFont IsNot Me.Font
    End Function
    ''' <summary>sesetuje hodnotu vlastnosti <see cref="NewItemFont"/></summary>
    Protected Overridable Sub ResetNewItemFont()
        NewItemFont = Font
    End Sub
    ''' <summary>Nastává po změně hodnoty vlastnosti <see cref="NewItemFont"/></summary>
    <Description("Raised after value of the NewItemFont property is changed.")> _
    <Category("Property Changed")> Public Event NewItemFontChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="NewItemFontChanged"/></summary>
    Protected Overridable Sub OnNewItemFontChanged(ByVal e As EventArgs)
        RaiseEvent NewItemFontChanged(Me, e)
    End Sub
#End Region
#Region "NewItemText"
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="NewItemText"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _NewItemText As String = ""
    ''' <summary><see cref="ItemToolStrip.ForeColor"/> nových položek</summary>
    <DefaultValue("")> _
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Appearance)> _
    <Description("Fore color of newly added items")> _
    Public Property NewItemText() As String
        <DebuggerStepThrough()> Get
            Return _NewItemText
        End Get
        Set(ByVal value As String)
            Dim old As String = NewItemText
            _NewItemText = value
            If value <> old Then OnNewItemTextChanged(EventArgs.Empty)
        End Set
    End Property
    ''' <summary>Nastává po změně hodnoty vlastnosti <see cref="NewItemText"/></summary>
    <Description("Raised after value of thne NewItemText property is changed.")> _
    <Category("Property Changed")> Public Event NewItemTextChanged As EventHandler
    ''' <summary>Vyvolává událost <see cref="NewItemTextChanged"/></summary>
    Protected Overridable Sub OnNewItemTextChanged(ByVal e As EventArgs)
        RaiseEvent NewItemTextChanged(Me, e)
    End Sub
#End Region
#End Region
#End Region

#Region "Řádky"
    ''' <summary>Obsahuje hoddnotu vlastnosti <see cref="Rows"/></summary>
    Private WithEvents _Rows As New ListWithEvents(Of DayViewRow)(True)
    ''' <summary>Řádky zobrazené v prvku <see cref="DayView"/></summary>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)> _
    Public ReadOnly Property Rows() As ListWithEvents(Of DayViewRow)
        <DebuggerStepThrough()> Get
            Return _Rows
        End Get
    End Property
    ''' <summary>Řádek s daným ID</summary>
    ''' <param name="ID">Vlastnosti <see cref="DayViewRow.RowID"/></param>
    ''' <returns>Řádek s daným ID nebo NULL</returns>
    Public Function GetRowById(ByVal ID As Integer) As DayViewRow
        For Each Row As DayViewRow In Rows
            If Row.RowID = ID Then Return Row
        Next Row
        Return Nothing
    End Function
#Region "Přidávání, odebírání, změny"
#Region "Added"
    ''' <summary>Přidání řádku</summary>
    Private Sub Rows_Added(ByVal sender As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow), ByVal e As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow).ItemIndexEventArgs) Handles _Rows.Added
        tlpMain.SuspendLayout()
        Try
            Dim lbl As Label
            Dim pan As Panel
            With e.Item
                lbl = NewRowHeader(.RowText, .TextColor, .BackColor, .Enabled)
                pan = NewRowPanel(.BackColor, .Enabled)
            End With
            RowHeaders.Insert(e.Index, lbl)
            RowPanes.Insert(e.Index, pan)
            RowItems.Insert(e.Index, New List(Of ItemToolStrip))
            tlpMain.RowCount += 1
            While tlpMain.RowStyles.Count < tlpMain.RowCount
                tlpMain.RowStyles.Add(New RowStyle(SizeType.AutoSize))
            End While
            For i As Integer = 0 To tlpMain.RowCount - 1
                tlpMain.RowStyles(i).SizeType = SizeType.AutoSize
            Next i
            For i As Integer = RowHeaders.Count - 1 To e.Index + 1 Step -1
                tlpMain.SetRow(RowHeaders(i), i)
                tlpMain.SetRow(RowPanes(i), i)
            Next i
            tlpMain.Controls.Add(lbl, 0, e.Index)
            tlpMain.Controls.Add(pan, 1, e.Index)
            tlpMain.SetColumnSpan(pan, tlpMain.ColumnCount - 1)
        Finally
            tlpMain.ResumeLayout()
        End Try
        ApplyFirstColumn()
        RowTabIndex()
        OnRowAdded(e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="RowAdded"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnRowAdded(ByVal e As ListWithEvents(Of DayViewRow).ItemIndexEventArgs)
        RaiseEvent RowAdded(Me, e)
    End Sub
    ''' <summary>Nastane po přidání položky do kolekce <see cref="Rows"/></summary>
    <Category("Row Operations")> _
    <Description("Raised after item is added into the Rows collection.")> _
    Public Event RowAdded As EventHandler(Of ListWithEvents(Of DayViewRow).ItemIndexEventArgs)
#End Region
#Region "Cleared"
    ''' <summary>Odstranění všech řádků</summary>
    Private Sub Rows_Cleared(ByVal sender As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow), ByVal e As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow).ItemsEventArgs) Handles _Rows.Cleared
        tlpMain.SuspendLayout()
        Try
            Dim i As Integer = 0
            For Each item As Label In RowHeaders
                DestroyRow(i)
                item.Parent.Controls.Remove(item)
                i += 1
            Next item
            For Each item As Panel In RowPanes
                item.Parent.Controls.Remove(item)
                For Each ctl As ItemToolStrip In item.Controls
                    RemoveItemHandlers(ctl)
                Next ctl
            Next item
            tlpMain.RowCount = 0
            RowHeaders.Clear()
            RowPanes.Clear()
            RowItems.Clear()
            ApplyFirstColumn()
        Finally
            tlpMain.ResumeLayout()
        End Try
        OnRowsCleared(e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="RowsCleared"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnRowsCleared(ByVal e As ListWithEvents(Of DayViewRow).ItemsEventArgs)
        RaiseEvent RowsCleared(Me, e)
    End Sub
    ''' <summary>Nastává při smazání všech položek kolekce <see cref="Rows"/> pomocí metody <see cref="ListWithEvents(Of DayViewRow).Clear"/></summary>
    ''' <remarks>Nenastane pokud je poslední položka vymazána metodou <see cref="ListWithEvents(Of DayViewRow).Remove"/> nebo <see cref="ListWithEvents(Of DayViewRow).RemoveAt"/></remarks>
    <Category("Row Operations")> _
    <Description("Raised after the Rows collection is cleared.")> _
    Public Event RowsCleared As EventHandler(Of ListWithEvents(Of DayViewRow).ItemsEventArgs)
#End Region
#Region "ItemChanged"
    ''' <summary>Změna řádku</summary>
    Private Sub Rows_ItemChanged(ByVal sender As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow), ByVal e As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow).OldNewItemEventArgs) Handles _Rows.ItemChanged
        tlpMain.SuspendLayout()
        Try
            RowHeaders(e.Index).Text = e.Item.RowText
            ApplyFirstColumn()
            'Odstranění položek
            For Each OldItem As ItemToolStrip In RowItems(e.Index)
                RemoveItemHandlers(OldItem)
            Next OldItem
            RowPanes(e.Index).Controls.Clear()
            RowItems(e.Index).Clear()
            'Přidání nových položek
            For Each NewItem As DayViewItem In e.Item.Records
                AddNewItemToolStripIfItIsInWindow(NewItem, e.Index)
            Next NewItem
        Finally
            tlpMain.ResumeLayout()
        End Try
        RowTabIndex()
        OnRowChanged(e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="RowChanged"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnRowChanged(ByVal e As ListWithEvents(Of DayViewRow).OldNewItemEventArgs)
        RaiseEvent RowChanged(Me, e)
    End Sub
    ''' <summary>Nastane po záměně položky v kolekci <see cref="Rows"/></summary>
    <Category("Row Operations")> _
    <Description("Raised after item in the Rows collection is replaced.")> _
    Public Event RowChanged As EventHandler(Of ListWithEvents(Of DayViewRow).OldNewItemEventArgs)
#End Region
#Region "ItemValueChanged"
    ''' <summary>Změna vlastnosti řádku (včetně operací s položkami)</summary>
    Private Sub Rows_ItemValueChanged(ByVal sender As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow), ByVal e As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow).ItemValueChangedEventArgs) Handles _Rows.ItemValueChanged
        If TypeOf e.OriginalEventArgs Is Tools.IReportsChange.ValueChangedEventArgsBase Then
            tlpMain.SuspendLayout()
            Try
                With DirectCast(e.OriginalEventArgs, Tools.IReportsChange.ValueChangedEventArgsBase)
                    Select Case .ValueName
                        Case DayViewRow.PropName_RowText
                            RowHeaders(Rows.IndexOf(e.Item)).Text = DirectCast(e.OriginalEventArgs, Tools.IReportsChange.ValueChangedEventArgs(Of String)).NewValue
                            ApplyFirstColumn()
                        Case DayViewRow.PropName_BackColor
                            With DirectCast(e.OriginalEventArgs, Tools.IReportsChange.ValueChangedEventArgs(Of Color))
                                If .NewValue <> Color.Transparent Then
                                    RowHeaders(Rows.IndexOf(e.Item)).BackColor = .NewValue
                                    RowPanes(Rows.IndexOf(e.Item)).BackColor = .NewValue
                                Else
                                    RowHeaders(Rows.IndexOf(e.Item)).ResetBackColor()
                                    RowPanes(Rows.IndexOf(e.Item)).ResetBackColor()
                                End If
                            End With
                        Case DayViewRow.PropName_TextColor
                            With DirectCast(e.OriginalEventArgs, Tools.IReportsChange.ValueChangedEventArgs(Of Color))
                                If .NewValue <> Color.Transparent Then
                                    RowHeaders(Rows.IndexOf(e.Item)).ForeColor = .NewValue
                                Else
                                    RowHeaders(Rows.IndexOf(e.Item)).ResetForeColor()
                                End If
                            End With
                        Case DayViewRow.PropName_Enabled
                            With DirectCast(e.OriginalEventArgs, Tools.IReportsChange.ValueChangedEventArgs(Of Boolean))
                                RowHeaders(Rows.IndexOf(e.Item)).Enabled = .NewValue
                                RowPanes(Rows.IndexOf(e.Item)).Enabled = .NewValue
                            End With
                    End Select
                End With
            Finally
                tlpMain.ResumeLayout()
            End Try
            OnRowValueChanged(e.OriginalEventArgs)
        ElseIf TypeOf e.OriginalEventArgs Is DayViewRow.ItemOperationEventArgsBase Then
            With DirectCast(e.OriginalEventArgs, DayViewRow.ItemOperationEventArgsBase)
                Dim RowIndex As Integer = Rows.IndexOf(e.Item)
                Select Case .Operation
                    Case DayViewRow.ItemOperationEventArgsBase.Operations.Added
                        OnProgramaticallyAddItem(RowIndex, e.OriginalEventArgs)
                    Case DayViewRow.ItemOperationEventArgsBase.Operations.Cleared
                        OnProgramaticallyClearItems(RowIndex, e.OriginalEventArgs)
                    Case DayViewRow.ItemOperationEventArgsBase.Operations.ItemChanged
                        OnProgramaticallyChangeItem(RowIndex, e.OriginalEventArgs)
                    Case DayViewRow.ItemOperationEventArgsBase.Operations.ItemValueChanged
                        OnProgramaticallyChangeItemValue(RowIndex, e.OriginalEventArgs)
                    Case DayViewRow.ItemOperationEventArgsBase.Operations.Removed
                        OnProgramaticallyRemoveItem(RowIndex, e.OriginalEventArgs)
                End Select
            End With
        End If
    End Sub
    ''' <summary>Vyvolává událost <see cref="RowValueChanged"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnRowValueChanged(ByVal e As Tools.IReportsChange.ValueChangedEventArgsBase)
        RaiseEvent RowValueChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně hodnoty skalární vlastnosti položky kolekce <see cref="Rows"/></summary>
    <Category("Row Operations")> _
    <Description("Raised after a scalar property of item of the Rows collection is changed.")> _
    Public Event RowValueChanged As EventHandler(Of Tools.IReportsChange.ValueChangedEventArgsBase)
#End Region
#Region "Removed"
    ''' <summary>Odstranění řádku</summary>
    Private Sub Rows_Removed(ByVal sender As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow), ByVal e As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewRow).ItemIndexEventArgs) Handles _Rows.Removed
        tlpMain.SuspendLayout()
        Try
            DestroyRow(e.Index)
            RowHeaders(e.Index).Parent.Controls.Remove(RowHeaders(e.Index))
            RowHeaders.RemoveAt(e.Index)
            RowPanes(e.Index).Parent.Controls.Remove(RowPanes(e.Index))
            RowPanes.RemoveAt(e.Index)
            For Each item As ItemToolStrip In RowItems(e.Index)
                RemoveItemHandlers(item)
            Next item
            RowItems.RemoveAt(e.Index)
            For i As Integer = e.Index To RowHeaders.Count - 1
                tlpMain.SetRow(RowHeaders(i), i)
                tlpMain.SetRow(RowPanes(i), i)
            Next i
            tlpMain.RowCount = RowHeaders.Count
        Finally
            tlpMain.ResumeLayout()
        End Try
        ApplyFirstColumn()
        RowTabIndex()
        OnRowRemoved(e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="RowRemoved"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnRowRemoved(ByVal e As ListWithEvents(Of DayViewRow).ItemIndexEventArgs)
        RaiseEvent RowRemoved(Me, e)
    End Sub
    ''' <summary>Nastane po odstranění jedné položky z kolekce <see cref="Rows"/></summary>
    ''' <remarks>Nenastane pokud jsou z kolekce <see cref="Rows"/> odstraněny všechny položky najednou pomocí metody <see cref="ListWithEvents(Of DayViewRow).Clear"/>.</remarks>
    <Category("Row Operations")> _
    <Description("Raised after single item is removed from the Rows collection.")> _
    Public Event RowRemoved As EventHandler(Of ListWithEvents(Of DayViewRow).ItemIndexEventArgs)
#End Region
#Region "Podpůrné funkce řádků"
    ''' <summary>Doplňkové akce spojené s odstraněním řádky</summary>
    ''' <param name="index">Řádek nad nímž provést doplňkové akce</param>
    ''' <remarks>Odstraňuje handlery událostí</remarks>
    Private Sub DestroyRow(ByVal index As Integer)
        RemoveHandler RowPanes(index).Paint, AddressOf RowPane_Paint
        RemoveHandler RowHeaders(index).Paint, AddressOf RowHeader_Paint
        RemoveHandler RowPanes(index).MouseDown, AddressOf RowPane_MouseDown
        'RemoveHandler RowPanes(index).MouseMove, AddressOf RowPane_MouseMove
        'RemoveHandler RowPanes(index).MouseUp, AddressOf RowPane_Mouseup
    End Sub
    ''' <summary>Doplní <see cref="Panel.TabIndex"/> k položkám kolekcí <see cref="RowHeaders"/> a <see cref="RowPanes"/></summary>
    Private Sub RowTabIndex()
        For i As Integer = 0 To Rows.Count - 1
            RowHeaders(i).TabIndex = i * 2
            RowPanes(i).TabIndex = i * 2 + 1
        Next i
    End Sub
    ''' <summary>Inicializuje <see cref="Label"/> představující záhlaví řádku</summary>
    ''' <param name="text">Text záhlaví</param>
    ''' <param name="BackColor">Barva pozadí záhlaví (pokud je hodnota <see cref="Color.Transparent"/> bude ignorována)</param>
    ''' <param name="ForeColor">Barva textu záhlaví (pokud je hodnota <see cref="Color.Transparent"/> bude ignorována)</param>
    ''' <param name="Enabled">Hodnota pro <see cref="Label.Enabled"/></param>
    Private Function NewRowHeader(ByVal text As String, ByVal ForeColor As Color, ByVal BackColor As Color, ByVal Enabled As Boolean) As Label
        Dim Lbl As New Label
        With Lbl
            .TextAlign = ContentAlignment.MiddleRight
            .Text = text
            .AutoSize = True
            .Anchor = AnchorStyles.Top Or AnchorStyles.Left Or AnchorStyles.Bottom Or AnchorStyles.Right
            .Font = New Font(.Font, FontStyle.Bold)
            .Margin = New Padding(0)
            If ForeColor <> Color.Transparent Then .ForeColor = ForeColor
            If BackColor <> Color.Transparent Then .BackColor = BackColor
            .Enabled = Enabled
            AddHandler .Paint, AddressOf RowHeader_Paint
        End With
        Return Lbl
    End Function
    ''' <summary>Inicializuje nový <see cref="ToolStripPanel"/> reprezentující obsah řádku</summary>
    ''' <param name="BackColor">Barva pozadí záhlaví (pokud je hodnota <see cref="Color.Transparent"/> bude ignorována)</param>
    ''' <param name="Enabled">Hodnota pro <see cref="Panel.Enabled"/></param>
    Private Function NewRowPanel(ByVal BackColor As Color, ByVal Enabled As Boolean) As Panel
        Dim ret As New Panel
        With ret
            .Dock = DockStyle.Fill
            .BackColor = Me.tlpMain.BackColor
            .Margin = New Padding(0)
            .Padding = New Padding(0, 0, 0, 1)
            AddHandler .Paint, AddressOf RowPane_Paint
            .AutoSize = True
            If BackColor <> Color.Transparent Then .BackColor = BackColor
            .Enabled = Enabled
            AddHandler .MouseDown, AddressOf RowPane_MouseDown
            'AddHandler .MouseDown, AddressOf RowPane_MouseMove
            'AddHandler .MouseUp, AddressOf rowpane_mouseup
        End With
        Return ret
    End Function
    ''' <summary>Nastaví šířku 1. sloupce <see cref="tlpHead"/> podle <see cref="tlpMain"/></summary>
    Private Sub ApplyFirstColumn()
        tlpHead.ColumnStyles(0) = New ColumnStyle(SizeType.Absolute, tlpMain.GetColumnWidths(0))
    End Sub
#End Region
#Region "Row Events"
    ''' <summary>Reaguje na událost <see cref="Panel.MouseDown"/> položky kolekce <see cref="RowPanes"/></summary>
    Private Sub RowPane_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs)
        If e.Button = Windows.Forms.MouseButtons.Left Then
            Dim x As Date = X2Time(e.X)
            If Not BlockEvMorn OrElse (x.TimeOfDay >= Morning AndAlso x.TimeOfDay <= Evening) Then
                With DirectCast(sender, Panel)
                    If e.X < .ClientRectangle.Width AndAlso x + TimeSpan.FromMinutes(MinimumItemWidth) <= Start + TimeWindow Then
                        Dim NewItem As ItemToolStrip = New ItemToolStrip()
                        Dim NewRow As Integer = RowPanes.IndexOf(sender)
                        NewItem.Tag = New DayViewItem(x, Me.MinimumItemWidth, "")
                        NewItem.BackColor = NewItemBackColor
                        NewItem.ForeColor = NewItemForeColor
                        NewItem.LabelFont = NewItemFont
                        NewItem.Text = NewItemText
                        Dim ea As New CancelMoveRowEventArgs(e.X, 0, RightFromEnd(x + TimeSpan.FromMinutes(MinimumItemWidth)) - e.X, NewItem.Height, NewRow)
                        NewItem.Location = ea.Location
                        NewItem.Size = ea.Size
                        NewItem.SetMoveState(ItemToolStrip.MoveStates.Moving, NewItem.Location, e.Location - NewItem.Location, NewItem.Width)
                        Item_BeforeMove(NewItem, ea)
                        If Not ea.Cancel Then
                            NewItem.AutoSize = False
                            NewItem.Left = ea.x
                            NewItem.Top = 0
                            NewItem.Width = ea.Width
                            .Controls.Add(NewItem)
                            NewItem.Select()
                            Cursor.Position = NewItem.PointToScreen(New Point(NewItem.ClientSize.Width, NewItem.ClientSize.Height / 2))
                            NewItem.Capture = True
                            NewItem.SetMoveState(ItemToolStrip.MoveStates.RightResizing, NewItem.Location, New Point(NewItem.ClientSize.Width + NewItem.Left, NewItem.ClientSize.Height / 2 + NewItem.Top), NewItem.Width)
                            AddItemHandlers(NewItem)
                        Else
                            NewItem = Nothing
                            Exit Sub
                        End If
                        .Invalidate(New Rectangle(NewItem.Location, NewItem.Size))
                    End If
                End With
            End If
        End If
    End Sub
    '''' <summary>Reaguje na událost <see cref="Panel.MouseMove"/> položky kolekce <see cref="RowPanes"/></summary>
    'Private Sub RowPane_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
    '    If e.Button = Windows.Forms.MouseButtons.Left AndAlso NewItem IsNot Nothing Then
    '        Dim ea As New CancelMoveEventArgs(NewItem.Left, NewItem.Top, e.X - NewItem.Left, e.Y - NewItem.Top)
    '        NewItem.MovingState = ItemToolStrip.MoveStates.RightResizing 'TODO: Může bejt i left
    '        Item_BeforeMove(NewItem, ea, BeforeMoveSpecialRequests.ResizeNew)
    '        If Not ea.Cancel Then
    '            NewItem.Left = ea.x
    '            NewItem.Top = 0
    '            NewItem.Width = ea.Width
    '            NewItem.Height = RowPanes(NewRow).ClientRectangle.Height
    '            RowPanes(NewRow).Invalidate()
    '        End If
    '    End If
    'End Sub
    '''' <summary>Reaguje na událost <see cref="Panel.MouseUp"/> položky kolekce <see cref="RowPanes"/></summary>
    'Private Sub RowPane_MouseUp(ByVal sender As Object, ByVal e As MouseEventArgs)
    '    If e.Button = Windows.Forms.MouseButtons.Left AndAlso NewItem IsNot Nothing Then
    '        NewItem = Nothing
    '        Me.Capture = False
    '        RowPanes(NewRow).Invalidate()
    '    End If
    'End Sub
    ''' <summary>Reaguje na událost <see cref="Panel.Paint"/> položky z kolekce <see cref="RowPanes"/></summary>
    ''' <remarks>Kreslí ohraničení řádků a pozadí <see cref="Morning"/> a <see cref="Evening"/></remarks>
    <DebuggerStepThrough()> _
    Private Sub RowPane_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
        With DirectCast(sender, Panel)
            PaintEvMorBackground(sender, e)
            'Vodorovná čára
            If e.ClipRectangle.IntersectsWith(New Rectangle(0, .ClientSize.Height - 1, .ClientSize.Width, 1)) Then
                e.Graphics.DrawLine(New Pen(Me.HorizontalLineColor), e.ClipRectangle.Left, .ClientSize.Height - 1, e.ClipRectangle.Right, .ClientSize.Height - 1)
            End If
            ''Nová položka
            'If NewItem IsNot Nothing AndAlso sender Is RowPanes(NewRow) Then
            '    Dim NewRect As New Rectangle(NewItem.Left, 0, NewItem.Width, RowPanes(NewRow).ClientRectangle.Height - 1)
            '    If e.ClipRectangle.IntersectsWith(NewRect) Then
            '        'TODO: Color
            '        e.Graphics.FillRectangle(Brushes.AliceBlue, Rectangle.Intersect(e.ClipRectangle, NewRect))
            '    End If
            'End If
        End With
    End Sub
    ''' <summary>Vykreslí pozadí oblasí <see cref="Morning"/> a <see cref="Evening"/></summary>
    ''' <param name="sender">Ovládací prvek</param>
    ''' <param name="e">Argumenty události</param>
    ''' <param name="OffsetX">Posin oblasti v pixelech oproti pozici <see cref="LeftFromStart"/> resp. <see cref="RightFromEnd"/></param>
    Private Sub PaintEvMorBackground(ByVal sender As Control, ByVal e As PaintEventArgs, Optional ByVal OffsetX As Integer = 0)
        With sender
            'Morning
            If Morning > TimeSpan.Zero Then
                Dim morningX As Integer = LeftFromStart(Me.Start.Date + Morning)
                If morningX > 0 Then
                    Dim MorRect As New Rectangle(OffsetX, 0, morningX, .ClientRectangle.Height)
                    If e.ClipRectangle.IntersectsWith(MorRect) Then
                        e.Graphics.FillRectangle(New SolidBrush(Me.MorEvColor), Rectangle.Intersect(MorRect, e.ClipRectangle))
                    End If
                End If
            End If
            'Evening
            If Evening < New TimeSpan(24, 0, 0) Then
                Dim eveningX As Integer = RightFromEnd(Me.Start.Date + Evening)
                If eveningX < .ClientRectangle.Width Then
                    Dim EvRect As New Rectangle(eveningX + OffsetX, 0, .ClientRectangle.Width - eveningX - OffsetX, .ClientRectangle.Height)
                    If e.ClipRectangle.IntersectsWith(EvRect) Then
                        e.Graphics.FillRectangle(New SolidBrush(Me.MorEvColor), Rectangle.Intersect(EvRect, e.ClipRectangle))
                    End If
                End If
            End If
        End With
    End Sub

    Private Sub tlpHead_Paint(ByVal sender As Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles tlpHead.Paint
        PaintEvMorBackground(sender, e, tlpHead.GetColumnWidths()(0))
        If e.ClipRectangle.IntersectsWith(New Rectangle(0, tlpHead.ClientRectangle.Height - 1, tlpHead.ClientRectangle.Width, 1)) Then
            e.Graphics.DrawLine(New Pen(HorizontalLineColor), e.ClipRectangle.Left, tlpHead.Height - 1, e.ClipRectangle.Right, tlpHead.Height - 1)
        End If
    End Sub
    ''' <summary>Reaguje na událost <see cref="Label.Paint"/> položky z kolekce <see cref="RowHeaders"/></summary>
    <DebuggerStepThrough()> Private Sub RowHeader_Paint(ByVal sender As Object, ByVal e As PaintEventArgs)
        With DirectCast(sender, Label)
            If e.ClipRectangle.IntersectsWith(New Rectangle(0, .ClientSize.Height - 1, .ClientSize.Width, 1)) Then
                e.Graphics.DrawLine(New Pen(Me.HorizontalLineColor), e.ClipRectangle.Left, .ClientSize.Height - 1, e.ClipRectangle.Right, .ClientSize.Height - 1)
            End If
        End With
    End Sub
#End Region
#End Region
#End Region

#Region "Scrollbary a změny velikosti"
#Region "Změny velikostí"
    ''' <summary>Raises the <see cref="System.Windows.Forms.Control.Resize"/> event.</summary>
    ''' <param name="e">An <see cref="System.EventArgs"/> that contains the event dat</param>
    Protected Overrides Sub OnResize(ByVal e As System.EventArgs)
        HeadResize()
        Me.SuspendLayout()
        Try
            tlpHead.MinimumSize = New Size(Me.ClientSize.Width - (tlpMain.Width - tlpMain.ClientSize.Width) - vsbVert.Width, 0) 'MaxCtrlHeight(New Tools.CollectionsT.GenericT.Wrapper(Of Control)(tlpHead.Controls)))
            tlpHead.MaximumSize = New Size(tlpHead.MinimumSize.Width, 0)
        Finally
            Me.ResumeLayout()
        End Try
        MyBase.OnResize(e)
    End Sub
    '''' <summary>Maximální výška <see cref="Control">Controlu</see></summary>
    '''' <param name="Ctrl">Položky z nich určit maximální výšku</param>
    '''' <returns>Maximum z <see cref="Control.Height"/> položek <paramref name="Ctrl"/></returns>
    'Private Shared Function MaxCtrlHeight(ByVal Ctrl As IEnumerable(Of Control)) As Integer
    '    Dim Max As Integer = Integer.MinValue
    '    For Each ctl As Control In Ctrl
    '        If Max < ctl.Height Then Max = ctl.Height
    '    Next ctl
    '    Return Max
    'End Function

    <DebuggerStepThrough()> Private Sub tlpHead_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles tlpHead.Resize
        HeadResize()
    End Sub
    ''' <summary>Reaguje na změnu velikosti celého ovládacího prvku a na změnu velikosti záhlaví</summary>
    Private Sub HeadResize()
        ApplyMinimumSize()
        panMain.Top = tlpHead.Top + tlpHead.Height
        Dim NewHeight As Integer = Me.ClientSize.Height - hsbHoriz.Height - panMain.Top
        If NewHeight > 0 Then panMain.Height = NewHeight Else panMain.ClientSize = New Size(panMain.ClientSize.Width, 1)
    End Sub
    ''' <summary>Nastaví <see cref="Control.MinimumSize"/> této instance na maximum z <see cref="MinimumMinimumSize"/> a <see cref="MinimumSize"/></summary>
    Private Sub ApplyMinimumSize()
        With MinimumMinimumSize
            MyBase.MinimumSize = New Size( _
                Math.Max(.Width, MinimumSize.Width), _
                Math.Max(.Height, MinimumSize.Height))
        End With
    End Sub
    Private Sub panMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles panMain.Resize
        If panMain.Width <> vsbVert.Left - panMain.Left Then
            panMain.Width = vsbVert.Left - panMain.Left
            Exit Sub
        End If
        tlpMain.MinimumSize = New Size(panMain.ClientSize.Width, 0)
        tlpMain.Width = panMain.ClientSize.Width
        tlpMain.MaximumSize = New Size(panMain.ClientSize.Width, 0)
        HeightChanged()
    End Sub
    ''' <summary>Obsluhuje změnu výšky <see cref="panMain"/> nebo <see cref="tlpMain"/></summary>
    Private Sub HeightChanged()
        If tlpMain.Height > panMain.ClientSize.Height Then
            With vsbVert
                .Enabled = True
                .Minimum = 0
                .Maximum = tlpMain.Height
                .LargeChange = Math.Max(1, panMain.ClientSize.Height)
                .SmallChange = .LargeChange / 5
                If -tlpMain.Top > .Maximum - .LargeChange Then tlpMain.Top = -(.Maximum - .LargeChange)
            End With
        Else
            vsbVert.Enabled = False
            tlpMain.Top = 0
        End If
    End Sub
    Private Sub tlpMain_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles tlpMain.Resize
        HeightChanged()
        'Změna velikosti a pozice položek
        For Each row As List(Of ItemToolStrip) In Me.RowItems
            For Each item As ItemToolStrip In row
                With DirectCast(item.Tag, DayViewItem)
                    If .Start.Date <> Start.Date Then Continue For 'This should ocur only when SetStart is on call stack
                    item.Left = LeftFromStart(.Start)
                    item.Width = RightFromEnd(.End) - item.Left
                End With
            Next item
        Next row
    End Sub
#End Region
#Region "Svislý"
    ''' <summary>Logika svislého scrollování</summary>
    Protected Overridable Sub OnVScroll(ByVal e As System.Windows.Forms.ScrollEventArgs)
        tlpMain.Top = -e.NewValue
        OnScroll(e)
    End Sub
    <DebuggerStepThrough()> Private Sub vsbVert_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles vsbVert.Scroll
        OnVScroll(e)
    End Sub
    ''' <summary>Vertikální scrollbar</summary>
    <Browsable(False)> Protected Overloads ReadOnly Property VerticalScroll() As VScrollBar
        <DebuggerStepThrough()> Get
            Return vsbVert
        End Get
    End Property
#End Region
    '''' <summary>Raises the <see cref="System.Windows.Forms.ScrollableControl.Scroll"/> event.</summary>
    '''' <param name="se">A <see cref="System.Windows.Forms.ScrollEventArgs"/> that contains the event data.</param>
    'Protected Overrides Sub OnScroll(ByVal se As System.Windows.Forms.ScrollEventArgs)
    '    Select Case se.ScrollOrientation
    '        Case ScrollOrientation.VerticalScroll
    '            OnVScroll(se)
    '            Debug.Print("V: Old {0}, New {1}, Value {2}", se.OldValue, se.NewValue, Me.VerticalScroll.Value) '///
    '        Case ScrollOrientation.HorizontalScroll
    '            OnHScroll(se)
    '            Debug.Print("H: Old {0}, New {1}, Value {2}", se.OldValue, se.NewValue, Me.HorizontalScroll.Value) '///
    '    End Select
    '    MyBase.OnScroll(se)
    'End Sub
#Region "Vodorovný"
    ''' <summary>Logika vodorovného scrollování</summary>
    Protected Overridable Sub OnHScroll(ByVal e As System.Windows.Forms.ScrollEventArgs)
        Start = Start.Date.AddMinutes(e.NewValue)
        OnScroll(e)
    End Sub
    Private Sub hsbHoriz_Scroll(ByVal sender As Object, ByVal e As System.Windows.Forms.ScrollEventArgs) Handles hsbHoriz.Scroll
        OnHScroll(e)
    End Sub
    ''' <summary>Horizontální scrollbar</summary>
    <Browsable(False)> _
    Protected Overloads ReadOnly Property HorizontalScroll() As HScrollBar
        <DebuggerStepThrough()> Get
            Return hsbHoriz
        End Get
    End Property
    ''' <summary>Podle <see cref="SlotWidth"/> a <see cref="TimeWindow"/> nastaví <see cref="hsbHoriz"/></summary>
    Private Sub SetHSBProperties()
        With hsbHoriz
            .Minimum = 0
            .Maximum = 24 * 60
            .LargeChange = TimeWindow.TotalMinutes
            .SmallChange = SlotWidth
        End With
    End Sub
#End Region
#End Region

#Region "Data v řádcích"
#Region "Podpůrné funkce"
#Region "Převody ItemToolStrip ↔ DayViewItem"
    ''' <summary>Najde prvek <see cref="ToolStripItem"/> který reprezentuje položku <see cref="DayViewItem"/></summary>
    ''' <param name="RowIndex">Index řádku na kterém hledat</param>
    ''' <param name="item">Položka kterou hledat</param>
    ''' <returns><see cref="ItemToolStrip"/> z řádku <paramref name="RowIndex"/> který obsahuje <see cref="ItemToolStrip.Tag">Tag</see> <paramref name="item"/> nebo null pokud taková položka neexistuje</returns>
    Private Function GetItemToolStrip(ByVal RowIndex As Integer, ByVal item As DayViewItem) As ItemToolStrip
        For Each iItem As ItemToolStrip In RowItems(RowIndex)
            If iItem.Tag Is item Then Return iItem
        Next iItem
        Return Nothing
    End Function
    ''' <summary>Zjistí v jakém řádku se nachází daná položka kolekce <see cref="Rows"/></summary>
    ''' <param name="item">Položka jejíž řádek identifikovat</param>
    ''' <returns>Index řádku v němž se nachází položka <paramref name="item"/> nebo -1 pokud položka nebyla nalezena</returns>
    Private Function RowFromItem(ByVal item As DayViewItem) As Integer
        Dim i As Integer = 0
        For Each row As DayViewRow In Me.Rows
            If row.Records.Contains(item) Then Return i
            i += 1
        Next row
        Return -1
    End Function
#End Region
#Region "Add"
    ''' <summary>Inicializuje nový <see cref="ItemToolStrip"/></summary>
    ''' <param name="Item">Položka, kterou má být <see cref="ItemToolStrip"/> inicializován</param>
    Private Function NewToolStrip(ByVal Item As DayViewItem) As ItemToolStrip
        Dim Left As Integer = LeftFromStart(Item.Start)
        Dim Width As Integer = RightFromEnd(Item.Start + TimeSpan.FromMinutes(Item.Length)) - Left
        Dim ret As New ItemToolStrip(Item.Text, Item.Font)
        With ret
            Try
                .BackColor = Item.BackColor
            Catch
                .BackColor = Me.BackColor
            End Try
            .ForeColor = Item.ForeColor
            .Label.Font = Item.Font
            .GripStyle = If(Item.Locked, ToolStripGripStyle.Hidden, ToolStripGripStyle.Visible)
            .CanMove = Not Item.Locked
            .Tag = Item
            .AutoSize = False
            .Left = Left
            .Width = Width
            .AcceptsLeftRight = True
            '.AcceptsTab = False 'Netreba - default
            '.TabStop = True 'Netreba - default
            .BorderColor = SelectedItemBorderColor
        End With
        Return ret
    End Function
    ''' <summary>Přidá předvytvořený <see cref="ItemToolStrip"/> do kolekcí <see cref="RowPanes">RowPanes</see>.<see cref="Panel.Controls">Controls</see> a <see cref="RowItems"/> a přidá k němu handley událostí</summary>
    ''' <param name="RowIndex">Řádek na který se má <paramref name="NewItem"/> přidat</param>
    ''' <param name="NewItem">Nový <see cref="ItemToolStrip"/></param>
    Private Sub AddItemToolStrip(ByVal RowIndex As Integer, ByVal NewItem As ItemToolStrip)
        RowPanes(RowIndex).Controls.Add(NewItem)
        RowItems(RowIndex).Add(NewItem)
        AddItemHandlers(NewItem)
    End Sub
    ''' <summary>Přidá předvytvořený <see cref="ItemToolStrip"/> do kolekcí <see cref="RowPanes">RowPanes</see>.<see cref="Panel.Controls">Controls</see> a <see cref="RowItems"/> a přidá k němu handley událostí pomocí <seealso cref="AddItemToolStrip"/> pokud tento <see cref="ItemToolStrip"/> je alespoň částečně viditelný v průzoru</summary>
    ''' <param name="RowIndex">Řádek na který se má <paramref name="NewItem"/> přidat</param>
    ''' <param name="NewItem">Nový <see cref="ItemToolStrip"/></param>
    ''' <returns>True pokud <paramref name="NewItem"/> byl přidán, jinak False</returns>
    Private Function AddItemToolStripIfItIsInWindow(ByVal NewItem As ItemToolStrip, ByVal RowIndex As Integer) As Boolean
        If Not (NewItem.Left < 0 AndAlso NewItem.Right <= 0 OrElse NewItem.Left >= RowPanes(RowIndex).Width AndAlso NewItem.Right > RowPanes(RowIndex).Width) Then 'Nepřidávat položky, které jsou mimo průzor
            AddItemToolStrip(RowIndex, NewItem)
            Return True
        End If
        Return False
    End Function
    ''' <summary>Vytvoří nový <see cref="ItemToolStrip"/> z <see cref="DayViewItem"/> a přidá jej do kolekcí <see cref="RowPanes">RowPanes</see>.<see cref="Panel.Controls">Controls</see> a <see cref="RowItems"/> a přidá k němu handlery událostí - toto vše jen v případě, že položka zasahuje do průzoru</summary>
    ''' <param name="CreateFrom"><see cref="DayViewItem"/> z něhož má být <see cref="ItemToolStrip"/> vytvořen</param>
    ''' <param name="RowIndex">Řádek na nějž má být nově vytvořený <see cref="ItemToolStrip"/> přidán</param>
    ''' <returns>Nově vytvořený a přidaný <see cref="ItemToolStrip"/> pokud byl vytvořen a přidán. Jinak null</returns>
    Private Function AddNewItemToolStripIfItIsInWindow(ByVal CreateFrom As DayViewItem, ByVal RowIndex As Integer) As ItemToolStrip
        Dim NewItem As ItemToolStrip = NewToolStrip(CreateFrom)
        If AddItemToolStripIfItIsInWindow(NewItem, RowIndex) Then _
            Return NewItem _
        Else _
            Return Nothing
    End Function
#End Region
#Region "Pozice"
    ''' <summary>Určuje pozici levé hrany položky podle data začátku</summary>
    ''' <param name="Start">Datum začátku</param>
    ''' <exception cref="ArgumentException"><paramref name="Start">Start</paramref>.<see cref="DateTime.[Date]">Date</see> je různé od <see cref="Start">Start</see>.<see cref="DateTime.[Date]">Date</see></exception>
    Private Function LeftFromStart(ByVal Start As Date) As Integer
        If Start.Date <> Me.Start.Date Then Throw New ArgumentException("Položka musí začínat ve stejný den jaký je zobrazen", "Start")
        If Start < Me.Start Then Return -10
        If Start > Me.Start + Me.TimeWindow Then Return tlpMain.ClientSize.Width + 10
        Dim StartAtSlot As Single = (Start - Me.Start).TotalMinutes / SlotWidth
        Dim SlotLeft As Integer = SlotNoLeft(Math.Truncate(StartAtSlot))
        If CInt(StartAtSlot) <> StartAtSlot Then
            Return SlotLeft + tlpMain.GetColumnWidths(Math.Truncate(StartAtSlot) + 1) * (StartAtSlot - Math.Truncate(StartAtSlot))
        Else
            Return SlotLeft
        End If
    End Function
    ''' <summary>Zjistí pozici levé hrany sloupce reprezentující časový slot s daným pořadovým číslem</summary>
    ''' <param name="SlotNo">Pořadové číslo slotu (0-based)</param>
    ''' <returns>Pozice levé hrany časového slotu číslo <paramref name="SlotNo"/></returns>
    Private Function SlotNoLeft(ByVal SlotNo As Integer) As Integer
        Dim arr As Integer() = tlpMain.GetColumnWidths
        Dim Left As Integer = 0
        For i As Integer = 1 To SlotNo
            Left += arr(i)
        Next i
        Return Left
    End Function
    ''' <summary>Určuje pozici pravé hrany položky podle data konce</summary>
    ''' <param name="End">Datum konce</param>
    ''' <exception cref="ArgumentException"><paramref name="End">Start</paramref>.<see cref="DateTime.[Date]">Date</see> je různé od <see cref="Start">Start</see>.<see cref="DateTime.[Date]">Date</see> a nejedná se ani o 0:00 následujícího dne</exception>
    Private Function RightFromEnd(ByVal [End] As Date) As Integer
        If [End].Date <> Me.Start.Date AndAlso Not ([End].Date.AddDays(-1) = Me.Start.Date AndAlso [End].TimeOfDay = TimeSpan.Zero) Then Throw New ArgumentException("Položky musí končit ve stejný den jako je zobrazen nebo v 0:00 dne následujícího", "End")
        If [End] > Start + TimeWindow Then Return tlpMain.ClientSize.Width + 10
        If [End] < Start Then Return -10
        Dim EndAtSlot As Single = ([End] - Me.Start).TotalMinutes / SlotWidth
        Dim SlotLeft As Integer = SlotNoLeft(Math.Truncate(EndAtSlot))
        If CInt(EndAtSlot) <> EndAtSlot Then
            Return SlotLeft + tlpMain.GetColumnWidths(Math.Truncate(EndAtSlot) + 1) * (EndAtSlot - Math.Truncate(EndAtSlot))
        Else
            Return SlotLeft
        End If
    End Function
    ''' <summary>Zjistí časovou hodnotu levé hrany položky</summary>
    ''' <param name="item">Položka</param>
    ''' <param name="Left">Namísto vlastnosti <paramref name="item"/>.<see cref="ItemToolStrip.Left">Left</see> se použuje tento parametr</param>
    ''' <returns>Pokud je levá hrana v zobrazené ploše vrátí časovou hodnotu odpovídající pozici levé hrany jinak vrátí uloženou pozici začátku přednostně pomocí <see cref="ItemToolStrip.Tag2"/> (pokud není null) jinak pomocí <see cref="ItemToolStrip.Tag"/>.</returns>
    Private Function GetStart(ByVal item As ItemToolStrip, ByVal Left As Integer) As Date
        If Left >= 0 AndAlso Left < item.Parent.ClientRectangle.Width Then
            Return X2Time(Left)
        ElseIf item.Tag2 IsNot Nothing Then
            Return DirectCast(item.Tag2, DateRangeEventArgs).Start
        Else
            Return DirectCast(item.Tag, DayViewItem).Start
        End If
    End Function
    ''' <summary>Zjistí časovou hodnotu levé hrany položky</summary>
    ''' <param name="item">Položka</param>
    ''' <returns>Pokud je levá hrana v zobrazené ploše vrátí časovou hodnotu odpovídající pozici levé hrany jinak vrátí uloženou pozici začátku přednostně pomocí <see cref="ItemToolStrip.Tag2"/> (pokud není null) jinak pomocí <see cref="ItemToolStrip.Tag"/>.</returns>
    <DebuggerStepThrough()> Private Function GetStart(ByVal item As ItemToolStrip) As Date
        Return GetStart(item, item.Left)
    End Function
    ''' <summary>Zjistí časovou hodnotu pravé hrany položky</summary>
    ''' <param name="item">Položka</param>
    ''' <param name="Right">Namísto vlastnosti <paramref name="item"/>.<see cref="ItemToolStrip.Right">Right</see> se použije tento parametr</param>
    ''' <returns>Pokud je pravá hrana v zobrazené ploše vrátí časovou hodnotu odpovídající pozici pravé hrany jinak vrátí uloženou pozici konce přednostně pomocí <see cref="ItemToolStrip.Tag2"/> (pokud není null) jinak pomocí <see cref="ItemToolStrip.Tag"/>.</returns>
    Private Function GetEnd(ByVal item As ItemToolStrip, ByVal Right As Integer) As Date
        If Right <= item.Parent.ClientRectangle.Width AndAlso Right > 0 Then
            Return X2Time(Right)
        ElseIf item.Tag2 IsNot Nothing Then
            Return DirectCast(item.Tag2, DateRangeEventArgs).End
        Else
            Return DirectCast(item.Tag, DayViewItem).End
        End If
    End Function
    ''' <summary>Zjistí časovou hodnotu pravé hrany položky</summary>
    ''' <param name="item">Položka</param>
    ''' <returns>Pokud je pravá hrana v zobrazené ploše vrátí časovou hodnotu odpovídající pozici pravé hrany jinak vrátí uloženou pozici konce přednostně pomocí <see cref="ItemToolStrip.Tag2"/> (pokud není null) jinak pomocí <see cref="ItemToolStrip.Tag"/>.</returns>
    <DebuggerStepThrough()> Private Function GetEnd(ByVal item As ItemToolStrip) As Date
        Return GetEnd(item, item.Right)
    End Function
    ''' <summary>Zaokrouhlí číslo na daný krok</summary>
    ''' <param name="Number">Číslo k zaokrouhlení</param>
    ''' <param name="Size">Krok zaokrouhlování</param>
    ''' <returns>Číslo <paramref name="Number"/> zakrouhledné na nejbližší celý krok <paramref name="Size"/></returns>
    Private Shared Function Round(ByVal Number As UShort, ByVal Size As UShort) As UShort
        If Number Mod Size = 0 Then Return Number
        Dim NejblizsiVissi As UShort = (Number \ Size + 1) * Size
        Dim NejblizsiNizsi As UShort = (Number \ Size) * Size
        Dim Nejblizsi As UShort
        If NejblizsiVissi <= NejblizsiNizsi Then
            Nejblizsi = NejblizsiVissi
        Else
            Nejblizsi = NejblizsiNizsi
        End If
        Return Nejblizsi
    End Function
    ''' <summary>Přepočítá X-ovou souřadnici v panelu řádku na čas</summary>
    Private Function X2Time(ByVal X As Integer) As Date
        If X < 0 Then Return Date.MinValue
        Dim Widths As Integer() = tlpMain.GetColumnWidths
        If X > tlpMain.ClientSize.Width - Widths(0) Then Return Date.MaxValue
        Dim SlotTime As Date = Start
        Dim SumX As Integer = 0
        For i As Integer = 0 To Me.SlotsPerwindow - 1
            If X = SumX Then Return SlotTime
            If X < SumX + Widths(i + 1) Then Return SlotTime + TimeSpan.FromMinutes(SlotWidth * ((X - SumX) / Widths(i + 1)))
            SumX += Widths(i + 1)
            SlotTime += TimeSpan.FromMinutes(SlotWidth)
        Next i
        If X = SumX Then Return SlotTime
    End Function
#End Region
#End Region
#Region "Události"
#Region "Pomocné"
    ''' <summary>Vypnutí události</summary>
    Protected Enum Suspension
        ''' <summary>Událost není vypnuta</summary>
        Enabled
        ''' <summary>Budou provedeny úkony spojené s nastanutím události ale nebude událost nebude vyvolána</summary>
        DoNotRaise
        ''' <summary>Nebude provedeno nic</summary>
        DoNothing
    End Enum
    ''' <summary>Obsahuje hodnotu vlastnosti <see cref="PrgEventsState"/></summary>
    <EditorBrowsable(EditorBrowsableState.Never)> Private _PrgEventsState As Suspension = Suspension.Enabled
    ''' <summary>Stav provádění metod <see cref="OnProgramaticallyAddItem"/>, <see cref="OnProgramaticallyClearItems"/>, <see cref="OnProgramaticallyChangeItem"/>, <see cref="OnProgramaticallyChangeItemValue"/> a <see cref="OnProgramaticallyRemoveItem"/></summary>
    Protected Property PrgEventsState() As Suspension
        <DebuggerStepThrough()> Get
            Return _PrgEventsState
        End Get
        Set(ByVal value As Suspension)
            _PrgEventsState = value
        End Set : End Property
    ''' <summary>Přidá handlery událostí k položce zobrazení</summary>
    ''' <param name="Item">Položka k níž přidat handlery</param>
    Private Sub AddItemHandlers(ByVal Item As ItemToolStrip)
        AddHandler Item.BeforeMove, AddressOf Item_BeforeMove
        AddHandler Item.AfterMove, AddressOf Item_AfterMove
        AddHandler Item.EndMove, AddressOf Item_EndResizeMove
        AddHandler Item.EndResize, AddressOf Item_EndResizeMove
        AddHandler Item.BeginMove, AddressOf Item_BeginMove
        AddHandler Item.BeginResize, AddressOf Item_BeginResize
        AddHandler Item.GotFocus, AddressOf Item_GotFocus
        AddHandler Item.LostFocus, AddressOf Item_LostFocus

        AddHandler Item.KeyDown, AddressOf Item_KeyDown
        AddHandler Item.KeyPress, AddressOf Item_KeyPress
        AddHandler Item.KeyUp, AddressOf Item_KeyUp

        AddHandler Item.ItemClicked, AddressOf Item_ItemClicked
        AddHandler Item.Click, AddressOf Item_Click
        AddHandler Item.DoubleClick, AddressOf Item_DoubleClick
        AddHandler Item.MouseDownAll, AddressOf Item_MouseDownAll
        AddHandler Item.MouseClick, AddressOf Item_MouseClick
        AddHandler Item.MouseDoubleClick, AddressOf Item_MouseDoubleClick
        AddHandler Item.MouseEnter, AddressOf Item_MouseEnter
        AddHandler Item.MouseHover, AddressOf Item_MouseHover
        AddHandler Item.MouseLeave, AddressOf Item_MouseLeave
        AddHandler Item.MouseMove, AddressOf Item_MouseMove
        AddHandler Item.MouseUpAll, AddressOf Item_MouseUpAll
        AddHandler Item.MouseWheel, AddressOf Item_MouseWheel
    End Sub
    ''' <summary>Odebere handlery událostí od položky zobrazení</summary>
    ''' <param name="Item">Položka od níž odebrat handlery</param>
    Private Sub RemoveItemHandlers(ByVal Item As ItemToolStrip)
        RemoveHandler Item.BeforeMove, AddressOf Item_BeforeMove
        RemoveHandler Item.AfterMove, AddressOf Item_AfterMove
        RemoveHandler Item.EndMove, AddressOf Item_EndResizeMove
        RemoveHandler Item.EndResize, AddressOf Item_EndResizeMove
        RemoveHandler Item.BeginMove, AddressOf Item_BeginMove
        RemoveHandler Item.BeginResize, AddressOf Item_BeginResize
        RemoveHandler Item.GotFocus, AddressOf Item_GotFocus
        RemoveHandler Item.LostFocus, AddressOf Item_LostFocus

        RemoveHandler Item.KeyDown, AddressOf Item_KeyDown
        RemoveHandler Item.KeyPress, AddressOf Item_KeyPress
        RemoveHandler Item.KeyUp, AddressOf Item_KeyUp

        RemoveHandler Item.ItemClicked, AddressOf Item_ItemClicked
        RemoveHandler Item.Click, AddressOf Item_Click
        RemoveHandler Item.DoubleClick, AddressOf Item_DoubleClick
        RemoveHandler Item.MouseDownAll, AddressOf Item_MouseDownAll
        RemoveHandler Item.MouseClick, AddressOf Item_MouseClick
        RemoveHandler Item.MouseDoubleClick, AddressOf Item_MouseDoubleClick
        RemoveHandler Item.MouseEnter, AddressOf Item_MouseEnter
        RemoveHandler Item.MouseHover, AddressOf Item_MouseHover
        RemoveHandler Item.MouseLeave, AddressOf Item_MouseLeave
        RemoveHandler Item.MouseMove, AddressOf Item_MouseMove
        RemoveHandler Item.MouseUpAll, AddressOf Item_MouseUpAll
        RemoveHandler Item.MouseWheel, AddressOf Item_MouseWheel
    End Sub
#End Region
#Region "ItemAdded"
    ''' <summary>Nastane po programovém přidání položky do kolekce <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></summary>
    <Category("Item Operations")> _
    <Description("Raised after item is added to the Rows.Records collection programatically.")> _
    Public Event ItemAddedProgramatically As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
    ''' <summary>Nastane po přidání položky do kolekce <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></summary>
    <Category("Item Operations")> _
    <Description("Raised after item is added to the Rows.Records collection.")> _
    Public Event ItemAdded As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
    ''' <summary>Nastane po přidání položky do kolekce <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see> uživatelem</summary>
    <Category("Item Operations")> _
    <Description("Raised after item is added to the Rows.Records collection by user.")> _
    Public Event ItemAddedByUser As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
    ''' <summary>Vyvolává událost <see cref="ItemAddingByUser"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnItemAddedByUser(ByVal e As RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
        Dim its As ItemToolStrip = GetItemToolStrip(e.RowIndex, e.OriginalArgs.Item)
        If its IsNot Nothing Then its.Select()
        RaiseEvent ItemAddedByUser(Me, e)
        OnItemAdded(e.RowIndex, New DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(DayViewRow.ItemOperationEventArgsBase.Operations.Added, e.OriginalArgs))
    End Sub
    ''' <summary>Reaguje na přidání položky z kódu</summary>
    ''' <param name="RowIndex">Index do kolekce <see cref="Rows"/></param>
    ''' <param name="e">Parametry události <see cref="DayViewRow.RecordAdded"/></param>
    ''' <remarks>Informace pro potomky: Dbejte na hodnotu <see cref="PrgEventsState"/>!</remarks>
    Protected Overridable Sub OnProgramaticallyAddItem(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
        If PrgEventsState = Suspension.DoNothing Then Exit Sub
        'Dim NewItem As ItemToolStrip = NewToolStrip(e.OperationArgs.Item)
        'If Not (NewItem.Left < 0 AndAlso NewItem.Right <= 0 OrElse NewItem.Left >= RowPanes(RowIndex).Width AndAlso NewItem.Right > RowPanes(RowIndex).Width) Then 'Nepřidávat položky, které jsou mimo průzor
        '    RowPanes(RowIndex).Controls.Add(NewItem)
        '    RowItems(RowIndex).Add(NewItem)
        '    AddItemHandlers(NewItem)
        'End If
        AddNewItemToolStripIfItIsInWindow(e.OperationArgs.Item, RowIndex)
        If PrgEventsState = Suspension.DoNotRaise Then Exit Sub
        RaiseEvent ItemAddedProgramatically(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(RowIndex, e.OperationArgs))
        OnItemAdded(RowIndex, e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemAdded"/></summary>
    ''' <param name="RowIndex">Řádek kde došlo ke změně</param>
    ''' <param name="e">Parametry změny</param>
    Protected Overridable Sub OnItemAdded(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
        ItemTabOrder(RowIndex)
        RaiseEvent ItemAdded(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(RowIndex, e.OperationArgs))
    End Sub
#End Region
#Region "ItemRemoved"
    ''' <summary>Nastane po programovém odebrání položky z kolekce <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></summary>
    <Category("Item Operations")> _
    <Description("Raised after item is removed from the Rows.Records collection programatically.")> _
    Public Event ItemRemovedProgramatically As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
    ''' <summary>Nastane po odebrání položky z kolekce <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></summary>
    <Category("Item Operations")> _
    <Description("Raised after item is removed from the Rows.Records collection.")> _
    Public Event ItemRemoved As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
    ''' <summary>Nastane po odebrání položky z kolekce <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see> uživatelem</summary>
    <Category("Item Operations")> _
    <Description("Raised after item is removed from the Rows.Records collection by user.")> _
    Public Event ItemRemovedByUser As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
    ''' <summary>Vyvolává událost <see cref="ItemRemovedByUser"/> a volá metodu <see cref="ItemRemoved"/></summary>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnItemRemovedByUser(ByVal e As RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
        RaiseEvent ItemRemovedByUser(Me, e)
        OnItemRemoved(e.RowIndex, New DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(DayViewRow.ItemOperationEventArgsBase.Operations.Removed, e.OriginalArgs))
    End Sub
    ''' <summary>Reaguje na odebrání položky z kódu</summary>
    ''' <param name="RowIndex">Index do kolekce <see cref="Rows"/></param>
    ''' <param name="e">Parametry události <see cref="DayViewRow.RecordRemoved"/></param>
    ''' <remarks>Informace pro potomky: Dbejte na hodnotu <see cref="PrgEventsState"/>!</remarks>
    Protected Overridable Sub OnProgramaticallyRemoveItem(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
        If PrgEventsState = Suspension.DoNothing Then Exit Sub
        Dim tos As ItemToolStrip = GetItemToolStrip(RowIndex, e.OperationArgs.Item)
        If tos IsNot Nothing Then
            tos.Parent.Controls.Remove(tos)
            RemoveItemHandlers(tos)
            RowItems(RowIndex).Remove(tos)
        End If
        If PrgEventsState = Suspension.DoNotRaise Then Exit Sub
        RaiseEvent ItemRemovedProgramatically(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(RowIndex, e.OperationArgs))
        OnItemRemoved(RowIndex, e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemRemoved"/></summary>
    ''' <param name="RowIndex">Řádek kde došlo ke změně</param>
    ''' <param name="e">Parametry změny</param>
    Protected Overridable Sub OnItemRemoved(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs))
        ItemTabOrder(RowIndex)
        RaiseEvent ItemRemoved(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(RowIndex, e.OperationArgs))
    End Sub
#End Region
#Region "ItemChanged"
    ''' <summary>Nastane po nahrazení položky v kolekci <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></summary>
    <Category("Item Operations")> _
    <Description("Raised after item in the Rows.Records collection is replaced.")> _
    Public Event ItemChanged As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).OldNewItemEventArgs))
    ''' <summary>Reaguje na nahrazení položky z kódu</summary>
    ''' <param name="RowIndex">Index do kolekce <see cref="Rows"/></param>
    ''' <param name="e">Parametry události <see cref="DayViewRow.RecordChanged"/></param>
    ''' <remarks>Informace pro potomky: Dbejte na hodnotu <see cref="PrgEventsState"/>!</remarks>
    Protected Overridable Sub OnProgramaticallyChangeItem(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).OldNewItemEventArgs))
        If PrgEventsState = Suspension.DoNothing Then Exit Sub
        'Odstranit starou
        Dim tos As ItemToolStrip = GetItemToolStrip(RowIndex, e.OperationArgs.Item)
        If tos IsNot Nothing Then
            RowItems(RowIndex).Remove(tos)
            tos.Parent.Controls.Remove(tos)
            RemoveItemHandlers(tos)
        End If
        'Přidat novou
        AddNewItemToolStripIfItIsInWindow(e.OperationArgs.Item, RowIndex)
        If PrgEventsState = Suspension.DoNotRaise Then Exit Sub
        OnItemChanged(RowIndex, e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemChanged"/></summary>
    ''' <param name="RowIndex">Řádek kde došlo ke změně</param>
    ''' <param name="e">Parametry změny</param>
    Protected Overridable Sub OnItemChanged(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).OldNewItemEventArgs))
        ItemTabOrder(RowIndex)
        RaiseEvent ItemChanged(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).OldNewItemEventArgs)(RowIndex, e.OperationArgs))
    End Sub
#End Region
#Region "ItemValueChanged"
    ''' <summary>Nastane po programové změně vlastnosti položky v kolekci <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></summary>
    <Category("Item Operations")> _
    <Description("Raised after value of property of item in the Rows.Records collection is changed programatically.")> _
    Public Event ItemValueChangedProgramatically As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs))
    ''' <summary>Nastane po změně vlastnosti položky v kolekci <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></summary>
    <Category("Item Operations")> _
    <Description("Raised after value of property of item in the Rows.records collection is changed.")> _
    Public Event ItemValueChanged As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs))
    ''' <summary>Nastane po změně vlastnosti položky v kolekci <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see> uživatelem</summary>
    <Category("Item Operations")> _
    <Description("Raised after value of property of item in the Rows.Records collection is by user.")> _
    Public Event ItemValueChangedByUser As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs))
    ''' <summary>Vyvolává událost <see cref="ItemValueChangedByUser"/> a volá metodu <see cref="OnItemValueChanged"/></summary>
    Protected Overridable Sub OnItemValueChangedByUser(ByVal e As RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs))
        e.OriginalArgs.Item.UserChanged = True
        RaiseEvent ItemValueChangedByUser(Me, e)
        OnItemValueChanged(e.RowIndex, New DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs)(DayViewRow.ItemOperationEventArgsBase.Operations.ItemChanged, e.OriginalArgs))
    End Sub
    ''' <summary>Reaguje na změnu hodnoty vlastnosti položky z kódu</summary>
    ''' <param name="RowIndex">Index do kolekce <see cref="Rows"/></param>
    ''' <param name="e">Parametry události <see cref="DayViewRow.RecordValueChanged"/></param>
    ''' <remarks>Informace pro potomky: Dbejte na hodnotu <see cref="PrgEventsState"/>!</remarks>
    Protected Overridable Sub OnProgramaticallyChangeItemValue(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs))
        If PrgEventsState = Suspension.DoNothing Then Exit Sub
        Dim tos As ItemToolStrip = GetItemToolStrip(RowIndex, e.OperationArgs.Item)
        If tos IsNot Nothing Then
            tos.BackColor = e.OperationArgs.Item.BackColor
            tos.Label.Font = e.OperationArgs.Item.Font
            tos.ForeColor = e.OperationArgs.Item.ForeColor
            tos.CanMove = Not e.OperationArgs.Item.Locked
            tos.GripStyle = If(e.OperationArgs.Item.Locked, ToolStripGripStyle.Hidden, ToolStripGripStyle.Visible)
            tos.Text = e.OperationArgs.Item.Text
            tos.Left = LeftFromStart(e.OperationArgs.Item.Start) 'End
            tos.Width = RightFromEnd(e.OperationArgs.Item.End) - tos.Left 'Start
            'Pokud se položka vysunula mimo průzor, prič s ní
            If tos.Left < 0 AndAlso tos.Right <= 0 OrElse tos.Left >= tos.Parent.ClientRectangle.Width AndAlso tos.Right > tos.Parent.ClientRectangle.Width Then
                RemoveItemHandlers(tos)
                RowItems(RowIndex).Remove(tos)
                tos.Parent.Controls.Remove(tos)
            End If
        Else 'Pokud se položka dostala do průzoru, zobrazit
            AddNewItemToolStripIfItIsInWindow(e.OperationArgs.Item, RowIndex)
        End If
        If PrgEventsState = Suspension.DoNotRaise Then Exit Sub
        RaiseEvent ItemValueChangedProgramatically(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs)(RowIndex, e.OperationArgs))
        OnItemValueChanged(RowIndex, e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemValueChanged"/></summary>
    ''' <param name="RowIndex">Řádek kde došlo ke změně</param>
    ''' <param name="e">Parametry změny</param>
    <DebuggerStepThrough()> Protected Overridable Sub OnItemValueChanged(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs))
        RaiseEvent ItemValueChanged(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemValueChangedEventArgs)(RowIndex, e.OperationArgs))
    End Sub
#End Region
#Region "Cleared"
    ''' <summary>Nastane po vyprázdnění kolekce <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see> metodou <see cref="ListWithEvents(Of DayViewItem).Clear"/></summary>
    <Category("Item Operations")> _
    <Description("Raised after the Rows.Records collection is cleared.")> _
    Public Event ItemsCleared As EventHandler(Of RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemsEventArgs))
    ''' <summary>Reaguje na odstranění všech položek z kódu</summary>
    ''' <param name="RowIndex">Index do kolekce <see cref="Rows"/></param>
    ''' <param name="e">Parametry události <see cref="DayViewRow.RecordsCleared"/></param>
    ''' <remarks>Informace pro potomky: Dbejte na hodnotu <see cref="PrgEventsState"/>!</remarks>
    Protected Overridable Sub OnProgramaticallyClearItems(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemsEventArgs))
        If PrgEventsState = Suspension.DoNothing Then Exit Sub
        RowPanes(RowIndex).Controls.Clear()
        For Each item As ItemToolStrip In RowItems(RowIndex)
            RemoveItemHandlers(item)
        Next item
        RowItems(RowIndex).Clear()
        If PrgEventsState = Suspension.DoNotRaise Then Exit Sub
        OnItemsCleared(RowIndex, e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemsCleared"/></summary>
    ''' <param name="RowIndex">Řádek kde došlo ke změně</param>
    ''' <param name="e">Parametry změny</param>
    <DebuggerStepThrough()> Protected Overridable Sub OnItemsCleared(ByVal RowIndex As Integer, ByVal e As DayViewRow.ItemOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemsEventArgs))
        RaiseEvent ItemsCleared(Me, New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemsEventArgs)(RowIndex, e.OperationArgs))
    End Sub
#End Region
#Region "Pohyb"
    ''' <summary>Zajišťuje že položky budou přesouvány jen tam, kde je volno, bodou umisťovány k intervalům a nevyběhnou ven</summary>
    ''' <param name="sender">Zdroj události <see cref="ItemToolStrip.BeforeMove"/></param>
    ''' <param name="e">Parametry události <see cref="ItemToolStrip.BeforeMove"/></param>
    Private Sub Item_BeforeMove(ByVal sender As Object, ByVal e As CancelMoveEventArgs)
        With DirectCast(sender, ItemToolStrip)
            Dim OldTag2 As DateRangeEventArgs = Nothing
            Dim ItemParent As Panel = Nothing
            Try
                OldTag2 = .Tag2
                'Příprava              
                If TypeOf e Is CancelMoveRowEventArgs Then 'Signalizuje validaci ještě nevytvořené položky, která nemá rodiče
                    ItemParent = RowPanes(DirectCast(e, CancelMoveRowEventArgs).Row)
                Else
                    ItemParent = .Parent
                End If
                'Testy a úpravy polohy
                If DirectCast(.Tag, DayViewItem).Start >= Start AndAlso (.MovingState = ItemToolStrip.MoveStates.LeftResizing Or .MovingState = ItemToolStrip.MoveStates.Moving) Then
                    If e.x < 0 OrElse e.x > ItemParent.ClientSize.Width Then e.Cancel = True : Exit Sub
                    Dim NewStart As Date = X2Time(e.x)
                    If NewStart > Start + TimeWindow - TimeSpan.FromMinutes(MinimalUserChange) Then e.Cancel = True : Exit Sub 'Začátek nesmí být úplně na konci
                    Dim StartOffset As UShort = (NewStart - Start).TotalMinutes
                    If StartOffset Mod MinimalUserChange = 0 Then
                        'OK
                    Else
                        Dim NejblizsiStart As UShort = Round(StartOffset, MinimalUserChange)
                        If Math.Abs(StartOffset - NejblizsiStart) < MinimalUserChange / 3 OrElse TypeOf e Is CancelMoveRowEventArgs Then
                            'zaokrouhlení začátku na povolený krok
                            Dim newX As Integer = LeftFromStart(Start + TimeSpan.FromMinutes(NejblizsiStart))
                            Dim oldRight As Integer = e.Rectangle.Right
                            e.x = newX
                            If .MovingState = ItemToolStrip.MoveStates.LeftResizing Then
                                e.Width = oldRight - newX 'Zachování pozice pravé hrany
                            End If
                        Else
                            'Nelze nasunout začátek jentak někam ale jen po povolených krocích
                            e.Cancel = True
                            Exit Sub
                        End If
                    End If
                End If
                If DirectCast(.Tag, DayViewItem).End <= Start + TimeWindow AndAlso .MovingState = ItemToolStrip.MoveStates.RightResizing Then
                    If e.Rectangle.Right > ItemParent.ClientSize.Width OrElse e.Rectangle.Right < 0 Then e.Cancel = True : Exit Sub
                    Dim NewEnd As Date = X2Time(e.Rectangle.Right)
                    If NewEnd < Start + TimeSpan.FromMinutes(MinimalUserChange) Then e.Cancel = True : Exit Sub 'Konec nesmí být úplně na začátku
                    Dim EndOffset As UShort = (NewEnd - Start).TotalMinutes
                    If EndOffset Mod MinimalUserChange = 0 Then
                        'OK
                    Else
                        Dim NejblizsiKonec As UShort = Round(EndOffset, MinimalUserChange)
                        If Math.Abs(EndOffset - NejblizsiKonec) < MinimalUserChange / 3 Then
                            'Zaokrouhlení konce na povolený krok
                            e.Width = RightFromEnd(Start + TimeSpan.FromMinutes(NejblizsiKonec)) - e.x
                        Else
                            'Nelze nasunout konec jentak někam, ale jen po povolených krocích
                            e.Cancel = True
                            Exit Sub
                        End If
                    End If
                End If
                If e.x < 0 Then
                    If DirectCast(.Tag, DayViewItem).Start < Start AndAlso .MovingState = ItemToolStrip.MoveStates.RightResizing Then
                        'OK
                    Else
                        'Začátek nelze vysunout ven (ale lze tu věc resizovat vpravo)
                        e.Cancel = True
                        Exit Sub
                    End If
                ElseIf e.Rectangle.Right > ItemParent.ClientSize.Width Then
                    If DirectCast(.Tag, DayViewItem).End > Start + TimeWindow AndAlso (.MovingState = ItemToolStrip.MoveStates.LeftResizing OrElse .MovingState = ItemToolStrip.MoveStates.Moving) Then
                        If .MovingState = ItemToolStrip.MoveStates.LeftResizing Then
                            e.Width = ItemParent.ClientSize.Width + 10 - e.x
                        Else 'if .MovingState=ItemToolStrip.MoveStates.Moving 
                            e.Width = RightFromEnd(DirectCast(.Tag, DayViewItem).End) - e.x
                        End If
                    Else
                        'Konec nelze vysunout ven (ale lze tu věc posouvat a měnit jí velikost)
                        e.Cancel = True
                        Exit Sub
                    End If
                Else
                    For Each ctl As Control In ItemParent.Controls
                        'Nelze nasunout někam, kde už něco je
                        If ctl IsNot sender Then
                            If (e.x >= ctl.Left AndAlso e.x < ctl.Right) OrElse _
                                (e.Rectangle.Right > ctl.Left AndAlso e.Rectangle.Right <= ctl.Right) OrElse _
                                (e.x <= ctl.Left AndAlso e.Rectangle.Right >= ctl.Right) Then
                                e.Cancel = True
                                Exit Sub
                            End If
                        End If
                    Next ctl
                End If
                If .MovingState = ItemToolStrip.MoveStates.Moving Then 'AndAlso .Right > Parent.ClientSize.Width Then
                    'Ošetření přesunu položky umístěné přes konec zobrazené plochy
                    Dim OldStart As Date, OldEnd As Date
                    Dim NewStart As Date = X2Time(e.x)
                    If .Tag2 Is Nothing Then
                        With DirectCast(.Tag, DayViewItem)
                            OldStart = .Start
                            OldEnd = .End
                        End With
                    Else
                        With DirectCast(.Tag2, DateRangeEventArgs)
                            OldStart = .Start
                            OldEnd = .End
                        End With
                    End If
                    Dim NewEnd As Date = OldEnd - (OldStart - NewStart)
                    If NewEnd.Date <> OldStart.Date AndAlso NewEnd <> OldStart.Date.AddDays(1) Then
                        e.Cancel = True
                        Exit Sub
                    End If
                    .Tag2 = New DateRangeEventArgs(NewStart, NewEnd)
                    Dim EndRight As Integer = RightFromEnd(NewEnd)
                    ' If EndRight <= Parent.ClientSize.Width Then
                    e.Width = EndRight - e.x
                    'End If
                    If e.Rectangle.Right > ItemParent.ClientRectangle.Width OrElse e.x < 0 Then
                        'Ověření položky šoupoucí se mimo okraje k neviditelným položkám
                        For Each Item As DayViewItem In Me.Rows(RowFromItem(.Tag)).Records
                            If Item IsNot .Tag Then
                                If (NewStart >= Item.Start AndAlso NewStart < Item.End) OrElse _
                                    (NewEnd > Item.Start AndAlso NewEnd <= Item.End) OrElse _
                                    (NewStart <= Item.Start AndAlso NewEnd >= Item.End) Then
                                    e.Cancel = True
                                    Exit Sub
                                End If
                            End If
                        Next Item
                    End If
                End If
                If .MovingState = ItemToolStrip.MoveStates.LeftResizing OrElse .MovingState = ItemToolStrip.MoveStates.RightResizing Then
                    'Ošetření MinimumItemwidth
                    Dim Start As Date
                    Dim [End] As Date
                    If e.Rectangle.Right > ItemParent.ClientSize.Width Then
                        If .Tag2 Is Nothing Then
                            [End] = DirectCast(.Tag, DayViewItem).End
                        Else
                            [End] = DirectCast(.Tag2, DateRangeEventArgs).End
                        End If
                    Else
                        [End] = X2Time(e.Rectangle.Right)
                    End If
                    If e.x < 0 Then
                        If .Tag2 Is Nothing Then
                            Start = DirectCast(.Tag, DayViewItem).Start
                        Else
                            Start = DirectCast(.Tag2, DateRangeEventArgs).Start
                        End If
                    Else
                        Start = X2Time(e.x)
                    End If
                    If ([End] - Start).TotalMinutes < Me.MinimumItemWidth Then
                        e.Cancel = True
                        Exit Sub
                    End If
                End If
                If BlockEvMorn Then
                    'Ošetření Morning a Evening
                    Dim StartBeforeMove As Date = DirectCast(.Tag, DayViewItem).Start
                    Dim EndBeforeMove As Date = DirectCast(.Tag, DayViewItem).End
                    Dim StartDuringMove As Date, EndDuringMove As Date
                    If .Tag2 Is Nothing Then
                        StartDuringMove = StartBeforeMove
                        EndDuringMove = EndBeforeMove
                    Else
                        StartDuringMove = DirectCast(.Tag2, DateRangeEventArgs).Start
                        EndDuringMove = DirectCast(.Tag2, DateRangeEventArgs).End
                    End If
                    Dim EndAfterMove As Date, StartAfterMove As Date
                    Select Case .MovingState
                        Case ItemToolStrip.MoveStates.LeftResizing, ItemToolStrip.MoveStates.Moving
                            StartAfterMove = X2Time(e.x)
                            If e.Rectangle.Right <= ItemParent.ClientRectangle.Width Then
                                EndAfterMove = X2Time(e.Rectangle.Right)
                            ElseIf .MovingState = ItemToolStrip.MoveStates.LeftResizing Then
                                EndAfterMove = EndDuringMove
                            Else
                                EndAfterMove = EndDuringMove + (StartAfterMove - StartDuringMove)
                            End If
                        Case ItemToolStrip.MoveStates.RightResizing
                            EndAfterMove = X2Time(e.Rectangle.Right)
                            StartAfterMove = StartDuringMove
                    End Select
                    If _
                        (StartBeforeMove.TimeOfDay >= Morning AndAlso StartAfterMove.TimeOfDay < Morning) _
                        OrElse _
                        (EndBeforeMove.TimeOfDay <= Evening AndAlso EndAfterMove.TimeOfDay > Evening) _
                        OrElse _
                        (StartAfterMove.TimeOfDay < Morning AndAlso StartAfterMove < StartBeforeMove) _
                        OrElse _
                        (EndAfterMove.TimeOfDay > Evening AndAlso EndAfterMove > EndBeforeMove) _
                    Then
                        e.Cancel = True
                        Exit Sub
                    End If
                End If
            Finally
                If e.Cancel Then
                    .Tag2 = OldTag2
                End If
            End Try
        End With
    End Sub
    ''' <summary>Zobrazuje <see cref="ToolTip"/> s rozměry měněné položky</summary>
    Private Sub Item_AfterMove(ByVal sender As Object, ByVal e As MoveEventArgs)
        With DirectCast(sender, ItemToolStrip)
            Dim x As Integer
            Select Case .MovingState
                Case ItemToolStrip.MoveStates.LeftResizing, ItemToolStrip.MoveStates.Moving
                    x = 0
                Case ItemToolStrip.MoveStates.RightResizing
                    x = .Width - 50
            End Select
            'If .MovingState = ItemToolStrip.MoveStates.Moving AndAlso .Right > .Parent.ClientSize.Width Then
            '    Dim OldStart As Date
            '    Dim OldEnd As Date
            '    If .Tag2 IsNot Nothing Then
            '        OldStart = DirectCast(.Tag2, DateRangeEventArgs).Start
            '        OldEnd = DirectCast(.Tag2, DateRangeEventArgs).End
            '    Else
            '        OldStart = DirectCast(.Tag, DayViewItem).Start
            '        OldEnd = DirectCast(.Tag, DayViewItem).End
            '    End If

            'End If
            Dim StartTime As Date
            Dim EndTime As Date
            'Pokud je prvek mimo rodiče načítá se datum z Tag2 (aktualizované) nebo z přidružené položky (původní)
            If .Left >= 0 Then
                StartTime = X2Time(.Left)
            ElseIf .Tag2 IsNot Nothing Then
                StartTime = DirectCast(.Tag2, DateRangeEventArgs).Start
            Else
                StartTime = DirectCast(.Tag, DayViewItem).Start
            End If
            If .Right <= .Parent.ClientSize.Width Then
                EndTime = X2Time(.Right)
            ElseIf .Tag2 IsNot Nothing Then
                EndTime = DirectCast(.Tag2, DateRangeEventArgs).End
            Else
                EndTime = DirectCast(.Tag, DayViewItem).End
            End If
            totToolTip.Show(String.Format("{0:H:mm}÷{1:H:mm}", StartTime, EndTime), sender, x, .Height)
        End With
    End Sub
    ''' <summary>Začátek pohybu položky</summary>
    ''' <remarks><seealso cref="Item_BeginResize"/></remarks>
    Private Sub Item_BeginMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        With DirectCast(sender, ItemToolStrip)
            .Tag2 = Nothing
        End With
    End Sub
    ''' <summary>Začátek změn velikosti položky</summary>
    ''' <remarks><seealso cref="Item_BeforeMove"/></remarks>
    Private Sub Item_BeginResize(ByVal sender As Object, ByVal e As ItemToolStrip.BeginResizeEventArgs)
        With DirectCast(sender, ItemToolStrip)
            .Tag2 = Nothing
        End With
    End Sub
    ''' <summary>Nastane před tím než dojde k definitivní změně pozice položky uživatelem (nastává ve chvíli, kdy uživatel uvolní tlačítko myši používané ke změně pozice položky)</summary>
    ''' <remarks>Tuto událost lze stornovat</remarks>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Action)> _
    <Description("Fired before change of position of item by user is finally accepted. Fired when user releases mouse button used to change item position.")> _
    Public Event BeforeEndMoveItem As EventHandler(Of ItemMoveCancelEventArgs)
    ''' <summary>Vyvolává událost <see cref="BeforeEndMoveItem"/></summary>
    ''' <param name="e">Parametry události. Lze využít ke stornování.</param>
    <DebuggerStepThrough()> Protected Overridable Sub OnBeforeEndMoveItem(ByVal e As ItemMoveCancelEventArgs)
        RaiseEvent BeforeEndMoveItem(Me, e)
    End Sub
    ''' <summary>Konec změny velikosti nebo polohy položky</summary>
    Private Sub Item_EndResizeMove(ByVal sender As Object, ByVal e As EventArgs)
        totToolTip.Hide(sender)
        With DirectCast(sender, ItemToolStrip)
            'Kontrola nové položky
            Dim Found As Boolean = False
            For Each row As DayViewRow In Rows
                If row.Records.Contains(DirectCast(.Tag, DayViewItem)) Then Found = True : Exit For
            Next row
            If Not Found Then PerformUserAdd(sender) : Exit Sub

            Dim item As DayViewItem = .Tag
            Dim StartTime As Date
            Dim EndTime As Date
            'Pokud položka vyčuhuje z plochy nelze její čas určit z x pozice
            If .Left >= 0 Then
                StartTime = X2Time(.Left)
            Else
                StartTime = item.Start
            End If
            If .Right <= .Parent.ClientSize.Width Then
                EndTime = X2Time(.Right)
            Else
                EndTime = item.End
            End If
            .Tag2 = Nothing
            Dim eA As New ItemMoveCancelEventArgs(RowFromItem(.Tag), Me.Rows(RowFromItem(.Tag)).Records.IndexOf(DirectCast(.Tag, DayViewItem)), DirectCast(.Tag, DayViewItem).Start, DirectCast(.Tag, DayViewItem).End, StartTime, EndTime)
            OnBeforeEndMoveItem(eA)
            If eA.Cancel Then 'Storno pohybu
                .Left = LeftFromStart(DirectCast(.Tag, DayViewItem).Start)
                .Width = RightFromEnd(DirectCast(.Tag, DayViewItem).End) - .Left
            Else
                PrgEventsState = Suspension.DoNotRaise
                Try
                    item.Start = StartTime
                    item.Length = (EndTime - item.Start).TotalMinutes
                    item.UserChanged = True
                Finally
                    PrgEventsState = Suspension.Enabled
                End Try
            End If
        End With
    End Sub
    ''' <summary>Provádí posun položky z klávesnice</summary>
    ''' <param name="Item">Položka</param>
    ''' <param name="Dir">Směr</param> 
    ''' <exception cref="InvalidEnumArgumentException"><paramref name="Dir"/> není členem <see cref="LeftRightAlignment"/></exception>
    Private Sub MoveItem(ByVal Item As ItemToolStrip, ByVal Dir As LeftRightAlignment)
        With DirectCast(Item.Tag, DayViewItem)
            If .Locked Then Exit Sub
            Dim NewX As Integer, NewR As Integer
            If Dir = LeftRightAlignment.Right Then
                NewX = LeftFromStart(.Start + TimeSpan.FromMinutes(MinimalUserChange))
                NewR = RightFromEnd(.End + TimeSpan.FromMinutes(MinimalUserChange))
            ElseIf Dir = LeftRightAlignment.Left Then
                NewX = LeftFromStart(.Start - TimeSpan.FromMinutes(MinimalUserChange))
                NewR = RightFromEnd(.End - TimeSpan.FromMinutes(MinimalUserChange))
            Else
                Throw New InvalidEnumArgumentException("Dir", Dir, GetType(LeftRightAlignment))
            End If
            If NewX < 0 OrElse NewX >= Item.Parent.ClientRectangle.Width OrElse NewR < 0 OrElse NewR > Item.Parent.ClientRectangle.Width Then Exit Sub
            Dim e As New CancelMoveEventArgs(NewX, Item.Top, NewR - NewX, Item.Height)
            Item_BeforeMove(Item, e)
            If e.Cancel Then Exit Sub
            Item.Left = e.x
            Item.Width = e.Width
            Item_AfterMove(Item, New MoveEventArgs(Item.Location, Item.Size))
            Item_EndResizeMove(Item, EventArgs.Empty)
        End With
    End Sub
#End Region
#Region "Uživatelské přidávání"
    ''' <summary>Logika přidání položky uživatelem</summary>
    ''' <param name="sender"><see cref="ItemToolStrip"/> reprezentující položku, která je přidávána</param>
    Private Sub PerformUserAdd(ByVal sender As ItemToolStrip)
        Dim RowIndex As Integer = RowPanes.IndexOf(sender.Parent)
        sender.Parent.Controls.Remove(sender)
        RemoveItemHandlers(sender)
        With DirectCast(sender.Tag, DayViewItem)
            Dim item As New DayViewItem(X2Time(sender.Left), (X2Time(sender.Right) - X2Time(sender.Left)).TotalMinutes, sender.Text, , , sender.LabelFont)
            item.BackColor = sender.BackColor
            item.ForeColor = sender.ForeColor
            item.UserChanged = True
            Dim e As New ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs(item, RowIndex)
            OnItemAddingByUser(e)
            If e.Cancel Then
                MsgBox(IfNull(e.CancelMessage, "Položku nelze přidat."), MsgBoxStyle.Exclamation, "Položku nelze přidat")
            Else
                PrgEventsState = Suspension.DoNotRaise
                Me.Rows(e.NewIndex).Records.Add(e.Item)
                OnItemAddedByUser(New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(RowIndex, New ListWithEvents(Of DayViewItem).ItemIndexEventArgs(e.Item, Rows(RowIndex).Records.Count - 1)))
            End If
        End With
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemaddingByUser"/></summary>
    ''' <param name="e">Parametry události. Proměnná <see cref="ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs.NewIndex"/> neobsahuje index položky ale index řádku!</param>
    ''' <remarks>Změnou hodnoty vlastnosti <see cref="ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs.Item"/> nebo jejích vlastností můžete upravit přidávanou položku</remarks>
    Private Sub OnItemAddingByUser(ByVal e As ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs)
        RaiseEvent ItemAddingByUser(Me, e)
    End Sub
    ''' <summary>Nastane před přidáním položky uživatelem. Operaci je možné stornovat</summary>
    ''' <param name="e">Parametry události. Proměnná <see cref="ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs.NewIndex"/> neobsahuje index položky ale index řádku!</param>
    ''' <remarks>Pokud událost stornujete pomocí <see cref="ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs.Cancel"/>, nastavte hodnotu <see cref="ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs.CancelMessage"/> an zprávu, která má být zobrazena. Jinak bude zobrazena obecná zpráva.</remarks>
    <Category("Item Operations")> _
    <Description("Raised before item is added by user. Allows cancelation.")> _
    Public Event ItemAddingByUser As EventHandler(Of ListWithEvents(Of DayViewItem).CancelableItemIndexEventArgs)
#End Region
#Region "Standardní události ItemToolStripů"
#Region "Focus"
    ''' <summary>Ošetřuje získání focusu položkou</summary>
    Private Sub Item_GotFocus(ByVal sender As Object, ByVal e As EventArgs)
        LastActiveItem = DirectCast(sender, ItemToolStrip).Tag
        OnActiveItemChanged(EventArgs.Empty)
    End Sub
    ''' <summary>Ošetřuje ztrátu focusu položkou</summary>
    Private Sub Item_LostFocus(ByVal sender As Object, ByVal e As EventArgs)
        OnActiveItemChanged(EventArgs.Empty)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ActiveItemChanged"/></summary>
    ''' <param name="e">Parametry události</param>
    <DebuggerStepThrough()> Protected Overridable Sub OnActiveItemChanged(ByVal e As EventArgs)
        RaiseEvent ActiveItemChanged(Me, e)
    End Sub
    ''' <summary>Nastane po změně vlastnosti <see cref="ActiveItem"/></summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Focus)> _
    <Description("Fired after the ActiveItem property is changed")> _
    Public Event ActiveItemChanged As EventHandler
    ''' <summary>Uživatelem vybraná položka</summary>
    ''' <exception cref="InvalidOperationException">Pokus o aktivaci položky která není v průzoru</exception>
    ''' <exception cref="ArgumentException">Pokus o aktivaci položky, která se nanachází v <see cref="Rows">Rows</see>.<see cref="DayViewRow.Records">Records</see></exception>
    ''' <exception cref="ArgumentNullException">Nastavovaná hodnota je null a <see cref="ActiveItem"/> není null</exception>
    ''' <value>Nesmí bý null - nelze deaktivovat položku</value>
    ''' <returns>Vrací aktivní položku nebo null pokud není žádná aktivní položka</returns>
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden), Browsable(False)> _
    Public Property ActiveItem() As DayViewItem
        Get
            If Me.ActiveControl Is Nothing OrElse Not TypeOf Me.ActiveControl Is ItemToolStrip Then Return Nothing
            Return Me.ActiveControl.Tag
        End Get
        Set(ByVal value As DayViewItem)
            Dim OldActiveItem As DayViewItem = ActiveItem
            If value Is Nothing AndAlso ActiveItem IsNot Nothing Then Throw New ArgumentNullException("value", "Cannot set ActiveItem to null")
            If value Is Nothing Then Exit Property
            'Vyhledávání položky
            Dim RowI As Integer = 0
            For Each Row As DayViewRow In Me.Rows
                For Each Item As DayViewItem In Row.Records
                    If Item Is value Then
                        Dim its As ItemToolStrip = GetItemToolStrip(RowI, Item)
                        If its IsNot Nothing Then its.Select() Else Throw New InvalidOperationException("Položku, která není vidět nezle aktivovat")
                        'Select vyvolá událost ActiveItemChanged
                        Exit Property
                    End If
                Next Item
                RowI += 1
            Next Row
            Throw New ArgumentException("Položka není obsažena v tomto DayView a proto ji nelze aktivivat")
        End Set
    End Property
    ''' <summary>Naposledy aktivovaná položka</summary>
    Private LastActiveItem As DayViewItem
    ''' <summary>Nastaví vlastnosti <see cref="ItemToolStrip.TabIndex"/> položek v daném řádku</summary>
    ''' <param name="Row">Index řádku</param>
    Protected Sub ItemTabOrder(ByVal Row As Integer)
        Dim Items As New List(Of ItemToolStrip)(New Tools.CollectionsT.GenericT.Wrapper(Of ItemToolStrip)(RowPanes(Row).Controls))
        Items.Sort(AddressOf ItemToolStripDayViewTagComparison)
        Dim ti As Integer = 0
        For Each item As ItemToolStrip In Items
            item.TabIndex = ti
            ti += 1
        Next item
    End Sub
    ''' <summary>Represents the method that compares two objects of the same type.</summary>
    ''' <returns>Value Condition Less than 0 if <paramref name="x"/> is less than <paramref name="y"/>; 0 if <paramref name="x"/> equals <paramref name="y"/>; greater than 0 if <paramref name="x"/> is greater than <paramref name="y"/>.</returns>
    <DebuggerStepThrough()> Private Shared Function ItemToolStripDayViewTagComparison(ByVal x As ItemToolStrip, ByVal y As ItemToolStrip) As Integer
        Return Date.Compare(DirectCast(x.Tag, DayViewItem).Start, DirectCast(y.Tag, DayViewItem).Start)
    End Function
#End Region
#Region "Key"
#Region "KeyDown"
    ''' <summary>Reaguje na událost <see cref="ItemToolStrip.KeyDown"/> položky</summary>
    ''' <param name="sender">Původce události</param>
    ''' <param name="e">Argumenty události</param>
    <DebuggerStepThrough()> Private Sub Item_KeyDown(ByVal sender As Object, ByVal e As KeyEventArgs)
        OnItemKeyDown(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Reaguje na stisknutí klávesy nad položkou (pro <see cref="Keys.Delete"/> maže nauzamčenou položku)</summary>
    ''' <param name="ItemControl">Ovládací prvek, který položku reprezentuje</param>
    ''' <param name="Item">Položka</param>
    ''' <param name="e">Parametry události</param>
    ''' <remarks>Default behaviour:
    ''' <list type="table">
    ''' <listheader><term>Key</term><description>Action performed</description></listheader>
    ''' <item><term><see cref="Keys.Delete"/> (no Ctrl, Shift or Alt)</term><description>Performs user-caused deleting of item.</description></item>
    ''' <item><term><see cref="Keys.Left"/> (no Ctrl, Shift or Alt)</term><description>Performs user-caused moving of item left (towards morning) by <see cref="MinimalUserChange"/></description></item>
    ''' <item><term><see cref="Keys.Right"/> (no Ctrl, Shift or Alt)</term><description>Performs user-caused moving of item right (towards evening) by <see cref="MinimalUserChange"/></description></item>
    ''' <item><term><see cref="Keys.Escape"/> (Ctrl, Shift, Alt independent)</term><description>If current item is new item actually beeing placed by user, placement is canceled (item is destroyed before it is added).</description></item>
    ''' </list>
    ''' </remarks>
    Protected Overridable Sub OnItemKeyDown(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As KeyEventArgs)
        Dim e2 As DayView.ItemKeyEventArgs = New ItemKeyEventArgs(e, Item)
        RaiseEvent ItemKeyDown(Me, e2)
        If e2.Handled Then Return
        Select Case e.KeyCode
            Case Keys.Delete
                If Not e.Control AndAlso Not e.Shift AndAlso Not e.Alt Then
                    UserDeleteItem(Item)
                End If
            Case Keys.Left
                If Not e.Control AndAlso Not e.Shift AndAlso Not e.Alt Then
                    MoveItem(ItemControl, LeftRightAlignment.Left)
                End If
            Case Keys.Right
                If Not e.Control AndAlso Not e.Shift AndAlso Not e.Alt Then
                    MoveItem(ItemControl, LeftRightAlignment.Right)
                End If
            Case Keys.Escape
                Dim Found As Boolean = False
                For Each row As DayViewRow In Rows
                    If row.Records.Contains(DirectCast(ItemControl.Tag, DayViewItem)) Then Found = True : Exit For
                Next row
                If Not Found Then
                    totToolTip.Hide(ItemControl)
                    RemoveItemHandlers(ItemControl)
                    ItemControl.Parent.Controls.Remove(ItemControl)
                End If
                'Case Keys.Tab
                '    If Not e.Control AndAlso Not e.Alt Then
                '        SelectNext(ItemControl, Not e.Shift)
                '    End If
        End Select
    End Sub
    ''' <summary>Nastane po stlačení klávesy na položce</summary>
    ''' <remarks>There is defined default behavior for certain keys (performed by <see cref="OnItemKeyDown"/>). To suppress this behaviour set <paramref name="e"/>.<see cref="ItemKeyEventArgs.Handled">Handled</see> to true.</remarks>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Key)> _
    <Description("Raised after key is pushed down on item.")> _
    Public Event ItemKeyDown As EventHandler(Of ItemKeyEventArgs)
#End Region
#Region "KeyPress"
    ''' <summary>Reaguje na událost <see cref="ItemToolStrip.KeyPress"/> položky</summary>
    ''' <param name="sender">Zdroj události</param>
    ''' <param name="e">Parametry události</param>
    Private Sub Item_KeyPress(ByVal sender As Object, ByVal e As KeyPressEventArgs)
        OnItemKeyPress(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemKeyPress"/></summary>
    ''' <param name="ItemControl">Control that represents <paramref name="item"/></param>
    ''' <param name="item">Item that caused the event</param>
    ''' <param name="e">Event arguments</param>
    ''' <remarks>Reaguje na událost <see cref="ItemToolStrip.KeyPress"/> položky</remarks>
    Private Sub OnItemKeyPress(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As KeyPressEventArgs)
        RaiseEvent ItemKeyPress(Me, New ItemKeyPressEventArgs(e, Item))
    End Sub
    ''' <summary>Nastane po stisku klávesy na položce</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Key)> _
    <Description("Raised after key is pressed on item.")> _
    Public Event ItemKeyPress As EventHandler(Of ItemKeyPressEventArgs)
#End Region
#Region "KeyUp"
    ''' <summary>Reaguje na událost <see cref="ItemToolStrip.KeyUp"/> položky</summary>
    ''' <param name="sender">Původce události</param>
    ''' <param name="e">Argumenty události</param>
    Private Sub Item_KeyUp(ByVal sender As Object, ByVal e As KeyEventArgs)
        OnItemKeyUp(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemKeyUp"/></summary>
    ''' <param name="ItemControl">Ovládací prvek, který položku reprezentuje</param>
    ''' <param name="Item">Položka</param>
    ''' <param name="e">Parametry události</param>
    Private Sub OnItemKeyUp(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As KeyEventArgs)
        RaiseEvent ItemKeyUp(Me, New ItemKeyEventArgs(e, Item))
    End Sub
    ''' <summary>Nastane po uvolnění klávesy na položce</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Key)> _
    <Description("Raised after key is reeleased on item.")> _
    Public Event ItemKeyUp As EventHandler(Of ItemKeyEventArgs)
#End Region
#End Region
#Region "Mouse"
#Region "MouseDownAll"
    ''' <summary>Reaguje na událost <see cref="ItemToolStrip.MouseDownAll"/> položky</summary>
    <DebuggerStepThrough()> Private Sub Item_MouseDownAll(ByVal sender As Object, ByVal e As MouseEventArgs)
        OnItemMouseDownAll(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Reaguje na stisknutí myši nad položkou - pro neuzamčené položky zobrazí kontextové menu při stisknutí pravého tlačítka.</summary>
    ''' <param name="ItemControl">Ovládací prvek, který položku reprezentuje</param>
    ''' <param name="Item">Položka</param>
    ''' <param name="e">Parametry události</param>
    Protected Overridable Sub OnItemMouseDownAll(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As MouseEventArgs)
        If Not Item.Locked AndAlso e.Button = Windows.Forms.MouseButtons.Right Then
            cmsItem.Show(ItemControl, e.Location)
        End If
        RaiseEvent ItemMouseDown(Me, New ItemMouseEventArgs(e, Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseDown"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseDown ocures for an item.")> _
    Public Event ItemMouseDown As EventHandler(Of ItemMouseEventArgs)
#End Region
#Region "ItemClicked"
    ''' <summary>Handles the <see cref="ItemToolStrip.ItemClicked"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_ItemClicked(ByVal sender As Object, ByVal e As ToolStripItemClickedEventArgs)
        OnItemItemClicked(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemItemClicked"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemItemClicked(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As ToolStripItemClickedEventArgs)
        RaiseEvent ItemItemClicked(Me, New ItemToolStripItemClickedEventArgs(Item, e))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.ItemClicked"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event ItemClicked ocures for an item.")> _
    Public Event ItemItemClicked As EventHandler(Of ItemToolStripItemClickedEventArgs)
#End Region
#Region "Click"
    ''' <summary>Handles the <see cref="ItemToolStrip.Click"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_Click(ByVal sender As Object, ByVal e As EventArgs)
        OnItemClick(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemClick"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemClick(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As EventArgs)
        RaiseEvent ItemClick(Me, New ItemEventArgs(Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.Click"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event Click ocures for an item.")> _
    Public Event ItemClick As EventHandler(Of ItemEventArgs)
#End Region
#Region "DoubleClick"
    ''' <summary>Handles the <see cref="ItemToolStrip.DoubleClick"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_DoubleClick(ByVal sender As Object, ByVal e As EventArgs)
        OnItemDoubleClick(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemDoubleClick"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemDoubleClick(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As EventArgs)
        RaiseEvent ItemDoubleClick(Me, New ItemEventArgs(Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.DoubleClick"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event DoubleClick ocures for an item.")> _
    Public Event ItemDoubleClick As EventHandler(Of ItemEventArgs)
#End Region
#Region "MouseClick"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseClick"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseClick(ByVal sender As Object, ByVal e As MouseEventArgs)
        OnItemMouseClick(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseClick"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseClick(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As MouseEventArgs)
        RaiseEvent ItemMouseClick(Me, New ItemMouseEventArgs(e, Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseClick"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseClick ocures for an item.")> _
    Public Event ItemMouseClick As EventHandler(Of ItemMouseEventArgs)
#End Region
#Region "MouseDoubleClick"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseDoubleClick"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseDoubleClick(ByVal sender As Object, ByVal e As MouseEventArgs)
        OnItemMouseDoubleClick(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseDoubleClick"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseDoubleClick(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As MouseEventArgs)
        RaiseEvent ItemMouseDoubleClick(Me, New ItemMouseEventArgs(e, Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseDoubleClick"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseDoubleClick ocures for an item.")> _
    Public Event ItemMouseDoubleClick As EventHandler(Of ItemMouseEventArgs)
#End Region
#Region "MouseEnter"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseEnter"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseEnter(ByVal sender As Object, ByVal e As EventArgs)
        OnItemMouseEnter(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseEnter"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseEnter(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As EventArgs)
        RaiseEvent ItemMouseEnter(Me, New ItemEventArgs(Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseEnter"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseEnter ocures for an item.")> _
    Public Event ItemMouseEnter As EventHandler(Of ItemEventArgs)
#End Region
#Region "MouseHover"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseHover"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseHover(ByVal sender As Object, ByVal e As EventArgs)
        OnItemMouseHover(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseHover"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseHover(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As EventArgs)
        RaiseEvent ItemMouseHover(Me, New ItemEventArgs(Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseHover"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseHover ocures for an item.")> _
    Public Event ItemMouseHover As EventHandler(Of ItemEventArgs)
#End Region
#Region "MouseLeave"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseLeave"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseLeave(ByVal sender As Object, ByVal e As EventArgs)
        OnItemMouseLeave(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseLeave"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseLeave(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As EventArgs)
        RaiseEvent ItemMouseLeave(Me, New ItemEventArgs(Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseLeave"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseLeave ocures for an item.")> _
    Public Event ItemMouseLeave As EventHandler(Of ItemEventArgs)
#End Region
#Region "MouseMove"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseMove"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseMove(ByVal sender As Object, ByVal e As MouseEventArgs)
        OnItemMouseMove(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseMove"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseMove(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As MouseEventArgs)
        RaiseEvent ItemMouseMove(Me, New ItemMouseEventArgs(e, Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseMove"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseMove ocures for an item.")> _
    Public Event ItemMouseMove As EventHandler(Of ItemMouseEventArgs)
#End Region
#Region "MouseUpAll"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseUpAll"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseUpAll(ByVal sender As Object, ByVal e As MouseEventArgs)
        OnItemMouseUpAll(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseUp"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseUpAll(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As MouseEventArgs)
        RaiseEvent ItemMouseUp(Me, New ItemMouseEventArgs(e, Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseUp"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseUp ocures for an item.")> _
    Public Event ItemMouseUp As EventHandler(Of ItemMouseEventArgs)
#End Region
#Region "MouseWheel"
    ''' <summary>Handles the <see cref="ItemToolStrip.MouseWheel"/> event of item</summary>
    ''' <param name="sender">Source of event</param>
    ''' <param name="e">Event parameters</param>
    <DebuggerStepThrough()> Private Sub Item_MouseWheel(ByVal sender As Object, ByVal e As MouseEventArgs)
        OnItemMouseWheel(sender, DirectCast(sender, ItemToolStrip).Tag, e)
    End Sub
    ''' <summary>Raises the <see cref="ItemMouseWheel"/> event</summary>
    ''' <param name="ItemControl">Control that represents <paramref name="Item"/></param>
    ''' <param name="Item">Item that caused the event</param>
    ''' <param name="e">Event parameters</param>
    Protected Overridable Sub OnItemMouseWheel(ByVal ItemControl As Control, ByVal Item As DayViewItem, ByVal e As MouseEventArgs)
        RaiseEvent ItemMouseWheel(Me, New ItemMouseEventArgs(e, Item))
    End Sub
    ''' <summary>Raised after the event <see cref="ItemToolStrip.MouseWheel"/> ocures for an item.</summary>
    <KnownCategory(KnownCategoryAttribute.KnownCategories.Mouse)> _
    <Description("Raised after the event MouseWheel ocures for an item.")> _
    Public Event ItemMouseWheel As EventHandler(Of ItemMouseEventArgs)
#End Region
#End Region
    '''' <summary>Activates a child control. Optionally specifies the direction in the tab order to select the control from.</summary>
    '''' <param name="forward">true to move forward in the tab order; false to move backward in the tab order.</param>
    '''' <param name="directed">true to specify the direction of the control to select; otherwise, false.</param>
    'Protected Overrides Sub [Select](ByVal directed As Boolean, ByVal forward As Boolean)
    '    If Not Me.Enabled OrElse Not Me.Visible Then
    '        MyBase.Select(directed, forward)
    '    Else
    '        SelectNext(Nothing, iif(directed, forward, True))
    '    End If
    'End Sub
    'Private Enum enmOnEnd
    '    SelectNextControl
    '    DoNothing
    '    Cycle
    'End Enum
    '''' <summary>Vybere následující/předchozí položku k aktuálně vybrané položce</summary>
    '''' <param name="Current">Aktuálně vybraná položka (pokud je null bude vybráná první/poslední položka)</param>
    '''' <param name="Forward">Směr. True pro směr zleva a doprava zezhora dolů; false pro směr zprava doleva a zdola nahoru.</param>
    'Private Sub SelectNext(ByVal Current As ItemToolStrip, ByVal Forward As Boolean, Optional ByVal OnEnd As enmOnEnd = enmOnEnd.SelectNextControl)
    '    Dim RowI As Integer
    '    Dim NextItem As DayViewItem = Nothing
    '    Dim NextRow As Integer
    '    If Current Is Nothing Then
    '        RowI = iif(Forward, -1, Rows.Count)
    '    Else
    '        RowI = RowFromItem(Current.Tag)
    '        NextRow = RowI
    '        Dim Row As DayViewRow = Rows(RowI)
    '        Dim CurrentItem As DayViewItem = Current.Tag
    '        'Hledání v aktuálním řádku
    '        For Each Item As DayViewItem In Row.Records
    '            If iif(Forward, Item.Start > CurrentItem.Start, Item.Start < CurrentItem.Start) AndAlso GetItemToolStrip(RowI, Item) IsNot Nothing Then
    '                If NextItem Is Nothing OrElse iif(Forward, NextItem.Start > Item.Start, NextItem.Start < Item.Start) Then
    '                    NextItem = Item
    '                End If
    '            End If
    '        Next Item
    '    End If
    '    'Hledání ve všech řádcích
    '    If NextItem Is Nothing Then
    '        For NextRowIndex As Integer = RowI + iif(Forward, 1, -1) To iif(Forward, Rows.Count - 1, 0) Step iif(Forward, 1, -1)
    '            NextRow = NextRowIndex
    '            For Each NextRowItem As DayViewItem In Rows(NextRowIndex).Records
    '                If GetItemToolStrip(NextRow, NextRowItem) IsNot Nothing Then
    '                    If NextItem Is Nothing OrElse iif(Forward, NextRowItem.Start < NextItem.Start, NextRowItem.Start > NextItem.Start) Then
    '                        NextItem = NextRowItem
    '                    End If
    '                End If
    '            Next NextRowItem
    '            If NextItem IsNot Nothing Then Exit For
    '        Next NextRowIndex
    '    End If
    '    If NextItem IsNot Nothing Then
    '        GetItemToolStrip(NextRow, NextItem).Select()
    '    Else
    '        Select Case OnEnd
    '            Case enmOnEnd.Cycle
    '                SelectNext(Nothing, Forward, enmOnEnd.DoNothing)
    '            Case enmOnEnd.SelectNextControl
    '                If Not Me.FindForm Is Nothing Then
    '                    Me.FindForm.SelectNextControl(Me, Forward, True, True, True)
    '                End If
    '            Case Else 'Do nothing
    '        End Select
    '    End If
    'End Sub
#End Region
#Region "Delete"
    ''' <summary>Logika mazání položky uživatelem</summary>
    ''' <param name="item">Položka, která má být smazána (pokud je null použije se <see cref="ActiveItem"/>)</param>
    Private Sub UserDeleteItem(Optional ByVal item As DayViewItem = Nothing)
        If item Is Nothing Then item = ActiveItem
        If item.Locked Then Exit Sub
        Dim ce As New Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewItem).CancelableItemEventArgs(item, True)
        OnItemDeleting(ce)
        If Not ce.Cancel Then
            PrgEventsState = Suspension.DoNotRaise
            Dim row As Integer = RowFromItem(item)
            Dim ItemIndex As Integer = Rows(row).Records.IndexOf(item)
            Try
                Rows(row).Records.RemoveAt(ItemIndex)
            Finally
                PrgEventsState = Suspension.Enabled
            End Try
            OnItemRemovedByUser(New RowOperationEventArgs(Of ListWithEvents(Of DayViewItem).ItemIndexEventArgs)(row, New ListWithEvents(Of DayViewItem).ItemIndexEventArgs(item, ItemIndex)))
        Else
            If ce.CancelMessage = "" Then
                MsgBox("Položku nelze odstranit", MsgBoxStyle.Exclamation, "Položku nelze odstranit")
            Else
                MsgBox(ce.CancelMessage, MsgBoxStyle.Exclamation, "Položku nelze odstranit")
            End If
        End If
    End Sub

    ''' <summary>Uživatelské smazání položky <see cref="ActiveItem"/></summary>
    <DebuggerStepThrough()> Private Sub tmiDeleteItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tmiDeleteItem.Click
        UserDeleteItem()
    End Sub
    ''' <summary>Vyvolává událost <see cref="ItemDeleting"/></summary>
    ''' <param name="e">Parametry události</param>
    <DebuggerStepThrough()> Protected Overridable Sub OnItemDeleting(ByVal e As Tools.CollectionsT.GenericT.ListWithEvents(Of DayViewItem).CancelableItemEventArgs)
        RaiseEvent ItemDeleting(Me, e)
    End Sub
    ''' <summary>Nastává před smazáním položky uživatelem</summary>
    ''' <remarks>Nenastává před smazáním položky programově. Lze stornovat. Pokud je nsatavena <paramref name="e"/>.<see cref="ListWithEvents(Of DayViewItem).CancelableItemEventArgs.CancelMessage">CancelMessage</see> je zobrazena pomocí <see cref="MsgBox"/>, jinak je zobrazena obecná hláška.</remarks>
    <Category("Item Operations")> _
    <Description("Raised before item is deleted by user. Allows cancelation.")> _
    Public Event ItemDeleting As EventHandler(Of ListWithEvents(Of DayViewItem).CancelableItemEventArgs)
#End Region
#End Region
#End Region
End Class
