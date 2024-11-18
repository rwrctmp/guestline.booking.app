The Challenge: 

Create a program to manage hotel room availability and reservations. The application should read from files containing hotel data and booking data, then allow a user to check room availability for a specified hotel, date range, and room type.  

Example command to run the program:  

myapp --hotels hotels.json --bookings bookings.json   

 

Example: hotels.json  

[

    {

        "id": "H1",

        "name": "Hotel California",

        "roomTypes": [

            {

                "code": "SGL",

                "description": "Single Room",

                "amenities": ["WiFi", "TV"],

                "features": ["Non-smoking"]

            },

            {

                "code": "DBL",

                "description": "Double Room",

                "amenities": ["WiFi", "TV", "Minibar"],

                "features": ["Non-smoking", "Sea View"]

            }

        ],

        "rooms": [

            {

                "roomType": "SGL",

                "roomId": "101"

            },

            {

                "roomType": "SGL",

                "roomId": "102"

            },

            {

                "roomType": "DBL",

                "roomId": "201"

            },

            {

                "roomType": "DBL",

                "roomId": "202"

            }

        ]

    }

]

 

 

Example: bookings.json  

[

    {

        "hotelId": "H1",

        "arrival": "20240901",

        "departure": "20240903",

        "roomType": "DBL",

        "roomRate": "Prepaid"

    },

    {

        "hotelId": "H1",

        "arrival": "20240902",

        "departure": "20240905",

        "roomType": "SGL",

        "roomRate": "Standard"

    }

]

 

 

The program should implement the 2 commands described below. 

The program should exit when a blank line is entered.  

 

Availability Command 

 

Example console input: 

Availability(H1, 20240901, SGL) 

Availability(H1, 20240901-20240903, DBL)    

 

Output: the program should give the availability count for the specified room type and date range.

Note: hotels sometimes accept overbookings so the value can be negative to indicate this. 

 

Search Command 

 

Example input:

Search(H1, 35, SGL)

The program should return a comma separated list of date ranges and availability where the room is available. The 35 is the number of days to look ahead. 

If there is no availability the program should return an empty line. 

 

Example output:

 

(20241101-20241103, 2), (20241203-20241210, 1)



Meaning that in next 35 days there are 2 rooms of type “SGL” in hotel “H1" available in the period of 2024.11.01-2024.11.03 and 1 room available in 2024.12.03-2024.12.10 period.
