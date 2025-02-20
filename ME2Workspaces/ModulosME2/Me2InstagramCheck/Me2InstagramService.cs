using Dapper;

using Me2Workspaces.ModulosME2.Database;
using Newtonsoft.Json;

namespace Me2Workspaces.ModulosME2.Me2InstagramCheck
{
    public class Me2InstagramService
    {
        public async Task<bool> CreateUser(UserData userData)
        {
            try
            {


                ConnectionDB connectionDB = new();
                using (var connection = await connectionDB.NewConnection())
                {
                    if (connection == null) return false;

                    var query = @"INSERT INTO me2instagramdata 
                    (id, followers_count, follows_count, media_count, profile_picture_url, username, website, list_like_count, list_comments_count, list_captions_count, list_timestamp_count) 
                    VALUES 
                    (@id, @followers_count, @follows_count, @media_count, @profile_picture_url, @username, @Website, @list_like_count, @list_comments_count, @list_captions_count, @list_timestamp_count)";

                    var parameters = new
                    {
                        userData.id,
                        userData.followers_count,
                        userData.follows_count,
                        userData.media_count,
                        userData.profile_picture_url,
                        userData.username,
                        userData.website,
                        list_like_count = JsonConvert.SerializeObject(userData.list_like_count),
                        list_comments_count = JsonConvert.SerializeObject(userData.list_comments_count),
                        list_captions_count = JsonConvert.SerializeObject(userData.list_captions_count),
                        list_timestamp_count = JsonConvert.SerializeObject(userData.list_timestamp_count)
                    };

                    var result = await connection.ExecuteAsync(query, parameters);
                    return result > 0;
                }


            }
            catch (Exception ex)
            {
               
   
                return false;
            }
        }


        public async Task<bool> UpdateUser(UserData userData)
        {
            try
            {


                ConnectionDB connectionDB = new();
                using (var connection = await connectionDB.NewConnection())
                {
                    if (connection == null) return false;

                    var query = @"UPDATE me2instagramdata SET 
                    followers_count = @followers_count, 
                    follows_count = @follows_count, 
                    media_count = @media_count, 
                    profile_picture_url = @profile_picture_url, 
                    username = @username, 
                    website = @website, 
                    list_like_count = @list_like_count, 
                    list_comments_count = @list_comments_count, 
                    list_captions_count = @list_captions_count, 
                    list_timestamp_count = @list_timestamp_count 
                    WHERE id = @id";

                    var parameters = new
                    {
                        userData.id,
                        userData.followers_count,
                        userData.follows_count,
                        userData.media_count,
                        userData.profile_picture_url,
                        userData.username,
                        userData.website,
                        list_like_count = JsonConvert.SerializeObject(userData.list_like_count),
                        list_comments_count = JsonConvert.SerializeObject(userData.list_comments_count),
                        list_captions_count = JsonConvert.SerializeObject(userData.list_captions_count),
                        list_timestamp_count = JsonConvert.SerializeObject(userData.list_timestamp_count)
                    };

                    var result = await connection.ExecuteAsync(query, parameters);
                    return result > 0;

                }
            }
            catch (Exception ex)
            {
               

                return false;
            }
        }


        public async Task<bool> DeleteUser(string id)
        {
            ConnectionDB connectionDB = new();
            using (var connection = await connectionDB.NewConnection())
            {
                if (connection == null) return false;

                var query = "DELETE FROM me2instagramdata WHERE id = @Id";
                var parameters = new { Id = id };

                var result = await connection.ExecuteAsync(query, parameters);
                return result > 0;
            }
        }

        public async Task<UserData> GetUserByUsername(string username)
        {
            ConnectionDB connectionDB = new();
            using (var connection = await connectionDB.NewConnection())
            {
                if (connection == null) return null;

                var query = "SELECT * FROM me2instagramdata WHERE username = @Username";
                var parameters = new { Username = username };

                var userData = await connection.QueryFirstOrDefaultAsync<UserData>(query, parameters);

                if (userData != null)
                {
                    userData.list_like_count = JsonConvert.DeserializeObject<List<int>>(userData.list_like_count.ToString());
                    userData.list_comments_count = JsonConvert.DeserializeObject<List<int>>(userData.list_comments_count.ToString());
                    userData.list_captions_count = JsonConvert.DeserializeObject<List<string>>(userData.list_captions_count.ToString());
                    userData.list_timestamp_count = JsonConvert.DeserializeObject<List<string>>(userData.list_timestamp_count.ToString());
                }

                return userData;
            }
        }

        public async Task<UserData> GetUserById(string id)
        {
            ConnectionDB connectionDB = new();
            using (var connection = await connectionDB.NewConnection())
            {
                if (connection == null) return null;

                var query = "SELECT * FROM me2instagramdata WHERE id = @Id";
                var parameters = new { Id = id };

                var userData = await connection.QueryFirstOrDefaultAsync<UserData>(query, parameters);

                if (userData != null)
                {
                    userData.list_like_count = JsonConvert.DeserializeObject<List<int>>(userData.list_like_count.ToString());
                    userData.list_comments_count = JsonConvert.DeserializeObject<List<int>>(userData.list_comments_count.ToString());
                    userData.list_captions_count = JsonConvert.DeserializeObject<List<string>>(userData.list_captions_count.ToString());
                    userData.list_timestamp_count = JsonConvert.DeserializeObject<List<string>>(userData.list_timestamp_count.ToString());
                }

                return userData;
            }
        }

        public async Task<bool> AtualizaBD(UserData userData)
        {
            if (await UsernameExists(userData.username))
            {
                await UpdateUser(userData);
            }
            else
            {
                await CreateUser(userData);
            }

            return true;
        }

        public async Task<bool> UsernameExists(string username)
        {
            ConnectionDB connectionDB = new();
            using (var connection = await connectionDB.NewConnection())
            {
                if (connection == null) return false;

                var query = "SELECT COUNT(1) FROM me2instagramdata WHERE username = @Username";
                var parameters = new { Username = username };

                var count = await connection.ExecuteScalarAsync<int>(query, parameters);

                return count > 0;
            }
        }

        public async Task<List<UserData>> GetAll()
        {
            try
            {

           
            ConnectionDB connectionDB = new();
            using (var connection = await connectionDB.NewConnection())
            {
                if (connection == null) return null;

                var query = @"SELECT id, followers_count, follows_count, media_count, profile_picture_url, username, website,
                             list_like_count AS list_like_count_json,
                             list_comments_count AS list_comments_count_json,
                             list_captions_count AS list_captions_count_json,
                             list_timestamp_count AS list_timestamp_count_json
                      FROM me2instagramdata";

                var userDataList = await connection.QueryAsync<UserData>(query);

                if (userDataList == null)
                {
                    return new List<UserData>();
                }

                foreach (var userData in userDataList)
                {
                    if (userData != null)
                    {
                        // Deserialize the JSON strings into the list properties
                        userData.list_like_count = JsonConvert.DeserializeObject<List<int>>(userData.list_like_count_json);
                        userData.list_comments_count = JsonConvert.DeserializeObject<List<int>>(userData.list_comments_count_json);
                        userData.list_captions_count = JsonConvert.DeserializeObject<List<string>>(userData.list_captions_count_json);
                        userData.list_timestamp_count = JsonConvert.DeserializeObject<List<string>>(userData.list_timestamp_count_json);
                    }
                }

                return userDataList.ToList();
            }

            }
            catch (Exception ex)
            {
               

                return [];
            }
        }

    }
}
