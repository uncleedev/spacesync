Imports System.ComponentModel
Imports System.Drawing.Design
Imports System.Drawing.Imaging
Imports System.Drawing.Printing
Imports System.Globalization
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Google.Protobuf.WellKnownTypes
Imports Guna.UI2.WinForms
Imports Newtonsoft.Json
Public Class frm_Main

    ' Import the ShowWindow function from the user32.dll library
    '<DllImport("user32.dll", SetLastError:=True)>
    'Private Shared Function ShowWindow(ByVal hWnd As IntPtr, ByVal nCmdShow As Integer) As Boolean
    'End Function

    ' Define the constants for the ShowWindow function
    'Private Const SW_HIDE As Integer = 0
    'Private Const SW_SHOW As Integer = 5

    'FORM LOAD

    'LOGOUT
    Private Sub btn_logout_Click(sender As Object, e As EventArgs) Handles btn_logout.Click
        Me.Hide()
        frm_Login.Show()
    End Sub

    Dim oldCount As Integer
    Private Sub frm_Main_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        tbctrl_Section.TabMenuVisible = False

        Dim checkBoxCourse As New DataGridViewCheckBoxColumn()
        checkBoxCourse.HeaderText = "Check"
        checkBoxCourse.Name = "check"
        dtgv_instructorCourse.Columns.Add(checkBoxCourse)

        Dim checkBoxSection As New DataGridViewCheckBoxColumn()
        checkBoxSection.HeaderText = "Check"
        checkBoxSection.Name = "check"
        dtgv_instructorSection.Columns.Add(checkBoxSection)

        oldCount = schedule.CountSchedules

        'REFRESH ALL DATA
        refreshData()

        'CHANGE COLUMN NAME
        'IN PROGRAM
        dtgv_program.Columns("program_code").HeaderText = "Progam Code"
        dtgv_program.Columns("name").HeaderText = "Description"
        dtgv_program.Columns("department").HeaderText = "Department"
        dtgv_program.Columns("NoOfYear").HeaderText = "No of Year"

        'IN SECTION
        dtgv_section.Columns("category").HeaderText = "Section"
        dtgv_section.Columns("program_code").HeaderText = "Program Code"
        dtgv_section.Columns("year").HeaderText = "Year"
        dtgv_section.Columns("name").HeaderText = "Name"
        dtgv_instructorSection.Columns("category").HeaderText = "Sections"

        'IN COURSE
        dtgv_course.Columns("course_code").HeaderText = "Subject Code"
        dtgv_course.Columns("description").HeaderText = "Description"
        dtgv_course.Columns("credits").HeaderText = "Credits"
        dtgv_course.Columns("program_code").HeaderText = "Program"
        dtgv_course.Columns("yrLevel").HeaderText = "Year Level"
        dtgv_course.Columns("TypeOfRoom").HeaderText = "Type of Room"
        dtgv_instructorCourse.Columns("course_code").HeaderText = "Subject"
        dtgv_instructorCourse.Columns("program_code").HeaderText = "Program"

        'IN INSTRUCTOR
        dtgv_instructor.Columns("instructor_no").HeaderText = "Instructor No"
        dtgv_instructor.Columns("name").HeaderText = "Name"
        dtgv_instructor.Columns("email").HeaderText = "Email"
        dtgv_instructor.Columns("gender").HeaderText = "Gender"
        dtgv_instructor.Columns("employee_status").HeaderText = "Status"
        dtgv_instructor.Columns("availability").HeaderText = "Availability"

        'IN ROOM
        dtgv_room.Columns("room_no").HeaderText = "Room No"
        dtgv_room.Columns("building_name").HeaderText = "Building"
        dtgv_room.Columns("floor_level").HeaderText = "Floor Level"
        dtgv_room.Columns("TypeOfRoom").HeaderText = "Type of Room"

        'IN SCHEDULE
        dtgv_schedule.Columns("scheduleID").HeaderText = "Schedule ID"
        dtgv_schedule.Columns("room_no").HeaderText = "Room No"
        dtgv_schedule.Columns("course_code").HeaderText = "Subject Code"
        dtgv_schedule.Columns("instructor_no").HeaderText = "Instructor Name"
        dtgv_schedule.Columns("section").HeaderText = "Section"
        dtgv_schedule.Columns("class_day").HeaderText = "Class Day"
        dtgv_schedule.Columns("start_time").HeaderText = "Start Time"
        dtgv_schedule.Columns("end_time").HeaderText = "End Time"

        UpdateDate()
        time_date.Interval = 1000
        time_date.Start()
        CenterLabel()

        Timer1.Interval = 1000
        Timer1.Start()

        btn_updateProfile.Cursor = Cursors.No

    End Sub
    'TIMER
    Private Sub time_date_Tick(sender As Object, e As EventArgs) Handles time_date.Tick
        UpdateDate()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Dim currentTime As String = Date.Now.ToString("HH:mm:ss")
        Dim count As Integer = schedule.CountTime(currentTime)
        Dim newCount As Integer = schedule.CountSchedules

        If (count > 0) Or (oldCount <> newCount) Then
            Timer1.Interval = 10000
            Timer1.Start()
            refreshRoomsData()
            refreshData()
        End If
        Timer1.Interval = 1000
        Timer1.Start()
        oldCount = schedule.CountSchedules
    End Sub

    'Maximized
    Private Sub ctrlbx_max_Click(sender As Object, e As EventArgs) Handles ctrlbx_max.Click
        refreshRoomsData()
        CenterLabel()

        ' Check if the form is maximized
        'If Me.WindowState = FormWindowState.Maximized Then
        ' Show the taskbar
        'ShowWindow(Me.Handle, SW_SHOW)
        'Else
        ' Hide the taskbar if the form is not maximized
        'ShowWindow(Me.Handle, SW_HIDE)
        'End If
    End Sub


    'ANY CHANGES
    Private Sub Changes(sender As Object, e As EventArgs) Handles txt_porgCode.TextChanged, txt_progDescript.TextChanged, txt_progDepart.TextChanged, txt_sectionName.TextChanged, cb_sectionProg.SelectedIndexChanged, cb_sectionYear.SelectedIndexChanged, txt_roomNo.TextChanged, txt_roomBldg.TextChanged, cb_roomType.SelectedIndexChanged, cb_roomFloorLevel.SelectedIndexChanged, cb_courseProg.SelectedIndexChanged, cb_courseRoom.SelectedIndexChanged, cb_courseYear.SelectedIndexChanged, txt_subjectCode.TextChanged, txt_courseDescript.TextChanged, txt_courseUnit.TextChanged
        Dim textboxes = {txt_porgCode, txt_progDescript, txt_progDepart, txt_sectionName, txt_roomNo, txt_roomBldg, txt_subjectCode, txt_courseDescript, txt_courseUnit}
        Dim comboBoxes = {cb_progNoYear, cb_sectionProg, cb_sectionYear, cb_roomType, cb_roomFloorLevel, cb_courseProg, cb_courseRoom, cb_courseYear}

        TextChange(textboxes)
        SelectedChange(comboBoxes)

    End Sub

    'NAVIGATION
    Private Sub NavItem_Click(sender As Object, e As EventArgs) Handles btn_Dashboard.Click, btn_Course.Click, btn_Program.Click, btn_Section.Click, btn_Instructor.Click, btn_Room.Click, btn_Schedule.Click, btn_Print.Click, btn_Setting.Click, btn_Section.Click, btn_Monitor.Click



        Dim btn = DirectCast(sender, Guna2Button)
        Dim tabPage As TabPage = Nothing

        Select Case btn.Name
            Case "btn_Dashboard"
                tabPage = tbp_Dashboard
            Case "btn_Monitor"
                tabPage = tbp_Monitor
            Case "btn_Course"
                tabPage = tbp_Course
            Case "btn_Program"
                tabPage = tbp_Program
            Case "btn_Instructor"
                tabPage = tbp_Instructor
            Case "btn_Room"
                tabPage = tbp_Room
            Case "btn_Schedule"
                tabPage = tbp_Schedule
            Case "btn_Print"
                tabPage = tbp_Print
            Case "btn_Setting"
                tabPage = tbp_Setting
            Case "btn_Section"
                tabPage = tbp_Section
        End Select

        HandleButton_Click(btn, tabPage)
    End Sub


    'EXIT SYSTEM
    Private Sub btn_Close_Click(sender As Object, e As EventArgs) Handles btn_Close.Click
        Application.Exit()
    End Sub

    'CLEAR ALL SELECTED
    Private Sub ClearALlSelected(sender As Object, e As EventArgs) Handles btn_Program.Click, btn_Section.Click, btn_Course.Click, btn_Instructor.Click, btn_Room.Click, btn_Schedule.Click

        Dim dtgv() As Guna2DataGridView = {dtgv_program, dtgv_section, dtgv_course, dtgv_instructor, dtgv_room, dtgv_schedule}
        Dim textboxes() As Guna2TextBox = {txt_porgCode, txt_progDescript, txt_progDepart, txt_sectionName, txt_roomNo, txt_roomBldg, txt_subjectCode, txt_courseDescript, txt_courseUnit, txt_instructorNo, txt_instructorFName, txt_instructorLName, txt_instructorEmail}
        Dim comboBoxes() As Guna2ComboBox = {cb_progNoYear, cb_sectionProg, cb_sectionYear, cb_roomType, cb_roomFloorLevel, cb_courseProg, cb_courseRoom, cb_courseYear, cb_scheduleRoom, cb_scheduleCourse, cb_scheduleSection, cb_scheduleInstructor, cb_instructorGender, cb_instructorStatus}
        Dim checkBoxes() As Guna2CheckBox = {cbk_scheduleSun, cbk_scheduleMon, cbk_scheduleTue, cbk_scheduleWed, cbk_scheduleThu, cbk_scheduleFri, cbk_scheduleSat, cbk_instrucMon, cbk_instrucTue, cbk_instrucWed, cbk_instrucThu, cbk_instrucFri, cbk_instrucSat, cbk_instrucSun}

        dtp_startTime.Value = DateTime.Today.Date.AddHours(6)
        dtp_endTime.Value = DateTime.Today.Date.AddHours(19)


        For Each index As DataGridView In dtgv
            For Each row As DataGridViewRow In index.SelectedRows
                row.Selected = False
            Next
        Next


        DefaultCheckbox(checkBoxes)
        ClearTextbox(textboxes)
        DefaultCombobox(comboBoxes)
    End Sub

    'PROGRAM SECTION

    'SELECT PROGRAM
    Dim oldProgram_code As String
    Private Sub program_SelectionChange(sender As Object, e As EventArgs) Handles dtgv_program.SelectionChanged
        If dtgv_program.SelectedRows.Count > 0 Then

            'SET THE SIZE OF THE COLUMNs
            Dim tWidthProg As Integer = dtgv_program.Width
            dtgv_program.Columns("program_code").Width = CInt(tWidthProg * 0.25)
            dtgv_program.Columns("name").Width = CInt(tWidthProg * 0.35)
            dtgv_program.Columns("department").Width = CInt(tWidthProg * 0.2)
            dtgv_program.Columns("NoOfYear").Width = CInt(tWidthProg * 0.2)

            'FILL THE PROGRAM DETAILS
            Dim selectedRows As DataGridViewRow = dtgv_program.SelectedRows(0)
            txt_porgCode.Text = selectedRows.Cells("program_code").Value.ToString
            txt_progDescript.Text = selectedRows.Cells("name").Value.ToString
            txt_progDepart.Text = selectedRows.Cells("department").Value.ToString
            cb_progNoYear.SelectedItem = selectedRows.Cells("NoOfYear").Value.ToString
            oldProgram_code = selectedRows.Cells("program_code").Value.ToString

            'VALIDATE THE BUTTON
            btn_addProg.Cursor = Cursors.No
            btn_updateProg.Cursor = Cursors.Hand
            btn_deleteProg.Cursor = Cursors.Hand
        Else
            'VALIDATE THE BUTTON
            btn_addProg.Cursor = Cursors.Hand
            btn_updateProg.Cursor = Cursors.No
            btn_deleteProg.Cursor = Cursors.No
        End If
    End Sub

    'ADDING PROGRAM
    Private Sub btn_addProg_Click(sender As Object, e As EventArgs) Handles btn_addProg.Click
        If btn_addProg.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_porgCode, txt_progDescript, txt_progDepart}
            Dim comboBoxes() As Guna2ComboBox = {cb_progNoYear}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                MessageBox.Show("Fill the required Fields")
            Else
                program.CreateProgram(txt_porgCode.Text, txt_progDescript.Text, txt_progDepart.Text, cb_progNoYear.SelectedItem)

                refreshData()

                ClearTextbox(textboxes)
                DefaultCombobox(comboBoxes)
            End If
        End If
    End Sub

    'UPDATE PROGRAM
    Private Sub btn_updateProg_Click(sender As Object, e As EventArgs) Handles btn_updateProg.Click
        If btn_updateProg.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_porgCode, txt_progDescript, txt_progDepart}
            Dim comboBoxes() As Guna2ComboBox = {cb_progNoYear}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                MessageBox.Show("Fill the reqired Fields")
            Else
                If oldProgram_code IsNot Nothing Then
                    program.UpdatePrograms(txt_porgCode.Text, txt_progDescript.Text, txt_progDepart.Text, cb_progNoYear.SelectedItem, oldProgram_code)

                    oldProgram_code = txt_porgCode.Text

                    refreshData()

                    ClearTextbox(textboxes)
                    DefaultCombobox(comboBoxes)
                End If
            End If
        End If
    End Sub

    'DELETE PROGRAM
    Private Sub btn_deleteProg_Click(sender As Object, e As EventArgs) Handles btn_deleteProg.Click
        If btn_deleteProg.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_porgCode, txt_progDescript, txt_progDepart}
            Dim comboBoxes() As Guna2ComboBox = {cb_progNoYear}
            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    program.DeleteProgramByNo(txt_porgCode.Text)
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try

            refreshData()

            ClearTextbox(textboxes)
            DefaultCombobox(comboBoxes)
        End If
    End Sub
    'CLEAR PROGRAM
    Private Sub btn_programClear_Click(sender As Object, e As EventArgs) Handles btn_programClear.Click
        Dim textboxes = {txt_porgCode, txt_progDescript, txt_progDepart, txt_sectionName}
        Dim comboBoxes = {cb_progNoYear, cb_sectionProg, cb_sectionYear}

        Dim dtgv = {dtgv_program}

        For Each index As DataGridView In dtgv
            For Each row As DataGridViewRow In index.SelectedRows
                row.Selected = False
            Next
        Next

        ClearTextbox(textboxes)
        DefaultCombobox(comboBoxes)
    End Sub

    'SEARCH PROGRAM
    Private Sub txt_searchProg_TextChanged(sender As Object, e As EventArgs) Handles txt_searchProg.TextChanged

        Dim stSearch = txt_searchProg.Text.Trim

        If stSearch <> "" Then
            Dim filteredData As New DataTable
            Dim dataView As New DataView(programsData)


            dataView.RowFilter = "program_code LIKE '%" & stSearch & "%' OR name LIKE '%" & stSearch & "%' OR department LIKE '%" & stSearch & "%' OR NoOfYear LIKE '%" & stSearch & "%'"
            filteredData = dataView.ToTable

            dtgv_program.DataSource = filteredData
        Else
            dtgv_program.DataSource = programsData
        End If
    End Sub



    'SECTION SECTION

    'SELECTED SECTION
    Dim category As String
    Private Sub section_SelectionChange(sender As Object, e As EventArgs) Handles dtgv_section.SelectionChanged
        If dtgv_section.SelectedRows.Count > 0 Then
            'FILL THE PROGRAM DETAILS
            Dim selectedRows As DataGridViewRow = dtgv_section.SelectedRows(0)
            category = selectedRows.Cells("category").Value.ToString
            cb_sectionProg.SelectedItem = selectedRows.Cells("program_code").Value.ToString
            cb_sectionYear.SelectedItem = selectedRows.Cells("year").Value.ToString
            txt_sectionName.Text = selectedRows.Cells("name").Value.ToString

            'VALIDATE THE BUTTON
            btn_addSection.Cursor = Cursors.No
            btn_updateSection.Cursor = Cursors.Hand
            btn_deleteSection.Cursor = Cursors.Hand

        Else
            'VALIDATE THE BUTTON
            btn_addSection.Cursor = Cursors.Hand
            btn_updateSection.Cursor = Cursors.No
            btn_deleteSection.Cursor = Cursors.No
        End If
    End Sub

    'ADDING SECTION
    Private Sub btn_addSection_Click(sender As Object, e As EventArgs) Handles btn_addSection.Click
        If btn_addSection.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_sectionName}
            Dim comboBoxes() As Guna2ComboBox = {cb_sectionProg, cb_sectionYear}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the reqired Fields")
            Else
                For Each row As DataRow In programsData.Rows
                    If cb_sectionProg.SelectedItem.ToString() = row("program_code").ToString() Then

                        If CInt(cb_sectionYear.SelectedItem) <= CInt(row("NoOfYear")) Then
                            section.AddSection(cb_sectionProg.SelectedItem.ToString(), cb_sectionYear.SelectedItem.ToString(), txt_sectionName.Text)

                            refreshData()

                            ClearTextbox(textboxes)
                            DefaultCombobox(comboBoxes)
                        Else
                            msg_warning.Show("Selected year exceeds the number of years for the program.")
                        End If

                        Exit For

                    End If
                Next
            End If
        End If
    End Sub

    'UPDATE SECTION
    Private Sub btn_updateSection_Click(sender As Object, e As EventArgs) Handles btn_updateSection.Click
        If btn_updateSection.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_sectionName}
            Dim comboBoxes() As Guna2ComboBox = {cb_sectionProg, cb_sectionYear}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the reqired Fields")
            Else

                For Each row As DataRow In programsData.Rows
                    If cb_sectionProg.SelectedItem.ToString() = row("program_code").ToString() Then

                        If CInt(cb_sectionYear.SelectedItem) <= CInt(row("NoOfYear")) Then
                            section.UpdateSection(category, cb_sectionProg.SelectedItem.ToString, cb_sectionYear.SelectedItem.ToString, txt_sectionName.Text)

                            refreshData()

                            ClearTextbox(textboxes)
                            DefaultCombobox(comboBoxes)
                        Else
                            msg_warning.Show("Selected year exceeds the number of years for the program.")
                        End If

                        Exit For

                    End If
                Next

            End If
        End If
    End Sub

    'DELETE SECTION
    Private Sub btn_deleteSection_Click(sender As Object, e As EventArgs) Handles btn_deleteSection.Click
        If btn_deleteSection.Cursor = Cursors.Hand Then
            Dim textboxes = {txt_sectionName}
            Dim comboBoxes = {cb_sectionProg, cb_sectionYear}

            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    section.DeleteSectionByCategory(category)
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try


            refreshData()

            ClearTextbox(textboxes)
            DefaultCombobox(comboBoxes)
        End If
    End Sub

    'CLEAR SECTION
    Private Sub btn_sectionClear_Click(sender As Object, e As EventArgs) Handles btn_sectionClear.Click
        Dim textboxes = {txt_porgCode, txt_progDescript, txt_progDepart, txt_sectionName}
        Dim comboBoxes = {cb_progNoYear, cb_sectionProg, cb_sectionYear}

        Dim dtgv = {dtgv_section}

        For Each index As DataGridView In dtgv
            For Each row As DataGridViewRow In index.SelectedRows
                row.Selected = False
            Next
        Next

        ClearTextbox(textboxes)
        DefaultCombobox(comboBoxes)
    End Sub

    'SEARCH SECTION
    Private Sub txt_searchSection_TextChanged(sender As Object, e As EventArgs) Handles txt_searchSection.TextChanged
        Dim stSearch = txt_searchSection.Text.Trim

        If stSearch <> "" Then
            Dim filteredData As New DataTable
            Dim dataView As New DataView(sectionsData)


            dataView.RowFilter = "category LIKE '%" & stSearch & "%' OR program_code LIKE '%" & stSearch & "%' OR name LIKE '%" & stSearch & "%'"
            filteredData = dataView.ToTable

            dtgv_section.DataSource = filteredData
        Else
            dtgv_section.DataSource = sectionsData
        End If
    End Sub


    'ROOM SECTION

    'SELECTED ROOM
    Dim oldRoomNo As String
    Private Sub room_SelectionChange(sender As Object, e As EventArgs) Handles dtgv_room.SelectionChanged
        If dtgv_room.SelectedRows.Count > 0 Then
            'FILL THE PROGRAM DETAILS
            Dim selectedRows As DataGridViewRow = dtgv_room.SelectedRows(0)
            oldRoomNo = selectedRows.Cells("room_no").Value.ToString
            txt_roomNo.Text = selectedRows.Cells("room_no").Value.ToString
            txt_roomBldg.Text = selectedRows.Cells("building_name").Value.ToString
            cb_roomFloorLevel.SelectedItem = selectedRows.Cells("floor_level").Value.ToString
            cb_roomType.SelectedItem = selectedRows.Cells("TypeOfRoom").Value.ToString

            'VALIDATE THE BUTTON
            btn_addRoom.Cursor = Cursors.No
            btn_updateRoom.Cursor = Cursors.Hand
            btn_deleteRoom.Cursor = Cursors.Hand

        Else
            'VALIDATE THE BUTTON
            btn_addRoom.Cursor = Cursors.Hand
            btn_updateRoom.Cursor = Cursors.No
            btn_deleteRoom.Cursor = Cursors.No
        End If
    End Sub

    'ADDING ROOM
    Private Sub btn_addRoom_Click(sender As Object, e As EventArgs) Handles btn_addRoom.Click
        If btn_addRoom.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_roomNo, txt_roomBldg}
            Dim comboBoxes() As Guna2ComboBox = {cb_roomType, cb_roomFloorLevel}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the reqired Fields")
            Else

                room.AddNewRoom(txt_roomNo.Text, txt_roomBldg.Text, cb_roomFloorLevel.SelectedItem.ToString, cb_roomType.SelectedItem.ToString)

                refreshData()

                ClearTextbox(textboxes)
                DefaultCombobox(comboBoxes)
            End If
        End If
    End Sub

    'UPDATE ROOM
    Private Sub btn_updateRoom_Click(sender As Object, e As EventArgs) Handles btn_updateRoom.Click
        If btn_updateRoom.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_roomNo, txt_roomBldg}
            Dim comboBoxes() As Guna2ComboBox = {cb_roomType, cb_roomFloorLevel}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the reqired Fields")
            Else
                If oldRoomNo IsNot Nothing Then
                    room.UpdateRoom(txt_roomNo.Text, txt_roomBldg.Text, cb_roomFloorLevel.SelectedItem.ToString, cb_roomType.SelectedItem.ToString, oldRoomNo)

                    refreshData()

                    ClearTextbox(textboxes)
                    DefaultCombobox(comboBoxes)
                End If
            End If
        End If
    End Sub

    'DELETE ROOM
    Private Sub btn_deleteRoom_Click(sender As Object, e As EventArgs) Handles btn_deleteRoom.Click
        If btn_deleteRoom.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_roomNo, txt_roomBldg}
            Dim comboBoxes() As Guna2ComboBox = {cb_roomType, cb_roomFloorLevel}

            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    room.DeletebyRoomNo(txt_roomNo.Text)
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try

            refreshData()

            ClearTextbox(textboxes)
            DefaultCombobox(comboBoxes)
        End If
    End Sub

    'CLEAR ROOM
    Private Sub btn_roomClear_Click(sender As Object, e As EventArgs) Handles btn_roomClear.Click
        Dim textboxes() As Guna2TextBox = {txt_roomNo, txt_roomBldg}
        Dim comboBoxes() As Guna2ComboBox = {cb_roomType, cb_roomFloorLevel}

        Dim dtgv = {dtgv_room}

        For Each index As DataGridView In dtgv
            For Each row As DataGridViewRow In index.SelectedRows
                row.Selected = False
            Next
        Next

        ClearTextbox(textboxes)
        DefaultCombobox(comboBoxes)
    End Sub

    'SEARCH ROOM
    Private Sub txt_searchRoom_TextChanged(sender As Object, e As EventArgs) Handles txt_searchRoom.TextChanged
        Dim stSearch As String = txt_searchRoom.Text.Trim

        If stSearch <> "" Then
            Dim filteredData As New DataTable
            Dim dataView As New DataView(roomsData)


            dataView.RowFilter = "room_no LIKE '%" & stSearch & "%' OR building_name LIKE '%" & stSearch & "%' OR floor_level LIKE '%" & stSearch & "%' OR TypeOfRoom LIKE '%" & stSearch & "%'"
            filteredData = dataView.ToTable

            dtgv_room.DataSource = filteredData
        Else
            dtgv_room.DataSource = roomsData
        End If
    End Sub

    'END ROOM


    'COURSE SECTION

    'SELECTED COURSE
    Dim oldSubject, oldProgram As String
    Private Sub course_SelectionChange(sender As Object, e As EventArgs) Handles dtgv_course.SelectionChanged
        If dtgv_course.SelectedRows.Count > 0 Then

            'SET THE SIZE OF THE COLUMN
            Dim tWidth = dtgv_course.Width
            dtgv_course.Columns("course_code").Width = CInt(tWidth * 0.15)
            dtgv_course.Columns("description").Width = CInt(tWidth * 0.25)
            dtgv_course.Columns("credits").Width = CInt(tWidth * 0.15)
            dtgv_course.Columns("program_code").Width = CInt(tWidth * 0.15)
            dtgv_course.Columns("yrLevel").Width = CInt(tWidth * 0.15)
            dtgv_course.Columns("TypeOfRoom").Width = CInt(tWidth * 0.15)

            'FILL THE PROGRAM DETAILS
            Dim selectedRows = dtgv_course.SelectedRows(0)
            txt_subjectCode.Text = selectedRows.Cells("course_code").Value.ToString
            txt_courseDescript.Text = selectedRows.Cells("description").Value.ToString
            txt_courseUnit.Text = selectedRows.Cells("credits").Value.ToString
            cb_courseProg.SelectedItem = selectedRows.Cells("program_code").Value.ToString
            cb_courseYear.SelectedItem = selectedRows.Cells("yrLevel").Value.ToString
            cb_courseRoom.SelectedItem = selectedRows.Cells("TypeOfRoom").Value.ToString

            'STORE OLD VALUES
            oldSubject = selectedRows.Cells("course_code").Value.ToString
            oldProgram = selectedRows.Cells("program_code").Value.ToString

            'VALIDATE THE BUTTON
            btn_addCourse.Cursor = Cursors.No
            btn_updateCourse.Cursor = Cursors.Hand
            btn_deleteCourse.Cursor = Cursors.Hand
        Else
            'VALIDATE THE BUTTON
            btn_addCourse.Cursor = Cursors.Hand
            btn_updateCourse.Cursor = Cursors.No
            btn_deleteCourse.Cursor = Cursors.No
        End If
    End Sub

    'ADDING COURSE
    Private Sub btn_addCourse_Click(sender As Object, e As EventArgs) Handles btn_addCourse.Click
        If btn_addCourse.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_subjectCode, txt_courseDescript, txt_courseUnit}
            Dim comboBoxes() As Guna2ComboBox = {cb_courseProg, cb_courseRoom, cb_courseYear}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the reqired Fields")
            Else
                For Each row As DataRow In programsData.Rows
                    If cb_courseProg.SelectedItem.ToString() = row("program_code").ToString() Then

                        If CInt(cb_courseYear.SelectedItem) <= CInt(row("NoOfYear")) Then
                            course.AddCourse(txt_subjectCode.Text, txt_courseDescript.Text, CInt(Val(txt_courseUnit.Text)), cb_courseProg.SelectedItem.ToString, cb_courseYear.SelectedItem.ToString, cb_courseRoom.SelectedItem.ToString)

                            refreshData()

                            ClearTextbox(textboxes)
                            DefaultCombobox(comboBoxes)
                        Else
                            msg_warning.Show("Selected year exceeds the number of years for the program.")
                        End If

                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    'UPDATE COURSE
    Private Sub btn_updateCourse_Click(sender As Object, e As EventArgs) Handles btn_updateCourse.Click
        If btn_updateCourse.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_subjectCode, txt_courseDescript, txt_courseUnit}
            Dim comboBoxes() As Guna2ComboBox = {cb_courseProg, cb_courseRoom, cb_courseYear}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the reqired Fields")

            Else
                For Each row As DataRow In programsData.Rows
                    If cb_courseProg.SelectedItem.ToString() = row("program_code").ToString() Then

                        If CInt(cb_courseYear.SelectedItem) <= CInt(row("NoOfYear")) Then

                            course.UpdateCourse(txt_subjectCode.Text, txt_courseDescript.Text, CInt(Val(txt_courseUnit.Text)), cb_courseProg.SelectedItem.ToString, cb_courseYear.SelectedItem.ToString, cb_courseRoom.SelectedItem.ToString, oldSubject, oldProgram)
                            refreshData()

                            ClearTextbox(textboxes)
                            DefaultCombobox(comboBoxes)
                        Else
                            msg_warning.Show("Selected year exceeds the number of years for the program.")
                        End If

                        Exit For
                    End If
                Next
            End If
        End If
    End Sub

    'DELETE COURSE
    Private Sub btn_deleteCourse_Click(sender As Object, e As EventArgs) Handles btn_deleteCourse.Click
        If btn_deleteCourse.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_subjectCode, txt_courseDescript, txt_courseUnit}
            Dim comboBoxes() As Guna2ComboBox = {cb_courseProg, cb_courseRoom, cb_courseYear}

            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    course.DeleteCourseByCode(txt_subjectCode.Text)
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try

            refreshData()

            ClearTextbox(textboxes)
            DefaultCombobox(comboBoxes)
        End If
    End Sub

    'CLEAR COURSE
    Private Sub btn_courseClear_Click(sender As Object, e As EventArgs) Handles btn_courseClear.Click
        Dim textboxes() As Guna2TextBox = {txt_subjectCode, txt_courseDescript, txt_courseUnit}
        Dim comboBoxes() As Guna2ComboBox = {cb_courseProg, cb_courseRoom, cb_courseYear}

        Dim dtgv = {dtgv_course}

        For Each index As DataGridView In dtgv
            For Each row As DataGridViewRow In index.SelectedRows
                row.Selected = False
            Next
        Next

        ClearTextbox(textboxes)
        DefaultCombobox(comboBoxes)
    End Sub

    'SEARCH COURSE
    Private Sub txt_searchCourse_TextChanged(sender As Object, e As EventArgs) Handles txt_searchCourse.TextChanged
        Dim stSearch As String = txt_searchCourse.Text.Trim

        If stSearch <> "" Then
            Dim filteredData As New DataTable
            Dim dataView As New DataView(coursesData)

            dataView.RowFilter = "course_code LIKE '%" & stSearch & "%' OR description LIKE '%" & stSearch & "%' OR TypeOfRoom LIKE '%" & stSearch & "%'"
            filteredData = dataView.ToTable

            dtgv_course.DataSource = filteredData
        Else
            dtgv_course.DataSource = coursesData
        End If
    End Sub


    'INSTRUCTOR SECTION
    ' SELECTED INSTRUCTOR
    Dim oldInstructor As String
    Dim oldSection As String
    Dim oldCourse As String

    Private Sub instructor_SelectedChange() Handles dtgv_instructor.SelectionChanged
        Dim checkBoxes() As Guna2CheckBox = {cbk_instrucMon, cbk_instrucTue, cbk_instrucWed, cbk_instrucThu, cbk_instrucFri, cbk_instrucSat, cbk_instrucSun}

        If dtgv_instructor.SelectedRows.Count > 0 Then
            DefaultCheckbox(checkBoxes)

            For Each row As DataGridViewRow In dtgv_instructorCourse.SelectedRows
                row.Selected = False
            Next

            For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
                row.Cells("check").Value = False
            Next

            For Each row As DataGridViewRow In dtgv_instructorSection.SelectedRows
                row.Selected = False
            Next

            For Each row As DataGridViewRow In dtgv_instructorSection.Rows
                row.Cells("check").Value = False
            Next

            Dim selectedRow As DataGridViewRow = dtgv_instructor.SelectedRows(0)

            txt_instructorNo.Text = selectedRow.Cells("instructor_no").Value.ToString()

            Dim fullName As String = selectedRow.Cells("name").Value.ToString()
            Dim nameParts() As String = fullName.Split(" "c)
            txt_instructorFName.Text = String.Join(" ", nameParts.Take(nameParts.Length - 1))
            txt_instructorLName.Text = nameParts.Last()

            txt_instructorEmail.Text = selectedRow.Cells("email").Value.ToString()
            cb_instructorGender.SelectedItem = selectedRow.Cells("gender").Value.ToString()
            cb_instructorStatus.SelectedItem = selectedRow.Cells("employee_status").Value.ToString()

            Dim availabilityString As String = selectedRow.Cells("availability").Value.ToString()
            Dim availabilityList As List(Of String) = availabilityString.Split(","c).Select(Function(day) day.Trim()).ToList()

            For Each day As String In availabilityList
                Select Case day
                    Case "Sun"
                        cbk_instrucSun.Checked = True
                    Case "Mon"
                        cbk_instrucMon.Checked = True
                    Case "Tue"
                        cbk_instrucTue.Checked = True
                    Case "Wed"
                        cbk_instrucWed.Checked = True
                    Case "Thu"
                        cbk_instrucThu.Checked = True
                    Case "Fri"
                        cbk_instrucFri.Checked = True
                    Case "Sat"
                        cbk_instrucSat.Checked = True
                End Select
            Next

            Dim coursesString As String = selectedRow.Cells("courses").Value.ToString()
            Dim coursesList As List(Of String) = coursesString.Split(","c).Select(Function(course) course.Trim()).ToList()

            For Each course As String In coursesList
                For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
                    If row.Cells("course_code").Value.ToString() = course Then
                        row.Cells("check").Value = True
                    End If
                Next
            Next

            Dim sectionString As String = selectedRow.Cells("sections").Value.ToString()
            Dim sectionList As List(Of String) = sectionString.Split(","c).Select(Function(section) section.Trim()).ToList()

            For Each section As String In sectionList
                For Each row As DataGridViewRow In dtgv_instructorSection.Rows
                    If row.Cells("category").Value.ToString() = section Then
                        row.Cells("check").Value = True
                    End If
                Next
            Next

            oldInstructor = selectedRow.Cells("instructor_no").Value.ToString()
            oldCourse = selectedRow.Cells("Courses").Value.ToString()
            oldSection = selectedRow.Cells("Sections").Value.ToString()

            ' Validate buttons
            btn_addInstructor.Cursor = Cursors.No
            btn_updateInstrutor.Cursor = Cursors.Hand
            btn_deleteInstrutor.Cursor = Cursors.Hand
        Else
            ' Validate buttons
            btn_addInstructor.Cursor = Cursors.Hand
            btn_updateInstrutor.Cursor = Cursors.No
            btn_deleteInstrutor.Cursor = Cursors.No
        End If
    End Sub


    'ADDING INSTRUCTOR
    Private Sub btn_addInstructor_Click(sender As Object, e As EventArgs) Handles btn_addInstructor.Click
        If btn_addInstructor.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_instructorNo, txt_instructorFName, txt_instructorLName, txt_instructorEmail}
            Dim comboBoxes() As Guna2ComboBox = {cb_instructorGender, cb_instructorStatus}
            Dim checkBoxes() As Guna2CheckBox = {cbk_instrucMon, cbk_instrucTue, cbk_instrucWed, cbk_instrucThu, cbk_instrucFri, cbk_instrucSat, cbk_instrucSun}
            Dim datagridviews() As Guna2DataGridView = {}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)
            Dim isNotCheck As Boolean = IsAnyCheckBoxChecked(datagridviews, "check")

            Dim availabilityList As New List(Of String)()

            If cbk_instrucSun.Checked Then
                availabilityList.Add("Sun")
            End If

            If cbk_instrucMon.Checked Then
                availabilityList.Add("Mon")
            End If

            If cbk_instrucTue.Checked Then
                availabilityList.Add("Tue")
            End If

            If cbk_instrucWed.Checked Then
                availabilityList.Add("Wed")
            End If

            If cbk_instrucThu.Checked Then
                availabilityList.Add("Thu")
            End If

            If cbk_instrucFri.Checked Then
                availabilityList.Add("Fri")
            End If

            If cbk_instrucSat.Checked Then
                availabilityList.Add("Sat")
            End If

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the required Fields")
            ElseIf Not isNotCheck Then
                msg_warning.Show("Select Section or Course")
            Else
                instructor.CreateNewInstructor(txt_instructorNo.Text, txt_instructorFName.Text, txt_instructorLName.Text, txt_instructorEmail.Text, cb_instructorGender.SelectedItem.ToString(), cb_instructorStatus.SelectedItem.ToString(), availabilityList)


                For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
                    Dim cell As DataGridViewCheckBoxCell = TryCast(row.Cells("check"), DataGridViewCheckBoxCell)
                    If cell IsNot Nothing AndAlso cell.Value IsNot Nothing AndAlso Convert.ToBoolean(cell.Value) Then
                        Dim subjectCode As String = row.Cells("course_code").Value.ToString().TrimStart
                        instructor.AddCoursesToInstructor(txt_instructorNo.Text, subjectCode)
                    End If
                Next

                For Each row As DataGridViewRow In dtgv_instructorSection.Rows
                    Dim cell As DataGridViewCheckBoxCell = TryCast(row.Cells("check"), DataGridViewCheckBoxCell)
                    If cell IsNot Nothing AndAlso cell.Value IsNot Nothing AndAlso Convert.ToBoolean(cell.Value) Then
                        Dim section As String = row.Cells("category").Value.ToString().TrimStart
                        instructor.AddSectionsToInstructor(txt_instructorNo.Text, section)
                    End If
                Next

                For Each row As DataGridViewRow In dtgv_instructor.SelectedRows
                    row.Selected = False
                Next

                For Each row As DataGridViewRow In dtgv_instructorCourse.SelectedRows
                    row.Selected = False

                Next

                For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
                    row.Cells("check").Value = False
                Next

                For Each row As DataGridViewRow In dtgv_instructorSection.SelectedRows
                    row.Selected = False
                Next

                For Each row As DataGridViewRow In dtgv_instructorSection.Rows
                    row.Cells("check").Value = False
                Next

                refreshData()

                ClearTextbox(textboxes)
                DefaultCombobox(comboBoxes)
                DefaultCheckbox(checkBoxes)
            End If
        End If
    End Sub

    'UPDATE INSTRUCTOR
    Private Sub btn_updateInstrutor_Click(sender As Object, e As EventArgs) Handles btn_updateInstrutor.Click
        If btn_updateInstrutor.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_instructorNo, txt_instructorFName, txt_instructorLName, txt_instructorEmail}
            Dim comboBoxes() As Guna2ComboBox = {cb_instructorGender, cb_instructorStatus}
            Dim checkBoxes() As Guna2CheckBox = {cbk_instrucMon, cbk_instrucTue, cbk_instrucWed, cbk_instrucThu, cbk_instrucFri, cbk_instrucSat, cbk_instrucSun}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            instructor.DeleteSectionsByInstructor(oldInstructor)
            instructor.DeleteSubjectsByInstructor(oldInstructor)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the required Fields")
            Else

                Dim availabilityList As New List(Of String)()

                If cbk_instrucSun.Checked Then
                    availabilityList.Add("Sun")
                End If

                If cbk_instrucMon.Checked Then
                    availabilityList.Add("Mon")
                End If

                If cbk_instrucTue.Checked Then
                    availabilityList.Add("Tue")
                End If

                If cbk_instrucWed.Checked Then
                    availabilityList.Add("Wed")
                End If

                If cbk_instrucThu.Checked Then
                    availabilityList.Add("Thu")
                End If

                If cbk_instrucFri.Checked Then
                    availabilityList.Add("Fri")
                End If

                If cbk_instrucSat.Checked Then
                    availabilityList.Add("Sat")
                End If


                instructor.UpdateInstructor(txt_instructorNo.Text, txt_instructorFName.Text, txt_instructorLName.Text, txt_instructorEmail.Text, cb_instructorGender.SelectedItem.ToString, cb_instructorStatus.SelectedItem.ToString, availabilityList, oldInstructor)

                instructor.DeleteCourseByInstructor(txt_instructorNo.Text)
                instructor.DeleteSectionByInstructor(txt_instructorNo.Text)

                For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
                    Dim cell As DataGridViewCheckBoxCell = TryCast(row.Cells("check"), DataGridViewCheckBoxCell)
                    If cell IsNot Nothing AndAlso cell.Value IsNot Nothing AndAlso Convert.ToBoolean(cell.Value) Then
                        Dim subjectCode As String = row.Cells("course_code").Value.ToString().TrimStart
                        instructor.AddCoursesToInstructor(txt_instructorNo.Text, subjectCode)
                    End If
                Next

                For Each row As DataGridViewRow In dtgv_instructorSection.Rows
                    Dim cell As DataGridViewCheckBoxCell = TryCast(row.Cells("check"), DataGridViewCheckBoxCell)
                    If cell IsNot Nothing AndAlso cell.Value IsNot Nothing AndAlso Convert.ToBoolean(cell.Value) Then
                        Dim section As String = row.Cells("category").Value.ToString().TrimStart
                        instructor.AddSectionsToInstructor(txt_instructorNo.Text, section)
                    End If
                Next

                msg_information.Show("Successfully Updated!")

                refreshData()

                For Each row As DataGridViewRow In dtgv_instructor.SelectedRows
                    row.Selected = False
                Next

                For Each row As DataGridViewRow In dtgv_instructorCourse.SelectedRows
                    row.Selected = False

                Next

                For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
                    row.Cells("check").Value = False
                Next

                For Each row As DataGridViewRow In dtgv_instructorSection.SelectedRows
                    row.Selected = False
                Next

                For Each row As DataGridViewRow In dtgv_instructorSection.Rows
                    row.Cells("check").Value = False
                Next

                ClearTextbox(textboxes)
                DefaultCombobox(comboBoxes)
                DefaultCheckbox(checkBoxes)
            End If
        End If
    End Sub

    'DELETE INSTRUCTOR
    Private Sub btn_deleteInstrutor_Click(sender As Object, e As EventArgs) Handles btn_deleteInstrutor.Click
        If btn_deleteInstrutor.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_instructorNo, txt_instructorFName, txt_instructorLName, txt_instructorEmail}
            Dim comboBoxes() As Guna2ComboBox = {cb_instructorGender, cb_instructorStatus}
            Dim checkBoxes() As Guna2CheckBox = {cbk_instrucMon, cbk_instrucTue, cbk_instrucWed, cbk_instrucThu, cbk_instrucFri, cbk_instrucSat, cbk_instrucSun}

            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    instructor.DeleteInstructorNo(txt_instructorNo.Text)
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try

            For Each row As DataGridViewRow In dtgv_instructor.SelectedRows
                row.Selected = False
            Next

            For Each row As DataGridViewRow In dtgv_instructorCourse.SelectedRows
                row.Selected = False

            Next

            For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
                row.Cells("check").Value = False
            Next

            For Each row As DataGridViewRow In dtgv_instructorSection.SelectedRows
                row.Selected = False
            Next

            For Each row As DataGridViewRow In dtgv_instructorSection.Rows
                row.Cells("check").Value = False
            Next

            refreshData()

            ClearTextbox(textboxes)
            DefaultCombobox(comboBoxes)
            DefaultCheckbox(checkBoxes)
        End If
    End Sub

    'CLEAR INSTRUCTOR
    Private Sub btn_instructorClear_Click(sender As Object, e As EventArgs) Handles btn_instructorClear.Click
        Dim textboxes() As Guna2TextBox = {txt_instructorNo, txt_instructorFName, txt_instructorLName, txt_instructorEmail}
        Dim comboBoxes() As Guna2ComboBox = {cb_instructorGender, cb_instructorStatus}
        Dim checkBoxes() As Guna2CheckBox = {cbk_instrucMon, cbk_instrucTue, cbk_instrucWed, cbk_instrucThu, cbk_instrucFri, cbk_instrucSat, cbk_instrucSun}

        For Each row As DataGridViewRow In dtgv_instructor.SelectedRows
            row.Selected = False
        Next

        For Each row As DataGridViewRow In dtgv_instructorCourse.SelectedRows
            row.Selected = False

        Next

        For Each row As DataGridViewRow In dtgv_instructorCourse.Rows
            row.Cells("check").Value = False
        Next

        For Each row As DataGridViewRow In dtgv_instructorSection.SelectedRows
            row.Selected = False
        Next

        For Each row As DataGridViewRow In dtgv_instructorSection.Rows
            row.Cells("check").Value = False
        Next

        ClearTextbox(textboxes)
        DefaultCombobox(comboBoxes)
        DefaultCheckbox(checkBoxes)
    End Sub

    'SEARCH INSTRUCTOR
    Private Sub txt_searchInstructor_TextChanged(sender As Object, e As EventArgs) Handles txt_searchInstructor.TextChanged
        Dim stSearch As String = txt_searchInstructor.Text.Trim

        If stSearch <> "" Then
            Dim filteredData As New DataTable
            Dim dataView As New DataView(instructorsData)

            dataView.RowFilter = "instructor_no LIKE '%" & stSearch & "%' OR name LIKE '%" & stSearch & "%' OR email LIKE '%" & stSearch & "%' OR gender LIKE '%" & stSearch & "%' OR employee_status LIKE '%" & stSearch & "%' OR availability LIKE '%" & stSearch & "%'"
            filteredData = dataView.ToTable

            dtgv_instructor.DataSource = filteredData
        Else
            dtgv_instructor.DataSource = instructorsData
        End If
    End Sub


    'SCHEDULE SECTION
    'DETAILS SCHEDULE
    Dim instructor_no As String
    Private Sub cb_scheduleInstructor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_scheduleInstructor.SelectedIndexChanged
        Dim checkBoxes() As Guna2CheckBox = {cbk_scheduleSun, cbk_scheduleMon, cbk_scheduleTue, cbk_scheduleWed, cbk_scheduleThu, cbk_scheduleFri, cbk_scheduleSat}

        cb_scheduleCourse.Items.Clear()
        cb_scheduleCourse.Items.Add("Select Course")
        cb_scheduleCourse.SelectedIndex = 0

        DefaultCheckbox(checkBoxes)

        cbk_scheduleSun.Enabled = False
        cbk_scheduleMon.Enabled = False
        cbk_scheduleTue.Enabled = False
        cbk_scheduleWed.Enabled = False
        cbk_scheduleThu.Enabled = False
        cbk_scheduleFri.Enabled = False
        cbk_scheduleSat.Enabled = False

        If cb_scheduleInstructor.SelectedIndex > 0 Then
            Dim fullName As String = cb_scheduleInstructor.SelectedItem.ToString
            Dim nameParts() As String = fullName.Split(" "c)
            Dim firstname As String = String.Join(" ", nameParts.Take(nameParts.Length - 1))
            Dim lastname As String = nameParts(nameParts.Length - 1)

            Dim instructorNo As DataTable = instructor.FindInstructorNoByName(firstname, lastname)

            For Each row As DataRow In instructorNo.Rows
                Dim instrucAvaila As DataTable = instructor.ReadInstructorAvailability(row("instructor_no"))

                instructor_no = row("instructor_no")

                For Each rowAvailability As DataRow In instrucAvaila.Rows
                    Dim availabilityString As String = rowAvailability("availability")
                    Dim parts() As String = availabilityString.Split(",")
                    Dim availabilityList As New List(Of String)()

                    For Each part As String In parts
                        Dim cleanedPart As String = part.TrimEnd(":"c)
                        availabilityList.Add(cleanedPart)
                    Next

                    For Each day As String In availabilityList
                        Select Case day.Trim
                            Case "Sun"
                                cbk_scheduleSun.Enabled = True
                            Case "Mon"
                                cbk_scheduleMon.Enabled = True
                            Case "Tue"
                                cbk_scheduleTue.Enabled = True
                            Case "Wed"
                                cbk_scheduleWed.Enabled = True
                            Case "Thu"
                                cbk_scheduleThu.Enabled = True
                            Case "Fri"
                                cbk_scheduleFri.Enabled = True
                            Case "Sat"
                                cbk_scheduleSat.Enabled = True
                            Case Else
                                cbk_scheduleSun.Enabled = False
                                cbk_scheduleMon.Enabled = False
                                cbk_scheduleTue.Enabled = False
                                cbk_scheduleWed.Enabled = False
                                cbk_scheduleThu.Enabled = False
                                cbk_scheduleFri.Enabled = False
                                cbk_scheduleSat.Enabled = False
                        End Select
                    Next
                Next

                Dim instructorCourse As DataTable = instructor.ReadSubjectCodeByInstrucNo(row("instructor_no"))

                For Each course As DataRow In instructorCourse.Rows
                    cb_scheduleCourse.Items.Add(course("course_code"))
                Next
            Next
        End If
    End Sub
    Private Sub cb_scheduleCourse_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_scheduleCourse.SelectedIndexChanged

        cb_scheduleSection.Items.Clear()
        cb_scheduleSection.Items.Add("Select Section")
        cb_scheduleSection.SelectedIndex = 0

        If cb_scheduleCourse.SelectedIndex > 0 Then
            Dim sections As DataTable = section.ReadSectionByCourseAndInstructor(cb_scheduleCourse.SelectedItem.ToString, instructor_no)
            For Each row As DataRow In sections.Rows
                cb_scheduleSection.Items.Add(row("section"))
            Next
        Else
            cb_scheduleSection.SelectedIndex = 0
        End If

    End Sub

    Private Sub cb_scheduleSection_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cb_scheduleSection.SelectedIndexChanged
        cb_scheduleRoom.Items.Clear()
        cb_scheduleRoom.Items.Add("Select Room")
        cb_scheduleRoom.SelectedIndex = 0

        If cb_scheduleSection.SelectedIndex > 0 Then
            Dim rooms As DataTable = room.RealAllRoomByCourseAndSection(cb_scheduleCourse.SelectedItem.ToString, cb_scheduleSection.SelectedItem.ToString)
            For Each row As DataRow In rooms.Rows
                cb_scheduleRoom.Items.Add(row("room_no"))
            Next
        Else
            cb_scheduleRoom.SelectedIndex = 0
        End If
    End Sub


    Dim scheInstruc As String
    Dim scheCourse As String
    Dim scheSection As String
    Dim scheRoom As String
    Dim scheStart As DateTime
    Dim scheEnd As DateTime
    Dim scheClass As List(Of String)
    Dim scheduleID As Integer
    'SELECTED SCHEDULE
    Private Sub schedule_SelectedChange() Handles dtgv_schedule.SelectionChanged
        Dim comboBoxes() As Guna2ComboBox = {cb_scheduleRoom, cb_scheduleCourse, cb_scheduleSection, cb_scheduleInstructor}
        Dim checkBoxes() As Guna2CheckBox = {cbk_scheduleSun, cbk_scheduleMon, cbk_scheduleTue, cbk_scheduleWed, cbk_scheduleThu, cbk_scheduleFri, cbk_scheduleSat}

        If dtgv_schedule.SelectedRows.Count > 0 Then

            DefaultCheckbox(checkBoxes)

            For Each selectedRows As DataGridViewRow In dtgv_schedule.SelectedRows

                cb_scheduleInstructor.SelectedItem = selectedRows.Cells("instructor_no").Value.ToString
                cb_scheduleCourse.SelectedItem = selectedRows.Cells("course_code").Value.ToString

                cb_scheduleSection.Items.Add(selectedRows.Cells("section").Value.ToString)
                cb_scheduleSection.SelectedItem = selectedRows.Cells("section").Value.ToString

                cb_scheduleRoom.SelectedItem = selectedRows.Cells("room_no").Value.ToString

                Dim startTime As String = selectedRows.Cells("start_time").Value.ToString
                Dim startTimeFormat As DateTime = DateTime.ParseExact(startTime, "hh:mm tt", CultureInfo.InvariantCulture)
                dtp_startTime.Value = startTimeFormat

                Dim endTime As String = selectedRows.Cells("end_time").Value.ToString
                Dim endTimeFormat As DateTime = DateTime.ParseExact(endTime, "hh:mm tt", CultureInfo.InvariantCulture)
                dtp_endTime.Value = endTimeFormat

                scheStart = startTimeFormat.ToString("HH:mm:ss tt")
                scheEnd = endTimeFormat.ToString("HH:mm:ss tt")


                Dim classDayString As String = selectedRows.Cells("class_day").Value.ToString
                Dim substrings() As String = classDayString.Split(","c)

                Dim classDayList As New List(Of String)(substrings)

                For Each day As String In classDayList
                    Select Case day.Trim()
                        Case "Sun"
                            cbk_scheduleSun.Checked = True
                        Case "Mon"
                            cbk_scheduleMon.Checked = True
                        Case "Tue"
                            cbk_scheduleTue.Checked = True
                        Case "Wed"
                            cbk_scheduleWed.Checked = True
                        Case "Thu"
                            cbk_scheduleThu.Checked = True
                        Case "Fri"
                            cbk_scheduleFri.Checked = True
                        Case "Sat"
                            cbk_scheduleSat.Checked = True
                    End Select
                Next

                scheInstruc = selectedRows.Cells("instructor_no").Value.ToString
                scheCourse = selectedRows.Cells("course_code").Value.ToString
                scheSection = selectedRows.Cells("section").Value.ToString
                scheRoom = selectedRows.Cells("room_no").Value.ToString
                scheduleID = selectedRows.Cells("scheduleID").Value
                scheStart = startTime
                scheEnd = endTime
                scheClass = classDayList
            Next

            ' Validate buttons
            btn_addSchedule.Cursor = Cursors.No
            btn_updateSchedule.Cursor = Cursors.Hand
            btn_deleteSchedule.Cursor = Cursors.Hand

        Else
            ' Validate buttons
            btn_addSchedule.Cursor = Cursors.Hand
            btn_updateSchedule.Cursor = Cursors.No
            btn_deleteSchedule.Cursor = Cursors.No
        End If
    End Sub

    'ADDING SCHEDULE
    Private Sub btn_addSchedule_Click(sender As Object, e As EventArgs) Handles btn_addSchedule.Click
        If btn_addSchedule.Cursor = Cursors.Hand Then
            Dim comboBoxes() As Guna2ComboBox = {cb_scheduleRoom, cb_scheduleCourse, cb_scheduleSection, cb_scheduleInstructor}
            Dim checkBoxes() As Guna2CheckBox = {cbk_scheduleSun, cbk_scheduleMon, cbk_scheduleTue, cbk_scheduleWed, cbk_scheduleThu, cbk_scheduleFri, cbk_scheduleSat}

            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)
            Dim isNoCheck As Boolean = NoCheckInBoxes(checkBoxes)

            Dim classList As New List(Of String)()

            For Each cb As Guna2CheckBox In checkBoxes
                If cb.Checked Then
                    classList.Add(cb.Text)
                End If
            Next

            If isNotSelected Then
                msg_warning.Show("Fill the required Fields")
            ElseIf Not isNoCheck Then
                msg_warning.Show("Select class day")
            Else
                'schedule.IsConflict(cb_scheduleRoom.SelectedItem.ToString, classList, dtp_startTime.Value.ToString("HH:mm:ss"), dtp_endTime.Value.ToString("HH:mm:ss"), scheCourse, scheSection, scheRoom)

                If schedule.isCountConflict(cb_scheduleRoom.SelectedItem.ToString, cb_scheduleSection.SelectedItem.ToString, instructor_no, classList, dtp_startTime.Value.ToString("HH:mm:ss"), dtp_endTime.Value.ToString("HH:mm:ss")) Then
                    msg_warning.Show("Schedule overlap detected!")
                Else
                    schedule.AddSchedule(cb_scheduleRoom.SelectedItem.ToString, cb_scheduleCourse.SelectedItem.ToString, instructor_no, cb_scheduleSection.SelectedItem.ToString, classList, dtp_startTime.Value.ToString("HH:mm:ss"), dtp_endTime.Value.ToString("HH:mm:ss"))

                    refreshData()

                    DefaultCheckbox(checkBoxes)
                    DefaultCombobox(comboBoxes)
                End If

            End If
        End If
    End Sub

    'UPDATE SCHEDULE
    Private Sub btn_updateSchedule_Click(sender As Object, e As EventArgs) Handles btn_updateSchedule.Click
        If btn_updateSchedule.Cursor = Cursors.Hand Then
            Dim comboBoxes() As Guna2ComboBox = {cb_scheduleRoom, cb_scheduleCourse, cb_scheduleSection, cb_scheduleInstructor}
            Dim checkBoxes() As Guna2CheckBox = {cbk_scheduleSun, cbk_scheduleMon, cbk_scheduleTue, cbk_scheduleWed, cbk_scheduleThu, cbk_scheduleFri, cbk_scheduleSat}

            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)
            Dim isNoCheck As Boolean = NoCheckInBoxes(checkBoxes)

            Dim classList As New List(Of String)()

            For Each cb As Guna2CheckBox In checkBoxes
                If cb.Checked Then
                    classList.Add(cb.Text)
                End If
            Next

            If isNotSelected Then
                msg_warning.Show("Fill the required Fields")
            ElseIf Not isNoCheck Then
                msg_warning.Show("Select class day")
            Else
                If schedule.ConflictSched(cb_scheduleRoom.SelectedItem.ToString, instructor_no, cb_scheduleSection.SelectedItem.ToString, classList, dtp_startTime.Value.ToString("HH:mm:ss"), dtp_endTime.Value.ToString("HH:mm:ss"), scheduleID) Then
                    msg_warning.Show("Schedule conflict detected!")
                ElseIf schedule.OverlapSched(cb_scheduleRoom.SelectedItem.ToString, instructor_no, cb_scheduleSection.SelectedItem.ToString, classList, dtp_startTime.Value.ToString("HH:mm:ss"), dtp_endTime.Value.ToString("HH:mm:ss"), scheduleID) Then
                    msg_warning.Show("Schedule overlap detected!")
                Else
                    schedule.UpdateSchedule(cb_scheduleRoom.SelectedItem.ToString, cb_scheduleCourse.SelectedItem.ToString, instructor_no, cb_scheduleSection.SelectedItem.ToString, classList, dtp_startTime.Value.ToString("HH:mm:ss"), dtp_endTime.Value.ToString("HH:mm:ss"), scheduleID)

                    refreshData()

                    DefaultCheckbox(checkBoxes)
                    DefaultCombobox(comboBoxes)
                End If
            End If
        End If
    End Sub

    'DELETE SCHEDULE
    Private Sub btn_deleteSchedule_Click(sender As Object, e As EventArgs) Handles btn_deleteSchedule.Click
        If btn_deleteSchedule.Cursor = Cursors.Hand Then
            Dim comboBoxes() As Guna2ComboBox = {cb_scheduleRoom, cb_scheduleCourse, cb_scheduleSection, cb_scheduleInstructor}
            Dim checkBoxes() As Guna2CheckBox = {cbk_scheduleSun, cbk_scheduleMon, cbk_scheduleTue, cbk_scheduleWed, cbk_scheduleThu, cbk_scheduleFri, cbk_scheduleSat}


            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    schedule.DeleteScheduleBySelected(scheduleID)
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try

            refreshData()

            DefaultCombobox(comboBoxes)
            DefaultCheckbox(checkBoxes)
        End If
    End Sub

    'CLEAR SCHEDULE
    Private Sub btn_scheduleClear_Click(sender As Object, e As EventArgs) Handles btn_scheduleClear.Click
        Dim comboBoxes() As Guna2ComboBox = {cb_scheduleRoom, cb_scheduleCourse, cb_scheduleSection, cb_scheduleInstructor}
        Dim checkBoxes() As Guna2CheckBox = {cbk_scheduleSun, cbk_scheduleMon, cbk_scheduleTue, cbk_scheduleWed, cbk_scheduleThu, cbk_scheduleFri, cbk_scheduleSat}

        For Each row As DataGridViewRow In dtgv_schedule.SelectedRows
            row.Selected = False
        Next

        dtp_startTime.Value = DateTime.Today.Date.AddHours(6)
        dtp_endTime.Value = DateTime.Today.Date.AddHours(19)

        DefaultCombobox(comboBoxes)
        DefaultCheckbox(checkBoxes)
    End Sub

    'SEARCH SCHEDULE
    Private Sub txt_searchSchedule_TextChanged(sender As Object, e As EventArgs) Handles txt_searchSchedule.TextChanged
        Dim stSearch As String = txt_searchSchedule.Text.Trim

        If stSearch <> "" Then
            Dim filteredData As New DataTable
            Dim dataView As New DataView(schedulesData)

            dataView.RowFilter = "room_no LIKE '%" & stSearch & "%' OR course_code LIKE '%" & stSearch & "%' OR section LIKE '%" & stSearch & "%' OR instructor_no LIKE '%" & stSearch & "%' OR class_day LIKE '%" & stSearch & "%'"
            filteredData = dataView.ToTable

            dtgv_schedule.DataSource = filteredData
        Else
            dtgv_schedule.DataSource = schedulesData
        End If
    End Sub


    'PRINT SECTION
    Private Sub rb_student_CheckedChanged(sender As Object, e As EventArgs) Handles rb_student.CheckedChanged
        If rb_student.Checked = True Then
            dtgv_print.DataSource = studentSchedulesData


            dtgv_print.Columns("room_no").DisplayIndex = 0
            dtgv_print.Columns("Student").DisplayIndex = 1
            dtgv_print.Columns("course_code").DisplayIndex = 2
            dtgv_print.Columns("Description").DisplayIndex = 3
            dtgv_print.Columns("section").DisplayIndex = 4
            dtgv_print.Columns("class_day").DisplayIndex = 7

            Dim dataGridViewWidth As Integer = dtgv_print.Width
            dtgv_print.Columns("Student").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("room_no").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("course_code").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("description").Width = dataGridViewWidth * 0.15
            dtgv_print.Columns("Credits").Width = dataGridViewWidth * 0.05
            dtgv_print.Columns("section").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("Instructor").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("class_day").Width = dataGridViewWidth * 0.15
            dtgv_print.Columns("Time").Width = dataGridViewWidth * 0.15


        End If
    End Sub

    Private Sub rb_instructor_CheckedChanged(sender As Object, e As EventArgs) Handles rb_instructor.CheckedChanged
        If rb_instructor.Checked = True Then

            dtgv_print.DataSource = instructorSchedulesData

            dtgv_print.Columns("room_no").HeaderText = "Room No"
            dtgv_print.Columns("course_code").HeaderText = "Subject Code"
            dtgv_print.Columns("section").HeaderText = "Section"
            dtgv_print.Columns("class_day").HeaderText = "Class Day"


            dtgv_print.Columns("room_no").DisplayIndex = 0
            dtgv_print.Columns("course_code").DisplayIndex = 1
            dtgv_print.Columns("class_day").DisplayIndex = 6


            Dim dataGridViewWidth As Integer = dtgv_print.Width
            dtgv_print.Columns("room_no").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("course_code").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("description").Width = dataGridViewWidth * 0.2
            dtgv_print.Columns("Credits").Width = dataGridViewWidth * 0.05
            dtgv_print.Columns("section").Width = dataGridViewWidth * 0.1
            dtgv_print.Columns("Instructor").Width = dataGridViewWidth * 0.15
            dtgv_print.Columns("class_day").Width = dataGridViewWidth * 0.15
            dtgv_print.Columns("Time").Width = dataGridViewWidth * 0.15
        End If
    End Sub

    'SEARCH PRINT
    Private Sub txt_searhPrint_TextChanged(sender As Object, e As EventArgs) Handles txt_searhPrint.TextChanged
        Dim stSearch As String = txt_searhPrint.Text.Trim

        If rb_instructor.Checked = True Then

            If stSearch <> "" Then
                Dim filteredData As New DataTable
                Dim dataView As New DataView(instructorSchedulesData)

                dataView.RowFilter = "room_no LIKE '%" & stSearch & "%' OR course_code LIKE '%" & stSearch & "%' OR section LIKE '%" & stSearch & "%' OR class_day LIKE '%" & stSearch & "%' OR Instructor LIKE '%" & stSearch & "%' OR Time LIKE '%" & stSearch & "%'"
                filteredData = dataView.ToTable
                dtgv_print.DataSource = filteredData
            Else
                dtgv_print.DataSource = instructorSchedulesData
            End If

        ElseIf rb_student.Checked = True Then

            If stSearch <> "" Then
                Dim filteredData As New DataTable
                Dim dataView As New DataView(studentSchedulesData)

                dataView.RowFilter = "room_no LIKE '%" & stSearch & "%' OR Student LIKE '%" & stSearch & "%' OR course_code LIKE '%" & stSearch & "%' OR Section LIKE '%" & stSearch & "%' OR class_day LIKE '%" & stSearch & "%' OR Time LIKE '%" & stSearch & "%'"
                filteredData = dataView.ToTable
                dtgv_print.DataSource = filteredData
            Else
                dtgv_print.DataSource = studentSchedulesData
            End If
        End If
    End Sub


    Private Sub btn_printSearch_Click(sender As Object, e As EventArgs) Handles btn_printSearch.Click
        Dim pageSettings As New PageSettings()
        pageSettings.PaperSize = New PaperSize("Letter", 850, 1100)
        pageSettings.Landscape = True
        print_doc.DefaultPageSettings = pageSettings

        print_dialog.Document = print_doc
        print_dialog.WindowState = FormWindowState.Maximized
        print_dialog.ShowDialog()
    End Sub

    Private mRow As Integer = 0
    Private newPage As Boolean = True

    Private Sub print_doc_PrintPage(sender As Object, e As PrintPageEventArgs) Handles print_doc.PrintPage
        ' Load the background image
        Dim backgroundImage As Image = Image.FromFile("../../../Src/Resources/Icons/logo_cdm.png")

        ' Calculate the position to center the image
        Dim imageX As Integer = (e.PageBounds.Width - backgroundImage.Width) / 2
        Dim imageY As Integer = (e.PageBounds.Height - backgroundImage.Height) / 2

        ' Set the transparency of the image
        Dim attributes As New Imaging.ImageAttributes()
        Dim colorMatrix As New Imaging.ColorMatrix()
        colorMatrix.Matrix33 = 0.2F ' Set opacity to 20%
        attributes.SetColorMatrix(colorMatrix, Imaging.ColorMatrixFlag.Default, Imaging.ColorAdjustType.Bitmap)

        ' Draw the background image
        e.Graphics.DrawImage(backgroundImage, New Rectangle(imageX, imageY, backgroundImage.Width, backgroundImage.Height), 0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, attributes)

        ' Header text formatting
        Dim format As New StringFormat
        format.Alignment = StringAlignment.Center
        e.Graphics.DrawString(oldSchoolName, New Font("Franklin Gothic Medium", 16, FontStyle.Bold), Brushes.Black, New PointF(e.PageBounds.Width / 2, 52), format)
        e.Graphics.DrawString("Class Scheduling System", New Font("Franklin Gothic Medium", 12, FontStyle.Bold), Brushes.Black, New PointF(e.PageBounds.Width / 2, 80), format)

        Dim fmt As New StringFormat(StringFormatFlags.LineLimit)
        fmt.LineAlignment = StringAlignment.Center
        fmt.Alignment = StringAlignment.Center

        Dim y As Integer = 150
        Dim h As Integer = 0
        Dim rc As Rectangle
        Dim row As DataGridViewRow

        ' Calculate total table width dynamically based on text width
        Dim totalTableWidth As Integer = 0
        Dim cellWidths(dtgv_print.Columns.Count - 1) As Integer

        ' Measure column widths
        For i As Integer = 0 To dtgv_print.Columns.Count - 1
            If dtgv_print.Columns(i).Visible Then
                Dim maxWidth As Integer = CInt(e.Graphics.MeasureString(dtgv_print.Columns(i).HeaderText, dtgv_print.Font).Width) + 10 ' Add padding

                ' Measure the width of each cell in the column
                For Each row In dtgv_print.Rows
                    Dim cellText As String = row.Cells(i).FormattedValue.ToString()
                    Dim cellWidth As Integer = CInt(e.Graphics.MeasureString(cellText, dtgv_print.Font).Width) + 10 ' Add padding
                    If cellWidth > maxWidth Then
                        maxWidth = cellWidth
                    End If
                Next

                cellWidths(i) = maxWidth
                totalTableWidth += maxWidth
            End If
        Next

        ' Calculate starting x position to center the table
        Dim x As Integer = (e.PageBounds.Width - totalTableWidth) / 2

        ' Print header
        If newPage Then
            For i As Integer = 0 To dtgv_print.Columns.Count - 1
                If dtgv_print.Columns(i).Visible Then
                    Dim cellText As String = dtgv_print.Columns(i).HeaderText
                    Dim cellWidth As Integer = cellWidths(i)
                    rc = New Rectangle(x, y, cellWidth, 40)

                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    fmt.Alignment = StringAlignment.Center
                    fmt.LineAlignment = StringAlignment.Center
                    e.Graphics.DrawString(cellText, New Font("Franklin Gothic Medium", 9, FontStyle.Bold), Brushes.Black, rc, fmt)

                    x += rc.Width
                    h = Math.Max(h, rc.Height)
                End If
            Next
            y += h
            newPage = False
        End If

        ' Print rows
        Dim displayNow As Integer

        For displayNow = mRow To dtgv_print.RowCount - 1
            row = dtgv_print.Rows(displayNow)
            x = (e.PageBounds.Width - totalTableWidth) / 2
            h = 0

            For i As Integer = 0 To dtgv_print.Columns.Count - 1
                If dtgv_print.Columns(i).Visible Then
                    Dim cell As DataGridViewCell = row.Cells(i)
                    Dim cellWidth As Integer = cellWidths(i)
                    rc = New Rectangle(x, y, cellWidth, cell.Size.Height)

                    e.Graphics.DrawRectangle(Pens.Black, rc)
                    fmt.Alignment = StringAlignment.Center
                    fmt.LineAlignment = StringAlignment.Center
                    e.Graphics.DrawString(cell.FormattedValue.ToString(), dtgv_print.Font, Brushes.Black, rc, fmt)

                    x += rc.Width
                    h = Math.Max(h, rc.Height)
                End If
            Next
            y += h

            ' Check if we need a new page
            If y + h > e.PageBounds.Height - 100 Then
                mRow = displayNow + 1
                e.HasMorePages = True
                newPage = True
                Return
            End If
        Next

        ' If all rows are printed
        mRow = 0
        e.HasMorePages = False
    End Sub


    Private Sub btn_browse_Click(sender As Object, e As EventArgs) Handles btn_browse.Click
        Dim result As DialogResult = openFile.ShowDialog()
        If result = DialogResult.OK Then
            txt_file.Text = openFile.FileName
        End If
    End Sub

    'ADD STUDENTS
    Private Sub btn_addStudents_Click(sender As Object, e As EventArgs) Handles btn_addStudents.Click
        If btn_addStudents.Cursor = Cursors.Hand Then

            Try
                If String.IsNullOrEmpty(txt_file.Text) Then
                    msg_warning.Show("Please select a file first.")
                    Return
                End If

                If Path.GetExtension(txt_file.Text).ToLower() <> ".json" Then
                    msg_warning.Show("Only JSON files can be uploaded.")
                    Return
                End If

                Dim filename As String = Path.GetFileName(txt_file.Text)
                Dim uploadPath As String = Path.Combine("Upload", filename)

                If Not Directory.Exists("Upload") Then
                    Directory.CreateDirectory("Upload")
                End If

                File.Copy(txt_file.Text, uploadPath, True)
                msg_information.Show("File Uploaded")

                ' Load and process the JSON file
                ProcessJsonFile(uploadPath)

            Catch ex As Exception
                msg_warning.Show("Error processing JSON file: " & ex.Message)
            End Try
            refreshData()
        End If
    End Sub

    'UPDATE STUDENTS
    Private Sub btn_updateStudents_Click(sender As Object, e As EventArgs) Handles btn_updateStudents.Click
        If btn_updateStudents.Cursor = Cursors.Hand Then
            person.DeleteStudents()

            Try
                If String.IsNullOrEmpty(txt_file.Text) Then
                    msg_warning.Show("Please select a file first.")
                    Return
                End If

                If Path.GetExtension(txt_file.Text).ToLower() <> ".json" Then
                    msg_warning.Show("Only JSON files can be uploaded.")
                    Return
                End If

                Dim filename As String = Path.GetFileName(txt_file.Text)
                Dim uploadPath As String = Path.Combine("Upload", filename)

                If Not Directory.Exists("Upload") Then
                    Directory.CreateDirectory("Upload")
                End If

                File.Copy(txt_file.Text, uploadPath, True)
                msg_information.Show("File Uploaded")

                ' Load and process the JSON file
                ProcessJsonFile(uploadPath)

            Catch ex As Exception
                msg_warning.Show("Error processing JSON file: " & ex.Message)
            End Try

            refreshData()
        End If
    End Sub

    'DELETE STUDENT
    Private Sub btn_deleteStudents_Click(sender As Object, e As EventArgs) Handles btn_deleteStudents.Click
        If btn_deleteStudents.Cursor = Cursors.Hand Then
            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    person.DeleteStudents()
                    msg_information.Show("Successfully Deleted!")
                    txt_file.Clear()
                    refreshData()
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try
        End If
    End Sub

    Private Sub ProcessJsonFile(filePath As String)
        Try
            Dim jsonData As String = File.ReadAllText(filePath)
            Dim studentsData As StudentsData = JsonConvert.DeserializeObject(Of StudentsData)(jsonData)

            For Each student In studentsData.Students

                person.AddNewStudent(student.StudentNo, student.FirstName, student.LastName, student.Email, student.Gender, student.StudentStatus, student.ProgramCode)

                For Each enrollment In student.Enrollments
                    person.enrollCourseToStudent(student.StudentNo, enrollment.CourseCode, enrollment.Section)
                Next
            Next

            msg_information.Show("Data uploaded successfully")

            refreshData()


        Catch ex As Exception
            msg_warning.Show("Error processing JSON file: " & ex.Message)
        End Try
    End Sub

    Public Class StudentsData
        Public Property Students As List(Of Student)
    End Class

    Public Class Student
        Public Property StudentNo As String
        Public Property FirstName As String
        Public Property LastName As String
        Public Property Email As String
        Public Property Gender As String
        Public Property StudentStatus As String
        Public Property ProgramCode As String
        Public Property Enrollments As List(Of Enrollment)
    End Class

    Public Class Enrollment
        Public Property CourseCode As String
        Public Property Section As String
    End Class

    'UPDATE PROFILE
    Private Sub btn_updateProfile_Click(sender As Object, e As EventArgs) Handles btn_updateProfile.Click
        If btn_updateProfile.Cursor = Cursors.Hand Then
            school.UpdateSchool(txt_schoolId.Text, txt_schoolName.Text, txt_schoolAdd.Text, oldSchoolId)
            refreshData()
        End If
    End Sub

    Private Sub txt_schoolId_TextChanged(sender As Object, e As EventArgs) Handles txt_schoolId.TextChanged
        If txt_schoolId.Text = oldSchoolId Then
            btn_updateProfile.Cursor = Cursors.No
        Else
            btn_updateProfile.Cursor = Cursors.Hand
        End If
    End Sub

    Private Sub txt_schoolName_TextChanged(sender As Object, e As EventArgs) Handles txt_schoolName.TextChanged
        If txt_schoolName.Text = oldSchoolName Then
            btn_updateProfile.Cursor = Cursors.No
        Else
            btn_updateProfile.Cursor = Cursors.Hand
        End If
    End Sub

    Private Sub txt_schoolAdd_TextChanged(sender As Object, e As EventArgs) Handles txt_schoolAdd.TextChanged
        If txt_schoolAdd.Text = oldSchoolAdd Then
            btn_updateProfile.Cursor = Cursors.No
        Else
            btn_updateProfile.Cursor = Cursors.Hand
        End If
    End Sub

    Private Sub btn_reset_Click(sender As Object, e As EventArgs) Handles btn_reset.Click
        Dim tables As String() = {"tbl_users", "tbl_schools", "tbl_students", "tbl_programs", "tbl_instructors", "tbl_rooms", "tbl_courses"}

        Dim result As DialogResult = msg_confirm.Show("Are you sure you want to reset the system all the data will gone also your grade?")

        If result = DialogResult.Yes Then
            For Each table As String In tables
                executeQuery($"DELETE FROM {table}")
            Next
            msg_information.Show("Succesfully Deleted!")
            refreshData()
            txt_file.Clear()
            Me.Hide()
            frm_Setup.Show()
        End If
    End Sub

    Dim oldUsername As String

    'SELECTED USER
    Private Sub user_SelectionChange(sender As Object, e As EventArgs) Handles dtgv_users.SelectionChanged
        If dtgv_users.SelectedRows.Count > 0 Then

            'FILL THE PROGRAM DETAILS
            Dim selectedRows = dtgv_users.SelectedRows(0)
            oldUsername = selectedRows.Cells("username").Value.ToString
            txt_username.Text = selectedRows.Cells("username").Value.ToString
            txt_password.Text = selectedRows.Cells("password").Value.ToString
            cb_role.SelectedItem = selectedRows.Cells("role").Value.ToString

            'VALIDATE THE BUTTON
            btn_addUser.Cursor = Cursors.No
            btn_updateUser.Cursor = Cursors.Hand
            btn_deleteUser.Cursor = Cursors.Hand
        Else
            'VALIDATE THE BUTTON
            btn_addUser.Cursor = Cursors.Hand
            btn_updateUser.Cursor = Cursors.No
            btn_deleteUser.Cursor = Cursors.No
        End If
    End Sub

    Private Sub tbp_Setting_Click(sender As Object, e As EventArgs) Handles tbp_Setting.Click
        txt_username.Clear()
        txt_password.Clear()
        cb_role.SelectedIndex = 0
        dtgv_users.ClearSelection()
    End Sub

    'CLEAR USER
    Private Sub btn_clearUser_Click(sender As Object, e As EventArgs) Handles btn_clearUser.Click
        txt_username.Clear()
        txt_password.Clear()
        cb_role.SelectedIndex = 0

        For Each row As DataGridViewRow In dtgv_users.SelectedRows
            row.Selected = False
        Next
    End Sub

    'DELETE USER
    Private Sub btn_deleteUser_Click(sender As Object, e As EventArgs) Handles btn_deleteUser.Click
        If btn_deleteUser.Cursor = Cursors.Hand Then
            Try
                Dim result As DialogResult = msg_confirm.Show("Are you sure you want to delete this?")

                If result = DialogResult.Yes Then
                    user.deleteUserByUsername(txt_username.Text)
                    msg_information.Show("Successfully Deleted!")
                    refreshData()
                End If
            Catch ex As Exception
                msg_warning.Show("Error: " & ex.Message)
            End Try
        End If
    End Sub

    'UPDATE USER
    Private Sub btn_updateUser_Click(sender As Object, e As EventArgs) Handles btn_updateUser.Click
        If btn_updateUser.Cursor = Cursors.Hand Then
            user.updateUserByUsername(txt_username.Text, txt_password.Text, cb_role.SelectedItem.ToString, oldUsername)
            refreshData()
        End If
    End Sub

    'ADD USER
    Private Sub btn_addUser_Click(sender As Object, e As EventArgs) Handles btn_addUser.Click
        If btn_addUser.Cursor = Cursors.Hand Then
            Dim textboxes() As Guna2TextBox = {txt_username, txt_password}
            Dim comboBoxes() As Guna2ComboBox = {cb_role}

            Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)
            Dim isNotSelected As Boolean = ISComboBoxChange(comboBoxes)

            If isEmpty Or isNotSelected Then
                msg_warning.Show("Fill the reqired Fields")
            Else

                user.createNewUser(txt_username.Text, txt_password.Text, cb_role.SelectedItem.ToString)

                refreshData()

                ClearTextbox(textboxes)
                DefaultCombobox(comboBoxes)
            End If

        End If
    End Sub

    Private Sub dtgv_schedule_CellContentClick(sender As Object, e As DataGridViewCellEventArgs) Handles dtgv_schedule.CellContentClick

    End Sub
End Class
