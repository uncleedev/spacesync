Public Class frm_LoadingScreen
    Private Sub frm_LoadingScreen_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Cursor = Cursors.WaitCursor
        Me.loadingBar.Value = 0
        Timer1.Interval = 100
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Me.loadingBar.Value += 5

        If loadingBar.Value = 50 Then
            openConn()
            ' Check connection at 50%
            If conn.State <> ConnectionState.Open Then
                Timer1.Stop()
                MessageBox.Show("Connection Failed!" & vbCrLf & "Please try opening your Apache and MySQL server.")
                ' Optionally reset the loading bar or close the form
                Me.loadingBar.Value = 0
                Me.Close()
                Exit Sub
                Exit Sub
            End If
        End If

        If loadingBar.Value >= 100 Then
            Timer1.Stop()

            If school.CountSchool() > 0 Then
                frm_Login.Show()
            Else
                frm_Setup.Show()
            End If
            closeConn()
            Me.Hide()
        End If
    End Sub
End Class
