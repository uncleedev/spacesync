
Public Class Programs
    Private sql As String

    'ADD PROGRAMS'
    Sub CreateProgram(program_code As String, name As String, department As String, NoOfYear As Integer)
        Try
            sql = "INSERT INTO tbl_programs (program_code, name, department, NoOfYear) VALUES('" & program_code & "', '" & name & "', '" & department & "', '" & NoOfYear & "')"
            create(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'READING ALL PROGRAMS DATA
    Function ReadAllPrograms() As DataTable
        Dim dt As New DataTable

        Try
            sql = "SELECT * FROM tbl_programs"
            dt = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dt
    End Function

    'UPDATE ALL PROGRAMS'
    Public Sub UpdatePrograms(program_code As String, name As String, department As String, NoOfYear As String, oldProgram_code As String)
        Try
            sql = "UPDATE tbl_programs SET program_code = '" & program_code & "', name = '" & name & "', department = '" & department & "', NoOfYear = '" & NoOfYear & "' WHERE program_code = '" & oldProgram_code & "'"
            update(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'DELETE PROGRAM BY NUMBER'
    Public Sub DeleteProgramByNo(program_code As String)
        Try
            sql = "DELETE FROM tbl_programs WHERE program_code = '" & program_code & "'"
            delete(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub
End Class