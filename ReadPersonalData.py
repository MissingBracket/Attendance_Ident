import pyasn1
from smartcard.Exceptions import CardRequestTimeoutException
from smartcard.System import readers
from smartcard.util import toHexString
from pyasn1.codec.ber import encoder, decoder

from smartcard.CardType import ATRCardType
from smartcard.CardRequest import CardRequest
from smartcard.util import toHexString, toBytes
import ctypes  # An included library with Python install.
from threading import Thread
import re

def popWindow():
    box = ctypes.windll.user32.MessageBoxW(0, Message, "Dane studenta", 0)


def getCard():
    try:
        cardrequest = CardRequest(timeout=60)
        cardservice = cardrequest.waitforcard()
        cardservice.connection.connect()
        return cardservice
    except Exception:
        return None


alreadRead = False
while True:
    try:
        selectDir = [0x00, 0xA4, 0x04, 0x00, 0x07, 0xD6, 0x16, 0x00, 0x00, 0x30, 0x01, 0x01]
        selectFile = [0x00, 0xA4, 0x02, 0x00, 0x02, 0x00, 0x02]
        getResponse = [0x00, 0xc0, 0x00, 0x00, 0x12]

        read_binary_base = [0x00, 0xB0]
        read_region = [0x00, 0x00]
        read_length = [0xf8]

        #cardservice = None
        while (True):

            cardservice = getCard()

            if cardservice != None:
                if alreadRead == False:
                    break
            else:
                alreadRead =False




        data, sw1, sw2 = cardservice.connection.transmit( selectDir )
        data, sw1, sw2 = cardservice.connection.transmit( selectFile )

        data, sw1, sw2 = cardservice.connection.transmit( getResponse )


        out = []
        while True:
            data, sw1, sw2 = cardservice.connection.transmit(read_binary_base + read_region + read_length)
            read_region[1] += int(read_length[0] / 4)
            if read_region[1] > 255:
                read_region[0] += 1
                read_region[1] -= 256
            out += data
            if (data[-1] == 0 and data[-2] == 0):
                break

        out = bytes(out)
        decoded = decoder.decode(out)

        x = decoded[0]._componentValues[1]._componentValues[2]._componentValues[1]._value

        string = x.decode('utf-8')
        parts = string.split("\f")
        SurnamePattern = re.compile('[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ\-]+')
        NamePattern = re.compile('[A-Za-zżźćńółęąśŻŹĆĄŚĘŁÓŃ]+')
        IndexPattern = re.compile('[0-9]{6}')

        FirstName=re.search(NamePattern, parts[3]).group()
        SecondName=""
        Surname = re.search(SurnamePattern, parts[2]).group()
        Index = ""

        if len(parts) == 5:
           SecondName = re.search(NamePattern, parts[4]).group()
           Index = re.search(IndexPattern, parts[4]).group()
        else:
            Index = re.search(IndexPattern, parts[3]).group()



        Message = FirstName + " " + SecondName +" " + Surname + ", " + Index
        print(Message)
        alreadRead=True
        popWindow()
        #f = open("file", mode='wb')
        #f.write(bytes(out))
    except Exception as e:
        pass


