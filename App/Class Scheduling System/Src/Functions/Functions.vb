Imports System.Drawing.Printing
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Google.Protobuf.Reflection.SourceCodeInfo.Types
Imports Google.Protobuf.WellKnownTypes
Imports Guna.UI2.WinForms


Module Functions
    'FOR HANDLE BUTOON
    Dim currentBtn As Guna2Button

    'THIS IS FOR NAVIGATION BUTTON
    Sub HandleButton_Click(clickBtn As Guna2Button, tbp As TabPage)


        If currentBtn IsNot Nothing Then
            currentBtn.FillColor = Color.FromArgb(11, 102, 106)
        End If

        currentBtn = clickBtn
        currentBtn.FillColor = Color.FromArgb(38, 166, 154)

        frm_Main.tbctrl_Section.SelectedTab = tbp
    End Sub

    'CHEKING IF THE TEXTBOX IS EMPTY
    Function EmptyCheck(textbox As Guna2TextBox) As Boolean
        Return textbox.Text.Trim() = ""
    End Function

    'ANY CHECK IN DATAGRIDVIEW
    Function IsAnyCheckBoxChecked(datagridviews As IEnumerable(Of Guna2DataGridView), columnName As String) As Boolean
        Dim allChecked As Boolean = True

        For Each datagridview As Guna2DataGridView In datagridviews
            Dim isCheckedInGrid As Boolean = False

            If datagridview.Columns.Contains(columnName) Then
                For Each row As DataGridViewRow In datagridview.Rows
                    If Not row.IsNewRow Then
                        Dim cell As DataGridViewCheckBoxCell = TryCast(row.Cells(columnName), DataGridViewCheckBoxCell)
                        If cell IsNot Nothing AndAlso cell.Value IsNot Nothing AndAlso Convert.ToBoolean(cell.Value) Then
                            isCheckedInGrid = True
                            Exit For
                        End If
                    End If
                Next
            Else
                Throw New ArgumentException($"Column named '{columnName}' cannot be found.")
            End If

            If Not isCheckedInGrid Then
                allChecked = False
                Exit For
            End If
        Next

        Return allChecked
    End Function

    Function NoCheckInBoxes(checkboxes As IEnumerable(Of Guna2CheckBox)) As Boolean
        For Each checkbox As Guna2CheckBox In checkboxes
            If checkbox.Checked Then

                Return True
            End If
        Next

        Return False
    End Function


    'CLEAR ALL TEXTBOXES
    Sub ClearTextbox(textboxes As IEnumerable(Of Guna2TextBox))
        For Each textbox As Guna2TextBox In textboxes
            textbox.Clear()
        Next
    End Sub

    'SELECT TO DEFAULT THE COMBOBOXES
    Sub DefaultCombobox(comboBoxes As IEnumerable(Of Guna2ComboBox))
        For Each combobox As Guna2ComboBox In comboBoxes
            combobox.SelectedIndex = 0
        Next
    End Sub

    'DEFAULT CHECKBOX
    Sub DefaultCheckbox(checkboxes As IEnumerable(Of Guna2CheckBox))
        For Each checkbox As Guna2CheckBox In checkboxes
            checkbox.Checked = False
        Next
    End Sub

    'UPDATE TEXTBOXES
    Function UpdateTextBoxes(textboxes As IEnumerable(Of Guna2TextBox))
        Dim isEmpty As Boolean = False

        For Each textbox As Guna2TextBox In textboxes
            If EmptyCheck(textbox) Then
                textbox.BorderColor = Color.FromArgb(255, 138, 128)
                isEmpty = True
            Else
                textbox.BorderColor = Color.FromArgb(38, 166, 154)
            End If
        Next

        Return isEmpty
    End Function

    'CHEKING IF THE COMBOBOX IS CHANGE
    Function ISComboBoxChange(comboBoxes As IEnumerable(Of Guna2ComboBox)) As Boolean
        Dim isNotSelected As Boolean = False

        For Each comboBox As Guna2ComboBox In comboBoxes
            If comboBox.SelectedIndex = 0 Then
                comboBox.BorderColor = Color.FromArgb(255, 138, 128)
                isNotSelected = True
            Else
                comboBox.BorderColor = Color.FromArgb(38, 166, 154)
            End If
        Next

        Return isNotSelected
    End Function

    'WHEN TEXTBOX CHANGE AUTOMATICALLY CHANGE THE BAKGROUND COLOR TO DEFAULT
    Sub TextChange(textboxes() As Guna2TextBox)
        For Each textbox As Guna2TextBox In textboxes
            textbox.BorderColor = Color.FromArgb(38, 166, 154)
        Next
    End Sub

    Sub SelectedChange(comboboxes() As Guna2ComboBox)
        For Each combobox As Guna2ComboBox In comboboxes
            combobox.BorderColor = Color.FromArgb(38, 166, 154)
        Next
    End Sub

    'UPDATE REAL TIME THE DATE AND TIME
    Sub UpdateDate()
        Dim currentDate As DateTime = DateTime.Now

        frm_Main.lbl_date.Text = currentDate.ToString("MMMM dd, yyyy") + " | " + currentDate.ToString("dddd")

        frm_Main.lbl_time.Text = currentDate.ToString("hh:mm:ss tt")
    End Sub



    'CREATING BLOCK IN MONITORING
    Sub AddSchedulePanels(scheduleData As DataTable, borderColor As Color, fillColor As Color)
        For Each row As DataRow In scheduleData.Rows
            Dim roomInfo As String = row("room") & Environment.NewLine & row("instructor_name") & Environment.NewLine & row("course_code") & Environment.NewLine & row("section") & Environment.NewLine & row("start_time") & " - " & row("end_time")
            Dim roomPanel As New Guna2Panel()

            With roomPanel
                .Size = New Size(220, 120)
                .BackColor = Color.Transparent
                .BorderThickness = 2
                .BorderRadius = 10
                .ShadowDecoration.BorderRadius = 10
                .ShadowDecoration.Color = Color.Black
                .ShadowDecoration.Depth = 64
                .ShadowDecoration.Enabled = True
                .ShadowDecoration.Shadow = New Padding(0, 0, 5, 5)
                .BorderColor = borderColor
                .FillColor = fillColor
                .Margin = New Padding(0, 0, 10, 10)
            End With

            Dim roomLabel As New Label()

            With roomLabel
                .Text = roomInfo
                .Dock = DockStyle.Fill
                .TextAlign = ContentAlignment.MiddleCenter
                .Font = New Font("Franklin Gothic Medium", 12)
                .ForeColor = Color.FromArgb(38, 51, 57)
            End With

            roomPanel.Controls.Add(roomLabel)
            frm_Main.flowPanel.Controls.Add(roomPanel)
        Next
    End Sub


    'CENTER THE LABEL
    Sub CenterLabel()
        Dim widthTime As Integer = CInt(Math.Round((frm_Main.panel_time.Size.Width - frm_Main.lbl_time.Size.Width) / 2))
        Dim widthDate As Integer = CInt(Math.Round((frm_Main.panel_time.Size.Width - frm_Main.lbl_date.Size.Width) / 2))

        frm_Main.lbl_time.Location = New Point(widthTime, frm_Main.lbl_time.Location.Y)
        frm_Main.lbl_date.Location = New Point(widthDate, frm_Main.lbl_date.Location.Y)
    End Sub


End Module
