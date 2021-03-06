Imports System, System.Diagnostics, System.Collections.Generic, System.ComponentModel, System.Text, System.Web, System.Web.UI, System.Web.UI.WebControls, System.Security.Permissions
Imports System.Reflection, System.Drawing.Design, System.Runtime.InteropServices, System.Web.UI.Design

'Feel free to remove these statements if you don't like them.
'Corresponding statements at the end of the file and in Numberings.vb must be also removed.
#If AssemblyBuild <> 1 Then
'This statement wraps the Pager class with the DataPager namespace if it is built in the App_Code directory
'If it is built in separate project (with #Const AssemblyBuild = 1 defined) then only root namespace from projest's settings is used
Namespace DataPager
#End If

''' <summary>Pager for data sources</summary>
''' <remarks>This class enables paging of any data source that can take 2 parameters that defines begin and length or end of page and that can be asked for number of records tha it contains.</remarks>
<CLSCompliant(True)> _
<DefaultProperty("DataSourceID")> _
<DefaultEvent("CountSelecting")> _
<ToolboxData("<{0}:Pager runat=""server""/>")> _
<AspNetHostingPermission(SecurityAction.Demand, Level:=AspNetHostingPermissionLevel.Minimal)> _
<AspNetHostingPermission(SecurityAction.InheritanceDemand, Level:=AspNetHostingPermissionLevel.Minimal)> _
<ControlValueProperty("PageIndex", GetType(Integer), "1")> _
<Drawing.ToolboxBitmap(GetType(Pager), "Pager.bmp")> _
Public Class Pager
    Inherits CompositeControl
#Region "Enums" 'Enums used by Pager
    ''' <summary>Possible styles of navigation links for outer navigation buttons</summary>
    ''' <remarks>Used by the <see cref="PrevLastStyle"/> property to define which server control is used to render navigation buttons for first, previous, next and last page.</remarks>
    Public Enum enmBtnStyle
        ''' <summary><see cref="Button"/></summary>
        ''' <remarks>Navigation control is rendered using <see cref="Button"/> server control.</remarks>
        Button
        ''' <summary><see cref="LinkButton"/></summary>
        ''' <remarks>Navigation control is rendered using <see cref="LinkButton"/> server control.</remarks>
        LinkButton
        ''' <remarks>Navigation control is rendered using <see cref="ImageButton"/> server control.</remarks>
        ''' <summary><see cref="ImageButton"/></summary>
        ImageButton
    End Enum
    ''' <summary>Possible styles of navigation links for concrete pages</summary>
    ''' <remarks>Used by the <see cref="NumbersStyle"/> property to define which server control is used to render navigation buttons for concrete pages.</remarks>
    Public Enum enmPageStyle
        ''' <summary><see cref="Button"/></summary>
        ''' <remarks>
        ''' Navigation control is rendered using <see cref="Button"/> server control.
        ''' Value is equal to <see cref="enmBtnStyle.Button"/>.
        ''' </remarks>
        Button = enmBtnStyle.Button
        ''' <summary><see cref="LinkButton"/></summary>
        ''' <remarks>
        ''' Navigation control is rendered using <see cref="LinkButton"/> server control.
        ''' ''' Value is equal to <see cref="enmBtnStyle.LinkButton"/>.
        ''' </remarks>
        LinkButton = enmBtnStyle.LinkButton
    End Enum
    ''' <summary>Possible styles of pager</summary>
    ''' <remarks>Used by the <see cref="ManyNumbersMode"/> property to define how Pager behaves where there are more pages to be displayed.</remarks>
    Public Enum enmPageNumbersStyle
        ''' <summary>Pager will always display all pages</summary>
        ''' <remarks>This is not recommended to be used if source can contain many records divided into many pages because many numbers is rendered and responce becomes being big.</remarks>
        All
        ''' <summary>Only pages in so-called neighbourhood of current page are displayed</summary>
        ''' <remarks><seealso cref="NeighbourhoodSize"/></remarks>
        Neighbours
        ''' <summary>Pages in so-called neighbourhood are displayed and also first and last page is displayed</summary>
        ''' <remarks><seealso cref="NeighbourhoodSize"/></remarks>
        FirstNeighboursLast
        ''' <summary>Several pages from the beginning and from the end of pages range are dispayed and also pages in so-called neighbourhood are displayed.</summary>
        ''' <remarks><seealso cref="NeighbourhoodSize"/> <seealso cref="BeginEndSize"/></remarks>
        BeginNeighboursEnd
        ''' <summary>Only current page is displayed</summary>
        Current
    End Enum
#End Region

#Region "Simple properties" '"Simple" properties (with not complicated getters and setters)
#Region "Texts" 'Texts-related properties
    ''' <summary>Contains value of the <see cref="Text"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _Text As String = ""
    ''' <summary>Additional text displayed in front of page numbers</summary>
    ''' <remarks>
    ''' Default value is an empty string.
    ''' If value is an empty string no text is displayed and no HTML tag is rentered in place of it.
    ''' </remarks>
    <Bindable(True), Category("Appearance")> _
    <DefaultValue(""), Localizable(True)> _
    <Description("An additional text shown in pager")> _
    Public Overridable Property Text() As String
        <DebuggerHidden()> Get
            Return _Text
        End Get
        Set(ByVal Value As String)
            _Text = Value
            'If label that displays the text is already loaded change its text
            If lblText IsNot Nothing Then lblText.Text = Value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="PrevLastStyle"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _PrevLastStyle As enmBtnStyle = enmBtnStyle.Button
    ''' <summary>Defines style of outer navigation buttons (first, previous, next, last)</summary>
    ''' <remarks>Default value is <see cref="enmBtnStyle.Button"/>.</remarks>
    ''' <exception cref="ArgumentException">When <paramref name="value"/> is not member of <see cref="enmBtnStyle"/></exception>
    <Category("Appearance")> _
    <DefaultValue(GetType(enmBtnStyle), "Button"), Localizable(False)> _
    <Description("Style of First, Previous, Next and Last buttons")> _
    Public Property PrevLastStyle() As enmBtnStyle
        <DebuggerHidden()> Get
            Return _PrevLastStyle
        End Get
        Set(ByVal value As enmBtnStyle)
            If [Enum].GetName(GetType(enmBtnStyle), value) Is Nothing Then Throw New ArgumentException("Given value is not member of enmBtnStyle", "value")
            If _PrevLastStyle <> value Then
                _PrevLastStyle = value
                'If navigation controls are already loaded change them
                If OuterCreated AndAlso ShowPrevNext Then CreateOuter()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="PrevText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _PrevText As String = "<"
    ''' <summary>Text of the previous button</summary>
    ''' <remarks>
    ''' Default value is "&lt;".
    ''' If <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/> this defines <see cref="ImageButton.AlternateText"/>.
    ''' </remarks>
    <DefaultValue("<")> _
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("Text of button linking to previous page (if image is used then this property defines alternate text)")> _
    Public Property PrevText() As String
        <DebuggerHidden()> Get
            Return _PrevText
        End Get
        Set(ByVal value As String)
            _PrevText = value
            'If controls is already loaded change its property
            If btnPrev IsNot Nothing Then btnPrev.Text = value
            If lbtPrev IsNot Nothing Then lbtPrev.Text = value
            If imbPrev IsNot Nothing Then imbPrev.AlternateText = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NextText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NextText As String = ">"
    ''' <summary>Text of the next button</summary>
    ''' <remarks>
    ''' Default value is ">".
    ''' If <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/> this defines <see cref="ImageButton.AlternateText"/>.
    ''' </remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue(">")> _
    <Description("Text of button linking to next page (if image is used then this property defines alternate text)")> _
    Public Property NextText() As String
        <DebuggerHidden()> Get
            Return _NextText
        End Get
        Set(ByVal value As String)
            _NextText = value
            'If controls is already loaded change its property
            If btnNext IsNot Nothing Then btnNext.Text = value
            If lbtNext IsNot Nothing Then lbtNext.Text = value
            If imbNext IsNot Nothing Then imbNext.AlternateText = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="FirstText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _FirstText As String = "|<"
    ''' <summary>Text of the first button</summary>
    ''' <remarks>
    ''' Default value is "|&lt;".
    ''' If <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/> this defines <see cref="ImageButton.AlternateText"/>.
    ''' </remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("Text of button linking to first page (if image is used then this property defines alternate text)")> _
    <DefaultValue("|<")> _
    Public Property FirstText() As String
        <DebuggerHidden()> Get
            Return _FirstText
        End Get
        Set(ByVal value As String)
            _FirstText = value
            'If controls is already loaded change its property
            If btnFirst IsNot Nothing Then btnFirst.Text = value
            If lbtFirst IsNot Nothing Then lbtFirst.Text = value
            If imbFirst IsNot Nothing Then imbFirst.AlternateText = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="LastText"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _LastText As String = ">|"
    ''' <summary>Text of the last button</summary>
    ''' <remarks>
    ''' Default value is ">|".
    ''' If <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/> this defines <see cref="ImageButton.AlternateText"/>.
    ''' </remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue(">|")> _
    <Description("Text of button linking to last page (if image is used then this property defines alternate text)")> _
    Public Property LastText() As String
        <DebuggerHidden()> Get
            Return _LastText
        End Get
        Set(ByVal value As String)
            _LastText = value
            'If controls is already loaded change its property
            If btnLast IsNot Nothing Then btnLast.Text = value
            If lbtLast IsNot Nothing Then lbtLast.Text = value
            If imbLast IsNot Nothing Then imbLast.AlternateText = value
        End Set
    End Property
#End Region

#Region "Images" 'Images for ImmageButtons
    ''' <summary>Contains value of the <see cref="PrevImageURL"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _PrevImageURL As String = ""
    ''' <summary>Image for the previous button</summary>
    ''' <remarks>
    ''' Default vaue is an empty string. That causes that no image can be displayed by default.
    ''' This property accepts valid ASP.NET path (relative, ~-beginning, absolute).
    ''' This property is used only when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/>.
    ''' <seealso cref="PrevLastStyle"/>
    ''' </remarks>
    <DefaultValue("")> _
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("URL of image button linking to previous page")> _
    <Editor(GetType(ImageUrlEditor), GetType(UrlEditor))> _
    <UrlProperty()> _
    Public Property PrevImageURL() As String
        <DebuggerHidden()> Get
            Return _PrevImageURL
        End Get
        Set(ByVal value As String)
            _PrevImageURL = value
            'If ImageButton is already louded change its image
            If imbPrev IsNot Nothing Then imbPrev.ImageUrl = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NextImageURL"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NextImageURL As String = ""
    ''' <summary>Image for the next button</summary>
    ''' <remarks>
    ''' Default vaue is an empty string. That causes that no image can be displayed by default.
    ''' This property accepts valid ASP.NET path (relative, ~-beginning, absolute).
    ''' This property is used only when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/>.
    ''' <seealso cref="PrevLastStyle"/>
    ''' </remarks>
    <DefaultValue("")> _
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("URL of image button linking to next page")> _
    <Editor(GetType(ImageUrlEditor), GetType(UrlEditor))> _
    <UrlProperty()> _
    Public Property NextImageURL() As String
        <DebuggerHidden()> Get
            Return _NextImageURL
        End Get
        Set(ByVal value As String)
            _NextImageURL = value
            'If ImageButton is already louded change its image
            If imbNext IsNot Nothing Then imbNext.ImageUrl = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="FirstImageURL"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _FirstImageURL As String = ""
    ''' <summary>Image for the first button</summary>
    ''' <remarks>
    ''' Default vaue is an empty string. That causes that no image can be displayed by default.
    ''' This property accepts valid ASP.NET path (relative, ~-beginning, absolute).
    ''' This property is used only when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/>.
    ''' <seealso cref="PrevLastStyle"/>
    ''' </remarks>
    <DefaultValue("")> _
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("URL of image button linking to First page")> _
    <Editor(GetType(ImageUrlEditor), GetType(UrlEditor))> _
    <UrlProperty()> _
    Public Property FirstImageURL() As String
        <DebuggerHidden()> Get
            Return _FirstImageURL
        End Get
        Set(ByVal value As String)
                _FirstImageURL = value
            'If ImageButton is already louded change its image
            If imbFirst IsNot Nothing Then imbFirst.ImageUrl = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="LastImageURL"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _LastImageURL As String = ""
    ''' <summary>Image for the last button</summary>
    ''' <remarks>
    ''' Default vaue is an empty string. That causes that no image can be displayed by default.
    ''' This property accepts valid ASP.NET path (relative, ~-beginning, absolute).
    ''' This property is used only when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/>.
    ''' <seealso cref="PrevLastStyle"/>
    ''' </remarks>
    <DefaultValue("")> _
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("URL of image button linking to Last page")> _
    <Editor(GetType(ImageUrlEditor), GetType(UrlEditor))> _
    <UrlProperty()> _
    Public Property LastImageURL() As String
        <DebuggerHidden()> Get
            Return _LastImageURL
        End Get
        Set(ByVal value As String)
            _LastImageURL = value
            'If ImageButton is already louded change its image
            If imbLast IsNot Nothing Then imbLast.ImageUrl = value
        End Set
    End Property
#End Region

#Region "ToolTips" 'Tool tips for controls
    ''' <summary>Contains value of the <see cref="PrevToolTip"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _PrevToolTip As String = ""
    ''' <summary>Tooltip for the previous button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("Tool tip of button linking to previous page")> _
    <DefaultValue("")> _
    Public Property PrevToolTip() As String
        <DebuggerHidden()> Get
            Return _PrevToolTip
        End Get
        Set(ByVal value As String)
            _PrevToolTip = value
            'If the control is already created change its tooltip
            If btnPrev IsNot Nothing Then btnPrev.ToolTip = value
            If lbtPrev IsNot Nothing Then lbtPrev.ToolTip = value
            If imbPrev IsNot Nothing Then imbPrev.ToolTip = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NextToolTip"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NextToolTip As String = ""
    ''' <summary>Tooltip for the next button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue("")> _
    <Description("Tool tip of button linking to next page")> _
    Public Property NextToolTip() As String
        <DebuggerHidden()> Get
            Return _NextToolTip
        End Get
        Set(ByVal value As String)
            _NextToolTip = value
            'If the control is already created change its tooltip
            If btnNext IsNot Nothing Then btnNext.ToolTip = value
            If lbtNext IsNot Nothing Then lbtNext.ToolTip = value
            If imbNext IsNot Nothing Then imbNext.ToolTip = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="LastToolTip"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _LastToolTip As String = ""
    ''' <summary>Tooltip for the last button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue("")> _
    <Description("Tool tip of button linking to lastpage")> _
    Public Property LastToolTip() As String
        <DebuggerHidden()> Get
            Return _LastToolTip
        End Get
        Set(ByVal value As String)
            _LastToolTip = value
            'If the control is already created change its tooltip
            If btnLast IsNot Nothing Then btnLast.ToolTip = value
            If lbtLast IsNot Nothing Then lbtLast.ToolTip = value
            If imbLast IsNot Nothing Then imbLast.ToolTip = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="FirstToolTip"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _FirstToolTip As String = ""
    ''' <summary>Tooltip for the previous button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue("")> _
    <Description("Tool tip of button linking to firts page")> _
    Public Property FirstToolTip() As String
        <DebuggerHidden()> Get
            Return _FirstToolTip
        End Get
        Set(ByVal value As String)
            _FirstToolTip = value
            'If the control is already created change its tooltip
            If btnFirst IsNot Nothing Then btnFirst.ToolTip = value
            If lbtFirst IsNot Nothing Then lbtFirst.ToolTip = value
            If imbFirst IsNot Nothing Then imbFirst.ToolTip = value
        End Set
    End Property
#End Region

    ''' <summary>Contains value of the <see cref="NumbersStyle"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NumbersStyle As enmPageStyle = enmPageStyle.LinkButton
    ''' <summary>Style of buttons of concrete pages</summary>
    ''' <remarks>
    ''' Default value if <see cref="enmPageStyle.LinkButton"/>.
    ''' Determines the style of buttons of concrete pages.
    ''' </remarks>
    ''' <exception cref="ArgumentException">If <paramref name="value"/> is not member of <see cref="enmPageStyle"/></exception>
    <Category("Appearance")> _
    <DefaultValue(GetType(enmPageStyle), "LinkButton"), Localizable(False)> _
    <Description("Style of page number buttons")> _
    Public Property NumbersStyle() As enmPageStyle
        <DebuggerHidden()> Get
            Return _NumbersStyle
        End Get
        Set(ByVal value As enmPageStyle)
            If [Enum].GetName(GetType(enmPageStyle), value) Is Nothing Then Throw New ArgumentException("Given value is not member of enmPageStyle")
            If _NumbersStyle <> value Then
                _NumbersStyle = value
                'If buttons are already cretated must be recreated
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="DisableNonsenses"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _DisbalNonsenses As Boolean = True
    ''' <summary>Deactivates navigation buttons that dont change <see cref="PageIndex"/></summary>
    ''' <remarks>
    ''' Default value is True.
    ''' If set to True then navigation buttons clicking on which doesn't change <see cref="PageIndex"/> are disabled.
    ''' This means if <see cref="PageIndex"/> is 1 than the first and the previous buttons are disabled and if <see cref="PageIndex"/> is <see cref="PagesTotal"/> then the next and the last buttons are disabled.
    ''' </remarks>
    <Category("Behavior")> _
    <DefaultValue(True), Localizable(False)> _
    <Description("Defines if disable controls navigating to first and previous page when first page is selected and controls navigating to last and next page when last page is selected or not.")> _
    Public Property DisableNonsenses() As Boolean
        <DebuggerHidden()> Get
            Return _DisbalNonsenses
        End Get
        Set(ByVal value As Boolean)
            If _DisbalNonsenses <> value Then
                _DisbalNonsenses = value
                'If buttons are already created then recreate them
                If OuterCreated Then CreateOuter()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the<see cref="ShowPrevNext"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _ShowPrevNext As Boolean = True
    ''' <summary>Determines if the previous and the last buttons are displayed</summary>
    <Category("Appearance")> _
    <DefaultValue(True)> _
    <Localizable(False)> _
    <Description("Gets or sets if Prevous and Next buttons are shown")> _
    Public Property ShowPrevNext() As Boolean
        <DebuggerHidden()> Get
            Return _ShowPrevNext
        End Get
        Set(ByVal value As Boolean)
            If _ShowPrevNext <> value Then
                _ShowPrevNext = value
                'If controls are already created then update them
                If OuterCreated Then CreateOuter()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="ShowFirstLast"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _ShowFirstLast As Boolean = True
    ''' <summary>Determines if the first and the last buttons are displayed</summary>
    <Category("Appearance")> _
    <DefaultValue(True)> _
    <Localizable(False)> _
    <Description("Gets or sets if First and Last buttons are shown")> _
    Public Property ShowFirstLast() As Boolean
        <DebuggerHidden()> Get
            Return _ShowFirstLast
        End Get
        Set(ByVal value As Boolean)
            If _ShowFirstLast <> value Then
                _ShowFirstLast = value
                'If controls are already created then update them
                If OuterCreated Then CreateOuter()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="ShowPages"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _ShowPages As Boolean = True
    ''' <summary>Determines if buttons for cocrete pages are displayed</summary>
    <Category("Appearance")> _
    <DefaultValue(True)> _
    <Localizable(False)> _
    <Description("Gets or sets if individual page buttons are shown")> _
    Public Property ShowPages() As Boolean
        <DebuggerHidden()> Get
            Return _ShowPages
        End Get
        Set(ByVal value As Boolean)
            If _ShowPages <> value Then
                _ShowPages = value
                'If concrete pages' buttons' generating procedure has already ran then rerun it
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="ManyNumbersMode"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _ManyNumbersMode As enmPageNumbersStyle = enmPageNumbersStyle.BeginNeighboursEnd
    ''' <summary>Defines behaviour of pages' numbers when there are more pages</summary>
    ''' <remarks>
    ''' Default value is <see cref="enmPageNumbersStyle.BeginNeighboursEnd"/>.
    ''' This defines which page numbers are displayed. This takes effect only when <see cref="ShowPages"/> is True.
    ''' </remarks>
    ''' <exception cref="ArgumentException">When <paramref name="value"/> is not member of<see cref="enmPageNumbersStyle"/></exception>
    <Category("Appearance")> _
    <Localizable(False)> _
    <DefaultValue(GetType(enmPageNumbersStyle), "BeginNeighboursEnd")> _
    <Description("Defines the behavoir of page numbers when there are many pages")> _
    Public Property ManyNumbersMode() As enmPageNumbersStyle
        <DebuggerHidden()> Get
            Return _ManyNumbersMode
        End Get
        Set(ByVal value As enmPageNumbersStyle)
            If [Enum].GetName(GetType(enmPageNumbersStyle), value) Is Nothing Then Throw New ArgumentException("Value is not member of enmPageNumbersStyle")
            If _ManyNumbersMode <> value Then
                _ManyNumbersMode = value
                'If pages are already created then recterate tehm
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NeighbourhoodSize"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NeighbourhoodSize As UShort = 10
    ''' <summary>Defines the size of so-called neighbourhood of current page</summary>
    ''' <remarks>
    ''' <para>Default value is 10.</para>
    ''' <para>
    ''' <see cref="Pager"/> displays <see cref="NeighbourhoodSize"/> pages that surronds current page (current page is not counted into this number; current page is defined by the <see cref="PageIndex"/> property) - if it is possible. The neighbourhood can overlap with begin and/or end area. If <see cref="PageIndex"/> is as near to 1 or to <see cref="PagesTotal"/> as the neighbourhood "runs of" the page range neighbourhood is shifted left or right in order to display as much page numbers as possible.
    ''' If no neighbourhood-shifting takes effect same number of page numbers on the left side nd on the right side of the current page is displayed. This if the reason why only even values reasonable. Odd values are rounded down to the nearest even value. So, 3 has the same effect as 2 and 1 has the same effect as 0 (which is not allowed, but can be simulated through setting this property to 1).
    ''' Similar effect as setting <see cref="NeighbourhoodSize"/> to 1 is setting <see cref="ManyNumbersMode"/> to <see cref=" enmPageNumbersStyle.Current"/>.
    ''' </para>
    ''' <para>This property has no effect when <see cref="ShowPages"/> is False or when <see cref="ManyNumbersMode"/> is <see cref="enmPageNumbersStyle.All"/> or <see cref="enmPageNumbersStyle.Current"/>.</para>
    ''' <seealso cref="ManyNumbersMode"/> <seealso cref="ShowPages"/> <seealso cref="BeginEndSize"/>
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">If <paramref name="value"/> is less than or equal to zero</exception>
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("Defines the number of pages tha belongs to so-called neighbourhood of current page (excluding current page). Maximally NeighbourhoodSize/2 pages are shown on each side of current page if there is enough number of pages on both sides (including pages that possibly belongs to begin or end), othervise the neighbourhood is shifted left or right in order to show as many pages as possble (NeighbourhoodSize maximally)")> _
    <DefaultValue(10I)> _
    Public Property NeighbourhoodSize() As Integer
        <DebuggerHidden()> Get
            Return _NeighbourhoodSize
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then Throw New ArgumentOutOfRangeException("value", "NeighbourhoodSize must be greater then zero")
            If _NeighbourhoodSize <> value Then
                _NeighbourhoodSize = value
                'If page numbers have been already created then recreate them
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="BeginEndSize"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _BeginEndSize As UShort = 10
    ''' <summary>Defines the size of the ben area and of the end area.</summary>
    ''' <remarks>
    ''' <para>Default value is 10.</para>
    ''' <para>Defines how many page numbers (maximally) are displayed at the beginning of number line and at the end of number line. Note that so-called neighbourhood and begin and end area can overlap.</para>
    ''' <para>This property takes effect only when <see cref="ShowPages"/> is True and <see cref="ManyNumbersMode"/> is <see cref="enmPageNumbersStyle.BeginNeighboursEnd"/>.</para>
    ''' <seealso cref="NeighbourhoodSize"/> <seealso cref="ShowPages"/> <seealso cref="ManyNumbersMode"/>
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">If <paramref name="value"/> is less than or equal to than zero</exception>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue(10I)> _
    <Description("Defines the size of the beginning and the end area - the number of pages shown at the beginning and ath the end of sequence")> _
    Public Property BeginEndSize() As Integer
        <DebuggerHidden()> Get
            Return _BeginEndSize
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then Throw New ArgumentOutOfRangeException("value", "BeginEndSize must be over zero")
            If _BeginEndSize <> value Then
                _BeginEndSize = value
                'If page numbers have been already created then recreate them
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NumberingType"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NumberingType As enmNumberingTypes = enmNumberingTypes.Numbers1
    ''' <summary>Defines which numbering system and which numbers is used for displaying page numbers.</summary>
    ''' <remarks>
    ''' Default value is <see cref="enmNumberingTypes.Numbers1"/>.
    ''' This doesn't affect <see cref="PageIndex"/> numbering system.
    ''' </remarks>
    ''' <exception cref="ArgumentException">When <paramref name="value"/> is not meber of <see cref="enmNumberingTypes"/></exception>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue(GetType(enmNumberingTypes), "Numbers1")> _
    <Description("Defines which numbers and numbering system is used for numbering pages")> _
    Public Property NumberingType() As enmNumberingTypes
        <DebuggerHidden()> Get
            Return _NumberingType
        End Get
        Set(ByVal value As enmNumberingTypes)
            If [Enum].GetName(GetType(enmNumberingTypes), value) Is Nothing Then Throw New ArgumentException("Given value is not member of enmNumberingTypes")
            If _NumberingType <> value Then
                _NumberingType = value
                'If page numbers have been already created then recreate them
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NumberPrefix"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NumberPrefix As String = ""
    ''' <summary>Text displayed in front of each page number</summary>
    ''' <remarks>
    ''' Default value is an empty string.
    ''' This defines string prefix of number of page.
    ''' </remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue("")> _
    <Description("Text printed before page number")> _
    Public Property NumberPrefix() As String
        <DebuggerHidden()> Get
            Return _NumberPrefix
        End Get
        Set(ByVal value As String)
            If _NumberPrefix <> value Then
                _NumberPrefix = value
                'If page numbers have been already created then recreate them
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NumberSufix"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NumberSufix As String = ""
    ''' <summary>Text displayed after each number</summary>
    ''' <remarks>
    ''' Default value is an empty string.
    ''' This defines string sufix of number of page.
    ''' </remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue("")> _
    <Description("Text printed after  page number")> _
    Public Property NumberSufix() As String
        <DebuggerHidden()> Get
            Return _NumberSufix
        End Get
        Set(ByVal value As String)
            If _NumberSufix <> value Then
                _NumberSufix = value
                'If page numbers have been already created then recreate them
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="NumberSeparator"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _NumberSeparator As String = " "
    ''' <summary>Text displayed as separator between each two numbers</summary>
    ''' <remarks>
    ''' Default value is " " (space).
    ''' This seperator is displayet between each two numbers (including number of current page). It is not displayed between number and missing numbers indicator and between missing numbers indicator and number.
    ''' <seealso cref="PageIndex"/> <seealso cref="MissingNumbersIndicator"/>
    ''' </remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <DefaultValue(" ")> _
    <Description("Text printed between page numbers")> _
    Public Property NumberSeparator() As String
        <DebuggerHidden()> Get
            Return _NumberSeparator
        End Get
        Set(ByVal value As String)
            If _NumberSeparator <> value Then
                _NumberSeparator = value
                'If page numbers have been already created then recreate them
                If PagesCreated Then CreatePages()
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="MissingNumbersIndicator"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _MissingNumbersIndicator As String = " ... "
    ''' <summary>Text displayed in place where some page numbers are missing</summary>
    ''' <remarks>
    ''' Default value is " ... " (space, three dots and space).
    ''' This text is displaye between begin region and neighbourhood (or curent page) and between neighbourhood (or current page) and end region.
    ''' It is displayed also when begin and end regions are not displayed. It is not displayed when in place where neighbouring regions are overlaping or where there is no space between neighbouring regions.
    ''' </remarks>
    <Category("Appearance")> _
    <Localizable(True)> _
    <Description("Text printed in place where sume numbers are missing")> _
    <DefaultValue(" ... ")> _
    Public Property MissingNumbersIndicator() As String
        <DebuggerHidden()> Get
            Return _MissingNumbersIndicator
        End Get
        Set(ByVal value As String)
            If _MissingNumbersIndicator <> value Then
                _MissingNumbersIndicator = value
                'If page numbers are already created then change missing placeholders texts
                If PagesCreated AndAlso Missings IsNot Nothing Then For Each msng As Label In Missings : msng.Text = value : Next msng
            End If
        End Set
    End Property

#Region "CSS" 'CSS classes
    ''' <summary>Contains value of the <see cref="CSSClassPrev"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _CSSClassPrev As String = ""
    ''' <summary>Defines CSS class associated with the previous button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <DefaultValue("")> _
    <Category("Style")> _
    <Localizable(True)> _
    <Description("CSS class of the previous button")> _
    Public Property CSSClassPrev() As String
        <DebuggerHidden()> Get
            Return _CSSClassPrev
        End Get
        Set(ByVal value As String)
            _CSSClassPrev = value
            'If control has been already created then change its CssClass property
            If btnPrev IsNot Nothing Then btnPrev.CssClass = value
            If lbtPrev IsNot Nothing Then lbtPrev.CssClass = value
            If imbPrev IsNot Nothing Then imbPrev.CssClass = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="CSSClassLast"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _CSSClassLast As String = ""
    ''' <summary>Defines CSS class associated with the last button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <DefaultValue("")> _
    <Category("Style")> _
    <Localizable(True)> _
    <Description("CSS class of the Last button")> _
    Public Property CSSClassLast() As String
        <DebuggerHidden()> Get
            Return _CSSClassLast
        End Get
        Set(ByVal value As String)
            _CSSClassLast = value
            'If control has been already created then change its CssClass property
            If btnLast IsNot Nothing Then btnLast.CssClass = value
            If lbtLast IsNot Nothing Then lbtLast.CssClass = value
            If imbLast IsNot Nothing Then imbLast.CssClass = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="CSSClassFirst"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _CSSClassFirst As String = ""
    ''' <summary>Defines CSS class associated with the first button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <DefaultValue("")> _
    <Category("Style")> _
    <Localizable(True)> _
    <Description("CSS class of the First button")> _
    Public Property CSSClassFirst() As String
        <DebuggerHidden()> Get
            Return _CSSClassFirst
        End Get
        Set(ByVal value As String)
            _CSSClassFirst = value
            'If control has been already created then change its CssClass property
            If btnFirst IsNot Nothing Then btnFirst.CssClass = value
            If lbtFirst IsNot Nothing Then lbtFirst.CssClass = value
            If imbFirst IsNot Nothing Then imbFirst.CssClass = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="CSSClassNext"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _CSSClassNext As String = ""
    ''' <summary>Defines CSS class associated with the next button</summary>
    ''' <remarks>Default value is an empty string.</remarks>
    <DefaultValue("")> _
    <Category("Style")> _
    <Localizable(True)> _
    <Description("CSS class of the Next button")> _
    Public Property CSSClassNext() As String
        <DebuggerHidden()> Get
            Return _CSSClassNext
        End Get
        Set(ByVal value As String)
            _CSSClassNext = value
            'If control has been already created then change its CssClass property
            If btnNext IsNot Nothing Then btnNext.CssClass = value
            If lbtNext IsNot Nothing Then lbtNext.CssClass = value
            If imbNext IsNot Nothing Then imbNext.CssClass = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="CSSClassNumberBlock"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _CSSClassNumberBlock As String = ""
    ''' <summary>Defines the CSS class associated with the page numbers block</summary>
    ''' <remarks>
    ''' Default value is an empty string.
    ''' Page numbers are surronded with the &lt;span> element. This defines the class atributte ot this &lt;span>.
    ''' </remarks>
    <DefaultValue("")> _
    <Category("Style")> _
    <Localizable(True)> _
    <Description("CSS class of the <span> thet surronds page buttons")> _
    Public Property CSSClassNumberBlock() As String
        <DebuggerHidden()> Get
            Return _CSSClassNumberBlock
        End Get
        Set(ByVal value As String)
            If _CSSClassNumberBlock <> value Then
                _CSSClassNumberBlock = value
            End If
        End Set
    End Property

    ''' <summary>Contains value fo the <see cref="CSSClassCurrent"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _CSSClassCurrent As String = ""
    ''' <summary>Defines the CSS class associated with the number of current page</summary>
    ''' <remarks>
    ''' Default value is an empty string.
    ''' This class is aplyed to control that represent number of current page (defines by <see cref="PageIndex"/>).
    ''' </remarks>
    <DefaultValue("")> _
    <Category("Style")> _
    <Localizable(True)> _
    <Description("CSS class of the current page button")> _
    Public Property CSSClassCurrent() As String
        <DebuggerHidden()> Get
            Return _CSSClassCurrent
        End Get
        Set(ByVal value As String)
            If _CSSClassCurrent <> value Then
                _CSSClassCurrent = value
                If PagesCreated AndAlso ActualPageMarker IsNot Nothing Then
                    'If actual pagl has been already created then change its CssClass
                    ActualPageMarker.CssClass = value
                End If
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="CSSClassMissing"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _CSSClassMissing As String = ""
    ''' <summary>Defines the CSS class asociated with missing numbers marker</summary>
    ''' <remarks>
    ''' Default value is an empty string.
    ''' This CSS class is applyed on control indicating than in place where is is placed are missing some page numbers.
    ''' <seealso cref="MissingNumbersIndicator"/>
    ''' </remarks>
    <DefaultValue("")> _
    <Category("Style")> _
    <Localizable(True)> _
    <Description("CSS class missing pagenumbers indicator (...)")> _
    Public Property CSSClassMissing() As String
        <DebuggerHidden()> Get
            Return _CSSClassMissing
        End Get
        Set(ByVal value As String)
            If _CSSClassMissing <> value Then
                _CSSClassMissing = value
                If PagesCreated AndAlso Missings IsNot Nothing Then
                    'If missing numbers placeholder hav been already created than recreate them
                    For Each wc As WebControl In Missings
                        wc.CssClass = value
                    Next wc
                End If
            End If
        End Set
    End Property
#End Region
#Region "Controls visibility" 'Read-only properties that defines if particular control is visible
    ''' <summary>Defines if the first and the last <see cref="Button"/> controls are displayed</summary>
    ''' <remarks>This is True when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.Button"/> and <see cref="ShowFirstLast"/> is True.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property FirstLastButtonShown() As Boolean
        Get
            Return PrevLastStyle = enmBtnStyle.Button AndAlso Me.ShowFirstLast
        End Get
    End Property
    ''' <summary>Defines if the first and the last <see cref="LinkButton"/> controls are displayed</summary>
    ''' <remarks>This is True when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.LinkButton"/> and <see cref="ShowFirstLast"/> is True.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property FirstLastLinkButtonShown() As Boolean
        Get
            Return PrevLastStyle = enmBtnStyle.LinkButton AndAlso Me.ShowFirstLast
        End Get
    End Property
    ''' <summary>Defines if the first and the last <see cref="ImageButton"/> controls are displayed</summary>
    ''' <remarks>This is True when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/> and <see cref="ShowFirstLast"/> is True.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property FirstLastImageShown() As Boolean
        Get
            Return PrevLastStyle = enmBtnStyle.ImageButton AndAlso Me.ShowFirstLast
        End Get
    End Property
    ''' <summary>Defines if the previous and the next <see cref="Button"/> controls are displayed</summary>
    ''' <remarks>This is True when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.Button"/> and <see cref="ShowPrevNext"/> is True.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property PrevNextButtonShown() As Boolean
        Get
            Return PrevLastStyle = enmBtnStyle.Button AndAlso Me.ShowPrevNext
        End Get
    End Property
    ''' <summary>Defines if the previous and the next <see cref="LinkButton"/> controls are displayed</summary>
    ''' <remarks>This is True when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.LinkButton"/> and <see cref="ShowPrevNext"/> is True.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property PrevNextLinkButtonShown() As Boolean
        Get
            Return PrevLastStyle = enmBtnStyle.LinkButton AndAlso Me.ShowPrevNext
        End Get
    End Property
    ''' <summary>Defines if the previous and the next <see cref="ImageButton"/> controls are displayed</summary>
    ''' <remarks>This is True when <see cref="PrevLastStyle"/> is <see cref="enmBtnStyle.ImageButton"/> and <see cref="ShowPrevNext"/> is True.</remarks>
    <Browsable(False)> _
    Public ReadOnly Property PrevNextImageShown() As Boolean
        Get
            Return PrevLastStyle = enmBtnStyle.ImageButton AndAlso Me.ShowPrevNext
        End Get
    End Property
#End Region
#End Region

#Region "Data properties" 'Propertie associated with data
    ''' <summary>Contains value of the<see cref="EndParameterIsLength"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _EndParameterIsLength As Boolean
    ''' <summary>Defines if the <see cref="EndParameterValue"/> should be the end (False) of page or lenght (True) of page.</summary>
    ''' <remarks>
    ''' Default value is False.
    ''' If set to False than data source being paged should accept two parameters - index of first item on page and index of last item on page.
    ''' If set to True than data source being paged should accept two parameters - index of first item on page and number of items on page.
    ''' This affects only the <see cref="EndParameterValue"/> property.
    ''' </remarks>
    <Category("Data"), DefaultValue(False)> _
    <Localizable(False)> _
    <Description("Determines if the EndParameterName parameter of DataSource points to the end of page (False) or to the length of the page (True)")> _
    Public Property EndParameterIsLength() As Boolean
        <DebuggerHidden()> Get
            Return _EndParameterIsLength
        End Get
        Set(ByVal value As Boolean)
            _EndParameterIsLength = value
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="PageSize"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _PageSize As UShort = 1US
    ''' <summary>Number of records per page</summary>
    ''' <remarks>
    ''' Default value is 1.
    ''' This defines how many records is displayed on one page. No that in case there is not enough number of records in the data source or last page is displayed and number of records in the data source modulo <see cref="PageSize"/> is not zero than number of records actually displayed can be less then value of this property.
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="value"/> is less than or equal to zero</exception>
    <Category("Behavior"), Localizable(True)> _
    <Description("Defines maximal number of records per page")> _
    <DefaultValue(1I)> _
    Public Property PageSize() As Integer
        <DebuggerHidden()> Get
            Return _PageSize
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then Throw New ArgumentOutOfRangeException("value", "PageSize must be greater than zero")
            If _PageSize <> value Then
                _PageSize = value
                'If controls have been already created then recreate them
                If OuterCreated OrElse PagesCreated Then CreateChildControls()
            End If
        End Set
    End Property

    ''' <summary>Index of current page</summary>
    ''' <value>Value must be in the &lt;1;<see cref="PagesTotal"/>> interval</value>
    ''' <returns>Index of page currently displayed. If it is impossible to display any pages (number of records in the data source is 0) than return 0.</returns>
    ''' <remarks>
    ''' <para>Index is 1-based</para>
    ''' <para>If there are no data i the data source setting this property will always fail.</para>
    ''' <para>This is the only property of <see cref="Pager"/> that is stored in ViewState</para>
    ''' <seealso cref="ViewState"/>
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="value"/> is less than or equal to zero or <paramref name="value"/> is greater than <see cref="PagesTotal"/></exception>
    <Category("Data"), Localizable(False)> _
    <Description("Gets or sets index of current page")> _
    <DefaultValue(1I)> _
    <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    <Browsable(False), Bindable(True)> _
    Public Property PageIndex() As Integer
        <DebuggerHidden()> Get
            If PagesTotal = 0 Then Return 0
            Dim s As Object = ViewState("PageIndex")
            If s Is Nothing Then
                Return 1UI
            Else
                Return s
            End If
        End Get
        Set(ByVal value As Integer)
            If value <= 0 OrElse value > PagesTotal Then Throw New ArgumentException("PageIndex must be greater than zero and smaller or equal to PagesTotal")
            If CInt(ViewState("PageIndex")) <> value Then
                ViewState("PageIndex") = value
                'If controls have been already created then recreate them
                If PagesCreated OrElse OuterCreated Then CreateChildControls()
            End If
        End Set
    End Property

    ''' <summary>Number of current page in selected numbering system</summary>
    ''' <returns>Gets the number of current page (defined by <see cref="PageIndex"/>) converted into numbering system defined by <see cref="NumberingType"/>.</returns>
    ''' <remarks>This is the string representaion of number of current page in current numbering system.</remarks>
    ''' <exception cref="InvalidOperationException">When <see cref="PageIndex"/> or <see cref="PagesTotal"/> is zero (which means the there are no data in the data source)</exception>
    <Category("Data"), Description("Gets number of current page reflecting numbering system chosen")> _
    <Browsable(False)> _
    Public ReadOnly Property PageNumber() As String
        <DebuggerHidden()> Get
            If PageIndex <= 0 Then Throw New InvalidOperationException("PageNumberFromIndex cannot be obtainde when PageIndex less than or equal to 0")
            Return PageNumberFromIndex(PageIndex)
        End Get
    End Property

    ''' <summary>Converts page index into number in selected numbering system</summary>
    ''' <param name="Index">Page index to convert</param>
    ''' <returns>Page number converted to current numbering system and involved by <see cref="PageIndex"/> (current page) and <see cref="PagesTotal"/> (if such involving is applicable for numbering system currently selected).</returns>
    ''' <remarks>
    ''' The result of this function depends on <see cref="NumberingType"/>, <see cref="PageIndex"/> and <see cref="PagesTotal"/>.
    ''' <paramref name="Index"/> is 1-based.
    ''' Internaly uses overloaded <see cref="PageNumberFromIndex"/>.
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="Index"/> is less than or equal to 0</exception>
    ''' <exception cref="InvalidOperationException">When <see cref="PageIndex"/> or <see cref="PagesTotal"/> is zero (which means the there are no data in the data source)</exception>
    Public Function PageNumberFromIndex(ByVal Index As Integer) As String
        If Index <= 0 Then
            Throw New ArgumentOutOfRangeException("Index", "Index must be greater than zero")
        ElseIf PagesTotal <= 0 Then
            Throw New InvalidOperationException("PageNumberFromIndex cannot be obtainde when PagesTotal less than or equal to 0")
        ElseIf PageIndex <= 0 Then
            Throw New InvalidOperationException("PageNumberFromIndex cannot be obtainde when PageIndex less than or equal to 0")
        End If
        Return PageNumberFromIndex(Index, PageIndex, PagesTotal, NumberingType)
    End Function

    ''' <summary>Convers index of page into it's number in given numbering system involved by given count of pages and given actual page</summary>
    ''' <param name="Index">Index to convert</param>
    ''' <param name="Current">Index of current page</param>
    ''' <param name="Count">Number of pages in source</param>
    ''' <param name="Mode">Numbering system</param>
    ''' <returns>String representaion of page index in given numbering system involved by given current page and number of pages.</returns>
    ''' <remarks>
    ''' Indexes (<paramref name="Current"/> and <paramref name="Index"/>) are 1-based.
    ''' <seealso cref="NumericSystem"/>
    ''' </remarks>
    Public Shared Function PageNumberFromIndex(ByVal Index As Integer, ByVal Current As Integer, ByVal Count As Integer, ByVal Mode As enmNumberingTypes) As String
        Dim NSystem As NumericSystem = NumericSystem.Create(Mode)
        Return NSystem.Number(Index, Current, Count)
    End Function

    ''' <summary>Index of first record displayed on current page</summary>
    ''' <returns>Index of first record displayed on current page defined by <see cref="PageIndex"/></returns>
    ''' <remarks>
    ''' Returned index is 1-based.
    ''' <seealso cref="NumberOfFirstRecordOnSpecificPage"/>
    ''' </remarks>
    ''' <exception cref="InvalidOperationException">When <see cref="PageIndex"/> is 0 (which means that there are no data in the data source)</exception>
    <Category("Data"), Description("Gets number of first record on page (1-based)")> _
    <Browsable(False)> _
    Public ReadOnly Property NumberOfFirstRecordOnPage() As Integer
        Get
            Try
                    Return NumberOfFirstRecordOnSpecificPage(PageIndex)
                Catch ex As ArgumentOutOfRangeException
                    If PageIndex = 0 Then
                        Throw New InvalidOperationException("Number of first record on page cannot be computed when PageIndex is 0", ex)
                    Else
                        Throw ex
                    End If
            End Try
        End Get
    End Property

    ''' <summary>Index of first record displayed on specific page</summary>
    ''' <param name="PageIndex">Number of page which's first index will be returned</param>
    ''' <returns>Number of first record displayed on page which's index is specified by <paramref name="PageIndex"/></returns>
    ''' <remarks>
    ''' Return value as well as <paramref name="PageIndex"/> is 1-based.
    ''' <seealso cref="NumberOfFirstRecordOnPage"/>
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="PageIndex"/> is less than or equal to zero</exception>
    Public Function NumberOfFirstRecordOnSpecificPage(ByVal PageIndex As Integer) As Integer
        If PageIndex <= 0 Then Throw New ArgumentOutOfRangeException("PageIndex", "page index must be greater than zero")
        Return PageSize * (PageIndex - 1) + 1
    End Function

    ''' <summary>Number of last record on current page (always even if <see cref="EndParameterIsLength"/> is True)</summary>
    ''' <remarks>
    ''' WARNING!!! It is possible that this number is greater than number of records in the data source (and because of it also greater than actual last number). This can happen when there are less records on the last page than is <see cref="PageSize"/>.
    ''' Chack the <see cref="TrueNumberOfLastRecorOnPage"/> property for actual last record number on page.
    ''' Value affected by <see cref="EndParameterIsLength"/> is returned by the <see cref="EndParameterValue"/> property.
    ''' </remarks>
    ''' <exception cref="InvalidOperationException">
    ''' When <see cref="PageIndex"/> is 0 (which means that there are no data in the data source).
    ''' Thrown by <see cref="NumberOfFirstRecordOnPage"/>.
    ''' </exception>
    <Category("Data"), Description("Gets number of last record on page (always even when EndParameterIsLength = True.")> _
    <Browsable(False)> _
    Public ReadOnly Property NumberOfLastRecorOnPage() As Integer
        Get
            Return NumberOfFirstRecordOnPage + PageSize - 1
        End Get
    End Property

#Region "Select Count" 'Detecting number of records in source
    ''' <summary>Contains value of the <see cref="RecordsTotal"/> property</summary>
    Private _RecordsTotal As UInteger
    ''' <summary>Number of records in the source</summary>
    ''' <remarks>
    ''' This property caches result passed to the <see cref="CountSelecting"/> event.
    ''' The Set accessor is not Public, it is Protected.
    ''' In design time this property returns value of <see cref="DesignTimeRecordsTotal"/>.
    ''' The set accessor sets <see cref="SelectCountDone"/> to True.
    ''' <seealso cref="DesignMode"/> <seealso cref="DesignTimeRecordsTotal"/> <seealso cref="OnCountSelecting"/> <seealso cref="CountSelecting"/> <seealso cref="SelectCountDone"/>
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="value"/> is less than zero</exception>
    <Category("Data"), Description("Gets (or sets from inherited class); number of records in source")> _
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property RecordsTotal() As Integer
        Get
            If MyBase.DesignMode Then
                Return DesignTimeRecordsTotal
            Else
                'If value is requested befor it was retrieved then retrieve it first
                If Not SelectCountDone Then OnCountSelecting()
                Return _RecordsTotal
            End If
        End Get
        Protected Set(ByVal value As Integer)
            If value < 0 Then Throw New ArgumentOutOfRangeException("value", "RecordsTotal cannot be less than zero")
            If value <> _RecordsTotal Then
                SelectCountDone = True
                If _RecordsTotal <> value Then
                        _RecordsTotal = value
                        If PageIndex > PagesTotal Then PageIndex = PagesTotal
                    'If controls have been already created then recreate them
                    If PagesCreated OrElse OuterCreated Then CreateChildControls()
                End If
            End If
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="SelectCountDone"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _SelectCountDone As Boolean = False
    ''' <summary>Defines if number of records in the source has been already retrieved</summary>
    ''' <remarks>
    ''' The Set Accessor is private.
    ''' This is set to True by setting <see cref="RecordsTotal"/>.
    ''' </remarks>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    <Category("Data"), Description("Gets information if number of records in source has been already selected")> _
    Public Property SelectCountDone() As Boolean
        <DebuggerHidden()> Get
            Return _SelectCountDone
        End Get
        Private Set(ByVal value As Boolean)
            _SelectCountDone = value
        End Set
    End Property

    ''' <summary>Actual number of last record on current page (always even if <see cref="EndParameterIsLength"/> is True)</summary>
    ''' <remarks>
    ''' In contrast to <see cref="NumberOfLastRecorOnPage"/> this always returns actual value (even on last page).
    ''' Value involved by <see cref="EndParameterIsLength"/> returns <see cref="EndParameterValue"/> but it corresponds with <see cref="NumberOfLastRecorOnPage"/>, so it can be greater than <see cref="RecordsTotal"/>.
    ''' </remarks>
    <Category("Data"), Description("Gets number of last record on page (always even when EndParameterIsLength = True.")> _
    <Browsable(False)> _
    Public ReadOnly Property TrueNumberOfLastRecorOnPage() As Integer
        Get
            Return Math.Min(NumberOfLastRecorOnPage, RecordsTotal)
        End Get
    End Property

    ''' <summary>Raises the <see cref="CountSelecting"/> event</summary>
    ''' <param name="e">
    ''' In case of calling from overriden method pass instance of the <see cref="CountSelectingEventArgs"/> class into this parameter. After call it will contain number of records in the source.
    ''' Při volání z přetížené metody předejte do tohoto parametru instanci třídy <see cref="CountSelectingEventArgs"/>. Vrátí se vám v ní počet záznamů ve zdroji.
    ''' If you will pass Nothing (Null) or if you will omit the parameter, you will get no value, but no error will be raised and everything will work as expected.
    ''' </param>
    ''' <remarks>When overriden in derived class one of first statements of overriden method should be call to this method fo base class.</remarks>
    Protected Overridable Sub OnCountSelecting(Optional ByVal e As CountSelectingEventArgs = Nothing)
        If e Is Nothing Then e = New CountSelectingEventArgs
        RaiseEvent CountSelecting(Me, e)
        RecordsTotal = e.Count
    End Sub

    ''' <summary>This event is raised when <see cref="Pager"/> is retrieving number of records in paged data source.</summary>
    ''' <param name="sender">Instance of <see cref="Pager"/> that raised this event.</param>
    ''' <param name="e">Event's parameters</param>
    ''' <remarks>
    ''' If you want to youse <see cref="Pager"/> you must handle this event and you must set the number of records in paged data source to the <see cref="CountSelectingEventArgs.Count"/> property of the <paramref name="e"/> parameter.
    ''' This event should have only one handler otherwise only the value from last called handler will passed to the <see cref="Pager"/>.
    ''' </remarks>
    <Category("Data")> _
    <Description("Raised when Pager is getting count of records in source. You must handle this event and pass proper value back in order to get Pager working.")> _
    Public Event CountSelecting(ByVal sender As Pager, ByVal e As CountSelectingEventArgs)

    ''' <summary>This class represents argumens of the <see cref="CountSelecting"/> event</summary>
    Public Class CountSelectingEventArgs : Inherits EventArgs
        ''' <summary>Do-nothing CTor</summary>
        ''' <remarks>This CTor is here in order not to allow everybody to instantiate this class</remarks>
        Friend Sub New() 'Do nothing
        End Sub
        ''' <summary>Contains value of the <see cref="Count"/> property</summary>
        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private _Count As UInteger = 0
        ''' <summary>Number of records in data source</summary>
        ''' <remarks>This property must be set in the handler of the <see cref="CountSelecting"/>event.</remarks>
        Public Property Count() As Integer
            Get
                Return _Count
            End Get
            Set(ByVal value As Integer)
                _Count = value
            End Set
        End Property
    End Class

    ''' <summary>When set to True the <see cref="CountSelecting"/> event will not be fired automatically</summary>
    ''' <remarks>
    ''' This is set to True by the <see cref="SelectCount"/> method with the<code>BlockAutomaticSelectCount</code> parameter set to True.
    ''' </remarks>
    Protected BlockAutomaticSelectCount As Boolean = False

    ''' <summary>Retrieves total number of records in the data source and sets the <see cref="RecordsTotal"/> property</summary>
    ''' <returns>Total number of records in the data source</returns>
    ''' <param name="BlockAutomaticSelectCount">Set this parameter to true whn you are calling this function before <see cref="Pager"/> automatically retrieves number of records and you wish that <see cref="Pager"/> will not re-retrieve it automatically in its native time.</param>
    ''' <remarks>
    ''' Use this function to retireve total number of records in the data source before <see cref="Pager"/> retrieve it automatically or use it to re-retrieve it.
    ''' In case of re-retirieving number of records after automatic retrieving by <see cref="Pager"/> the parameter <paramref name="BlockAutomaticSelectCount"/> has no effect.
    ''' Note that automatic retrieving of number of records in the data source is done in the Init stage of lifecycle of the control.
    ''' Note thet retrieving of number of records in the data source is always done through the <see cref="CountSelecting"/> event.
    ''' </remarks>
    Public Function SelectCount(Optional ByVal BlockAutomaticSelectCount As Boolean = False) As Integer
        OnCountSelecting()
        BlockAutomaticSelectCount = True
        Return RecordsTotal
    End Function

    ''' <summary>Raises the Load event. </summary>
    ''' <param name="e">The <see cref="EventArgs"/> object that contains the event data.</param>
    ''' <remarks>
    ''' <para>This method notifies the server control that it should perform actions common to each HTTP request for the page it is associated with, such as setting up a database query. At this stage in the page lifecycle, server controls in the hierarchy are created and initialized, view state is restored, and form controls reflect client-side data.</para>
    ''' <para>Use the IsPostBack property to determine whether the page is being loaded in response to a client postback, or if it is being loaded and accessed for the first time.</para>
    ''' </remarks>
    Protected Overrides Sub OnLoad(ByVal e As System.EventArgs)
        MyBase.OnLoad(e)
        LoadDone = True
    End Sub

    ''' <summary>Countains value of the <see cref="InitDone"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _InitDone As Boolean = False
    ''' <summary>Contains value of the <see cref="LoadDone"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _LoadDone As Boolean = False
    ''' <summary>Indicates that the initialization stage of lifecycle of the <see cref="Pager"/> control has been finished.</summary>
    ''' <remarks><list>
    ''' <item>At the end of the <see cref="OnInit"/> method is set to True</item>
    ''' <item>In case of overriding the <see cref="OnInit"/> method in derived class and not callin base class's <see cref="OnInit"/> metnod it is necessary to set value of this property to True manually at the end of the method.</item>
    ''' <item>Value cannot be set to False</item>
    ''' </list></remarks>
    ''' <exception cref="ArgumentException">When trying to set value to False</exception>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
    Public Property InitDone() As Boolean
        <DebuggerHidden()> _
        Get
            Return _InitDone
        End Get
        Protected Set(ByVal value As Boolean)
            If value = True Then
                _InitDone = value
            Else
                Throw New ArgumentException("InitDone can be set only to true")
            End If
        End Set
    End Property
    ''' <summary>Indicates that the load stage of lifecycle of the <see cref="Pager"/> control has been finished.</summary>
    ''' <remarks><list>
    ''' <item>At the end of the <see cref="OnLoad"/> method is set to True</item>
    ''' <item>In case of overriding the <see cref="OnLoad"/> method in derived class and not callin base class's <see cref="OnLoad"/> metnod it is necessary to set value of this property to True manually at the end of the method.</item>
    ''' <item>Value cannot be set to False</item>
    ''' </list></remarks>
    ''' <exception cref="ArgumentException">When trying to set value to False</exception>
    <Browsable(False), DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)> _
        Public Property LoadDone() As Boolean
        <DebuggerHidden()> _
        Get
            Return _InitDone
        End Get
        Protected Set(ByVal value As Boolean)
            If value = True Then
                _InitDone = value
            Else
                Throw New ArgumentException("InitDone can be set only to true")
            End If
        End Set
    End Property

    ''' <summary>Raises the System.Web.UI.Control.Init event.</summary>
    ''' <param name="e">An System.EventArgs object that contains the event data.</param>
    Protected Overrides Sub OnInit(ByVal e As System.EventArgs)
        MyBase.OnInit(e)
        If Not BlockAutomaticSelectCount Then
            OnCountSelecting()
        End If
        CreateChildControls()
        InitDone = True
    End Sub
#End Region

    ''' <summary>Number of records on current page</summary>
    ''' <remarks>
    ''' I case of last page can be less then <see cref="PageSize"/>. This returns actual number.
    ''' Returns 0 if there are no data.
    ''' </remarks>
    <Category("Data"), Description("Gets number of records on current page")> _
    <Browsable(False)> _
    Public ReadOnly Property RecordsPerCurrentPage() As Integer
        Get
            Try
                Return RecordsTotal Mod PageSize
            Catch ex As DivideByZeroException
                Return 0
            End Try
        End Get
    End Property

    ''' <summary>Number of pages</summary>
    ''' <remarks>Total number of pages. Returns 0 if there are no data.</remarks>
    <Category("Data"), Description("Gets number of pages")> _
    <Browsable(False)> _
    Public ReadOnly Property PagesTotal() As Integer
        Get
            Try
                Return Math.Ceiling(RecordsTotal / PageSize)
            Catch ex As DivideByZeroException
                Return 0
            End Try
        End Get
    End Property

    ''' <summary>Updates <see cref="PageIndex"/> in order to display record with given number</summary>
    ''' <param name="Number">Number of record to be displayed</param>
    ''' <remarks>Muves view to the page than contains record witn number <paramref name="Number"/></remarks>
    Sub MakeVisible(ByVal Number As Integer)
        Throw New NotImplementedException
        If Number > RecordsTotal OrElse Number <= 0 Then
            Throw New ArgumentOutOfRangeException("Number", "Number must be greater than zero and les or equal to RecordsTotal")
        ElseIf Number >= NumberOfFirstRecordOnPage AndAlso Number <= NumberOfLastRecorOnPage Then
            Return
        Else
            PageIndex = Number / PageSize
        End If
    End Sub

    ''' <summary>This property should be used by data source to retrieve index of first record on current page</summary>
    <Browsable(False), Category("Data")> _
    <Description("Use this property as source for DataSource's parameter that gets number of first item")> _
    <Bindable(True)> _
    Public ReadOnly Property StartParameterVlaue() As Integer
        Get
            If PageIndex <= 1 Then Return 1 Else Return NumberOfFirstRecordOnPage
        End Get
    End Property
    ''' <summary>This property should be used by data source to retireve index of last record or number of records on current page (depending on <see cref="EndParameterIsLength"/>)</summary>
    <Browsable(False), Category("Data")> _
    <Description("Use this property as source for datasourse's parameter that gets number of last item (or number of items according to EndParematerIsLength property)")> _
    <Bindable(True)> _
    Public ReadOnly Property EndParameterValue() As Integer
        Get
                If EndParameterIsLength OrElse PageIndex <= 1 Then
                    Return PageSize
                Else
                    Return NumberOfLastRecorOnPage
                End If
            End Get
    End Property

    ''' <summary>Contains value of the <see cref="DesignTimeRecordsTotal"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _DesignTimeRecordsTotal As UInteger = 100
    ''' <summary>Defines fictional number of records displayed in design time</summary>
    ''' <remarks>
    ''' Use this for testing purposes. Takes no effect at runtime.
    ''' <seealso cref="RecordsTotal"/> <seealso cref="DesignMode"/>
    ''' </remarks>
    ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="value"/> is less then or equal to zero</exception>
    ''' <exception cref="InvalidOperationException">When trying to set this property at runtime</exception>
    <Category("Appearance"), DesignOnly(True)> _
    <Description("Gets or sets fictive number of pages displayed at desgn time in order to test control appearance")> _
    <DefaultValue(100I)> _
    Public Property DesignTimeRecordsTotal() As Integer
        <DebuggerHidden()> Get
            Return _DesignTimeRecordsTotal
        End Get
        Set(ByVal value As Integer)
            If value <= 0 Then Throw New ArgumentOutOfRangeException("value", "Value must be greater than zero")
            If Me.DesignMode Then Throw New InvalidOperationException("DesignTimeRecordsTotal can be set only in design time")
            If _DesignTimeRecordsTotal <> value Then
                _DesignTimeRecordsTotal = value
                'If controles have been created then recreate them
                If PagesCreated OrElse OuterCreated Then CreateChildControls()
            End If
        End Set
    End Property
#End Region

#Region "Controls" 'Web controls this composite composite controls consists of realated code
    ''' <summary>First page navigation <see cref="Button"/></summary>
    Protected btnFirst As Button
    ''' <summary>Previous page navigation <see cref="Button"/></summary>
    Protected btnPrev As Button
    ''' <summary>First page navigation <see cref="LinkButton"/></summary>
    Protected lbtFirst As LinkButton
    ''' <summary>Previous page navigation <see cref="LinkButton"/></summary>
    Protected lbtPrev As LinkButton
    ''' <summary>First page navigation <see cref="ImageButton"/></summary>
    Protected imbFirst As ImageButton
    ''' <summary>Previous page navigation <see cref="ImageButton"/></summary>
    Protected imbPrev As ImageButton
    ''' <summary><see cref="Label"/> that contains additional text</summary>
    Protected lblText As Label
    ''' <summary>Next page navigation <see cref="Button"/></summary>
    Protected btnNext As Button
    ''' <summary>Last page navigation <see cref="Button"/></summary>
    Protected btnLast As Button
    ''' <summary>Next page navigation <see cref="LinkButton"/></summary>
    Protected lbtNext As LinkButton
    ''' <summary>Last page navigation <see cref="LinkButton"/></summary>
    Protected lbtLast As LinkButton
    ''' <summary>Next page navigation <see cref="ImageButton"/></summary>
    Protected imbNext As ImageButton
    ''' <summary>Last page navigation <see cref="ImageButton"/></summary>
    Protected imbLast As ImageButton
    ''' <summary>Controls for navigation to concrete pages</summary>
    ''' <remarks>
    ''' This <see cref="List(Of WebControl)"/> also contains <see cref="WebControl"/>s marking missing numbers (if applicable)
    ''' <seealso cref="Missings"/>
    ''' </remarks>
    Protected Pages As List(Of WebControl)
    ''' <summary>Controls marking missing numbers in <see cref="Pages"/></summary>
    ''' <remarks>Controls form this <see cref="List(Of Label)"/> are also present in <see cref="Pages"/></remarks>
    Protected Missings As List(Of Label)
    ''' <summary><see cref="Label"/> or <see cref="Button"/> that marks actual page</summary>
    ''' <remarks>Also present in <see cref="Pages"/></remarks>
    Protected ActualPageMarker As WebControl
    ''' <summary>Contains value of the <see cref="OuterCreated"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _OuterCreated As Boolean = False
    ''' <summary>Indicates if so-called outer navigation controls have been already created</summary>
    ''' <remarks>
    ''' This is set to true even if no controls are visible. E.g. due to <see cref="ShowFirstLast"/> is False and <seealso cref="ShowPrevNext"/> is false.
    ''' This is set to true by the <see cref="CreateOuter"/> method.
    ''' This property can be set only to True.
    ''' If overriding <see cref="CreateOuter"/> method in derived class then it is essential to set this property to True at the end of overriden method.
    ''' </remarks>
    ''' <exception cref="ArgumentException">When <paramref name="value"/> is False</exception>
    <Browsable(False)> _
    Protected Property OuterCreated() As Boolean
        <DebuggerHidden()> Get
            Return _OuterCreated
        End Get
        Set(ByVal value As Boolean)
            If value = False Then Throw New ArgumentException("OuterCreated can be set only to True")
            _OuterCreated = True
        End Set
    End Property

    ''' <summary>Contains value of the <see cref="PagesCreated"/> property</summary>
    <EditorBrowsable(EditorBrowsableState.Never)> _
    Private _PagesCreated As Boolean = False
    ''' <summary>Indicates if per-page navigation controls have been aleady created</summary>
    ''' <remarks>
    ''' This is set to True even if no controls are visible. E.g. due to <see cref="ShowPages"/> is False.
    ''' This isset to True by the <see cref="CreatePages"/> method.
    ''' This property can be set only to True.
    ''' If overriding <see cref="CreatePages"/> method in derived class than it is essential to set this property to True at the end of overrriden method.
    ''' </remarks>
    <Browsable(False)> _
    Protected Property PagesCreated() As Boolean
        <DebuggerHidden()> Get
            Return _PagesCreated
        End Get
        Set(ByVal value As Boolean)
            If value = False Then Throw New ArgumentException("PagesCreated can be set only to True")
            _PagesCreated = value
        End Set
    End Property
    ''' <summary>Ceates so-called outer navigation controls (Buttons First, Previous, Next, Last) and <see cref="Text"/> <see cref="Label"/></summary>
    ''' <remarks>
    ''' It is essential to set <see cref="OuterCreated"/> to True at the end of overriding method in derived class (if overriden).
    ''' <para>If derived class uses custom controls for outer controls then it should override this method</para>
    ''' </remarks>
    Protected Overridable Sub CreateOuter()
        'The base principal is that controls are created only once, are never removed and are never re-created, handlers are never re-added. Only visible property is manipulated.
        'First Button
        If FirstLastButtonShown Then
            If btnFirst Is Nothing Then
                btnFirst = New Button
                AddHandler btnFirst.Command, AddressOf Command
                Controls.Add(btnFirst)
            End If
            With btnFirst
                .ID = "btnFirst"
                .Text = FirstText
                .ToolTip = FirstToolTip
                .CssClass = CSSClassFirst
                .CommandName = cmdGoToIndex
                .CommandArgument = 1
                .Enabled = Not DisableNonsenses OrElse PageIndex > 1
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf btnFirst IsNot Nothing Then : btnFirst.Visible = False
        End If
        'Prev Button
        If PrevNextButtonShown Then
            If btnPrev Is Nothing Then
                btnPrev = New Button
                AddHandler btnPrev.Command, AddressOf Command
                Controls.Add(btnPrev)
            End If
            With btnPrev
                .ID = "btnPrev"
                .Text = PrevText
                .ToolTip = PrevToolTip
                .CssClass = CSSClassPrev
                .CommandName = cmdGoPrev
                .Enabled = Not DisableNonsenses OrElse PageIndex > 1
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf btnPrev IsNot Nothing Then : btnPrev.Visible = False
        End If
        'First LinkButton
        If FirstLastLinkButtonShown Then
            If lbtFirst Is Nothing Then
                lbtFirst = New LinkButton
                AddHandler lbtFirst.Command, AddressOf Command
                Controls.Add(lbtFirst)
            End If
            With lbtFirst
                .ID = "lbtFirst"
                .Text = FirstText
                .ToolTip = FirstToolTip
                .CssClass = CSSClassFirst
                .CommandName = cmdGoToIndex
                .CommandArgument = 1
                .Enabled = Not DisableNonsenses OrElse PageIndex > 1
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf lbtFirst IsNot Nothing Then : lbtFirst.Visible = False
        End If
        'Prev LinkButton
        If PrevNextLinkButtonShown Then
            If lbtPrev Is Nothing Then
                lbtPrev = New LinkButton
                AddHandler lbtPrev.Command, AddressOf Command
                Controls.Add(lbtPrev)
            End If
            With lbtPrev
                .ID = "lbtPrev"
                .Text = PrevText
                .ToolTip = PrevToolTip
                .CssClass = CSSClassPrev
                .CommandName = cmdGoPrev
                .Enabled = Not DisableNonsenses OrElse PageIndex > 1
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf lbtPrev IsNot Nothing Then : lbtPrev.Visible = False
        End If
        'First Image
        If FirstLastImageShown Then
            If imbFirst Is Nothing Then
                imbFirst = New ImageButton
                AddHandler imbFirst.Command, AddressOf Command
                Controls.Add(imbFirst)
            End If
            With imbFirst
                .ID = "imbFirst"
                .AlternateText = FirstText
                .ToolTip = FirstToolTip
                .CssClass = CSSClassFirst
                .ImageUrl = FirstImageURL
                .CommandName = cmdGoToIndex
                .CommandArgument = 1
                .Enabled = Not DisableNonsenses OrElse PageIndex > 1
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf imbFirst IsNot Nothing Then : imbFirst.Visible = False
        End If
        'Prev Image
        If PrevNextImageShown Then
            If imbPrev Is Nothing Then
                imbPrev = New ImageButton
                AddHandler imbPrev.Command, AddressOf Command
                Controls.Add(imbPrev)
            End If
            With imbPrev
                .ID = "imbPrev"
                .AlternateText = PrevText
                .ToolTip = PrevToolTip
                .CssClass = CSSClassPrev
                .ImageUrl = PrevImageURL
                .CommandName = cmdGoPrev
                .Enabled = Not DisableNonsenses OrElse PageIndex > 1
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf imbPrev IsNot Nothing Then : imbPrev.Visible = False
        End If
        'Text
        If Me.Text <> "" Then
            If lblText Is Nothing Then
                lblText = New Label
                Controls.Add(lblText)
            End If
            With lblText
                .ID = "lblText"
                .Text = Me.Text
                .Visible = True
            End With
        ElseIf lblText IsNot Nothing Then : lblText.Visible = False
        End If
        'Last Button
        If FirstLastButtonShown Then
            If btnLast Is Nothing Then
                btnLast = New Button
                AddHandler btnLast.Command, AddressOf Command
                Controls.Add(btnLast)
            End If
            With btnLast
                .ID = "btnLast"
                .Text = LastText
                .ToolTip = LastToolTip
                .CssClass = CSSClassLast
                .CommandName = cmdGoLast
                .Enabled = Not DisableNonsenses OrElse PageIndex < PagesTotal
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf btnLast IsNot Nothing Then : btnLast.Visible = False
        End If
        'Next Button
        If PrevNextButtonShown Then
            If btnNext Is Nothing Then
                btnNext = New Button
                AddHandler btnNext.Command, AddressOf Command
                Controls.Add(btnNext)
            End If
            With btnNext
                .ID = "btnNext"
                .Text = NextText
                .ToolTip = NextToolTip
                .CssClass = CSSClassNext
                .CommandName = cmdGoNext
                .Enabled = Not DisableNonsenses OrElse PageIndex < PagesTotal
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf btnNext IsNot Nothing Then : btnNext.Visible = False
        End If
        'Last LinkButton
        If FirstLastLinkButtonShown Then
            If lbtLast Is Nothing Then
                lbtLast = New LinkButton
                AddHandler lbtLast.Command, AddressOf Command
                Controls.Add(lbtLast)
            End If
            With lbtLast
                .ID = "lbtLast"
                .Text = LastText
                .ToolTip = LastToolTip
                .CssClass = CSSClassLast
                .CommandName = cmdGoLast
                .Enabled = Not DisableNonsenses OrElse PageIndex < PagesTotal
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf lbtLast IsNot Nothing Then : lbtLast.Visible = False
        End If
        'Next LinkButton
        If PrevNextLinkButtonShown Then
            If lbtNext Is Nothing Then
                lbtNext = New LinkButton
                AddHandler lbtNext.Command, AddressOf Command
                Controls.Add(lbtNext)
            End If
            With lbtNext
                .ID = "lbtNext"
                .Text = NextText
                .ToolTip = NextToolTip
                .CssClass = CSSClassNext
                .CommandName = cmdGoNext
                .Enabled = Not DisableNonsenses OrElse PageIndex < PagesTotal
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf lbtNext IsNot Nothing Then : lbtNext.Visible = False
        End If
        'Last ImageButton
        If FirstLastImageShown Then
            If imbLast Is Nothing Then
                imbLast = New ImageButton
                AddHandler imbLast.Command, AddressOf Command
                Controls.Add(imbLast)
            End If
            With imbLast
                .ID = "imbLast"
                .AlternateText = LastText
                .ToolTip = LastToolTip
                .CssClass = CSSClassLast
                .ImageUrl = LastImageURL
                .CommandName = cmdGoLast
                .Enabled = Not DisableNonsenses OrElse PageIndex < PagesTotal
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf imbLast IsNot Nothing Then : imbLast.Visible = False
        End If
        'Next ImageButton
        If PrevNextImageShown Then
            If imbNext Is Nothing Then
                imbNext = New ImageButton
                AddHandler imbNext.Command, AddressOf Command
                Controls.Add(imbNext)
            End If
            With imbNext
                .ID = "imbNext"
                .AlternateText = NextText
                .ToolTip = NextToolTip
                .CssClass = CSSClassNext
                .ImageUrl = NextImageURL
                .CommandName = cmdGoNext
                .Enabled = Not DisableNonsenses OrElse PageIndex < PagesTotal
                .Visible = True
                .EnableViewState = False
            End With
        ElseIf imbNext IsNot Nothing Then : imbNext.Visible = False
        End If
        _OuterCreated = True
    End Sub
    ''' <summary>Creates per-page navigation conttrols</summary>
    ''' <remarks>
    ''' It is essential to set <see cref="PagesCreated"/> to True at the end of overriding method in derived class (if overriden)
    ''' <para>If derived class uses cutom controls to represent concrete pages then it should override this method.</para>
    ''' </remarks>
    Protected Overridable Sub CreatePages()
        ClearPages()
        'Pages
        If ShowPages Then
            InitPages()
        End If
        If Pages IsNot Nothing Then
            For Each Page As WebControl In Pages
                Controls.Add(Page)
            Next Page
        End If
        _PagesCreated = True
    End Sub

    ''' <summary>Called by the ASP.NET page framework to notify server controls that use composition-based implementation to create any child controls they contain in preparation for posting back or rendering.</summary>
    ''' <remarks>
    ''' This calls <see cref="CreateOuter"/> and <see cref="CreatePages"/> methods.
    ''' This method cannot be called again until it finishes (indirect recursion is blocked). Sub is exited when it is called through indirect-recursion (no exception is thrown). This behaviour doesn't depend on the caller - this class, derived class, base class (ASP.NET page framework). 
    ''' Though it is not restricted this method should not be overriden. Rather override <see cref="CreateOuter"/> and <see cref="CreatePages"/> methods.
    ''' This should be overriden only when thouse methods are not in use and it should happen only when there is no easy boundary between so-called outer and per-page contrls in derived class.
    ''' </remarks>
    Protected Overrides Sub CreateChildControls()
        Static Pending As Boolean
        If Pending Then Exit Sub
        Pending = True
        CreateOuter()
        CreatePages()
        Pending = False
    End Sub

    ''' <summary>Removes references to per-page navigation controls and its handlers</summary>
    ''' <remarks>
    ''' This is called by <see cref="CreatePages"/> at the beginning.
    ''' <list>
    ''' <item>Handlers to controls' command event are removed.</item>
    ''' <item>Controls are removed from the <see cref="Controls"/> collection.</item>
    ''' <item><see cref="Pages"/>, <see cref="Missings"/> and <see cref="ActualPageMarker"/> are set to <c>Nothing</c> (null reference).</item>
    ''' </list>
    ''' If derived class uses own controls for pages it should override or neve use this method.
    ''' </remarks>
    Protected Overridable Sub ClearPages()
        If Pages Is Nothing Then Return
        For Each pg As WebControl In Pages
            If TypeOf pg Is LinkButton Then
                RemoveHandler CType(pg, LinkButton).Command, AddressOf Command
            ElseIf TypeOf pg Is Button Then
                RemoveHandler CType(pg, Button).Command, AddressOf Command
            End If
            Controls.Remove(pg)
        Next pg
        Pages = Nothing
        Missings = Nothing
        ActualPageMarker = Nothing
    End Sub
    ''' <summary>Recreates the child controls n a control derived from System.Web.UI.WebControls.CompositeControl. </summary>
    Protected Overrides Sub RecreateChildControls()
        EnsureChildControls()
    End Sub
    ''' <summary>Writes the System.Web.UI.WebControls.CompositeControl content to the specified System.Web.UI.HtmlTextWriter object, for display on the client.</summary>
    ''' <param name="writer">An System.Web.UI.HtmlTextWriter that represents the output stream to render HTML content on the client.</param>
    ''' <remarks>This should be overriden in derived class if it uses custom controls.</remarks>
    Protected Overrides Sub Render(ByVal writer As System.Web.UI.HtmlTextWriter)
        AddAttributesToRender(writer)
        writer.AddAttribute(HtmlTextWriterAttribute.Class, CssClass)
        writer.RenderBeginTag(HtmlTextWriterTag.Div)
        writer.Write(writer.NewLine)
        'Pre-per-page buttons buttons
        If FirstLastButtonShown Then : btnFirst.RenderControl(writer)
        ElseIf FirstLastLinkButtonShown Then : lbtFirst.RenderControl(writer)
        ElseIf FirstLastImageShown Then : imbFirst.RenderControl(writer) : End If
        If PrevNextButtonShown Then : btnPrev.RenderControl(writer)
        ElseIf PrevNextLinkButtonShown Then : lbtPrev.RenderControl(writer)
        ElseIf PrevNextImageShown Then : imbPrev.RenderControl(writer) : End If
        'Text
        If Me.Text <> "" Then writer.Write(writer.NewLine) : lblText.RenderControl(writer)
        writer.Write(writer.NewLine)
        'Per-page buttons
        If ShowPages Then
            writer.Write(HtmlTextWriter.SpaceChar)
            writer.AddAttribute(HtmlTextWriterAttribute.Class, CSSClassNumberBlock)
            writer.RenderBeginTag(HtmlTextWriterTag.Span)
            WritePages(writer)
            writer.RenderEndTag()
            writer.Write(HtmlTextWriter.SpaceChar)
            writer.Write(writer.NewLine)
        End If
        'Post-per-page buttons buttons
        If PrevNextButtonShown Then : btnNext.RenderControl(writer)
        ElseIf PrevNextLinkButtonShown Then : lbtNext.RenderControl(writer)
        ElseIf PrevNextImageShown Then : imbNext.RenderControl(writer) : End If
        If FirstLastButtonShown Then : btnLast.RenderControl(writer)
        ElseIf FirstLastLinkButtonShown Then : lbtLast.RenderControl(writer)
        ElseIf FirstLastImageShown Then : imbLast.RenderControl(writer) : End If
        writer.Write(writer.NewLine)
        writer.RenderEndTag()
    End Sub
    ''' <summary>Initializes pages (per-page navigation controls)</summary>
    ''' <remarks>This is used only by <see cref="CreatePages"/></remarks>
    Private Sub InitPages()
        If Not ShowPages Then Exit Sub
        'Total number of pages
        Dim TotalPagesCount As UInteger = PagesTotal
        'Actual page number
        Dim ActualPage As UInteger = PageIndex
        'Size of so-called neighbourhood
        Dim Neighbourhood As UInteger = NeighbourhoodSize
        'Size of the begin and the end areas
        Dim EndSize As UInteger = BeginEndSize
        Dim Prefix As String = NumberPrefix
        Dim Sufix As String = NumberSufix
        Dim Space As String = MissingNumbersIndicator
        Dim NSystem As enmNumberingTypes = NumberingType
        Dim NSystProv As NumericSystem = NumericSystem.Create(NSystem)
        Pages = New List(Of WebControl)
        Missings = New List(Of Label)
        'Calculate the sizes of areas
        Dim A1, A2, B1, B2, C1, C2 As UInteger
        Dim NeighPůl As UInteger = Neighbourhood \ 2
        Select Case ManyNumbersMode
            Case enmPageNumbersStyle.All
                A1 = 1 : A2 = 0
                B1 = 1 : B2 = TotalPagesCount
                C1 = 1 : C2 = 0
            Case enmPageNumbersStyle.Current
                A1 = 1 : A2 = 0
                B1 = ActualPage : B2 = ActualPage
                C1 = 1 : C2 = 0
            Case enmPageNumbersStyle.FirstNeighboursLast
                A1 = 1 : A2 = 1
                B1B2(A2, B1, B2, ActualPage, NeighPůl, TotalPagesCount, EndSize)
                If B2 < TotalPagesCount Then
                    C1 = TotalPagesCount : C2 = TotalPagesCount
                Else
                    C1 = 1 : C2 = 0
                End If
            Case enmPageNumbersStyle.Neighbours
                A1 = 1 : A2 = 0
                B1B2(A2, B1, B2, ActualPage, NeighPůl, TotalPagesCount, EndSize)
                C1 = 1 : C2 = 0
            Case Else
                A1 = 1 'The begin of the begin
                A2 = Math.Min(EndSize, TotalPagesCount) 'The end of the begin
                B1B2(A2, B1, B2, ActualPage, NeighPůl, TotalPagesCount, EndSize)
                    C1 = Tools.MathT.Max(B2 + 1, CInt(TotalPagesCount) - EndSize + 1, A2 + 1) 'The begin of the end
                C2 = TotalPagesCount 'The end of the end
        End Select
        'The begin area
        For i As UInteger = A1 To A2
            If i <> ActualPage Then
                Pages.Add(NewNumber(NumbersStyle, NSystProv, i, ActualPage, TotalPagesCount, Prefix, Sufix, AddressOf Command))
            Else
                ActualPageMarker = NewActualNumber(NumbersStyle, NSystProv, i, ActualPage, TotalPagesCount, Prefix, Sufix, CSSClassCurrent)
                Pages.Add(ActualPageMarker)
            End If
        Next i
        'Separator between the begin and so-called neighbourhood
        If B1 > A2 + 1 AndAlso A2 >= A1 Then
            Dim lbl As New Label
            With lbl
                .Text = Space
                .CssClass = CSSClassMissing
            End With
            Pages.Add(lbl)
            Missings.Add(lbl)
        End If
        'So-called neighbourhood (center)
        'Starting at center - neighbourhood \ 2 (or at begin's end + 1)
        '   (or the neighbourhood can be moved left if it is near the end)
        'ends at center + neighbourhood \ 2 (or end's begin - 1)
        '   (or the neighbourhood can be moved right if it is near the end) 
        For i As UInteger = B1 To B2
            If i <> ActualPage Then
                Pages.Add(NewNumber(NumbersStyle, NSystProv, i, ActualPage, TotalPagesCount, Prefix, Sufix, AddressOf Command))
            Else
                ActualPageMarker = NewActualNumber(NumbersStyle, NSystProv, i, ActualPage, TotalPagesCount, Prefix, Sufix, CSSClassCurrent)
                Pages.Add(ActualPageMarker)
            End If
        Next i
        'separator between neighbourhood and the end
        If C1 > B2 + 1 AndAlso C1 <= C2 Then
            Dim lbl As New Label
            With lbl
                .Text = Space
                .CssClass = CSSClassMissing
            End With
            Pages.Add(lbl)
            Missings.Add(lbl)
        End If
        'The end
        For i As UInteger = C1 To C2
            If i <> ActualPage Then
                Pages.Add(NewNumber(NumbersStyle, NSystProv, i, ActualPage, TotalPagesCount, Prefix, Sufix, AddressOf Command))
            Else
                ActualPageMarker = NewActualNumber(NumbersStyle, NSystProv, i, ActualPage, TotalPagesCount, Prefix, Sufix, CSSClassCurrent)
                Pages.Add(ActualPageMarker)
            End If
        Next i
    End Sub
    ''' <summary>According to given parameters computes params <paramref name="B1"/> and <paramref name="B2"/></summary>
    ''' <param name="A2">The end of the begin</param>
    ''' <param name="B1">The begin of the neighbourhood</param>
    ''' <param name="B2">The end of the neighbourhood</param>
    ''' <param name="ActualPage">Current page, see <see cref="PageIndex"/></param>
    ''' <param name="NeighPůl">Half of <see cref="NeighbourhoodSize"/></param>
    ''' <param name="TotalPagesCount">Total number of pages, see <see cref="PagesTotal"/></param>
    ''' <param name="EndSize">Size of the begin and of the end, see <see cref="BeginEndSize"/></param>
    ''' <remarks>This is designed only for use from <see cref="InitPages"/></remarks>
    Private Sub B1B2(ByVal A2 As UInteger, <Out()> ByRef B1 As UInteger, <Out()> ByRef B2 As UInteger, ByVal ActualPage As UInteger, ByVal NeighPůl As UInteger, ByVal TotalPagesCount As UInteger, ByVal EndSize As UInteger)
        B1 = Math.Max(A2 + 1, _
                CInt(ActualPage) - NeighPůl - Math.Max(0, _
                    CInt(NeighPůl + ActualPage) - CInt(TotalPagesCount))) 'The begin of the neighbourhood
        B2 = Math.Min( _
            Math.Min( _
                ActualPage + NeighPůl + Math.Max(0, _
                    CInt(NeighPůl) - ActualPage + 1I _
                ), _
                Math.Max(CInt(PagesTotal) - EndSize, 0) _
            ), TotalPagesCount) 'The end of the neighbourhood
    End Sub
    ''' <summary>Creates page number</summary>
    ''' <param name="Style">Defines the type of control tah realizes the number</param>
    ''' <param name="Syst">Numbering system used for number</param>
    ''' <param name="Index">1-based value for this number</param>
    ''' <param name="ActualPage">Number of page currentlly displayed</param>
    ''' <param name="Count">Total n umber of pages</param>
    ''' <param name="Prefix">Number's prefix</param>
    ''' <param name="Sufix">Number's sufix</param>
    ''' <param name="AddressOfCommad">Handler of the event. This will be connected with the <c>Command</c> event</param>
    ''' <returns>Instance of server control that realizes the number</returns>
    Private Shared Function NewNumber(ByVal Style As enmPageStyle, ByVal Syst As NumericSystem, ByVal Index As UInteger, ByVal ActualPage As UInteger, ByVal Count As UInteger, ByVal Prefix As String, ByVal Sufix As String, ByVal AddressOfCommad As CommandEventHandler) As WebControl
        If Style = enmPageStyle.Button Then
            Dim btn As New Button
            With btn
                .Text = Prefix & Syst.Number(Index, ActualPage, Count) & Sufix
                NewNumber = btn
                .CommandArgument = Index
                .CommandName = cmdGoToIndex
                AddHandler btn.Command, AddressOfCommad
                .EnableViewState = False
                .ID = "btnPage" & Index
            End With
        Else
            Dim lbt As New LinkButton
            With lbt
                .Text = Prefix & Syst.Number(Index, ActualPage, Count) & Sufix
                NewNumber = lbt
                .CommandArgument = Index
                .CommandName = cmdGoToIndex
                AddHandler lbt.Command, AddressOfCommad
                .EnableViewState = False
                .ID = "lbtPage" & Index
            End With
        End If
    End Function

 
    ''' <summary>Creates current page number</summary>
    ''' <param name="Style">Defines the type of control tah realizes the number</param>
    ''' <param name="Syst">Numbering system used for number</param>
    ''' <param name="Index">1-based value for this number</param>
    ''' <param name="ActualPage">Number of page currentlly displayed</param>
    ''' <param name="Count">Total n umber of pages</param>
    ''' <param name="Prefix">Number's prefix</param>
    ''' <param name="Sufix">Number's sufix</param>
    ''' <returns>Instance of server control that realizes the number</returns>
    Private Shared Function NewActualNumber(ByVal Style As enmPageStyle, ByVal Syst As NumericSystem, ByVal Index As UInteger, ByVal ActualPage As UInteger, ByVal Count As UInteger, ByVal Prefix As String, ByVal Sufix As String, ByVal CSSClass As String) As WebControl
        If Style = enmPageStyle.Button Then
            Dim btn As New Button
            With btn
                .Text = Prefix & Syst.Number(Index, ActualPage, Count) & Sufix
                NewActualNumber = btn
                .CommandArgument = Index
                .CommandName = cmdGoToIndex
                .Enabled = False
                .CssClass = CSSClass
                .EnableViewState = False
            End With
        Else
            Dim lbt As New LinkButton
            With lbt
                .Text = Prefix & Syst.Number(Index, ActualPage, Count) & Sufix
                NewActualNumber = lbt
                .CommandArgument = Index
                .CommandName = cmdGoToIndex
                .Enabled = False
                .CssClass = CSSClass
                .EnableViewState = False
            End With
        End If
    End Function

    ''' <summary>Writes pages' numbers</summary>
    Private Sub WritePages(ByVal writer As HtmlTextWriter)
        If Not ShowPages Then Exit Sub
        Dim NumSep As String = NumberSeparator
        For i As Integer = 0 To Pages.Count - 1
            If i > 0 AndAlso Not TypeOf Pages(i - 1) Is Label AndAlso Not TypeOf Pages(i) Is Label Then
                writer.WriteEncodedText(NumSep)
            End If
            Pages(i).RenderControl(writer)
        Next i
    End Sub

    ''' <summary>Name of the event for navigation to last page</summary>
    Protected Const cmdGoLast$ = "GoLast"
    ''' <summary>Name of the event for navigation to next page</summary>
    Protected Const cmdGoNext$ = "GoNext"
    ''' <summary>Name of event for navigation to previous page</summary>
    Protected Const cmdGoPrev$ = "GoPrev"
    ''' <summary>Name of event for navigation to concrete page</summary>
    Protected Const cmdGoToIndex$ = "GoToIndex"
    ''' <summary>Name of event for navigation to first page</summary>
    ''' <remarks>Never used. <see cref="cmdGoToIndex"/> with 1 parameter is used instead. This can be used from derived class.</remarks>
    Protected Const cmdGoFirst$ = "GoFirst"

    ''' <summary>Handles events from all butoons (navigation controls)</summary>
    ''' <param name="sender">Navigation control tha caused the event</param>
    ''' <param name="e">Event's paramatars</param>
    ''' <remarks>This is the place where navigation is being done.</remarks>
    Protected Overridable Sub Command(ByVal sender As Object, ByVal e As CommandEventArgs)
        Select Case e.CommandName
            Case cmdGoLast
                If PageIndex < PagesTotal Then
                    Dim OldIndex As UInteger = PageIndex
                    Command(sender, New CommandEventArgs(cmdGoToIndex, PagesTotal))
                    If PageIndex <> OldIndex Then OnWentLast(New EventArgs)
                End If
            Case cmdGoNext
                If PageIndex < PagesTotal Then
                    Dim OldIndex As UInteger = PageIndex
                    Command(sender, New CommandEventArgs(cmdGoToIndex, PageIndex + 1))
                    If OldIndex <> PageIndex Then OnWentNext(New EventArgs)
                End If
            Case cmdGoPrev
                If PageIndex - 1 > 0 Then
                    Dim OldIndex As UInteger = PageIndex
                    Command(sender, New CommandEventArgs(cmdGoToIndex, PageIndex - 1))
                    If OldIndex <> PageIndex Then OnWentPrev(New EventArgs)
                End If
            Case cmdGoFirst 'Never used (but can be used by derived class)
                If PageIndex <> 1 Then
                    Dim OldIndex As UInteger = PageIndex
                    Command(sender, New CommandEventArgs(cmdGoToIndex, 1))
                    If OldIndex <> PageIndex Then OnWentFirst(New EventArgs)
                End If
            Case cmdGoToIndex
                Dim NewIndex As UInteger = e.CommandArgument
                If PageIndex <> NewIndex AndAlso NewIndex > 0 AndAlso NewIndex <= PagesTotal Then
                    Dim CancelArgs As New CancelEventArgs(False)
                    OnPageIndexChanging(e)
                    If Not CancelArgs.Cancel Then
                        PageIndex = e.CommandArgument
                        OnWentIndex(New EventArgs)
                    End If
                End If
        End Select
    End Sub

    ''' <summary>This even is fired after <see cref="PageIndex"/> is changed as result of user's clicking to the last button</summary>
    ''' <param name="sender">Instance that caused the event</param>
    ''' <param name="e">Parameters</param>
    <Category("Navigation")> _
    <Description("Raised after index is changed due to user clicking on Last Button")> _
    Public Event WentLast(ByVal sender As Pager, ByVal e As EventArgs)
    ''' <summary>Fires the <see cref="WentLast"/> event</summary>
    ''' <param name="e">Parameters</param>
    ''' <remarks>If override in derived class the base class's method should be called in order the event to be raised</remarks>
    Protected Overridable Sub OnWentLast(ByVal e As EventArgs)
        RaiseEvent WentLast(Me, e)
    End Sub

    ''' <summary>This event is fired after <see cref="PageIndex"/> is changed as result of user's clicking to the next button</summary>
    ''' <param name="sender">Instance that caused the event</param>
    ''' <param name="e">Parameters</param>
    <Category("Navigation")> _
    <Description("Raised after index is changed due to user clicking on Next Button")> _
    Public Event WentNext(ByVal sender As Pager, ByVal e As EventArgs)
    ''' <summary>Fires the <see cref="WentNext"/> event</summary>
    ''' <param name="e">Parameters</param>
    ''' <remarks>If override in derived class the base class's method should be called in order the event to be raised</remarks>
    Protected Overridable Sub OnWentNext(ByVal e As EventArgs)
        RaiseEvent WentNext(Me, e)
    End Sub

    ''' <summary>This event is fired after <see cref="PageIndex"/> is changed as result of user's clicking to the previous button</summary>
    ''' <param name="sender">Instance that caused the event</param>
    ''' <param name="e">Parameters</param>
    <Category("Navigation")> _
    <Description("Raised after index is changed due to user clicking on Prev Button")> _
    Public Event WentPrev(ByVal sender As Pager, ByVal e As EventArgs)
    ''' <summary>Fires the <see cref="WentPrev"/> event</summary>
    ''' <param name="e">Parameters</param>
    ''' <remarks>If override in derived class the base class's method should be called in order the event to be raised</remarks>
    Protected Overridable Sub OnWentPrev(ByVal e As EventArgs)
        RaiseEvent WentPrev(Me, e)
    End Sub

    ''' <summary>This event is fired after <see cref="PageIndex"/> is changed as result of user's clicking to any button (either concrete or outer)</summary>
    ''' <param name="sender">Instance that caused the event</param>
    ''' <param name="e">Parameters</param>
    <Category("Navigation")> _
    <Description("Raised after index is changed due to user clicking on whatever button")> _
    Public Event WentIndex(ByVal sender As Pager, ByVal e As EventArgs)
    ''' <summary>Fires the <see cref="WentIndex"/> event</summary>
    ''' <param name="e">Parameters</param>
    ''' <remarks>If override in derived class the base class's method should be called in order the event to be raised</remarks>
    Protected Overridable Sub OnWentIndex(ByVal e As EventArgs)
        RaiseEvent WentIndex(Me, e)
    End Sub

    ''' <summary>This event can be fired after <see cref="PageIndex"/> is changed as result of user's clicking to the first button, but is never used</summary>
    ''' <param name="sender">Instance that caused the event</param>
    ''' <param name="e">Parameters</param>
    ''' <remarks>
    ''' The <see cref="WentIndex"/> event is used instead with <see cref="PageIndex"/> set to 1.
    ''' This event can be used by derived class that uses <see cref="cmdGoFirst"/>
    '''If override in derived class the base class's method should be called in order the event to be raised
    ''' </remarks>
    <Category("Navigation")> _
    <Description("NEVER USED!!! (can be used by derived classes)")> _
    <Browsable(False), EditorBrowsable(EditorBrowsableState.Never)> _
    Public Event WentFirst(ByVal sender As Pager, ByVal e As EventArgs)
    ''' <summary>Fires the <see cref="WentFirst"/> event</summary>
    ''' <param name="e">Parameters</param>
    ''' <remarks>
    ''' This method is never used but can be used from derived classes
    ''' Instead of this method the <see cref="OnWentIndex"/> method is used and <see cref="PageIndex"/> is set to 1
    ''' </remarks>
    Protected Overridable Sub OnWentFirst(ByVal e As EventArgs)
        RaiseEvent WentFirst(Me, e)
    End Sub

    ''' <summary>This event is fired before user-caused navigation is done</summary>
    ''' <param name="sender">Instance that caused the event</param>
    ''' <param name="e">Parameters</param>
    ''' <remarks>Set the <see cref="CancelEventArgs.Cancel"/> property of the <paramref name="e"/> argument to True in order to cancel navigation</remarks>
    <Category("Navigation")> _
    <Description("Raised before any navigation by user occures")> _
    Public Event PageIndexChanging(ByVal sender As Pager, ByVal e As CancelEventArgs)
    ''' <summary>Fires the <see cref="PageIndexChanging"/> event</summary>
    ''' <param name="e">Parameters</param>
    ''' <remarks>
    ''' Set the <see cref="CancelEventArgs.Cancel"/> property of the <paramref name="e"/> argument to True in order to cancel navigation
    ''' If override in derived class the base class's method should be called in order the event to be raised
    ''' </remarks>
    Protected Overridable Sub OnPageIndexChanging(ByVal e As EventArgs)
        RaiseEvent PageIndexChanging(Me, e)
    End Sub
#End Region
End Class

#If AssemblyBuild <> 1 Then
End Namespace
#End If