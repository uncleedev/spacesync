-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: Apr 21, 2024 at 12:58 PM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.1.25

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `scheduling_schema`
--

-- --------------------------------------------------------

--
-- Table structure for table `tbl_courses`
--

CREATE TABLE `tbl_courses` (
  `ID` int(11) NOT NULL,
  `course_code` varchar(50) DEFAULT NULL,
  `description` varchar(50) DEFAULT NULL,
  `credits` int(11) DEFAULT NULL,
  `program_code` varchar(50) DEFAULT NULL,
  `yrLevel` enum('1','2','3','4','5','6','7') DEFAULT NULL,
  `TypeOfRoom` enum('Regular','Laboratory','Court') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_curriculums`
--

CREATE TABLE `tbl_curriculums` (
  `ID` int(11) NOT NULL,
  `course_code` varchar(50) DEFAULT NULL,
  `program_code` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_enrollments`
--

CREATE TABLE `tbl_enrollments` (
  `ID` int(11) NOT NULL,
  `students_no` varchar(50) DEFAULT NULL,
  `course_code` varchar(50) DEFAULT NULL,
  `section` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_instructors`
--

CREATE TABLE `tbl_instructors` (
  `ID` int(11) NOT NULL,
  `instructor_no` varchar(50) DEFAULT NULL,
  `firstname` varchar(100) DEFAULT NULL,
  `lastname` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `gender` enum('Male','Female') DEFAULT NULL,
  `employee_status` enum('Part Time','Full Time') DEFAULT NULL,
  `availability` set('Mon','Tue','Wed','Thu','Fri','Sat','Sun') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_programs`
--

CREATE TABLE `tbl_programs` (
  `ID` int(11) NOT NULL,
  `program_code` varchar(50) DEFAULT NULL,
  `name` varchar(255) DEFAULT NULL,
  `department` varchar(100) DEFAULT NULL,
  `NoOfYear` enum('1','2','3','4','5','6') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Triggers `tbl_programs`
--
DELIMITER $$
CREATE TRIGGER `update_category_trigger` AFTER UPDATE ON `tbl_programs` FOR EACH ROW BEGIN
    UPDATE tbl_sections
    SET category = CONCAT(NEW.program_code, ' ', tbl_sections.year, tbl_sections.name)
    WHERE tbl_sections.program_code = OLD.program_code;
END
$$
DELIMITER ;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_rooms`
--

CREATE TABLE `tbl_rooms` (
  `ID` int(11) NOT NULL,
  `room_no` varchar(50) DEFAULT NULL,
  `building_name` varchar(100) DEFAULT NULL,
  `floor_level` varchar(11) DEFAULT NULL,
  `TypeOfRoom` enum('Regular','Laboratory','Court') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_schedules`
--

CREATE TABLE `tbl_schedules` (
  `ID` int(11) NOT NULL,
  `room_no` varchar(50) DEFAULT NULL,
  `course_code` varchar(50) DEFAULT NULL,
  `instructor_no` varchar(50) DEFAULT NULL,
  `section` varchar(50) DEFAULT NULL,
  `class_day` set('Mon','Tue','Wed','Thurs','Fri','Sat','Sun') DEFAULT NULL,
  `start_time` time DEFAULT NULL,
  `end_time` time DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_schools`
--

CREATE TABLE `tbl_schools` (
  `ID` int(11) NOT NULL,
  `school_id` varchar(100) DEFAULT NULL,
  `school_name` varchar(100) DEFAULT NULL,
  `address` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_sections`
--

CREATE TABLE `tbl_sections` (
  `ID` int(11) NOT NULL,
  `category` varchar(50) DEFAULT NULL,
  `program_code` varchar(50) DEFAULT NULL,
  `year` int(11) DEFAULT NULL,
  `name` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_sessions`
--

CREATE TABLE `tbl_sessions` (
  `ID` int(11) NOT NULL,
  `instructor_no` varchar(50) DEFAULT NULL,
  `section` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_students`
--

CREATE TABLE `tbl_students` (
  `ID` int(11) NOT NULL,
  `students_no` varchar(50) DEFAULT NULL,
  `firstname` varchar(100) DEFAULT NULL,
  `lastname` varchar(100) DEFAULT NULL,
  `email` varchar(100) DEFAULT NULL,
  `gender` enum('Male','Female') DEFAULT NULL,
  `student_status` enum('Regular','Irregular') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_subjects`
--

CREATE TABLE `tbl_subjects` (
  `ID` int(11) NOT NULL,
  `instructor_no` varchar(50) DEFAULT NULL,
  `course_code` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

-- --------------------------------------------------------

--
-- Table structure for table `tbl_users`
--

CREATE TABLE `tbl_users` (
  `ID` int(11) NOT NULL,
  `username` varchar(50) DEFAULT NULL,
  `password` varchar(50) DEFAULT NULL,
  `role` enum('Admin','Editor','Viewer') DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Indexes for dumped tables
--

--
-- Indexes for table `tbl_courses`
--
ALTER TABLE `tbl_courses`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `unique_courseProg` (`course_code`,`program_code`) USING BTREE,
  ADD KEY `course_code` (`course_code`),
  ADD KEY `tbl_courses_ibfk_1` (`program_code`);

--
-- Indexes for table `tbl_curriculums`
--
ALTER TABLE `tbl_curriculums`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `tbl_curriculums_ibfk_1` (`course_code`),
  ADD KEY `tbl_curriculums_ibfk_2` (`program_code`);

--
-- Indexes for table `tbl_enrollments`
--
ALTER TABLE `tbl_enrollments`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `tbl_enrollments_ibfk_1` (`students_no`),
  ADD KEY `tbl_enrollments_ibfk_2` (`course_code`),
  ADD KEY `tbl_enrollments_ibfk_3` (`section`);

--
-- Indexes for table `tbl_instructors`
--
ALTER TABLE `tbl_instructors`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `instructor_no` (`instructor_no`),
  ADD UNIQUE KEY `unique_name` (`firstname`,`lastname`);

--
-- Indexes for table `tbl_programs`
--
ALTER TABLE `tbl_programs`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `program_code` (`program_code`);

--
-- Indexes for table `tbl_rooms`
--
ALTER TABLE `tbl_rooms`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `room_no` (`room_no`);

--
-- Indexes for table `tbl_schedules`
--
ALTER TABLE `tbl_schedules`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `tbl_schedules_ibfk_1` (`room_no`),
  ADD KEY `tbl_schedules_ibfk_2` (`course_code`),
  ADD KEY `tbl_schedules_ibfk_4` (`section`),
  ADD KEY `instructor_no` (`instructor_no`);

--
-- Indexes for table `tbl_schools`
--
ALTER TABLE `tbl_schools`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `school_id` (`school_id`);

--
-- Indexes for table `tbl_sections`
--
ALTER TABLE `tbl_sections`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `category` (`category`),
  ADD KEY `tbl_sections_ibfk_1` (`program_code`);

--
-- Indexes for table `tbl_sessions`
--
ALTER TABLE `tbl_sessions`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `tbl_sessions_ibfk_1` (`instructor_no`),
  ADD KEY `tbl_sessions_ibfk_2` (`section`);

--
-- Indexes for table `tbl_students`
--
ALTER TABLE `tbl_students`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `students_no` (`students_no`);

--
-- Indexes for table `tbl_subjects`
--
ALTER TABLE `tbl_subjects`
  ADD PRIMARY KEY (`ID`),
  ADD KEY `tbl_subjects_ibfk_1` (`instructor_no`),
  ADD KEY `tbl_subjects_ibfk_2` (`course_code`);

--
-- Indexes for table `tbl_users`
--
ALTER TABLE `tbl_users`
  ADD PRIMARY KEY (`ID`),
  ADD UNIQUE KEY `username` (`username`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `tbl_courses`
--
ALTER TABLE `tbl_courses`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_curriculums`
--
ALTER TABLE `tbl_curriculums`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_enrollments`
--
ALTER TABLE `tbl_enrollments`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_instructors`
--
ALTER TABLE `tbl_instructors`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_programs`
--
ALTER TABLE `tbl_programs`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_rooms`
--
ALTER TABLE `tbl_rooms`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_schedules`
--
ALTER TABLE `tbl_schedules`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_schools`
--
ALTER TABLE `tbl_schools`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_sections`
--
ALTER TABLE `tbl_sections`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_sessions`
--
ALTER TABLE `tbl_sessions`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_students`
--
ALTER TABLE `tbl_students`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_subjects`
--
ALTER TABLE `tbl_subjects`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- AUTO_INCREMENT for table `tbl_users`
--
ALTER TABLE `tbl_users`
  MODIFY `ID` int(11) NOT NULL AUTO_INCREMENT;

--
-- Constraints for dumped tables
--

--
-- Constraints for table `tbl_courses`
--
ALTER TABLE `tbl_courses`
  ADD CONSTRAINT `tbl_courses_ibfk_1` FOREIGN KEY (`program_code`) REFERENCES `tbl_programs` (`program_code`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_curriculums`
--
ALTER TABLE `tbl_curriculums`
  ADD CONSTRAINT `tbl_curriculums_ibfk_1` FOREIGN KEY (`course_code`) REFERENCES `tbl_courses` (`course_code`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_curriculums_ibfk_2` FOREIGN KEY (`program_code`) REFERENCES `tbl_programs` (`program_code`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_enrollments`
--
ALTER TABLE `tbl_enrollments`
  ADD CONSTRAINT `tbl_enrollments_ibfk_1` FOREIGN KEY (`students_no`) REFERENCES `tbl_students` (`students_no`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_enrollments_ibfk_2` FOREIGN KEY (`course_code`) REFERENCES `tbl_courses` (`course_code`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_enrollments_ibfk_3` FOREIGN KEY (`section`) REFERENCES `tbl_sections` (`category`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_schedules`
--
ALTER TABLE `tbl_schedules`
  ADD CONSTRAINT `tbl_schedules_ibfk_1` FOREIGN KEY (`room_no`) REFERENCES `tbl_rooms` (`room_no`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_schedules_ibfk_2` FOREIGN KEY (`course_code`) REFERENCES `tbl_courses` (`course_code`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_schedules_ibfk_4` FOREIGN KEY (`section`) REFERENCES `tbl_sections` (`category`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_schedules_ibfk_5` FOREIGN KEY (`instructor_no`) REFERENCES `tbl_instructors` (`instructor_no`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_sections`
--
ALTER TABLE `tbl_sections`
  ADD CONSTRAINT `tbl_sections_ibfk_1` FOREIGN KEY (`program_code`) REFERENCES `tbl_programs` (`program_code`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_sessions`
--
ALTER TABLE `tbl_sessions`
  ADD CONSTRAINT `tbl_sessions_ibfk_1` FOREIGN KEY (`instructor_no`) REFERENCES `tbl_instructors` (`instructor_no`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_sessions_ibfk_2` FOREIGN KEY (`section`) REFERENCES `tbl_sections` (`category`) ON DELETE CASCADE ON UPDATE CASCADE;

--
-- Constraints for table `tbl_subjects`
--
ALTER TABLE `tbl_subjects`
  ADD CONSTRAINT `tbl_subjects_ibfk_1` FOREIGN KEY (`instructor_no`) REFERENCES `tbl_instructors` (`instructor_no`) ON DELETE CASCADE ON UPDATE CASCADE,
  ADD CONSTRAINT `tbl_subjects_ibfk_2` FOREIGN KEY (`course_code`) REFERENCES `tbl_courses` (`course_code`) ON DELETE CASCADE ON UPDATE CASCADE;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
