IF OBJECT_ID('dbo.Transport', 'U') IS NOT NULL
    DROP TABLE dbo.Transport;
GO
IF OBJECT_ID('dbo.CP', 'U') IS NOT NULL
    DROP TABLE dbo.CP;
GO
IF OBJECT_ID('dbo.Employee', 'U') IS NOT NULL
    DROP TABLE dbo.Employee;
GO
IF OBJECT_ID('dbo.City', 'U') IS NOT NULL
    DROP TABLE dbo.City;
GO
IF OBJECT_ID('dbo.Vehicle', 'U') IS NOT NULL
    DROP TABLE dbo.Vehicle;
GO

CREATE TABLE dbo.Employee (
    id NVARCHAR(10) PRIMARY KEY,
    firstName NVARCHAR(50) NOT NULL,
    lastName NVARCHAR(50) NOT NULL,
    birthNumber CHAR(11) NOT NULL,
    birthDay DATE NOT NULL,
);
GO

CREATE TABLE dbo.City (
    id INT PRIMARY KEY,
    latitude DECIMAL(9,6) NOT NULL,     --Height
    longitude DECIMAL(9,6) NOT NULL,    --Width
    cityName NVARCHAR(100) NOT NULL,
    stateName NVARCHAR(50) NOT NULL  
);
GO


CREATE TABLE dbo.Vehicle (
    id INT IDENTITY(1,1) PRIMARY KEY,
    vehicleName NVARCHAR(100) NOT NULL,
);
GO

CREATE TABLE dbo.CP (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_employee NVARCHAR(10) NOT NULL,
    id_startCity INT NOT NULL,
    id_endCity INT NOT NULL,
    creationDate DATE NOT NULL,
    startTime DATETIMEOFFSET NOT NULL,
    endTime DATETIMEOFFSET NOT NULL,
    cpState VARCHAR(100) NOT NULL
);
GO

CREATE TABLE dbo.Transport (
    id INT IDENTITY(1,1) PRIMARY KEY,
    id_cp INT NOT NULL,
    id_vehicle INT NOT NULL
);
GO

--CP
ALTER TABLE dbo.CP ADD CONSTRAINT FK_emp FOREIGN KEY (id_employee) REFERENCES dbo.Employee (id) ON DELETE CASCADE;
GO
ALTER TABLE dbo.CP ADD CONSTRAINT FK_startCity FOREIGN KEY (id_startCity) REFERENCES dbo.City (id) ON DELETE CASCADE;
GO
ALTER TABLE dbo.CP ADD CONSTRAINT FK_endCity FOREIGN KEY (id_endCity) REFERENCES dbo.City (id) ON DELETE NO ACTION;
GO

ALTER TABLE dbo.CP
ADD CONSTRAINT CK_CP_cpState
CHECK (cpState IN ('Vytvorený', 'Schválený', 'Vyúètovaný', 'Zrušený'));

--Transport 
ALTER TABLE dbo.Transport ADD CONSTRAINT FK_cp FOREIGN KEY (id_cp) REFERENCES dbo.CP (id) ON DELETE CASCADE;
GO
ALTER TABLE dbo.Transport ADD CONSTRAINT FK_veh FOREIGN KEY (id_vehicle) REFERENCES dbo.Vehicle (id) ON DELETE CASCADE;
GO


INSERT INTO dbo.Employee (id, firstName, lastName, birthNumber, birthDay)
VALUES
('EMP001', 'Ján', 'Novák', '851010/1234', '1985-10-10'),
('EMP002', 'Eva', 'Svobodová', '561010/1234', '1990-05-20'),
('EMP003', 'Peter', 'Kováè', '780101/5678', '1978-01-01');
GO

INSERT INTO dbo.City (id, longitude, latitude, cityName, stateName)
VALUES
(1, 48, 17, 'Bratislava', 'Slovensko'),
(2, 50, 14, 'Praha', 'Èesko'),
(3, 48, 21, 'Košice', 'Slovensko'),
(4, 49, 18, 'Brno', 'Èesko');
GO

INSERT INTO dbo.Vehicle (vehicleName)
VALUES
('Škoda Octavia'),
('VW Passat'),
('Ford Transit'),
('Fiat 500');
GO

INSERT INTO dbo.CP (id_employee, id_startCity, id_endCity, creationDate, startTime, endTime, cpState)
VALUES
('EMP001', 1, 2, '2023-10-25', '2023-11-01 08:00:00 +01:00', '2023-11-01 12:00:00 +01:00', 'Vyúètovaný'),
('EMP002', 2, 3, '2023-10-26', '2023-11-02 09:00:00 +01:00', '2023-11-02 14:00:00 +01:00', 'Schválený'),
('EMP003', 1, 4, '2023-10-27', '2023-11-03 10:00:00 +01:00', '2023-11-02 15:00:00 +01:00', 'Vytvorený')

GO

INSERT INTO dbo.Transport (id_cp, id_vehicle)
VALUES
(1, 1), -- CP 1 (BA->PRG) s Vozidlom 1 (Škoda Octavia)
(2, 2), -- CP 2 (PRG->KOS) s Vozidlom 2 (VW Passat)
(2, 3),
(3, 3); -- CP 2 (PRG->KOS) s Vozidlom 2 (VW Passat)
GO
