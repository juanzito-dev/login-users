
Imports MySql.Data.MySqlClient

Public Class formPanel
    Dim conexion As MySqlConnection
    Sub New(ByVal rol As String)

        InitializeComponent()
        If (rol = "administrador") Then
            Panel1.Visible = True
        Else
            Panel1.Visible = False
        End If
        conexion = New MySqlConnection

        conexion.ConnectionString = "server=localhost; database=usuarios_roles;Uid=root;Pwd=;"

    End Sub

    Sub Actualizar()
        Dim ds As DataSet = New DataSet
        Dim cmd As New MySqlCommand
        Dim adaptador As MySqlDataAdapter = New MySqlDataAdapter

        Try
            conexion.Open()
            cmd.Connection = conexion

            cmd.CommandText = "SELECT idUsuario,Nombre,Activo FROM usuarios ORDER BY Nombre ASC"
            adaptador.SelectCommand = cmd
            adaptador.Fill(ds, "Tabla")
            DataGridView1.DataSource = ds
            DataGridView1.DataMember = "Tabla"

            conexion.Close()

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try


    End Sub

    Private Sub DataGridView1_SelectionChanged(sender As Object, e As DataGridViewCellEventArgs) Handles DataGridView1.CellClick
        If (DataGridView1.SelectedRows.Count > 0) Then
            TextBox1.Text = DataGridView1.Item("Nombre", DataGridView1.SelectedRows(0).Index).Value
            TextBox4.Text = DataGridView1.Item("idUsuario", DataGridView1.SelectedRows(0).Index).Value
        End If
    End Sub

    Private Sub frmMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Actualizar()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim nombre As String
        nombre = TextBox1.Text

        ActivarUsuario(nombre, conexion)
        Actualizar()
    End Sub



    Private Sub ActivarUsuario(ByVal nombre As String, ByVal con As MySqlConnection)
        Dim cmd As New MySqlCommand
        Dim reader As MySqlDataReader
        Try

            conexion.Open()
            cmd.Connection = con
            cmd.CommandText = "SELECT Activo FROM usuarios WHERE Nombre=@nombre AND Activo=1"

            cmd.Prepare()

            cmd.Parameters.AddWithValue("@nombre", nombre)

            reader = cmd.ExecuteReader()

            If (reader.HasRows) Then
                MessageBox.Show("Este usuario ya esta activo.")
                reader.Close()

            Else
                reader.Close()

                cmd.CommandText = "UPDATE usuarios SET Activo=1 WHERE Nombre=@nombre"
                cmd.Prepare()
                cmd.Parameters.Clear()

                cmd.Parameters.AddWithValue("@nombre", nombre)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Activacion exitosa.")

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        conexion.Close()

    End Sub


    Private Sub CambiarPass(ByVal NewPass As String, ByVal id As Integer, ByVal con As MySqlConnection, ByVal usu As String)

        Dim cmd As New MySqlCommand


        Try
            conexion.Open()
            cmd.Connection = con

            cmd.CommandText = "UPDATE usuarios SET Pass=@pass WHERE idUsuario=@ID"
            cmd.Prepare()

            cmd.Parameters.AddWithValue("@pass", NewPass)
            cmd.Parameters.AddWithValue("@ID", id)

            cmd.ExecuteNonQuery()

            MessageBox.Show("Cambio exitoso.")

        Catch ex As Exception
        End Try

        conexion.Close()

    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Dim Id As Integer
        Dim passNueva As String

        Id = Integer.Parse(TextBox4.Text)
        passNueva = TextBox3.Text
        CambiarPass(passNueva, Id, conexion, TextBox1.Text)
        Actualizar()

    End Sub

    Private Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Dim nombre As String
        nombre = TextBox1.Text

        DesactivarUsuario(nombre, conexion)
        Actualizar()
    End Sub

    Private Sub DesactivarUsuario(ByVal nombre As String, ByVal con As MySqlConnection)
        Dim cmd As New MySqlCommand
        Dim reader As MySqlDataReader
        Try

            conexion.Open()
            cmd.Connection = con
            cmd.CommandText = "SELECT Activo FROM usuarios WHERE Nombre=@nombre AND Activo=0"

            cmd.Prepare()

            cmd.Parameters.AddWithValue("@nombre", nombre)

            reader = cmd.ExecuteReader()

            If (reader.HasRows) Then
                MessageBox.Show("Este usuario ya esta desactivado.")
                reader.Close()

            Else
                reader.Close()

                cmd.CommandText = "UPDATE usuarios SET Activo=0 WHERE Nombre=@nombre"
                cmd.Prepare()
                cmd.Parameters.Clear()

                cmd.Parameters.AddWithValue("@nombre", nombre)
                cmd.ExecuteNonQuery()

                MessageBox.Show("Desactivacion exitosa.")

            End If

        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        conexion.Close()

    End Sub

    Private Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Dim busqueda As String
        busqueda = TextBox2.Text

        BusquedaUser(busqueda, conexion)

    End Sub


    Sub BusquedaUser(ByVal busqueda As String, ByVal con As MySqlConnection)
        Dim ds As DataSet = New DataSet
        Dim cmd As New MySqlCommand
        Dim adaptador As MySqlDataAdapter = New MySqlDataAdapter
        Dim readerb As MySqlDataReader

        Try
            conexion.Open()
            cmd.Connection = conexion
            DataGridView1.DataSource = Nothing
            DataGridView1.Refresh()
            DataGridView1.Rows.Clear()
            cmd.CommandText = "SELECT idUsuario,Nombre,Activo FROM usuarios WHERE Nombre LIKE '%" + busqueda + "%' ORDER BY Nombre ASC"
            cmd.Prepare()
            readerb = cmd.ExecuteReader()
            If (readerb.HasRows) Then
                readerb.Close()
                adaptador.SelectCommand = cmd
                adaptador.Fill(ds, "Tabla")

                DataGridView1.DataSource = ds
                DataGridView1.DataMember = "Tabla"




            Else
                readerb.Close()
                MessageBox.Show("No hay coincidencias.")

            End If

            conexion.Close()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class