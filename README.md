# _Hair Salon_

#### _An app to organize stylists' and clients' information, 08.18.17_

#### By _**Kaili Nishihira**_

## Description

_An app which will enable the user to enter and retrieve a stylist's name and service pricing. The user will also be able to assign a stylist to a client and enter and retrieve a client's name and contact information._

|| Behavior  | Input  | Output  |
|---|---|---|---|
|1| User may view a list of all stylists on the Home/Index view  | Click `Home` in navigation bar  | View displays buttons with stylist's names `Ken` `Frederic` |
|2| User may view a stylist's details. <li>Click stylist's button in Index view</li>  | Click `Chris`  | Chris McMillan <br> Women's Cut $100 <br> Men's Cut $80 <br><br> Client List<li>Jennifer Lawrence</li><li>Taylor Swift</li> |
|3| User may enter a new stylist <li>Click `Add Stylist` in navigation bar</li> <li>View returns a form to enter the stylist's information| Enter a new stylist: <br> First Name: 'Vidal' <br> Last Name: 'Sassoon' <br> Women's Cut: $ '170' <br> Men's Cut: $ '150' <br> Click `Add Stylist`| Index view of all stylists: <br> ... <br> `Vidal` <br> ... |
|4| User may add a new client to a specific stylist <li>In stylist's details view, enter client's information into the form 'New client for (stylist's name)'</li> | Enter a new client for Chris McMillan: <br> First Name: Lisa <br> Last Name: Smith <br> Phone: 808-555-1234 <br> Email: lisa.smith@gmail.com <br> Click `Add client` | Chris McMillan Client List: <br> ... <br> Smith, Lisa <br> ... |
|5| User may view a client's details <li>Click `All clients` in the navigation bar to locate a client's name</li> <li>You may also locate a client's name in the stylist's details view under 'client list'</li> | Click on 'Smith, Lisa'  | Lisa Smith <br> Stylist: Chris McMillan <br> Phone: 808-555-1234 <br> Email: lisa.smith@gmail.com  |
|6| User may update a client's first name, last name, phone number, email and stylist<li>Click on client's name</li> <li>View returns the client's details</li> <li>Enter updated information in 'Edit client details' form</li> <li>Click `Update`</li> | Currently: <br> Lisa Smith <br><br> Last Name: 'Ford' <br> Click `update` | 'Client Details' view: <br> Lisa Ford <br> ... |
|7| User may delete a client <li>Click on client's name</li> <li>View returns the client's details</li>  | Click `Delete client`  | Confirmation view: 'Client has been deleted'  |
|8| User may delete a stylist <li>Click on stylist's name</li> <li>View returns the stylist's details</li> <li>Click `edit`</li> View returns 'Edit Stylist Details'</li> | Click `Delete stylist`  | Confirmation view: 'Stylist has been deleted'  |
|9| User may update a stylist's first name, last name, pricing for women's and men's hair cuts <li>Click stylist's button</li> <li>View returns 'Stylist Details'</li> <li>Click `edit`</li> <li>View returns 'Edit Stylist Details' with form to update the stylist's information | Currently: <br> Men's Cut $40 <br> <br> Men's Cut $ '45' <br> Click `update`  | 'Stylist Details' view: <br> ... <br> Men's Cut $45 <br> ...  |




## Setup/Installation Requirements

* _Download and install [.NET Core 1.1 SDK](https://www.microsoft.com/net/download/core)_
* _Download and install [Mono](http://www.mono-project.com/download/)_
* _Download and install [MAMP](https://www.mamp.info/en/)_
* _Set MySQL Port to 3306_
* _Clone repository_

#### There are two options to create the database:
##### 1. In MySQL
`> CREATE DATABASE hair_salon;`<br>
`> USE hair_salon;`<br>
`> CREATE TABLE stylists (id serial PRIMARY KEY, first_name VARCHAR(255), last_name VARCHAR(255), womens_cut INT, mens_cut INT);`<br>
`> CREATE TABLE clients (id serial PRIMARY KEY, first_name VARCHAR(255), last_name VARCHAR(255), phone VARCHAR(255), email VARCHAR(255), stylist_id INT );`
##### 2. Import from the Cloned Repository
* _Click 'Open start page' in MAMP_
* _Under 'Tools', select 'phpMyAdmin'_
* _Click 'Import' tab_
* _Choose database file (from cloned repository folder)_
* _Click 'Go'_

## Technologies Used
* _C#_
* _.NET_
* _[Bootstrap](http://getbootstrap.com/getting-started/)_
* _[MySQL](https://www.mysql.com/)_

### License

Copyright (c) 2017 **_Kaili Nishihira**

*Licensed under the [MIT License](https://opensource.org/licenses/MIT)*
