# Habbit Logger
This application allows its user to register a habbit with a desired measure. The user needs to enter the date and the measure quantity of the habbit to be logged.

##Usage

When the application is open, the user is presented with the following options:
#### 1 Insert a new log

The user is able to log new habbits with the following parameters:

**Date**: The format used is yyyy/MM/dd. The program validates if the user entered a valid date.
**Measure**: And integer quantity of the habbit to be measured. It has to be higher than 0.

#### 2 Delete a previous log

The user need to input the log ID to be deleted.

####3 Update a previous log

The user needs the input log ID to be updated.
#### 4 View the recorded logs
####5 Switch to another habbit

The console will print all the habibits stored in the database. The user has to write the name of the desired habbit to be modified.

#### 6 Create a new habbit log

The user is also able to create new habbit tables. They have to input a measure unit and a name for the habbit.


#### 7 Show habbit statistics
####0 Exit the application


##To be done

1. Split  user input from DB managment
2. Rewrite methods to reuse more code
3. Display results in a more friendly way
4. Add more habbit statistics
