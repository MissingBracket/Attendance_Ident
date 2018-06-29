drop procedure if exists `Add_student`;

#kody 
#0 - nie dodano
#1 - zły dzień tygodnia
#2 - zła godzina
#3 - brak przedmiotu lub obowiazku przedmiotu
#4 - brak zajec
#9 - brak studenta
delimiter //
CREATE PROCEDURE `Add_student` (IN ind integer, OUT result integer)
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
delimiter ;