namespace HabitLogger.mrgee1978.DomainLayer.Interfaces;

// Created an IValidation interface in order to allow different types
// to validate data in their own way
public interface IValidation
{
    public int GetValidInteger(string message, string info);
    public string GetValidString(string message, string info);
    public string GetValidDateString(string message, string info);
}
