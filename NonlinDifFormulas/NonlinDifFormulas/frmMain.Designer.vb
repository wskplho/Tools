<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Dim tsbTest As System.Windows.Forms.ToolStripButton
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(frmMain))
        Me.picMain = New System.Windows.Forms.PictureBox
        Me.tosRceAdd = New System.Windows.Forms.ToolStrip
        Me.tslUloženéRovnice = New System.Windows.Forms.ToolStripLabel
        Me.tsbAdd = New System.Windows.Forms.ToolStripButton
        Me.tsbDel = New System.Windows.Forms.ToolStripButton
        Me.tosRovnice = New System.Windows.Forms.ToolStrip
        Me.tosRight = New System.Windows.Forms.ToolStrip
        Me.tslMěřítko = New System.Windows.Forms.ToolStripLabel
        Me.tslXmin = New System.Windows.Forms.ToolStripLabel
        Me.tstXmin = New System.Windows.Forms.ToolStripTextBox
        Me.tslXmax = New System.Windows.Forms.ToolStripLabel
        Me.tstXmax = New System.Windows.Forms.ToolStripTextBox
        Me.tslYmin = New System.Windows.Forms.ToolStripLabel
        Me.tstYmin = New System.Windows.Forms.ToolStripTextBox
        Me.tslYmax = New System.Windows.Forms.ToolStripLabel
        Me.tstYmax = New System.Windows.Forms.ToolStripTextBox
        Me.tslRovnice = New System.Windows.Forms.ToolStripLabel
        Me.tslNázev = New System.Windows.Forms.ToolStripLabel
        Me.tstNázev = New System.Windows.Forms.ToolStripTextBox
        Me.tslDx = New System.Windows.Forms.ToolStripLabel
        Me.tstDx = New System.Windows.Forms.ToolStripTextBox
        Me.tslDy = New System.Windows.Forms.ToolStripLabel
        Me.tstDy = New System.Windows.Forms.ToolStripTextBox
        Me.tslPočátečníPodmínky = New System.Windows.Forms.ToolStripLabel
        Me.tslPPx = New System.Windows.Forms.ToolStripLabel
        Me.tstPPx = New System.Windows.Forms.ToolStripTextBox
        Me.tslPPy = New System.Windows.Forms.ToolStripLabel
        Me.tstPPy = New System.Windows.Forms.ToolStripTextBox
        Me.tslTmin = New System.Windows.Forms.ToolStripLabel
        Me.tstTmin = New System.Windows.Forms.ToolStripTextBox
        Me.tslTmax = New System.Windows.Forms.ToolStripLabel
        Me.tstTmax = New System.Windows.Forms.ToolStripTextBox
        Me.tslΔt = New System.Windows.Forms.ToolStripLabel
        Me.tstΔt = New System.Windows.Forms.ToolStripTextBox
        Me.mnsMain = New System.Windows.Forms.MenuStrip
        Me.mniSoubor = New System.Windows.Forms.ToolStripMenuItem
        Me.mniOtevřít = New System.Windows.Forms.ToolStripMenuItem
        Me.mniUložit = New System.Windows.Forms.ToolStripMenuItem
        Me.mniUložitJako = New System.Windows.Forms.ToolStripMenuItem
        Me.mniNový = New System.Windows.Forms.ToolStripMenuItem
        Me.tssSoubor1 = New System.Windows.Forms.ToolStripSeparator
        Me.tmiKonec = New System.Windows.Forms.ToolStripMenuItem
        Me.tmiVykreslit = New System.Windows.Forms.ToolStripMenuItem
        Me.tmiNápověda = New System.Windows.Forms.ToolStripMenuItem
        Me.tmiAbout = New System.Windows.Forms.ToolStripMenuItem
        Me.ofdOpen = New System.Windows.Forms.OpenFileDialog
        Me.sfdSave = New System.Windows.Forms.SaveFileDialog
        Me.tlpLeft = New System.Windows.Forms.TableLayoutPanel
        Me.tssMěřítkoRovnice = New System.Windows.Forms.ToolStripSeparator
        Me.tssRovnicePodmínky = New System.Windows.Forms.ToolStripSeparator
        Me.tslDz = New System.Windows.Forms.ToolStripLabel
        Me.tstDz = New System.Windows.Forms.ToolStripTextBox
        Me.tslDu = New System.Windows.Forms.ToolStripLabel
        Me.tstDu = New System.Windows.Forms.ToolStripTextBox
        Me.tslČas = New System.Windows.Forms.ToolStripLabel
        Me.tssPodmínkyČas = New System.Windows.Forms.ToolStripSeparator
        Me.tlpRight = New System.Windows.Forms.TableLayoutPanel
        Me.tslPPz = New System.Windows.Forms.ToolStripLabel
        Me.tstPPz = New System.Windows.Forms.ToolStripTextBox
        Me.tslPPu = New System.Windows.Forms.ToolStripLabel
        Me.tstPPu = New System.Windows.Forms.ToolStripTextBox
        tsbTest = New System.Windows.Forms.ToolStripButton
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.tosRceAdd.SuspendLayout()
        Me.tosRovnice.SuspendLayout()
        Me.tosRight.SuspendLayout()
        Me.mnsMain.SuspendLayout()
        Me.tlpLeft.SuspendLayout()
        Me.tlpRight.SuspendLayout()
        Me.SuspendLayout()
        '
        'picMain
        '
        Me.picMain.BackColor = System.Drawing.Color.Black
        Me.picMain.Dock = System.Windows.Forms.DockStyle.Fill
        Me.picMain.Location = New System.Drawing.Point(0, 24)
        Me.picMain.Name = "picMain"
        Me.picMain.Size = New System.Drawing.Size(743, 702)
        Me.picMain.TabIndex = 0
        Me.picMain.TabStop = False
        '
        'tosRceAdd
        '
        Me.tosRceAdd.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tosRceAdd.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tosRceAdd.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslUloženéRovnice, Me.tsbAdd, Me.tsbDel})
        Me.tosRceAdd.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.tosRceAdd.Location = New System.Drawing.Point(0, 0)
        Me.tosRceAdd.Name = "tosRceAdd"
        Me.tosRceAdd.ShowItemToolTips = False
        Me.tosRceAdd.Size = New System.Drawing.Size(98, 64)
        Me.tosRceAdd.TabIndex = 1
        '
        'tslUloženéRovnice
        '
        Me.tslUloženéRovnice.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tslUloženéRovnice.Name = "tslUloženéRovnice"
        Me.tslUloženéRovnice.Size = New System.Drawing.Size(96, 13)
        Me.tslUloženéRovnice.Text = "Uložené rovnice"
        '
        'tsbAdd
        '
        Me.tsbAdd.Image = Global.NonlinDifFormulas.My.Resources.Resources.NewCardHS
        Me.tsbAdd.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbAdd.Name = "tsbAdd"
        Me.tsbAdd.Size = New System.Drawing.Size(96, 20)
        Me.tsbAdd.Text = "&Přidat"
        '
        'tsbDel
        '
        Me.tsbDel.Enabled = False
        Me.tsbDel.Image = Global.NonlinDifFormulas.My.Resources.Resources.DeleteHS
        Me.tsbDel.ImageTransparentColor = System.Drawing.Color.Magenta
        Me.tsbDel.Name = "tsbDel"
        Me.tsbDel.Size = New System.Drawing.Size(96, 20)
        Me.tsbDel.Text = "&Odstranit"
        '
        'tosRovnice
        '
        Me.tosRovnice.AutoSize = False
        Me.tosRovnice.Dock = System.Windows.Forms.DockStyle.Fill
        Me.tosRovnice.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tosRovnice.Items.AddRange(New System.Windows.Forms.ToolStripItem() {tsbTest})
        Me.tosRovnice.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.VerticalStackWithOverflow
        Me.tosRovnice.Location = New System.Drawing.Point(0, 64)
        Me.tosRovnice.Name = "tosRovnice"
        Me.tosRovnice.ShowItemToolTips = False
        Me.tosRovnice.Size = New System.Drawing.Size(98, 638)
        Me.tosRovnice.TabIndex = 2
        '
        'tosRight
        '
        Me.tosRight.CanOverflow = False
        Me.tosRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.tosRight.Enabled = False
        Me.tosRight.GripStyle = System.Windows.Forms.ToolStripGripStyle.Hidden
        Me.tosRight.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tslMěřítko, Me.tslXmin, Me.tstXmin, Me.tslXmax, Me.tstXmax, Me.tslYmin, Me.tstYmin, Me.tslYmax, Me.tstYmax, Me.tssMěřítkoRovnice, Me.tslRovnice, Me.tslNázev, Me.tstNázev, Me.tslDx, Me.tstDx, Me.tslDy, Me.tstDy, Me.tslDz, Me.tstDz, Me.tslDu, Me.tstDu, Me.tssRovnicePodmínky, Me.tslPočátečníPodmínky, Me.tslPPx, Me.tstPPx, Me.tslPPy, Me.tstPPy, Me.tslPPz, Me.tstPPz, Me.tslPPu, Me.tstPPu, Me.tssPodmínkyČas, Me.tslČas, Me.tslTmin, Me.tstTmin, Me.tslTmax, Me.tstTmax, Me.tslΔt, Me.tstΔt})
        Me.tosRight.Location = New System.Drawing.Point(0, 0)
        Me.tosRight.Name = "tosRight"
        Me.tosRight.ShowItemToolTips = False
        Me.tosRight.Size = New System.Drawing.Size(121, 702)
        Me.tosRight.TabIndex = 0
        '
        'tslMěřítko
        '
        Me.tslMěřítko.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tslMěřítko.Name = "tslMěřítko"
        Me.tslMěřítko.Size = New System.Drawing.Size(118, 13)
        Me.tslMěřítko.Text = "Měřítko"
        '
        'tslXmin
        '
        Me.tslXmin.Name = "tslXmin"
        Me.tslXmin.Size = New System.Drawing.Size(118, 13)
        Me.tslXmin.Text = "X-min"
        '
        'tstXmin
        '
        Me.tstXmin.MaxLength = 50
        Me.tstXmin.Name = "tstXmin"
        Me.tstXmin.Size = New System.Drawing.Size(116, 21)
        Me.tstXmin.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslXmax
        '
        Me.tslXmax.Name = "tslXmax"
        Me.tslXmax.Size = New System.Drawing.Size(118, 13)
        Me.tslXmax.Text = "X-max"
        '
        'tstXmax
        '
        Me.tstXmax.MaxLength = 50
        Me.tstXmax.Name = "tstXmax"
        Me.tstXmax.Size = New System.Drawing.Size(116, 21)
        Me.tstXmax.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslYmin
        '
        Me.tslYmin.Name = "tslYmin"
        Me.tslYmin.Size = New System.Drawing.Size(118, 13)
        Me.tslYmin.Text = "Y-min"
        '
        'tstYmin
        '
        Me.tstYmin.MaxLength = 50
        Me.tstYmin.Name = "tstYmin"
        Me.tstYmin.Size = New System.Drawing.Size(116, 21)
        Me.tstYmin.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslYmax
        '
        Me.tslYmax.Name = "tslYmax"
        Me.tslYmax.Size = New System.Drawing.Size(118, 13)
        Me.tslYmax.Text = "Y-max"
        '
        'tstYmax
        '
        Me.tstYmax.MaxLength = 50
        Me.tstYmax.Name = "tstYmax"
        Me.tstYmax.Size = New System.Drawing.Size(116, 21)
        Me.tstYmax.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslRovnice
        '
        Me.tslRovnice.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tslRovnice.Name = "tslRovnice"
        Me.tslRovnice.Size = New System.Drawing.Size(118, 13)
        Me.tslRovnice.Text = "Rovnice"
        '
        'tslNázev
        '
        Me.tslNázev.Name = "tslNázev"
        Me.tslNázev.Size = New System.Drawing.Size(118, 13)
        Me.tslNázev.Text = "Název"
        '
        'tstNázev
        '
        Me.tstNázev.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tstNázev.MaxLength = 50
        Me.tstNázev.Name = "tstNázev"
        Me.tstNázev.Size = New System.Drawing.Size(116, 21)
        Me.tstNázev.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslDx
        '
        Me.tslDx.Name = "tslDx"
        Me.tslDx.Size = New System.Drawing.Size(118, 13)
        Me.tslDx.Text = "dx"
        '
        'tstDx
        '
        Me.tstDx.Name = "tstDx"
        Me.tstDx.Size = New System.Drawing.Size(116, 21)
        Me.tstDx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslDy
        '
        Me.tslDy.Name = "tslDy"
        Me.tslDy.Size = New System.Drawing.Size(118, 13)
        Me.tslDy.Text = "dy"
        '
        'tstDy
        '
        Me.tstDy.Name = "tstDy"
        Me.tstDy.Size = New System.Drawing.Size(116, 21)
        Me.tstDy.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslPočátečníPodmínky
        '
        Me.tslPočátečníPodmínky.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tslPočátečníPodmínky.Name = "tslPočátečníPodmínky"
        Me.tslPočátečníPodmínky.Size = New System.Drawing.Size(118, 13)
        Me.tslPočátečníPodmínky.Text = "Podmínky"
        '
        'tslPPx
        '
        Me.tslPPx.Name = "tslPPx"
        Me.tslPPx.Size = New System.Drawing.Size(118, 13)
        Me.tslPPx.Text = "x"
        '
        'tstPPx
        '
        Me.tstPPx.MaxLength = 50
        Me.tstPPx.Name = "tstPPx"
        Me.tstPPx.Size = New System.Drawing.Size(116, 21)
        Me.tstPPx.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslPPy
        '
        Me.tslPPy.Name = "tslPPy"
        Me.tslPPy.Size = New System.Drawing.Size(118, 13)
        Me.tslPPy.Text = "y"
        '
        'tstPPy
        '
        Me.tstPPy.MaxLength = 50
        Me.tstPPy.Name = "tstPPy"
        Me.tstPPy.Size = New System.Drawing.Size(116, 21)
        Me.tstPPy.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslTmin
        '
        Me.tslTmin.Name = "tslTmin"
        Me.tslTmin.Size = New System.Drawing.Size(118, 13)
        Me.tslTmin.Text = "t-min"
        '
        'tstTmin
        '
        Me.tstTmin.Name = "tstTmin"
        Me.tstTmin.Size = New System.Drawing.Size(116, 21)
        Me.tstTmin.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslTmax
        '
        Me.tslTmax.Name = "tslTmax"
        Me.tslTmax.Size = New System.Drawing.Size(118, 13)
        Me.tslTmax.Text = "t-max"
        '
        'tstTmax
        '
        Me.tstTmax.Name = "tstTmax"
        Me.tstTmax.Size = New System.Drawing.Size(116, 21)
        Me.tstTmax.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'tslΔt
        '
        Me.tslΔt.Name = "tslΔt"
        Me.tslΔt.Size = New System.Drawing.Size(118, 13)
        Me.tslΔt.Text = "Δt"
        '
        'tstΔt
        '
        Me.tstΔt.Name = "tstΔt"
        Me.tstΔt.Size = New System.Drawing.Size(116, 21)
        Me.tstΔt.TextBoxTextAlign = System.Windows.Forms.HorizontalAlignment.Center
        '
        'mnsMain
        '
        Me.mnsMain.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniSoubor, Me.tmiVykreslit, Me.tmiNápověda})
        Me.mnsMain.Location = New System.Drawing.Point(0, 0)
        Me.mnsMain.Name = "mnsMain"
        Me.mnsMain.Size = New System.Drawing.Size(743, 24)
        Me.mnsMain.TabIndex = 0
        Me.mnsMain.Text = "MenuStrip1"
        '
        'mniSoubor
        '
        Me.mniSoubor.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.mniOtevřít, Me.mniUložit, Me.mniUložitJako, Me.mniNový, Me.tssSoubor1, Me.tmiKonec})
        Me.mniSoubor.Name = "mniSoubor"
        Me.mniSoubor.Size = New System.Drawing.Size(53, 20)
        Me.mniSoubor.Text = "&Soubor"
        '
        'mniOtevřít
        '
        Me.mniOtevřít.Image = Global.NonlinDifFormulas.My.Resources.Resources.openHS
        Me.mniOtevřít.Name = "mniOtevřít"
        Me.mniOtevřít.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.O), System.Windows.Forms.Keys)
        Me.mniOtevřít.Size = New System.Drawing.Size(148, 22)
        Me.mniOtevřít.Text = "&Otevřít"
        '
        'mniUložit
        '
        Me.mniUložit.Image = Global.NonlinDifFormulas.My.Resources.Resources.saveHS
        Me.mniUložit.Name = "mniUložit"
        Me.mniUložit.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.S), System.Windows.Forms.Keys)
        Me.mniUložit.Size = New System.Drawing.Size(148, 22)
        Me.mniUložit.Text = "&Uložit"
        '
        'mniUložitJako
        '
        Me.mniUložitJako.Name = "mniUložitJako"
        Me.mniUložitJako.Size = New System.Drawing.Size(148, 22)
        Me.mniUložitJako.Text = "Uložit j&ako..."
        '
        'mniNový
        '
        Me.mniNový.Image = Global.NonlinDifFormulas.My.Resources.Resources.NewDocumentHS
        Me.mniNový.Name = "mniNový"
        Me.mniNový.ShortcutKeys = CType((System.Windows.Forms.Keys.Control Or System.Windows.Forms.Keys.N), System.Windows.Forms.Keys)
        Me.mniNový.Size = New System.Drawing.Size(148, 22)
        Me.mniNový.Text = "&Nový"
        '
        'tssSoubor1
        '
        Me.tssSoubor1.Name = "tssSoubor1"
        Me.tssSoubor1.Size = New System.Drawing.Size(145, 6)
        '
        'tmiKonec
        '
        Me.tmiKonec.Name = "tmiKonec"
        Me.tmiKonec.ShortcutKeyDisplayString = "Alt+F4"
        Me.tmiKonec.Size = New System.Drawing.Size(148, 22)
        Me.tmiKonec.Text = "&Konec"
        '
        'tmiVykreslit
        '
        Me.tmiVykreslit.Enabled = False
        Me.tmiVykreslit.ForeColor = System.Drawing.Color.Red
        Me.tmiVykreslit.Name = "tmiVykreslit"
        Me.tmiVykreslit.Size = New System.Drawing.Size(59, 20)
        Me.tmiVykreslit.Text = "&Vykreslit"
        '
        'tmiNápověda
        '
        Me.tmiNápověda.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tmiAbout})
        Me.tmiNápověda.Name = "tmiNápověda"
        Me.tmiNápověda.Size = New System.Drawing.Size(68, 20)
        Me.tmiNápověda.Text = "&Nápověda"
        '
        'tmiAbout
        '
        Me.tmiAbout.Name = "tmiAbout"
        Me.tmiAbout.Size = New System.Drawing.Size(146, 22)
        Me.tmiAbout.Text = "&O programu ..."
        '
        'ofdOpen
        '
        Me.ofdOpen.DefaultExt = "equa.xml"
        Me.ofdOpen.Filter = "XML soubory rovnic (*.equa.xml)|*.equa.xml|XML soubory (*.xml)|*.xml|Všechny soub" & _
            "ory (*.*)|*.*"
        Me.ofdOpen.SupportMultiDottedExtensions = True
        '
        'sfdSave
        '
        Me.sfdSave.DefaultExt = "equa.xml"
        Me.sfdSave.Filter = "XML soubory rovnic (*.equa.xml)|*.equa.xml|XML soubory (*.xml)|*.xml|Všechny soub" & _
            "ory (*.*)|*.*"
        Me.sfdSave.SupportMultiDottedExtensions = True
        '
        'tlpLeft
        '
        Me.tlpLeft.AutoSize = True
        Me.tlpLeft.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpLeft.ColumnCount = 1
        Me.tlpLeft.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.tlpLeft.Controls.Add(Me.tosRceAdd, 0, 0)
        Me.tlpLeft.Controls.Add(Me.tosRovnice, 0, 1)
        Me.tlpLeft.Dock = System.Windows.Forms.DockStyle.Left
        Me.tlpLeft.Location = New System.Drawing.Point(0, 24)
        Me.tlpLeft.Name = "tlpLeft"
        Me.tlpLeft.RowCount = 2
        Me.tlpLeft.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tlpLeft.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100.0!))
        Me.tlpLeft.Size = New System.Drawing.Size(98, 702)
        Me.tlpLeft.TabIndex = 3
        '
        'tssMěřítkoRovnice
        '
        Me.tssMěřítkoRovnice.Name = "tssMěřítkoRovnice"
        Me.tssMěřítkoRovnice.Size = New System.Drawing.Size(118, 6)
        '
        'tssRovnicePodmínky
        '
        Me.tssRovnicePodmínky.Name = "tssRovnicePodmínky"
        Me.tssRovnicePodmínky.Size = New System.Drawing.Size(118, 6)
        '
        'tsbTest
        '
        tsbTest.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text
        tsbTest.Image = CType(resources.GetObject("tsbTest.Image"), System.Drawing.Image)
        tsbTest.ImageTransparentColor = System.Drawing.Color.Magenta
        tsbTest.Name = "tsbTest"
        tsbTest.Size = New System.Drawing.Size(96, 17)
        tsbTest.Text = "test"
        '
        'tslDz
        '
        Me.tslDz.Name = "tslDz"
        Me.tslDz.Size = New System.Drawing.Size(118, 13)
        Me.tslDz.Text = "dz"
        '
        'tstDz
        '
        Me.tstDz.Name = "tstDz"
        Me.tstDz.Size = New System.Drawing.Size(116, 21)
        '
        'tslDu
        '
        Me.tslDu.Name = "tslDu"
        Me.tslDu.Size = New System.Drawing.Size(118, 13)
        Me.tslDu.Text = "du"
        '
        'tstDu
        '
        Me.tstDu.Name = "tstDu"
        Me.tstDu.Size = New System.Drawing.Size(116, 21)
        '
        'tslČas
        '
        Me.tslČas.Font = New System.Drawing.Font("Tahoma", 8.25!, System.Drawing.FontStyle.Bold)
        Me.tslČas.Name = "tslČas"
        Me.tslČas.Size = New System.Drawing.Size(118, 13)
        Me.tslČas.Text = "Čas"
        '
        'tssPodmínkyČas
        '
        Me.tssPodmínkyČas.Name = "tssPodmínkyČas"
        Me.tssPodmínkyČas.Size = New System.Drawing.Size(118, 6)
        '
        'tlpRight
        '
        Me.tlpRight.AutoScroll = True
        Me.tlpRight.AutoSize = True
        Me.tlpRight.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
        Me.tlpRight.ColumnCount = 1
        Me.tlpRight.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle)
        Me.tlpRight.Controls.Add(Me.tosRight, 0, 0)
        Me.tlpRight.Dock = System.Windows.Forms.DockStyle.Right
        Me.tlpRight.Location = New System.Drawing.Point(622, 24)
        Me.tlpRight.Name = "tlpRight"
        Me.tlpRight.RowCount = 1
        Me.tlpRight.RowStyles.Add(New System.Windows.Forms.RowStyle)
        Me.tlpRight.Size = New System.Drawing.Size(121, 702)
        Me.tlpRight.TabIndex = 4
        '
        'tslPPz
        '
        Me.tslPPz.Name = "tslPPz"
        Me.tslPPz.Size = New System.Drawing.Size(118, 13)
        Me.tslPPz.Text = "z"
        '
        'tstPPz
        '
        Me.tstPPz.Name = "tstPPz"
        Me.tstPPz.Size = New System.Drawing.Size(116, 21)
        '
        'tslPPu
        '
        Me.tslPPu.Name = "tslPPu"
        Me.tslPPu.Size = New System.Drawing.Size(118, 13)
        Me.tslPPu.Text = "u"
        '
        'tstPPu
        '
        Me.tstPPu.Name = "tstPPu"
        Me.tstPPu.Size = New System.Drawing.Size(116, 21)
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(743, 726)
        Me.Controls.Add(Me.tlpRight)
        Me.Controls.Add(Me.tlpLeft)
        Me.Controls.Add(Me.picMain)
        Me.Controls.Add(Me.mnsMain)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MainMenuStrip = Me.mnsMain
        Me.Name = "frmMain"
        Me.Text = "Diff Equa"
        CType(Me.picMain, System.ComponentModel.ISupportInitialize).EndInit()
        Me.tosRceAdd.ResumeLayout(False)
        Me.tosRceAdd.PerformLayout()
        Me.tosRovnice.ResumeLayout(False)
        Me.tosRovnice.PerformLayout()
        Me.tosRight.ResumeLayout(False)
        Me.tosRight.PerformLayout()
        Me.mnsMain.ResumeLayout(False)
        Me.mnsMain.PerformLayout()
        Me.tlpLeft.ResumeLayout(False)
        Me.tlpLeft.PerformLayout()
        Me.tlpRight.ResumeLayout(False)
        Me.tlpRight.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents tosRight As System.Windows.Forms.ToolStrip
    Friend WithEvents tslMěřítko As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslXmin As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslXmax As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslYmin As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslYmax As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstXmin As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tstXmax As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tstYmin As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tstYmax As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslRovnice As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslNázev As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstNázev As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslDx As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstDx As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslDy As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstDy As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tosRceAdd As System.Windows.Forms.ToolStrip
    Friend WithEvents tslUloženéRovnice As System.Windows.Forms.ToolStripLabel
    Friend WithEvents mnsMain As System.Windows.Forms.MenuStrip
    Friend WithEvents mniSoubor As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mniOtevřít As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mniUložit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mniUložitJako As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents mniNový As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmiKonec As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tmiNápověda As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tslPočátečníPodmínky As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tslPPx As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstPPx As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslPPy As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstPPy As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tssSoubor1 As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tmiAbout As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tosRovnice As System.Windows.Forms.ToolStrip
    Friend WithEvents tsbAdd As System.Windows.Forms.ToolStripButton
    Friend WithEvents picMain As System.Windows.Forms.PictureBox
    Friend WithEvents tmiVykreslit As System.Windows.Forms.ToolStripMenuItem
    Friend WithEvents tsbDel As System.Windows.Forms.ToolStripButton
    Friend WithEvents ofdOpen As System.Windows.Forms.OpenFileDialog
    Friend WithEvents sfdSave As System.Windows.Forms.SaveFileDialog
    Friend WithEvents tslTmin As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstTmin As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslTmax As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstTmax As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslΔt As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstΔt As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tlpLeft As System.Windows.Forms.TableLayoutPanel
    Friend WithEvents tssMěřítkoRovnice As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tssRovnicePodmínky As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tslDz As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstDz As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslDu As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstDu As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslČas As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tssPodmínkyČas As System.Windows.Forms.ToolStripSeparator
    Friend WithEvents tslPPz As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstPPz As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tslPPu As System.Windows.Forms.ToolStripLabel
    Friend WithEvents tstPPu As System.Windows.Forms.ToolStripTextBox
    Friend WithEvents tlpRight As System.Windows.Forms.TableLayoutPanel
End Class
