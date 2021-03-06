﻿Imports System.ComponentModel
Imports Tools.ComponentModelT

''' <summary>Settings dialog</summary>
Friend Class frmSettings
    ''' <summary>Settings proxy</summary>
    Private proxy As New SettingsProxy
    ''' <summary>CTor</summary>
    Public Sub New()
        InitializeComponent()
        prgProperties.SelectedObject = proxy
    End Sub

    ''' <summary>Proxy class between <see cref="My.Settings"/> and <see cref="PropertyGrid"/></summary>
    Private Class SettingsProxy
        ''' <summary>Contains value of the <see cref="Changed"/> property</summary>
        Private _Changed As Boolean
        ''' <summary>Indicates if settings was changed</summary>
        <Browsable(False)> _
        Public Property Changed() As Boolean
            <DebuggerStepThrough()> Get
                Return _Changed
            End Get
            <DebuggerStepThrough()> Private Set(ByVal value As Boolean)
                _Changed = value
            End Set
        End Property
        ''' <summary>CTor</summary>
        Public Sub New()
            Me.ThumbSize = My.Settings.ThumbSize
            Me.LargeFloating = My.Settings.LargeFloating
            Me.TCBehavior = My.Settings.TCBehavior
            Me.IgnoreIptcLenghtConstraints = My.Settings.IgnoreIptcLengthConstraints
            Me.IptcUtf8 = My.Settings.IptcUtf8
        End Sub
#Region "Properties"
        ''' <summary>Contains value of the <see cref="ThumbSize"/> property</summary>
        Private _ThumbSize As Size
        ''' <summary>Proxy for <see cref="My.MySettings.ThumbSize"/></summary>
        <LDisplayName("Tools.Metanol.Resources", "ThumbSize_n", GetType(SettingsProxy))> _
        <LDescription("Tools.Metanol.Resources", "ThumbSize_d", GetType(SettingsProxy))> _
        <LCategory("Tools.Metanol.Resources", "catDisplay", GetType(SettingsProxy), "Display", LCategoryAttribute.enmLookUpOrder.ResourceOnly)> _
        <DefaultValue(GetType(Size), "64,64")> _
        Public Property ThumbSize() As Size
            <DebuggerStepThrough()> Get
                Return _ThumbSize
            End Get
            <DebuggerStepThrough()> Set(ByVal value As Size)
                If value <> ThumbSize Then Changed = True
                _ThumbSize = value
            End Set
        End Property
        ''' <summary>Contains value of the <see cref="LargeFloating"/> property</summary>
        Private _LargeFloating As Boolean
        ''' <summary>Proxy for <see cref="My.MySettings.LargeFloating"/></summary>
        <LDisplayName("Tools.Metanol.Resources", "LargeFloating_n", GetType(SettingsProxy))> _
        <LDescription("Tools.Metanol.Resources", "LargeFloating_d", GetType(SettingsProxy))> _
        <LCategory("Tools.Metanol.Resources", "catDisplay", GetType(SettingsProxy), "Display", LCategoryAttribute.enmLookUpOrder.ResourceOnly)> _
        <DefaultValue(False)> _
        Public Property LargeFloating() As Boolean
            <DebuggerStepThrough()> Get
                Return _LargeFloating
            End Get
            <DebuggerStepThrough()> Set(ByVal value As Boolean)
                If value <> LargeFloating Then Changed = True
                _LargeFloating = value
            End Set
        End Property
        ''' <summary>Contains value of the <see cref="TCBehavior"/> property</summary>
        Private _TCBehavior As Boolean
        <DefaultValue(True)> _
        <LDisplayName("Tools.Metanol.Resources", "TCBehavior_n", GetType(SettingsProxy))> _
        <LDescription("Tools.Metanol.Resources", "TCBehavior_d", GetType(SettingsProxy))> _
        <LCategory("Tools.Metanol.Resources", "catBehavior", GetType(SettingsProxy), "Behavior", LCategoryAttribute.enmLookUpOrder.ResourceOnly)> _
        Public Property TCBehavior() As Boolean
            <DebuggerStepThrough()> Get
                Return _TCBehavior
            End Get
            <DebuggerStepThrough()> Set(ByVal value As Boolean)
                If value <> TCBehavior Then Changed = True
                _TCBehavior = value
            End Set
        End Property
        Private _IptcUtf8 As Boolean
        <DefaultValue(False)> _
        <LDisplayName("Tools.Metanol.Resources", "IptcUtf8_n", GetType(SettingsProxy))> _
        <LDescription("Tools.Metanol.Resources", "IptcUtf8_d", GetType(SettingsProxy))> _
        <LCategory("Tools.Metanol.Resources", "catIPTC", GetType(SettingsProxy), "IPTC", LCategoryAttribute.enmLookUpOrder.ResourceOnly)> _
        Public Property IptcUtf8 As Boolean
            <DebuggerStepThrough()> Get
                Return _IptcUtf8
            End Get
            <DebuggerStepThrough()> Set(ByVal value As Boolean)
                If value <> IptcUtf8 Then Changed = True
                _IptcUtf8 = value
            End Set
        End Property

        Private _IgnoreIptcLenghtConstraints As Boolean
        <DefaultValue(False)> _
        <LDisplayName("Tools.Metanol.Resources", "IgnoreLenghtConstraints_n", GetType(SettingsProxy))> _
        <LDescription("Tools.Metanol.Resources", "IgnoreLenghtConstraints_d", GetType(SettingsProxy))> _
        <LCategory("Tools.Metanol.Resources", "catIPTC", GetType(SettingsProxy), "IPTC", LCategoryAttribute.enmLookUpOrder.ResourceOnly)> _
        Public Property IgnoreIptcLenghtConstraints As Boolean
            <DebuggerStepThrough()> Get
                Return _IgnoreIptcLenghtConstraints
            End Get
            <DebuggerStepThrough()> Set(ByVal value As Boolean)
                If value <> IgnoreIptcLenghtConstraints Then Changed = True
                _IgnoreIptcLenghtConstraints = value
            End Set
        End Property

#End Region
        ''' <summary>Save proxy to setting and saves ssetting</summary>
        Public Sub Save()
            My.Settings.ThumbSize = Me.ThumbSize
            My.Settings.LargeFloating = Me.LargeFloating
            My.Settings.TCBehavior = Me.TCBehavior
            My.Settings.IptcUtf8 = Me.IptcUtf8
            My.Settings.IgnoreIptcLengthConstraints = Me.IgnoreIptcLenghtConstraints
            My.Settings.Save()
        End Sub
    End Class

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If proxy.Changed Then proxy.Save()
    End Sub
End Class