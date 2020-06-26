Imports MySql.Data.MySqlClient
Public Class FormLogin
    Dim conexion As MySqlConnection
    Dim rol As String

    Sub New()
        InitializeComponent()
        conexion = New MySqlConnection
        conexion.ConnectionString = "server=localhost;database=usuarios_roles;Uid=root;Pwd=;"
    End Sub
    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        FormRegistro.Show()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If (txtUser.Text <> "" And txtPass.Text <> "") Then
            CheckUserExist(txtUser.Text, txtPass.Text, conexion)
        Else
            MessageBox.Show("Por favor rellena todos los campos.")
        End If
    End Sub

    Private Sub FormLogin_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPass.PasswordChar = "*"
    End Sub


    Function CheckUserExist(ByVal user As String, ByVal pass As String, ByVal conexion As MySqlConnection)
        Dim cmd As New MySqlCommand
        Dim reader As MySqlDataReader

        Try
            conexion.Open()
            cmd.Connection = conexion
            cmd.CommandText = "SELECT Pass,Activo,idRol FROM usuarios WHERE Nombre=@nombre"
            cmd.Prepare()

            cmd.Parameters.AddWithValue("@nombre", user)
            reader = cmd.ExecuteReader()

            If (reader.HasRows) Then
                While reader.Read()
                    If (reader(0).ToString = pass) Then
                        If (reader(1).ToString = "True") Then
                            If (reader.GetInt16("idRol") = 1) Then
                                rol = "administrador"
                            ElseIf (reader.GetInt16("idRol") = 2) Then
                                rol = "operador"
                            ElseIf (reader.GetInt16("idRol") = 3) Then
                                rol = "invitado"
                            Else
                                MessageBox.Show("error")
                            End If
                            Dim formPanel As New formPanel(rol)
                            formPanel.Show()
                        Else
                            MessageBox.Show("Error. Tu usuario se encuentra desativado.")
                        End If
                    Else
                        txtPass.BackColor = Color.Red
                        MessageBox.Show("Error. Contraseña incorrecta")
                        txtPass.BackColor = Color.White
                    End If
                End While
            Else
                txtUser.BackColor = Color.Red
                MessageBox.Show("Error. Usuario incorrecto ")
                txtUser.BackColor = Color.White

            End If
            conexion.Close()


        Catch ex As Exception
            MessageBox.Show("Error.")
            conexion.Close()
        End Try

    End Function

End Class
