DROP TABLE IF EXISTS habit;

CREATE TABLE habit (
                       id INTEGER PRIMARY KEY AUTOINCREMENT ,
                       user TEXT NOT NULL,
                       habit TEXT NOT NULL,
                       count INTEGER NULL,
                       date DATETIME NULL
);

INSERT INTO
    habit (user, habit, count)
values
    ( 'John','drinkingWater', 10);

-- Insert new habit
INSERT INTO
    habit (user, habit, count, date)
values
    ( 'John','drinkingCoffee', 10, '2025-07-06' );

-- Retrieve stored habits
SELECT *
FROM habit
WHERE user = 'John';

-- update previous habits: count or date
UPDATE habit
SET date = '2025-07-01',
    count = 5
where id = 2;

-- delete single habit
delete from habit
where id = 1;

-- delete all instances of a habit
delete from habit
where habit = 'drinkingCoffee';