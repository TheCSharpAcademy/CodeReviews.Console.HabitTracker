
# Habit Logger Console App
This is a C# Academy project with some additional functionalities for tracking daily 
calories. The application allows users to log their daily calorie intake and perform 
various operations on the logged data. Below are the requirements, features, user 
manual, areas for improvement, and additional challenges of the application.  

## Requirements
- [x] Track one habit only by quantity (ex. number of water glasses a day), not
      by time (ex. hours of sleep)
- [x] Store and retrieve data from a real database (SQLite)
- [x] Create a SQLite database and a table for logging habits if they don't exist
- [x] Show the user a menu of options
- [x] The users should be able to insert, delete, update and view their logged habits
- [x] Handle all possible errors so that the application never crashes
- [x] The application should only be terminated when the user inserts 0
- [x] Interact with the database using raw SQL without using mappers such as
      Entity Framework
- [x] Project needs to contain a Read Me file where you'll explain how your app works

## Features and validations
* **SQLite database connection**: The application connects to a SQLite database
  to store and read data.
* **New Database Creation**: If the database or table doesn't exist, the application
  creates them when it starts.
* **User input checks**:
  * Date must be in format YYYY-MM-DD.
  * Duplicate dates cannot be entered unless updating an existing record.
  * Calories input must be a positive integer.
* **ID check**: Verify if the entered ID exists in the database before
  performing operations.

## User Manual
### Menu Navigation
* User navigates in Main menu by pressing specific keys and Enter  
  ![Menu](https://github.com/HopelessCoding/learning/assets/161690352/27c11482-75d6-4b6f-bf2b-7a2cef5370bf)

### Menu Options  
* **A - Add New Record**: Allows the user to add a new record.
  * User should enter a valid date or leave it empty to use current date
  * If date already exists program writes a message and returns to main menu
  * User should enter valid calories
* **V - View All Records**: List all record from the database  
 ![Input](https://github.com/HopelessCoding/learning/assets/161690352/d7e576a2-a892-4f6f-9a75-21ad50c85c9a)
* **U - Update Record**: Updates an existing record by entering its ID
  * User should enter a valid date or leave it empty to use current date
  * If date already exists program writes a message and returns to main menu
  * User should enter valid calories
* **D - Delete Record**: Deletes a record by entering its ID
* **R - Generate Reports**: Opens reports menu 
* **0 - Close Application**: Terminates the application

### Reports Menu Options
* **3 - Last 3 Days**: Prints report from last 3 days  
![Report](https://github.com/HopelessCoding/learning/assets/161690352/8886c595-63ca-43cf-8a2c-c72aa0fbda8c)
* **7 - Last 7 Days**: Prints report from last 7 days
* **A - Average Calories for Last X Days**: Calculates average calories from last
  X days based on user input 
* **0 - Exit to Main Menu**: Returns back to main menu  

## Areas for Improvement and Lessons Learned
* **Consider All Potential Use and Error Cases**: Thinking about potential use
  cases and error scenarios beforehand can simplify the coding process
  * No need to start adding some validation checks when everything else is done
* **Code Structure**: Consider organizing the code, such as using classes and multiple
  projects, during the coding phase for better organization.
  * Will help the final code organization, especially important in larger projects
* **Return to Main Menu**: It would be nice to add clear funtionality which would return
  user back to main menu when they have already selected a command
  * Now the user is returned back to the main menu by pressing Enter or entering
    a faulty value
* **Validation Checks**: Very happy for all the validation checks which are done,
  take lot of time to learn and implement those correctly but it was worth it
  * Those probly could be implemented in a smarter way in some cases but next time
 
## Addtional challenges for future
- [x] Report functionality
- [ ] User created habits to track
- [ ] Seed Data into the database automatically when the database gets created for
      the first time, generating a few habits and inserting a hundred records with
      randomly generated values
