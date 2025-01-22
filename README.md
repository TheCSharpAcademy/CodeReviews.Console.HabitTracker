# Habit Tracker

This is a console application, written in C#, that allows you to track your habits.

## Features

- **Custom Habits** - You have the ability to create custom habits, including being able to enter a unit of measurement.
- **Statistics** - Statistics related to how often you're doing a habit, as well as a sum of whatever unit you're using to measure said habit, are shown over the past week, month, and year.
- **Permanent Data** - All data is stored in an SQLite database, which is created when starting the application.

## How To Use

The following is an image that shows the UI that you are greeted with when opening the application:

![application UI](https://i.imgur.com/tUW8d7K.png)

The options given are fairly self explanatory.

**View/New/Update/Delete Habit** all relate to the custom habit functionality, where you have the option of modifying and viewing custom habits.

**New/Update/Delete Habit Entry** do the same for habit entries that are related to your habits, where **View Statistics** will give you statistics related to the past week, month, and year for each habit. This includes the number of entries you've made, as well as the sum of the values you've inputted, based on the habit's unit of measurement:

![habit statistics](https://i.imgur.com/sg3nyt7.png)
