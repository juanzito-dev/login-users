
Imports MySql.Data.MySqlClient
Public Class FormRegistro

    Dim conexion As MySqlConnection


    Sub New()
        InitializeComponent()
        conexion = New MySqlConnection
        conexion.ConnectionString = "server=localhost; database=usuarios_roles;Uid=root;Pwd=;"
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click

        If (txtUser.Text <> "" And txtPass.Text <> "" And txtPass2.Text <> "") Then

            If (txtPass.Text = txtPass2.Text) Then

                If (UsuarioExiste(txtUser.Text, conexion)) Then
                    MessageBox.Show("Error. Nombre de usuario existente.")
                Else
                    RegistrarUsuario(txtUser.Text, txtPass.Text, conexion)
                End If

            Else
                MessageBox.Show("Error. Las contraseñas no coinciden.")
            End If
        Else
            MessageBox.Show("Por favor rellena todos los campos")
        End If









    End Sub


    Function UsuarioExiste(ByVal nombre As String, ByVal conexion As MySqlConnection)
        Dim resultado As Boolean

        Dim cmd As New MySqlCommand
        Dim reader As MySqlDataReader



        Try
            conexion.Open()
            cmd.Connection = conexion
            cmd.CommandText = "SELECT Nombre FROM usuarios WHERE Nombre=@nombre"
            cmd.Prepare()
            cmd.Parameters.AddWithValue("@nombre", nombre)

            reader = cmd.ExecuteReader()

            If (reader.HasRows) Then
                MsgBox("Este nombre de usuario ya existe")
            Else
                resultado = False
            End If

            conexion.Close()

            Return resultado

        Catch ex As Exception
            MsgBox("Error al conectar a la base de datos")
        End Try


    End Function




    Private Sub RegistrarUsuario(ByVal nombre As String, ByVal pass As String, ByVal conexion As MySqlConnection)
        Dim cmd As New MySqlCommand


        Try
            conexion.Open()

            cmd.Connection = conexion

            cmd.CommandText = "INSERT INTO usuarios(Nombre,Pass,idRol,Activo) VALUES(@nombre,@pass,@rol,@activo)"

            cmd.Prepare()

            cmd.Parameters.AddWithValue("@nombre", nombre)
            cmd.Parameters.AddWithValue("@pass", pass)
            cmd.Parameters.AddWithValue("@rol", 3)
            cmd.Parameters.AddWithValue("@activo", 0)

            cmd.ExecuteNonQuery()


            conexion.Close()

            MsgBox("Registro exitoso.")
            txtUser.Text = ""
            txtPass.Text = ""
            txtPass2.Text = ""

        Catch ex As Exception

            MessageBox.Show("Error.")

        End Try

    End Sub

    Private Sub FormRegister_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        txtPass.PasswordChar = "*"
        txtPass2.PasswordChar = "*"
    End Sub


End Class