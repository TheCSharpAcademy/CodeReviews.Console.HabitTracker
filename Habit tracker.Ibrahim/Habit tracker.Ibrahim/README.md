<h1>Prayer Tracker Application</h1>

<h2>By: Ibrahim Gilani</h2>

<h3>Overview</h3>

Prayer Tracker is a simple yet powerful application designed to help users keep track of their daily prayers. Built with C# and using SQLite for data storage, this application offers features like viewing prayer history, adding new prayer records, updating existing ones, and deleting records. It also provides reports on prayer averages over different time periods.

<h3>Usage</h3>

Upon starting the application, you will be greeted with a welcome message and instructions to access the main menu. The main menu provides the following options:

View History: Displays your prayer history.
Insert Prayer Data: Add new prayer records.
Delete Prayer Data: Remove existing prayer records.
Update Prayer Data: Modify the number of prayers for a specific date.
View Reports: Shows average prayers for the last week, year, and all time.
Navigate through these options by typing the corresponding number and pressing Enter.

<h3>Features</h3>

Prayer Tracking: Keep a record of your daily prayers.
Data Management: Easily insert, update, or delete prayer data.
Reporting: View average prayers over different time periods.
Data Validation: Ensures correct data entry with built-in validation.

<h3>Input Validation</h3>

The Prayer Tracker application includes several input validation checks to ensure smooth and accurate data entry:

Date Validation: When entering dates (for adding, updating, or deleting prayer data), the application requires the format yyyy-MM-dd. If an invalid date or format is entered, it prompts the user to re-enter the correct date.

Prayer Count Validation: While inserting or updating prayer data, the application checks that the number of prayers entered is within the specified range (0 to 5). If a number outside this range or a non-numeric value is entered, it requests a valid input.

Menu Choice Validation: On the main menu, the application ensures that only valid choices (0 to 5) are entered. If an invalid option is selected, it prompts the user to enter a valid choice.

These validations enhance the user experience by guiding them to input correct data and preventing common entry errors.
