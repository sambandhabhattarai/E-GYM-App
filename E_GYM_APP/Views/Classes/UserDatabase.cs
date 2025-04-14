using SQLite;

namespace E_GYM_APP.Views.Classes;

public class UserDatabase
{
    private readonly SQLiteAsyncConnection _database;

    public UserDatabase()
    {
        string dbPath = Path.Combine(FileSystem.AppDataDirectory, "UserDatabase.db3");
        _database = new SQLiteAsyncConnection(dbPath);
        _database.CreateTableAsync<User>().Wait();
    }
    public Task<int> SaveUserAsync(User user)
    {
        if (user.Id != 0)
        {
            return _database.UpdateAsync(user);
        }
        else
        {
            return _database.InsertAsync(user);
        }
    }
    public async Task<User> GetUserByIdAsync()
    {
        string userId = Preferences.Default.Get("UserId", string.Empty);
        return await _database.Table<User>().Where(i => i.Uid == userId).FirstOrDefaultAsync();
    }
    public Task<List<User>> GetUserAsync()
    {
        return _database.Table<User>().ToListAsync();
    }
    public Task<int> DeleteUserAsync(User user)
    {
        return _database.DeleteAsync(user);
    }
    public async Task UpdateUserAsync(User user)
    {
        if (user == null)
            throw new ArgumentNullException(nameof(user));

        await _database.UpdateAsync(user);
    }
}
