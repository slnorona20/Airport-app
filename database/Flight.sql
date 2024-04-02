/* Flight */
SET AUTOCOMMIT = 0;

CREATE TABLE Flight (
    db_id INT PRIMARY KEY,
    flight_id VARCHAR(10),
    depart_date DATE NOT NULL,
    arrival_date DATE NOT NULL,
    origin_country VARCHAR(20),
    destination_country VARCHAR(20),
    aircraft_model VARCHAR(20),
    max_seats INT,
    available_seats INT
) ENGINE = InnoDB;
ALTER TABLE Flight
ADD UNIQUE INDEX unique_flight_depart (flight_id, depart_date);
ALTER TABLE Flight
ADD UNIQUE INDEX unique_flight_arrival (flight_id, arrival_date);
ALTER TABLE Flight
MODIFY COLUMN db_id INT AUTO_INCREMENT;

DELIMITER //
CREATE PROCEDURE spAddFlight(
    IN flightid VARCHAR(10), 
    IN departdate DATE,
    IN arrivaldate DATE,
    IN origincountry VARCHAR(20),
    IN destinationcountry VARCHAR(20),
    IN aircraftmodel VARCHAR(20),
    IN maxseats INT,
    IN availableseats INT
)
BEGIN
    INSERT INTO Flight (flight_id, depart_date, arrival_date, origin_country, destination_country, aircraft_model, max_seats, available_seats)
    VALUES (flightid, departdate, arrivaldate, origincountry, destinationcountry, aircraftmodel, maxseats, availableseats);
	
	SELECT     
		db_id AS Id,
        flight_id AS FlightId,
        depart_date AS Name,
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
	SELECT db_id, flight_id, depart_date, arrival_date, origin_country, destination_country, aircraft_model, max_seats, available_seats
    FROM Flight 
    WHERE db_id = dbid;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetAllFlights()
BEGIN
	SELECT db_id, flight_id, depart_date, arrival_date, origin_country, destination_country, aircraft_model, max_seats, available_seats
    FROM Flight;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spGetFlightByFlightId( IN email VARCHAR(100) )
BEGIN
	SELECT db_id, flight_id, depart_date, arrival_date, origin_country, destination_country, air_craft_model, max_seats, available_seats
    FROM Flight 
    WHERE flight_id = email;
END //
DELIMITER ;

DELIMITER //
CREATE PROCEDURE spUpdateFlight(
    IN dbid INT, 
    IN flightid VARCHAR(10), 
    IN departdate DATE,
    IN arrivedate DATE,
    IN origincountry VARCHAR(20),
    IN destinationcountry VARCHAR(20),
    IN aircraftmodel VARCHAR(20),
    IN maxseats INT,
    IN availableseats INT
)
BEGIN
	UPDATE Flight 
    SET
		flight_id = flightid, 
		depart_date = departdate,
		arrive_date = arrivedate,
		origin_country = origincountry,
		destination_country = destinationcountry,
		aircraft_model = aircraftmodel,
		max_seats = maxseats,
		available_seats = availableseats
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