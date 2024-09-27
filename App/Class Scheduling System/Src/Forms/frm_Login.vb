Imports System.Media
Imports System.Windows.Forms.VisualStyles.VisualStyleElement
Imports Google.Protobuf.WellKnownTypes
Imports Guna.UI2.WinForms

Public Class frm_Login
    'FORM LOAD
    Private Sub frm_Login_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ckb_lgHideIcon.Checked = True
        txt_password.UseSystemPasswordChar = True
        ckb_role.BorderColor = Color.FromArgb(38, 166, 154)
    End Sub

    Private Sub btn_login_Click(sender As Object, e As EventArgs) Handles btn_login.Click
        Dim textboxes() As Guna2TextBox = {txt_username, txt_password}
        Dim isEmpty As Boolean = False

        For Each textbox As Guna2TextBox In textboxes
            If EmptyCheck(textbox) Then
                textbox.BorderColor = Color.FromArgb(255, 138, 128)
                isEmpty = True
            Else
                textbox.BorderColor = Color.FromArgb(38, 166, 154)
            End If
        Next

        If isEmpty Then
            lbl_errorMsg.Text = "Fill the required Fields"
            lbl_errorMsg.Visible = True
        ElseIf ckb_role.SelectedItem = "Select Role" Then
            ckb_role.BorderColor = Color.FromArgb(255, 138, 128)
        Else
            If user.loginUser(txt_username.Text, txt_password.Text, ckb_role.SelectedItem.ToString) Then
                msg_succes.Show("Successfully Login")
                frm_Main.lbl_role.Text = ckb_role.SelectedItem.ToString

                If ckb_role.SelectedItem.ToString = "Viewer" Then
                    frm_Main.login_pic.Image = My.Resources.icon_viewer
                    frm_Main.pnl_sidebar.Visible = False
                    HandleButton_Click(frm_Main.btn_Monitor, frm_Main.tbp_Monitor)
                End If

                If ckb_role.SelectedItem.ToString = "Admin" Then
                    frm_Main.login_pic.Image = My.Resources.icon_admin
                    frm_Main.pnl_sidebar.Visible = True
                    HandleButton_Click(frm_Main.btn_Dashboard, frm_Main.tbp_Dashboard)
                End If


                Me.Hide()
                frm_Main.Show()
                txt_username.Clear()
                txt_password.Clear()
                ckb_role.StartIndex = 0

            Else
                lbl_errorMsg.Text = "Incorrent username or password"
                lbl_errorMsg.Visible = True
            End If
        End If
    End Sub

    Private Sub textbox_Change(sender As Object, e As EventArgs) Handles txt_username.TextChanged, txt_password.TextChanged, ckb_role.SelectedIndexChanged
        Dim textboxes() As Guna2TextBox = {txt_username, txt_password}
        ckb_role.BorderColor = Color.FromArgb(38, 166, 154)
        TextChange(textboxes)
        lbl_errorMsg.Visible = False
    End Sub

    Private Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        Application.Exit()
    End Sub

    Private Sub ckb_lgHideIcon_CheckedChanged(sender As Object, e As EventArgs) Handles ckb_lgHideIcon.CheckedChanged
        If ckb_lgHideIcon.Checked = True Then
            txt_password.UseSystemPasswordChar = True
        Else
            txt_password.UseSystemPasswordChar = False
        End If
    End Sub
End Class