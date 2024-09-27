Imports MySql.Data.MySqlClient
Imports Mysqlx

Module Database
    Public conn As New MySqlConnection
    Private cmd As New MySqlCommand
    Public dbServer As String = "localhost"
    Public dbUser As String = "root"
    Public dbPassword As String = ""
    Public dbName As String = "scheduling_schema"

    Sub openConn()
        Try
            conn.ConnectionString = "server='" & dbServer & "';user='" & dbUser & "';password='" & dbPassword & "';database='" & dbName & "'"
            conn.Open()
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    Sub closeConn()
        If conn.State = ConnectionState.Open Then
            conn.Close()
        End If
    End Sub

    Sub create(sql As String)
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            frm_Main.msg_add.Show("Successfully Added!")
        Catch ex As Exception
            frm_Main.msg_warning.Show("Error inserting Data: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    Sub executeQuery(sql As String)
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            frm_Main.msg_warning.Show("Error excuting Data: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    Function read(sql As String) As DataTable
        Dim dataTable As New DataTable()
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            Dim adapter As New MySqlDataAdapter(cmd)
            adapter.Fill(dataTable)
        Catch ex As Exception
            Console.WriteLine("Error reading data: " & ex.ToString)
        Finally
            closeConn()
        End Try
        Return dataTable
    End Function

    Sub update(sql As String)
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
            frm_Main.msg_update.Show("Successfully Updated!")
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Console.WriteLine("Error updating data: " & ex.ToString)
        Finally
            closeConn()
        End Try
    End Sub

    Sub delete(sql As String)
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            cmd.ExecuteNonQuery()
        Catch ex As Exception
            Console.WriteLine("Error deleting data: " & ex.Message)
        Finally
            closeConn()
        End Try
    End Sub

    Function check(sql As String)
        Dim result As Boolean = False
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            Dim count As Integer = Convert.ToInt64(cmd.ExecuteScalar())

            If count > 0 Then
                result = True
            End If

        Catch ex As Exception
            Console.WriteLine("Error cheking data " & ex.ToString)
        Finally
            closeConn()
        End Try
        Return result

    End Function

    Function count(sql As String) As Integer
        Dim counts As Integer = 0
        Try
            openConn()
            cmd = New MySqlCommand(sql, conn)
            counts = Convert.ToInt64(cmd.ExecuteScalar)
        Catch ex As Exception
            Console.WriteLine("Error cheking data " & ex.ToString)
        Finally
            closeConn()
        End Try
        Return counts
    End Function
End Module
