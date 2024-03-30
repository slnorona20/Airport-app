/* DROP DATABASE IF EXISTS Airport; */

// Crear la base de datos
CREATE DATABASE Airport;

//--------------------------------------------------------------
// Crear la tabla "AirportUser"
// Crear todos los "stored procedure" del model User
//---------------------------------------------------------------
SET AUTOCOMMIT = 0;

CREATE TABLE AirportUser (
    user_id INT PRIMARY KEY,
    user_name VARCHAR(50) NOT NULL,
    user_surname VARCHAR(50) NOT NULL,
    birthdate DATE,
    nationality VARCHAR(20),
    email VARCHAR(100)
) ENGINE = InnoDB;
ALTER TABLE AirportUser
ADD UNIQUE INDEX unique_email (email);s
ALTER TABLE AirportUser
MODIFY COLUMN user_id INT AUTO_INCREMENT;

DELIMITER //
CREATE PROCEDURE spAddUser(
    IN username VARCHAR(50), 
    IN usersurname VARCHAR(50),
    IN birthdate DATE,
    IN nationality VARCHAR(20),
    IN email VARCHAR(100)
)
BEGIN
    INSERT INTO AirportUser 
    (
        user_name,
        user_surname,
        birthdate,
        nationality,
        email
    )
    VALUES 
    (
        username,
        usersurname,
        birthdate,
        nationality,
        email
    );
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetUser( IN userId INT )
BEGIN
	SELECT user_id, user_name, user_surname, birthdate, nationality, email
    FROM AirportUser 
    WHERE user_id = userId;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetAllUsers()
BEGIN
	SELECT user_id, user_name, user_surname, nationality, email
    FROM AirportUser;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetUserByEmail( IN email VARCHAR(100) )
BEGIN
	SELECT user_id, user_name, user_surname, birthdate, nationality, email
    FROM AirportUser 
    WHERE email = email;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spUpdateUser(
	IN userId INT,
	IN username VARCHAR(50), 
    IN usersurname VARCHAR(50),
    IN birthday DATE,
    IN nationality VARCHAR(20),
    IN email VARCHAR(100)
)
BEGIN
	UPDATE AirportUser 
    SET
		user_name = username,
		user_surname = usersurname,
		birthdate = birthday,
		nationality = nationality,
        email = email
	WHERE user_id = userId;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spDeleteUser( IN userId INT )
BEGIN
	DELETE FROM AirportUser WHERE user_id = userId;
END //
DELIMITER 

COMMIT;

//----------------------------------------------
// Usar los "Stored procedures"
//----------------------------------------------
CALL spAddUser('Sergio', 'Noroña Pinero', '2002-01-22', 'Cubano', 'tuEmail@dominio');
CALL spGetAllUsers();
CALL spGetUser(1);
CALL spGetUserByEmail('tuEmail@dominio');
CALL spUpdateUser(1, 'Sergio Plutarco', 'Noroña Pinero', '2002-01-22', 'Cubano', 'tuEmail@dominio');
CALL spDeleteUser(1)