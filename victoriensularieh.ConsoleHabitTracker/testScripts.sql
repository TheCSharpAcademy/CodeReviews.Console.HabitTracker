-- SQLite
--entries listing sql statement
-- SELECT Habit.ID AS 'HabitID',
-- Habit.Name as 'Habit name', Entry.Date, Entry.Quantity,Unit.ID as 'UnitID',Unit.Symbol
-- FROM Entry,Habit,Unit
-- WHERE Entry.HabitID = Habit.ID
-- AND Habit.ID = Unit.ID
-- and Habit.ID = 1;


-- select Entry.Quantity,Unit.Symbol,Entry.Date
-- from entry,Habit h,Unit
-- WHERE h.ID = entry.habitId
-- and unit.ID = h.UnitID
-- and h.id = 1;

select h.Name,SUM(Entry.Quantity),Unit.Symbol
from entry,Habit h,Unit
WHERE h.ID = entry.habitId
and unit.ID = h.UnitID
GROUP BY h.ID;

-- select habit.Name as 'Habit',unit.Name as 'Unit'
-- from Habit,unit
-- where habit.ID = 1
-- and unit.id = habit.UnitID;