Imports Mysqlx.Crud
Imports Mysqlx.Prepare

Public Class Instructors
    Private sql As String

    'CREATE'
    Sub CreateNewInstructor(instructor_no As String, firstname As String, lastname As String, email As String, gender As String, employee_status As String, availabilityList As List(Of String))
        Try
            Dim availability As String = String.Join(",", availabilityList)

            sql = "INSERT INTO tbl_instructors(instructor_no, firstname, lastname, email, gender, employee_status, availability) VALUES('" & instructor_no & "', '" & firstname & "', '" & lastname & "', '" & email & "', '" & gender & "', '" & employee_status & "', '" & availability & "')"

            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'UPDATE'
    Public Sub UpdateInstructor(instructor_no As String, firstname As String, lastname As String, email As String, gender As String, employee_status As String, availabilityList As List(Of String), oldInstructor As String)
        Try
            Dim availability As String = String.Join(",", availabilityList)

            sql = "UPDATE tbl_instructors SET instructor_no = '" & instructor_no & "',  firstname = '" & firstname & "', lastname = '" & lastname & "', email = '" & email & "', gender = '" & gender & "' , employee_status = '" & employee_status & "', availability = '" & availability & "' WHERE instructor_no = '" & oldInstructor & "'"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'DELETE INSTRUCTORS COURSES
    Sub DeleteCourseByInstructor(instructor_no As String)
        Try
            sql = "DELETE FROM tbl_subjects WHERE instructor_no = '" & instructor_no & "'"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'DELETE INSTRUCTORS SECTIONS
    Sub DeleteSectionByInstructor(instructor_no As String)
        Try
            sql = "DELETE FROM tbl_sessions WHERE instructor_no = '" & instructor_no & "'"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'READ ALL INSTRUCTORS'
    Function ReadAllInstructors() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT i.instructor_no, CONCAT(i.firstname, ' ', i.lastname) AS Name, i.email AS Email, i.gender as Gender, i.employee_status, i.availability, " &
              "GROUP_CONCAT(DISTINCT s.course_code ORDER BY s.course_code SEPARATOR ', ') AS Courses, " &
              "GROUP_CONCAT(DISTINCT se.section ORDER BY se.section SEPARATOR ', ') AS Sections " &
              "FROM tbl_instructors i " &
              "LEFT JOIN tbl_subjects s ON i.instructor_no = s.instructor_no " &
              "LEFT JOIN tbl_sessions se ON i.instructor_no = se.instructor_no " &
              "GROUP BY i.instructor_no, i.firstname, i.lastname, i.email, i.gender, i.employee_status, i.availability"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    'INSTRUCTORS SCHEDULE
    Function ReadAllInstructorSchedule() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT s.room_no, s.course_code, c.description AS Description, c.credits AS Credits, CONCAT(i.firstname, ' ', i.lastname) AS Instructor, s.section, s.class_day, CONCAT(TIME_FORMAT(s.start_time, '%h:%i %p'), ' - ', TIME_FORMAT(s.end_time, '%h:%i %p')) AS Time FROM tbl_schedules s LEFT JOIN tbl_instructors i ON s.instructor_no = i.instructor_no LEFT JOIN tbl_courses c ON s.course_code = c.course_code ORDER BY FIELD(s.class_day, 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat', 'Sun'), s.start_time DESC;"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    'INSTRUCTORS AVAILABILITY
    Function ReadInstructorAvailability(instructor_no As String) As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT availability FROM tbl_instructors WHERE instructor_no = '" & instructor_no & "'"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    'FIND INSTRUCTOR NO
    Function FindInstructorNoByName(firstname As String, lastname As String) As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT instructor_no FROM tbl_instructors WHERE firstname = '" & firstname & "' AND lastname = '" & lastname & "'"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)

        End Try
        Return dataTable
    End Function

    'DELETE INSTRUCTOR
    Public Sub DeleteInstructorNo(instructor_no As String)
        Try
            sql = "DELETE FROM tbl_instructors WHERE instructor_no = '" & instructor_no & "'"
            delete(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)

        End Try
    End Sub

    Sub DeleteSubjectsByInstructor(instructor_no As String)
        Try
            sql = "DELETE FROM tbl_subjects WHERE instructor_no = '" & instructor_no & "'"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Sub DeleteSectionsByInstructor(instructor_no As String)
        Try
            sql = "DELETE FROM tbl_sessions WHERE instructor_no = '" & instructor_no & "'"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'COUNT ALL INSTRUCTOR'
    Function CountAllInstructors() As Integer
        Dim tryCount As Integer = 0

        Try
            sql = "SELECT COUNT(*) FROM tbl_instructors"
            tryCount = count(sql)

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return tryCount
    End Function

    'COUNT INSTRUCTOR FULL TIME'
    Function CountInstructorByFulltime() As Integer
        Dim tryCount As Integer = 0


        Try
            sql = "SELECT COUNT(*) FROM tbl_instructors WHERE employee_status = 'FULL TIME' "
            tryCount = count(sql)

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return tryCount
    End Function

    'COUNT INSTRUCTOR PART TIME'
    Function CountInstructorByPartTime()
        Dim tryCount As Integer
        Try
            sql = "SELECT COUNT(*) FROM tbl_instructors WHERE employee_status = 'PART TIME' "
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return tryCount
    End Function

    'ADD COURSES TO INSTRUCTOR'
    Public Sub AddCoursesToInstructor(insturctor_no As String, course_code As String)
        Try
            sql = "INSERT INTO tbl_subjects (instructor_no, course_code) VALUES ('" & insturctor_no & "','" & course_code & "')"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'ADD SECTIONS TO INSTRUCTOR'
    Public Sub AddSectionsToInstructor(insturctor_no As String, section As String)
        Try
            sql = "INSERT INTO tbl_sessions (instructor_no, section) VALUES ('" & insturctor_no & "','" & section & "')"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'INSTRUCTOR NO SCHEDULE'
    Public Function InstructorNoSchedule() As DataTable
        Dim dt As New DataTable()
        Try
            sql = "SELECT CONCAT(firstname, ' ', lastname) AS Name FROM tbl_instructors WHERE instructor_no NOT IN (SELECT DISTINCT instructor_no FROM tbl_schedules);"
            dt = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return dt
    End Function

    Function ReadSubjectCodeByInstrucNo(instructor_no As String) As DataTable
        Dim datatable As New DataTable
        Try
            sql = "SELECT course_code from tbl_subjects WHERE instructor_no = '" & instructor_no & "'"
            datatable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)

        End Try
        Return datatable
    End Function

End Class
