'In order to remove these statements see comment at the beginning of Pager.vb
#If AssemblyBuild <> 1 Then
Namespace DataPager
#End If

    ''' <summary>Base class for numeric systems</summary>
    ''' <remarks>
    ''' See <see>http://www.w3.org/TR/css3-lists/</see> for some info about exotic systems
    ''' Note that for displaying of some converted numbers form some system may be required unicode font installed. Some browsers (e.g. IE have bad fall-back behaviour so it cannot display correct characters even if fallback Unicode ric font is instaled)
    ''' My oppinion is that best fall back Unicode rich fonts are <see>Arial Unicode MS</see> or <see>Code2000</see>. Standard Windows fonts (Arial, Times New Roman, Courier New etc.) are rich enough for Greek and Cyrilic but not for circled forms and Unicode Roman numbers. Fonts like Batang Che, Ming Liu, MSsubsets of u Song etc. are oriented on specific subsets of Unicode and are not universal.
    ''' </remarks>
    Public MustInherit Class NumericSystem
        ''' <summary>Creates numeric system from its type</summary>
        ''' <param name="Type">The type of the numeric system</param>
        ''' <returns>Instance of numeric system ready to generate numbers</returns>
        ''' <exception cref="ArgumentException">When numeric system specified in <paramref name="Type"/> is not known</exception>
        Public Shared Function Create(ByVal Type As enmNumberingTypes) As NumericSystem
            Select Case Type
                Case enmNumberingTypes.Armenian
                    Return New Decades("0"c, "Ա"c, "Բ"c, "Գ"c, "Դ"c, "Ե"c, "Զ"c, "Է"c, "Ը"c, "Թ"c, _
                                       " "c, "Ժ"c, "Ի"c, "Լ"c, "Խ"c, "Ծ"c, "Կ"c, "Հ"c, "Ձ"c, "Ղ"c, _
                                       " "c, "Ճ"c, "Մ"c, "Յ"c, "Ն"c, "Շ"c, "Ո"c, "Չ"c, "Պ"c, "Ջ"c, _
                                       " "c, "Ռ"c, "Ս"c, "Վ"c, "Տ"c, "Ր"c, "Ց"c, "Ւ"c, "Փ"c, "Ք"c _
                            )
                Case enmNumberingTypes.CyrilicLower
                    Return New Alphabetic("абвгдежзиклмнопстухцчшщыэюя")
                Case enmNumberingTypes.CyrilicUpper
                    Return New Alphabetic("АБВГДЕЖЗИКЛМНОПСТУХЦЧШЩЫЭЮЯ")
                Case enmNumberingTypes.Czech
                    Return New Czech()
                Case enmNumberingTypes.DecimalMultiInCircle0
                    Return New Decadic(New String() {"⓪"c, "①"c, "②"c, "③"c, "④"c, "⑤"c, "⑥"c, "⑦"c, "⑧"c, "⑨"c}, 1, New String() {"⑩"c, "⑪"c, "⑫"c, "⑬"c, "⑭"c, "⑮"c, "⑯"c, "⑰"c, "⑱"c, "⑲"c, "⑳"c})
                Case enmNumberingTypes.DecimalMultiInCircle1
                    Return New Decadic(New String() {"⓪"c, "①"c, "②"c, "③"c, "④"c, "⑤"c, "⑥"c, "⑦"c, "⑧"c, "⑨"c}, 0, New String() {"⑩"c, "⑪"c, "⑫"c, "⑬"c, "⑭"c, "⑮"c, "⑯"c, "⑰"c, "⑱"c, "⑲"c, "⑳"c})
                Case enmNumberingTypes.DecimalSingeInCircle0
                    Return New Decadic(1, "⓪"c, "①"c, "②"c, "③"c, "④"c, "⑤"c, "⑥"c, "⑦"c, "⑧"c, "⑨"c)
                Case enmNumberingTypes.DecimalSingeInCircle1
                    Return New Decadic("⓪"c, "①"c, "②"c, "③"c, "④"c, "⑤"c, "⑥"c, "⑦"c, "⑧"c, "⑨"c)
                Case enmNumberingTypes.Georgian
                    Return New Decades("0"c, "ა"c, "ბ"c, "გ"c, "დ"c, "ე"c, "ვ"c, "ზ"c, "ჱ"c, "თ"c, _
                                       " "c, "ი"c, "კ"c, "ლ"c, "მ"c, "ნ"c, "ჲ"c, "ო"c, "პ"c, "ჟ"c, _
                                       " "c, "რ"c, "ს"c, "ტ"c, "ჳ"c, "ფ"c, "ქ"c, "ღ"c, "ყ"c, "შ"c, _
                                       " "c, "ჩ"c, "ც"c, "ძ"c, "წ"c, "ჭ"c, "ხ"c, "ჴ"c, "ჯ"c, "ჰ"c, _
                                       " "c, "ჵ"c _
                    )
                Case enmNumberingTypes.GreekLower
                    Return New Alphabetic("αβγδεζηθικλμνξοπστυφχψω")
                Case enmNumberingTypes.GreekUpper
                    Return New Alphabetic("ΑΒΓΔΕΖΗΘΙΚΛΜΝΞΟΠΣΤΥΦΧΨΩ")
                Case enmNumberingTypes.HexLower0
                    Return New Hexa(1, , True)
                Case enmNumberingTypes.HexLower0Leading0
                    Return New Hexa(1, True, True)
                Case enmNumberingTypes.HexLower1
                    Return New Hexa(Lower:=True)
                Case enmNumberingTypes.HexLower1Leading0
                    Return New Hexa(, True, True)
                Case enmNumberingTypes.HexUpper0
                    Return New Hexa(1)
                Case enmNumberingTypes.HexUpper0Leading0
                    Return New Hexa(1, True)
                Case enmNumberingTypes.HexUpper1
                    Return New Hexa
                Case enmNumberingTypes.HexUpper1Leading0
                    Return New Hexa(, True)
                Case enmNumberingTypes.LatinLower
                    Return New Alphabetic
                Case enmNumberingTypes.LatinUpper
                    Return New Alphabetic("ABCDEFGHIJKLMNOPQRSTUVWXYZ")
                Case enmNumberingTypes.LowerLatinInCircle
                    Return New Alphabetic("ⓐⓑⓒⓓⓔⓕⓖⓗⓘⓙⓚⓛⓜⓝⓞⓟⓠⓡⓢⓣⓤⓥⓦⓧⓨⓩ")
                Case enmNumberingTypes.Numbers0
                    Return New [Decimal](1)
                Case enmNumberingTypes.Numbers0Leading0
                    Return New DecimalLeaded(1)
                Case enmNumberingTypes.Numbers1
                    Return New [Decimal]
                Case enmNumberingTypes.Numbers1Leading0
                    Return New DecimalLeaded
                Case enmNumberingTypes.NumbersMinusPlus
                    Return New PlusMinus
                Case enmNumberingTypes.RomanLower
                    Return New Roman(False, False, True, False)
                Case enmNumberingTypes.RomanUpper
                    Return New Roman(False, False, False, False)
                Case enmNumberingTypes.RomanLowerUnicode
                    Return New Roman(True, False, True, False)
                Case enmNumberingTypes.RomanUpperUnicode
                    Return New Roman(True, False, False, False)
                Case enmNumberingTypes.RomanLowerⅻ
                    Return New Roman(True, True, True, False)
                Case enmNumberingTypes.RomanUpperⅫ
                    Return New Roman(True, True, False, False)
                Case enmNumberingTypes.RomanLowerBig
                    Return New Roman(False, False, True, True)
                Case enmNumberingTypes.RomanUpperBig
                    Return New Roman(False, False, False, True)
                Case enmNumberingTypes.RomanUpperBigUnicode
                    Return New Roman(True, False, False, True)
                Case enmNumberingTypes.RomalLowerBigUnicode
                    Return New Roman(True, False, True, True)
                Case enmNumberingTypes.UpperLatinInCircle
                    Return New Alphabetic("ⒶⒷⒸⒹⒺⒻⒼⒽⒾⒿⓀⓁⓂⓃⓄⓅⓆⓇⓈⓉⓊⓋⓌⓍⓎⓏ")
                Case enmNumberingTypes.Fractions
                    Return New Fractions
                Case enmNumberingTypes.CirclesTo10
                    Return New Combined(Combined.enmUpToModes.ByCount, Create(enmNumberingTypes.Numbers1), New Combined.SysComb(10, Create(enmNumberingTypes.DecimalMultiInCircle1)))
                Case enmNumberingTypes.CirclesTo20
                    Return New Combined(Combined.enmUpToModes.ByCount, Create(enmNumberingTypes.Numbers1), New Combined.SysComb(20, Create(enmNumberingTypes.DecimalMultiInCircle1)))
                Case enmNumberingTypes.CirclesTo9
                    Return New Combined(Combined.enmUpToModes.ByCount, Create(enmNumberingTypes.Numbers1), New Combined.SysComb(9, Create(enmNumberingTypes.DecimalMultiInCircle1)))
                Case enmNumberingTypes.CirclesToZlower
                    Return New Combined(Combined.enmUpToModes.ByCount, Create(enmNumberingTypes.LatinLower), New Combined.SysComb(26, Create(enmNumberingTypes.LowerLatinInCircle)))
                Case enmNumberingTypes.CirclesToZUpper
                    Return New Combined(Combined.enmUpToModes.ByCount, Create(enmNumberingTypes.LatinUpper), New Combined.SysComb(26, Create(enmNumberingTypes.UpperLatinInCircle)))
                Case enmNumberingTypes.FractionsTo10
                    Return New Combined(Combined.enmUpToModes.ByCount, Create(enmNumberingTypes.Numbers1), New Combined.SysComb(10, New Fractions))
                Case enmNumberingTypes.CzechTo10
                    Return New Combined(Combined.enmUpToModes.ByCount, Create(enmNumberingTypes.Numbers1), New Combined.SysComb(10, New Czech))
            End Select
            Throw New ArgumentException("Unknown numbering system", "Type")
        End Function
        ''' <summary>Returns number in numeric system</summary>
        ''' <param name="Index">The number to be converted</param>
        ''' <returns>Number in numeric system that is based on system's default base</returns>
        Public MustOverride Function Number(ByVal Index As UInteger) As String
        ''' <summary>Returns number in numeric system</summary>
        ''' <param name="Index">The number to be converted</param>
        ''' <param name="Current">Number that will be used as base for numbering system (if numbering system suports it)</param>
        ''' <param name="Count">Number that will be used as range for numbering system (if numbering system supports it)</param>
        ''' <returns>Number in numeric system based on <paramref name="Current"/> and limited by <paramref name="Count"/></returns>
        ''' <remarks>Derived class doesn't have to override this method if its results are same as results of <see cref="Number"/> with only one parameter</remarks>
        Public Overridable Function Number(ByVal Index As UInteger, ByVal Current As UInteger, ByVal Count As UInteger) As String
            Return Number(Index)
        End Function

        ''' <summary>Decadic system with shift</summary>
        Public Class [Decimal] : Inherits NumericSystem
            ''' <summary>Left shift</summary>
            Private _Offset As Integer = 0
            ''' <summary>CTor</summary>
            ''' <param name="Offset">Left shift</param>
            Public Sub New(Optional ByVal Offset As Integer = 0)
                _Offset = Offset
            End Sub
            ''' <summary>Decadic number (also so-called Arabic-Latin) possiby with shift</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <returns>Decimal number shifted left by offset</returns>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                Return Index - _Offset
            End Function
        End Class

        ''' <summary>Decadic system with shift and leading zeros</summary>
        Public Class DecimalLeaded : Inherits [Decimal]
            ''' <summary>CTor</summary>
            ''' <param name="Offset">Left shift</param>
            Public Sub New(Optional ByVal Offset As Integer = 0)
                MyBase.New(Offset)
            End Sub
            ''' <summary>Decadic number (also so-called Arabic-Latin) possibly with shift and leading zeros</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <param name="Current">Ignored</param>
            ''' <param name="Count">Number of digits of this number (shifted if applicable) defines final length of converted number with leading zeros added</param>
            ''' <returns>Number in numeric system based on <paramref name="Current"/> and limited by <paramref name="Count"/></returns>
            Public Overrides Function Number(ByVal Index As UInteger, ByVal Current As UInteger, ByVal Count As UInteger) As String
                Dim Num As String = Number(Index)
                Dim MaxN As String = MyBase.Number(Math.Max(Count - 1, 1))
                If MaxN(0) = "-"c Then MaxN = MaxN.Substring(1)
                Dim NumWM As String = Num
                If NumWM(0) = "-"c Then NumWM = NumWM.Substring(1)
                Dim ret As String = NumWM
                If NumWM.Length < MaxN.Length Then
                    ret = New String("0", MaxN.Length - NumWM.Length) & NumWM
                End If
                If Num <> NumWM Then ret = "-"c & ret
                Return ret
            End Function
        End Class

        ''' <summary>Represents decimal number with digits replaced by alternativ characters. The number can be shifted.</summary>
        ''' <remarks>
        ''' It is possible to define replacement for digits (0÷9) and it is also possible to define replacement for numbers (10÷20) that is applyed only to these numbers (not e.g. to numbers 110÷120)
        ''' Required Unicode support depends on replacement characters
        ''' </remarks>
        Public Class Decadic : Inherits [Decimal]
            ''' <summary>0÷9 numbers replacement</summary>
            ''' <remarks>This array must have exactly 10 members</remarks>
            Private From0To9() As String
            ''' <summary>10÷20 numbers replacement</summary>
            ''' <remarks>This array must have exactly 11 members</remarks>
            Private From10To20() As String
            ''' <summary>CTor</summary>
            ''' <param name="From0To9">0÷9 numbers replacement (array must have exactly 10 members)</param>
            Public Sub New(ByVal ParamArray From0To9 As String())
                Me.New(From0To9, 0, Nothing)
            End Sub
            ''' <summary>CTor</summary>
            ''' <param name="Offset">Left shift</param>
            ''' <param name="From0To9">0÷9 numbers replacement (array must have exactly 10 members)</param>
            Public Sub New(ByVal Offset As Integer, ByVal ParamArray From0To9 As String())
                Me.New(From0To9, Offset, Nothing)
            End Sub
            ''' <summary>CTor</summary>
            ''' <param name="From0To9">0÷9 numbers replacement (array must have exactly 10 members)</param>
            ''' <param name="Offset">Left shift</param>
            ''' <param name="From10To20">10÷20 numbers replacement (array must have exactly 11 members)</param>
            Public Sub New(ByVal From0To9 As String(), ByVal Offset As Integer, ByVal From10To20() As String)
                MyBase.New(Offset)
                If From0To9.Length <> 10 Then Throw New ArgumentException("From0To9 must have exactly 10 items", "From0To9")
                If From10To20 IsNot Nothing AndAlso From10To20.Length <> 11 Then Throw New ArgumentException("From10To20 must have exactly 11 items or be Nothing", "From0To9")
                Me.From0To9 = From0To9
                Me.From10To20 = From10To20
            End Sub

            ''' <summary>Returns number in numeric system</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <returns>Decadic number (possibly shifted) with digits replaced with alternative characters</returns>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                Dim Num As String = MyBase.Number(Index)
                If From10To20 IsNot Nothing AndAlso (Math.Abs(CInt(Num)) >= 10 AndAlso Math.Abs(CInt(Num)) <= 20) Then
                    For i As Integer = 10 To 20
                        Num = Num.Replace(CStr(i), From10To20(i - 10))
                    Next i
                Else
                    For i As Integer = 0 To 9
                        Num = Num.Replace(CStr(i), From0To9(i))
                    Next i
                End If
                Return Num
            End Function
        End Class

        ''' <summary>Represent numeric system that behaves as an alphabet</summary>
        ''' <remarks>
        ''' Lower latin alphabet is used as default and can be changed into alphabet with another characters and another number of characters
        ''' Required Unicode support depends on alphabet characters passed
        ''' </remarks>
        Public Class Alphabetic : Inherits NumericSystem
            ''' <summary>An alphabet</summary>
            Private Alphabet As String
            ''' <summary>CTor</summary>
            ''' <param name="Alphabet">An alphabet as string of characters - one place in the string represents one character in alpahbet</param>
            ''' <remarks>Characters in aplhabet should be unique, but there is no check being done)</remarks>
            Public Sub New(Optional ByVal Alphabet As String = "abcdefghijklmnopqrstuvwxyz")
                Me.Alphabet = Alphabet
            End Sub
            ''' <summary>Number converted into alphabetic numbering system</summary>
            ''' <param name="Index">Number to be converted</param>
            ''' <returns>Letter wich's position in the alphabet equals to <paramref name="Index"/>. If there are not enough letters in the alphabet more letters are returned.</returns>
            ''' <remarks>More letters format is ..., aa, ab, ac, .., az, ba, ...</remarks>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                If Index <= Alphabet.Length Then
                    Return Alphabet(Index - 1)
                Else
                    Return Number((Index - 1) \ Alphabet.Length) & Alphabet((Index - 1) Mod Alphabet.Length)
                End If
            End Function
        End Class

        ''' <summary>Hexa numbering system (possibly with offset)</summary>
        Public Class Hexa : Inherits NumericSystem
            Private Offset As Integer, Leaded, Lower As Boolean
            ''' <summary>CTor</summary>
            ''' <param name="Offset">Offset (left shift)</param>
            ''' <param name="Leaded">Add leading zeros</param>
            ''' <param name="Lower">Use small caps</param>
            Public Sub New(Optional ByVal Offset As Integer = 0, Optional ByVal Leaded As Boolean = False, Optional ByVal Lower As Boolean = False)
                Me.Offset = Offset
                Me.Leaded = Leaded
                Me.Lower = Lower
            End Sub
            ''' <summary>Returns number in numeric system</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <returns>Hexadecimal number equal to <paramref name="Index"/> (possibly shifted)</returns>
            ''' <remarks>This method adds no leading zeros</remarks>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                Index -= Offset
                Dim ret As String = Hex(Index).ToUpper
                If Lower Then ret = ret.ToUpper
                Return ret
            End Function
            ''' <summary>Returns number in numeric system</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <param name="Current">Ignored</param>
            ''' <param name="Count">The number of digits of this number (possibly shifted) defines lenght of converted numbers with added leading zeros</param>
            ''' <returns>Hexadecimal number equal to <paramref name="Index"/> possibly shifted and possibly with leading zeros</returns>
            Public Overloads Overrides Function Number(ByVal Index As UInteger, ByVal Current As UInteger, ByVal Count As UInteger) As String
                If Not Leaded Then
                    Return Number(Index)
                Else
                    Dim Max As Integer = Math.Max(Number(1).Length, Number(Math.Max(Count - 1, 1)).Length)
                    Dim ret As String = Number(Index)
                    If ret.Length < Max Then
                        ret = New String("0"c, Max - ret.Length) & ret
                    End If
                    Return ret
                End If
            End Function
        End Class

        ''' <summary>Roman numbers</summary>
        ''' <remarks>
        ''' It can be defined if numbers are constructed using lower/upper latin letters or using special unicode code points for lower/upper latin letters
        ''' <para>WARNING: Bar-codes behavior for big and very big numbers is EXPERIMENTAL! and supprorts only two levels of bars (one underscore and one overscore)</para>
        ''' <list><listheader>There are several level of required unicode support depending on settings</listheader>
        ''' <item>No special requirements apply when <see cref="Roman.UseUnicode"/> is False and <see cref="Roman.UseBigAndVeryBig"/> is False </item>
        ''' <item>Special diacritic and character combination/overlapping support is required for <see cref="Roman.UseBigAndVeryBig"/> True</item>
        ''' <item>Roman numbers support (in Numeric forms block) is required for <see cref="Roman.UseUnicode"/> True</item>
        ''' </list>
        ''' </remarks>
        Public Class Roman : Inherits NumericSystem
            ''' <summary>Use Unicode code points for numerals</summary>
            Private UseUnicode As Boolean
            ''' <summary>If <see cref="UseUnicode"/> is True and this is true then numerals I÷XII will be created as one character (composite glyph)</summary>
            Private UseSingleCharacter As Boolean
            ''' <summary>Use lower letters</summary>
            Private LowerCase As Boolean
            ''' <summary>Use overscore and underscore for large numbers</summary>
            ''' <remarks>
            ''' <para>EXPERIMENTAL</para>
            ''' only one underscore or overscore per numeral
            ''' </remarks>
            Private UseBigAndVeryBig As Boolean
            ''' <summary>CTor</summary>
            ''' <param name="Unicode">Use Unicode code-points instead of latin letters</param>
            ''' <param name="Ⅻ">Use one character for numbers I÷XII instead of composing them as usual (works only when <paramref name="Unicode"/> is True)</param>
            ''' <param name="Lower">Use lower letters</param>
            ''' <param name="Big">
            ''' Use underscore and overscore characters for big numbers
            ''' <para>EXPERIMENTAL</para>
            ''' only one underscore or overscore per numeral
            ''' </param>
            Public Sub New(ByVal Unicode As Boolean, ByVal Ⅻ As Boolean, ByVal Lower As Boolean, Optional ByVal Big As Boolean = False)
                Me.UseUnicode = Unicode
                Me.UseSingleCharacter = Ⅻ
                Me.LowerCase = Lower
                Me.UseBigAndVeryBig = Big
            End Sub
            ''' <summary>All available Roman numerals</summary>
            ''' <remarks>Named by unicode composite code points, valued by it's values</remarks>
            Private Enum AllNumbers As UShort
                ''' <summary>I (1)</summary>
                Ⅰ = 1
                ''' <summary>II (2)</summary>
                Ⅱ = 2
                ''' <summary>III (3)</summary>
                Ⅲ = 3
                ''' <summary>IV (4)</summary>
                Ⅳ = 4
                ''' <summary>V (5)</summary>
                Ⅴ = 5
                ''' <summary>VI (6)</summary>
                Ⅵ = 6
                ''' <summary>VII (7)</summary>
                Ⅶ = 7
                ''' <summary>VIII (8)</summary>
                Ⅷ = 8
                ''' <summary>IX (9)</summary>
                Ⅸ = 9
                ''' <summary>X (10)</summary>
                Ⅹ = 10
                ''' <summary>XI (11)</summary>
                Ⅺ = 11
                ''' <summary>XII (12)</summary>
                Ⅻ = 12
                ''' <summary>L (50)</summary>
                Ⅼ = 50
                ''' <summary>C (100)</summary>
                Ⅽ = 100
                ''' <summary>D (500)</summary>
                Ⅾ = 500
                ''' <summary>M (1000)</summary>
                Ⅿ = 1000
            End Enum
            ''' <summary>Basic Roman numerals</summary>
            ''' <remarks>
            ''' Corresponding values are aqual to values from <see cref="AllNumbers"/>
            ''' Named by unicode code points, valued by it's values
            ''' </remarks>
            Private Enum BaseNumbers
                ''' <summary>I</summary>
                Ⅰ = AllNumbers.Ⅰ
                ''' <summary>V</summary>
                Ⅴ = AllNumbers.Ⅴ
                ''' <summary>X</summary>
                Ⅹ = AllNumbers.Ⅹ
                ''' <summary>L</summary>
                Ⅼ = AllNumbers.Ⅼ
                ''' <summary>C</summary>
                Ⅽ = AllNumbers.Ⅽ
                ''' <summary>D</summary>
                Ⅾ = AllNumbers.Ⅾ
                ''' <summary>M</summary>
                Ⅿ = AllNumbers.Ⅿ
            End Enum
            ''' <summary>The character that corresponds to number from <see cref="AllNumbers"/></summary>
            ''' <param name="Number">Number's value</param>
            ''' <remarks>Composite Unicode character that represents roman number with <paramref name="Number"/> value</remarks>
            Private Function Unicode(ByVal Number As AllNumbers) As Char
                Return System.Enum.GetName(GetType(AllNumbers), Number)
            End Function
            ''' <summary>The character that corresponds to number from <see cref="BaseNumbers"/></summary>
            ''' <param name="Number">Number's value</param>
            ''' <returns>An Unicode character that represents roman number with <paramref name="Number"/> value</returns>
            Private Function Unicode(ByVal Number As BaseNumbers) As Char
                Return Unicode(CType(Number, AllNumbers))
            End Function
            ''' <summary>Converts Roman number into its uppercase string reprezentaion in latin aplhabet</summary>
            ''' <param name="Number">Number to be coverted</param>
            ''' <returns>String representing number that is containded in <see cref="AllNumbers"/></returns>
            ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="Number"/> is not member of <see cref="AllNumbers"/></exception>
            Private Function Lettrs(ByVal Number As AllNumbers) As String
                Select Case Number
                    Case AllNumbers.Ⅰ : Return "I"
                    Case AllNumbers.Ⅱ : Return "II"
                    Case AllNumbers.Ⅲ : Return "III"
                    Case AllNumbers.Ⅳ : Return "IV"
                    Case AllNumbers.Ⅴ : Return "V"
                    Case AllNumbers.Ⅵ : Return "VI"
                    Case AllNumbers.Ⅶ : Return "VII"
                    Case AllNumbers.Ⅷ : Return "VIII"
                    Case AllNumbers.Ⅸ : Return "IX"
                    Case AllNumbers.Ⅹ : Return "X"
                    Case AllNumbers.Ⅺ : Return "XI"
                    Case AllNumbers.Ⅻ : Return "XII"
                    Case AllNumbers.Ⅼ : Return "L"
                    Case AllNumbers.Ⅽ : Return "C"
                    Case AllNumbers.Ⅾ : Return "D"
                    Case AllNumbers.Ⅿ : Return "M"
                    Case Else : Throw New ArgumentOutOfRangeException("Number", "Number is not valid value of roman number tha has glyph in unicode")
                End Select
            End Function
            ''' <summary>Converts Roman number into its uppercase string reprezentaion in latin aplhabet</summary>
            ''' <param name="Number">Number to be coverted</param>
            ''' <returns>String representing number that is containded in <see cref="BaseNumbers"/></returns>
            ''' <exception cref="ArgumentOutOfRangeException">When <paramref name="Number"/> is not member of <see cref="BaseNumbers"/></exception>
            Private Function Letters(ByVal Number As BaseNumbers) As Char
                Return Letters(CType(Number, AllNumbers))
            End Function
            ''' <summary>Returns roman number corresponding to <paramref name="Index"/> value</summary>
            ''' <param name="Index">Number to be converted</param>
            ''' <returns>Roman number in format specified by properties of this instance</returns>
            ''' <remarks>
            ''' <list><listheader>Basic Roman numbers</listheader>
            ''' <item>I=Ⅰ=1</item>
            ''' <item>V=Ⅴ=5</item>
            ''' <item>X=Ⅹ=10</item>
            ''' <item>L=Ⅼ=50</item>
            ''' <item>C=Ⅽ=100</item>
            ''' <item>D=Ⅾ=500</item>
            ''' <item>M=Ⅿ=1000</item>
            ''' </list><list><listheader>Following numbers are part of EXPERIMENTAL behavoir and are generated only when <see cref="UseBigAndVeryBig"/> is True
            ''' </listheader>
            ''' <item>V̅=Ⅴ̅=5 000</item>
            ''' <item>X̅=Ⅹ̅=10 000</item>
            ''' <item>L̅=Ⅼ̅=50 000</item>
            ''' <item>C̅=Ⅽ̅=100 000</item>
            ''' <item>D̅=Ⅾ̅=500 000</item>
            ''' <item>M̅=Ⅿ̅=1000 000</item>
            ''' <item>V̲=Ⅴ̲=5 000 000</item>
            ''' <item>X̲=Ⅹ̲=10 000 000</item>
            ''' <item>L̲=Ⅼ̲=50 000 000</item>
            ''' <item>C̲=Ⅽ̲=100 000 000</item>
            ''' <item>D̲=Ⅾ̲=500 000 000</item>
            ''' <item>M̲=Ⅿ̲=1000 000 000</item>
            ''' </list>
            ''' </remarks>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                If UseSingleCharacter AndAlso Index <= 12 Then
                    If LowerCase Then
                        Return Char.ToLower(Unicode(CType(Index, AllNumbers)))
                    Else
                        Return Unicode(CType(Index, AllNumbers))
                    End If
                End If
                Dim I As Char = "I"c
                Dim X As Char = "X"c
                Dim V As Char = "V"c
                Dim L As Char = "L"c
                Dim C As Char = "C"c
                Dim D As Char = "D"c
                Dim M As Char = "M"c
                If LowerCase And Not UseUnicode Then
                    I = "i"c
                    V = "v"c
                    X = "x"c
                    L = "l"c
                    C = "c"c
                    D = "d"c
                    M = "m"c
                ElseIf LowerCase And UseUnicode Then
                    I = "ⅰ"c
                    V = "ⅴ"c
                    X = "ⅹ"c
                    L = "ⅼ"c
                    C = "ⅽ"c
                    D = "ⅾ"c
                    M = "ⅿ"c
                ElseIf Not LowerCase And UseUnicode Then
                    I = "Ⅰ"c
                    V = "Ⅴ"c
                    X = "Ⅹ"c
                    L = "Ⅼ"c
                    C = "Ⅽ"c
                    D = "Ⅾ"c
                    M = "Ⅿ"c
                End If
                Dim NPart As UInteger = Index
                Dim ret As String = ""
                Dim Part As UInteger
                If UseBigAndVeryBig Then
                    '*1 000 000
                    'M
                    Part = NPart \ 1000000000
                    ret &= Repeat(M & "̲"c, Part)
                    NPart -= Part * 1000000000
                    'CM
                    If NPart >= 900000000 Then
                        ret &= C & "̲"c & M & "̲"c
                        NPart -= 900000000
                    End If
                    'D
                    If NPart >= 500000000 Then
                        ret &= D & "̲"c
                        NPart -= 500000000
                    End If
                    'CD
                    If NPart >= 400000 Then
                        ret &= C & "̲"c & D & "̲"c
                        NPart -= 400000000
                    End If
                    'C
                    Part = NPart \ 100000000
                    ret &= Repeat(C & "̲"c, Part)
                    NPart -= Part * 100000000
                    'XC
                    If NPart >= 90000000 Then
                        ret &= X & "̲"c & C & "̲"c
                        NPart -= 90000000
                    End If
                    'L
                    If NPart >= 50000000 Then
                        ret &= L & "̲"c
                        NPart -= 50000000
                    End If
                    'XL
                    If NPart >= 40000000 Then
                        ret &= X & "̲"c & L & "̲"c
                        NPart -= 40000000
                    End If
                    'X
                    Part = NPart \ 10000000
                    ret &= Repeat(X & "̲"c, Part)
                    NPart -= Part * 10000000
                    'V
                    If NPart >= 5000000 Then
                        ret &= V & "̲"c
                        NPart -= 5000000
                    End If

                    '*1 000
                    'M
                    Part = NPart \ 1000000
                    ret &= Repeat(M & "̅"c, Part)
                    NPart -= Part * 1000000
                    'CM
                    If NPart >= 900000 Then
                        ret &= C & "̅"c & M & "̅"c
                        NPart -= 900000
                    End If
                    'D
                    If NPart >= 500000 Then
                        ret &= D & "̅"c
                        NPart -= 500000
                    End If
                    'CD
                    If NPart >= 400000 Then
                        ret &= C & "̅"c & D & "̅"c
                        NPart -= 400000
                    End If
                    'C
                    Part = NPart \ 100000
                    ret &= Repeat(C & "̅"c, Part)
                    NPart -= Part * 100000
                    'XC
                    If NPart >= 90000 Then
                        ret &= X & "̅"c & C & "̅"c
                        NPart -= 90000
                    End If
                    'L
                    If NPart >= 50000 Then
                        ret &= L & "̅"c
                        NPart -= 50000
                    End If
                    'XL
                    If NPart >= 40000 Then
                        ret &= X & "̅"c & L & "̅"c
                        NPart -= 40000
                    End If
                    'X
                    Part = NPart \ 10000
                    ret &= Repeat(X & "̅"c, Part)
                    NPart -= Part * 10000
                    'V
                    If NPart >= 5000 Then
                        ret &= V & "̅"c
                        NPart -= 5000
                    End If
                End If
                'M
                Part = NPart \ 1000
                ret &= New String(M, Part)
                NPart -= Part * 1000
                'CM
                If NPart >= 900 Then
                    ret &= C & M
                    NPart -= 900
                End If
                'D
                If NPart >= 500 Then
                    ret &= D
                    NPart -= 500
                End If
                'CD
                If NPart >= 400 Then
                    ret &= C & D
                    NPart -= 400
                End If
                'C
                Part = NPart \ 100
                ret &= New String(C, Part)
                NPart -= Part * 100
                'XC
                If NPart >= 90 Then
                    ret &= X & C
                    NPart -= 90
                End If
                'L
                If NPart >= 50 Then
                    ret &= L
                    NPart -= 50
                End If
                'XL
                If NPart >= 40 Then
                    ret &= X & L
                    NPart -= 40
                End If
                'X
                Part = NPart \ 10
                ret &= New String(X, Part)
                NPart -= Part * 10
                If NPart = 9 Then
                    ret &= I & X
                Else
                    If NPart >= 5 Then
                        ret &= V
                        NPart -= 5
                    End If
                    If NPart = 4 Then ret &= I & V Else ret &= New String(I, NPart)
                End If
                Return ret
            End Function
            ''' <summary>Repeat string</summary>
            ''' <param name="What"><see cref="String"/> to repeat</param>
            ''' <param name="Count">Number of repetitions</param>
            ''' <returns><see cref="String"/> tha consists of <paramref name="Count"/> <paramref name="What"/> Strings</returns>
            Private Function Repeat(ByVal What As String, ByVal Count As Integer) As String
                Dim ret As String = ""
                For i As Integer = 1 To Count
                    ret &= What
                Next i
                Return ret
            End Function
        End Class

        ''' <summary>Decadic numbers centered around actual value</summary>
        Public Class PlusMinus : Inherits NumericSystem
            ''' <summary>Returns number in numeric system</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <returns>Number in numeric system that is based on system's default base</returns>
            ''' <remarks>This returns the same number as corresponding method of the <see cref="Decadic"/> class</remarks>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                Return Index
            End Function
            ''' <summary>Returns number in numeric system</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <param name="Current">The number that will have value 0 if converted</param>
            ''' <param name="Count">Ignored</param>
            ''' <returns>If <paramref name="Index"/> equals to <paramref name="Current"/> then return value is 0, if it is smaller then return value is negative number otherwise positive</returns>
            ''' <remarks>Derived class must support this method even if numeric system reprezented by such class doesn't utilize basing and limiting</remarks>
            Public Overloads Overrides Function Number(ByVal Index As UInteger, ByVal Current As UInteger, ByVal Count As UInteger) As String
                Return CInt(Index) - Current
            End Function
        End Class

        ''' <summary>Czech-words based numbering system</summary>
        ''' <remarks>This class is Czech specific</remarks>
        Public Class Czech : Inherits NumericSystem
            ''' <summary>Names of three-zeros decades in Czech </summary>
            ''' <remarks>
            ''' This is <see cref="Array"/> of <see cref="Array"/> of <see cref="String"/> with following sematics:
            ''' Sub elements: singular, dual, plural
            ''' First element should contain enmty strings, second names for thousands, third for milions, ...
            ''' </remarks>
            Private Shared ReadOnly Exponentos As String()() = { _
                New String() {"", "", ""}, _
                New String() {"tisíc", "tisíce", "tisíc"}, _
                New String() {"milión", "milióny", "miliónů"}, _
                New String() {"miliada", "miliardy", "miliard"}, _
                New String() {"bilión", "bilióny", "biliónů"}, _
                New String() {"biliarda", "biliard", "biliard"}, _
                New String() {"trilión", "trilióny", "triliónů"}, _
                New String() {"triliarda", "triliardy", "triliard"}, _
                New String() {"kvadrilión", "kvadrilióny", "kvadriliónů"}, _
                New String() {"kvadriliarda", "kvadriliardy", "kvadriliard"} _
            }
            ''' <summary>Names of single numbers in several contexts</summary>
            ''' <remarks>
            ''' This is <see cref="Array"/> of <see cref="Array"/> of <see cref="String"/> with following sematics:
            ''' <list>
            ''' <item>(0) - standalone numerals and numerals at 10^0 position</item>
            ''' <item>(1) - teens (10÷19)</item>
            ''' <item>(2) - hundrendts</item>
            ''' <item>(3) - thousands + milions + miliards ...</item>
            ''' <item>(4) - names of tens (numeral at 10^1 position)</item>
            ''' </list>
            ''' </remarks>
            Private Shared ReadOnly Decimals As String()() = { _
                New String() {"nula", "deset", "nula", "nula", "nula"}, _
                New String() {"jedna", "jedenáct", "sto", "", "deset"}, _
                New String() {"dva", "dvanáct", "dvě stě", "dva", "dvacet"}, _
                New String() {"tři", "třináct", "tři sta", "tři", "třicet"}, _
                New String() {"čtyři", "čtrnáct", "čtyři sta", "čtyři", "čtyřicet"}, _
                New String() {"pět", "patnáct", "pět set", "pět", "padesát"}, _
                New String() {"šest", "šestnáct", "šest set", "šest", "šedesát"}, _
                New String() {"sedm", "sedmnáct", "sedm set", "sedm", "sedmdesát"}, _
                New String() {"osm", "osumnáct", "osm set", "osm", "osmdesát"}, _
                New String() {"devět", "devatenáct", "devět set", "devět", "devadesát"} _
            }
            ''' <summary>Returns number in numeric system</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <returns>Number in numeric system that is based on system's default base</returns>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                Dim ret As New System.Text.StringBuilder
                Dim NewIndex As UInteger = Index
                If NewIndex >= 1000000000UI Then
                    Dim Part As UInteger = NewIndex \ 1000000000UI
                    If Part = 1 Then
                        ret.Append(Exponentos(3)(0))
                    ElseIf Part <= 4 Then
                        ret.Append(Decimals(Part)(3) & " " & Exponentos(3)(1))
                    Else
                        ret.Append(SmallNumber(Part) & " " & Exponentos(3)(2))
                    End If
                    NewIndex -= Part * 1000000000UI
                End If
                If NewIndex >= 1000000UI Then
                    If ret.Length <> 0 Then ret.Append(" "c)
                    Dim Part As UInteger = NewIndex \ 1000000UI
                    If Part = 1 Then
                        ret.Append(Exponentos(2)(0))
                    ElseIf Part <= 4 Then
                        ret.Append(Decimals(Part)(3) & " " & Exponentos(2)(1))
                    Else
                        ret.Append(SmallNumber(Part) & " " & Exponentos(2)(2))
                    End If
                    NewIndex -= Part * 1000000UI
                End If
                If NewIndex >= 1000 Then
                    Dim Part As UInteger = NewIndex \ 1000
                    If ret.Length <> 0 Then ret.Append(" "c)
                    If Part = 1 Then
                        ret.Append(Exponentos(1)(0))
                    ElseIf Part <= 4 Then
                        ret.Append(Decimals(Part)(3) & " " & Exponentos(1)(1))
                    Else
                        ret.Append(SmallNumber(Part) & " " & Exponentos(1)(2))
                    End If
                    NewIndex -= Part * 1000.0
                End If
                If NewIndex > 0 OrElse Index = 0 Then
                    If ret.Length <> 0 Then ret.Append(" "c)
                    ret.Append(SmallNumber(NewIndex))
                End If
                Return ret.ToString
            End Function

            ''' <summary>Name of number form range 0÷999</summary>
            ''' <param name="Number">The number to be named</param>
            ''' <returns>Name of the <paramref name="Number"/> Number in Czech</returns>
            Private Function SmallNumber(ByVal Number As UShort) As String
                If Number < 0 OrElse Number > 999 Then Throw New ArgumentOutOfRangeException("Number", "Number must be within range <0;999>")
                Dim ret As String = ""
                If Number = 0 Then Return Decimals(0)(0)
                If Number >= 100 Then
                    ret = Decimals(Number \ 100)(2)
                    Number -= (Number \ 100) * 100
                End If
                If Number = 0 Then : Return ret : Else : ret &= " " : End If
                If Number < 10 Then : Return ret & Decimals(Number)(0)
                ElseIf Number < 20 Then : Return ret & Decimals(Number - 10)(1)
                ElseIf Number Mod 10 = 0 Then : Return ret & Decimals(Number \ 10 Mod 10)(4)
                Else : Return ret & Decimals(Number \ 10 Mod 10)(4) & " " & Decimals(Number Mod 10)(0)
                End If
            End Function
        End Class

        ''' <summary>Numeric system that composes numbers by decades (like Armenian or Georgian)</summary>
        ''' <remarks>
        ''' <list><listheader>Semantic of the <see cref="Decades"/> <see cref="Array"/></listheader>
        ''' <item>(0) - Term for zero (used only for zero, can be an empty string)</item>
        ''' <item>(1÷9) - 1÷9</item>
        ''' <item>(11 ÷ 19) - 10÷90</item>
        ''' <item>(21 ÷ 29) - 100÷900</item>
        ''' <item>(31 ÷ 39) - 1000÷9000</item>
        ''' <item>(41 ÷ 49) - 10000÷90000</item>
        ''' <item>...</item>
        ''' <item>10,20,30,40,... are not in use</item>
        ''' </list>
        ''' Required Unicode support depends on decade characters passed
        ''' </remarks>
        Public Class Decades : Inherits NumericSystem
            ''' <summary>Decades markers</summary>
            ''' <remarks>See <see cref="NumericSystem.Decades"/>'s comment for semantic of this <see cref="Array"/></remarks>
            Private Shadows Decades As Char()
            ''' <summary>CTor</summary>
            ''' <param name="Decades">Decades marker</param>
            ''' <remarks><seealso cref="Decades"/></remarks>
            Public Sub New(ByVal ParamArray Decades As Char())
                Me.Decades = Decades
            End Sub
            ''' <summary>Returns number in numeric system</summary>
            ''' <param name="Index">The number to be converted</param>
            ''' <returns>Number in numeric system</returns>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                If Index = 0 Then Return Decades(0)
                'The maximal number that is power of 10 and can be converted
                Dim Maximal10Power As Double = 10 ^ ((Decades.Length - 1) \ 10)
                'Maximal number that consists from only one numeral and can be converted
                Dim MaximalDecade As Double = ((Decades.Length - 1) Mod 10) * 10 ^ ((Decades.Length - 1) \ 10)
                'Maximal number that can be converted
                Dim MaximalNumber As Double = MaximalDecade + Maximal10Power - 1
                Dim ret As String = ""
                If Index > MaximalNumber Then
                    ret = Number(Index \ (Maximal10Power * 10))
                    Index -= Number(Index \ (Maximal10Power * 10))
                End If
                Dim Číslo As String = Index.ToString
                For i As Integer = 1 To Číslo.Length
                    If Číslo(i - 1) <> "0"c Then
                        ret &= Decades(CUInt(CStr(Číslo(i - 1))) + (Číslo.Length - i) * 10)
                    End If
                Next i
                Return ret
            End Function
        End Class

        ''' <summary>Fractions</summary>
        ''' <remarks>Decadic 1-based fractions</remarks>
        Public Class Fractions : Inherits [Decimal]
            ''' <summary>String used as slash</summary>
            Private Frac As String
            ''' <summary>CTor</summary>
            ''' <param name="Frac">String used as slash</param>
            Public Sub New(Optional ByVal Frac As String = "/")
                Me.Frac = Frac
            End Sub
            ''' <summary>Fractional number correspoinding with <paramref name="Index"/> and <paramref name="Count"/></summary>
            ''' <param name="Index">Number to be converted (numerator)</param>
            ''' <param name="Current">Ignored</param>
            ''' <param name="Count">Maximal number (denominator)</param>
            ''' <returns><see cref="String"/> containin <paramref name="Index"/>, <see cref="Frac"/> and <paramref name="Count"/></returns>
            Public Overrides Function Number(ByVal Index As UInteger, ByVal Current As UInteger, ByVal Count As UInteger) As String
                Return Number(Index) & Frac & Count
            End Function
        End Class

        ''' <summary>Numeric system that combines more systems</summary>
        ''' <remarks>Combinations can be done dependently on parameters of the <see cref="IndexOutOfRangeException"/> function</remarks>
        Public Class Combined : Inherits NumericSystem
            ''' <summary>Fallback system</summary>
            Private BaseSystem As NumericSystem
            ''' <summary>Systems combined with <see cref="BaseSystem"/></summary>
            ''' <remarks>Systems must be ordered from the smalest <see cref="SysComb.UpTo"/> to the highest</remarks>
            Private OtherSystems As SysComb()
            ''' <summary>Defines which parameter of the <see cref="Number"/> function is used to combination</summary>
            Private Mode As enmUpToModes

            ''' <summary>Used to select parametr of the <see cref="Number"/> function to used numeric system be dependent on</summary>
            Public Enum enmUpToModes
                ''' <summary>The <c>Index</c> parameter is used</summary>
                ByIndex
                ''' <summary>The <c>Current</c> parameter is used</summary>
                ByCurrent
                ''' <summary>The <c>Count</c> parameter is used</summary>
                ByCount
            End Enum

            ''' <summary>Defines which system is used in which range of values of parameter defined by <see cref="Mode"/></summary>
            Public Structure SysComb
                ''' <summary>CTor</summary>
                ''' <param name="UpTo">The greatest value of parameter with which system <paramref name="Syst"/> is used</param>
                ''' <param name="Syst">Used system</param>
                Public Sub New(ByVal UpTo As UInteger, ByVal Syst As NumericSystem)
                    Me.UpTo = UpTo
                    Me.Syst = Syst
                End Sub
                ''' <summary>The greatest value of parameter with which system <see cref="Syst"/> is used</summary>
                Public UpTo As UInteger
                ''' <summary>Used system</summary>
                Public Syst As NumericSystem
            End Structure
            ''' <summary>CTor</summary>
            ''' <param name="Mode">Defines parametr used to chose system</param>
            ''' <param name="Base">Base fallback system</param>
            ''' <param name="Systems">Defines parameter-system mapping</param>
            ''' <remarks>Systems in <paramref name="Systems"/> must be ordered from the smalest <see cref="SysComb.UpTo"/> to the highest</remarks>
            Public Sub New(ByVal Mode As enmUpToModes, ByVal Base As NumericSystem, ByVal ParamArray Systems As SysComb())
                Me.Mode = Mode
                Me.BaseSystem = Base
                Me.OtherSystems = Systems
            End Sub
            ''' <summary>Returns number in current system</summary>
            ''' <param name="Index">Number to convert</param>
            ''' <remarks>Parameter - system mapping is done only if <see cref="Mode"/> is <see cref="enmUpToModes.ByIndex"/> otherwise <see cref="BaseSystem"/> is used</remarks>
            Public Overloads Overrides Function Number(ByVal Index As UInteger) As String
                If Mode = enmUpToModes.ByIndex Then
                    Return SelectSystem(Index).Number(Index)
                Else
                    Return BaseSystem.Number(Index)
                End If
            End Function
            ''' <summary>Returns number in system selected according to one of parameters</summary>
            ''' <param name="Index">Number to be converted</param>
            ''' <param name="Current">Current number</param>
            ''' <param name="Count">Maximal number</param>
            ''' <returns>Number in system whichs <see cref="SysComb.UpTo"/> is ower than or equal to selected parameter and also is the biggest of such smaller values. If there is no value according this than return number in <see cref="BaseSystem"/></returns>
            Public Overrides Function Number(ByVal Index As UInteger, ByVal Current As UInteger, ByVal Count As UInteger) As String
                Select Case Mode
                    Case enmUpToModes.ByCount
                        Return SelectSystem(Count).Number(Index, Current, Count)
                    Case enmUpToModes.ByCurrent
                        Return SelectSystem(Current).Number(Index, Current, Count)
                    Case enmUpToModes.ByIndex
                        Return SelectSystem(Index).Number(Index, Current, Count)
                    Case Else
                        Throw New InvalidOperationException("Value of Mode is set to value not member of enmUpToModes")
                End Select
            End Function
            ''' <summary>Returns system that should be used for <paramref name="Param"/> value</summary>
            ''' <param name="Param">Parameter</param>
            ''' <returns>Instance of <see cref="NumericSystem"/></returns>
            Private Function SelectSystem(ByVal Param As UInteger) As NumericSystem
                For i As Integer = 0 To OtherSystems.Length - 1
                    If Param <= OtherSystems(i).UpTo Then
                        Try
                            Return OtherSystems(i).Syst
                        Catch
                            Return BaseSystem
                        End Try
                    End If
                Next i
                Return BaseSystem
            End Function
        End Class


    End Class

    ''' <summary>Standard numbering types</summary>
    ''' <remarks><seealso cref="NumericSystem"/></remarks>
    <CLSCompliant(True)> _
    Public Enum enmNumberingTypes
        ''' <summary>1-based standard decadic numbers</summary>
        ''' <remarks><example>1,2,3,4,5,...</example></remarks>
        Numbers1
        ''' <summary>0-based standard decadic numbers</summary>
        ''' <remarks><example>0,1,2,3,4,5,...</example></remarks>
        Numbers0
        ''' <summary>Current-value-based standard decadic numbers</summary>
        ''' <remarks><example>...-2,-3,-1,0,1,2,3,...</example></remarks>
        NumbersMinusPlus
        ''' <summary>Uppercase Latin letters</summary>
        ''' <remarks><example>A,B,C,...,AA,AB,AC,..</example></remarks>
        LatinUpper
        ''' <summary>Lovercase latin letters</summary>
        ''' <remarks><example>a,b,c,...,aa,ab,ac...</example></remarks>
        LatinLower
        ''' <summary>Uppercase Latin-written Roman numbers</summary>
        ''' <remarks><example>I,II,III,IV,V,...</example></remarks>
        RomanUpper
        ''' <summary>Lowercase Latin-written Roman numbers</summary>
        ''' <remarks><example>i,ii,iii,iv,v,...</example></remarks>
        RomanLower
        ''' <summary>Uppercase Unicode-writen Roman letters (one character per digit)</summary>
        ''' <remarks><example>Ⅰ, ⅠⅠ, ⅠⅠⅠ, ⅠⅤ, ...</example> Requires special Unicode support</remarks>
        RomanUpperUnicode
        ''' <summary>Lowercase Unicode-writen Roman letters (one character per digit)</summary>
        ''' <remarks><example>ⅰ, ⅰⅰ, ⅰⅰⅰ, ⅰⅴ, ...</example> Requires special Unicode support</remarks>
        RomanLowerUnicode
        ''' <summary>Uppercase Unicode-written Roman letters (one character per number up to XII)</summary>
        ''' <remarks><examle>Ⅰ,Ⅱ,Ⅲ,Ⅳ,Ⅴ,Ⅵ,Ⅶ,Ⅷ,Ⅸ,Ⅹ,Ⅺ,Ⅻ,ⅩⅠⅠⅠ, ...</examle> Requires special Unicode support</remarks>
        RomanUpperⅫ
        ''' <summary>Lowercase Unicode-written Roman letters (one character per number up to XII)</summary>
        ''' <remarks><examle>ⅰ,ⅱ,ⅲ,ⅳ,ⅴ,ⅵ,ⅶ,ⅷ,ⅸ,ⅹ,ⅺ,ⅻ,ⅹⅰⅰⅰ,...</examle> Requires special Unicode support</remarks>
        RomanLowerⅻ
        ''' <summary>Lowercase Latin-written Roman numbers with support for big numbers</summary>
        ''' <remarks>EXPERIMENTAL <examle>i,ii,iii,iv,v,...,m̲,...</examle> Requires special Unicode support</remarks>
        RomanLowerBig
        ''' <summary>Uppercase Latin-written Roman numbers with support for big numbers</summary>
        ''' <remarks>EXPERIMENTAL <examle>I,II,III,IV,V,...,M̲,...</examle> Requires special Unicode support</remarks>
        RomanUpperBig
        ''' <summary>Lowercase Unicode-writen Roman letters (one character per digit) with support for big numbers</summary>
        ''' <remarks>EXPERIMENTAL <examle>ⅰ, ⅰⅰ, ⅰⅰⅰ, ⅰⅴ, ...,ⅿ̲,...</examle> Requires special Unicode support</remarks>
        RomalLowerBigUnicode
        ''' <summary>Uppercase Unicode-writen Roman letters (one character per digit) with suppoert for big numbers</summary>
        ''' <remarks>EXPERIMENTAL <examle>Ⅰ, ⅠⅠ, ⅠⅠⅠ, ⅠⅤ, ...,Ⅿ̲,...</examle> Requires special Unicode support</remarks>
        RomanUpperBigUnicode
        ''' <summary>Uppercase Cyrilic (Азбука)</summary>
        ''' <remarks><examle>А,Б,Б,Г,Д,..,АА,АБ,...</examle> Requires Unicode support that is commonly accessible</remarks>
        CyrilicUpper
        ''' <summary>Lowercas Cyrilic (Азбука)</summary>
        ''' <remarks><examle>а,б,в,г,д,...,аа,аб,...</examle> Requires Unicode support that is commonly accessible</remarks>
        CyrilicLower
        ''' <summary>Uppercase Greek (Αλφαβήτα)</summary>
        ''' <remarks><examle>Α,Β,Γ,Δ,Ε,...,ΑΑ,ΑΒ,...</examle> Requirec Unicode support that is commonly accessible</remarks>
        GreekUpper
        ''' <summary>Lowercase Greek (Αλφαβήτα))</summary>
        ''' <remarks><examle>α,β,γ,δ,ε,...,αα,αβ,...</examle> Requires Unicode support that is commonly accessible</remarks>
        GreekLower
        ''' <summary>Traditional Armenian numbering</summary>
        ''' <remarks>
        ''' <para>Ա(1), Բ(2), Գ(3), Դ(4), Ե(5), Զ(6), Է(7), Ը(8), Թ(9), Ժ(10), ԺԱ(11), ԺԲ(12), ԺԳ(13), ԺԴ(14), ԺԵ(15), ԺԶ(16), ԺԷ(17), ԺԸ(18), ԺԹ(19), Ի(20), …, ԻԹ(29), …, Լ(30), ..., Խ(40), …, Ծ(50), …, Կ(60), …, Հ(70), …, Ձ(80), …, Ղ(90), …, Ճ(100), ՃԱ(101), …,ՃԹ(109), ՃԺ(110), ՃԺԱ(111), …, Մ(200), ՄԱ(201), …, ՄԺ(210), ՄԺԱ(211), …, Յ(300), …, Ն(400) …, Շ(500) …, Ո(600) …, Չ(700) …, Պ(800) …, Ջ(900) …, Ռ(1000), ՌԱ(1001), …, ՌԺ(1010), …, ՌՃ(1100), …, ՌՃԺ(1110), ՌՃԺԱ(1111), …, ՌՃԻ(1120), …, Ս(2000), …, Վ(3000), …, Տ(4000), …, Ր(5000), …, Ց(6000), …, Ւ(7000), …, Փ(8000), …, Ք(9000)</para>
        ''' <para>http://en.wikipedia.org/wiki/Armenian_numerals</para>
        ''' <examle>Ա, Բ, Գ, Դ, ...</examle>
        ''' Requires special Unicode support
        ''' </remarks>
        Armenian
        ''' <summary>Traditional Georgian numbering</summary>
        ''' <remarks>
        ''' <para>ა(1),ბ(2),გ(3),დ(4),ე(5),ვ(6),ზ(7),ჱ(8),თ(9),ი(10),კ(20),ლ(30),მ(40),ნ(50),ჲ(60),ო(70),პ(80),ჟ(90),რ(100),ს(200),ტ(300),ჳ(400),ფ(500),ქ(600),ღ(700),ყ(800),შ(900),ჩ(1000),ც(2000),ძ(3000),წ(4000),ჭ(5000),ხ(6000),ჴ(7000),ჯ(8000),ჰ(9000),ჵ(10000)</para>
        ''' <para>Principal is same as <see cref="Armenian"/></para>
        ''' <examle>ა,ბ,გ,დ,...</examle>
        ''' Requires special Unicode support
        ''' </remarks>
        Georgian
        '''' <remarks><examle>אבגדהו</examle></remarks>
        'Hebrew - NOT IMPLEMENTED asking somebody who understands it pls.
        ''' <summary>Decadic 1-based numbers in circle, one circle per digit</summary>
        ''' <remarks><examle>①,②,③,④,...,①⓪,①①,...</examle> Requires special Unicode support. Note that the ⓪ character is not present in some fonts where characters ① etc. are.</remarks>
        DecimalSingeInCircle1
        ''' <summary>Decadic 1-based numbers in circle, one circle per number up to 20</summary>
        ''' <remarks><examle>①,②,...,⑩,⑪,⑳,②①,...</examle> Requires special Unicode support. Note that the ⓪ character is not present in some fonts where characters ① etc. are.</remarks>
        DecimalMultiInCircle1
        ''' <summary>Decadic 0-based numbers in circle, one circle per digit</summary>
        ''' <remarks><examle>⓪,①,②,③,④,...,①⓪,①①,...</examle> Requires special Unicode support. Note that the ⓪ character is not present in some fonts where characters ① etc. are.</remarks>
        DecimalSingeInCircle0
        ''' <summary>Decadic 0-based numbers in circle, one circle per number up to 20. Note that the ⓪ character is not present in some fonts where characters ① etc. are.</summary>
        ''' <remarks><examle>⓪,①,②,...,⑩,⑪,⑳,②①,...</examle></remarks>
        DecimalMultiInCircle0
        ''' <summary>Lowercase Latin letters in circle</summary>
        ''' <remarks><examle>ⓐ,ⓑ,ⓒ, ..., ⓐⓐ, ⓐⓑ, ...</examle> Requires special Unicode support</remarks>
        LowerLatinInCircle
        ''' <summary>Uppercase Latin letters in circle</summary>
        ''' <remarks><examle>Ⓐ,Ⓑ,Ⓒ,...,ⒶⒶ,ⒶⒷ</examle> Requires special Unicode support</remarks>
        UpperLatinInCircle
        ''' <summary>Uppercase hexadecimal numbers</summary>
        ''' <remarks><examle>1,2,3,4,5,6,7,8,9,A,B,C,...</examle></remarks>
        HexUpper1
        ''' <summary>Lowercase hexadecimal numbers</summary>
        ''' <remarks><example>1,2,3,4,5,6,7,8,9,a,b,c,...</example></remarks>
        HexLower1
        ''' <summary>0-based uppercase hexadecimal numbers</summary>
        ''' <remarks><examle>0,1,2,3,4,5,6,7,8,9,A,B,C,...</examle></remarks>
        HexUpper0
        ''' <summary>0-base lowercase hexadecimal numbers</summary>
        ''' <remarks><example>0,1,2,3,4,5,6,7,8,9,a,b,c,...</example></remarks>
        HexLower0
        ''' <summary>1-based decadic numbers with leading zeros</summary>
        ''' <remarks><example>01,02,03,...</example></remarks>
        Numbers1Leading0
        ''' <summary>0-based decadic numbers with leading zeros</summary>
        ''' <remarks><example>00,01,02,03,...</example></remarks>
        Numbers0Leading0
        ''' <summary>1-based uppercase hexadecimal numbers with leading zeros</summary>
        ''' <remarks><examle>01,02,03,04,05,06,07,08,09,0A,0B,0C,...</examle></remarks>
        HexUpper1Leading0
        ''' <summary>1-based lowercase hexadecimal numbers with leading zeros</summary>
        ''' <remarks>01,02,03,04,05,06,07,08,09,0a,0b,0c,...</remarks>
        HexLower1Leading0
        ''' <summary>0-based uppercase hexadecimal numbers with leading zeros</summary>
        ''' <remarks><examle>00,01,02,03,04,05,06,07,08,09,0A,0B,0C,...</examle></remarks>
        HexUpper0Leading0
        ''' <summary>0-based lowercase hexadecimal numbers with leading zeros</summary>
        ''' <remarks><example>00,01,02,03,04,05,06,07,08,09,0a,0b,0c,...</example></remarks>
        HexLower0Leading0
        '''' <remarks>one,two,three</remarks>
        'English - not implemented
        '''' <remarks>first,secont,third</remarks>
        'EnglishOrdinar - not implemented
        ''' <summary>Czech base numbers</summary>
        ''' <remarks><example>jedna,dva,tři</example></remarks>
        Czech
        '''' <remarks>první,druhá,třetí,...</remarks>
        'CzechOrdinar 'Not implemented
        ''' <summary>Fractions</summary>
        ''' <remarks>1/10, 2/10, 3/10, ...</remarks>
        Fractions
        ''' <summary>Up to 9 <see cref="DecimalMultiInCircle1"/> then <see cref="Numbers1"/></summary>
        CirclesTo9
        ''' <summary>Up to 10 <see cref="DecimalMultiInCircle1"/> then <see cref="Numbers1"/></summary>
        CirclesTo10
        ''' <summary>Up to 20 <see cref="DecimalMultiInCircle1"/> then <see cref="Numbers1"/></summary>
        CirclesTo20
        ''' <summary>Up to 25 <see cref="LowerLatinInCircle"/> then <see cref="LatinLower"/></summary>
        CirclesToZlower
        ''' <summary>Up to 25 <see cref="UpperLatinInCircle"/> then <see cref="LatinUpper"/></summary>
        CirclesToZUpper
        ''' <summary>Up to 25 <see cref="Fractions"/> then <see cref="Numbers1"/></summary>
        FractionsTo10
        ''' <summary>Up to 25 <see cref="Czech"/> then <see cref="Numbers1"/></summary>
        CzechTo10
    End Enum

#If AssemblyBuild <> 1 Then
End Namespace
#End If