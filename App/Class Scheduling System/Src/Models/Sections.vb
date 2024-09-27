Public Class Sections
    Private sql As String


    'ADD SECTIONS'
    Public Sub AddSection(program_code As String, year As String, name As String)
        Try
            Dim sql As String = "INSERT INTO tbl_sections(category, program_code, year, name) VALUES('" & program_code & " " & year & name & "', '" & program_code & "', '" & year & "', '" & name & "')"
            create(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'READ ALL SECTION BASE ON COURSE AND INSTRUCTOR
    Function ReadSectionByCourseAndInstructor(course_code As String, instructor_no As String) As DataTable
        Dim dataTable As New DataTable
        'AND section NOT IN (SELECT section FROM tbl_schedules WHERE instructor_no = '" & instructor_no & "' OR course_code = '" & course_code & "')
        Try
            sql = "SELECT section 
                   FROM tbl_sessions
                   INNER JOIN tbl_sections ON tbl_sections.category = tbl_sessions.section
                   INNER JOIN tbl_courses ON tbl_courses.program_code = tbl_sections.program_code AND tbl_courses.yrLevel = tbl_sections.year 
                   WHERE instructor_no = '" & instructor_no & "' AND course_code = '" & course_code & "'
                   "
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    'READING ALL SECTIONS DATA
    Function ReadAllSections() As DataTable
        Dim dt As New DataTable

        Try
            sql = "SELECT * FROM tbl_sections"
            dt = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dt
    End Function


    'UPDATE ALL INFORMATION'
    Public Sub UpdateSection(category As String, program_code As String, year As String, name As String)
        Try
            sql = "UPDATE tbl_sections SET program_code = '" & program_code & "', year = '" & year & "', name = '" & name & "', category = CONCAT('" & program_code & "', ' ', '" & year & "', '" & name & "') WHERE category = '" & category & "'"
            update(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub



    'DELETE SECTION BY CATEGORY'
    Public Sub DeleteSectionByCategory(category As String)
        Try
            sql = "DELETE FROM tbl_sections WHERE category = '" & category & "'"
            delete(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Function SectionNotSchedule() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT category as Category FROM tbl_sections WHERE category NOT IN (SELECT DISTINCT section FROM tbl_schedules);"

            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function
End Class
