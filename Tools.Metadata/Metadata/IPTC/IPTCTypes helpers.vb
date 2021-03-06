Imports Tools.CollectionsT.GenericT, System.Globalization.CultureInfo, Tools.DataStructuresT.GenericT
Imports Tools.VisualBasicT.Interaction, Tools.ComponentModelT, Tools.DrawingT.DesignT
Imports System.Drawing.Design, System.Windows.Forms, System.Drawing
Imports Tools.MetadataT.IptcT.IptcDataTypes
Namespace MetadataT.IptcT
    Partial Public Class Iptc
        'TODO: Tune IPTC's behavior in PropertyGrid

        ''' <summary>Indicates if given string contains only graphic characters and spaces</summary>
        ''' <param name="Str">String to be verified</param>
        ''' <returns>True if string contains only graphic characters and spaces, false otherwise</returns>
        ''' <remarks>All characters with ASCII code higher than space are considered graphic</remarks>
        ''' <version version="1.5.4">Parameter <c>Str</c> renamed to <c>str</c></version>
        Public Shared Function IsTextWithSpaces(ByVal str As String) As Boolean
            For Each ch As Char In Str
                If AscW(ch) < AscW(" "c) Then Return False
            Next ch
            Return True
        End Function
        ''' <summary>Indicates if given string contains only graphic characters, spaces, Crs and Lfs</summary>
        ''' <param name="Str">String to be verified</param>
        ''' <returns>True if string contains only graphic characters, spaces, Crs and Lfs, false otherwise</returns>
        ''' <remarks>All characters with ASCII code higher than space are considered graphic</remarks>
        ''' <version version="1.5.4">Parameter <c>Str</c> renamed to <c>str</c></version>
        Public Shared Function IsText(ByVal str As String) As Boolean
            For Each ch As Char In Str
                If AscW(ch) < AscW(" "c) AndAlso AscW(ch) <> AscW(vbCr) AndAlso AscW(ch) <> AscW(vbLf) Then Return False
            Next ch
            Return True
        End Function
        ''' <summary>Indicates if given string contains only graphic characters</summary>
        ''' <param name="Str">String to be verified</param>
        ''' <returns>True if string contains only graphic characters, false otherwise</returns>
        ''' <remarks>All characters with ASCII code higher than space are considered graphic</remarks>
        ''' <version version="1.5.4">Parameter <c>Str</c> renamed to <c>str</c></version>
        Public Shared Function IsGraphicCharacters(ByVal str As String) As Boolean
            For Each ch As Char In Str
                If AscW(ch) < AscW(" "c) Then Return False
            Next ch
            Return True
        End Function
        ''' <summary>Indicates if given string contains only alpha characters</summary>
        ''' <param name="Str">String to be verified</param>
        ''' <returns>True if string contains only alpha characters, false otherwise</returns>
        ''' <version version="1.5.4">Parameter <c>Str</c> renamed to <c>str</c></version>
        Public Shared Function IsAlpha(ByVal str As String) As Boolean
            For Each ch As Char In Str
                Select Case AscW(ch)
                    Case AscW("a"c) To AscW("z"c), AscW("A"c) To AscW("Z"c)
                    Case Else : Return False
                End Select
            Next ch
            Return True
        End Function
        ''' <summary>Returns <see cref="Type"/> that is used to store values of particular <see cref="IPTCTypes">IPTC type</see></summary>
        ''' <param name="IPTCType">IPTC type to get <see cref="Type"/> for</param>
        ''' <exception cref="InvalidEnumArgumentException"><paramref name="IPTCType"/> is not member of <see cref="IPTCTypes"/></exception>
        ''' <version version="1.5.4">Parameter <c>IPTCType</c> renamed to <c>iptcType</c></version>
        Public Shared Function GetUnderlyingType(ByVal iptcType As IptcTypes) As Type
            Select Case IPTCType
                Case IptcTypes.Alpha : Return GetType(String)
                Case IptcTypes.AudioType : Return GetType(IptcAudioType)
                Case IptcTypes.Boolean_Binary : Return GetType(Boolean)
                Case IptcTypes.BW460 : Return GetType(Drawing.Bitmap)
                Case IptcTypes.Byte_Binary : Return GetType(Byte)
                Case IptcTypes.ByteArray : Return GetType(Byte())
                Case IptcTypes.CCYYMMDD : Return GetType(Date)
                Case IptcTypes.CCYYMMDDOmmitable : Return GetType(OmmitableDate)
                Case IptcTypes.Enum_Binary : Return GetType([Enum])
                Case IptcTypes.Enum_NumChar : Return GetType([Enum])
                Case IptcTypes.GraphicCharacters : Return GetType(String)
                Case IptcTypes.HHMMSS : Return GetType(TimeSpan)
                Case IptcTypes.HHMMSS_HHMM : Return GetType(Time)
                Case IptcTypes.ImageType : Return GetType(IptcImageType)
                Case IptcTypes.Num2_Str : Return GetType(NumStr2)
                Case IptcTypes.Num3_Str : Return GetType(NumStr3)
                Case IptcTypes.NumericChar : Return GetType(IConvertible)
                Case IptcTypes.StringEnum : Return GetType(DataStructuresT.GenericT.T1orT2(Of [Enum], String))
                Case IptcTypes.SubjectReference : Return GetType(IptcSubjectReference)
                Case IptcTypes.Text : Return GetType(String)
                Case IptcTypes.TextWithSpaces : Return GetType(String)
                Case IptcTypes.UNO : Return GetType(IptcUno)
                Case IptcTypes.UnsignedBinaryNumber : Return GetType(ULong)
                Case IptcTypes.UShort_Binary : Return GetType(UShort)
                Case Else : Throw New InvalidEnumArgumentException("IPTCType", IPTCType, GetType(IptcTypes))
            End Select
        End Function
        ''' <summary>Gets details about tag format</summary>
        ''' <param name="Tag">tag to get details for</param>
        ''' <exception cref="InvalidEnumArgumentException">Either <see cref="DataSetIdentification.RecordNumber"/> of <paramref name="Tag"/> is unknown or <see cref="DataSetIdentification.DatasetNumber"/> of <paramref name="Tag"/> is unknown within <see cref="DataSetIdentification.RecordNumber"/> of <paramref name="Tag"/></exception>
        ''' <version version="1.5.4">Parameter <c>Tag</c> renamed to <c>tag</c></version>
        Public Shared Function GetTag(ByVal tag As DataSetIdentification) As IptcTag
            Return GetTag(Tag.RecordNumber, Tag.DatasetNumber)
        End Function
        '#Region "Verificators"
        '        ''' <summary>Verifies if given value belongs to specific enumeration.</summary>
        '        ''' <param name="verify">Value to be verified</param>
        '        ''' <typeparam name="T">Type of enum to verify <paramref name="verify"/> in</typeparam>
        '        ''' <exception cref="InvalidEnumArgumentException"><paramref name="verify"/> is not member of <paramref name="T"/> and <paramref name="T"/> has no <see cref="RestrictAttribute"/> or it has <see cref="RestrictAttribute"/> se to false.</exception>
        '        ''' <exception cref="ArgumentException"><paramref name="T"/> is not <see cref="[Enum]"/> and <paramref name="T"/> has <see cref="RestrictAttribute"/> set to True or it has no <see cref="RestrictAttribute"/></exception>
        '        <CLSCompliant(False)> _
        '        Public Sub VerifyNumericEnum(Of T As {IConvertible, Structure})(ByVal verify As T)
        '            Dim Attrs As Object() = GetType(T).GetCustomAttributes(GetType(RestrictAttribute), False)
        '            If Attrs Is Nothing OrElse Attrs.Length = 0 OrElse DirectCast(Attrs(0), RestrictAttribute).Restrict = True Then _
        '                If Not InEnum(verify) Then Throw New InvalidEnumArgumentException("verify", verify.ToInt32(InvariantCulture), GetType(T))
        '        End Sub
        '        ''' <summary>Verifies if given value if valid fro unrestricted string enum</summary>
        '        ''' <param name="verify">Value to be verified</param>
        '        ''' <param name="Len">Maximal lenght of string</param>
        '        ''' <param name="Fixed">Is <paramref name="Len"/> fixed lenght</param>
        '        ''' <typeparam name="T">Type of enumeration</typeparam>
        '        ''' <exception cref="ArgumentException"><paramref name="T"/> is not enumeration -or- string value violates lenght constraint -or- string value contains invalid (non-aplha) character</exception>
        '        ''' <exception cref="InvalidEnumArgumentException">Enum value is not member of <paramref name="T"/></exception>
        '        <CLSCompliant(False)> _
        '        Public Sub VerifyStringEnumNR(Of T As {IConvertible, Structure})(ByVal verify As T1orT2(Of T, String), ByVal Len As Byte, ByVal Fixed As Boolean)
        '            If verify.contains1 Then
        '                VerifyNumericEnum(DirectCast(verify.value1, T))
        '            Else
        '                VerifyAlpha(verify.value2, Len, Fixed)
        '            End If
        '        End Sub
        '        ''' <summary>Verifye if given string contains only alpha characters</summary>
        '        ''' <param name="verify"><see cref="String"/> to be verified</param>
        '        ''' <param name="Len">Maximal allowed length of string</param>
        '        ''' <param name="Fixed">Is <paramref name="Len"/> fixed length</param>
        '        ''' <exception cref="ArgumentException"><paramref name="verify"/> contains non-alpha character or violates lenght constraint</exception>
        '        Public Sub VerifyAlpha(ByVal verify As String, ByVal Len As Byte, ByVal Fixed As Boolean)
        '            If (Fixed AndAlso verify.Length <> Len OrElse Len <> 0 AndAlso verify.Length > Len) OrElse Not IsAlpha(verify) Then Throw New ArgumentException("Non alpha character")
        '        End Sub
        '#End Region
#Region "Serializers and deserializers"
#Region "Deserializers"
        ''' <summary>Ready signed ingere from byte array</summary>
        ''' <param name="Count">Number of bytes to read (can be 1,2,4,8)</param>
        ''' <param name="Bytes">Byte array to read from</param>
        ''' <returns>Signed integer stored in given byte array</returns>
        ''' <exception cref="ArgumentException"><paramref name="Count"/> is not 1,2,4 or 8</exception>
        ''' <exception cref="System.IO.EndOfStreamException">There are not enough bytes in <paramref name="Bytes"/></exception>
        ''' <version version="1.5.4">Parameter names changed: <c>Count</c> to <c>count</c>, <c>Bytes</c> to <c>bytes</c></version>
        Protected Shared Function IntFromBytes(ByVal count As Byte, ByVal bytes As Byte()) As Long
            Dim Str As New IOt.BinaryReader(New System.IO.MemoryStream(Bytes), IOt.BinaryReader.ByteAlign.BigEndian)
            Select Case Count
                Case 1 'SByte
                    Return Str.ReadSByte
                Case 2 'Short
                    Return Str.ReadInt16
                Case 4 'Integer
                    Return Str.ReadInt32
                Case 8 'Long
                    Return Str.ReadInt64
                Case Else
                    Throw New ArgumentException(String.Format(ResourcesT.Exceptions.Only124And8BytesIntegersCanBeReadVia0, "IntFromBytes"))
            End Select
        End Function
        ''' <summary>Ready unsigned ingere from byte array</summary>
        ''' <param name="Count">Number of bytes to read (can be 1,2,4,8)</param>
        ''' <param name="Bytes">Byte array to read from</param>
        ''' <returns>Unsigned integer stored in given byte array</returns>
        ''' <exception cref="ArgumentException"><paramref name="Count"/> is not 1,2,4 or 8</exception>
        ''' <exception cref="System.IO.EndOfStreamException">There are not enough bytes in <paramref name="Bytes"/></exception>
        ''' <version version="1.5.4">Parameter names changed: <c>Count</c> to <c>count</c>, <c>Bytes</c> to <c>bytes</c></version>
        <CLSCompliant(False)> _
        Protected Shared Function UIntFromBytes(ByVal count As Byte, ByVal bytes As Byte()) As ULong
            Dim Str As New IOt.BinaryReader(New System.IO.MemoryStream(Bytes), IOt.BinaryReader.ByteAlign.BigEndian)
            Select Case Count
                Case 1 'Byte
                    Return Str.ReadByte
                Case 2 'UShort
                    Return Str.ReadUInt16
                Case 4 'UInteger
                    Return Str.ReadUInt32
                Case 8 'ULong
                    Return Str.ReadUInt64
                Case Else
                    Throw New ArgumentException(String.Format(ResourcesT.Exceptions.Only124And8BytesIntegersCanBeReadVia0, "UIntFromBytes"))
            End Select
        End Function
        ''' <summary>Converts array of bytes that contains string to number</summary>
        ''' <param name="Bytes">Bytest to be converted</param>
        ''' <returns>Number that can be converted at least to <see cref="Long"/> or <see cref="ULong"/></returns>
        ''' <exception cref="InvalidCastException">Cannot convert string stored in <paramref name="Bytes"/> to <see cref="Decimal"/></exception>
        ''' <version version="1.5.4">Name of parameter <c>Bytes</c> changed to <c>bytes</c></version>
        Protected Shared Function NumCharFromBytes(ByVal bytes As Byte()) As Decimal
            Dim Str As String = System.Text.Encoding.ASCII.GetString(Bytes)
            Return Str
        End Function
#End Region
#Region "Serializers"
        ''' <summary>Converts number to array of bytes in which the number is stored as ASCII string</summary>
        ''' <param name="Count">Number of bytes to get (0 means no limit)</param>
        ''' <param name="Number">Number to be stored</param>
        ''' <returns>Array of bytes that contains <paramref name="Count"/> bytes consisting of string representation of <paramref name="Number"/></returns>
        ''' <param name="Fixed"><paramref name="Count"/> represents fixed lenght of returned byte array (True) or maximal variable lenght (False)</param>
        ''' <exception cref="ArgumentException">
        ''' <paramref name="Count"/> is 0 and <paramref name="Fixed"/> is True -or-
        ''' <paramref name="Count"/> is not 0 and number cannot be stored in number of bytes specified in <paramref name="Count"/>
        ''' </exception>
        ''' <version version="1.5.4">Parameter names changed to camelCase</version>
        Protected Shared Function ToBytes(ByVal count As Byte, ByVal number As Decimal, Optional ByVal fixed As Boolean = True) As Byte()
            If Count = 0 And Fixed = True Then Throw New ArgumentException(String.Format(ResourcesT.Exceptions.CannotBe1When2Is3, "Len", 0, "Fixed", "true"))
            ToBytes = Nothing
            Try
                If Fixed Then
                    Return System.Text.Encoding.ASCII.GetBytes(Number.ToString(New String("0"c, Count), InvariantCulture))
                Else
                    Return System.Text.Encoding.ASCII.GetBytes(Number.ToString("0", InvariantCulture))
                End If
            Finally
                If ToBytes Is Nothing Then ToBytes = New Byte() {}
                If Count <> 0 AndAlso ToBytes.Length > Count Then Throw New ArgumentException(String.Format(ResourcesT.Exceptions.Number0CannotBeStoredIn1Bytes, Number, Count))
            End Try
        End Function
        ''' <summary>Converts signed integer to array of bytes</summary>
        ''' <param name="Count">Length of integral value and returned array (can be 1,2,4,8)</param>
        ''' <param name="Int">Value to be converted</param>
        ''' <returns>Array of bytes representing <paramref name="Int"/></returns>
        ''' <exception cref="ArgumentException"><paramref name="Count"/> is not 1,2,4 or 8</exception>
        ''' <exception cref="OverflowAction"><paramref name="Int"/>'s value does not fit into integral value of <paramref name="Count"/> bytes</exception>
        ''' <version version="1.5.4">Fixed bug: Method didnt't worked property when <paramref name="Count"/> was 1 (it always stored 0)</version>
        ''' <version version="1.5.4">Parameter names changed: <c>Count</c> to <c>count</c>, <c>Int</c> to <c>int</c></version>
        Protected Shared Function ToBytes(ByVal count As Byte, ByVal int As Long) As Byte()
            Dim Str As New System.IO.BinaryWriter(New System.IO.MemoryStream(Count))
            Select Case Count
                Case 1 'SByte
                    Str.Write(CSByte(Int))
                Case 2 'Short
                    Str.Write(MathT.LEBE(CShort(Int)))
                Case 4 'Integer
                    Str.Write(MathT.LEBE(CInt(Int)))
                Case 5 'Long
                    Str.Write(MathT.LEBE(CLng(Int)))
                Case Else
                    Throw New ArgumentException(String.Format(ResourcesT.Exceptions.Only124And8BytesIntegersCanBeReadVia0, "IntToBytes"))
            End Select
            Dim Arr(Count - 1) As Byte
            Dim Buff As Byte() = DirectCast(Str.BaseStream, System.IO.MemoryStream).GetBuffer
            Array.ConstrainedCopy(Buff, 0, Arr, 0, Count)
            Return Arr
        End Function
        ''' <summary>Converts unsigned integer to array of bytes</summary>
        ''' <param name="Count">Length of integral value and returned array (can be 1,2,4,8)</param>
        ''' <param name="Int">Value to be converted</param>
        ''' <returns>Array of bytes representing <paramref name="Int"/></returns>
        ''' <exception cref="ArgumentException"><paramref name="Count"/> is not 1,2,4 or 8</exception>
        ''' <exception cref="OverflowAction"><paramref name="Int"/>'s value does not fit into integral value of <paramref name="Count"/> bytes</exception>
        ''' <version version="1.5.4">Fixed bug: Method didnt't worked property when <paramref name="Count"/> was 1 (it always stored 0)</version>
        ''' <version version="1.5.4">Parameter names changed: <c>Count</c> to <c>count</c>, <c>Int</c> to <c>int</c></version>
        <CLSCompliant(False)>
        Protected Shared Function ToBytes(ByVal count As Byte, ByVal int As ULong) As Byte()
            Dim Str As New System.IO.BinaryWriter(New System.IO.MemoryStream(count))
            Select Case count
                Case 1 'SByte
                    Str.Write(CByte(int))
                Case 2 'Short
                    Str.Write(MathT.LEBE(CUShort(int)))
                Case 4 'Integer
                    Str.Write(MathT.LEBE(CUInt(int)))
                Case 8 'Long
                    Str.Write(MathT.LEBE(CULng(int)))
                Case Else
                    Throw New ArgumentException(String.Format(ResourcesT.Exceptions.Only124And8BytesIntegersCanBeReadVia0, "ToBytes"))
            End Select
            Dim Arr(count - 1) As Byte
            Dim Buff As Byte() = DirectCast(Str.BaseStream, System.IO.MemoryStream).GetBuffer
            Array.ConstrainedCopy(Buff, 0, Arr, 0, count)
            Return Arr
        End Function
#End Region
#End Region
    End Class
    ''' <summary>Types od data used by IPTC tags</summary>
    ''' <version version="1.5.4">Added <see cref="IptcTypes.PictureNumber"/></version>
    ''' <version version="1.5.4">Added <see cref="IptcTypes.UInt_Binary"/></version>
    Public Enum IptcTypes
        ''' <summary>Unsigned binary number of unknown length (represented by <see cref="ULong"/>)</summary>
        UnsignedBinaryNumber
        ''' <summary>Binary stored boolean value (can be stored in multiple bytes. If any of bytes is nonzero, value is true) (represented by <see cref="Boolean"/></summary>
        Boolean_Binary
        ''' <summary>Binary stored 1 byte long unsigned integer (represented by <see cref="Byte"/>)</summary>
        Byte_Binary
        ''' <summary>Binary stored 2 byte long unsigned integer (represented by <see cref="UShort"/>)</summary>
        UShort_Binary
        ''' <summary>Binary stored 4 byte long unsigned integer (represented by <see cref="UInteger"/>)</summary>
        ''' <version version="1.5.4">This enum member is new in version 1.5.4</version>
        UInt_Binary
        ''' <summary>Number of variable length stored as string.</summary>
        ''' <remarks>
        ''' <list type="table"><listheader><term>Length up to characters</term><description>Represented by</description></listheader>
        ''' <item><term>2</term><description><see cref="Byte"/></description></item>
        ''' <item><term>4</term><description><see cref="Short"/></description></item>
        ''' <item><term>9</term><description><see cref="Integer"/></description></item>
        ''' <item><term>19</term><description><see cref="Long"/></description></item>
        ''' <item><term>29</term><description><see cref="Decimal"/></description></item>
        ''' <item><term>unknown</term><description><see cref="Decimal"/></description></item>
        ''' </list>
        ''' </remarks>
        NumericChar
        ''' <summary>Grahic characters (no whitespaces, no control characters) (represented by <see cref="String"/>)</summary>
        GraphicCharacters
        ''' <summary>Graphic characters and spaces (no tabs, no CR, no LF, no control characters) (represented by <see cref="String"/>)</summary>
        TextWithSpaces
        ''' <summary>Printable text (no tabs, no control characters) (represented by <see cref="String"/>)</summary>
        Text
        ''' <summary>Black and white bitmap with width 460px (represented <see cref="Drawing.Bitmap"/>)</summary>
        BW460
        ''' <summary>Enumeration stored as binary number (represented by various enums)</summary>
        Enum_Binary
        ''' <summary>Enumeration stored as numeric string (represented by various enums)</summary>
        Enum_NumChar
        ''' <summary>Date stored as numeric characters in the YYYYMMDD format (represented by <see cref="Date"/>)</summary>
        CCYYMMDD
        ''' <summary>Date stored as numeric characters in the YYYYMMDD format (represented by <see cref="OmmitableDate"/>) Each component YYYY, MM and DD can be set to 0 is unknown</summary>
        CCYYMMDDOmmitable
        ''' <summary>Time stored as numeric characters (and the � sign) in format HHMMSS�HHMM (with time-zone offset from UTC) (represented by <see cref="Time"/></summary>
        HHMMSS_HHMM
        ''' <summary>Generic array of bytes (represented by array of <see cref="Byte"/>)</summary>
        ByteArray
        ''' <summary>Unique Object Identifier (represented by <see cref="UNO"/>)</summary>
        UNO
        ''' <summary>Combination of 2-digits number and optional <see cref="String"/> (represented by <see cref="T:Tools.DrawingT.IPTCt.NumStr2"/>)</summary>
        Num2_Str
        ''' <summary>Combination of 3-digits number and optional <see cref="String"/> (represented by <see cref="T:Tools.MetadataT.IPTCt.NumStr3"/>)</summary>
        Num3_Str
        ''' <summary>Subject reference (combination of IPR, subject number and description) (represented by <see cref="SubjectReference"/>)</summary>
        SubjectReference
        ''' <summary>Alphabetic characters from�latin alphabet (A-Z and a-z) (represented by <see cref="String"/>)</summary>
        Alpha
        ''' <summary>Enum which's values are strings (represented by various enums). Actual string value can be obtained via <see cref="Xml.Serialization.XmlEnumAttribute"/></summary>
        StringEnum
        ''' <summary>Type of image stored as numeric character and alphabetic character (represented by <see cref="ImageType"/>)</summary>
        ImageType
        ''' <summary>Type of audio stored as numeric character and alphabetic character (represented by <see cref="AudioType"/>)</summary>
        AudioType
        ''' <summary>Duration in hours, minutes and seconds. Represented by <see cref="TimeSpan"/></summary>
        HHMMSS
        ''' <summary>The picture number provides a universally unique reference to an image</summary>
        ''' <version version="1.5.4">This enumeration member is new in version 1.5.4</version>
        PictureNumber
    End Enum
End Namespace
