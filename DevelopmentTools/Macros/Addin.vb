Imports System
Imports Microsoft.VisualStudio.CommandBars
Imports Extensibility
Imports EnvDTE
Imports EnvDTE80

'Localize: This entire project

''' <summary>Represents Visual Studio add-in implemented by this assembly</summary>
''' <remarks>The add-in adds commands to Visual Studio 2011 that were created from VS 2010 macros implemented in this assembly</remarks>
''' <version version="1.5.4">This class is new in version 1.5.4</version>
<CLSCompliant(False)>
Public Class Addin
    Implements IDTExtensibility2, IDTCommandTarget

    ''' <summary>Holds instance of a <see cref="DTE"/> object this addin was initialized with</summary>
    Private application As DTE2
    ''' <summary>Holds instance of a <see cref="EnvDTE.AddIn"/> object this instance was initialized with</summary>
    Private addinInstance As EnvDTE.AddIn

    ''' <summary>CTor - creates a new instance of the <see cref="Addin"/> class</summary>
    Public Sub New()
    End Sub

    ''' <summary>Implements the <see cref="IDTExtensibility2.OnConnection">OnConnection</see> method of the <see cref="IDTExtensibility2"/> interface. Receives notification that the Add-in is being loaded.</summary>
    ''' <param name='application'>Root object of the host application.</param>
    ''' <param name='connectMode'>Describes how the Add-in is being loaded.</param>
    ''' <param name='addInInst'>Object representing this Add-in.</param>
    ''' <param name="custom">Ignored by this implementation</param>
    ''' <exception cref="ArgumentNullException"><paramref name="application"/> or <paramref name="addInInst"/> is null.</exception>
    ''' <exception cref="ArgumentException"><paramref name="application"/> cannot be casted to <see cref="EnvDTE80.DTE2"/>. -or- <paramref name="addInInst"/> cannot be casted to <see cref="EnvDTE.AddIn"/>.</exception>
    Public Sub OnConnection(ByVal application As Object, ByVal connectMode As ext_ConnectMode, ByVal addInInst As Object, ByRef custom As System.Array) Implements IDTExtensibility2.OnConnection
        If application Is Nothing Then Throw New ArgumentNullException("application")
        If addInInst Is Nothing Then Throw New ArgumentException("addInInst")
        Try
            Me.application = CType(application, EnvDTE80.DTE2)
        Catch ex As InvalidCastException
            Throw New ArgumentException("Value must be of type " & GetType(EnvDTE80.DTE2).FullName, "application", ex)
        End Try
        Try
            Me.addinInstance = CType(addInInst, EnvDTE.AddIn)
        Catch ex As InvalidCastException
            Throw New ArgumentException("Value must be of type " & GetType(EnvDTE.AddIn).FullName, "addInInst", ex)
        End Try
        If connectMode = ext_ConnectMode.ext_cm_UISetup Then
            Dim CommandObj As Command
            'Dim objCommandBar As CommandBar

            'If your command no longer appears on the appropriate command bar, or if you would like to re-create the command,
            ' close all instances of Visual Studio .NET, open a command prompt (MS-DOS window), and run the command 'devenv /setup'.
            'objCommandBar = CType(_applicationObject.Commands.AddCommandBar("MacrosXML", vsCommandBarType.vsCommandBarTypeMenu, _applicationObject.CommandBars.Item("Tools")), Microsoft.VisualStudio.CommandBars.CommandBar)

            'CommandObj = _applicationObject.Commands.AddNamedCommand(objAddIn, "GetClipboardText", "GetClipboardText", "TODO: Enter your command description", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "PasteXMLDoc", "PasteXMLDoc", "Pasetest text copied from Object Browser as XML comment", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "XMLSummary", "XMLSummary", "Creates short XML <summary>", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "XMLDoc_Long", "XMLDoc_Long", "Creates long XML <summary> and other elements", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "InheritXMLDoc", "InheritXMLDoc", "Copies XML from function current function overrides", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "Nl2Para", "Nl2Para", "Converts new lines to <para>s", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "CreateSee", "CreateSee", "Wraps idenfifier in <see cref="""">", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "CreateParamref", "CreateParamref", "Wraps parameter name in <paramref name=""""/>", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "NoComments", "NoComments", "Removes all XML comments", True, 59, Nothing, 1 + 2)  '1+2 == vsCommandStatusSupported+vsCommandStatusEnabled
            'CommandObj.AddControl(objCommandBar)

            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "EditProperties", "EditProperties", "Edits selected properties of selectd node in Solution Explorer", True, 59, Nothing, 1 + 2)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "SetLogicalName", "SetLogicalName", "Allows edit LogicalName of selected embeded resource", True, 59, Nothing, 1 + 2)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "UnsetLogicalName", "UnsetLogicalName", "Removes LogicalName of selected embeded resource", True, 59, Nothing, 1 + 2)

            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "CDefine2Enum", "CDefine2Enum", "In selected text replaces C-style #defines with enum (VB)", True, 59, Nothing, 1 + 2)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "Consts2Enum", "Consts2Enum", "In selected text replaces constants with enum (VB)", True, 59, Nothing, 1 + 2)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "EnumMerge", "EnumMerge", "In selected text merges enums (VB)", True, 59, Nothing, 1 + 2)
            CommandObj = Me.application.Commands.AddNamedCommand(addinInstance, "MsdnEnum2Enum", "MsdnEnum2Enum", "In selected text converts enumeration pasted from MSDN to VB-style enum", True, 59, Nothing, 1 + 2)
            CommandObj = Me.application.Commands.AddNamedCommand2(addinInstance, "Guid", "Guid", "Insert GUID to text", False, 1, CInt(vsCommandStatus.vsCommandStatusSupported Or vsCommandStatus.vsCommandStatusEnabled), vsCommandStyle.vsCommandStylePictAndText, vsCommandControlType.vsCommandControlTypeButton)

            'Else
            '    'If you are not using events, you may wish to remove some of these to increase performance.
            '    EnvironmentEvents.DTEEvents = CType(Me.application.Events.DTEEvents, EnvDTE.DTEEvents)
            '    EnvironmentEvents.DocumentEvents = CType(Me.application.Events.DocumentEvents(Nothing), EnvDTE.DocumentEvents)
            '    EnvironmentEvents.WindowEvents = CType(Me.application.Events.WindowEvents(Nothing), EnvDTE.WindowEvents)
            '    EnvironmentEvents.TaskListEvents = CType(Me.application.Events.TaskListEvents(""), EnvDTE.TaskListEvents)
            '    EnvironmentEvents.FindEvents = CType(Me.application.Events.FindEvents, EnvDTE.FindEvents)
            '    EnvironmentEvents.OutputWindowEvents = CType(Me.application.Events.OutputWindowEvents(""), EnvDTE.OutputWindowEvents)
            '    EnvironmentEvents.SelectionEvents = CType(Me.application.Events.SelectionEvents, EnvDTE.SelectionEvents)
            '    EnvironmentEvents.SolutionItemsEvents = CType(Me.application.Events.SolutionItemsEvents, EnvDTE.ProjectItemsEvents)
            '    EnvironmentEvents.MiscFilesEvents = CType(Me.application.Events.MiscFilesEvents, EnvDTE.ProjectItemsEvents)
            '    EnvironmentEvents.DebuggerEvents = CType(Me.application.Events.DebuggerEvents, EnvDTE.DebuggerEvents)
        End If
    End Sub

    '''<summary>Implements the <see cref="IDTExtensibility2.OnDisconnection">OnDisconnection</see> method of the <see cref="IDTExtensibility2"/> interface. Receives notification that the Add-in is being unloaded.</summary>
    '''<param name='disconnectMode'>Describes how the Add-in is being unloaded.</param>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    Public Sub OnDisconnection(ByVal disconnectMode As ext_DisconnectMode, ByRef custom As Array) Implements IDTExtensibility2.OnDisconnection
    End Sub

    '''<summary>Implements the <see cref="IDTExtensibility2.OnAddInsUpdate">OnAddInsUpdate</see> method of the <see cref="IDTExtensibility2"/> interface. Receives notification that the collection of Add-ins has changed.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    Public Sub OnAddInsUpdate(ByRef custom As Array) Implements IDTExtensibility2.OnAddInsUpdate
    End Sub

    '''<summary>Implements the <see cref="IDTExtensibility2.OnStartupComplete">OnStartupComplete</see> method of the <see cref="IDTExtensibility2"/> interface. Receives notification that the host application has completed loading.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    '''<remarks></remarks>
    Public Sub OnStartupComplete(ByRef custom As Array) Implements IDTExtensibility2.OnStartupComplete
    End Sub

    '''<summary>Implements the <see cref="IDTExtensibility2.OnBeginShutdown">OnBeginShutdown</see> method of the <see cref="IDTExtensibility2"/> interface. Receives notification that the host application is being unloaded.</summary>
    '''<param name='custom'>Array of parameters that are host application specific.</param>
    Public Sub OnBeginShutdown(ByRef custom As Array) Implements IDTExtensibility2.OnBeginShutdown
    End Sub

    ''' <summary>Prefix of adddin name - it's name of type <see cref="Addin"/></summary>
    Private Shared ReadOnly addinPrefix$ = GetType(Addin).FullName

    '''<summary>Implements the <see cref="IDTCommandTarget.QueryStatus">QueryStatus</see> method of the <see cref="IDTCommandTarget"/> interface. This is called when the command's availability is updated</summary>
    '''<param name='commandName'>The name of the command to determine state for.</param>
    '''<param name='neededText'>Text that is needed for the command.</param>
    '''<param name='status'>The state of the command in the user interface.</param>
    '''<param name='commandText'>Text requested by the neededText parameter.</param>
    Public Sub QueryStatus(ByVal commandName As String, ByVal neededText As vsCommandStatusTextWanted, ByRef status As vsCommandStatus, ByRef commandText As Object) Implements IDTCommandTarget.QueryStatus
        status = vsCommandStatus.vsCommandStatusUnsupported
        If neededText = EnvDTE.vsCommandStatusTextWanted.vsCommandStatusTextWantedNone Then
            Select Case commandName
                Case addinPrefix & ".PasteXMLDoc" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".XMLSummary" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".XMLDoc_Long" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".InheritXMLDoc" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".Nl2Para" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".CreateSee" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".CreateParamref" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".NoComments" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".NoComments" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported

                Case addinPrefix & ".EditProperties" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".SetLogicalName" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".UnsetLogicalName" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported

                Case addinPrefix & ".CDefine2Enum" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".Consts2Enum" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".EnumMerge" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".MsdnEnum2Enum" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
                Case addinPrefix & ".Guid" : status = vsCommandStatus.vsCommandStatusEnabled Or vsCommandStatus.vsCommandStatusSupported
            End Select
        End If
    End Sub

    '''<summary>Implements the <see cref="IDTCommandTarget.Exec">Exec</see> method of the <see cref="IDTCommandTarget"/> interface. This is called when the command is invoked.</summary>
    '''<param name='commandName'>The name of the command to execute.</param>
    '''<param name='executeOption'>Describes how the command should be run.</param>
    '''<param name='varIn'>Parameters passed from the caller to the command handler.</param>
    '''<param name='varOut'>Parameters passed from the command handler to the caller.</param>
    '''<param name='handled'>Informs the caller if the command was handled or not.</param>
    Public Sub Exec(ByVal commandName As String, ByVal executeOption As vsCommandExecOption, ByRef varIn As Object, ByRef varOut As Object, ByRef handled As Boolean) Implements IDTCommandTarget.Exec
        handled = False
        If executeOption = vsCommandExecOption.vsCommandExecOptionDoDefault Then
            DteHelper.DTE = application
            handled = True
            Try
                Select Case commandName
                    Case addinPrefix & ".PasteXMLDoc" : XMLDoc.PasteXMLDoc()
                    Case addinPrefix & ".XMLSummary" : XMLDoc.XMLSummary()
                    Case addinPrefix & ".XMLDoc_Long" : XMLDoc.XMLDoc_Long()
                    Case addinPrefix & ".InheritXMLDoc" : XMLDoc.InheritXMLDoc()
                    Case addinPrefix & ".Nl2Para" : XMLDoc.Nl2Para()
                    Case addinPrefix & ".CreateSee" : XMLDoc.CreateSee()
                    Case addinPrefix & ".CreateParamref" : XMLDoc.CreateParamref()
                    Case addinPrefix & ".NoComments" : XMLDoc.NoComments()

                    Case addinPrefix & ".EditProperties" : ItemProperties.EditProperties()
                    Case addinPrefix & ".SetLogicalName" : ItemProperties.SetLogicalName()
                    Case addinPrefix & ".UnsetLogicalName" : ItemProperties.UnsetLogicalName()

                    Case addinPrefix & ".CDefine2Enum" : VisualBasic.CDefine2Enum()
                    Case addinPrefix & ".Consts2Enum" : VisualBasic.Consts2Enum()
                    Case addinPrefix & ".EnumMerge" : VisualBasic.EnumMerge()
                    Case addinPrefix & ".MsdnEnum2Enum" : VisualBasic.MsdnEnum2Enum()
                    Case addinPrefix & ".Guid" : Misc.Guid()

                    Case Else : handled = False
                End Select
            Catch ex As Exception
                MsgBox("Error executing command " & commandName & ":" & vbCrLf & ex.Message, MsgBoxStyle.Exclamation, ex.GetType.Name)
            End Try
        End If
    End Sub

End Class
