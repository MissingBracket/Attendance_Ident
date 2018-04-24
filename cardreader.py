import pyasn1
from smartcard.System import readers
from smartcard.util import toHexString
from pyasn1.codec.ber import encoder, decoder

from smartcard.CardType import ATRCardType
from smartcard.CardRequest import CardRequest
from smartcard.util import toHexString, toBytes    
def fn():
    r=readers()
    print (r)
    ['SchlumbergerSema Reflex USB v.2 0', 'Utimaco CardManUSB 0']
    connection = r[0].createConnection()
    connection.connect()
    SELECT = [0x00, 0xA4, 0x00, 0x00, 0x00]
    SELECT2 = [0x00, 0xA4, 0x04, 0x04, 0x06]
    DF_TELECOM = [0x4d, 0x50,0x43,0x4f,0x53,0x31]
    data, sw1, sw2 = connection.transmit( SELECT2 +DF_TELECOM)
    print ("%x %x" % (sw1, sw2)       )
    print (len(data))





cardtype = ATRCardType( toBytes( "3B 6A 00 00 80 65 A2 01 01 01 3D 72 D6 41" ) )
cardrequest = CardRequest( timeout=1, cardType=cardtype )
cardservice = cardrequest.waitforcard()

cardservice.connection.connect()
print (toHexString( cardservice.connection.getATR() )  )

#00 A4 04 00 07 A0 00 00 00 04 10 10 00

Basic_Select = [0x00, 0xA4, 0x04, 0x00]
Basic_Select2= [0x00,0xA4,0x00,0x00,0x02,0x00,0x01]
Sel_Len_Data_RespLen = [0x07, 0xD6, 0x16, 0x00, 0x00,0x30,0x01,0x01]

#Select1 = [0x00, 0xA4, 0x04, 0x00, 0x07, 0xA0, 0x00,0x00,0x00,0x04,0x10,0x10,0x00]
SELECT2 = [0x00, 0xA4, 0x04, 0x04, 0x06]
DF_TELECOM = [0x4d, 0x50,0x43,0x4f,0x53,0x31]
read_binary_base = [0x00, 0xB0]
rbl = [0xf8]
getresponse = [0x00, 0xC0,0x00,0x00,0x12]
#data, sw1, sw2 = cardservice.connection.transmit( SELECT2 + DF_TELECOM )

x= Basic_Select+Sel_Len_Data_RespLen
data, sw1, sw2 = cardservice.connection.transmit( x )
print ("%x %x" % (sw1, sw2)     )
print (len(data))

data, sw1, sw2 = cardservice.connection.transmit( Basic_Select2 )
print ("%x %x" % (sw1, sw2)     )                           
print (len(data))
out = []
region = [0x00, 0x00]
while True:
    data, sw1, sw2 = cardservice.connection.transmit( read_binary_base + region+rbl )
    print ("%x %x" % (sw1, sw2)     )
    print (len(data))
    region[1] += int(rbl[0]/4)
    if region[1] > 255:
        region[0]+=1
        region[1]-=256
    out += data
    if   (data[-1] == 0 and data[-2] == 0)     :
        break
    

dataa = bytes(out)
decoded = decoder.decode(dataa)
f = open("file", mode='wb')
f.write(bytes(out))
