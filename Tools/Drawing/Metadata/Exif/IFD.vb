﻿Imports RecordDic = Tools.CollectionsT.GenericT.DictionaryWithEvents(Of UShort, Tools.DrawingT.MetadataT.ExifT.ExifRecord)
Imports SubIFDDic = Tools.CollectionsT.GenericT.DictionaryWithEvents(Of UShort, Tools.DrawingT.MetadataT.ExifT.SubIFD)
Imports RecordList = Tools.CollectionsT.GenericT.ListWithEvents(Of Tools.DrawingT.MetadataT.ExifT.ExifRecord)
Imports SubIFDList = Tools.CollectionsT.GenericT.ListWithEvents(Of Tools.DrawingT.MetadataT.ExifT.SubIFD)
Imports Tools.ComponentModelT, System.Linq

#If Config <= Nightly Then 'Stage: Nightly
Namespace DrawingT.MetadataT.ExifT
    ''' <summary>Provides read-write access to Image File Directory of Exif data</summary>
    Public Class Ifd
        Implements IReportsChange
#Region "CTors"
        ''' <summary>Contains value of the <see cref="OriginalOffset"/> property</summary>
        <EditorBrowsable(EditorBrowsableState.Never)> Private ReadOnly _OriginalOffset As UInteger
        ''' <summary>Gtes original offset of the IFD</summary>
        ''' <returns>Original offset of IFD in Exif block. 0 if this instance was not constructed from <see cref="ExifIFDReader"/>.</returns>
        ''' <remarks>As this property is not wery important it has no CLS-compliant alternative.</remarks>
        <CLSCompliant(False), EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public ReadOnly Property OriginalOffset() As UInteger
            Get
                Return _OriginalOffset
            End Get
        End Property
        ''' <summary>CTor - empty IFD</summary>
        Public Sub New()
            AddHandler Records.Adding, AddressOf Records_Adding
            AddHandler Records.Added, AddressOf Records_Added
            AddHandler Records.Removing, AddressOf Records_Removing
            AddHandler Records.Removed, AddressOf Records_Removed
            AddHandler Records.Clearing, AddressOf Records_Clearing
            AddHandler Records.Cleared, AddressOf Records_Cleared
            AddHandler Records.ItemChanging, AddressOf Records_ItemChanging
            AddHandler Records.ItemChanged, AddressOf Records_ItemChanged
            AddHandler Records.ItemValueChanged, AddressOf Records_ItemValueChanged
            AddHandler Records.CollectionChanged, AddressOf Records_CollectionChanged
            Records.AllowAddCancelableEventsHandlers = False
            AddHandler SubIFDs.Adding, AddressOf SubIFDs_Adding
            AddHandler SubIFDs.Added, AddressOf SubIFDs_Added
            AddHandler SubIFDs.Removing, AddressOf SubIFDs_Removing
            AddHandler SubIFDs.Removed, AddressOf SubIFDs_Removed
            AddHandler SubIFDs.Clearing, AddressOf SubIFDs_Clearing
            AddHandler SubIFDs.Cleared, AddressOf SubIFDs_Cleared
            AddHandler SubIFDs.ItemChanging, AddressOf SubIFDs_ItemChanging
            AddHandler SubIFDs.ItemChanged, AddressOf SubIFDs_ItemChanged
            AddHandler SubIFDs.ItemValueChanged, AddressOf SubIFDs_ItemValueChanged
            AddHandler SubIFDs.CollectionChanged, AddressOf SubIFDs_CollectionChanged
            SubIFDs.AllowAddCancelableEventsHandlers = False
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        ''' 
        ''' <param name="AutoReadNext">Automatically read IFDs that follows this one</param>
        Public Sub New(ByVal Reader As ExifIFDReader, ByVal AutoReadNext As Boolean)
            Me.New()
            If Reader Is Nothing Then Exit Sub
            _OriginalOffset = Reader.Offest
            For Each rec As DirectoryEntry In Reader.Entries
                Records.Add(rec.Tag, New ExifRecord(rec.Data, rec.DataType, rec.Components, rec.DataType <> ExifDataTypes.ASCII AndAlso rec.DataType <> ExifDataTypes.Byte AndAlso rec.DataType <> ExifDataTypes.NA))
            Next rec
            ReadStandardSubIFDs(Reader)
            If AutoReadNext Then ReadNextIFDs(Reader)
        End Sub
        ''' <summary>Reads IFDs following this one</summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that read this IFD</param>
        ''' <remarks>This implementation reads all the IFDs that follows (are pointed by) this instance. Newly read IFDs are of type <see cref="IFD"/>.
        ''' <para>Note for inheritors: Derived class my chose to override this method and read IFDs of different type.</para></remarks>
        Protected Overridable Sub ReadNextIFDs(ByVal Reader As ExifIFDReader)
            Dim CurrentIfd As Ifd = Me
            Dim CurrentReader As ExifIFDReader = Reader
            While CurrentReader.NextIFD <> 0
                CurrentReader = New ExifIFDReader(CurrentReader.ExifReader, CurrentReader.NextIFD)
                CurrentIfd.Following = New Ifd(CurrentReader)
                CurrentIfd = CurrentIfd.Following
            End While
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        Public Sub New(ByVal Reader As ExifIFDReader)
            Me.New(Reader, False)
        End Sub
        ''' <summary>If overriden in derived class reads known subIFDs nested within this IFD.</summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD.</param>
        ''' <exception cref="ArgumentNullException">Can be thrown by overriden method when <paramref name="Reader"/> is null.</exception>
        ''' <remarks>This implementation does nothing.
        ''' <para>Note for inheritors: This method is called by CTor if the <see cref="IFD"/> class after all records have been initialized. This method is not intended to be called directly from user code.</para></remarks>
        Protected Overridable Sub ReadStandardSubIFDs(ByVal Reader As ExifIFDReader)
        End Sub
#End Region
#Region "Dictionaries event handlers"
#Region "Private handlers"
#Region "Records"
        Private Sub Records_Adding(ByVal sender As RecordDic, ByVal e As RecordDic.CancelableKeyValueEventArgs)
            OnRecordAdding(e)
        End Sub
        Private Sub Records_Added(ByVal sender As RecordDic, ByVal e As RecordDic.KeyValueEventArgs)
            OnRecordAdded(e)
        End Sub
        Private Sub Records_Removing(ByVal sender As RecordDic, ByVal e As RecordDic.CancelableKeyValueEventArgs)
            OnRecordRemoving(e)
        End Sub
        Private Sub Records_Removed(ByVal sender As RecordDic, ByVal e As RecordDic.KeyValueEventArgs)
            OnRecordRemoved(e)
        End Sub
        Private Sub Records_Clearing(ByVal sender As RecordDic, ByVal e As CancelMessageEventArgs)
            OnRecordsClearing(e)
        End Sub
        Private Sub Records_Cleared(ByVal sender As RecordDic, ByVal e As RecordDic.DictionaryItemsEventArgs)
            OnRecordsCleared(e)
        End Sub
        Private Sub Records_ItemChanging(ByVal sender As RecordDic, ByVal e As RecordDic.CancelableKeyValueEventArgs)
            OnRecordChanging(e)
        End Sub
        Private Sub Records_ItemChanged(ByVal sender As RecordDic, ByVal e As RecordDic.OldNewValueEventArgs)
            OnRecordChanged(e)
        End Sub
        Private Sub Records_ItemValueChanged(ByVal sender As RecordDic, ByVal e As RecordList.ItemValueChangedEventArgs)
            OnRecordValueChanged(e)
        End Sub
        Private Sub Records_CollectionChanged(ByVal sender As RecordDic, ByVal e As RecordDic.DictionaryChangedEventArgs)
            OnRecordsChanged(e)
            OnRecordsChanged(DirectCast(e, CollectionChangedEventArgsBase))
        End Sub
#End Region
#Region "SubIFDs"
        Private Sub SubIFDs_Adding(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.CancelableKeyValueEventArgs)
            OnSubIFDAdding(e)
        End Sub
        Private Sub SubIFDs_Added(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.KeyValueEventArgs)
            OnSubIFDAdded(e)
        End Sub
        Private Sub SubIFDs_Removing(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.CancelableKeyValueEventArgs)
            OnSubIFDRemoving(e)
        End Sub
        Private Sub SubIFDs_Removed(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.KeyValueEventArgs)
            OnSubIFDRemoved(e)
        End Sub
        Private Sub SubIFDs_Clearing(ByVal sender As SubIFDDic, ByVal e As CancelMessageEventArgs)
            OnSubIFDsClearing(e)
        End Sub
        Private Sub SubIFDs_Cleared(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.DictionaryItemsEventArgs)
            OnSubIFDsCleared(e)
        End Sub
        Private Sub SubIFDs_ItemChanging(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.CancelableKeyValueEventArgs)
            OnSubIFDChanging(e)
        End Sub
        Private Sub SubIFDs_ItemChanged(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.OldNewValueEventArgs)
            OnSubIFDChanged(e)
        End Sub
        Private Sub SubIFDs_ItemValueChanged(ByVal sender As SubIFDDic, ByVal e As SubIFDList.ItemValueChangedEventArgs)
            OnSubIFDValueChanged(e)
        End Sub
        Private Sub SubIFDs_CollectionChanged(ByVal sender As SubIFDDic, ByVal e As SubIFDDic.DictionaryChangedEventArgs)
            OnSubIFDsChanged(e)
            OnSubIFDsChanged(DirectCast(e, CollectionChangedEventArgsBase))
        End Sub
#End Region
#End Region
#Region "Protected handlers"
#Region "Records"
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.Adding">Adding</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled. In such case the <see cref="OperationCanceledException"/> is thrown by collection.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive</para>
        ''' <para>Calls <see cref="OnRecordAddingAlways"/></para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordAdding(ByVal e As RecordDic.CancelableKeyValueEventArgs)
            OnRecordAddingAlways(e, e.Newkey, e.Item)
        End Sub
        ''' <summary>Called everywhen when record is aded or replaced to the <see cref="Records"/> collection</summary>
        ''' <param name="e">Event arguments - can be used to cancel the event</param>
        ''' <param name="Key">Key being added or replaced</param>
        ''' <param name="Item">New item to pe placed at <paramref name="Key"/></param>
        ''' <exception cref="ArgumentOutOfRangeException"><paramref name="Key"/> is not valid <see cref="UShort"/> value</exception>
        ''' <remarks>This event can be cancelled.
        ''' <para>Called by <see cref="OnRecordAdding"/> and <see cref="OnRecordChanging"/></para>
        ''' <para>Note for inheritors: Alway call base class method.</para>
        ''' <para>This implementation cancels the event when record with key which is used as pointer to subIFD is passed and the record being passed is not UInt16, 1 element, fixed lenght.</para></remarks>
        Protected Overridable Sub OnRecordAddingAlways(ByVal e As CancelMessageEventArgs, ByVal Key As Integer, ByVal Item As ExifRecord)
            If Key < UShort.MinValue OrElse Key > UShort.MaxValue Then Throw New ArgumentOutOfRangeException(String.Format(ResourcesT.Exceptions.MustBeWithinRangeOfValuesOfType1, "Key", "UInt16"))
            If SubIFDs.ContainsKey(Key) AndAlso (Item.DataType.NumberOfElements <> 1 OrElse Item.Fixed = False OrElse Item.DataType.DataType <> ExifDataTypes.UInt16) Then _
                e.Cancel = True _
                : e.CancelMessage = ResourcesT.Exceptions.YouShouldNotReplaceRecordsWhichServesAsPointersToSubIFDsIfYouDoSoReplacementRecordMustBeOfTypeUInt16WithOneElementFixedLength
        End Sub
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.Added">Added</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordAdded(ByVal e As RecordDic.KeyValueEventArgs)
        End Sub
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.Removing">Removing</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled. In such case the <see cref="OperationCanceledException"/> is thrown by collection.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive</para>
        ''' <para>Calls <see cref="OnRecordRemovingAlways"/></para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordRemoving(ByVal e As RecordDic.CancelableKeyValueEventArgs)
            OnRecordRemovingAlways(e, e.Newkey, e.Item)
        End Sub
        ''' <summary>Called whenewer record is about to be removed from or replaced in the <see cref="Records"/> collection</summary>
        ''' <remarks>This event can be cancelled.
        ''' <para>Called by: <see cref="OnRecordRemoving"/>, <see cref="OnRecordsClearing"/>, <see cref="OnRecordChanging"/>.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>        
        Protected Overridable Sub OnRecordRemovingAlways(ByVal e As CancelMessageEventArgs, ByVal Key As Integer, ByVal Item As ExifRecord)
            If Key < UShort.MinValue OrElse Key > UShort.MaxValue Then Throw New ArgumentOutOfRangeException(String.Format(ResourcesT.Exceptions.MustBeWithinRangeOfValuesOfType1, "Key", "UInt16"))
            If SubIFDs.ContainsKey(Key) Then
                e.Cancel = True
                e.CancelMessage = ResourcesT.Exceptions.CannotRemoveRecordWhichPoitsToSubIFDRemoveSubIFDFirst
            End If
        End Sub

        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.Removed">Removed</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordRemoved(ByVal e As RecordDic.KeyValueEventArgs)
        End Sub
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.Clearing">Clearing</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled. In such case the <see cref="OperationCanceledException"/> is thrown by collection.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive. Handler is marked as CLS-incompliant although it has CLS-compliant header because all other handlers are CLS-incompliant.</para>
        ''' <para>Calls <see cref="OnRecordRemovingAlways"/> for each record in the <see cref="Records"/> collection.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordsClearing(ByVal e As CancelMessageEventArgs)
            For Each r In Me.Records
                OnRecordRemovingAlways(e, r.Key, r.Value)
                If e.Cancel Then Exit For
            Next
        End Sub
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.Cleared">Cleared</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordsCleared(ByVal e As RecordDic.DictionaryItemsEventArgs)
        End Sub
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.ItemChanging">ItemChanging</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive.</para>
        ''' <para>Calls <see cref="OnRecordRemovingAlways"/> and <see cref="OnRecordAddingAlways"/></para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordChanging(ByVal e As RecordDic.CancelableKeyValueEventArgs)
            OnRecordRemovingAlways(e, e.Newkey, e.Item)
            OnRecordAddingAlways(e, e.Newkey, e.Item)
        End Sub
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.ItemChanged">ItemChanged</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordChanged(ByVal e As RecordDic.OldNewValueEventArgs)
        End Sub
        ''' <summary>Handles the <see cref="Records">Records</see>.<see cref="DictionaryWithEvents.ItemValueChanged">ItemValueChanged</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordValueChanged(ByVal e As RecordList.ItemValueChangedEventArgs)
        End Sub
        ''' <summary>Handles any change of the <see cref="Records"/> collection or its item (the <see cref="DictionaryWithEvents.CollectionChanged"/> event)</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This method is not CLS-compliant, but there is CLS-compliant overload.
        ''' <para>Note for inheritors: Alwas call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnRecordsChanged(ByVal e As RecordDic.DictionaryChangedEventArgs)
            If OnRecordsChanged_OnStack.Count = 0 OrElse OnRecordsChanged_OnStack.Peek IsNot e Then
                OnRecordsChanged_OnStack.Push(e)
                Try
                    OnRecordsChanged(DirectCast(e, CollectionChangedEventArgsBase))
                Finally
                    OnRecordsChanged_OnStack.Pop()
                End Try
            End If
            OnChanged(e)
        End Sub
        ''' <summary>Indicates call stack of both <see cref="OnRecordsChanged"/> overloads</summary>
        ''' <remarks>This is here in order to CLS compliant call CLS incompliant and vice versa</remarks>
        Private OnRecordsChanged_OnStack As New Stack(Of RecordDic.DictionaryChangedEventArgs)
        ''' <summary>Handles any change of the <see cref="Records"/> collection or its item (the <see cref="DictionaryWithEvents.CollectionChanged"/> event)</summary>
        ''' <param name="e">Event arguments - this is actually instance of CLS-incompliant generic class <see cref="RecordDic.DictionaryChangedEventArgs"/>.</param>
        ''' <remarks>In case your language can use CLS-incompliant method, you should rather use CLS-incompliant overload of this methos.
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        ''' <exception cref="TypeMismatchException"><paramref name="e"/> is not of type <see cref="DictionaryWithEvents(Of TKey, TValue)"/>[<see cref="UShort"/>, <see cref="ExifRecord"/>].<see cref="RecordDic.DictionaryChangedEventArgs">DictionaryChangedEventArgs</see></exception>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Protected Overridable Sub OnRecordsChanged(ByVal e As CollectionChangedEventArgsBase)
            If Not TypeOf e Is RecordDic.DictionaryChangedEventArgs Then Throw New TypeMismatchException("e", e, GetType(RecordDic.DictionaryChangedEventArgs))
            If OnRecordsChanged_OnStack.Count = 0 OrElse OnRecordsChanged_OnStack.Peek IsNot e Then
                OnRecordsChanged_OnStack.Push(e)
                Try
                    OnRecordsChanged(DirectCast(e, RecordDic.DictionaryChangedEventArgs))
                Finally
                    OnRecordsChanged_OnStack.Pop()
                End Try
            End If
        End Sub
#End Region
#Region "SubIFDs"
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.Adding">Adding</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled. In such case the <see cref="OperationCanceledException"/> is thrown by collection.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive</para>
        ''' <para>This implementation does nothing.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDAdding(ByVal e As SubIFDDic.CancelableKeyValueEventArgs)
            OnSubIFDAddingAlways(e.Newkey, e.Item, e)
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.Added">Added</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDAdded(ByVal e As SubIFDDic.KeyValueEventArgs)
            OnSubIFDAddedAlways(e.Key, e.Item)
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.Removing">Removing</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled. In such case the <see cref="OperationCanceledException"/> is thrown by collection.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive</para>
        ''' <para>This implementation does nothing.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDRemoving(ByVal e As SubIFDDic.CancelableKeyValueEventArgs)
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.Removed">Removed</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDRemoved(ByVal e As SubIFDDic.KeyValueEventArgs)
            OnSubIFDRemovedAlways(e.Item)
        End Sub
        ''' <summary>Handles removal of subIFD from any reason</summary>
        ''' <param name="Item">Item that was removed</param>
        ''' <remarks>Called by <see cref="OnSubIFDRemoved"/>, <see cref="OnSubIFDsCleared"/>, <see cref="OnSubIFDChanged"/>.
        ''' <para>Sets <paramref name="Item"/>.<see cref="SubIFD.Exif">Exif</see> to null, <paramref name="Item"/>.<see cref="SubIFD.ParentIFD">ParentIFD</see> to null and <paramref name="Item"/>.<see cref="SubIFD.ParentRecord">ParentRecord</see> to zero.</para>
        ''' <para>Note for inherotors: Always call base class method.</para></remarks>
        Protected Overridable Sub OnSubIFDRemovedAlways(ByVal Item As SubIFD)
            Item.Exif = Nothing
            Item.ParentIFD = Nothing
            Item.ParentRecord = 0
        End Sub
        ''' <summary>Handles adding of subIFD from any reason before it is addaed. This event can be cancelled.</summary>
        ''' <param name="Item">Item being added</param>
        ''' <param name="e">Event that supports cancelling</param>
        ''' <param name="Key">Record number which points to subIFD being added. This is always value form range of <see cref="UShort"/>.</param>
        ''' <remarks>Called by <see cref="OnSubIFDAdding"/>, <see cref="OnSubIFDChanging"/>
        ''' <para>This method checks if subIFD can be added or not. <paramref name="Item"/>.<see cref="SubIFD.Exif"/> must be null or same as of this instance, <paramref name="Item"/>.<see cref="SubIFD.ParentIFD"/> must be null.
        ''' Also <paramref name="Key"/> must represent record which either is not present in current instance or is of type single <see cref="ExifDataTypes.UInt16"/>.</para>
        ''' <para>Note for inheritors: Always call base class methosd.</para></remarks>
        ''' <exception cref="ArgumentOutOfRangeException"><paramref name="Key"/> is not within range of values of <see cref="UInteger"/></exception>
        Protected Overridable Sub OnSubIFDAddingAlways(ByVal Key As Integer, ByVal Item As SubIFD, ByVal e As CancelMessageEventArgs)
            If Key < UInteger.MinValue OrElse Key > UInteger.MaxValue Then _
                Throw New ArgumentOutOfRangeException(String.Format(ResourcesT.Exceptions.MustBeWithinRangeOfValuesOfType1, "Key", "UInt16"))
            If Item Is Nothing Then
                e.Cancel = True
                e.CancelMessage = String.Format(ResourcesT.Exceptions.CannotBeNull, ResourcesT.Exceptions.SubIFD)
            ElseIf Item.Exif IsNot Nothing AndAlso Item.Exif IsNot Me.Exif Then
                e.Cancel = True
                e.CancelMessage = ResourcesT.Exceptions.ExifPofSubIFDBeingAddedReplacedMustBeEitherNullOrSameAsExifOfParentIFD
            ElseIf Item.ParentIFD IsNot Nothing Then
                e.Cancel = True
                e.CancelMessage = ResourcesT.Exceptions.ParentIFDOfSubIFDBeingAddedReplacedMustMeNull
            ElseIf Me.Records.ContainsKey(Key) AndAlso (Me.Records(Key).DataType.NumberOfElements <> 1 OrElse Me.Records(Key).DataType.DataType <> ExifDataTypes.UInt32) Then
                e.Cancel = True
                e.CancelMessage = ResourcesT.Exceptions.IfParentIFDAlreadyContainsRecordThatIsAboutToBecomeParentRecordOfSubIFDItMustOfTypeOneElementOfTypeUInt32
            End If
        End Sub
        ''' <summary>Handles adding of subIFD from any reason after it is added</summary>
        ''' <param name="Item">Item that was added</param>
        ''' <param name="Key">Key at which it was added</param>
        ''' <remarks>Sets <see cref="SubIFD.Exif"/>, <see cref="SubIFD.ParentIFD"/> and <see cref="SubIFD.ParentRecord"/> of <paramref name="Item"/>. Creates new record with number <paramref name="Key"/> if it does not exist yet.
        ''' <para>Called by <see cref="OnSubIFDAdded"/> and <see cref="OnSubIFDChanged"/></para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        Protected Overridable Sub OnSubIFDAddedAlways(ByVal Key As Integer, ByVal Item As SubIFD)
            Item.Exif = Me.Exif
            Item.ParentIFD = Me
            Item.ParentRecord = Key
            If Not Me.Records.ContainsKey(Key) Then
                Me.Records.Add(Key, New ExifRecord(New ExifRecordDescription(ExifDataTypes.UInt16, 1), 0US, True))
            End If
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.Clearing">Clearing</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled. In such case the <see cref="OperationCanceledException"/> is thrown by collection.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive. Handler is marked as CLS-incompliant although it has CLS-compliant header because all other handlers are CLS-incompliant.</para>
        ''' <para>This implementation does nothing.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDsClearing(ByVal e As CancelMessageEventArgs)
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.Cleared">Cleared</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDsCleared(ByVal e As SubIFDDic.DictionaryItemsEventArgs)
            For Each item In e.Items
                OnSubIFDRemovedAlways(item.Value)
            Next
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.ItemChanging">ItemChanging</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>The event can be cancelled.
        ''' <para>This handler is not CLS-compliant an there is no CLS-compliant alternaive</para>
        ''' <para>This implementation does nothing.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDChanging(ByVal e As SubIFDDic.CancelableKeyValueEventArgs)
            If e.Item IsNot Me(e.Newkey) Then _
                OnSubIFDAddingAlways(e.Newkey, e.Item, e)
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.ItemChanged">ItemChanged</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing.</para>
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDChanged(ByVal e As SubIFDDic.OldNewValueEventArgs)
            If e.Item IsNot e.OldValue Then
                OnSubIFDRemovedAlways(e.OldValue)
                OnSubIFDAddedAlways(e.Key, e.Item)
            End If
        End Sub
        ''' <summary>Handles the <see cref="SubIFDs">SubIFDs</see>.<see cref="DictionaryWithEvents.ItemValueChanged">ItemValueChanged</see> event</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This handler is not CLS-compliant an there is no CLS-compliant alternaive.
        ''' <para>This implementation does nothing.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDValueChanged(ByVal e As SubIFDList.ItemValueChangedEventArgs)
        End Sub
        ''' <summary>Handles any change of the <see cref="SubIFDs"/> collection or its item (the <see cref="DictionaryWithEvents.CollectionChanged"/> event)</summary>
        ''' <param name="e">Event arguments</param>
        ''' <remarks>This method is not CLS-compliant, but there is CLS-compliant overload.
        ''' <para>Note for inheritors: Alwas call base class method.</para></remarks>
        <CLSCompliant(False)> _
        Protected Overridable Sub OnSubIFDsChanged(ByVal e As SubIFDDic.DictionaryChangedEventArgs)
            If OnSubIFDsChanged_OnStack.Count = 0 OrElse OnSubIFDsChanged_OnStack.Peek IsNot e Then
                OnSubIFDsChanged_OnStack.Push(e)
                Try
                    OnSubIFDsChanged(DirectCast(e, CollectionChangedEventArgsBase))
                Finally
                    OnSubIFDsChanged_OnStack.Pop()
                End Try
            End If
            OnChanged(e)
        End Sub
        ''' <summary>Indicates call stack of both <see cref="OnSubIFDsChanged"/> overloads</summary>
        ''' <remarks>This is here in order to CLS compliant call CLS incompliant and vice versa</remarks>
        Private OnSubIFDsChanged_OnStack As New Stack(Of SubIFDDic.DictionaryChangedEventArgs)
        ''' <summary>Handles any change of the <see cref="SubIFDs"/> collection or its item (the <see cref="DictionaryWithEvents.CollectionChanged"/> event)</summary>
        ''' <param name="e">Event arguments - this is actually instance of CLS-incompliant generic class <see cref="SubIFDDic.DictionaryChangedEventArgs"/>.</param>
        ''' <remarks>In case your language can use CLS-incompliant method, you should rather use CLS-incompliant overload of this methos.
        ''' <para>Note for inheritors: Always call base class method.</para></remarks>
        ''' <exception cref="TypeMismatchException"><paramref name="e"/> is not of type <see cref="DictionaryWithEvents(Of TKey, TValue)"/>[<see cref="UShort"/>, <see cref="Exift.IfdExif"/>].<see cref="SubIFDDic.DictionaryChangedEventArgs">DictionaryChangedEventArgs</see></exception>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Protected Overridable Sub OnSubIFDsChanged(ByVal e As CollectionChangedEventArgsBase)
            If Not TypeOf e Is SubIFDDic.DictionaryChangedEventArgs Then Throw New TypeMismatchException("e", e, GetType(SubIFDDic.DictionaryChangedEventArgs))
            If OnSubIFDsChanged_OnStack.Count = 0 OrElse OnSubIFDsChanged_OnStack.Peek IsNot e Then
                OnSubIFDsChanged_OnStack.Push(e)
                Try
                    OnSubIFDsChanged(DirectCast(e, SubIFDDic.DictionaryChangedEventArgs))
                Finally
                    OnSubIFDsChanged_OnStack.Pop()
                End Try
            End If
        End Sub
#End Region
#End Region
        ''' <summary>Raised when value of member changes</summary>
        ''' <remarks><paramref name="e"/>Contain additional information that can be used in event-handling code (contains instance of generic class <see cref="IReportsChange.ValueChangedEventArgs(Of T)"/>)
        ''' <para>Changes of the <see cref="Exif"/> and the <see cref="Previous"/> property are not tracked.</para></remarks>
        ''' <seealso cref="OnChanged"/>
        Public Event Changed(ByVal sender As IReportsChange, ByVal e As System.EventArgs) Implements IReportsChange.Changed
        ''' <summary>Raises the <see cref="Changed"/> event, handles any change in current instance</summary>
        ''' <param name="e">Event argument. If the event is caused directly by this instance (not by nested object) the <paramref name="e"/> parameter is <see cref="IReportsChange.ValueChangedEventArgsBase"/>.</param>
        ''' <remarks>Changes of the <see cref="Exif"/> and the <see cref="Previous"/> property are not tracked</remarks>
        ''' <seelaso cref="Changed"/>
        Protected Overridable Sub OnChanged(ByVal e As EventArgs)
            RaiseEvent Changed(Me, e)
        End Sub
#End Region
        ''' <summary>Contains value of the <see cref="Exif"/> property</summary>
        Private _Exif As Exif
        ''' <summary>Gets instance of <see cref="Exif"/> this IPTC behaves as instance of</summary>
        ''' <value>Setting this property changes <see cref="Exif"/> property of all subsequent IFDs in <see cref="Following"/> linked-list and of all subIFDs in <see cref="SubIFDs"/>.</value>
        ''' <returns>Instance of the <see cref="MetadataT.ExifT.Exif"/> class this instance is associated with; or null if this instance is not associated with instance of <see cref="Exif"/>.</returns>
        ''' <exception cref="ArgumentException">Internal only: Value being set differs from value of the <see cref="Exif"/> property of <see cref="Previous"/> IFD (when <see cref="Previous"/> is non-null)</exception>
        Public Property Exif() As Exif
            Get
                Return _Exif
            End Get
            Friend Set(ByVal value As Exif)
                If value Is Exif Then Exit Property
                If Me.Previous IsNot Nothing AndAlso Me.Previous.Exif IsNot value Then _
                    Throw New ArgumentException(ResourcesT.Exceptions.CannotSetValueOfTheExifPropertyToOtherInstanceThenIsValueOfExifPropertyOfPreviousIFD)
                _Exif = value
                If Me.Following IsNot Nothing Then Me.Following.Exif = value
                For Each MySubIFD In Me.SubIFDs
                    MySubIFD.Value.Exif = value
                Next
                OnExifChanged()
            End Set
        End Property
        ''' <summary>If overriden in derived class performs derived class-specific tasks related to change of the <see cref="Exif"/> property</summary>
        ''' <remarks>Note for inheritors: You do not have to call base class method. <para>This implementation does nothing.</para></remarks>
        Protected Overridable Sub OnExifChanged()
        End Sub
#Region "Records"
        ''' <summary>Contains value of the <see cref="Records"/> property</summary>
        Private _Records As New RecordDic(False, True)
        ''' <summary>Records in this Image File Directory</summary>
        ''' <remarks>Record cannot be removed from or replaced in the collection when it points to subIFD. The <see cref="OperationCanceledException"/> is thrown in case of attempt to do so.</remarks>
        <CLSCompliant(False)> _
        Public ReadOnly Property Records() As RecordDic
            Get
                Return _Records
            End Get
        End Property
        ''' <summary>Gets or sets value of specified record</summary>
        ''' <param name="Type">Type of record specifies data types of recor as well as number of components</param>
        ''' <value>New value for record. New value is assigned even if old value is of incompatible type. If value is null an item is deleted.</value>
        ''' <returns>Value of record with tag number specified in <paramref name="Type"/> if type specifies that number of components can vary or if number of components match actual number of components in record. If there is no tag with specified number present in this IFD or number of components constraint is being violated null is returned.</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="Type"/> is null</exception>
        ''' <exception cref="OperationCanceledException">In setter: Such record alerady exists and points to subIFD.</exception>
        ''' <seelaso cref="Records"/>
        <CLSCompliant(False)> _
        Default Public Overridable Property Record(ByVal Type As ExifTagFormat) As ExifRecord
            Get
                If Type Is Nothing Then Throw New ArgumentNullException("Type")
                If Records.ContainsKey(Type.Tag) Then
                    With Records(Type.Tag)
                        If Array.IndexOf(Type.DataTypes, Records(Type.Tag).DataType.DataType) >= 0 Then
                            If Type.NumberOfElements = 0 OrElse Type.NumberOfElements = .DataType.NumberOfElements Then
                                Return Records(Type.Tag)
                            Else
                                Return Nothing
                            End If
                        Else
                            Return Nothing
                        End If
                    End With
                Else
                    Return Nothing
                End If
            End Get
            Set(ByVal value As ExifRecord)
                If Type Is Nothing Then Throw New ArgumentNullException("value", String.Format(ResourcesT.Exceptions.CannotBeSetToNull, "Record"))
                If value Is Nothing Then
                    If Records.ContainsKey(Type.Tag) Then Records.Remove(Type.Tag)
                Else
                    If Records.ContainsKey(Type.Tag) Then
                        Records(Type.Tag) = value
                    Else
                        Records.Add(Type.Tag, value)
                    End If
                End If
            End Set
        End Property
        ''' <summary>Gets or sets record by <see cref="Integer">integer</see> key</summary>
        ''' <param name="key">Number of record to get or set</param>
        ''' <returns>Record with given <paramref name="key"/></returns>
        ''' <value>If record with given <paramref name="key"/> exists it si replaced. If it does not exist it is added to the <see cref="Records"/> collection.</value>
        ''' <remarks>This is CLS-compliant overload or CLS-incompliant property.</remarks>
        ''' <exception cref="ArgumentOutOfRangeException"><paramref name="Key"/> is less than <see cref="UShort.MinValue"/> or greater than <see cref="UShort.MaxValue"/></exception>
        ''' <exception cref="KeyNotFoundException">In getter: <paramref name="key"/> is not member of <see cref="GetRecordKeys"/></exception>
        ''' <exception cref="OperationCanceledException">In setter: Record with given <paramref name="key"/> already exists and points to subIFD.</exception>
        ''' <seelaso cref="GetRecordKeys"/><seelaso cref="Records"/>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Default Public Property Record(ByVal key As Integer) As ExifRecord
            Get
                If key < UShort.MinValue OrElse key > UShort.MaxValue Then Throw New ArgumentOutOfRangeException("Key", ResourcesT.Exceptions.ExifRecordKeyMustBeValidUInt16Value)
                Return Records(key)
            End Get
            Set(ByVal value As ExifRecord)
                If key < UShort.MinValue OrElse key > UShort.MaxValue Then Throw New ArgumentOutOfRangeException("Key", ResourcesT.Exceptions.ExifRecordKeyMustBeValidUInt16Value)
                If Records.ContainsKey(key) Then Records(key) = value _
                Else Records.Add(key, value)
            End Set
        End Property
        ''' <summary>Gets all the keys in the <see cref="Records"/> collection</summary>
        ''' <returns><see cref="Records"/>.<see cref="RecordDic.Keys">Keys</see></returns>
        ''' <remarks>This function is here for languages which cannot consume CLS-incompliant property <see cref="Records"/>.</remarks>
        ''' <seelaso cref="Records"/><seelaso cref="Record"/>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Function GetRecordKeys() As IEnumerable(Of Integer)
            Return From key In Records.Keys Select CInt(key)
        End Function
#End Region
#Region "Linked list"
        ''' <summary>Contains value of the <see cref="Following"/> property</summary>
        Private _Following As Ifd
        ''' <summary>Gets or sets IFD that follows this IFD in Exif block</summary>
        ''' <returns><see cref="IFD"/> which follows current IFD or null if there is no folowing IFD</returns>
        ''' <value>Sets following IFD of current IFD.</value>
        ''' <remarks>By setting this property you detaches all the following IFDs from linked-list of IFDs and replaces them by value being set (this may optionally have set the <see cref="Following"/> property).
        ''' <para>This property is not initialized automatically when instance of <see cref="IFD"/> is created directly. Code that wants to utilize linked lists of IFDs must initialize this property itself - such as <see cref="MetadataT.ExifT.Exif"/> does.</para></remarks>
        ''' <exception cref="ArgumentException">Value being set have current instance as one of its <see cref="Following"/> IFDs =or= Value being set has non-null value of the <see cref="Previous"/> property. =or= Value being set has non-null <see cref="Exif"/> property which is different of <see cref="Exif"/> of this instance (including situation when <see cref="Exif"/> property of this instance is null and <see cref="Exif"/> property of value being set is non-null). =or= <see cref="Exif"/> is not null and value being set is already used as IFD at another position in <see cref="Exif"/>.</exception>
        ''' <exception cref="TypeMismatchException">Value being set is of type <see cref="SubIFD"/> -or- <see cref="Exif"/> is not null and this instance if <see cref="Exif">Exif</see>.<see cref="MetadataT.ExifT.Exif.IFD0">IFD0</see> and value being set is not of type <see cref="IFDMain"/>.</exception>
        Public Property Following() As Ifd
            Get
                Return _Following
            End Get
            Set(ByVal value As Ifd)
                If value Is Following Then Exit Property
                If value.Previous IsNot Nothing Then Throw New ArgumentException(ResourcesT.Exceptions.ThePreviousPropertyOfIFDBeingSetAsFollowingMustBeNull)
                If TypeOf value Is SubIFD Then Throw New TypeMismatchException("value", value, GetType(Ifd), ResourcesT.Exceptions.FollowingIFDCannotBeSubIFD)
                Dim old As Ifd = Me.Following
                Dim Current As Ifd = value.Following
                While Current IsNot Nothing
                    If Current Is Me Then Throw New ArgumentException(ResourcesT.Exceptions.AttemptToCreateCyclicLinkedListOfIFDs)
                    Current = Current.Following
                End While
                If value.Exif IsNot Nothing AndAlso value.Exif IsNot Exif Then _
                    Throw New ArgumentException(ResourcesT.Exceptions.FollowingIFDMustBeMemberOfSameExifOrBeMemberOfNoExifAsCurrentIFD)
                If Exif IsNot Nothing AndAlso Exif.ContainsIFD(value) Then _
                    Throw New ArgumentException(ResourcesT.Exceptions.GivenIFDIsAlreadyInUse)
                VerifyFollowing(value)
                If Following IsNot Nothing Then Following.Previous = Nothing
                _Following = value
                value.Previous = Me
                Current = value
                value.Exif = Me.Exif
                OnFollowingChanged(New IReportsChange.ValueChangedEventArgs(Of Ifd)(old, value, "Following"))
            End Set
        End Property
        ''' <summary>Handles change of the <see cref="Following"/> property.</summary>
        ''' <param name="e">Event arguments</param>
        Protected Overridable Sub OnFollowingChanged(ByVal e As IReportsChange.ValueChangedEventArgs(Of Ifd))
            OnChanged(e)
        End Sub
        ''' <summary>Performs additional verification of value being passed to the <see cref="Following"/> prooperty</summary>
        ''' <param name="Following">Value to verify</param>
        ''' <exception cref="Exception">Overriden method can throw any exception when it refuses to accept given value ias new value of the <see cref="Following"/> property</exception>
        ''' <exception cref="TypeMismatchException">This implementation throws <see cref="TypeMismatchException"/> when <see cref="Exif"/> is not null and this instance is <see cref="Exif">Exif</see>.<see cref="MetadataT.ExifT.Exif.IFD0">IFD0</see> and <paramref name="Following"/> is not of type <see cref="IFDMain"/> (so it is not necessary to perform this verification in <see cref="IFDMain"/> derived class).</exception>
        Protected Overridable Sub VerifyFollowing(ByVal Following As Ifd)
            If Me.Exif IsNot Nothing AndAlso Me Is Me.Exif.IFD0 AndAlso Not TypeOf Following Is IfdMain Then _
                Throw New TypeMismatchException(ResourcesT.Exceptions.TypeOfIFDFollowingAfterIFD0MustBeIFDMain, Following, GetType(IfdMain))
        End Sub
        ''' <summary>Contains value of the <see cref="Previous"/> property</summary>
        Private _Previous As Ifd
        ''' <summary>Gets IFD that precedes current IFD</summary>
        ''' <returns><see cref="IFD"/> which precedes current IFD; null when this IFD is first in linked-list of IFDs</returns>
        ''' <value>Set accessor of this property is not publicly accessible, so it cannot be set diretly. This property is set when instance is passed to <see cref="Following"/> property (and unset when it is removed from there).</value>
        ''' <remarks>Internal note: Value being set of this property is nohow checked. You should always ensure that <see cref="Following"/> of previous IFD is same as <see cref="Previous"/> of following IFD.</remarks>
        Public Property Previous() As Ifd
            Get
                Return _Previous
            End Get
            Friend Set(ByVal value As Ifd)
                _Previous = value
            End Set
        End Property
#End Region
#Region "SubIFDs"
        ''' <summary>Gets dictionary of subIFDs of this IFD.</summary>
        ''' <returns>Dictionary which contains all the subIFDs in this IFD. Each subIFD is pointed by one record of type <see cref="ExifDataTypes.UInt32"/>.</returns>
        ''' <seelaso cref="SubIFD"/>
        ''' <remarks>When an item is added to this dictionary it must have <see cref="SubIFD.Exif"/> either null or same is this instance and it must heva <see cref="SubIFD.ParentIFD"/> null. It also cannot be null itself.
        ''' Its <see cref="SubIFD.Exif"/>, <see cref="SubIFD.ParentIFD"/> and <see cref="SubIFD.ParentRecord"/> is set to appropriate values. If this instance contains record which will become parent record of subIFD being added it must be of type <see cref="ExifDataTypes.UInt16"/>. If there is no record with given number, such record is created. If any constraint is violated <see cref="OperationCanceledException"/> is thrown.
        ''' <para>When item is removed its <see cref="SubIFD.Exif"/>, <see cref="SubIFD.ParentIFD"/> are set to null.</para></remarks>
        ''' <seelaso cref="OnSubIFDAddedAlways"/>, <seelaso cref="OnSubIFDAddingAlways"/>, <seelaso cref="OnSubIFDRemovedAlways"/>
        <CLSCompliant(False)> _
        Public ReadOnly Property SubIFDs() As SubIFDDic
            <DebuggerStepThroughAttribute()> Get
                Return _SubIFDs
            End Get
        End Property
        ''' <summary>Gets or sets SubIFD pointed by record with given number</summary>
        ''' <param name="Key">Number of Exif record in current IFD which a) is pointer (getter) b) will become pointer (setter) of a) returned SubIFDs (getter) b) value being set (setter)</param>
        ''' <exception cref="ArgumentOutOfRangeException"><paramref name="Key"/> is less than <see cref="UShort.MinValue"/> or greater than <see cref="UShort.MaxValue"/></exception>
        ''' <exception cref="KeyNotFoundException">In getter: <paramref name="Key"/> does not exist (is not present in <see cref="GetSubIFDsKeys"/>)</exception>
        ''' <remarks>You can set value for key which is not present in <see cref="GetSubIFDsKeys"/>. If the key is present, record with this number must be of type <see cref="ExifDataTypes.UInt32"/></remarks>
        ''' <seelaso cref="SubIFDs"/><seelaso cref="GetSubIFDsKeys"/>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Property SubIFD(ByVal Key As Integer) As SubIFD
            Get
                If Key < UShort.MinValue OrElse Key > UShort.MaxValue Then Throw New ArgumentOutOfRangeException("Key", ResourcesT.Exceptions.SubIFDKeyMustBeValidUInt16Value)
                Return SubIFDs(Key)
            End Get
            Set(ByVal value As SubIFD)
                'TODO: Explain all the exceptions that can be thrown by setter
                If Key < UShort.MinValue OrElse Key > UShort.MaxValue Then Throw New ArgumentOutOfRangeException("Key", ResourcesT.Exceptions.SubIFDKeyMustBeValidUInt16Value)
                If SubIFDs.ContainsKey(Key) Then _
                    SubIFDs(Key) = value _
                Else SubIFDs.Add(Key, value)
            End Set
        End Property
        ''' <summary>Gets all the keys of <see cref="SubIFDs"/> dictionary</summary>
        ''' <returns><see cref="SubIFDs"/>.<see cref="SubIFDDic.Keys">Keys</see></returns>
        ''' <remarks>This method is here for compatibility with languages that cannot consume CLS-incompliant property <see cref="SubIFDs"/>.</remarks>
        ''' <seelaso cref="SubIFDs"/><seelaso cref="SubIFD"/>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Function GetSubIFDsKeys() As IEnumerable(Of Integer)
            Return From key In SubIFDs.Keys Select CInt(key)
        End Function
        ''' <summary>Contains value of the <see cref="SubIFDs"/> property</summary>
        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private _SubIFDs As New SubIFDDic(False, True)
#End Region
    End Class


#Region "IFD classes"
    ''' <summary>Exif main and thumbnail IFD</summary>
    Partial Class IFDMain : Inherits Ifd
        ''' <summary>CTor - empty IFD</summary>
        Public Sub New()
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        ''' <param name="AutoReadNext">Automatically read IFDs that follow this one</param>
        Public Sub New(ByVal Reader As ExifIFDReader, ByVal AutoReadNext As Boolean)
            MyBase.New(Reader, AutoReadNext)
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        Public Sub New(ByVal Reader As ExifIFDReader)
            Me.New(Reader, False)
        End Sub
        ''' <summary>Reads IFDs following this one</summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that read this IFD</param>
        ''' <remarks>This implementation reads all the IFDs that follows (are pointed by) this instance. Newly read IFDs are of type <see cref="IFDMain"/>.</remarks>
        Protected Overrides Sub ReadNextIFDs(ByVal Reader As ExifIFDReader)
            Dim CurrentIfd As Ifd = Me
            Dim CurrentReader As ExifIFDReader = Reader
            While CurrentReader.NextIFD <> 0
                CurrentReader = New ExifIFDReader(CurrentReader.ExifReader, CurrentReader.NextIFD)
                CurrentIfd.Following = New IfdMain(CurrentReader)
                CurrentIfd = CurrentIfd.Following
            End While
        End Sub
        ''' <summary>Gets or sets value of specified record</summary>
        ''' <param name="Type">Recognized tagname of record that determines data type as well as number of components</param>
        ''' <value>New value for record. New value is assigned even if old value is of incompatible type. If value is null an item is deleted.</value>
        ''' <returns>Value of record with tag number specified by <paramref name="Type"/> if type for this tag number specifies that number of components can vary or if number of components match actual number of components in record. If there is no tag with specified number present in this IFD or number of components constraint is being violated null is returned.</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="Type"/> is null</exception>
        <CLSCompliant(False)> _
        Default Public Overridable Overloads Property Record(ByVal Type As Tags) As ExifRecord
            Get
                Return MyBase.Record(Me.TagFormat(Type))
            End Get
            Set(ByVal value As ExifRecord)
                MyBase.Record(Me.TagFormat(Type)) = value
            End Set
        End Property
        ''' <summary>Reads known subIFDs nested within this IFD.</summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD.</param>
        ''' <exception cref="ArgumentNullException"><paramref name="Reader"/> is null.</exception>
        ''' <remarks><para>Note: This method is called by CTor if the <see cref="Ifd"/> class after all records have been initialized.
        ''' This method is not intended to be called directly from user code.</para></remarks>
        Protected Overrides Sub ReadStandardSubIFDs(ByVal Reader As ExifIFDReader)
            If Reader Is Nothing Then Throw New ArgumentNullException("Reader")
            Dim ExifIfd = Me.Record(Tags.ExifIFD)
            If ExifIfd IsNot Nothing Then
                Dim ExifSubIFDReader As New SubIFDReader(Reader.ExifReader, ExifIfd.Data, _
                     ExifReader.ExifSubIFDName, Reader, _
                     Reader.Entries.FindIndex(Function(a As DirectoryEntry) a.Tag = Tags.ExifIFD))
                Dim ExifSubIfd As New IfdExif(ExifSubIFDReader, True)
                Me.SubIFDs.Add(Tags.ExifIFD, ExifSubIfd)
            End If
            Dim GPSIfd = Me.Record(Tags.GPSIFD)
            If GPSIfd IsNot Nothing Then
                Dim GPSSubIFDReader As New SubIFDReader(Reader.ExifReader, GPSIfd.Data, _
                    ExifReader.GPSSubIFDName, Reader, _
                    Reader.Entries.FindIndex(Function(a As DirectoryEntry) a.Tag = Tags.GPSIFD))
                Dim GPSSubIfd As New IfdGps(GPSSubIFDReader, True)
                Me.SubIFDs.Add(Tags.GPSIFD, GPSSubIfd)
            End If
        End Sub
        ''' <summary>Handles adding of subIFD from any reason before it is addaed. This event can be cancelled.</summary>
        ''' <param name="Item">Item being added</param>
        ''' <param name="e">Event that supports cancelling</param>
        ''' <param name="Key">Record number which points to subIFD being added. This is always value form range of <see cref="UShort"/>.</param>
        ''' <remarks>This methods calls base class method <see cref="IFD.OnSubIFDAddingAlways"/>.
        ''' Then ensures that SubIFD on key <see cref="Tags.GPSIFD"/> is always of type <see cref="IFDGPS"/> and at <see cref="Tags.ExifIFD"/> is always of type <see cref="IFDExif"/></remarks>
        ''' <exception cref="ArgumentOutOfRangeException"><paramref name="Key"/> is not within range of values of <see cref="UInteger"/></exception>
        Protected Overrides Sub OnSubIFDAddingAlways(ByVal Key As Integer, ByVal Item As SubIFD, ByVal e As ComponentModelT.CancelMessageEventArgs)
            MyBase.OnSubIFDAddingAlways(Key, Item, e)
            If e.Cancel Then Exit Sub
            If Key = Tags.ExifIFD AndAlso Not TypeOf Item Is IfdExif Then
                e.Cancel = True
                e.CancelMessage = String.Format(ResourcesT.Exceptions.Key0CanHoldOnlyIFDOfType1, "ExifIFD", "IFDExif")
                Exit Sub
            End If
            If Key = Tags.GPSIFD AndAlso Not TypeOf Item Is IfdGps Then
                e.Cancel = True
                e.CancelMessage = String.Format(ResourcesT.Exceptions.Key0CanHoldOnlyIFDOfType1, "GPSIFD", "IFDGPS")
                Exit Sub
            End If
        End Sub
        ''' <summary>Gets or sets Exif IFD nested within this IFD</summary>
        ''' <returns>Exif IFD nested in this IFD or null</returns>
        ''' <value>You can set or replace Exif SubIFD by setting this property. By setting it to null you can remove it.</value>
        Public Property ExifSubIFD() As IfdExif
            Get
                If Me.SubIFDs.ContainsKey(Tags.ExifIFD) Then _
                     Return Me.SubIFDs(Tags.ExifIFD)
                Return Nothing
            End Get
            Set(ByVal value As IfdExif)
                If value Is Nothing Then
                    If Me.SubIFDs.ContainsKey(Tags.ExifIFD) Then Me.SubIFDs.Remove(Tags.ExifIFD)
                    Exit Property
                End If
                If Me.SubIFDs.ContainsKey(Tags.ExifIFD) Then Me.SubIFDs(Tags.ExifIFD) = value _
                Else Me.SubIFDs.Add(Tags.ExifIFD, value)
            End Set
        End Property
        ''' <summary>Gets or sets GPS IFD nested within this IFD</summary>
        ''' <returns>GPS IFD nested in this IFD or null</returns>
        ''' <value>You can set or replace GPS SubIFD by setting this property. By setting it to null you can remove it.</value>
        Public Property GPSSubIFD() As IfdGps
            Get
                If Me.SubIFDs.ContainsKey(Tags.GPSIFD) Then _
                     Return Me.SubIFDs(Tags.GPSIFD)
                Return Nothing
            End Get
            Set(ByVal value As IfdGps)
                If value Is Nothing Then
                    If Me.SubIFDs.ContainsKey(Tags.GPSIFD) Then Me.SubIFDs.Remove(Tags.GPSIFD)
                    Exit Property
                End If
                If Me.SubIFDs.ContainsKey(Tags.GPSIFD) Then Me.SubIFDs(Tags.GPSIFD) = value _
                Else Me.SubIFDs.Add(Tags.GPSIFD, value)
            End Set
        End Property
        ''' <summary>Gets value indicating if this Exif contains link to thumbnail</summary>
        ''' <returns>True if <see cref="Compression"/> is <see cref="CompressionValues.JPEG"/> and both <see cref="JPEGInterchangeFormat"/> and <see cref="JPEGInterchangeFormatLength"/> are set or <see cref="Compression"/> is <see cref="CompressionValues.uncompressed"/> and both <see cref="StripOffsets"/> and <see cref="StripByteCounts"/> are set.</returns>
        Public ReadOnly Property HasThumbnail() As Boolean
            Get
                If Me.Compression.HasValue Then
                    Select Case Me.Compression
                        Case CompressionValues.JPEG
                            Return Me.JPEGInterchangeFormat.HasValue AndAlso Me.JPEGInterchangeFormatLength.HasValue
                        Case CompressionValues.uncompressed
                            Return Me.StripOffsets IsNot Nothing AndAlso Me.StripByteCounts IsNot Nothing AndAlso Me.StripOffsets.Length > 0 AndAlso Me.StripByteCounts.Length > 0
                    End Select
                End If
                Return False
            End Get
        End Property
        ''' <summary>Gtes stream that contains raw thumbnail data</summary>
        ''' <param name="Reader">Original reader that was used to retrieve all exif information from image. The reader must contain exactly same data this IFD was constructed from otherwise corrupted thumbnail image may be returned.</param>
        ''' <returns>Stream to read image data. Format of image data depends on <see cref="Compression"/> and if <see cref="Compression"/> is <see cref="CompressionValues.uncompressed"/> also depends on <see cref="PhotometricInterpretation"/>. Returns null if <see cref="HasThumbnail"/> is false.</returns>
        ''' <exception cref="InvalidOperationException"><see cref="Compression"/> is <see cref="CompressionValues.uncompressed"/> and lengths of <see cref="StripOffsets"/> and <see cref="StripByteCounts"/> differs.</exception>
        ''' <exception cref="ArgumentNullException"><paramref name="Reader"/> is null.</exception>
        ''' <remarks>In order tu succsefully retrieve image thumbnail data the <paramref name="Reader"/>.<see cref="ExifReader.Stream"/> must be the same strem this IFD was constructed from and must not be closed.</remarks>
        ''' <seelaso cref="GetThumbnail"/>
        Public Function GetThumbnailRawStream(ByVal Reader As ExifReader) As IO.Stream
            If Not Me.HasThumbnail Then Return Nothing
            If Reader Is Nothing Then Throw New ArgumentNullException("Reader")
            Select Case Compression
                Case CompressionValues.JPEG
                    Return New IOt.ConstrainedReadOnlyStream(Reader.Stream, Me.JPEGInterchangeFormat, Me.JPEGInterchangeFormatLength)
                Case CompressionValues.uncompressed
                    If Me.StripOffsets.Length <> Me.StripByteCounts.Length Then Throw New InvalidOperationException(ResourcesT.Exceptions.ForUncompressedThumbnailStripOffsetsAndStripByteCountsMustHaveSameLength)
                    Dim Streams As New List(Of IO.Stream)
                    For i = 0 To Me.StripOffsets.Length - 1
                        Streams.Add(New IOt.ConstrainedReadOnlyStream(Reader.Stream, Me.StripOffsets(i), Me.StripByteCounts(i)))
                    Next
                    Return New IOt.UnionReadOnlyStream(Streams)
            End Select
            Return Nothing
        End Function
        ''' <summary>Gerts thumbnail image embdeded in this IFD</summary>
        ''' <param name="Reader">Original reader that was used to retrieve all exif information from image. The reader must contain exactly same data this IFD was constructed from otherwise corrupted thumbnail image may be returned.</param>
        ''' <exception cref="InvalidOperationException"><see cref="Compression"/> is <see cref="CompressionValues.uncompressed"/> and lengths of <see cref="StripOffsets"/> and <see cref="StripByteCounts"/> differs.
        ''' -or- <see cref="Compression"/> is <see cref="CompressionValues.uncompressed"/> and <see cref="PhotometricInterpretation"/> is not set or is not member of <see cref="PhotometricInterpretationValues"/>.</exception>
        ''' <exception cref="ArgumentNullException"><paramref name="Reader"/> is null.</exception>
        ''' <remarks>In order tu succsefully retrieve image thumbnail data the <paramref name="Reader"/>.<see cref="ExifReader.Stream"/> must be the same strem this IFD was constructed from and must not be closed.</remarks>
        ''' <seelaso cref="GetThumbnailRawStream"/>
        Public Function GetThumbnail(ByVal Reader As ExifReader) As Drawing.Bitmap
            Dim ImageData = GetThumbnailRawStream(Reader)
            If ImageData Is Nothing Then Return Nothing
            Select Case Me.Compression
                Case CompressionValues.JPEG : Return New Drawing.Bitmap(ImageData)
                Case CompressionValues.uncompressed
                    If Not Me.PhotometricInterpretation.HasValue Then Throw New InvalidOperationException(String.Format(ResourcesT.Exceptions.When0Is1Then2MustBeSet, "Compression", "uncompressed", "PhotometricInterpretation"))
                    Select Case PhotometricInterpretation
                        Case PhotometricInterpretationValues.RGB 'TODO: DOes it work?
                            Return New Drawing.Bitmap(ImageData)
                        Case PhotometricInterpretationValues.YCbCr 'TODO: Does it work?
                            Return New Drawing.Bitmap(ImageData)
                        Case Else : Throw New InvalidOperationException(String.Format("{0} is not member of {1}.", "PhotometricInterpretation", "PhotometricInterpretationValues"), New InvalidEnumArgumentException("PhotometricInterpretation", PhotometricInterpretation, PhotometricInterpretation.GetType))
                    End Select
            End Select
            Return Nothing
        End Function
    End Class
    ''' <summary>Exif Sub IFD</summary>
    Partial Class IFDExif : Inherits SubIFD
        ''' <summary>CTor - empty IFD</summary>
        Public Sub New()
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        Public Sub New(ByVal Reader As ExifIFDReader)
            Me.New(Reader, False)
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        ''' <param name="AutoReadNext">Automatically read IFDs tha follows this one</param>
        Public Sub New(ByVal Reader As ExifIFDReader, ByVal AutoReadNext As Boolean)
            MyBase.New(Reader, AutoReadNext)
        End Sub
        ''' <summary>Gets or sets value of specified record</summary>
        ''' <param name="Type">Recognized tagname of record that determines data type as well as number of components</param>
        ''' <value>New value for record. New value is assigned even if old value is of incompatible type. If value is null an item is deleted.</value>
        ''' <returns>Value of record with tag number specified by <paramref name="Type"/> if type for this tag number specifies that number of components can vary or if number of components match actual number of components in record. If there is no tag with specified number present in this IFD or number of components constraint is being violated null is returned.</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="Type"/> is null</exception>
        <CLSCompliant(False)> _
        Default Public Overridable Overloads Property Record(ByVal Type As Tags) As ExifRecord
            Get
                Return MyBase.Record(Me.TagFormat(Type))
            End Get
            Set(ByVal value As ExifRecord)
                MyBase.Record(Me.TagFormat(Type)) = value
            End Set
        End Property
        ''' <summary>Reads known subIFDs nested within this IFD.</summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD.</param>
        ''' <exception cref="ArgumentNullException"><paramref name="Reader"/> is null.</exception>
        ''' <remarks><para>Note: This method is called by CTor if the <see cref="IFD"/> class after all records have been initialized.
        ''' This method is not intended to be called directly from user code.</para></remarks>
        Protected Overrides Sub ReadStandardSubIFDs(ByVal Reader As ExifIFDReader)
            If Reader Is Nothing Then Throw New ArgumentNullException("Reader")
            Dim InteropIfd = Me.Record(Tags.InteroperabilityIFD)
            If InteropIfd IsNot Nothing Then
                Dim ExifSubIFDReader As New SubIFDReader(Reader.ExifReader, InteropIfd.Data, _
                     ExifReader.ExifSubIFDName, Reader, _
                     Reader.Entries.FindIndex(Function(a As DirectoryEntry) a.Tag = Tags.InteroperabilityIFD))
                Dim ExifSubIfd As New IfdInterop(ExifSubIFDReader, True)
                Me.SubIFDs.Add(Tags.InteroperabilityIFD, ExifSubIfd)
            End If
        End Sub
        ''' <summary>Gets or sets interoperability IFD nested within this IFD</summary>
        ''' <returns>Interoperability IFD nested in this ExifSubIFD or null</returns>
        ''' <value>You can set or replace interoperability SubIFD by setting this property. By setting it to null you can remove it.</value>
        Public Property InteropSubIFD() As IfdInterop
            Get
                If Me.SubIFDs.ContainsKey(Tags.InteroperabilityIFD) Then _
                     Return Me.SubIFDs(Tags.InteroperabilityIFD)
                Return Nothing
            End Get
            Set(ByVal value As IfdInterop)
                If value Is Nothing Then
                    If Me.SubIFDs.ContainsKey(Tags.InteroperabilityIFD) Then Me.SubIFDs.Remove(Tags.InteroperabilityIFD)
                    Exit Property
                End If
                If Me.SubIFDs.ContainsKey(Tags.InteroperabilityIFD) Then Me.SubIFDs(Tags.InteroperabilityIFD) = value _
                Else Me.SubIFDs.Add(Tags.InteroperabilityIFD, value)
            End Set
        End Property
        ''' <summary>Handles adding of subIFD from any reason before it is addaed. This event can be cancelled.</summary>
        ''' <param name="Item">Item being added</param>
        ''' <param name="e">Event that supports cancelling</param>
        ''' <param name="Key">Record number which points to subIFD being added. This is always value form range of <see cref="UShort"/>.</param>
        ''' <remarks>This methods calls base class method <see cref="IFD.OnSubIFDAddingAlways"/>.
        ''' Then ensures that SubIFD on key <see cref="Tags.InteroperabilityIFD"/> is always of type <see cref="IFDInterop"/>.</remarks>
        ''' <exception cref="ArgumentOutOfRangeException"><paramref name="Key"/> is not within range of values of <see cref="UInteger"/></exception>
        Protected Overrides Sub OnSubIFDAddingAlways(ByVal Key As Integer, ByVal Item As SubIFD, ByVal e As ComponentModelT.CancelMessageEventArgs)
            MyBase.OnSubIFDAddingAlways(Key, Item, e)
            If e.Cancel Then Exit Sub
            If Key <> Tags.InteroperabilityIFD AndAlso Not TypeOf Item Is IfdInterop Then
                e.Cancel = True
                e.CancelMessage = String.Format(ResourcesT.Exceptions.Key0CanHoldOnlyIFDOfType1, "InteroperabilityIFD", "IFDInterop")
                Exit Sub
            End If
        End Sub
    End Class
    ''' <summary>Exif GPS IFD</summary>
    Partial Class IFDGPS : Inherits SubIFD
        ''' <summary>CTor - empty IFD</summary>
        Public Sub New()
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        ''' <param name="AutoReadNext">Automatically read IFDs that follows this one</param>
        Public Sub New(ByVal Reader As ExifIFDReader, ByVal AutoReadNext As Boolean)
            MyBase.New(Reader, AutoReadNext)
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        Public Sub New(ByVal Reader As ExifIFDReader)
            Me.New(Reader, False)
        End Sub
        ''' <summary>Gets or sets value of specified record</summary>
        ''' <param name="Type">Recognized tagname of record that determines data type as well as number of components</param>
        ''' <value>New value for record. New value is assigned even if old value is of incompatible type. If value is null an item is deleted.</value>
        ''' <returns>Value of record with tag number specified by <paramref name="Type"/> if type for this tag number specifies that number of components can vary or if number of components match actual number of components in record. If there is no tag with specified number present in this IFD or number of components constraint is being violated null is returned.</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="Type"/> is null</exception>
        <CLSCompliant(False)> _
        Default Public Overridable Overloads Property Record(ByVal Type As Tags) As ExifRecord
            Get
                Return MyBase.Record(Me.TagFormat(Type))
            End Get
            Set(ByVal value As ExifRecord)
                MyBase.Record(Me.TagFormat(Type)) = value
            End Set
        End Property
    End Class
    ''' <summary>Exif Interoperability IFD</summary>
    Partial Class IFDInterop : Inherits SubIFD
        ''' <summary>CTor - empty IFD</summary>
        Public Sub New()
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        ''' <param name="AutoReadNext">Automatically read IFDs that follows this one</param>
        Public Sub New(ByVal Reader As ExifIFDReader, ByVal AutoReadNext As Boolean)
            MyBase.New(Reader, AutoReadNext)
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        Public Sub New(ByVal Reader As ExifIFDReader)
            Me.New(Reader, False)
        End Sub
        ''' <summary>Gets or sets value of specified record</summary>
        ''' <param name="Type">Recognized tagname of record that determines data type as well as number of components</param>
        ''' <value>New value for record. New value is assigned even if old value is of incompatible type. If value is null an item is deleted.</value>
        ''' <returns>Value of record with tag number specified by <paramref name="Type"/> if type for this tag number specifies that number of components can vary or if number of components match actual number of components in record. If there is no tag with specified number present in this IFD or number of components constraint is being violated null is returned.</returns>
        ''' <exception cref="ArgumentNullException"><paramref name="Type"/> is null</exception>
        <CLSCompliant(False)> _
        Default Public Overridable Overloads Property Record(ByVal Type As Tags) As ExifRecord
            Get
                Return MyBase.Record(Me.TagFormat(Type))
            End Get
            Set(ByVal value As ExifRecord)
                MyBase.Record(Me.TagFormat(Type)) = value
            End Set
        End Property
    End Class
    ''' <summary>Represents any Exif Sub-IFD (an IFD embdeded somewhere in IFD block and pointed by some tag from another IFD)</summary>
    Public Class SubIFD : Inherits Ifd
        ''' <summary>CTor - empty IFD</summary>
        Public Sub New()
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        ''' <param name="AutoReadNext">Atomatically read IFDs that follows this one</param>
        Public Sub New(ByVal Reader As ExifIFDReader, ByVal AutoReadNext As Boolean)
            MyBase.New(Reader, AutoReadNext)
        End Sub
        ''' <summary>CTor - reads content from <see cref="ExifIFDReader"/></summary>
        ''' <param name="Reader"><see cref="ExifIFDReader"/> that has read data of this IFD. Can be null</param>
        Public Sub New(ByVal Reader As ExifIFDReader)
            Me.New(Reader, False)
        End Sub
        ''' <summary>Gets IFD this subIFD is nested within</summary>
        ''' <returns>IFD this subIFD is nested within or null when this subIFD have not been associated with parent IFD yet.</returns>
        ''' <value>Setter of this property is not bublicly accessible. Value of this property is set when subIFD is associted with parent IFD.</value>
        ''' <seelaso cref="ParentRecord"/>
        Public Property ParentIFD() As Ifd
            <DebuggerStepThrough()> Get
                Return _ParentIFD
            End Get
            Friend Set(ByVal value As Ifd)
                _ParentIFD = value
            End Set
        End Property
        ''' <summary>Contains value of the <see cref="ParentIFD"/> property</summary>
        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private _ParentIFD As Ifd
        ''' <summary>Contains value of the <see cref="ParentRecord"/> property</summary>
        <EditorBrowsable(EditorBrowsableState.Never)> _
        Private _ParentRecord As UShort
        ''' <summary>Gets tag number of record this subIFD is pointed by</summary>
        ''' <returns>Tag number of record this subIFD is pointed by</returns>
        ''' <value>Setter of this property is not publicly accessible. Value of this property is set when subIFD is associated with parent IFD</value>
        ''' <remarks>This property is of type which is not CLS-compliant. Function <see cref="getParentRecord"/> returns value of this property in CLS-compliant type.
        ''' <para>When some Exif tag is referenced as parent record of some subIFD its value is meaningless - actually it is address of start of subIFD in Exif stream which can change after saving. Although you can change value of such tag, it has no effect. Value of this the tag is automatically computed when Exif is about to be saved.</para></remarks>
        ''' <seelaso cref="getParentRecord"/><seelaso cref="ParentIFD"/>
        <CLSCompliant(False)> _
        Public Property ParentRecord() As UShort
            <DebuggerStepThrough()> Get
                Return _ParentRecord
            End Get
            Friend Set(ByVal value As UShort)
                _ParentRecord = value
            End Set
        End Property
        ''' <summary>Returns value of the <see cref="ParentRecord"/> property in CLS-compliant type <see cref="Integer"/></summary>
        ''' <returns><see cref="ParentRecord"/></returns>
        ''' <remarks>This function is provided only for CLS-compliance reasons. You'd better use <see cref="ParentRecord"/> property.</remarks>
        ''' <seelaso cref="ParentRecord"/>
        <EditorBrowsable(EditorBrowsableState.Advanced)> _
        Public Function getParentRecord() As Integer
            Return ParentRecord
        End Function
        ''' <summary>Contains value of the <see cref="Desc"/> property</summary>
        Private _Desc As String
        ''' <summary>Descriptive name of this Sub IFD</summary>
        ''' <returns>Usually contain an empty string for non starndard Sub IFDs and comon English name for standard Sub IFDs. For non-standard Sub IFDs only when library have some ideda what can this Sub IFD mean this Sub IFD is captioned somehow</returns>
        ''' <remarks>Currently there are no Non Standard Sub IFDs that have any caption, Captions of standard Sub IFDs are public constants declared in <see cref="ExifReader"/></remarks>
        Public Property Desc() As String
            Get
                Return _Desc
            End Get
            Friend Set(ByVal value As String)
                _Desc = value
            End Set
        End Property
    End Class
#End Region
End Namespace
#End If