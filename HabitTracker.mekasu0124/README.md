# Mek's Habit Tracker

Table Of Contents:
- [Introduction](#introduction)
- [Usage](#usage)
- [Issues](#issues)
- [Future Updates](#future-updates)

# Introduction

Welcome To Mek's Habit Tracker. This is a console based application that assists you in tracking your current habits that are tracked by a counter. This application was built using:
- [The C# Programming Language](https://learn.microsoft.com/en-us/dotnet/csharp/tour-of-csharp/)
- [System.Data.SQLite](https://system.data.sqlite.org/index.html/doc/trunk/www/index.wiki)
    - This is used to store/retrieve your data locally. Your information is not stored on a cloud-based backup system, but rather on your computer itself. **__Warning:__** If you delete this application, you lose your progress!
- [ConsoleTables](https://github.com/khalidabuhakmeh/ConsoleTables)
    - This is used to assist in pretty printing your information to the console to make for easier reading.

# Usage

![plot](/HabitTracker.mekasu0124/Assets/main_menu.png)
When you first start the application, you're presented with a menu of actions you can choose from. To record your first entry, you'll enter "1" into the console.

![plot](/HabitTracker.mekasu0124/Assets/new_item.png)
When adding a new item, you'll be prompted for the name of the habit and the description of the habit. At this time, the date the habit is created is automatically recorded for you, and these habits are only done by counters. Once you've successfully given information to the prompts, you'll see a success message that your habit has been saved and then you'll be routed back to the main menu.

![plot](/HabitTracker.mekasu0124/Assets/edit_item_menu.png)
If you select option 2 from the main menu, you'll be brought to the screen that shows all of your currently tracked habits, their current stand points, and then you'll be presented with a small menu asking if you'd like to edit the information of one of the currently tracked habits, or increase the counter of one of the currently tracked habits.

![plot](/HabitTracker.mekasu0124/Assets/editing_item.png)
If you select to edit a currently tracked habit, you'll be asked for a new name, if you'd like to reset the counter, and a new description for that habit. If you do not want to change any information, then just press enter to leave the entry blank and it will keep the current information. For resetting the counter, enter either "y" or "n" to act accordingly. Upon successfully updating the tracked habit, you'll see a success message and then you'll be routed back to the main menu.

![plot](/HabitTracker.mekasu0124/Assets/increasing_counter.png)
If you select to just update the counter of a currently tracked habit, then you'll see a simple success message that your habits counter was increased and then you'll be routed back to the main menu.

![plot](/HabitTracker.mekasu0124/Assets/delete_item.png)
Whenever you're done tracking an item, you'll select option 3 from the main menu to delete it, then you'll be asked for the number of the habit you'd like to delete. Upon successful deletion of that habit, you'll see a simple success message and then you'll be routed back to the main menu.

![plot](/HabitTracker.mekasu0124/Assets/view_all_entries.png)
Whenever you'd like to view the progress of all of your currently tracked habits, simply enter option 4 on the main menu and it'll do a pretty print of your habits. This will show you various information on each habit that is currently being tracked and you'll also see the main menu to ask what you'd like to do so that you can see your currently tracked habits to assist with main menu interaction.

At any time that you're finished using the application, you can enter 0 on the main menu to exit the application safely.

# Issues
If at any time you encounter any issues while using my application, please find me on discord, mekasu0124, and send me a message. I'll happily assist in any way that I can

# Future Updates
At this time, updates are not being incorporated into the application. Once I have more time to dedicate to this software, I'll be turning this application into an Avalonia Desktop GUI application with the abilities of other tracking methods instead of just a counter. 