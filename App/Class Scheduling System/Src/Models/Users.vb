Public Class Users

    Private sql As String

    Sub createNewUser(username As String, password As String, role As String)
        Try
            sql = "INSERT INTO tbl_users (username, password, role) VALUES('" & username & "', MD5('" & password & "'), '" & role & "')"
            executeQuery(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Function readUserByUsername(username As String) As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT * FROM tbl_users WHERE username = '" & username & "'"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    Sub updateUserByUsername(newUsername As String, newPassword As String, newRole As String, username As String)
        Try
            sql = "UPDATE tbl_users SET username = '" & newUsername & "', password = MD5('" & newPassword & "'), role = '" & newRole & "' WHERE username = '" & username & "'"
            update(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Sub deleteUserByUsername(username As String)
        Try
            sql = "DELETE FROM tbl_users WHERE username = '" & username & "'"
            delete(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Function readAllUsers() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT * FROM tbl_users LIMIT 18446744073709551615 OFFSET 1;"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    Function getUserRole(username As String, password As String)
        Dim dataTable As New DataTable
        Try
            sql = "SELECT * FROM tbl_users WHERE username = '" & username & "' AND password = '" & password & "'"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    Function loginUser(username As String, password As String, role As String) As Boolean
        Dim success As Boolean = False
        Try
            sql = "SELECT COUNT(*) FROM tbl_users WHERE username = '" & username & "' AND password =  MD5('" & password & "') AND  role = '" & role & "'"

            success = check(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return success
    End Function

    Function validateUsername(username As String) As Boolean
        Dim isTaken As Boolean = False
        Try
            sql = "SELECT COUNT(username) FROM tbl_users WHERE username = '" & username & "'"
            isTaken = check(sql)
            If isTaken = True Then
                MessageBox.Show("Username is already Taken!")
            End If

        Catch ex As Exception
        End Try
        Return isTaken
    End Function

End Class

