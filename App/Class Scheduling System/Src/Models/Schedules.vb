Imports System.Globalization
Imports System.Text
Imports Org.BouncyCastle.Asn1.Cms

Public Class Schedules
    Private sql As String

    'ADD SCHEDULE'
    Public Sub AddSchedule(room_no As String, course_no As String, instructor_no As String, section As String, classList As List(Of String), start_time As DateTime, end_time As DateTime)
        Dim classDay As String = String.Join(",", classList)
        Try
            Dim startTime As String = start_time.ToString("HH:mm:ss")
            Dim endTime As String = end_time.ToString("HH:mm:ss")
            sql = "INSERT INTO tbl_schedules (room_no, course_code, instructor_no, section, class_day, start_time, end_time) VALUES('" & room_no & "', '" & course_no & "', '" & instructor_no & "', '" & section & "', '" & classDay & "', '" & startTime & "', '" & endTime & "')"
            create(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'UPDATE SCHEDULE'
    Public Sub UpdateSchedule(room_no As String, course_no As String, instructor_no As String, section As String, classList As List(Of String), start_time As DateTime, end_time As DateTime, scheduleID As Integer)
        Dim classDay As String = String.Join(",", classList)
        Try
            Dim startTime As String = start_time.ToString("HH:mm")
            Dim endTime As String = end_time.ToString("HH:mm")
            sql = "UPDATE tbl_schedules SET room_no = '" & room_no & "',  course_code = '" & course_no & "',  instructor_no = '" & instructor_no & "',  section = '" & section & "',  class_day = '" & classDay & "',  start_time = '" & startTime & "',  end_time = '" & endTime & "' WHERE scheduleID = '" & scheduleID & "'"
            update(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub

    'READ ALL SCHEDULE'
    Public Function ReadAllSchedules() As DataTable
        Dim dt As New DataTable
        Try
            sql = "SELECT scheduleID, room_no, course_code, instructor_no, section, class_day, TIME_FORMAT(start_time, '%h:%i %p') AS start_time, TIME_FORMAT(end_time, '%h:%i %p') AS end_time FROM tbl_schedules;"
            dt = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return dt
    End Function

    Function CountSchedules()
        Dim totalCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_schedules"
            totalCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try

        Return totalCount
    End Function


    'DELETE SCHEDLE
    Sub DeleteScheduleBySelected(scheduleID As Integer)
        Try
            sql = "DELETE FROM tbl_schedules WHERE scheduleID = '" & scheduleID & "'"
            delete(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
    End Sub


    'CHECK IF THE ROOM IS AVAILABLE'
    Public Function IsRoomAvailable(room_no As String, classday As String, start_time As DateTime, end_time As DateTime) As Boolean
        Dim isAvailable As Boolean = True
        Try
            sql = "Select * FROM tbl_schedules WHERE room_no = '" & room_no & "' AND class_day = '" & classday & "' AND ((start_time <= '" & start_time.ToString("yyyy-MM-dd HH:mm:ss") & "' AND end_time >= '" & start_time.ToString("yyyy-MM-dd HH:mm:ss") & "') OR (start_time <= '" & end_time.ToString("yyyy-MM-dd HH:mm:ss") & "' AND end_time >= '" & end_time.ToString("yyyy-MM-dd HH:mm:ss") & "'))"
            Dim dt As DataTable = read(sql)
            If dt.Rows.Count > 0 Then
                isAvailable = False
            End If
        Catch ex As Exception
            Console.WriteLine(ex.ToString)
        End Try
        Return isAvailable
    End Function
    Function IsConflict(room_no As String, classList As List(Of String), start_time As String, end_time As String, oldCourse As String, oldSection As String, oldRoom As String) As Boolean
        Dim conflict_found As Boolean = False
        Dim classDayCondition As New List(Of String)()

        For Each day In classList
            classDayCondition.Add("class_day LIKE '%" & day & "%'")
        Next

        Dim classDayConditionStr As String = String.Join(" OR ", classDayCondition)

        Try
            Dim sql As String = "SELECT COUNT(*) FROM tbl_schedules " &
                            "WHERE room_no = '" & room_no & "' " &
                            "AND (" & classDayConditionStr & ") " &
                            "AND ('" & start_time & "' < end_time AND '" & end_time & "' > start_time) " &
                            "AND NOT (course_code = '" & oldCourse & "' AND section = '" & oldSection & "' AND room_no = '" & oldRoom & "')"

            Dim counts As Integer = count(sql)
            conflict_found = counts > 0
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return conflict_found
    End Function

    Function IsOverlap(room_no As String, section As String, course_code As String, start_time As String, end_time As String, classList As List(Of String), oldCourse As String, oldSection As String, oldRoom As String) As Boolean
        Dim overlap_found As Boolean = False
        Dim classDayCondition As New List(Of String)()

        For Each day In classList
            classDayCondition.Add("class_day = '" & day & "' AND '" & start_time & "' < end_time AND '" & end_time & "' > start_time")
        Next

        Dim classDayConditionStr As String = String.Join(" OR ", classDayCondition)

        Try
            Dim sql As String = "SELECT COUNT(*) FROM tbl_schedules " &
                            "WHERE (room_no = '" & room_no & "' OR section = '" & section & "' OR course_code = '" & course_code & "') " &
                            "AND (" & classDayConditionStr & ") " &
                            "AND NOT (course_code = '" & oldCourse & "' AND section = '" & oldSection & "' AND room_no = '" & oldRoom & "')"

            Dim counts As Integer = count(sql)
            overlap_found = counts > 0
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return overlap_found
    End Function

    Function OverlapSched(room_no As String, instructor_no As String, section As String, classList As List(Of String), start_time As String, end_time As String, scheduleID As Integer)
        Dim result As Boolean = False
        Dim classDay As String = String.Join(",", classList)
        Try
            sql = "SELECT COUNT(*) FROM tbl_schedules WHERE 
                   ((room_no = '" & room_no & "' AND class_day LIKE '%" & classDay & "%' AND start_time = '" & start_time & "' AND end_time = '" & end_time & "') OR
                   (instructor_no = '" & instructor_no & "' AND class_day LIKE '%" & classDay & "%' AND start_time = '" & start_time & "' AND end_time = '" & end_time & "') OR
                   (section = '" & section & "' AND class_day LIKE '%" & classDay & "%' AND start_time = '" & start_time & "' AND end_time = '" & end_time & "')) AND scheduleID != '" & scheduleID & "'"
            result = count(sql) > 0
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        Return result
    End Function

    Function ConflictSched(room_no As String, instructor_no As String, section As String, classList As List(Of String), start_time As String, end_time As String, scheduleID As Integer)
        Dim result As Boolean = False
        Dim classDay As String = String.Join(",", classList)
        Try
            sql = "SELECT COUNT(*) FROM tbl_schedules WHERE 
                   ((instructor_no = '" & instructor_no & "'  AND class_day LIKE '%" & classDay & "%' AND '" & start_time & "' < end_time AND '" & end_time & "' > start_time) OR
                   (section = '" & section & "' AND class_day LIKE '%" & classDay & "%' AND '" & start_time & "' < end_time AND '" & end_time & "' > start_time) OR
                   (room_no = '" & room_no & "' AND class_day LIKE '%" & classDay & "%' AND '" & start_time & "' < end_time AND '" & end_time & "' > start_time)) AND scheduleID != '" & scheduleID & "'"
            result = count(sql) > 0
        Catch ex As Exception
            Console.WriteLine(ex.Message)
        End Try

        Return result
    End Function

    Function isCountConflict(room_no As String, section As String, instructor_no As String, classList As List(Of String), start_time As String, end_time As String) As Integer

        Dim result As Boolean = False
        Dim classDay As String = String.Join(",", classList)

        Try
            sql = "SELECT COUNT(*) FROM tbl_schedules WHERE (room_no = '" & room_no & "' OR section = '" & section & "' OR instructor_no = '" & instructor_no & "' ) AND (class_day LIKE '%" & classDay & "%' AND NOT (start_time => '" & end_time & "' OR end_time <= '" & start_time & "'))"
            result = count(sql) > 0
        Catch ex As Exception
            Console.WriteLine(ex.Message)

        End Try

        Return result

    End Function

    Function WeeklySchedule()
        Dim dataTable As New DataTable

        Try
            sql = "SELECT 
                  d.day,
                  COUNT(s.scheduleID) as Total
                FROM 
                  (SELECT 'Sun' as day UNION ALL SELECT 'Mon' UNION ALL SELECT 'Tue' UNION ALL SELECT 'Wed' UNION ALL SELECT 'Thu' UNION ALL SELECT 'Fri' UNION ALL SELECT 'Sat') d
                  LEFT JOIN tbl_schedules s ON FIND_IN_SET(d.day, s.class_day)
                GROUP BY 
                  d.day
                ORDER BY 
                  FIELD(d.day, 'Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat');"

            dataTable = read(sql)

        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return dataTable
    End Function



    Function CountAllScheduleByDay() As Integer
        Dim totalCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_schedules WHERE FIND_IN_SET(DATE_FORMAT(NOW(), '%a'), class_day) > 0;"

            totalCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return totalCount
    End Function

    Function CountUpcomingScheduleByDay() As Integer
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_schedules WHERE FIND_IN_SET(DATE_FORMAT(NOW(), '%a'), class_day) > 0 AND start_time > CURTIME();"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return tryCount
    End Function

    Function CountOnGoingScheduleByDay() As Integer
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) FROM tbl_schedules WHERE FIND_IN_SET(DATE_FORMAT(NOW(), '%a'), class_day) > 0 AND TIME(NOW()) BETWEEN start_time AND end_time;"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return tryCount
    End Function

    Function CountFinishedScheduleByDay() As Integer
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) AS total_finished_schedules_today FROM tbl_schedules WHERE FIND_IN_SET(DATE_FORMAT(NOW(), '%a'), class_day) > 0 AND TIME(NOW()) > end_time;"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return tryCount
    End Function

    Function CountTime(currentTime As String) As Integer
        Dim tryCount As Integer = 0
        Try
            sql = "SELECT COUNT(*) AS match_count FROM tbl_schedules WHERE class_day LIKE CONCAT('%', DATE_FORMAT(NOW(), '%a'), '%') AND (DATE_FORMAT(start_time, '%H:%i:%s') = '" & currentTime & "' OR DATE_FORMAT(end_time, '%H:%i:%s') = '" & currentTime & "');"
            tryCount = count(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return tryCount
    End Function

    Function ReadAllStartTimeSchedule() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT start_time, end_time FROM tbl_schedules WHERE class_day LIKE CONCAT('%', DATE_FORMAT(NOW(), '%a'), '%') ORDER BY ABS(TIMESTAMPDIFF(MINUTE, NOW(), start_time)), ABS(TIMESTAMPDIFF(MINUTE, NOW(), end_time));"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return dataTable
    End Function

    Function ReadAllOngoingSchedule() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT course_code, section, class_day, TIME_FORMAT(start_time, '%h:%i %p') AS start_time, TIME_FORMAT(end_time, '%h:%i %p') AS end_time, CONCAT(tbl_rooms.room_no, ' - ', tbl_rooms.building_name) AS room, CONCAT(tbl_instructors.firstname, ' ', tbl_instructors.lastname) AS instructor_name FROM tbl_schedules JOIN tbl_rooms ON tbl_schedules.room_no = tbl_rooms.room_no JOIN tbl_instructors ON tbl_schedules.instructor_no = tbl_instructors.instructor_no WHERE FIND_IN_SET(DATE_FORMAT(NOW(), '%a'), tbl_schedules.class_day) > 0 AND CURTIME() BETWEEN start_time AND end_time;"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return dataTable
    End Function

    Function ReadAllNextSchedule() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT course_code, section, class_day, TIME_FORMAT(start_time, '%h:%i %p') AS start_time, TIME_FORMAT(end_time, '%h:%i %p') AS end_time, CONCAT(tbl_rooms.room_no, ' - ', tbl_rooms.building_name) AS room, CONCAT(tbl_instructors.firstname, ' ', tbl_instructors.lastname) AS instructor_name FROM tbl_schedules JOIN tbl_rooms ON tbl_schedules.room_no = tbl_rooms.room_no JOIN tbl_instructors ON tbl_schedules.instructor_no = tbl_instructors.instructor_no WHERE FIND_IN_SET(DATE_FORMAT(NOW(), '%a'), tbl_schedules.class_day) > 0 AND start_time > CURTIME() ORDER BY start_time;"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return dataTable
    End Function

    Function ReadAllFinishSchedule() As DataTable
        Dim dataTable As New DataTable
        Try
            sql = "SELECT course_code, section, class_day, TIME_FORMAT(start_time, '%h:%i %p') AS start_time, TIME_FORMAT(end_time, '%h:%i %p') AS end_time, CONCAT(tbl_rooms.room_no, ' - ', tbl_rooms.building_name) AS room, CONCAT(tbl_instructors.firstname, ' ', tbl_instructors.lastname) AS instructor_name FROM tbl_schedules JOIN tbl_rooms ON tbl_schedules.room_no = tbl_rooms.room_no JOIN tbl_instructors ON tbl_schedules.instructor_no = tbl_instructors.instructor_no WHERE FIND_IN_SET(DATE_FORMAT(NOW(), '%a'), tbl_schedules.class_day) > 0 AND end_time < CURTIME() ORDER BY end_time;"
            dataTable = read(sql)
        Catch ex As Exception
            Console.WriteLine(ex.ToString())
        End Try

        Return dataTable
    End Function

End Class
