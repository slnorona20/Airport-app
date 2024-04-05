/* Flight */
SET AUTOCOMMIT = 0;

CREATE TABLE Flight (
    db_id INT PRIMARY KEY,
    flight_id VARCHAR(10),
    departure_date DATE NOT NULL,
    arrival_date DATE NOT NULL,
    origin_country VARCHAR(20),
    destination_country VARCHAR(20),
    aircraft_model VARCHAR(20),
    max_seats INT,
    available_seats INT
) ENGINE = InnoDB;
ALTER TABLE Flight
ADD UNIQUE INDEX unique_flight_departure (flight_id, departure_date);
ALTER TABLE Flight
ADD UNIQUE INDEX unique_flight_arrival (flight_id, arrival_date);
ALTER TABLE Flight
MODIFY COLUMN db_id INT AUTO_INCREMENT;

DELIMITER //
CREATE PROCEDURE spAddFlight(
    IN flightid VARCHAR(10), 
    IN departureDate DATE,
    IN arrivalDate DATE,
    IN originCountry VARCHAR(20),
    IN destinationCountry VARCHAR(20),
    IN aircraftModel VARCHAR(20),
    IN maxSeats INT,
    IN availableSeats INT
)
BEGIN
    INSERT INTO Flight (flight_id, departure_date, arrival_date, origin_country, destination_country, aircraft_model, max_seats, available_seats)
    VALUES (flightid, departureDate, arrivalDate, originCountry, destinationCountry, aircraftModel, maxSeats, availableSeats);
	
	SELECT     
		db_id AS Id,
        flight_id AS FlightId,
        departure_date AS Name,
        arrival_date AS Surname,
        origin_country AS BirthDate,
        destination_country AS Nationality,
        aircraft_model AS Email,
        max_seats AS MaxSeats,
        available_seats AS AvailableSeats
	FROM Flight
    WHERE db_id = ( SELECT LAST_INSERT_ID() AS db_id );
END//
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetFlight( IN dbid INT )
BEGIN
	SELECT db_id, flight_id, departure_date, arrival_date, origin_country, destination_country, aircraft_model, max_seats, available_seats
    FROM Flight 
    WHERE db_id = dbid;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetFlights(IN departureDate Date, IN originCountry VARCHAR(20), IN destinationCountry VARCHAR(20))
BEGIN
	SELECT db_id, flight_id, departure_date, arrival_date, origin_country, destination_country, aircraft_model, max_seats, available_seats
    FROM Flight
    WHERE departure_date = departureDate AND origin_country = originCountry AND destination_country = destinationCountry;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE `spUpdateFlight`(
    IN dbid INT, 
    IN flightid VARCHAR(10), 
    IN departureDate DATE,
    IN arrivalDate DATE,
    IN originCountry VARCHAR(20),
    IN destinationCountry VARCHAR(20),
    IN aircraftCodel VARCHAR(20),
    IN maxSeats INT,
    IN availableSeats INT
)
BEGIN
	UPDATE Flight 
    SET
		flight_id = flightid, 
		departure_date = departureDate,
		arrival_date = arrivalDate,
		origin_country = originCountry,
		destination_country = destinationCountry,
		aircraft_model = aircraftModel,
		max_seats = maxSeats,
		available_seats = availableSeats
	WHERE db_id = dbid;
    
	SELECT     
		db_id AS Id,
        flight_id AS FlightId,
        departure_date AS Name,
        arrival_date AS Surname,
        origin_country AS BirthDate,
        destination_country AS Nationality,
        aircraft_model AS Email,
        max_seats AS MaxSeats,
        available_seats AS AvailableSeats
	FROM Flight
    WHERE db_id = dbid;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spDeleteFlight( IN dbid INT )
BEGIN
	DELETE FROM Flight WHERE db_id = dbid;
END //
DELIMITER 

COMMIT;