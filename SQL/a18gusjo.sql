drop database a18gusjo;
create database a18gusjo;
use a18gusjo;
SET SQL_SAFE_UPDATES = 0;
CREATE TABLE Region (
    nr CHAR(6),
    namn VARCHAR(30) NOT NULL UNIQUE,
    land VARCHAR(20),
    klimat VARCHAR(20),
	CHECK (nr RLIKE '[0-9][0-9][0-9][0-9][0-9][0-9]'),
    PRIMARY KEY (nr)
)  ENGINE INNODB;

CREATE INDEX RegionASC ON Region(namn ASC) USING BTREE;

CREATE TABLE Renspann (
    namn VARCHAR(30) NOT NULL,
    kapacitet TINYINT unsigned,
    fylldKapacitet TINYINT default 0,
    PRIMARY KEY (namn)
)  ENGINE INNODB;

CREATE TABLE Stank (
    namn VARCHAR(20),
    nivå TINYINT UNSIGNED,
    PRIMARY KEY (nivå),
    CHECK (nivå <= 5 AND nivå > 0)
)  ENGINE INNODB;

CREATE TABLE Underart (
    namn VARCHAR(20),
    artindex TINYINT UNSIGNED,
    PRIMARY KEY (artindex),
    CHECK (artindex <= 6 AND artindex > 0)
)  ENGINE INNODB;

CREATE TABLE Ren (
    nr CHAR(11) NOT NULL UNIQUE,
    klan VARCHAR(30) NOT NULL,
    underart VARCHAR(30) NOT NULL,
    stank TINYINT UNSIGNED,
    renspann_namn VARCHAR(30),
    tjänststatus VARCHAR (11) default 'itjänst',
    CHECK (nr RLIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CHECK (stank <= 5 AND stank > 0),
     CHECK (tjänststatus RLIKE 'itjänst' OR tjänststatus RLIKE 'pensionerad'),
    PRIMARY KEY (nr),
    
    FOREIGN KEY (stank)
        REFERENCES Stank (nivå),
          FOREIGN KEY (renspann_namn)
        REFERENCES Renspann (namn)
)  ENGINE INNODB;

CREATE TABLE TidigareKollega (
    ren_ett CHAR(11) NOT NULL,
    ren_två CHAR(11) NOT NULL,
    PRIMARY KEY (ren_ett , ren_två),
    FOREIGN KEY (ren_ett)
        REFERENCES Ren (nr),
    FOREIGN KEY (ren_två)
        REFERENCES Ren (nr)
)  ENGINE INNODB;

CREATE TABLE TjänsteRen (
    ren_nr CHAR(11) DEFAULT '' NOT NULL,
    lon VARCHAR(30),
    PRIMARY KEY (ren_nr),
    FOREIGN KEY (ren_nr)
        REFERENCES Ren (nr)
)  ENGINE INNODB;

CREATE TABLE PensioneradRen (
	ren_nummer CHAR(11) DEFAULT ''NOT NULL,
    polsaburknr VARCHAR(30),
    fabriknamn VARCHAR(30),
    smak VARCHAR(30),
    PRIMARY KEY (ren_nummer),
    FOREIGN KEY (ren_nummer)
        REFERENCES Ren (nr)
)  ENGINE INNODB;



CREATE TABLE TjänstgjordRegion (
    ren_nr CHAR(11) NOT NULL,
    region_nr CHAR(6) NOT NULL,
    PRIMARY KEY (ren_nr, region_nr),
    FOREIGN KEY (ren_nr)
        REFERENCES Ren (nr),
    FOREIGN KEY (region_nr)
        REFERENCES Region (nr)
)  ENGINE INNODB;

CREATE TABLE VunnaPriser (
    ren_nr CHAR(11) NOT NULL UNIQUE,
    prisnamn VARCHAR(40) not null unique,
    PRIMARY KEY (ren_nr, prisnamn),
      FOREIGN KEY (ren_nr)
        REFERENCES Ren (nr),
   CHECK (prisnamn RLIKE '[ ][0-9][0-9][0-9][0-9]$')
)  ENGINE INNODB;

CREATE TABLE gamlaVunnaPriser (
    ren_nr CHAR(11) NOT NULL UNIQUE,
    prisnamn VARCHAR(40) not null unique,
    PRIMARY KEY (ren_nr, prisnamn),
      FOREIGN KEY (ren_nr)
        REFERENCES Ren (nr),
   CHECK (prisnamn RLIKE '[ ][0-9][0-9][0-9][0-9]$')
)  ENGINE INNODB;

CREATE TABLE Registrering (
    namn CHAR(11),
    typ TINYINT UNSIGNED,
    PRIMARY KEY (typ),
    CHECK (typ <= 3 AND typ > 0)
)  ENGINE INNODB;

CREATE TABLE Släde (
    nr VARCHAR(10) NOT NULL UNIQUE,
    namn VARCHAR(30),
    tillvärkare VARCHAR(30),
    steglängd VARCHAR(30),
    region_stationerad CHAR(6),
    reg_typ TINYINT UNSIGNED,
	CHECK (reg_typ <= 3 AND reg_typ > 0),
    PRIMARY KEY (nr),
    FOREIGN KEY (region_stationerad) REFERENCES Region(nr),
	FOREIGN KEY (reg_typ) REFERENCES Registrering(typ)
)  ENGINE INNODB;

CREATE TABLE Expressläde (
    släde_nr VARCHAR(10) NOT NULL,
    raketantal VARCHAR(20),
    bromsverkan VARCHAR(20),
    PRIMARY KEY (släde_nr),
    FOREIGN KEY (släde_nr)
        REFERENCES Släde (nr)
)  ENGINE INNODB;

CREATE TABLE LastSläde (
    släde_nr VARCHAR(10) NOT NULL,
    extrakapacitet DOUBLE,
    klimattyp VARCHAR(20),
    PRIMARY KEY (släde_nr),
    FOREIGN KEY (släde_nr)
        REFERENCES Släde (nr)
)  ENGINE INNODB;

CREATE TABLE TotalKapacitet (
    slädesnummer VARCHAR (10),
    kapacitet DOUBLE,
    PRIMARY KEY (slädesnummer),
    FOREIGN KEY (slädesnummer)
		REFERENCES Släde (nr)
   
)  ENGINE INNODB;


###################################################
#TRIGGERS AND PROCEDURES
######################################################
DELIMITER //
create procedure FåKapacitet(
	snr CHAR(11)
    )
    BEGIN
    DECLARE kapacitet DOUBLE;
    SELECT extrakapacitet into kapacitet from LastSläde where släde_nr = snr;
   INSERT INTO
        TotalKapacitet (slädesnummer, kapacitet)
    VALUES
        (snr, kapacitet + 17.8);
    Select * from TotalKapacitet where slädesnummer = snr;
    Select * from TotalKapacitet where slädesnummer = snr;
    END
// DELIMITER ;

 DELIMITER //
Create trigger RenStatus BEFORE delete on TjänsteRen
FOR EACH ROW
BEGIN
 INSERT INTO
        PensioneradRen (ren_nummer)
    VALUES
        (old.ren_nr);
END
// DELIMITER ;


DELIMITER //
CREATE TRIGGER AutoRen after INSERT ON Ren 
FOR EACH ROW
BEGIN
 
insert into TjänsteRen (ren_nr) values (new.nr);
 
END 
// DELIMITER ;

DELIMITER //
Create trigger SlädeCheck before insert on Släde
FOR EACH ROW
BEGIN
  IF(new.namn = "rudolf" OR new.namn = "brynolf" )  THEN
    SIGNAL SQLSTATE '45000' set message_text ='Förvirra inte tomten, ange ett annat namn';
  END IF;
END
// DELIMITER ;

DELIMITER //
    Create procedure UppdateraFabrik(
	nummer CHAR(11), fabrik Varchar(30)
    )
    BEGIN
   UPDATE  PensioneradRen
	SET fabriknamn = fabrik
	WHERE ren_nummer = nummer;
    END
// DELIMITER ;
    
DELIMITER //
    Create procedure PensioneraRen(nummer CHAR(11), fabrik Varchar(30))
    BEGIN
   
     DELETE FROM  TjänsteRen WHERE (ren_nr) = nummer;
      Call UppdateraFabrik(nummer, fabrik);
      UPDATE Ren SET tjänststatus = "pensionerad" where nr = nummer;
    END
// DELIMITER ;
    

START TRANSACTION;
INSERT INTO Registrering(namn, typ)
VALUES('Registrerad', 1);
INSERT INTO Registrering(namn, typ)
VALUES('Avegisterad', 2);
INSERT INTO Registrering(namn, typ)
VALUES('Körförbud', 3);

INSERT INTO Underart(namn, artindex)
VALUES('pearyi', 1);
INSERT INTO Underart(namn, artindex)
VALUES('tarandus', 2);
INSERT INTO Underart(namn, artindex)
VALUES('buskensis', 3);
INSERT INTO Underart(namn, artindex)
VALUES('caboti', 4);
INSERT INTO Underart(namn, artindex)
VALUES('dawsoni', 5);
INSERT INTO Underart(namn, artindex)
VALUES('sibericus', 6);

INSERT INTO Stank(namn, nivå)
VALUES('Tolererbar', 1);
INSERT INTO Stank(namn, nivå)
VALUES('Förnedande', 2);
INSERT INTO Stank(namn, nivå)
VALUES('Kräkreflex triggas', 3);
INSERT INTO Stank(namn, nivå)
VALUES('Kaskadspyr', 4);
INSERT INTO Stank(namn, nivå)
VALUES('Så man svimmar', 5);

INSERT INTO Renspann(namn, kapacitet)
VALUES('Fräna Renarna', 10);
INSERT INTO Renspann(namn, kapacitet)
VALUES('Fula Renarna', 2);
INSERT INTO Renspann(namn, kapacitet)
VALUES('Kosmonauterna', 10);
INSERT INTO Renspann(namn, kapacitet)
VALUES('Raketdjuren', 2);

INSERT INTO Släde(nr, namn)
VALUES(1, 'Sleigher');
INSERT INTO Släde(nr, namn)
VALUES(2, 'Sleighern');
INSERT INTO Släde(nr, namn)
VALUES(3, 'Sleighis');
INSERT INTO Släde(nr, namn)
VALUES(4, 'Sleighf');
INSERT INTO LastSläde(Släde_nr, extrakapacitet)
VALUES(1, 2);

Select * from PensioneradRen;
Select * from TjänsteRen;
Select * from Ren;	

INSERT INTO Region(nr, namn)
VALUES('123457', 'Tibro');
INSERT INTO Region(nr, namn)
VALUES('123458', 'Haparanda');
INSERT INTO Region(nr, namn)
VALUES('123459', 'Tiahölm');
INSERT INTO Region(nr, namn)
VALUES('123451', 'Lischöping');
INSERT INTO Region(nr, namn)
VALUES('123453', 'Skära');
INSERT INTO Region(nr, namn)
VALUES('123454', 'Jokkmokk');
INSERT INTO Region(nr, namn)
VALUES('123455', 'Beirut');

commit;