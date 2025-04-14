using Microsoft.VisualBasic;
using SQLite;

namespace E_GYM_APP.Views.Classes
{
    public class WorkoutDatabase
    {
        SQLiteAsyncConnection Database;
        public WorkoutDatabase()
        {
            Init();
        }

        private void Init()
        {
            if (Database is not null)
                return;
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
            Database.CreateTableAsync<Exercise>().Wait(); 
        }
        public async Task<List<Exercise>> GetExercisesAsync(string userId)
        {
            Init();
            return await Database.Table<Exercise>().Where(e => e.UserId == userId).ToListAsync();
        }
        public async Task<int> SaveExerciseAsync(Exercise exercise)
        {
            Init();
            return await Database.InsertAsync(exercise);
        }
        public async Task<int> DeleteExerciseAsync(Exercise exercise)
        {
            Init();
            return await Database.DeleteAsync(exercise);
        }
        public async Task<Exercise> GetExerciseByNameAndUserIdAsync(string exerciseName, string userId)
        {
            Init();
            return await Database.Table<Exercise>()
                                 .Where(e => e.ExerciseName == exerciseName && e.UserId == userId)
                                 .FirstOrDefaultAsync();
        }
        public async Task<int> UpdateExerciseAsync(Exercise exercise)
        {
            Init();
            return await Database.UpdateAsync(exercise);
        }
    }
}
