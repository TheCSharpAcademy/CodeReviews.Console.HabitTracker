
namespace HabitTracker.BrozDa
{
    /// <summary>
    /// Defines basic CRUD operations for repository handling type <typeparamref name="T"/>
    /// </summary>
    /// <typeparam name="T">Entity type which this repository manages</typeparam>
    internal interface IRepository <T>
    {
        /// <summary>
        /// Inserts single entity to the repository
        /// </summary>
        /// <param name="entity">Entity to be inserted</param>
        void Insert(T entity);
        /// <summary>
        /// Updates single existing entity in the repository
        /// </summary>
        /// <param name="entity">Entity with updated values</param>
        void Update(T entity);
        /// <summary>
        /// Removes single entity from repository
        /// </summary>
        /// <param name="entity">Entity which will be deleted from repository</param>
        void Delete(T entity);
        /// <summary>
        /// Retrieves all entities from repository
        /// </summary>
        /// <returns>Collectin containing all entities</returns>
        IEnumerable<T> GetAll();
        /// <summary>
        /// Creates table in the repository
        /// </summary>
        void CreateTable();
        /// <summary>
        /// Inserts collection of entities to the repository
        /// </summary>
        /// <param name="records">Collection of entities to be inserted</param>
        void InsertBulk(IEnumerable<T> records);
        /// <summary>
        /// Retrieves collection containing ID numbers of entities in repository
        /// </summary>
        /// <returns>Collection containing ID numbers of entities</returns>
        IEnumerable<int> GetIds();
    }
}
