Imports System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel

Public Class Schools
    Private sql As String

    'CREATE NEW SCHOOL'
    Public Sub CreateNewSchool(school_id As String, school_name As String, address As String)
        Try
            sql = "INSERT INTO tbl_schools (school_id, school_name, address) VALUES ('" & school_id & "', '" & school_name & "', '" & address & "')"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Function CountSchool() As Integer
        Dim tryCount As Integer = 0

        Try
            sql = "SELECT COUNT(*) FROM tbl_schools"
            tryCount = count(sql)

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return tryCount
    End Function


    'READ ALL SCHOOL'
    Function ReadAllSchool() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT * FROM tbl_schools"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return dataTable
    End Function


    'UPDATE SCHOOL '
    Sub UpdateSchool(school_id As String, school_name As String, address As String, oldSchoolId As String)
        Try
            sql = "UPDATE tbl_schools SET school_id = '" & school_id & "', school_name = '" & school_name & "', address = '" & address & "' WHERE school_id = '" & oldSchoolId & "'"
            update(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub


    Function ValidateSchool(schoolId As String) As Boolean
        Dim isTaken As Boolean = False
        Try
            sql = "SELECT COUNT(*) FROM tbl_schools WHERE school_id = '" & schoolId & "'"
            isTaken = check(sql)
            If isTaken = True Then
                MessageBox.Show("Username is already Taken!")
            End If

        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return isTaken
    End Function
End Class
