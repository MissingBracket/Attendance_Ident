-- --------------------------------------------------------
-- Host:                         127.0.0.1
-- Wersja serwera:               5.7.18-log - MySQL Community Server (GPL)
-- Serwer OS:                    Win64
-- HeidiSQL Wersja:              9.5.0.5196
-- --------------------------------------------------------

/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET NAMES utf8 */;
/*!50503 SET NAMES utf8mb4 */;
/*!40014 SET @OLD_FOREIGN_KEY_CHECKS=@@FOREIGN_KEY_CHECKS, FOREIGN_KEY_CHECKS=0 */;
/*!40101 SET @OLD_SQL_MODE=@@SQL_MODE, SQL_MODE='NO_AUTO_VALUE_ON_ZERO' */;


-- Zrzut struktury bazy danych logowanieobecnosci
CREATE DATABASE IF NOT EXISTS `logowanieobecnosci` /*!40100 DEFAULT CHARACTER SET utf8 */;
USE `logowanieobecnosci`;

-- Zrzut struktury procedura logowanieobecnosci.Add_student
DELIMITER //
CREATE DEFINER=`skip-grants user`@`skip-grants host` PROCEDURE `Add_student`(IN ind integer, OUT result integer)
procedurka:BEGIN

#deklarujemy zmienne w które będziemy ładować dane z tabel
declare id_przedmiotu INT;
declare id_za INT;
declare ile INT;


#sprawdzamy czy istnieje taki student który potwierdza swoją obecność
if exists(Select * from student where indeks = ind) then
Begin
#ustalamy jaki mamy czas
set @teraz = NOW();
#wyciągamy godziny i minuty
set @godzina_minuty = date_format(@teraz, "%H:%i");

#porównujemy zakresy czasu z czasem logowania się studenta żeby wyznaczyć czas początku zajęć
if(@godzina_minuty >= '8:00' and @godzina_minuty < '9:30') then
	set @hou = '8:00';
elseif (@godzina_minuty >= '9:45' and @godzina_minuty < '11:15') then
	set @hou = '9:45';
elseif (@godzina_minuty >= '11:45' and @godzina_minuty < '13:15') then
	set @hou = '11:45';
elseif (@godzina_minuty >= '13:30' and @godzina_minuty < '15:00') then
	set @hou = '13:30';
elseif (@godzina_minuty >= '15:10' and @godzina_minuty < '16:40') then
	set @hou= '15:10';
elseif (@godzina_minuty >= '16:50' and @godzina_minuty < '18:20') then
	set @hou = '16:50';
elseif (@godzina_minuty >= '18:30' and @godzina_minuty < '20:00') then
	set @hou = '18:30';
else
Begin
set result = 2;
Leave procedurka;
End;

end if;

#ustalamy dzień tygodnia zajęć
set @dzien_tygodnia = dayofweek(@teraz);
if(@dzien_tygodnia = 2) then
	set @dzien_tygodnia = 'Poniedziałek';
elseif(@dzien_tygodnia = 3) then
	set @dzien_tygodnia = 'Wtorek';
elseif(@dzien_tygodnia = 4) then
	set @dzien_tygodnia = 'Środa';
elseif(@dzien_tygodnia = 5) then
	set @dzien_tygodnia = 'Czwartek';
elseif(@dzien_tygodnia = 6) then
	set @dzien_tygodnia = 'Piątek';
else
Begin
set result = 1;
Leave procedurka;
End;
end if;


#wyciągamy przedmiot (z przedmiotów na które chodzi logujący się student - zagnieżdżony select) 
#po dniu tygodnia i godzinie - zakładamy, że student powinien mieć tylko jeden taki przedmiot
Select przedmiot.id_przed, Count(przedmiot.id_przed) into id_przedmiotu, ile from  przedmiot, (Select obowiazek_obecnosci.przed_id from obowiazek_obecnosci where stud_id = ind) as przed
where przedmiot.dzien = @dzien_tygodnia and przedmiot.godzina = @hou and przedmiot.id_przed in (przed.przed_id);

if(ile =0) then
Begin
set result = 3;
Leave procedurka;
End;
end if;

#ustalamy dzień zajęć dzisiejszych
set @dzien = date(@teraz);
#set @czas = STR_TO_DATE(CONCAT(@dzien, ' ', @hou), '%Y-%m-%d %H:%i:%s');
#wyciągamy zajęcia po dniu dzisiejszym i typie przedmiotu ustalonym wcześniej
#zakładamy, że jeden przedmiot nie będzie więcej niż raz na dzień dla studenta
Select zajecia.id_zajec, Count(zajecia.id_zajec) into id_za, ile from zajecia where date(zajecia.data) = @dzien and zajecia.typ_zaj in (id_przedmiotu) order by zajecia.data desc limit 1;


if(ile =0) then
Begin
set result = 4;
Leave procedurka;
End;
end if;

insert into obecnosc values(id_za, ind);
set result = 5;
End;
else 
set result =9;

end if;


END//
DELIMITER ;

-- Zrzut struktury tabela logowanieobecnosci.obecnosc
CREATE TABLE IF NOT EXISTS `obecnosc` (
  `zaj_id` int(11) NOT NULL,
  `stud_id` int(11) NOT NULL,
  PRIMARY KEY (`zaj_id`,`stud_id`),
  KEY `FK_obecnosc_student` (`stud_id`),
  CONSTRAINT `FK_obecnosc_student` FOREIGN KEY (`stud_id`) REFERENCES `student` (`indeks`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_obecnosc_zajecia` FOREIGN KEY (`zaj_id`) REFERENCES `zajecia` (`id_zajec`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Zrzucanie danych dla tabeli logowanieobecnosci.obecnosc: ~0 rows (około)
/*!40000 ALTER TABLE `obecnosc` DISABLE KEYS */;
/*!40000 ALTER TABLE `obecnosc` ENABLE KEYS */;

-- Zrzut struktury tabela logowanieobecnosci.obowiazek_obecnosci
CREATE TABLE IF NOT EXISTS `obowiazek_obecnosci` (
  `stud_id` int(11) NOT NULL,
  `przed_id` int(11) NOT NULL,
  PRIMARY KEY (`stud_id`,`przed_id`),
  KEY `FK_obowiazek_obecnosci_przedmiot` (`przed_id`),
  CONSTRAINT `FK_obowiazek_obecnosci_przedmiot` FOREIGN KEY (`przed_id`) REFERENCES `przedmiot` (`id_przed`) ON DELETE CASCADE ON UPDATE CASCADE,
  CONSTRAINT `FK_obowiazek_obecnosci_student` FOREIGN KEY (`stud_id`) REFERENCES `student` (`indeks`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Zrzucanie danych dla tabeli logowanieobecnosci.obowiazek_obecnosci: ~0 rows (około)
/*!40000 ALTER TABLE `obowiazek_obecnosci` DISABLE KEYS */;
/*!40000 ALTER TABLE `obowiazek_obecnosci` ENABLE KEYS */;

-- Zrzut struktury tabela logowanieobecnosci.prowadzacy
CREATE TABLE IF NOT EXISTS `prowadzacy` (
  `id_prow` int(11) NOT NULL AUTO_INCREMENT,
  `imie` varchar(50) DEFAULT NULL,
  `nazwisko` varchar(50) DEFAULT NULL,
  `skrot_hasla` varchar(100) DEFAULT NULL,
  PRIMARY KEY (`id_prow`),
  UNIQUE KEY `Indeks 2` (`imie`,`nazwisko`,`skrot_hasla`)
) ENGINE=InnoDB AUTO_INCREMENT=2 DEFAULT CHARSET=utf8;

-- Zrzucanie danych dla tabeli logowanieobecnosci.prowadzacy: ~0 rows (około)
/*!40000 ALTER TABLE `prowadzacy` DISABLE KEYS */;
INSERT IGNORE INTO `prowadzacy` (`id_prow`, `imie`, `nazwisko`, `skrot_hasla`) VALUES
	(1, 'admin', 'admin', '76F02324451CE619B9335592E1531CF80EC5BF1997EB80AF5141EF8509758628');
/*!40000 ALTER TABLE `prowadzacy` ENABLE KEYS */;

-- Zrzut struktury tabela logowanieobecnosci.przedmiot
CREATE TABLE IF NOT EXISTS `przedmiot` (
  `id_przed` int(11) NOT NULL AUTO_INCREMENT,
  `prow_id` int(11) DEFAULT NULL,
  `dzien` varchar(50) DEFAULT NULL,
  `godzina` varchar(50) DEFAULT NULL,
  `sala` varchar(50) DEFAULT NULL,
  `nazwa` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`id_przed`),
  KEY `FK_przedmiot_prowadzacy` (`prow_id`),
  CONSTRAINT `FK_przedmiot_prowadzacy` FOREIGN KEY (`prow_id`) REFERENCES `prowadzacy` (`id_prow`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=18 DEFAULT CHARSET=utf8;

-- Zrzucanie danych dla tabeli logowanieobecnosci.przedmiot: ~0 rows (około)
/*!40000 ALTER TABLE `przedmiot` DISABLE KEYS */;
/*!40000 ALTER TABLE `przedmiot` ENABLE KEYS */;

-- Zrzut struktury tabela logowanieobecnosci.student
CREATE TABLE IF NOT EXISTS `student` (
  `indeks` int(11) NOT NULL,
  `imie` varchar(50) DEFAULT NULL,
  `nazwisko` varchar(50) DEFAULT NULL,
  PRIMARY KEY (`indeks`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- Zrzucanie danych dla tabeli logowanieobecnosci.student: ~0 rows (około)
/*!40000 ALTER TABLE `student` DISABLE KEYS */;
/*!40000 ALTER TABLE `student` ENABLE KEYS */;

-- Zrzut struktury tabela logowanieobecnosci.zajecia
CREATE TABLE IF NOT EXISTS `zajecia` (
  `id_zajec` int(11) NOT NULL AUTO_INCREMENT,
  `data` datetime DEFAULT NULL,
  `nazwa` varchar(50) DEFAULT NULL,
  `typ_zaj` int(11) DEFAULT NULL,
  PRIMARY KEY (`id_zajec`),
  UNIQUE KEY `Indeks 3` (`data`,`typ_zaj`),
  KEY `FK_zajecia_przedmiot` (`typ_zaj`),
  CONSTRAINT `FK_zajecia_przedmiot` FOREIGN KEY (`typ_zaj`) REFERENCES `przedmiot` (`id_przed`) ON DELETE CASCADE ON UPDATE CASCADE
) ENGINE=InnoDB AUTO_INCREMENT=33 DEFAULT CHARSET=utf8;

-- Zrzucanie danych dla tabeli logowanieobecnosci.zajecia: ~0 rows (około)
/*!40000 ALTER TABLE `zajecia` DISABLE KEYS */;
/*!40000 ALTER TABLE `zajecia` ENABLE KEYS */;

/*!40101 SET SQL_MODE=IFNULL(@OLD_SQL_MODE, '') */;
/*!40014 SET FOREIGN_KEY_CHECKS=IF(@OLD_FOREIGN_KEY_CHECKS IS NULL, 1, @OLD_FOREIGN_KEY_CHECKS) */;
/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
