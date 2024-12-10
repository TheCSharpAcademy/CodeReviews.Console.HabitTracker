# HabitTracker
HabitTracker is a simple C# console app that interacts with a SQlite Database to store your habbits. This has been my first time performing CRUD operations against a databse.

## Requirments
- [x] This habit can't be tracked by time (ex. hours of sleep), only by quantity (ex. number of water glasses a day)
- [x] Users need to be able to input the date of the occurrence of the habit
- [x] When the application starts, it should create a sqlite database, if one isn’t present.
- [x] The users should be able to insert, delete, update and view their logged habit.
- [x] You should handle all possible errors so that the application never crashes.
- [x] You can only interact with the database using ADO.NET. You can’t use mappers such as Entity Framework or Dapper.

## Features
* SQlite Database
  - Creates a Databse if one is not presents.
  - Allows users to perform CRUD operations against the databse.
* User Interface
  - Neat Console user Interface.
  - Allows users to navigate through various operations through keyboard.
* Simple Report function
  - Number of entries.
  - Sum of entries.
  - Average value of entries.
  - List of all entriess of specified habit.
 
## Lessons Learned
* To complete this project I was required to learn basic SQL functionality.
* I learned how to perform CRUD operations using ADO.Net on a SQLite Databse.
* I researched DRY principles and tried to incorporate them as far as possible.
* Github Markdown.

# Resources
* Basic HabitTracker Tutorial from [TheC#Acadeny](https://www.youtube.com/watch?v=d1JIJdDVFjs&embeds_referring_euri=https%3A%2F%2Fwww.thecsharpacademy.com%2F&source_ve_path=MjM4NTE)
* Basic [SQL Tutorial](https://www.codecademy.com/catalog/language/sql?g_network=g&g_productchannel=&g_adid=528849219334&g_locinterest=&g_keyword=codecademy%20sql&g_acctid=243-039-7011&g_adtype=&g_keywordid=kwd-352193271727&g_ifcreative=&g_campaign=account&g_locphysical=9215811&g_adgroupid=128133970708&g_productid=&g_source={sourceid}&g_merchantid=&g_placement=&g_partition=&g_campaignid=1726903838&g_ifproduct=&utm_id=t_kwd-352193271727:ag_128133970708:cp_1726903838:n_g:d_c&utm_source=google&utm_medium=paid-search&utm_term=codecademy%20sql&utm_campaign=INTL_Brand_Exact&utm_content=528849219334&g_adtype=search&g_acctid=243-039-7011&gad_source=1&gclid=CjwKCAiA6t-6BhA3EiwAltRFGA8Lp_4HRR3VKbtWl8pB_oaD6FGm3vVF-oXY7s4GSScY1gOK-yB_SBoC2_kQAvD_BwE)

