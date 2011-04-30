#pragma once

#include "..\Plugin\fsplugin.h"
#include "..\Common.h"
#include "..\ContentPluginBase.h"
#include "WFX common.h"
#include "..\Attributes.h"

namespace Tools{namespace TotalCommanderT{
    using namespace System;
    using namespace System::ComponentModel;
    using namespace System::Collections::Generic;

    ref class ProgressProcWrapper;
    ref class LogProcWrapper;
    ref class RequestProcWrapper;
    ref class CryptProcWrapper;

    /// <summary>Abstract base class for Total Commander file-system plugins (wfx)</summary>
    /// <remarks>See <see2 cref2="T:Tools.TotalCommanderT.PluginBuilder.Generator"/> for more information about how to generate Total Commander plugin from .NET</remarks>
    /// <version version="1.5.4">Added support for crypro (Total Commander secure password store)</version>
    public ref class FileSystemPlugin abstract : ContentPluginBase {
    protected:
        FileSystemPlugin();
    private:
        /// <summary>Holds plugin IDs and instances of all registered plugins</summary>
        static Dictionary<int, FileSystemPlugin^>^ registeredPlugins = gcnew Dictionary<int, FileSystemPlugin^>();
    public:
        /// <summary>Gets plugin by number</summary>
        /// <param name="pluginNr">Number of plugin as passed to <see cref="FsInit"/></param>
        /// <returns>Instance of file system plugin registered with given number, null if no plugin was registered under given number</returns>
        /// <remarks>When used from Total Commander only one plugin is registered per application domain</remarks>
        /// <version version="1.5.4">This function is new in version 1.5.4</version>
        [EditorBrowsable(EditorBrowsableState::Advanced)]
        static FileSystemPlugin^ GetPluginByNumber(int pluginNr);

#pragma region TC functions (required)
    private:
        ProgressProcWrapper^ progressProc;
        LogProcWrapper^ logProc;
        RequestProcWrapper^ requestProc;
        CryptProcWrapper^ cryptProc;
    public:
        /// <summary>Called when loading the plugin. The passed values should be stored in the plugin for later use. (Unicode implementation)</summary>
        /// <param name="PluginNr">Internal number this plugin was given in Total Commander. Has to be passed as the first parameter in all callback functions so Totalcmd knows which plugin has sent the request.</param>
        /// <param name="pProgressProc">Pointer to the progress callback function.</param>
        /// <param name="pLogProc">Pointer to the logging function</param>
        /// <param name="pRequestProc">Pointer to the request text proc</param>
        /// <returns>The return value is currently unused. You should return 0 when successful.</returns>
        /// <remarks><see cref="FsInit"/> is NOT called when the user initially installs the plugin. Only <se cref="FsGetDefRootName"/>.is called in this case, and then the plugin DLL is unloaded again. The plugin DLL is loaded when the user enters the plugin root in Network Neighborhood.
        /// <para>This function is called by Total Commander and is not intended for direct use. If you need use plugin outside of Total Commander use <see cref="InitializePlugin"/> instead.</para>
        /// <para>This plugin function is implemented by <see cref="OnInit"/>.</para>
        /// <para>This function has two implementations (overloads) - <see cref="FsInit(int,tProgressProc,tLogProc,tRequestProc)">ANSI</see> and <see cref="FsInit(int,tProgressProcW,tLogProcW,tRequestProcW)">Unicode</see>. Use of <see cref="FsInit(int,tProgressProcW,tLogProcW,tRequestProcW)">Unicode</see> is preffered.</para></remarks>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is true</exception>
        /// <version version="1.5.4">This overload is new in version 1.5.4</version>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("TC_FS_INIT")]
        int FsInit(int PluginNr, tProgressProcW pProgressProc, tLogProcW pLogProc, tRequestProcW pRequestProc);
        /// <summary>Called when loading the plugin. The passed values should be stored in the plugin for later use. (ANSI implementation)</summary>
        /// <param name="PluginNr">Internal number this plugin was given in Total Commander. Has to be passed as the first parameter in all callback functions so Totalcmd knows which plugin has sent the request.</param>
        /// <param name="pProgressProc">Pointer to the progress callback function.</param>
        /// <param name="pLogProc">Pointer to the logging function</param>
        /// <param name="pRequestProc">Pointer to the request text proc</param>
        /// <returns>The return value is currently unused. You should return 0 when successful.</returns>
        /// <remarks><see cref="FsInit"/> is NOT called when the user initially installs the plugin. Only <se cref="FsGetDefRootName"/>.is called in this case, and then the plugin DLL is unloaded again. The plugin DLL is loaded when the user enters the plugin root in Network Neighborhood.
        /// <para>This function is called by Total Commander and is not intended for direct use. If you need use plugin outside of Total Commander use <see cref="InitializePlugin"/> instead.</para>
        /// <para>This plugin function is implemented by <see cref="OnInit"/>.</para>
        /// <para>This function has two implementations (overloads) - <see cref="FsInit(int,tProgressProc,tLogProc,tRequestProc)">ANSI</see> and <see cref="FsInit(int,tProgressProcW,tLogProcW,tRequestProcW)">Unicode</see>. Use of <see cref="FsInit(int,tProgressProcW,tLogProcW,tRequestProcW)">Unicode</see> is preffered.</para></remarks>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is true</exception>
        /// <version version="1.5.4">Added <see cref="ObsoleteAttribute"/></version>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false), Obsolete("This is ANSI function. Use Unicode overload instead")]
        [PluginMethod("TC_FS_INIT")]
        int FsInit(int PluginNr, tProgressProc pProgressProc, tLogProc pLogProc, tRequestProc pRequestProc);
        /// <summary>Called when loading the plugin outside of Total Comander environment instead of <see cref="FsInit"/>. The passed values should be stored in the plugin for later use.</summary>
        /// <param name="PluginNr">Internal number this plugin was given in Total Commander. Has to be passed as the first parameter in all callback functions so Totalcmd knows which plugin has sent the request.</param>
        /// <param name="progress">Delegate to the progress callback function.</param>
        /// <param name="log">Delegate to the logging function</param>
        /// <param name="request">Delegate to the request text proc</param>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is true</exception>
        /// <exception cref="ArgumentNullException"><paramref name="progress"/>, <paramref name="log"/> or <paramref name="request"/> is null</exception>
        /// <remarks>Use this function to initialize the plugin when used outside of Total Commander.
        /// <para>This plugin function is implemented by <see cref="OnInit"/>.</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        void InitializePlugin(int PluginNr, ProgressCallback^ progress, LogCallback^ log, RequestCallback^ request);
        ///// <summary>When plugin is initialized, gets value indicating if it was initialiuzed by Total Commander or .NET application</summary>
        ///// <returns>True if plugin was initialized by Total Commander; false when it was initialized by .NET application</returns>
        ///// <exception cref="InvalidOperationException"><see cref="Initialized"/> is false</exception>
        //[EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        //property bool IsInTotalCommander{bool get();}

        /// <summary>Called to retrieve the first file in a directory of the plugin's file system.</summary>
        /// <param name="Path">Full path to the directory for which the directory listing has to be retrieved. Important: no wildcards are passed to the plugin! All separators will be backslashes, so you will need to convert them to forward slashes if your file system uses them!
        /// <para>As root, a single backslash is passed to the plugin. The root items appear in the plugin base directory retrieved by <see cref="FsGetDefRootName"/> at installation time. This default root name is NOT part of the path passed to the plugin!</para>
        /// <para>All subdirs are built from the directory names the plugin returns through <see cref="FsFindFirst"/> and <see cref="FsFindNext"/>, separated by single backslashes, e.g. \Some server\c:\subdir</para></param>
        /// <param name="FindData">A standard <see cref="WIN32_FIND_DATA"/> struct as defined in the Windows SDK, which contains the file or directory details. Use the dwFileAttributes field set to <see2 cref2="F:Tools.TotalCommanderT.FileAttributes.Directory"/> to distinguish files from directories. On Unix systems, you can | (or) the dwFileAttributes field with 0x80000000 and set the dwReserved0 parameter to the Unix file mode (permissions).</param>
        /// <returns>Return INVALID_HANDLE_VALUE (==-1, not zero!) if an error occurs, or a number of your choice if not. It is recommended to pass a pointer to an internal structure as this handle, which stores the current state of the search. This will allow recursive directory searches needed for copying whole trees. This handle will be passed to <see cref="FsFindNext"/> by the calling program.
        /// <para>When an error occurs, call <see cref="SetLastError"/> to set the reason of the error. Total Commander checks for the following two errors:</para>
        /// <list type="numbered"><item>ERROR_NO_MORE_FILES: The directory exists, but it's empty (Totalcmd can open it, e.g. to copy files to it)</item>
        /// <item>Any other error: The directory does not exist, and Total Commander will not try to open it.</item></list></returns>
        /// <remarks><see cref="FsFindFirst"/> may be called directly with a subdirectory of the plugin! You cannot rely on it being called with the root \ after it is loaded. Reason: Users may have saved a subdirectory to the plugin in the Ctrl+D directory hotlist in a previous session with the plugin.
        /// <para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("TC_FS_FINDFIRST")]
        HANDLE FsFindFirst(char* Path,WIN32_FIND_DATA *FindData);
        /// <summary>Called to retrieve the next file in a directory of the plugin's file system</summary>
        /// <param name="Hdl">The find handle returned by <see cref="FsFindFirst"/>.</param>
        /// <param name="FindData">A standard <see cref="WIN32_FIND_DATA"/> struct as defined in the Windows SDK, which contains the file or directory details. Use the dwFileAttributes field set to <see2 cref2="F:Tools.TotalCommanderT.FileAttributes.Directory"/> to distinguish files from directories. On Unix systems, you can | (or) the dwFileAttributes field with 0x80000000 and set the dwReserved0 parameter to the Unix file mode (permissions).</param>
        /// <returns>Return FALSE if an error occurs or if there are no more files, and TRUE otherwise. <see cref="SetLastError"/>() does not need to be called.</returns>
        /// <remarks><para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("TC_FS_FINDNEXT")]
        BOOL FsFindNext(HANDLE Hdl,WIN32_FIND_DATA *FindData);
        /// <summary>Called to end a <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> loop, either after retrieving all files, or when the user aborts it</summary>
        /// <param name="Hdl">The find handle returned by <see cref="FsFindFirst"/>.</param>
        /// <returns>Currently unused, should return 0.</returns>
        /// <remarks><para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("TC_FS_FINDCLOSE")]
        int FsFindClose(HANDLE Hdl);
#pragma endregion

#pragma region .NET Functions (required)
    private:
        /// <summary>Contains value of the <see cref="Initialized"/> property</summary>
        bool initialized;
        /// <summary>Contains value of the <see cref="PluginNr"/> property</summary>
        int pluginNr;
    public:
        /// <summary>Gets plugin number this plugin instance is recognized by Total Commender under</summary>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is false</exception>
        /// <version version="1.5.4">Accessibility changed from <see langword="protected"/> to <see langword="public"/></version>
        property int PluginNr{int get();}
    public:
        /// <summary>Gets value indicating if this plugin instance was initialized or not</summary>
        property bool Initialized{bool get();}
    protected:
        /// <summary>When overriden in derived class provides custom code invoked when plugin is initialized.</summary>
        /// <remarks>When this method is called the <see cref="Initialized"/> property has value true and <see cref="PluginNr"/> is already set.
        /// <para>Default implementation of this method does nothing.</para>
        /// <para>This method implements plugin function <see cref="FsInit"/> (alternatively <see cref="InitializePlugin"/>)</para></remarks>
        virtual void OnInit();

#pragma region Callbacks
        /// <summary>Callback function, which the plugin can call to show copy progress.</summary>
        /// <param name="SourceName">Name of the source file being copied. Depending on the direction of the operation (Get, Put), this may be a local file name of a name in the plugin file system.</param>
        /// <param name="TargetName">Name to which the file is copied.</param>
        /// <param name="PercentDone">Percentage of THIS file being copied. Total Commander automatically shows a second percent bar if possible when multiple files are copied.</param>
        /// <returns>Total Commander returns <c>true</c> if the user wants to abort copying, and <c>false</c> if the operation can continue.</returns>
        /// <remarks>You should call this function at least twice in the copy functions <see cref="GetFile"/>, <see cref="PutFile"/> and <see cref="RenMovFile"/>, at the beginning and at the end. If you can't determine the progress, call it with 0% at the beginning and 100% at the end.
        /// <para>During the <see cref="FindFirst"/>/<see cref="FindNext"/>/<see cref="FindClose"/> loop, the plugin may now call the <see cref="ProgressProc"/> to make a progess dialog appear. This is useful for very slow connections. Don't call <see cref="ProgressProc"/> for fast connections! The progress dialog will only be shown for normal dir changes, not for compound operations like get/put. The calls to <see cref="ProgressProc"/> will also be ignored during the first 5 seconds, so the user isn't bothered with a progress dialog on every dir change.</para></remarks>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is false</exception>
        /// <version version="1.5.4">The function now supports Unicode</version>
        bool ProgressProc(String^ SourceName, String^ TargetName,int PercentDone);
        /// <summary>Callback function, which the plugin can call to show the FTP connections toolbar, and to pass log messages to it. Totalcmd can show these messages in the log window (ftp toolbar) and write them to a log file.</summary>
        /// <param name="MsgType">Can be one of the <see cref="LogKind"/> flags</param>
        /// <param name="LogString">String which should be logged.
        /// <para>When <paramref name="MsgType"/>is <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/>, the string MUST have a specific format:</para>
        /// <para><c>"CONNECT"</c> followed by a single whitespace, then the root of the file system which was connected, without trailing backslash. Example: <c>CONNECT \Filesystem</c></para>
        /// <para>When <paramref name="MsgType"/> is <see2 cref2="F:Tools.TotalCommanderT.LogKind.TransferComplete"/>, this parameter should contain both the source and target names, separated by an arrow <c>" -> "</c>, e.g. <c>Download complete: \Filesystem\dir1\file1.txt -> c:\localdir\file1.txt</c></para></param>
        /// <remarks>Do NOT call <see cref="LogProc"/> with <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/> if your plugin does not require connect/disconnect! If you call it with <paramref name="MsgType"/> <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/>, the function <see cref="Disconnect"/> will be called (if defined) when the user presses the Disconnect button.</remarks>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is false</exception>
        /// <version version="1.5.4">The method now supports Unicode</version>
        void LogProc(LogKind MsgType,String^ LogString);
        /// <summary>Specialized version of the <see cref="LogProc"/> function used for logging conection open.</summary>
        /// <param name="FileSystem">Name of the filesystem plugin has connected to. The name must start with a backslash.</param>
        /// <remarks>Do not call this function when file system implemeneted by the plugin does not reguire connection</remarks>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is false</exception>
        /// <exception cref="ArgumentNullException"><paramref name="FileSystem"/> is null</exception>
        /// <exception cref="ArgumentException"><paramref name="FileSystem"/> either starts with \\ or it does not start with \</exception>
        void LogProcConnect(String^ FileSystem);
        /// <summary>Specialized version of the <see cref="LogProc"/> function used for transfer completion advertisement.</summary>
        /// <param name="Source">Path of source file. It can be either file in plugin file system (address staring with "\") or it can be file in TC file system (address like C:\ or starting with \\).</param>
        /// <param name="Target">Path of destination file.  It can be either file in plugin file system (address staring with "\") or it can be file in TC file system (address like C:\ or starting with \\).</param>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is false</exception>
        /// <exception cref="ArgumentNullException"><paramref name="Source"/> or <paramref name="Target"/> is null</exception>
        void LogProcTransferComplete(String^ Source, String^ Target);
        /// <summary>callback function, which the plugin can call to request input from the user. When using one of the standard parameters, the request will be in the selected language.</summary>
        /// <param name="RequestType">Can be one of the <see cref="InputRequestKind"/> flags</param>
        /// <param name="CustomTitle">Custom title for the dialog box. If NULL or empty, it will be "Total Commander"</param>
        /// <param name="CustomText">Override the text defined with <paramref name="RequestType"/>. Set this to NULL or an empty string to use the default text. The default text will be translated to the language set in the calling program.</param>
        /// <param name="DefaultText">This string contains the default text presented to the user. Set <paramref name="DefaultText"/>[0]=0 to have no default text.</param>
        /// <param name="maxlen">Maximum length allowed for returned text.</param>
        /// <returns>User-entered text if user clicked Yes or OK. Null otherwise</returns>
        /// <remarks>Leave <paramref name="CustomText"/> empty if you want to use the (translated) default strings!</remarks>
        /// <exception cref="InvalidOperationException"><see cref="Initialized"/> is false</exception>
        /// <exception cref="ArgumentException"><paramref name="DefaultText"/> is longer than <paramref name="maxlen"/></exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxlen"/> is less than 1</exception>
        /// <version version="1.5.4">The function now supports Unicode</version>
        String^ RequestProc(InputRequestKind RequestType,String^ CustomTitle, String^ CustomText, String^ DefaultText, int maxlen);

#pragma region Crypto
    protected:
        /// <summary>Performs a cryptography operation using Total Commander</summary>
        /// <param name="mode">Then mode of operation</param>
        /// <param name="connectionName">Name of the connection for this operation</param>
        /// <param name="password">Operation-specific, usually the password to be stored, or the target connection when copying/moving a connection</param>
        /// <returns>Password retrieved. Only when <paramref name="mode"/> is <see cref2="F:Tools.TotalCommanderT.CryptMode.LoadPassword"/> or <see cref="F:Tools.TotalCommanderT.CryptMode.LoadPasswordNoUI"/>. Otherwise returns <paramref name="password"/>.</returns>
        /// <exception cref="CryptException">Crypto operation failed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is false (i.e. either current version of Total Commander or plugin implementation does not support crypto).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionName"/> is null</exception>
        /// <remarks>This is lower-level function which calls directly Total Commander callback. It's preffered to use other crypto functions instead.</remarks>
        /// <sealso cref="SavePassword"/><sealso cref="LoadPassword"/><sealso cref="MovePassword"/><sealso cref="DeletePassword"/>
        /// <version version="1.5.4">This function is new in version 1.5.4</version>
        [EditorBrowsable(EditorBrowsableState::Advanced)]
        String^ CryptProc(CryptMode mode, String^ connectionName, String^ password, int maxlen);
        /// <summary>Saves a password for given connection is Total Commander secure password store</summary>
        /// <param name="connectioName">Name of the connection to save password for</param>
        /// <param name="password">The password to be saved</param>
        /// <exception cref="CryptException">Crypto operation failed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is false (i.e. either current version of Total Commander or plugin implementation does not support crypto).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionName"/> or <paramref name="password"/> is null</exception>
        /// <version version="1.5.4">This function is new in version 1.5.4</version>
        void SavePassword(String^ connectioName, String^ password);
        /// <summary>Loads a password form Total Commander secure password store for given connection</summary>
        /// <param name="connectionName">Name of the connection to load password for</param>
        /// <param name="showUI">True to ask user for master password, false to load password only when master password was already enetered</param>
        /// <exception cref="CryptException">Crypto operation failed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is false (i.e. either current version of Total Commander or plugin implementation does not support crypto).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionName"/> is null</exception>
        /// <remarks>
        /// When showing details of an existing connection this function should be first called with <paramref name="showUI"/> false.
        /// In case ith throws an <see cref="CryptException"/> with <see cref="CryptException::Reason"/> <see cref="CryptResult::NoMasterPassword"/> show a button "Edit password".
        /// Only call this function with <paramref name="showUI"/> true when user clicks that button, or tries to connect.
        /// This way the user does not have to enter the master password if he just wanted to make some other changes to the connection settings.
        /// </remarks>
        /// <version version="1.5.4">This function is new in version 1.5.4</version>
        String^ LoadPassword(String^ connectionName, bool showUI);
        /// <summary>Copies or moves the password form one connection in Total Commander safe password store to the other</summary>
        /// <param name="sourceConnectionName">Name of the source connection to take a password from</param>
        /// <param name="targetConnectionName">Name of the target connection to copy/move connection to</param>
        /// <param name="deleteOriginal">True to move the password, false to copy it</param>
        /// <exception cref="CryptException">Crypto operation failed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is false (i.e. either current version of Total Commander or plugin implementation does not support crypto).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="sourceConnectionName"/> or <paramref name="targetConnectionName"/> is null</exception>
        /// <version version="1.5.4">This function is new in version 1.5.4</version>
        void MovePassword(String^ sourceConnectionName, String^ targetConnectionName, bool deleteOriginal);
        /// <summary>Deletes a passowrd stored for given connection in Total Commander safe password store</summary>
        /// <param name="connectionName">Name of connection to delete the password of</param>
        /// <exception cref="CryptException">Crypto operation failed.</exception>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is false (i.e. either current version of Total Commander or plugin implementation does not support crypto).</exception>
        /// <exception cref="ArgumentNullException"><paramref name="connectionName"/> is null</exception>
        /// <version version="1.5.4">This function is new in version 1.5.4</version>
        void DeletePassword(String^ connectionName);
#pragma endregion
#pragma endregion

    private:
        /// <summary>Contains handle-object dictionary for objects returned by <see cref="FindFirst"/> and <see cref="FindNext"/></summary>
        Collections::Generic::Dictionary<int,Object^>^ handleDictionary ;
        /// <summary>Contains maximum key in <see cref="HandleDictionary"/></summary>
        int MaxHandle;
        /// <summary>Used to synchronize access to <see cref="HandleDictionary"/></summary>
        Object^ HandleSyncObj;
    protected:
        /// <summary>Gets dictionary containing objects referenced by Total Commander by handles</summary>
        /// <remarks>Do not add/remove items form this collection directly. Use dedicated Handle* functions instead (they are thread-safe).
        /// <para>In rare occasions, your plugin is not utilized by Total Commander but by another .NET-based application, you cannot rely on objects being passed to <see cref="FindNext"/>/<see cref="FindClose"/> being present in this dictionary and object returned by <se cref="FindFirst"/> being added to this dictionary. It's because .NET application doesn't need to rely on integer handles - it can store objects itself.</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        property Collections::Generic::Dictionary<int,Object^>^ HandleDictionary{Collections::Generic::Dictionary<int,Object^>^ get();}
        /// <summary>Gets next free handle for <see cref="HandleDictionary"/></summary>
        /// <returns>Always returns value greater than zero</returns>
        /// <threadsafety>This function is tread-safe</threadsafety>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        int GetNextHandle();
        /// <summary>Adds object to <see cref="HandleDictionary"/></summary>
        /// <param name="object">Object to add and obtain handle for</param>
        /// <returns>Handle assigned to object</returns>
        /// <remarks>You can assign multiple handles to same object.</remarks>
        /// <threadsafety>This function is tread-safe</threadsafety>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        int HandleAdd(Object^ object);
        /// <summary>Removes object from <see cref="HandleDictionary"/></summary>
        /// <param name="object">Object to be removed</param>
        /// <returns>True if object was present in <see cref="HandleDictionary"/> and it was removed; false if it was not present</returns>
        /// <remarks>If object has multiple handles assigned only the first handle is destroyed</remarks>
        /// <threadsafety>This function is tread-safe</threadsafety>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        bool HandleRemove(Object^ object);
        /// <summary>Removes object from <see cref="HandleDictionary"/> identified by integral handle</summary>
        /// <param name="handle">Handle of object to be removed</param>
        /// <returns>Ture if <paramref name="handle"/> was defined and it was removed; false if <paramref name="handle"/> was not defined (i.e. it was previously destroyed or never created)</returns>
        /// <threadsafety>This function is tread-safe</threadsafety>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        bool HandleRemove(int handle);
        /// <summary>Gets object identified by handle</summary>
        /// <param name="handle">Handle to get object for</param>
        /// <returns>The object stored in <see cref="HandleDictionary"/> under key <paramref name="handle"/>; null where <paramref name="handle"/> is nod defined.</returns>
        /// <remarks>In case you store null objects in <see cref="HandleDictionary"/> this function returns null either when <paramref name="handle"/> is invalid or objects stored under the handle is null. So, do not store null objects in <ses cref="HandleDictionary"/>.</remarks>
        /// <threadsafety>This function is tread-safe</threadsafety>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        Object^ HandleGet(int handle);
        /// <summary>Gets handle of object in <see cref="HandleDictionary"/></summary>
        /// <param name="object">Object to get handle of</param>
        /// <returns>Handle of <paramref name="object"/>; of -1 if <paramref name="object"/> is not present in <see cref="HandleDictionary"/></returns>
        /// <threadsafety>This function is tread-safe</threadsafety>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        int HandleGetHandle(Object^ object);
        /// <summary>Raplaces object in <see cref="HandleDictionary"/> with another one.</summary>
        /// <param name="handle">Handle to replace object for</param>
        /// <param name="object">New object to store with handle <paramref name="handle"/></param>
        /// <exception cref="System::Collections::Generic::KeyNotFoundException"><paramref name="handle"/> is nod defined</exception>
        /// <threadsafety>This function is tread-safe</threadsafety>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        void HandleReplace(int handle, Object^ object);
    public:
        /// <summary>When overriden in derived class retrieves the first file in a directory of the plugin's file system.</summary>
        /// <param name="Path">Full path to the directory for which the directory listing has to be retrieved. Important: no wildcards are passed to the plugin! All separators will be backslashes, so you will need to convert them to forward slashes if your file system uses them!
        /// <para>As root, a single backslash is passed to the plugin. The root items appear in the plugin base directory retrieved by <see cref="Name"/> at installation time. This default root name is NOT part of the path passed to the plugin!</para>
        /// <para>All subdirs are built from the directory names the plugin returns through <see cref="FindFirst"/> and <see cref="FindNext"/>, separated by single backslashes, e.g. \Some server\c:\subdir</para></param>
        /// <param name="FindData">A <see cref="FindData"/> struct (mimics WIN32_FIND_DATA as defined in the Windows SDK) to be pupulated with the file or directory details. Use the <see cref="FindData::Attributes"/> field set to <see2 cref2="F:Tools.TotalCommanderT.FileAttributes.Directory"/> to distinguish files from directories. On Unix systems, you can | (or) the <see cref="FindData::Attributes"/> field with 0x80000000 and set the <see cref="FindData::ReparsePointTag"/> parameter to the Unix file mode (permissions).</param>
        /// <returns>Any object. It is recommended to return object that represents current state of the search. This will allow recursive directory searches needed for copying whole trees. This object will be passed to <see cref="FindNext"/> by the calling program.
        /// Returned object is added to <see cref="HandleDictionary"/>
        /// <para>Null if there are no more files.</para></returns>
        /// <exception cref="IO::DirectoryNotFoundException">Directory does not exists</exception>
        /// <exception cref="UnauthorizedAccessException">The user does not have access to the directory</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">Another error occured</exception>
        /// <remarks><see cref="FindFirst"/> may be called directly with a subdirectory of the plugin! You cannot rely on it being called with the root \ after it is loaded. Reason: Users may have saved a subdirectory to the plugin in the Ctrl+D directory hotlist in a previous session with the plugin.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        virtual Object^ FindFirst(String^ Path,FindData% FindData) abstract;
        /// <summary>When overriden in derived class retrieves the next file in a directory of the plugin's file system</summary>
        /// <param name="Status">The object returned by <see cref="FindFirst"/>; null when Total Commander supplied handle that is  not in <see cref="HandleDictionary"/></param>
        /// <param name="FindData">A <see cref="FindData"/> struct (mimics WIN32_FIND_DATA as defined in the Windows SDK) to be pupulated with the file or directory details. Use the <see cref="FindData::Attributes"/> field set to <see2 cref2="F:Tools.TotalCommanderT.FileAttributes.Directory"/> to distinguish files from directories. On Unix systems, you can | (or) the <see cref="FindData::Attributes"/> field with 0x80000000 and set the <see cref="FindData::ReparsePointTag"/> parameter to the Unix file mode (permissions).</param>
        /// <returns>Return false if there are no more files, and true otherwise. <see cref="SetLastError"/>() does not need to be called.</returns>
        /// <exception cref="IO::DirectoryNotFoundException">Directory does not exists</exception>
        /// <exception cref="UnauthorizedAccessException">The user does not have access to the directory</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">Another error occured</exception>
        /// <remarks><note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        virtual bool FindNext(Object^ Status, FindData% FindData) abstract;
        /// <summary>When overriden in derived class performs custom clenup at end of a <see cref="FindFirst"/>/<see cref="FindNext"/> loop, either after retrieving all files, or when the user aborts it.</summary>
        /// <param name="Status">The object returned by <see cref="FindFirst"/>; null when Total Commander supplied handle that is  not in <see cref="HandleDictionary"/>. When this function exists, <paramref name="Status"/> automatically removed from <see cref="HandleDictionary"/></param>
        virtual void FindClose(Object^ Status);
#pragma endregion

#pragma region Optional methods
#pragma region Crypt
    private:
        int cryptoNr;
        bool cryptInitialized;
    public:
        /// <summary>Called when loading the plugin. The passed values should be stored in the plugin for later use. This function is only needed if you want to use the secure password store in Total Commander. (ANSI implementation)</summary>
        /// <param name="pCryptProc">Pointer to the crypto callback function. See <see cref="tCryptProc"/> for a description of this function</param>
        /// <param name="cryptoNr">A parameter which needs to be passed to the callback function</param>
        /// <param name="flags">Flags regarding the crypto connection. <see cref="CryptFlags"/></param>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is true</exception>
        /// <exception cref="NotSupportedException">Most-derived implementation of</exception>
        /// <remarks><para>This method is called by Total Commander and is not intended for direct use</para>
        /// This plugin function is implemented by <see cref="OnInitializeCryptography"/> is decorated with <see cref="MethodNotSupportedAttribute"/>.
        /// <para>This function has two implementations - <see cref="FsSetCryptCallback">ANSI</see> and <see cref="FsSetCryptCallbackW">Unicode</see> Use of <see cref="FsSetCryptCallbackW">Unicode</see> is preffered..</para></remarks>
        /// <version version="1.5.4">This method is new in 1.5.4</version>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false), Obsolete("This is ANSI function. Use Unicode overload instead")]
        [PluginMethod("OnInitializeCryptography","TC_FS_SETCRYPTCALLBACK")]
        void FsSetCryptCallback(tCryptProc pCryptProc, int cryptoNr, int flags);
        /// <summary>Called when loading the plugin. The passed values should be stored in the plugin for later use. This function is only needed if you want to use the secure password store in Total Commander. (Unicode implementation)</summary>
        /// <param name="pCryptProc">Pointer to the crypto callback function. See <see cref="tCryptProc"/> for a description of this function</param>
        /// <param name="cryptoNr">A parameter which needs to be passed to the callback function</param>
        /// <param name="flags">Flags regarding the crypto connection. <see cref="CryptFlags"/></param>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is true</exception>
        /// <exception cref="NotSupportedException">Most-derived implementation of</exception>
        /// <remarks><para>This method is called by Total Commander and is not intended for direct use</para>
        /// This plugin function is implemented by <see cref="OnInitializeCryptography"/> is decorated with <see cref="MethodNotSupportedAttribute"/>.
        /// <para>This function has two implementations - <see cref="FsSetCryptCallback">ANSI</see> and <see cref="FsSetCryptCallbackW">Unicode</see> Use of <see cref="FsSetCryptCallbackW">Unicode</see> is preffered..</para></remarks>
        /// <version version="1.5.4">This method is new in 1.5.4</version>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("OnInitializeCryptography","TC_FS_SETCRYPTCALLBACK")]
        void FsSetCryptCallbackW(tCryptProcW pCryptProc, int cryptoNr, int flags);
        
        /// <summary>Called when loading the plugin. The passed values should be stored in the plugin for later use. Use this function instead of <see cref="FsSetCryptCallback"/> when using the plugin outside of Total Commander.</summary>
        /// <param name="cryptProc">Crypto callback delegate. See <see cref="CryptCallback"/> for a description of this function.</param>
        /// <param name="CryptoNr">A parameter which needs to be passed to the callback function</param>
        /// <param name="Flags">Flags regarding the crypto connection. <see cref="CryptFlags"/></param>
        /// <exception cref="InvalidOperationException"><see cref="CryptInitialized"/> is true</exception>
        /// <exception cref="ArgumentNullException"><paramref name="cryptProc"/> is null and this plugin implementation supports crypto</exception>
        /// <remarks><para>Use this function to initialize the plugin when used outside of Total Commander.</para>
        /// This plugin function is implemented by <see cref="OnInitializeCryptography"/>
        /// <para>When you call this method and plugin does not support crypto (i.e. most-derived implementation of <see cref="OnInitializeCryptography"/> is decorated with <see cref="MethodNotSupportedAttribute"/> the call to this method is ignored and related crypto properties (such as <see cref="CryptInitialized"/> or <see cref="CryptoNr"/>) are not initialized</para></remarks>
        /// <version version="1.5.4">This method is new in 1.5.4</version>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        void InitializeCryptography(CryptCallback^ cryptProc, int cryptoNr, CryptFlags flags); 
        /// <summary>Gets value indicating if crypt was initialized (<see cref="InitializeCryptography"/> or <see cref="FsSetCryptCallback"/> has already been called</summary>
        /// <version version="1.5.4">This property is new in version 1.5.4</version>
        property bool CryptInitialized{bool get();}
        /// <summary>Gets crypto number that needs to be passed to crypto functions</summary>
        /// <returns>An integer number identifying this plugin instance inside Total Commander for crypto purposes. 0 when crypto was not initialized (i.e. <see creff="CryptInitialized"/> is false.</returns>
        /// <version version="1.5.4">This property is new in version 1.5.4</version>
        property int CryptoNr{int get();}
    protected:
        /// <summary>When overriden in derived class this method is called when loading the plugin after plugin cryptography was initialized.</summary>
        /// <param name="flags">Flags regarding the crypto connection</param>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>
        /// This function is only needed if you want to use the secure password store in Total Commander.
        /// No special code is needed in this function. If you derived directly from <see cref="FIleSystemPlugin"/> do not call base class method as it throws <see cref="NotSupportedException"/>.
        /// However it is necessary to override this function in derived class and NOT to decorate it with <seee cref="MethodNotSupportedAttribute"/> to indicate that your plugin wants to the secure password store in Total Commander.
        /// In older versions of Total Commmander (prior TC 7.55 / plugin interface 2.0) this function is never called (indicating that the secure password store is not supported by that version of Total Commander).
        /// <para>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.</para>
        /// <note type="inheritinfo">Do not thow any exceptions from this method. Any exceptions thrown will be passed to Total Commander which cannot handle them.</note>
        /// </remarks>
        /// <version version="1.5.4">This method is new in 1.5.4</version>
        [MethodNotSupportedAttribute]
        virtual void OnInitializeCryptography(CryptFlags flags);
#pragma endregion
#pragma region MkDir
    public:
        /// <summary>Create a directory on the plugin's file system.</summary>
        /// <param name="Path">Name of the directory to be created, with full path. The name always starts with a backslash, then the names returned by <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> separated by backslashes.</param>
        /// <returns>Return TRUE if the directory could be created, FALSE if not.</returns>
        /// <remarks><para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("MkDir","TC_FS_MKDIR")]
        BOOL FsMkDir(char* Path);
    public:
        /// <summary>When overriden in derived class creates a directory on the plugin's file system.</summary>
        /// <param name="Path">Name of the directory to be created, with full path. The name always starts with a backslash, then the names returned by <see cref="FindFirst"/>/<see cref="FindNext"/> separated by backslashes.</param>
        /// <returns>Return true if the directory could be created, false if not.</returns>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual bool MkDir(String^ Path);
#pragma endregion
#pragma region ExecuteFile    
public:
        /// <summary>Called to execute a file on the plugin's file system, or show its property sheet. It is also called to show a plugin configuration dialog when the user right clicks on the plugin root and chooses 'properties'. The plugin is then called with <paramref name="RemoteName"/>="\" and <paramref name="Verb"/>="properties" (requires TC>=5.51).</summary>
        /// <param name="MainWin">Parent window which can be used for showing a property sheet.</param>
        /// <param name="RemoteName">Name of the file to be executed, with full path.</param>
        /// <param name="Verb">This can be either "<c>open</c>", "<c>properties</c>", "<c>chmod</c>" or "<c>quote</c>" (case-insensitive).</param>
        /// <returns>Return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/> if Total Commander should download the file and execute it locally, <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> if the command was executed successfully in the plugin (or if the command isn't applicable and no further action is needed), <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Error"/> if execution failed, or <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/> if this was a (symbolic) link or .lnk file pointing to a different directory.</returns>
        /// <remarks>Meaning of verbs:
        /// <list type="table"><listheader><term>verb</term><description>meaning</description></listheader>
        /// <item><term>open</term><description>This is called when the user presses ENTER on a file. There are three ways to handle it:
        /// <list type="bulet">
        /// <item>For internal commands like "Add new connection", execute it in the plugin and return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> or <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Error"/></item>
        /// <item>Let Total Commander download the file and execute it locally: return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/></item>
        /// <item>If the file is a (symbolic) link, set <paramref name="RemoteName"/> to the location to which the link points (including the full plugin path), and return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/>. Total Commander will then switch to that directory. You can also switch to a directory on the local harddisk! To do this, return a path starting either with a drive letter, or an UNC location (\\server\share). The maximum allowed length of such a path is <see cref="FindData::MaxPath"/> - 1 characters!</item>
        /// </list></description></item>
        /// <item><term>properties</term><description>Show a property sheet for the file (optional). Currently not handled by internal Totalcmd functions if <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/> is returned, so the plugin needs to do it internally.</description></item>
        /// <item><term>chmod xxx</term><description>The xxx stands for the new Unix mode (attributes) to be applied to the file <paramref name="RemoteName"/>. This verb is only used when returning Unix attributes through <see cref="FsFindFirst"/>/<see cref="FsFindNext"/></description></item>
        /// <item><term>quote commandline</term><description>Execute the command line entered by the user in the directory <paramref name="RemoteName"/> . This is called when the user enters a command in Totalcmd's command line, and presses ENTER. This is optional, and allows to send plugin-specific commands. It's up to the plugin writer what to support here. If the user entered e.g. a cd directory command, you can return the new path in <paramref name="RemoteName"/> (max <see cref="FindData::MaxPath"/> - 1 characters), and give <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/> as return value. Return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> to cause a refresh (re-read) of the active panel.</description></item>
        /// </list>
        /// <para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("ExecuteFile","TC_FS_EXECUTEFILE")]
        int FsExecuteFile(HWND MainWin,char* RemoteName,char* Verb);
        /// <summary>Alternative function to <see cref="FsExecuteFile(HWND,char*,char*)"/> because ha have problems with exposing HWND type to managed code</summary>
        /// <param name="MainWin">Parent window which can be used for showing a property sheet.</param>
        /// <param name="RemoteName">Name of the file to be executed, with full path.</param>
        /// <param name="Verb">This can be either "<c>open</c>", "<c>properties</c>", "<c>chmod</c>" or "<c>quote</c>" (case-insensitive).</param>
        /// <returns>Return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/> if Total Commander should download the file and execute it locally, <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> if the command was executed successfully in the plugin (or if the command isn't applicable and no further action is needed), <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Error"/> if execution failed, or <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/> if this was a (symbolic) link or .lnk file pointing to a different directory.</returns>
        /// <remarks>This function is called by Total Commander and is not intended for direct use</remarks>
        /// <seelaso cref="FsExecuteFile(HWND,char*,char*)"/>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        int FsExecuteFile(HANDLE MainWin,char* RemoteName,char* Verb); 
    public:
        /// <summary>When overiden in derived class called to execute a file on the plugin's file system, or show its property sheet. It is also called to show a plugin configuration dialog when the user right clicks on the plugin root and chooses 'properties'. The plugin is then called with <paramref name="RemoteName"/>="\" and <paramref name="Verb"/>="properties" (requires TC>=5.51).</summary>
        /// <param name="hMainWin">Handle to parent window which can be used for showing a property sheet.</param>
        /// <param name="RemoteName">Name of the file to be executed, with full path. Do not assign string longer than <see cref="FindData::MaxPath"/>-1 or uncatchable <see cref="IO::PathTooLongException"/> will be thrown.</param>
        /// <param name="Verb">This can be either "<c>open</c>", "<c>properties</c>", "<c>chmod</c> number", "<c>quote</c> commandline" or "<c>mode</c> type" (case-insensitive).</param>
        /// <returns>Return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/> if Total Commander should download the file and execute it locally, <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> if the command was executed successfully in the plugin (or if the command isn't applicable and no further action is needed), <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Error"/> if execution failed, or <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/> if this was a (symbolic) link or .lnk file pointing to a different directory.</returns>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="InvalidOperationException">Excution cannot be done from other reason</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method and all most derived implementations of following methods are marked with <see cref="MethodNotSupportedAttribute"/> as well: <see cref="FtpModeAdvertisement"/>, <see cref="OpenFile"/>, <see cref="ShowFileInfo"/>, <see cref="ExecuteCommand"/></exception>
        /// <remarks>Meaning of verbs:
        /// <list type="table"><listheader><term>verb</term><description>meaning</description></listheader>
        /// <item><term>open</term><description>This is called when the user presses ENTER on a file. There are three ways to handle it:
        /// <list type="bulet">
        /// <item>For internal commands like "Add new connection", execute it in the plugin and return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> or <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Error"/></item>
        /// <item>Let Total Commander download the file and execute it locally: return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/></item>
        /// <item>If the file is a (symbolic) link, set <paramref name="RemoteName"/> to the location to which the link points (including the full plugin path), and return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/>. Total Commander will then switch to that directory. You can also switch to a directory on the local harddisk! To do this, return a path starting either with a drive letter, or an UNC location (\\server\share). The maximum allowed length of such a path is <see cref="FindData::MaxPath"/>-1 (= 259) characters!</item>
        /// </list></description></item>
        /// <item><term>properties</term><description>Show a property sheet for the file (optional). Currently not handled by internal Totalcmd functions if <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/> is returned, so the plugin needs to do it internally.</description></item>
        /// <item><term>chmod xxx</term><description>The xxx stands for the new Unix mode (attributes) to be applied to the file <paramref name="RemoteName"/>. This verb is only used when returning Unix attributes through <see cref="FindFirst"/>/<see cref="FindNext"/></description></item>
        /// <item><term>quote commandline</term><description>Execute the command line entered by the user in the directory <paramref name="RemoteName"/> . This is called when the user enters a command in Totalcmd's command line, and presses ENTER. This is optional, and allows to send plugin-specific commands. It's up to the plugin writer what to support here. If the user entered e.g. a cd directory command, you can return the new path in <paramref name="RemoteName"/> (max <see cref="FindData::MaxPath"/>-1 (= 259) characters), and give <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/> as return value. Return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> to cause a refresh (re-read) of the active panel.</description></item>
        /// <item><term>mode X</term><description>Sends information to plugin about FTP transfer mode set up in Total Commander. Plugin can safelly ignore it. X is I for binary, A for text and X*.txt *.log *.php etc. (X followed by list of masks separated by space) fro text mode.</description></item>
        /// </list>
        /// <para>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.</para>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note>
        /// <note type="inheritinfo">Plugin must implement this method and call base class method to <see cref="FtpModeAdvertisement"/>, <see cref="OpenFile"/>, <see cref="ShowFileInfo"/> and <see cref="ExecuteCommand"/> to be called. Method implementation contained in <see cref="FileSystemPlugin"/> must be overriden.
        /// <para>Authors of <see cref="FileSystemPlugin"/>-derived classes can chose either to implement <see cref="ExecuteFile"/> functionality on theri own directly in this method or call base class method and implement functionality in methods mentioned above.</para></note></remarks>
        [MethodNotSupportedAttribute]
        virtual ExecExitCode ExecuteFile(IntPtr hMainWin, String^% RemoteName, String^ Verb);

#pragma region "ExecuteFile helper methods"
    private:
        /// <summary>Contains value indicating if the <see cref="FtpModeAdvertisement"/> method is implemented (not marked with <see cref="MethodNotSupportedAttribute"/>) and so can be called.</summary>
        initonly bool FtpModeAdvertisementImplemented;
        /// <summary>Contains value indicating if the <see cref="OpenFile"/> method is implemented (not marked with <see cref="MethodNotSupportedAttribute"/>) and so can be called.</summary>
        initonly bool OpenFileImplemented;
        /// <summary>Contains value indicating if the <see cref="ShowFileInfo"/> method is implemented (not marked with <see cref="MethodNotSupportedAttribute"/>) and so can be called.</summary>
        initonly bool ShowFileInfoImplemented;
        /// <summary>Contains value indicating if the <see cref="ExecuteCommand"/> method is implemented (not marked with <see cref="MethodNotSupportedAttribute"/>) and so can be called.</summary>
        initonly bool ExecuteCommandImplemented;
        /// <summary>Contains value indicating if the <see cref="ExecuteFile"/> method is implemented (not marked with <see cref="MethodNotSupportedAttribute"/>) and so can be called.</summary>
        initonly bool ExecuteFileImplemented;
        /// <summary>Contains value indicating if the <see cref="OnInitializeCryptography"/> method is implemented (not marked with <see cref="MethodNotSupportedAttribute"/>) and so can be called.</summary>
        initonly bool OnInitializeCryptographyImplemented;
    protected:
        /// <summary>When overriden in derived class called when Total COmmander advertises FTP conection mode to plugin via <see cref="ExecuteFile"/></summary>
        /// <param name="hMainWin">Handle to Total Commander window.</param>
        /// <param name="RemoteName">Name of current file or directrory. This can be an empty string.</param>
        /// <param name="mode">FTP mode. I for binary, A for text and X*.txt *.whatever (X followed by list of masks separated by space) fro text mode. Plugin can ignore this.</param>
        /// <returns>One of <see cref="ExecExitCode"/> values.</returns>
        /// <remarks><note type="inheritinfo">This method is called only when plugin implements <see cref="ExecuteFile"/> function and thah function calls base class method.</note></remarks>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="InvalidOperationException">Excution cannot be done from other reason</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method and all most derived implementations of following methods are marked with <see cref="MethodNotSupportedAttribute"/> as well: <see cref="FtpModeAdvertisement"/>, <see cref="OpenFile"/>, <see cref="ShowFileInfo"/>, <see cref="ExecuteCommand"/></exception>
        [MethodNotSupportedAttribute]
        virtual ExecExitCode FtpModeAdvertisement(IntPtr hMainWin, String^ RemoteName, String^ mode);
        /// <summary>When overriden in derived class opens or executes given file.</summary>
        /// <param name="hMainWin">Handle to Total Commander window.</param>
        /// <param name="RemoteName">Full path of file to be opened or executed. In case the file is link (like *.lnk files in Windows) method should assignt link target path to this argument and return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/>. It will make Total Commander navigate to a new path.
        /// <para>Do not assign string longer than <see cref="FindData::MaxPath"/>-1 or uncatchable <see cref="IO::PathTooLongException"/> will be thrown.</para></param>
        /// <returns>Return <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Yourself"/> if Total Commander should download the file and execute it locally, <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.OK"/> if the command was executed successfully in the plugin (or if the command isn't applicable and no further action is needed), <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Error"/> if execution failed, or <see2 cref2="F:Tools.TotalCommanderT.ExecExitCode.Symlink"/> if this was a (symbolic) link or .lnk file pointing to a different directory.</returns>
        /// <remarks><note type="inheritinfo">This method is called only when plugin implements <see cref="ExecuteFile"/> function and thah function calls base class method.</note></remarks>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="InvalidOperationException">Excution cannot be done from other reason</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method and all most derived implementations of following methods are marked with <see cref="MethodNotSupportedAttribute"/> as well: <see cref="FtpModeAdvertisement"/>, <see cref="OpenFile"/>, <see cref="ShowFileInfo"/>, <see cref="ExecuteCommand"/></exception>
        [MethodNotSupportedAttribute]
        virtual ExecExitCode OpenFile(IntPtr hMainWin, String^% RemoteName);
        /// <summary>When overriden in derived class shows file properties for given file or directory.</summary>
        /// <param name="hMainWin">Handle to parent window which can be used for showing a property sheet.</param>
        /// <param name="RemoteName">Full path of file or directory to show properties of</param>
        /// <returns>One of <see cref="ExecExitCode"/> values.</returns>
        /// <remarks><note type="inheritinfo">This method is called only when plugin implements <see cref="ExecuteFile"/> function and thah function calls base class method.</note></remarks>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="InvalidOperationException">Excution cannot be done from other reason</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method and all most derived implementations of following methods are marked with <see cref="MethodNotSupportedAttribute"/> as well: <see cref="FtpModeAdvertisement"/>, <see cref="OpenFile"/>, <see cref="ShowFileInfo"/>, <see cref="ExecuteCommand"/></exception>
        [MethodNotSupportedAttribute]
        virtual ExecExitCode ShowFileInfo(IntPtr hMainWin, String^ RemoteName);
        /// <summary>When overriden in derived class executes command in plugin space</summary>
        /// <param name="hMainWin">Handle to Total Commander window.</param>
        /// <param name="RemoteName">Full path of currently show directory in Total Commander. Includes trailing \. If command changes current directory (like cd in Total Commander) asingn full path of new directory to this parameter. Total COmmander will navigate there.
        /// <para>Do not assign string longer than <see cref="FindData::MaxPath"/>-1 or uncatchable <see cref="IO::PathTooLongException"/> will be thrown.</para></param>
        /// <param name="command">Text of command to be executed. It's up to plugin authow which commads to support, but cd is very common.</param>
        /// <remarks><note type="inheritinfo">This method is called only when plugin implements <see cref="ExecuteFile"/> function and thah function calls base class method.</note></remarks>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="InvalidOperationException">Excution cannot be done from other reason</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method and all most derived implementations of following methods are marked with <see cref="MethodNotSupportedAttribute"/> as well: <see cref="FtpModeAdvertisement"/>, <see cref="OpenFile"/>, <see cref="ShowFileInfo"/>, <see cref="ExecuteCommand"/></exception>
        [MethodNotSupportedAttribute]
        virtual ExecExitCode ExecuteCommand(IntPtr hMainWin, String^% RemoteName, String^ command);
#pragma endregion
#pragma endregion
#pragma region RenMovFile
    public:
        /// <summary>Called to transfer (copy or move) a file within the plugin's file system.</summary>
        /// <param name="OldName">Name of the remote source file, with full path. The name always starts with a backslash, then the names returned by <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> separated by backslashes.</param>
        /// <param name="NewName">Name of the remote destination file, with full path. The name always starts with a backslash, then the names returned by <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> separated by backslashes.</param>
        /// <param name="Move">If true, the file needs to be moved to the new location and name. Many file systems allow to rename/move a file without actually moving any of its data, only the pointer to it.</param>
        /// <param name="OverWrite">Tells the function whether it should overwrite the target file or not. See notes below on how this parameter is used.</param>
        /// <param name="ri">A structure of type <see cref="RemoteInfoStruct"/> which contains the parameters of the file being renamed/moved (not of the target file!). In TC 5.51, the fields are set as follows for directories: <c>RemoteInfoStruct::SizeLow = 0</c>, <c>RemoteInfoStruct::SizeHigh = 0xFFFFFFFF</c></param>
        /// <returns>One of the <see cref="FileSystemExitCode"/> values</returns> 
        /// <remarks>Total Commander usually calls this function twice:
        /// <list tpe="bullet"><item>once with <paramref name="OverWrite"/>==false. If the remote file exists, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileExists"/>. If it doesn't exist, try to copy the file, and return an appropriate error code.</item>
        /// <item>a second time with <paramref name="OverWrite"/>==true, if the user chose to overwrite the file.</item></list>
        /// <para>While copying the file, but at least at the beginning and the end, call <see cref="ProgressProc"/> to show the copy progress and allow the user to abort the operation.</para>
        /// <para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("RenMovFile","TC_FS_RENMOVFILE")]
        int FsRenMovFile(char* OldName,char* NewName,BOOL Move, BOOL OverWrite,RemoteInfoStruct* ri);
    public:
        /// <summary>When overriden in derived class called to transfer (copy or move) a file within the plugin's file system.</summary>
        /// <param name="OldName">Name of the remote source file, with full path. The name always starts with a backslash, then the names returned by <see cref="FindFirst"/>/<see cref="FindNext"/> separated by backslashes.</param>
        /// <param name="NewName">Name of the remote destination file, with full path. The name always starts with a backslash, then the names returned by <see cref="FindFirst"/>/<see cref="FindNext"/> separated by backslashes.</param>
        /// <param name="Move">If true, the file needs to be moved to the new location and name. Many file systems allow to rename/move a file without actually moving any of its data, only the pointer to it.</param>
        /// <param name="OverWrite">Tells the function whether it should overwrite the target file or not. See notes below on how this parameter is used.</param>
        /// <param name="info">A structure of type <see cref="RemoteInfo"/> which contains the parameters of the file being renamed/moved (not of the target file!). In TC 5.51, the fields are set as follows for directories: <see cref="RemoteInfo::SizeLow"/>=0, <see cref="RemoteInfo::SizeHigh"/>=0xFFFFFFFF</param>
        /// <returns>One of the <see cref="FileSystemExitCode"/> values</returns> 
        /// <remarks>Total Commander usually calls this function twice:
        /// <list tpe="bullet"><item>once with <paramref name="OverWrite"/>==false. If the remote file exists, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileExists"/>. If it doesn't exist, try to copy the file, and return an appropriate error code.</item>
        /// <item>a second time with <paramref name="OverWrite"/>==true, if the user chose to overwrite the file.</item></list>
        /// <para>While copying the file, but at least at the beginning and the end, call <see cref="ProgressProc"/> to show the copy progress and allow the user to abort the operation.</para>
        /// <para>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.</para>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note>
        /// </remarks>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access.  ame effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="Security::SecurityException">Security error detected. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="IO::IOException">An IO error occured. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="IO::FileNotFoundException">Source file was not found. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileNotFound"/>.</exception>
        /// <exception cref="IO::DirectoryNotFoundException">Cannot locate parent directory of target file. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.WriteError"/>.</exception>
        /// <exception cref="InvalidOperationException">Requested operation is not supported (e.g. resume). Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.NotSupported"/>.</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method. Do not confuse with returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.NotSupported"/> - it has completelly different effect.</exception>
        [MethodNotSupportedAttribute]
        virtual FileSystemExitCode RenMovFile(String^ OldName, String^ NewName, bool Move, bool OverWrite, RemoteInfo info);
#pragma endregion
#pragma region GetFile
    public:
        /// <summary>Called to transfer a file from the plugin's file system to the normal file system (drive letters or UNC).</summary>
        /// <param name="RemoteName">Name of the file to be retrieved, with full path. The name always starts with a backslash, then the names returned by <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> separated by backslashes.</param>
        /// <param name="LocalName">Local file name with full path, either with a drive letter or UNC path (\\Server\Share\filename). The plugin may change the NAME/EXTENSION of the file (e.g. when file conversion is done), but not the path!</param>
        /// <param name="CopyFlags">Can be combination of the <see cref="CopyFlags"/> values</param>
        /// <param name="ri">This parameter contains information about the remote file which was previously retrieved via <see cref="FsFindFirst"/>/<see cref="FsFindNext"/>: The size, date/time, and attributes of the remote file. May be useful to copy the attributes with the file, and for displaying a progress dialog.</param>
        /// <returns>One of the <see cref="FileSystemExitCode"/> values</returns> 
        /// <remarks>Total Commander usually calls this function twice:
        /// <list type="bullet">
        /// <item>once with <paramref name="CopyFlags"/>==0 or <paramref name="CopyFlags"/>==<see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Move"/>. If the local file exists and resume is supported, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/>. If resume isn't allowed, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileExists"/></item>
        /// <item>a second time with <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Resume"/> or <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Overwrite"/>, depending on the user's choice. The resume option is only offered to the user if <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/> was returned by the first call.</item>
        /// <item><see2 cref2="F:Tools.TotalCommanderT.CopyFlags.SameCase"/> and <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.DifferentCase"/> are NEVER passed to this function, because the plugin can easily determine whether a local file exists or not.</item>
        /// <item><see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Move"/> is set, the plugin needs to delete the remote file after a successful download.</item>
        /// </list>
        /// <para>While copying the file, but at least at the beginning and the end, call <see cref="ProgressProc"/> to show the copy progress and allow the user to abort the operation.</para>
        /// <para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("GetFile","TC_FS_GETFILE")]
        int FsGetFile(char* RemoteName,char* LocalName,int CopyFlags, RemoteInfoStruct* ri);
    public:
        /// <summary>When overriden in derived class transfers a file from the plugin's file system to the normal file system (drive letters or UNC).</summary>
        /// <param name="RemoteName">Name of the file to be retrieved, with full path. The name always starts with a backslash, then the names returned by <see cref="FindFirst"/>/<see cref="FindNext"/> separated by backslashes.</param>
        /// <param name="LocalName">Local file name with full path, either with a drive letter or UNC path (\\Server\Share\filename). The plugin may change the NAME/EXTENSION of the file (e.g. when file conversion is done), but not the path! Do not assign strings longer than <see cref="FindData::MaxPath"/> or uncatchable <see cref="System::IO::PathTooLongException"/> will be thrown.</param>
        /// <param name="CopyFlags">Can be combination of the <see cref="CopyFlags"/> values</param>
        /// <param name="info">This parameter contains information about the remote file which was previously retrieved via <see cref="FindFirst"/>/<see cref="FindNext"/>: The size, date/time, and attributes of the remote file. May be useful to copy the attributes with the file, and for displaying a progress dialog.</param>
        /// <returns>One of the <see cref="FileSystemExitCode"/> values</returns> 
        /// <remarks>Total Commander usually calls this function twice:
        /// <list type="bullet">
        /// <item>once with <paramref name="CopyFlags"/>==0 or <paramref name="CopyFlags"/>==<see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Move"/>. If the local file exists and resume is supported, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/>. If resume isn't allowed, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileExists"/></item>
        /// <item>a second time with <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Resume"/> or <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Overwrite"/>, depending on the user's choice. The resume option is only offered to the user if <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/> was returned by the first call.</item>
        /// <item><see2 cref2="F:Tools.TotalCommanderT.CopyFlags.SameCase"/> and <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.DifferentCase"/> are NEVER passed to this function, because the plugin can easily determine whether a local file exists or not.</item>
        /// <item><see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Move"/> is set, the plugin needs to delete the remote file after a successful download.</item>
        /// </list>
        /// <para>While copying the file, but at least at the beginning and the end, call <see cref="ProgressProc"/> to show the copy progress and allow the user to abort the operation.</para>
        /// <para>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.</para>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note>
        /// </remarks>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access.  ame effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="Security::SecurityException">Security error detected. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="IO::IOException">An IO error occured. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="IO::FileNotFoundException">Source file was not found. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileNotFound"/>.</exception>
        /// <exception cref="IO::DirectoryNotFoundException">Cannot locate parent directory of target file. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.WriteError"/>.</exception>
        /// <exception cref="InvalidOperationException">Requested operation is not supported (e.g. resume). Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.NotSupported"/>.</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method. Do not confuse with returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.NotSupported"/> - it has completelly different effect.</exception>
        [MethodNotSupportedAttribute]
        virtual FileSystemExitCode GetFile(String^ RemoteName, String^% LocalName, CopyFlags CopyFlags, RemoteInfo info);
#pragma endregion
#pragma region PutFile    
    public:
        /// <summary>Called to transfer a file from the normal file system (drive letters or UNC) to the plugin's file system.</summary>
        /// <param name="LocalName">Local file name with full path, either with a drive letter or UNC path (\\Server\Share\filename). This file needs to be uploaded to the plugin's file system.</param>
        /// <param name="RemoteName">Name of the remote file, with full path. The name always starts with a backslash, then the names returned by <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> separated by backslashes. The plugin may change the NAME/EXTENSION of the file (e.g. when file conversion is done), but not the path!</param>
        /// <param name="CopyFlags">Can be combination of the <see cref="CopyFlags"/> values.</param>
        /// <returns>One of the <see cref="FileSystemExitCode"/> values</returns>
        /// <remarks>Total Commander usually calls this function twice, with the following parameters in <paramref name="CopyFlags"/>:
        /// <list type="bullet">
        /// <item>once with neither <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Resume"/> nor <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Overwrite"/> set. If the remote file exists and resume is supported, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/>. If resume isn't allowed, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileExists"/></item>
        /// <item>a second time with <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Resume"/> or <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Overwrite"/>, depending on the user's choice. The resume option is only offered to the user if <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/> was returned by the first call.</item>
        /// <item>The flags <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.SameCase"/> or <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.DifferentCase"/> are added to CopyFlags when the remote file exists and needs to be overwritten. This is a hint to the plugin to allow optimizations: Depending on the plugin type, it may be very slow to check the server for every single file when uploading.</item>
        /// <item>If the flag <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Move"/> is set, the plugin needs to delete the local file after a successful upload.</item>
        /// </list>
        /// <para>While copying the file, but at least at the beginning and the end, call ProgressProc to show the copy progress and allow the user to abort the operation.</para>
        /// <para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("PutFile","TC_FS_PUTFILE")]
        int FsPutFile(char* LocalName,char* RemoteName,int CopyFlags);
    public:
        /// <summary>When overriden in derived class transfers a file from the normal file system (drive letters or UNC) to the plugin's file system.</summary>
        /// <param name="LocalName">Local file name with full path, either with a drive letter or UNC path (\\Server\Share\filename). This file needs to be uploaded to the plugin's file system.</param>
        /// <param name="RemoteName">Name of the remote file, with full path. The name always starts with a backslash, then the names returned by <see cref="FindFirst"/>/<see cref="FindNext"/> separated by backslashes. The plugin may change the NAME/EXTENSION of the file (e.g. when file conversion is done), but not the path! Do not assign string longer than <see cref="FindData::MaxPath"/> to this parameter or uncatchable <see cref="IO::PathTooLongException"/> will be thrown.</param>
        /// <param name="CopyFlags">Can be combination of the <see cref="CopyFlags"/> values.</param>
        /// <returns>One of the <see cref="FileSystemExitCode"/> values</returns>
        /// <remarks>Total Commander usually calls this function twice, with the following parameters in <paramref name="CopyFlags"/>:
        /// <list type="bullet">
        /// <item>once with neither <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Resume"/> nor <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Overwrite"/> set. If the remote file exists and resume is supported, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/>. If resume isn't allowed, return <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileExists"/></item>
        /// <item>a second time with <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Resume"/> or <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Overwrite"/>, depending on the user's choice. The resume option is only offered to the user if <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ExistsResumeAllowed"/> was returned by the first call.</item>
        /// <item>The flags <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.SameCase"/> or <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.DifferentCase"/> are added to CopyFlags when the remote file exists and needs to be overwritten. This is a hint to the plugin to allow optimizations: Depending on the plugin type, it may be very slow to check the server for every single file when uploading.</item>
        /// <item>If the flag <see2 cref2="F:Tools.TotalCommanderT.CopyFlags.Move"/> is set, the plugin needs to delete the local file after a successful upload.</item>
        /// </list>
        /// <para>While copying the file, but at least at the beginning and the end, call ProgressProc to show the copy progress and allow the user to abort the operation.</para>
        /// <para>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.</para>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note>
        /// </remarks>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access.  ame effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="Security::SecurityException">Security error detected. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="IO::IOException">An IO error occured. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.ReadError"/>.</exception>
        /// <exception cref="IO::FileNotFoundException">Source file was not found. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.FileNotFound"/>.</exception>
        /// <exception cref="IO::DirectoryNotFoundException">Cannot locate parent directory of target file. Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.WriteError"/>.</exception>
        /// <exception cref="InvalidOperationException">Requested operation is not supported (e.g. resume). Same effect as returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.NotSupported"/>.</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method. Do not confuse with returning <see2 cref2="F:Tools.TotalCommanderT.FileSystemExitCode.NotSupported"/> - it has completelly different effect.</exception>
        [MethodNotSupportedAttribute]
        virtual FileSystemExitCode PutFile(String^ LocalName, String^% RemoteName, CopyFlags CopyFlags);
#pragma endregion
#pragma region DeleteFile
    public:
        /// <summary>Called to delete a file from the plugin's file system</summary>
        /// <param name="RemoteName">Name of the file to be deleted, with full path. The name always starts with a backslash, then the names returned by <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> separated by backslashes</param>
        /// <returns>Return TRUE if the file could be deleted, FALSE if not.</returns>
        /// <remarks><para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("DeleteFile","TC_FS_DELETEFILE")]
        BOOL FsDeleteFile(char* RemoteName);
    public:
        /// <summary>When overriden in derived class deletes a file from the plugin's file system</summary>
        /// <param name="RemoteName">Name of the file to be deleted, with full path. The name always starts with a backslash, then the names returned by <see cref="FindFirst"/>/<see cref="FindNext"/> separated by backslashes</param>
        /// <returns>Return true if the file could be deleted, false if not.</returns>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual bool DeleteFile(String^ RemoteName);
#pragma endregion
#pragma region RemoveDir
    public:
        /// <summary>Called to remove a directory from the plugin's file system.</summary>
        /// <param name="RemoteName">Name of the directory to be removed, with full path. The name always starts with a backslash, then the names returned by <see cref="FsFindFirst"/>/<see cref="FsFindNext"/> separated by backslashes.</param>
        /// <returns>Return TRUE if the directory could be removed, FALSE if not.</returns>
        /// <remarks><para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("RemoveDir","TC_FS_REMOVEDIR")]
        BOOL FsRemoveDir(char* RemoteName);
    public:
        /// <summary>When overriden in derived class removes a directory from the plugin's file system.</summary>
        /// <param name="RemoteName">Name of the directory to be removed, with full path. The name always starts with a backslash, then the names returned by <see cref="FindFirst"/>/<see cref="FindNext"/> separated by backslashes.</param>
        /// <returns>Return true if the directory could be removed, false if not.</returns>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual bool RemoveDir(String^ RemoteName);
#pragma endregion
#pragma region Disconnect
    public:
        /// <summary>Called when the user presses the Disconnect button in the FTP connections toolbar. This toolbar is only shown if <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/> is passed to <see cref="LogProc"/>.</summary>
        /// <param name="DisconnectRoot">This is the root dir which was passed to <see cref="LogProc"/> when connecting. It allows the plugin to have serveral open connections to different file systems (e.g. ftp servers). Should be either \ (for a single possible connection) or \Servername (e.g. when having multiple open connections).</param>
        /// <returns>Return TRUE if the connection was closed (or never open), FALSE if it couldn't be closed.</returns>
        /// <remarks>To get calls to this function, the plugin MUST call <see cref="LogProc"/> with the parameter <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/>. The parameter LogString MUST start with "<c>CONNECT</c>", followed by one whitespace and the root of the file system which has been connected. This file system root will be passed to <see cref="FsDisconnect"/> when the user presses the Disconnect button, so the plugin knows which connection to close.
        /// Do NOT call <see cref="LogProc"/> with <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/> if your plugin does not require connect/disconnect!
        /// <list><listheader>Examples</listheader>
        /// <item>FTP requires connect/disconnect. Connect can be done automatically when the user enters a subdir, disconnect when the user clicks the Disconnect button.</item>
        /// <item>Access to local file systems (e.g. Linux EXT2) does not require connect/disconnect, so don't call <see cref="LogProc"/> with the parameter <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/>.</item></list>
        /// <para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("Disconnect","TC_FS_DISCONNECT")]
        BOOL FsDisconnect(char* DisconnectRoot);
    public:
        /// <summary>When overridden in derived class, called when the user presses the Disconnect button in the FTP connections toolbar. This toolbar is only shown if <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/> is passed to <see cref="LogProc"/>.</summary>
        /// <param name="DisconnectRoot">This is the root dir which was passed to <see cref="LogProc"/> when connecting. It allows the plugin to have serveral open connections to different file systems (e.g. ftp servers). Should be either \ (for a single possible connection) or \Servername (e.g. when having multiple open connections).</param>
        /// <returns>Return true if the connection was closed (or never open), false if it couldn't be closed.</returns>
        /// <remarks>To get calls to this function, the plugin MUST call <see cref="LogProc"/> with the parameter <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/>. The parameter LogString MUST start with "<c>CONNECT</c>", followed by one whitespace and the root of the file system which has been connected. This file system root will be passed to <see cref="Disconnect"/> when the user presses the Disconnect button, so the plugin knows which connection to close.
        /// Do NOT call <see cref="LogProc"/> with <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/> if your plugin does not require connect/disconnect!
        /// <list><listheader>Examples</listheader>
        /// <item>FTP requires connect/disconnect. Connect can be done automatically when the user enters a subdir, disconnect when the user clicks the Disconnect button.</item>
        /// <item>Access to local file systems (e.g. Linux EXT2) does not require connect/disconnect, so don't call <see cref="LogProc"/> with the parameter <see2 cref2="F:Tools.TotalCommanderT.LogKind.Connect"/>.</item></list>
        /// </remarks>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual bool Disconnect(String^ DisconnectRoot);
#pragma endregion
#pragma region SetAttr
    public:
        /// <summary>Called to set the (Windows-Style) file attributes of a file/dir. <see cref="FsExecuteFile(HWND,char*,char*)"/> is called for Unix-style attributes.</summary>
        /// <param name="RemoteName">Name of the file/directory whose attributes have to be set</param>
        /// <param name="NewAttr">New file attributes</param>
        /// <returns>Return TRUE if successful, FALSE if the function failed.</returns>
        /// <remarks><para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("SetAttr","TC_FS_SETATTR")]
        BOOL FsSetAttr(char* RemoteName,int NewAttr);
    public:
        /// <summary>When overriden in derived class sets the (Windows-Style) file attributes of a file/dir. <see cref="ExecuteFile"/> is called for Unix-style attributes.</summary>
        /// <param name="RemoteName">Name of the file/directory whose attributes have to be set</param>
        /// <param name="NewAttr">New file attributes</param>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual void SetAttr(String^ RemoteName, StandardFileAttributes NewAttr);
#pragma endregion
#pragma region SetTime
    public:
        /// <summary>Called to set the (Windows-Style) file times of a file/dir.</summary>
        /// <param name="RemoteName">Name of the file/directory whose attributes have to be set</param>
        /// <param name="CreationTime">Creation time of the file. May be NULL to leave it unchanged.</param>
        /// <param name="LastAccessTime">Last access time of the file. May be NULL to leave it unchanged.</param>
        /// <param name="LastWriteTime">Last write time of the file. May be NULL to leave it unchanged. If your file system only supports one time, use this parameter!</param>
        /// <returns>Return TRUE if successful, FALSE if the function failed.</returns>
        /// <remarks><para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("SetTime","TC_FS_SETTIME")]
        BOOL FsSetTime(char* RemoteName,FILETIME *CreationTime, FILETIME *LastAccessTime,FILETIME *LastWriteTime);
    public:
        /// <summary>When overriden in derived class sets the (Windows-Style) file times of a file/dir.</summary>
        /// <param name="RemoteName">Name of the file/directory whose attributes have to be set</param>
        /// <param name="CreationTime">Creation time of the file. May be NULL to leave it unchanged.</param>
        /// <param name="LastAccessTime">Last access time of the file. May be NULL to leave it unchanged.</param>
        /// <param name="LastWriteTime">Last write time of the file. May be NULL to leave it unchanged. If your file system only supports one time, use this parameter!</param>
        /// <exception cref="UnauthorizedAccessException">The user does not have required access</exception>
        /// <exception cref="Security::SecurityException">Security error detected</exception>
        /// <exception cref="IO::IOException">An IO error occured</exception>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual void SetTime(String^ RemoteName, Nullable<DateTime> CreationTime, Nullable<DateTime> LastAccessTime, Nullable<DateTime> LastWriteTime);
#pragma endregion
#pragma region StatusInfo
    public:
        /// <summary>Called just as an information to the plugin that a certain operation starts or ends. It can be used to allocate/free buffers, and/or to flush data from a cache. There is no need to implement this function if the plugin doesn't require it.</summary>
        /// <param name="RemoteDir">This is the current source directory when the operation starts. May be used to find out which part of the file system is affected.</param>
        /// <param name="InfoStartEnd">Information whether the operation starts or ends</param>
        /// <param name="InfoOperation">Information of which operaration starts/ends</param>
        /// <remarks>Please note that future versions of the framework may send additional values!
        /// <para>This function has been added for the convenience of plugin writers. All calls to plugin functions will be enclosed in a pair of <see cref="FsStatusInfo"/> calls: At the start, <see cref="FsStatusInfo"/>(...,FS_STATUS_START,...) and when the operation is done FsStatusInfo(...,FS_STATUS_END,...). Multiple plugin calls can be between these two calls. For example, a download may contain multiple calls to <see cref="FsGetFile"/>, and <see cref="FsFindFirst"/>, <see cref="FsFindNext"/>, <see cref="FsFindClose"/> (for copying subdirs).</para>
        /// <para>This function is called by Total Commander and is not intended for direct use.</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("StatusInfo","TC_FS_STATUSINFO")]
        void FsStatusInfo(char* RemoteDir, int InfoStartEnd, int InfoOperation);
    public:
        /// <summary>Called instead of <see cref="FsStatusInfo"/> when plugin is used outside of Total Commander.</summary>
        /// <param name="RemoteDir">This is the current source directory when the operation starts. May be used to find out which part of the file system is affected.</param>
        /// <param name="InfoStartEnd">Information whether the operation starts or ends</param>
        /// <param name="InfoOperation">Information of which operaration starts/ends</param>
        /// <remarks>Please note that future versions of the framework may send additional values!</remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Advanced)]
        void StatusInfo(String^ RemoteDir, OperationStatus InfoStartEnd, OperationKind InfoOperation);
        /// <summary>When overiden in derived class handles operation status change reported by Total Commaner</summary>
        /// <param name="e">Event arguments</param>
        /// <remarks>Do not call this method from your code. It is called by Total Commander. In case you use plugin outside of Total Commander call <see cref="StatusInfo"/>.</remarks>
        virtual void OnOperationStatusChanged(OperationEventArgs^ e);
        /// <summary>When overriden in derived class handles start of operation reported by Total Commander</summary>
        /// <remarks>This method is called before operation sarts. Call of this method is always fololowed by call of <see cref="OnOperationStatusChanged"/>.
        /// <para>Do not call this method from your code. It is called by Total Commander. In case you use plugin outside of Total Commander call <see cref="StatusInfo"/>.</para></remarks>
        /// <param name="e">Event arguments</param>
        virtual void OnOperationStarting(OperationEventArgs^ e);
        /// <summary>When overriden in derived class handles end of operation reported by Total Commander</summary>
        /// <remarks>This method is called after operation finishes.  Call of this method is always fololowed by call of <see cref="OnOperationStatusChanged"/>.
        /// <para>Do not call this method from your code. It is called by Total Commander. In case you use plugin outside of Total Commander call <see cref="StatusInfo"/>.</para></remarks>
        /// <param name="e">Event arguments</param>
        virtual void OnOperationFinished(OperationEventArgs^ e);
#pragma endregion
#pragma region GetDefRoootName
    public:
        /// <summary>Called only when the plugin is installed. It asks the plugin for the default root name which should appear in the Network Neighborhood. This root name is NOT part of the path passed to the plugin when Totalcmd accesses the plugin file system! The root will always be "\", and all subpaths will be built from the directory names returned by the plugin.</summary>
        /// <param name="DefRootName">Pointer to a buffer (allocated by the calling program) which can receive the root name.</param>
        /// <param name="maxlen">Maximum number of characters (including the final 0) which fit in the buffer."</param>
        /// <remarks>Example: The root name may be "Linux file system" for a plugin which accesses Linux drives. If this function isn't implemented, Totalcmd will suggest the name of the DLL (without extension .DLL) as the plugin root. This function is called directly after loading the plugin (when the user installs it), <see cref="FsInit"/> is NOT called when installing the plugin.
        /// <para>This function is called by Total Commander and is not intended for direct use</para></remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("TC_FS_GETDEFROOTNAME")]
        void FsGetDefRootName(char* DefRootName,int maxlen);
#pragma endregion
#pragma region ExtractCustomIcon
    public:
        /// <summary>Called when a file/directory is displayed in the file list. It can be used to specify a custom icon for that file/directory.</summary>
        /// <param name="RemoteName">This is the full path to the file or directory whose icon is to be retrieved. When extracting an icon, you can return an icon name here - this ensures that the icon is only cached once in the calling program. The returned icon name must not be longer than <see cref="FindData::MaxPath"/> characters (including terminating 0!). The icon handle must still be returned in <paramref name="TheIcon"/>!</param>
        /// <param name="ExtractFlags">Flags for the extract operation. A combination of <see cref="IconExtractFlags"/>.</param>
        /// <param name="TheIcon">Here you need to return the icon handle.</param>
        /// <returns>One of the <see cref="IconExtractResult"/> values</returns> 
        /// <remarks>If you return <see2 cref2="F:Tools.TotalCommanderT.IconExtractResult.Delayed"/>, <see cref="FsExtractCustomIcon"/> will be called again from a background thread at a later time. A critical section is used by the calling app to ensure that <see cref="FsExtractCustomIcon"/> is never entered twice at the same time. This return value should be used for icons which take a while to extract, e.g. EXE icons. If the user turns off background loading of icons, the function will be called in the foreground with the <see2 cref2="F:Tools.TotalCommanderT.IconExtractFlags.BackgroundThread"/> flag.
        /// <para>This function is called by Total Commander and is not intended for direct use.</para>
        /// <para>This function is new in wfx version 1.1. It requires Total Commander >=5.51, but is ignored by older versions.</para></remarks>
        /// <exception cref="IO::PathTooLongException">String passed by plugin function <see cref="ExctractCustomIcon"/> to <paramref name="RemoteName"/> is longer than <see cref="FindData::MaxPath"/> - 1.</exception> 
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("ExctractCustomIcon","TC_FS_EXTRACTCUSTOMICON")]
        int FsExtractCustomIcon(char* RemoteName,int ExtractFlags,HICON* TheIcon);
        /// <summary>When overriden in derived class, called when a file/directory is displayed in the file list. It can be used to specify a custom icon for that file/directory.</summary>
        /// <param name="RemoteName">This is the full path to the file or directory whose icon is to be retrieved. When extracting an icon, you can return an icon name here - this ensures that the icon is only cached once in the calling program. The returned icon name must not be longer than <see cref="FindData::MaxPath"/> - 1 characters (otherwise uncatchable <see cref="IO::PathTooLongException"/> will be thrown by <see cref="FsExtractCustomIcon"/>). The icon itself must still be returned in <paramref name="TheIcon"/>!</param>
        /// <param name="ExtractFlags">Flags for the extract operation. A combination of <see cref="IconExtractFlags"/>.</param>
        /// <param name="TheIcon">Here you need to return the icon, unless return value is <see2 cref2="F:Tools.TotalCommanderT.IconExtractResult.Delayed"/> or <see2 cref2="F:Tools.TotalCommanderT.IconExtractResult.UseDefault"/></param>
        /// <returns>One of the <see cref="IconExtractResult"/> values</returns> 
        /// <remarks>If you return <see2 cref2="F:Tools.TotalCommanderT.IconExtractResult.Delayed"/>, <see cref="ExctractCustomIcon"/> will be called again from a background thread at a later time. A critical section is used by the calling app to ensure that <see cref="ExctractCustomIcon"/> is never entered twice at the same time. This return value should be used for icons which take a while to extract, e.g. EXE icons. If the user turns off background loading of icons, the function will be called in the foreground with the <see2 cref2="F:Tools.TotalCommanderT.IconExtractFlags.BackgroundThread"/> flag.
        /// <para>When most-derived method implementation is marked with <see cref="MethodNotSupportedAttribute"/>, it means that the most derived plugin implementation does not support operation provided by the method.</para>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        [MethodNotSupportedAttribute]
        virtual IconExtractResult ExctractCustomIcon(String^% RemoteName, IconExtractFlags ExtractFlags, Drawing::Icon^% TheIcon);
#pragma endregion
#pragma region SetDefaultParams
public:
        /// <summary>Called immediately after <see cref="FsInit"/>.</summary>
        /// <param name="dps">This structure of type <see cref="FsDefaultParamStruct"/> currently contains the version number of the plugin interface, and the suggested location for the settings file (ini file). It is recommended to store any plugin-specific information either directly in that file, or in that directory under a different name. Make sure to use a unique header when storing data in this file, because it is shared by other file system plugins! If your plugin needs more than 1kbyte of data, you should use your own ini file because ini files are limited to 64k.</param>
        /// <remarks>
        /// <para>This function is new in wfx version 1.3. It requires Total Commander >=5.51, but is ignored by older versions.</para>
        /// <para>This function is called by Total Commander and is not intended for direct use.</para>
        /// </remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("TC_FS_SETDEFAULTPARAMS")]
        void FsSetDefaultParams(FsDefaultParamStruct* dps);
    private:
        /// <summary>Contains value of the <see cref="PluginParams"/> property</summary>
        Nullable<DefaultParams> pluginParams;
    protected:
        /// <summary>Gets default parameters of the plugin</summary>
        /// <returns>Default parameters of the plugin</returns>
        /// <remarks>Value of this property is only valid after the <see cref="SetDefaultParams"/> method was called</remarks>
        /// <sealso cref="SetDefaultParams"/>
        /// <exception cref="InvalidOperationException">Property have not been initialized yet. This property can be initialized by calling <see cref="SetDefaultParams"/></exception>
        property DefaultParams PluginParams {DefaultParams get();}
    public:
        /// <summary>Called immediately after <see cref="OnInit"/>.</summary>
        /// <param name="dps">This structure curently contains version number of the Total Commander plugin interface (not this managed interface) and suggested location of settings file. It is recommended to store any plugin-specific information either directly in that file or in that directory under a different name.</param>
        /// <remarks>Make sure to use a unique header when storing data in this file, because it is shared by other file system plugins! If your plugin needs more than 1kbyte of data, you should use your own ini file because ini files are limited to 64k.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note>
        /// <note type="inheritinfo">Always call base class method. When base class method is not called, the <see cref="PluginParams"/> property does not have valid value.</note></remarks>
        /// <exception cref="InvalidOperationException">This method is called when it was already called. This method can be called only once on each instance.</exception>
        virtual void SetDefaultParams(DefaultParams dps);
#pragma endregion
#pragma region GetPreviewBitmap
    public:
        /// <summary>Called when a file/directory is displayed in thumbnail view. It can be used to return a custom bitmap for that file/directory.</summary>
        /// <param name="RemoteName">This is the full path to the file or directory whose bitmap is to be retrieved. When extracting a bitmap, you can return a bitmap name here - this ensures that the icon is only cached once in the calling program. The returned bitmap name must not be longer than <see cref="FindData::MaxPath"/> characters (including terminating 0!). The bitmap handle must still be returned in <paramref name="ReturnedBitmap"/>!</param>
        /// <param name="width">The maximum dimensions of the preview bitmap. If your image is smaller, or has a different side ratio, then you need to return an image which is smaller than these dimensions!</param>
        /// <param name="height">The maximum dimensions of the preview bitmap. If your image is smaller, or has a different side ratio, then you need to return an image which is smaller than these dimensions!</param>
        /// <param name="ReturnedBitmap">Here you need to return the bitmap handle.</param>
        /// <returns>The <see cref="BitmapHandling"/> value</returns>
        /// <remarks>This function is new in version 1.4. It requires Total Commander >=7.0, but is ignored by older versions.
        /// <para>Inportant notes</para>
        /// <list type="numbered">
        /// <item>This function is only called in Total Commander 7.0 and later. The reported plugin version will be >= 1.4.</item>
        /// <item>The bitmap handle goes into possession of Total Commander, which will delete it after using it. The plugin must not delete the bitmap handle!</item>
        /// <item>Make sure you scale your image correctly to the desired maximum width+height! Do not fill the rest of the bitmap - instead, create a bitmap which is SMALLER than requested! This way, Total Commander can center your image and fill the rest with the default background color.</item>
        /// </list>
        /// <para>This function is called by Total Commander and is not intended for direct use.</para>
        /// </remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("GetPreviewBitmap","TC_FS_GETPREVIEWBITMAP")]
        int FsGetPreviewBitmap(char* RemoteName,int width,int height, HBITMAP* ReturnedBitmap);
        /// <summary>When overriden in derved class called when a file/directory is displayed in thumbnail view. It can be used to return a custom bitmap for that file/directory.</summary>
        /// <param name="width">The maximum dimensions of the preview bitmap. If your image is smaller, or has a different side ratio, then you need to return an image which is smaller than these dimensions!</param>
        /// <param name="height">The maximum dimensions of the preview bitmap. If your image is smaller, or has a different side ratio, then you need to return an image which is smaller than these dimensions!</param>
        /// <returns>The <see cref="BitmapResult"/> indicating where to obtain the bitmap or the bitmap itself; null when default image shuld be used.</returns>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>
        /// <para>Inportant notes</para>
        /// <list type="numbered">
        /// <item>This function is only called in Total Commander 7.0 and later. The reported plugin version will be >= 1.4.</item>
        /// <item>The bitmap handle goes into possession of Total Commander, which will delete it after using it. The plugin must not delete the bitmap handle! (when <see2 cref2="F:Tools.TotalCommanderT.BitmapResult.Bitmap"/> is set.</item>
        /// <item>Make sure you scale your image correctly to the desired maximum width+height! Do not fill the rest of the bitmap - instead, create a bitmap which is SMALLER than requested! This way, Total Commander can center your image and fill the rest with the default background color.</item>
        /// </list>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        [MethodNotSupportedAttribute]
        virtual BitmapResult^ GetPreviewBitmap(String^ RemoteName, int width, int height);
#pragma endregion
#pragma region LinksToLocalFiles
    public:
        /// <summary>Indicates if plugin is temporary panel-style plugin</summary>
        /// <returns>True if the plugin is a temporary panel-style plugin; false if the plugin is a normal file system plugin</returns>
        /// <remarks><see cref="FsLinksToLocalFiles"/> must not be implemented unless your plugin is a temporary file panel plugin! Temporary file panels just hold links to files on the local file system.
        /// <para>If your plugin is a temporary panel plugin, the following functions MUST be thread-safe (can be called from background transfer manager):</para>
        /// <list type="bullet">
        /// <item><see cref="FsLinksToLocalFiles"/></item>
        /// <item><see cref="FsFindFirst"/></item>
        /// <item><see cref="FsFindNext"/></item>
        /// <item><see cref="FsFindClose"/></item>
        /// <item><see cref="FsGetLocalName"/></item>
        /// </list>
        /// <para>This means that when uploading subdirectories from your plugin to FTP in the background, Total Commander will call these functions in a background thread. If the user continues to work in the foreground, calls to <see cref="FsFindFirst"/> and <see cref="FsFindNext"/> may be occuring at the same time! Therefore it's very important to use the search handle to keep temporary information about the search.</para>
        /// <para><see cref="FsStatusInfo"/> will NOT be called from the background thread!</para>
        /// <para>This function is called by Total Commander and is not intended for direct use. Plugin implements this function via the <see cref="LinksToLocalFiles"/> property.</para>
        /// </remarks>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("get_LinksToLocalFiles","TC_FS_LINKSTOLOCALFILES")]
        BOOL FsLinksToLocalFiles(void);
        /// <summary>When overriden in derived class gets valud indicating if plugin is temporary panel-style plugin.</summary>
        /// <returns>True if the plugin is a temporary panel-style plugin; false if the plugin is a normal file system plugin</returns>
        /// <exception cref="NotSupportedException">The actual implementation of getter is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks><see cref="LinksToLocalFiles"/> must not be implemented unless your plugin is a temporary file panel plugin! Temporary file panels just hold links to files on the local file system.
        /// <para>If your plugin is a temporary panel plugin, the following functions MUST be thread-safe (can be called from background transfer manager):</para>
        /// <list type="bullet">
        /// <item><see cref="LinksToLocalFiles"/></item>
        /// <item><see cref="FindFirst"/></item>
        /// <item><see cref="FindNext"/></item>
        /// <item><see cref="FindClose"/></item>
        /// <item><see cref="GetLocalName"/></item>
        /// </list>
        /// <para>This means that when uploading subdirectories from your plugin to FTP in the background, Total Commander will call these functions in a background thread. If the user continues to work in the foreground, calls to <see cref="FsFindFirst"/> and <see cref="FsFindNext"/> may be occuring at the same time! Therefore it's very important to use the search handle to keep temporary information about the search.</para>
        /// <para><see cref="StatusInfo"/> will NOT be called from the background thread!</para>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note>
        /// <para>Unlike in case of methods, this is property. But the possible <see cref="MethodNotSupportedAttribute"/> must be applied onto method - this is the getter of this property.</para>
        /// <para>This property implements plugin function <see cref="FsLinksToLocalFiles"/></para>.
        /// </remarks>
        virtual property bool LinksToLocalFiles{
            [MethodNotSupportedAttribute]
            virtual bool get();
        }
#pragma endregion
#pragma region GetLocalName
    public:
        /// <summary>Gets local name of plugin file</summary>
        /// <param name="RemoteName">
        /// <para>In: Full path to the file name in the plugin namespace, e.g. \somedir\file.ext</para>
        /// <para>Out: Return the path of the file on the local file system, e.g. c:\windows\file.ext</para>
        /// </param>
        /// <param name="maxlen">Maximum number of characters you can return in <paramref name="RemoteName"/>, including the final 0.</param>
        /// <returns>True if the name points to a local file, which is returned in <paramref name="RemoteName"/>. False if the name does not point to a local file, <paramref name="RemoteName"/> is left unchanged.</returns>
        /// <remarks><see cref="FsGetLocalName"/> must not be implemented unless your plugin is a temporary file panel plugin! Temporary file panels just hold links to files on the local file system.
        /// <para>This function is called by Total Commander and is not intended for direct use. Plugin implements this function via the <see cref="LinksToLocalFiles"/> property.</para></remarks>
        /// <seelaso cref="FsLinksToLocalFiles"/><seealso cref="GetLocalName"/>
        /// <exception cref="IO::PathTooLongException">String longer than <paramref name="maxlen"/> - 1 is returned by <see cref="FsGetLocalName"/>.</exception>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("GetLocalName","TC_FS_GETLOCALNAME")]
        BOOL FsGetLocalName(char* RemoteName,int maxlen);
        /// <summary>When overriden in derved class gets local name of plugin file</summary>
        /// <param name="RemoteName">Full path to the file name in the plugin namespace, e.g. \somedir\file.ext</param>
        /// <param name="maxlen">Maximum number of characters you can return. Do not return longer strings because uncatchable <see cref="IO::PathTooLongException"/> will be throw by <see cref="FsGetLocalName"/>.</param>
        /// <returns>Return the path of the file on the local file system, e.g. c:\windows\file.ext; null if the name does not point to a local file. Do not return paths longer than <see cref="FindData::MaxPath"/> - 1 otherwise uncatchable <see cref="IO::PathTooLongException"/> will be thrown.</returns>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks><see cref="FsGetLocalName"/> must not be implemented unless your plugin is a temporary file panel plugin! Temporary file panels just hold links to files on the local file system.
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual String^ GetLocalName(String^ RemoteName, int maxlen);
#pragma endregion
#pragma region ContentgetDefaultView
    public:
        /// <summary>Called to get the default view to which Total Commander should switch when this file system plugin is entered.</summary>
        /// <param name="ViewContents">Return the default fields for this plugin here, e.g. "<c>[=&lt;fs>.size.bkM2]\n[=fs.writetime]</c>"
        /// <note>Note that in C, you need to write \\n to return a backslash and 'n' instead of a newline character!</note></param>
        /// <param name="ViewHeaders">Return the default headers shown in the sorting header bar, e.g. "<c>Size\nDate/Time</c>"</param>
        /// <param name="ViewWidths">Return the default column widths shown in the sorting header bar, e.g. "<c>148,23,-35,-35</c>" Negative values mean that the field is right-aligned. The first two widths are for name and extension</param>
        /// <param name="ViewOptions">The two values, separated by a vertical line, mean:
        /// <list type="number">
        /// <item><c>auto-adjust-width</c>, or <c>-1</c> for no adjust</item>
        /// <item>horizontal scrollbar flag</item></list></param>
        /// <returns>Return true if you returned a default view, false if no default view should be shown.</returns>
        /// <remarks>It's best to create a custom columns view in Total Commander, save it, and then copy the definitions from the Wincmd.ini to your plugin. The values in <paramref name="ViewContents"/> and <paramref name="ViewHeaders"/> are separated by a backslash and lowercase 'n' character.
        /// <note>Note that in C, you need to write \\n to return a backslash and 'n' instead of a newline character!</note>
        /// <para>This function is called by Total Commander and is not intended for direct use.</para></remarks>
        /// <version version="1.5.3">This function is new in version 1.5.3</version>
        /// <seealso cref="GetDefaultView"/>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("GetDefaultView","TC_FS_GETDEFAULTVIEW")]
        BOOL FsContentGetDefaultView(char* ViewContents,char* ViewHeaders,char* ViewWidths,char* ViewOptions,int maxlen);
        /// <summary>When overriden in derived class called to get the default view to which Total Commander should switch when this file system plugin is entered.</summary>
        /// <param name="maxlen">Maximum length if sting accepted by total commander. Lenghts of <see cref="ViewDefinition::GetColumnSourcesString"/>, <see cref="ViewDefinition::GetNamesString"/>, <see cref="ViewDefinition::GetWidthsString"/> and <see cref="ViewDefinition::GetOptionsString"/> returned shall not exceed this lenght or uncatchable exception will be thrown.</param>
        /// <returns>A <see cref="ViewDefinition"/> specifying default view to be used. Null not to show any specific view</returns>
        /// <exception cref="NotSupportedException">The actual implementation is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks><note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note></remarks>
        [MethodNotSupportedAttribute]
        virtual ViewDefinition^ GetDefaultView(int maxlen);
#pragma endregion
#pragma region GetBackgroundFlags
    public:
        /// <summary>Called by Total Commander 7.51 or newer to determine whether the plugin supports background operations (uploads and downloads), and if yes, how they are supported.</summary>
        /// <returns>A combination of <see cref="BackgroundTransferSupport"/> flags</returns>
        /// <remarks>
        /// <para>
        /// If the flag <see cref2="F:Tools.TotalCommanderT.BackgroundTransferSupport.AskUser"/> is set and the user checks the option to transfer the file in background,
        /// Total Commander starts a background thread and calls <see cref="FsStatusInfo"/> with parameters
        /// <see cref2="F:Tools.TotalCommanderT.OperationStatus.Start"/> and 
        /// <see cref2="F:Tools.TotalCommanderT.OperationKind.GetMultiThread"/> instead of <see cref2="F:Tools.TotalCommanderT.OperationKind.GetMulti"/> for downloads,
        /// and <see cref2="F:Tools.TotalCommanderT.OperationKind.PutMultiThread"/> instead of <see cref2="F:Tools.TotalCommanderT.OperationKind.PutMulti"/> for uploads.
        /// A corresponding <see cref2="F:Tools.TotalCommanderT.OperationStatus.End"/> is sent when the transfer is done.
        /// These notifications can be used to build up the background connection and to close it when done.
        /// You need to use <see cref="System.Threading.Thread"/> to recognize a background operation.
        /// Same thread is used for the entire operation in <see cref="FsStatusInfo"/>, <see cref="FsGetFile"/> and <see cref="FsPutFile"/>.
        /// </para>
        /// <para>
        /// If the flag <see cref2="F:Tools.TotalCommanderT.BackgroundTransferSupport.AskUser"/> is NOT set, all uploads and downloads with F5 or F6 will be started in a background thread.
        /// Initially the normal foreground transfer window will be shown, which will be changed to a background transfer window when the user clicks on "Background".
        /// For the plugin, the entire operation will take place in the same background thread, though.
        /// This method is recommended only for plugins where no extra connections are needed for multiple parallel transfers, or where the additional connections
        /// are built up very quickly. Example of a plugin using this method is the WinCE plugin for Windows Mobile devices.
        /// </para>
        /// <para>This function is called by Total Commander and is not intended for direct use. Plugin implements this function via getter of the <see cref="BackgroundFlags"/> property.</para>
        /// </remarks>
        /// <seealso cref="BackgroundFlags"/>
        /// <version version="1.5.4">This function is new in version 1.5.4</version>
        [EditorBrowsableAttribute(EditorBrowsableState::Never)]
        [CLSCompliantAttribute(false)]
        [PluginMethod("get_BackgroundFlags","TC_FS_GETBACKGROUNDFLAGS")]
        int FsGetBackgroundFlags(void);
        
        /// <summary>When implemneted in derived class gets value indicating whether the plugin supports background operations (uploads and downloads), and if yes, how they are supported.</summmary>
        /// <returns>A combination of <see cref="BackgroundTransferSupport"/> flags</returns>
        /// <exception cref="NotSupportedException">The actual implementation of getter is marked with <see cref="MethodNotSupportedAttribute"/> which means that the plugin doesnot support operation provided by the method.</exception>
        /// <remarks>
        /// <para>
        /// If the flag <see cref2="F:Tools.TotalCommanderT.BackgroundTransferSupport.AskUser"/> is set and the user checks the option to transfer the file in background,
        /// Total Commander starts a background thread and calls <see cref="FsStatusInfo"/> with parameters
        /// <see cref2="F:Tools.TotalCommanderT.OperationStatus.Start"/> and 
        /// <see cref2="F:Tools.TotalCommanderT.OperationKind.GetMultiThread"/> instead of <see cref2="F:Tools.TotalCommanderT.OperationKind.GetMulti"/> for downloads,
        /// and <see cref2="F:Tools.TotalCommanderT.OperationKind.PutMultiThread"/> instead of <see cref2="F:Tools.TotalCommanderT.OperationKind.PutMulti"/> for uploads.
        /// A corresponding <see cref2="F:Tools.TotalCommanderT.OperationStatus.End"/> is sent when the transfer is done.
        /// These notifications can be used to build up the background connection and to close it when done.
        /// You need to use <see cref="System.Threading.Thread"/> to recognize a background operation.
        /// Same thread is used for the entire operation in <see cref="FsStatusInfo"/>, <see cref="FsGetFile"/> and <see cref="FsPutFile"/>.
        /// </para>
        /// <para>
        /// If the flag <see cref2="F:Tools.TotalCommanderT.BackgroundTransferSupport.AskUser"/> is NOT set, all uploads and downloads with F5 or F6 will be started in a background thread.
        /// Initially the normal foreground transfer window will be shown, which will be changed to a background transfer window when the user clicks on "Background".
        /// For the plugin, the entire operation will take place in the same background thread, though.
        /// This method is recommended only for plugins where no extra connections are needed for multiple parallel transfers, or where the additional connections
        /// are built up very quickly. Example of a plugin using this method is the WinCE plugin for Windows Mobile devices.
        /// </para>
        /// <note type="inheritinfo">Do not thow any other exceptions. Such exception will be passed to Total Commander which cannot handle it.</note>
        /// <para>Unlike in case of methods, this is property. But the possible <see cref="MethodNotSupportedAttribute"/> must be applied onto method - this is the getter of this property.</para>
        /// <para>This property implements plugin function <see cref="FsGetBackgroundFlags"/></para>.
        /// </remarks>
        /// <version version="1.5.4">This property is new in version 1.5.4</version>
        property BackgroundTransferSupport BackgroundFlags{
            [MethodNotSupportedAttribute]
            virtual BackgroundTransferSupport get();
        }
#pragma endregion
#pragma endregion
    };
}}