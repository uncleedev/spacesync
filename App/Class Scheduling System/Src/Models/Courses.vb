Public Class Courses
    Private sql As String

    'ADD COURSE'
    Public Sub AddCourse(course_code As String, description As String, credits As Integer, program_code As String, yrLevel As Integer, TypeOfRoom As String)
        Try
            sql = "INSERT INTO tbl_courses (course_code, description, credits, program_code, yrLevel, TypeOfRoom) VALUES('" & course_code & "', '" & description & "', " & credits & ", '" & program_code & "', " & yrLevel & ", '" & TypeOfRoom & "')"
            create(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub

    'READ ALL COURSE'
    Public Function ReadAllCourse() As DataTable
        Dim dt As New DataTable
        Try
            sql = "SELECT * FROM tbl_courses"
            dt = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return dt
    End Function

    'UPDATE COURSE'
    Public Sub UpdateCourse(course_code As String, description As String, credits As Integer, program_code As String, yrLevel As Integer, TypeOfRoom As String, oldSubject As String, oldProgram As String)
        Try
            Dim sql As String = "UPDATE tbl_courses SET course_code = '" & course_code & "', description = '" & description & "', credits = " & credits & ", program_code = '" & program_code & "', yrLevel = " & yrLevel & ", TypeOfRoom = '" & TypeOfRoom & "' WHERE course_code = '" & oldSubject & "' AND program_code = '" & oldProgram & "'"
            update(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try
    End Sub

    'DELETE COURSE BY COURSE CODE'
    Public Sub DeleteCourseByCode(course_code As String)
        Try
            sql = "DELETE FROM tbl_courses WHERE course_code = '" & course_code & "'"
            delete(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub
End Class
