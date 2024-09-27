Imports System.Globalization
Imports Guna.UI2.WinForms
Imports Guna.UI2.WinForms.Suite

Module Data
    'CLASSES TO USE
    Public user As New Users
    Public school As New Schools
    Public program As New Programs
    Public section As New Sections
    Public course As New Courses
    Public instructor As New Instructors
    Public room As New Rooms
    Public schedule As New Schedules
    Public person As New Students

    'STORAGE OF DATA
    Public usersData As DataTable
    Public schoolsData As DataTable
    Public programsData As DataTable
    Public sectionsData As DataTable
    Public coursesData As DataTable
    Public instructorsData As DataTable
    Public roomsData As DataTable
    Public schedulesData As DataTable
    Public studentSchedulesData As DataTable
    Public instructorSchedulesData As DataTable
    Public insNotSche As DataTable
    Public secNotSche As DataTable
    Public roomNotSche As DataTable
    Public instrucCourse As DataTable
    Public scheduleToday As DataTable
    Public ongoingSchedule As DataTable
    Public nextSchedule As DataTable
    Public finishSchedule As DataTable
    Public scheduleTime As DataTable = schedule.ReadAllStartTimeSchedule
    Public dataChart As DataTable

    'ALL DATA
    Sub refreshData()
        refreshRoomsData()
        refreshUsersData()
        refreshSchoolsData()
        refreshProgramsData()
        RefreshSectionsData()
        RefreshCoursesData()
        refreshInstructorsData()
        refreshSchedulesData()
        refreshDashboard()
    End Sub


    'USER
    Sub refreshUsersData()
        usersData = user.readAllUsers

        usersData.Columns.Remove("id")

        frm_Main.dtgv_users.DataSource = usersData

        For Each row As DataGridViewRow In frm_Main.dtgv_users.SelectedRows
            row.Selected = False
        Next
    End Sub

    'SCHOOL

    Public oldSchoolId As String
    Public oldSchoolName As String
    Public oldSchoolAdd As String
    Sub refreshSchoolsData()
        schoolsData = school.ReadAllSchool

        schoolsData.Columns.Remove("id")

        For Each row In schoolsData.Rows
            frm_Main.txt_schoolId.Text = row("school_id")
            frm_Main.txt_schoolName.Text = row("school_name")
            frm_Main.txt_schoolAdd.Text = row("address")
            oldSchoolId = row("school_id")
            oldSchoolName = row("school_name")
            oldSchoolAdd = row("address")
        Next
    End Sub

    'PROGRAM
    Sub refreshProgramsData()
        programsData = program.ReadAllPrograms
        programsData.Columns.Remove("id")

        frm_Main.dtgv_program.DataSource = programsData

        ' SECTION PROGRAM
        frm_Main.cb_sectionProg.Items.Clear()
        frm_Main.cb_sectionProg.Items.Add("Select Program")
        frm_Main.cb_sectionProg.SelectedIndex = 0

        'COURSE PROGRAM
        frm_Main.cb_courseProg.Items.Clear()
        frm_Main.cb_courseProg.Items.Add("Select Program")
        frm_Main.cb_courseProg.SelectedIndex = 0

        For Each row As DataRow In programsData.Rows
            frm_Main.cb_sectionProg.Items.Add(row("program_code").ToString())
            frm_Main.cb_courseProg.Items.Add(row("program_code").ToString())
        Next


        For Each row As DataGridViewRow In frm_Main.dtgv_program.SelectedRows
            row.Selected = False
        Next
    End Sub

    'SECTION
    Sub RefreshSectionsData()
        sectionsData = section.ReadAllSections()
        sectionsData.Columns.Remove("id")
        frm_Main.dtgv_section.DataSource = sectionsData

        secNotSche = section.SectionNotSchedule


        Dim dataCopy As DataTable = sectionsData.Copy
        dataCopy.Columns.Remove("program_code")
        dataCopy.Columns.Remove("year")
        dataCopy.Columns.Remove("name")

        frm_Main.dtgv_instructorSection.DataSource = dataCopy

        For Each row As DataGridViewRow In frm_Main.dtgv_section.SelectedRows
            row.Selected = False
        Next
    End Sub

    'COURSE
    Sub RefreshCoursesData()
        coursesData = course.ReadAllCourse()

        coursesData.Columns.Remove("id")
        frm_Main.dtgv_course.DataSource = coursesData

        Dim dataCopy As DataTable = coursesData.Copy
        dataCopy.Columns.Remove("description")
        dataCopy.Columns.Remove("credits")
        dataCopy.Columns.Remove("yrLevel")
        dataCopy.Columns.Remove("TypeOfRoom")

        frm_Main.dtgv_instructorCourse.DataSource = dataCopy

        For Each row As DataGridViewRow In frm_Main.dtgv_course.SelectedRows
            row.Selected = False
        Next
    End Sub

    'INSTRUCTOR
    Sub refreshInstructorsData()
        instructorsData = instructor.ReadAllInstructors()
        frm_Main.dtgv_instructor.DataSource = instructorsData

        insNotSche = instructor.InstructorNoSchedule

        frm_Main.cb_scheduleInstructor.Items.Clear()
        frm_Main.cb_scheduleInstructor.Items.Add("Select Instructor")
        frm_Main.cb_scheduleInstructor.SelectedIndex = 0

        For Each row As DataRow In instructorsData.Rows
            frm_Main.cb_scheduleInstructor.Items.Add(row("name").ToString())
        Next

        For Each row As DataGridViewRow In frm_Main.dtgv_instructor.SelectedRows
            row.Selected = False
        Next
    End Sub


    Sub refreshRoomsData()
        frm_Main.flowPanel.Controls.Clear()


        ongoingSchedule = schedule.ReadAllOngoingSchedule
        nextSchedule = schedule.ReadAllNextSchedule
        finishSchedule = schedule.ReadAllFinishSchedule

        AddSchedulePanels(ongoingSchedule, Color.FromArgb(255, 138, 128), Color.FromArgb(255, 244, 243))
        AddSchedulePanels(nextSchedule, Color.FromArgb(255, 213, 9), Color.FromArgb(255, 248, 241))
        AddSchedulePanels(finishSchedule, Color.FromArgb(76, 175, 80), Color.FromArgb(246, 246, 241))

        roomsData = room.ReadAllRooms()
        roomsData.Columns.Remove("id")
        frm_Main.dtgv_room.DataSource = roomsData

        roomNotSche = room.roomNotSchedule

        For Each row As DataGridViewRow In frm_Main.dtgv_room.SelectedRows
            row.Selected = False
        Next

        frm_Main.stackData.DataPoints.Clear()

        Dim rooms As DataTable = room.RoomsSchedule

        For Each row As DataRow In rooms.Rows
            frm_Main.stackData.DataPoints.Add(row("room"), row("total"))
        Next
    End Sub


    Public startTimet As String
    Public endTimet As String

    'SCHEDULE
    Sub refreshSchedulesData()
        schedulesData = schedule.ReadAllSchedules
        studentSchedulesData = person.ReadAllStudentsShedule
        instructorSchedulesData = instructor.ReadAllInstructorSchedule

        frm_Main.dtgv_schedule.DataSource = schedulesData

        For Each row As DataRow In schedulesData.Rows

            For Each rowInstructor In instructorsData.Rows
                If row("instructor_no") = rowInstructor("instructor_no") Then
                    row("instructor_no") = rowInstructor("name")
                End If
            Next
        Next

        For Each row As DataGridViewRow In frm_Main.dtgv_schedule.SelectedRows
            row.Selected = False
        Next

        frm_Main.donotData.DataPoints.Clear()

        frm_Main.donotData.DataPoints.Add("Finished", schedule.CountFinishedScheduleByDay)
        frm_Main.donotData.DataPoints.Add("On-queue", schedule.CountUpcomingScheduleByDay)
        frm_Main.donotData.DataPoints.Add("On-going", schedule.CountOnGoingScheduleByDay)

        frm_Main.splineData.DataPoints.Clear()

        Dim weeklySchedules As DataTable = schedule.WeeklySchedule

        For Each row As DataRow In weeklySchedules.Rows
            frm_Main.splineData.DataPoints.Add(row("day"), row("total"))
        Next
    End Sub

    'DASHBOARD
    Sub refreshDashboard()
        frm_Main.lbl_countInstrutor.Text = instructor.CountAllInstructors
        frm_Main.lbl_countFullTime.Text = instructor.CountInstructorByFulltime
        frm_Main.lbl_countPartTime.Text = instructor.CountInstructorByPartTime

        Dim countStudents As Integer = person.CountAllStudents

        frm_Main.lbl_countStudent.Text = countStudents
        frm_Main.lbl_stuRegular.Text = person.CountRegularStudent
        frm_Main.lbl_countStuIrregular.Text = person.CountIrregularStudent

        frm_Main.lbl_countRoom.Text = room.CountAllRooms
        frm_Main.lbl_countRoomReg.Text = room.CountRegularRooms
        frm_Main.lbl_countRoomLab.Text = room.CountLabRooms
        frm_Main.lbl_countRoomCourt.Text = room.CountCourtRooms

        frm_Main.lbl_countSchedule.Text = schedule.CountAllScheduleByDay
        frm_Main.lbl_countQueueSchedule.Text = schedule.CountUpcomingScheduleByDay
        frm_Main.lbl_countGoingSchedule.Text = schedule.CountOnGoingScheduleByDay
        frm_Main.lbl_countFinishedSchedule.Text = schedule.CountFinishedScheduleByDay


        If countStudents > 0 Then
            frm_Main.lbl_students.Text = "Student's Already Added!"
            frm_Main.btn_addStudents.Cursor = Cursors.No
            frm_Main.btn_updateStudents.Cursor = Cursors.Hand
            frm_Main.btn_deleteStudents.Cursor = Cursors.Hand
        Else
            frm_Main.lbl_students.Text = "No Student's Data!"
            frm_Main.btn_addStudents.Cursor = Cursors.Hand
            frm_Main.btn_updateStudents.Cursor = Cursors.No
            frm_Main.btn_deleteStudents.Cursor = Cursors.No
        End If

    End Sub



End Module
