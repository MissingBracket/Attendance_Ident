import MySQLdb as sqldb
from MySQLdb import IntegrityError
def writeToDb(imie, nazwisko, indeks):
    try:
        db = sqldb.connect('localhost', 'root', '', 'logowanieObecnosci', charset="utf8")

        # prepare a cursor object using cursor() method
        cursor = db.cursor()

        # Drop table if it already exist using execute() method.
        cursor.execute("call Add_student(%s, @%s)", (indeks, 'a'))

        cursor.execute('SELECT @a')
        (code) = cursor.fetchone();

        db.commit()

        # disconnect from server
        db.close()
        if code[0] == 0:
            return "Nie dodano obecności - być może już potwierdzono obecność."
        if code[0] == 9:
            return "Nie ma takiego studenta."
        if code[0] == 1:
            return "Dziś nie odbywają się zajęcia"
        if code[0] == 2:
            return "O tej godzinie nie odbywają się zajęcia."
        if code[0] == 3:
            return "Ten student nie jest zapisany na przedmiot, który odbywa się teraz."
        if code[0] == 4:
            return "Dzisiaj nie odbywają się zajęcia z tego przedmiotu."
        if code[0] == 5:
            return "Dodano obecność."


    except IntegrityError as e:
        db.close()
        return "Nie dodano obecności - być może już potwierdzono obecność."

    except Exception as e:
        print(e)
        db.close()




        #print(result.fetch_row()[0])