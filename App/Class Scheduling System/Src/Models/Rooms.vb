Public Class Rooms
    Private sql As String


    'ADD ROOMS'
    Public Sub AddNewRoom(room_no As String, building_name As String, floor_level As Integer, TypeOfRoom As String)
        Try
            sql = "INSERT INTO tbl_rooms(room_no, building_name, floor_level, TypeOfRoom) VALUES ('" & room_no & "', '" & building_name & "', '" & floor_level & "', '" & TypeOfRoom & "')"
            create(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'UPDATE ROOMS'
    Public Sub UpdateRoom(room_no As String, building_name As String, floor_level As String, TypeOfRoom As String, oldRoomNo As String)
        Try
            sql = "UPDATE tbl_rooms SET room_no = '" & room_no & "', building_name = '" & building_name & "', floor_level = '" & floor_level & "', TypeOfRoom = '" & TypeOfRoom & "'  WHERE room_no = '" & oldRoomNo & "'"
            update(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'READ ALL ROOMS'
    Public Function ReadAllRooms() As DataTable
        Dim datatable As New DataTable

        Try
            sql = "SELECT * FROM tbl_rooms"
            datatable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return datatable
    End Function

    'READ ALL ROOM BASE ON COURSE AND SECTION
    Function RealAllRoomByCourseAndSection(course_code As String, section As String) As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT tbl_rooms.room_no FROM tbl_rooms INNER JOIN tbl_courses ON tbl_rooms.TypeOfRoom = tbl_courses.TypeOfRoom INNER JOIN tbl_sections ON tbl_courses.program_code = tbl_sections.program_code AND tbl_courses.yrLevel = tbl_sections.year WHERE tbl_courses.course_code = '" & course_code & "' AND tbl_sections.category = '" & section & "'"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

    Function RoomsSchedule()
        Dim dataTable As New DataTable
        Try
            sql = "SELECT CONCAT(r.building_name, ' - ', r.room_no) as room, COUNT(s.scheduleID) as total FROM tbl_rooms r LEFT JOIN tbl_schedules s ON r.room_no = s.room_no GROUP BY r.building_name, r.room_no ORDER BY total DESC;"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function


    'DELETE ROOMS BY ROOM NO'
    Public Sub DeletebyRoomNo(room_no As String)
        Try
            sql = "DELETE FROM tbl_rooms WHERE room_no = '" & room_no & "'"
            delete(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'COUNT ALL ROOMS'

    Function CountAllRooms() As String
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_rooms"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return tryCount
    End Function

    Function CountRegularRooms() As String
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_rooms WHERE TypeOfRoom = 'Regular'"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return tryCount
    End Function

    Function CountLabRooms() As String
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_rooms WHERE TypeOfRoom = 'Laboratory'"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return tryCount
    End Function
    Function CountCourtRooms() As String
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_rooms WHERE TypeOfRoom = 'Court'"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return tryCount
    End Function

    Function roomNotSchedule() As DataTable
        Dim dataTable As New DataTable

        Try
            sql = "SELECT room_no FROM tbl_rooms WHERE room_no NOT IN (SELECT DISTINCT room_no FROM tbl_schedules);"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dataTable
    End Function

End Class