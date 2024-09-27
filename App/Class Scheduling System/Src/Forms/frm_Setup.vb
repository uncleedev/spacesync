Imports System.Media
Imports Guna.UI2.WinForms

Public Class frm_Setup
    'FORM LOAD
    Private Sub frm_Setup_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ckb_showHide.Checked = True
        txt_password.UseSystemPasswordChar = True
    End Sub

    'BUTTTON SUBMIT
    Private Sub btn_submit_Click(sender As Object, e As EventArgs) Handles btn_submit.Click
        Dim textboxes() As Guna2TextBox = {txt_schoolId, txt_schoolName, txt_schoolAdd, txt_username, txt_password}
        Dim isEmpty As Boolean = UpdateTextBoxes(textboxes)

        If isEmpty Then
            lbl_errorMsg.Text = "Fill the required Fields"
            lbl_errorMsg.Visible = True
        Else
            If user.validateUsername(txt_username.Text) And school.ValidateSchool(txt_schoolId.Text) Then
                lbl_errorMsg.Text = "School or username is already taken!"
                lbl_errorMsg.Visible = True
            Else
                user.createNewUser(txt_username.Text, txt_password.Text, "Admin")
                school.CreateNewSchool(txt_schoolId.Text, txt_schoolName.Text, txt_schoolAdd.Text)
                msg_information.Show("Successfully Login!")
                Me.Hide()
                frm_Login.Show()
                ClearTextbox(textboxes)
            End If
        End If
    End Sub

    'TEXTBOX TEXTCHANGE
    Private Sub textbox_Change(sender As Object, e As EventArgs) Handles txt_schoolId.TextChanged, txt_schoolName.TextChanged, txt_schoolAdd.TextChanged, txt_username.TextChanged, txt_password.TextChanged
        Dim textboxes() As Guna2TextBox = {txt_schoolId, txt_schoolName, txt_schoolAdd, txt_username, txt_password}
        TextChange(textboxes)
        lbl_errorMsg.Visible = False
    End Sub

    Private Sub btn_exit_Click(sender As Object, e As EventArgs) Handles btn_exit.Click
        Application.Exit()
    End Sub

    'PASSWORD SHOW AND HIDE
    Private Sub ckb_showHide_CheckedChanged(sender As Object, e As EventArgs) Handles ckb_showHide.CheckedChanged
        If ckb_showHide.Checked = True Then
            txt_password.UseSystemPasswordChar = True
        Else
            txt_password.UseSystemPasswordChar = False
        End If
    End Sub
End Class
