Imports System.Drawing.Drawing2D
''' <summary>Vykresluje rovnici</summary>
Class Drawer
    Private Window As RectangleF
    Private Poč As PointF
    Private dx As SyntaktickyAnalyzator.Analyzer
    Private dy As SyntaktickyAnalyzator.Analyzer
    Private img As Bitmap
    Private g As Graphics
    Private AxPen As Pen = Pens.Green
    Private DrPen As Pen = Pens.Yellow
    Private TrnsMx As Matrix
    Private Tmin As Single
    Private Tmax As Single
    Private Δt As Single
#Region "CTors"
    Public Sub New( _
            ByVal Window As RectangleF, _
            ByVal Poč As PointF, _
            ByVal dx As SyntaktickyAnalyzator.Analyzer, _
            ByVal dy As SyntaktickyAnalyzator.Analyzer, _
            ByVal ImgSize As Size, _
            ByVal tMax As Single, ByVal Δt As Single, Optional ByVal tMin As Single = 0.0)
        img = New Bitmap(ImgSize.Width, ImgSize.Height)
        Me.Window = Window
        Me.dx = dx
        Me.dy = dy
        Me.Poč = Poč
        Me.Tmax = tMax
        Me.Tmin = tMin
        Me.Δt = Δt
        g = Graphics.FromImage(img)
        TransformG()
    End Sub
    Public Sub New(ByVal Window As RectangleF, ByVal Poč As PointF, ByVal dx As SyntaktickyAnalyzator.Analyzer, ByVal dy As SyntaktickyAnalyzator.Analyzer, _
            ByVal tMax As Single, ByVal Δt As Single, Optional ByVal tMin As Single = 0.0)
        Me.New(Window, Poč, dx, dy, New Size(1280, 1024), tMax, Δt, tMin)
    End Sub

    Public Sub New( _
            ByVal minX As Single, ByVal minY As Single, ByVal maxX As Single, ByVal maxY As Single, _
            ByVal PočX As Single, ByVal PočY As Single, _
            ByVal dx As SyntaktickyAnalyzator.Analyzer, ByVal dy As SyntaktickyAnalyzator.Analyzer, _
            ByVal tMax As Single, ByVal Δt As Single, Optional ByVal tMin As Single = 0.0, _
            Optional ByVal imgWidth As Integer = 1280, Optional ByVal imgHeight As Integer = 1024 _
    )
        Me.New(New Rectangle(minX, minY, maxX - minX, -(maxY - minY)), _
                New PointF(PočX, PočY), dx, dy, New Size(imgWidth, imgHeight), tMax, Δt, tMin)
    End Sub
    Public Sub New( _
            ByVal Window As RectangleF, _
            ByVal Poč As PointF, _
            ByVal dx As String, _
            ByVal dy As String, _
            ByVal ImgSize As Size, _
            ByVal tMax As Single, ByVal Δt As Single, Optional ByVal tMin As Single = 0.0)
        Me.New(Window, Poč, _
                New SyntaktickyAnalyzator.Analyzer(dx, New String() {"x", "y", "t"}), _
                New SyntaktickyAnalyzator.Analyzer(dy, New String() {"x", "y", "t"}), _
                ImgSize, tMax, Δt, tMin)
    End Sub
    Public Sub New( _
            ByVal Window As RectangleF, _
            ByVal Poč As PointF, _
            ByVal dx As String, _
            ByVal dy As String, _
            ByVal tMax As Single, ByVal Δt As Single, Optional ByVal tMin As Single = 0.0)
        Me.New(Window, Poč, _
                New SyntaktickyAnalyzator.Analyzer(dx, New String() {"x", "y", "t"}), _
                New SyntaktickyAnalyzator.Analyzer(dy, New String() {"x", "y", "t"}), _
                New Size(1280, 1024), tMax, Δt, tMin)
    End Sub
    Public Sub New( _
           ByVal minX As Single, ByVal minY As Single, ByVal maxX As Single, ByVal maxY As Single, _
           ByVal PočX As Single, ByVal PočY As Single, _
           ByVal dx As String, ByVal dy As String, _
           ByVal tMax As Single, ByVal Δt As Single, Optional ByVal tMin As Single = 0.0, _
           Optional ByVal imgWidth As Integer = 1280, Optional ByVal imgHeight As Integer = 1024 _
   )
        Me.New(minX, minY, maxX, maxY, PočX, PočY, _
                New SyntaktickyAnalyzator.Analyzer(dx, New String() {"x", "y", "t"}), _
                New SyntaktickyAnalyzator.Analyzer(dy, New String() {"x", "y", "t"}), _
                tMax, Δt, tMin, _
                imgWidth, imgHeight)
    End Sub
#End Region
    Private Sub TransformG()
        TrnsMx = New Matrix(Window, New PointF() { _
            New PointF(0, 0), New PointF(img.Width, 0), New PointF(0, img.Height)})
    End Sub

    Public Function Draw() As Image
        DrawAxes()
        DrawEqua()
        g.Flush(FlushIntention.Sync)
        Return img
    End Function
    Public Shared Function DrawAxes(ByVal minX As Single, ByVal minY As Single, ByVal maxX As Single, ByVal maxY As Single, ByVal imgWidth As Integer, ByVal imgHeight As Integer) As Image
        Dim prid As New Drawer(minX, minY, maxX, maxY, 0, 0, "0", "0", 0, 0, , imgWidth, imgHeight)
        prid.DrawAxes()
        prid.g.Flush()
        Return prid.img
    End Function
    Private Sub DrawAxes()
        '  2  
        '  |  
        '0-4-1
        '  |  
        '  3  
        Dim pts As PointF() = { _
                New PointF(Window.Left, 0), New PointF(Window.Right, 0), _
                New PointF(0, Window.Top), New PointF(0, Window.Bottom), _
                New PointF(0, 0)}
        TrnsMx.TransformPoints(pts)
        g.DrawLine(AxPen, pts(0), pts(1))
        g.DrawLine(AxPen, pts(2), pts(3))
        Dim f As New Font("Arial", 16, FontStyle.Regular, GraphicsUnit.Pixel)
        g.DrawString(Window.Top, f, Brushes.LightBlue, pts(2).X - g.MeasureString(Window.Top, f).Width, pts(2).Y)
        g.DrawString(Window.Bottom, f, Brushes.LightBlue, pts(3).X - g.MeasureString(Window.Bottom, f).Width, pts(3).Y - 16)
        g.DrawString(Window.Left, f, Brushes.LightBlue, pts(0).X, pts(0).Y)
        g.DrawString(Window.Right, f, Brushes.LightBlue, pts(1).X - g.MeasureString(Window.Right, f).Width, pts(1).Y)
        g.DrawString(0, f, Brushes.LightBlue, pts(4).X - g.MeasureString(0, f).Width, pts(4).Y)
    End Sub
    Private Sub DrawEqua()
        Dim x As Double = Poč.X
        Dim y As Double = Poč.Y
        For t As Double = Tmin To Tmax Step Δt
            Debug.Print(t & " = [" & x & ";" & y & "]") 'TODO:Remove
            Dim oldx As Double = x, oldy As Double = y
            If False Then 'Newtonovo schéma
                Dim newR As Double() = CountRight(t, oldx, oldy)
                x += Δt * newR(0)
                y += Δt * newR(1)
            Else 'RK schéma
                Dim k1, k2, k3, k4 As Double()
                k1 = CountRight(t, x, y)
                Dim ypomX As Double = x + k1(0) * Δt / 2, ypomy As Double = y + k1(1) * Δt / 2
                k2 = CountRight(t + Δt / 2, ypomX, ypomy)
                ypomX = x + k2(0) * Δt / 2 : ypomy = y + k2(1) * Δt / 2
                k3 = CountRight(t + Δt / 2, ypomX, ypomy)
                ypomX = x + k3(0) * Δt : ypomy = y + k3(1) * Δt
                k4 = CountRight(t + Δt, ypomX, ypomy)
                x += Δt * (k1(0) + 2 * k2(0) + 2 * k3(0) + k4(0)) / 6
                y += Δt * (k1(1) + 2 * k2(1) + 2 * k3(1) + k4(1)) / 6
            End If
            Dim pts() As PointF = {New PointF(oldx, oldy), New PointF(x, y)}
            TrnsMx.TransformPoints(pts)
            g.DrawLine(DrPen, pts(0), pts(1))
        Next t
    End Sub
    Private Function CountRight(ByVal t As Double, ByVal x As Double, ByVal y As Double) As Double()
        Dim ret(1) As Double
        ret(0) = dx.Calculate(New KeyValuePair(Of String, Double)() {New KeyValuePair(Of String, Double)("x", x), New KeyValuePair(Of String, Double)("y", y), New KeyValuePair(Of String, Double)("t", t)})
        ret(1) = dy.Calculate(New KeyValuePair(Of String, Double)() {New KeyValuePair(Of String, Double)("x", x), New KeyValuePair(Of String, Double)("y", y), New KeyValuePair(Of String, Double)("t", t)})
        Return ret
    End Function
End Class
