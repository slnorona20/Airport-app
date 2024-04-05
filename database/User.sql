/* DROP DATABASE IF EXISTS Airport; */

/* Crear la base de datos */
CREATE DATABASE Airport;

/*/--------------------------------------------------------------
// Crear la tabla "AirportUser"
// Crear todos los "stored procedure" del model User
//---------------------------------------------------------------*/
SET AUTOCOMMIT = 0;

CREATE TABLE AirportUser (
    user_id INT PRIMARY KEY,
    user_name VARCHAR(50),
    user_surname VARCHAR(50),
    birth_date DATE,
    nationality VARCHAR(20),
    email VARCHAR(100)
) ENGINE = InnoDB;
ALTER TABLE AirportUser
ADD UNIQUE INDEX unique_email (email);
ALTER TABLE AirportUser
MODIFY COLUMN user_id INT AUTO_INCREMENT;

DELIMITER //
CREATE PROCEDURE spAddUser(
    IN userName VARCHAR(50), 
    IN userSurname VARCHAR(50),
    IN birthDate DATE,
    IN nationality VARCHAR(20),
    IN email VARCHAR(100)
)
BEGIN
    INSERT INTO AirportUser  ( user_name, user_surname, birth_date, nationality, email )
    VALUES ( userName, userSurname, birthDate, nationality, email );
    
    SELECT     
		user_id AS Id,
        user_name AS Name,
        user_surname AS Surname,
        birth_date AS BirthDate,
        nationality AS Nationality,
        email AS Email
	FROM AirportUser
    WHERE user_id = ( SELECT LAST_INSERT_ID() AS user_id );
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetUser( IN userId INT )
BEGIN
	SELECT user_id, user_name, user_surname, birth_date, nationality, email
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
CREATE PROCEDURE spUpdateUser(
	IN userId INT,
	IN userName VARCHAR(50), 
    IN userSurname VARCHAR(50),
    IN birthDate DATE,
    IN nationality VARCHAR(20),
    IN email VARCHAR(100)
)
BEGIN
	UPDATE AirportUser 
    SET
		user_name = userName,
		user_surname = usersurName,
		birth_date = birthDate,
		nationality = nationality,
        email = email
	WHERE user_id = userId;
    
	SELECT     
		user_id AS Id,
        user_name AS Name,
        user_surname AS Surname,
        birth_date AS BirthDate,
        nationality AS Nationality,
        email AS Email
	FROM AirportUser
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
